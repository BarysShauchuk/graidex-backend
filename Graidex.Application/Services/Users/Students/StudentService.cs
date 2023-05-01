using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Http;
using OneOf.Types;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Users.Students;

namespace Graidex.Application.Services.Users.Students
{
    public class StudentService : IStudentService
    {
        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;
        private readonly IValidator<StudentInfoDto> studentInfoDtoValidator;
        private readonly IValidator<ChangePasswordDto> changePasswordDtoValidator;

        public StudentService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            IValidator<StudentInfoDto> studentInfoDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
            this.studentInfoDtoValidator = studentInfoDtoValidator;
            this.changePasswordDtoValidator = changePasswordDtoValidator;
        }

        public async Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrent(string password)
        {
            string email = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, student.PasswordHash))
            {
                return new WrongPassword();
            }

            await studentRepository.Delete(student);
            return new Success();
        }

        public Task<OneOf<StudentInfoDto>> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<OneOf<StudentInfoDto, UserNotFound>> GetCurrentAsync()
        {
            string email = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var studentInfo = mapper.Map<StudentInfoDto>(student);
            return studentInfo;
        }

        public async Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentInfoAsync(StudentInfoDto studentInfo)
        {
            var validationResult = await studentInfoDtoValidator.ValidateAsync(studentInfo);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            mapper.Map(studentInfo, student);
            await studentRepository.Update(student);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, UserNotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords)
        {
            var validationResult = await this.changePasswordDtoValidator.ValidateAsync(passwords);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            if (!BCrypt.Net.BCrypt.Verify(passwords.CurrentPassword, student.PasswordHash))
            {
                return new WrongPassword();
            }

            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwords.NewPassword);
            await studentRepository.Update(student);

            return new Success();
        }
    }
}
