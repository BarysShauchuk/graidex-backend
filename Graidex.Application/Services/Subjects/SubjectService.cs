using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Subject;
using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Users;
using Graidex.Application.Validators.Users.Students;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly ICurrentUserService currentUser;
        private readonly ITeacherRepository teacherRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ITestRepository testRepository;
        private readonly IMapper mapper;
        private readonly IValidator<CreateSubjectDto> createSubjectDtoValidator;
        private readonly IValidator<UpdateSubjectDto> updateSubjectDtoValidator;

        public SubjectService(
            ICurrentUserService currentUser,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository,
            ITestRepository testRepository,
            IMapper mapper,
            IValidator<CreateSubjectDto> createSubjectDtoValidator,
            IValidator<UpdateSubjectDto> updateSubjectDtoValidator)
        {
            this.currentUser = currentUser;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.testRepository = testRepository;
            this.mapper = mapper;
            this.createSubjectDtoValidator = createSubjectDtoValidator;
            this.updateSubjectDtoValidator = updateSubjectDtoValidator;
        }

        public async Task<OneOf<SubjectDto, ValidationFailed, UserNotFound>> CreateForCurrentAsync(CreateSubjectDto createSubjectDto)
        {
            var validationResult = await createSubjectDtoValidator.ValidateAsync(createSubjectDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var subject = this.mapper.Map<Subject>(createSubjectDto);
            subject.TeacherId = teacher.Id;
            await this.subjectRepository.Add(subject);

            var subjectDto = this.mapper.Map<SubjectDto>(subject);

            return subjectDto;
        }

        public async Task<OneOf<List<SubjectDto>, UserNotFound>> GetAllOfCurrentAsync()
        {
            string email = this.currentUser.GetEmail();
            IEnumerable<string> roles = this.currentUser.GetRoles();

            if (roles.Contains("Teacher"))
            {
                var teacher = await this.teacherRepository.GetByEmail(email);
                if (teacher is null)
                {
                    return this.currentUser.UserNotFound("Teacher");
                }

                var subjects = this.subjectRepository.GetAll().Where(x => x.TeacherId == teacher.Id);
                var subjectDtos = this.mapper.Map<List<SubjectDto>>(subjects);

                return subjectDtos;
            }

            else if (roles.Contains("Student"))
            {
                var student = await this.studentRepository.GetByEmail(email);
                if (student is null)
                {
                    return this.currentUser.UserNotFound("Student");
                }

                var subjects = this.subjectRepository.GetAll().Where(x => x.Students.Any(y => y.Id == student.Id));
                var subjectDtos = this.mapper.Map<List<SubjectDto>>(subjects);

                return subjectDtos;
            }
            
            return this.currentUser.UserNotFound("User");
        }

        public async Task<OneOf<SubjectInfoDto, UserNotFound, NotFound>> GetSubjectOfTeacherByIdAsync(int id)
        {
            string email = this.currentUser.GetEmail();

            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var subject = await this.subjectRepository.GetById(id);
            if (subject is null)
            {
                return new NotFound();
            }

            var subjectDto = this.mapper.Map<SubjectInfoDto>(subject);
            subjectDto.TeacherEmail = teacher.Email;

            return subjectDto;
        }

        public async Task<OneOf<SubjectInfoDto, UserNotFound, NotFound>> GetSubjectOfStudentByIdAsync(int id)
        {
            string email = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var subject = await this.subjectRepository.GetById(id);
            if (subject is null)
            {
                return new NotFound();
            }

            var subjectDto = this.mapper.Map<SubjectInfoDto>(subject);

            var subjectTeacher = await this.teacherRepository.GetById(subject.TeacherId);

            if (subjectTeacher is not null)
            {
                subjectDto.TeacherEmail = subjectTeacher.Email;
            }

            return subjectDto;
        }


         public async Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> UpdateSubjectInfoAsync(int id, UpdateSubjectDto updateSubjectDto)
         {
            var validationResult = await updateSubjectDtoValidator.ValidateAsync(updateSubjectDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = this.currentUser.GetEmail();

            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var subject = await this.subjectRepository.GetById(id);
            if (subject is null)
            {
                return new NotFound();
            }


            mapper.Map(updateSubjectDto, subject);
            await subjectRepository.Update(subject);

            return new Success();
        }

        public async Task<OneOf<Success, UserNotFound, NotFound>> DeleteByIdAsync(int id)
        {
            string email = this.currentUser.GetEmail();

            var teacher = await this.teacherRepository.GetByEmail(email);

            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var subject = await this.subjectRepository.GetById(id);
            if (subject is null)
            {
                return new NotFound();
            }

            await subjectRepository.Delete(subject);

            return new Success();
        }

        public async Task<OneOf<List<SubjectContentDto>, NotFound>> GetAllContentByIdAsync(int id)
        {
            var subject = await this.subjectRepository.GetById(id);

            if (subject is null)
            {
                return new NotFound();
            }

            var subjectContents = await this.subjectRepository.GetContentById(subject.Id);

            var subjectContentDtos = this.mapper.Map<List<SubjectContentDto>>(subjectContents);

            return subjectContentDtos;
        }

        public async Task<OneOf<List<SubjectContentDto>, UserNotFound, NotFound>> GetVisibleContentOfByIdAsync(int id)
        {
            string email = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var subject = await this.subjectRepository.GetById(id);
            if (subject is null)
            {
                return new NotFound();
            }

            var subjectContents = await this.subjectRepository.GetContentById(subject.Id);

            var visibleContent  = subjectContents.Where(x => x.IsVisible == true).ToList();

            var activeContent = this.testRepository.GetAll().Where(x =>
                x.SubjectId == id
                && !x.IsVisible
                && x.AllowedStudents.Any(x => x.Id == student.Id) 
                && x.StartDateTime < DateTime.Now
                && x.EndDateTime > DateTime.Now);

            visibleContent.AddRange(activeContent.ToList());

            var subjectContentDtos = this.mapper.Map<List<SubjectContentDto>>(visibleContent);

            return subjectContentDtos;
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable>> ChangeContentVisibilityByIdAsync(int contentId, bool isVisible)
        {
            var contentItem = await this.subjectRepository.GetContentItemById(contentId);

            if (contentItem is null)
            {
                return new NotFound();
            }

            if (contentItem.IsVisible == isVisible)
            {
                return new Success();
            }

            string[] contentTypesWithImmutableVisibility = new[] 
            {
                "TestDraft",
            };

            if (contentTypesWithImmutableVisibility.Contains(contentItem.ItemType))
            {
                return new ItemImmutable("TestDraft visibility cannot be changed.");
            }

            await this.subjectRepository.UpdateContentVisibilityById(contentId, isVisible);
            return new Success();
        }
    }
}
