using AutoMapper;
using Graidex.Application.DTOs.Subject;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Users;
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
        private readonly IMapper mapper;

        public SubjectService(
            ICurrentUserService currentUser,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper)
        {
            this.currentUser = currentUser;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<OneOf<SubjectDto, ValidationFailed, UserNotFound>> CreateForCurrentAsync(CreateSubjectDto createSubjectDto)
        {
            // TODO: Validate object

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

        public async Task<OneOf<List<SubjectDto>, UserNotFound>> GetAll()
        {
            string email = this.currentUser.GetEmail();
            string role = this.currentUser.GetRole();
            switch (role)
            {
                case "Teacher":
                    var teacher = await this.teacherRepository.GetByEmail(email);
                    if (teacher is null)
                    {
                        return this.currentUser.UserNotFound("Teacher");
                    }

                    var subjects = this.subjectRepository.GetAll().Where(x => x.TeacherId == teacher.Id);
                    var subjectDtos = this.mapper.Map<List<SubjectDto>>(subjects);

                    return subjectDtos;

                case "Student":
                    var student = await this.studentRepository.GetByEmail(email);
                    if (student is null)
                    {
                        return this.currentUser.UserNotFound("Student");
                    }

                    subjects = this.subjectRepository.GetAll().Where(x => x.Students.Any(y => y.Id == student.Id));
                    subjectDtos = this.mapper.Map<List<SubjectDto>>(subjects);

                    return subjectDtos;
            }
            return this.currentUser.UserNotFound("User");
        }

        public async Task<OneOf<SubjectInfoDto, UserNotFound, NotFound>> GetByIdAsync(int id)
        {
            string email = this.currentUser.GetEmail();
            string role = this.currentUser.GetRole();
            switch (role)
            {
                case "Teacher":
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

                    if (subject.TeacherId != teacher.Id)
                    {
                        return new NotFound();
                    }

                    var subjectDto = this.mapper.Map<SubjectInfoDto>(subject);
                    subjectDto.TeacherEmail = teacher.Email;

                    return subjectDto;

                case "Student":
                    var student = await this.studentRepository.GetByEmail(email);
                    if (student is null)
                    {
                        return this.currentUser.UserNotFound("Student");
                    }

                    subject = await this.subjectRepository.GetById(id);
                    if (subject is null)
                    {
                        return new NotFound();
                    }

                    if (!subject.Students.Any(x => x.Id == student.Id))
                    {
                        return new NotFound();
                    }

                    subjectDto = this.mapper.Map<SubjectInfoDto>(subject);

                    var subjectTeacher = await this.teacherRepository.GetById(subject.TeacherId);

                    if (subjectTeacher is not null)
                    {
                        subjectDto.TeacherEmail = subjectTeacher.Email;
                    }

                    return subjectDto;
            }
            return this.currentUser.UserNotFound("User");
        }

        public async Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> UpdateSubjectInfoAsync(int id, UpdateSubjectDto updateSubjectDto)
        {
            // TODO: Add validation
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

            if (subject.TeacherId != teacher.Id)
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

            if (subject.TeacherId != teacher.Id)
            {
                return new NotFound();
            }

            await subjectRepository.Delete(subject);

            return new Success();
        }
    }
}
