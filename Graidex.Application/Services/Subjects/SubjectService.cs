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
            subjectDto.TeacherEmail = email;

            return subjectDto;
        }
    }
}
