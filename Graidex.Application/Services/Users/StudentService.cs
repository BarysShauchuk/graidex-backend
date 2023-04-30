using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
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

namespace Graidex.Application.Services.Users
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IValidator<StudentInfoDto> studentInfoDtoValidator;
        // private readonly IValidator<ChangePasswordDto> changePasswordDtoValidator;

        public StudentService(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IValidator<StudentInfoDto> studentInfoDtoValidator/*,
            IValidator<ChangePasswordDto> changePasswordDtoValidator*/)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.studentInfoDtoValidator = studentInfoDtoValidator;
            //this.changePasswordDtoValidator = changePasswordDtoValidator;
        }

        public async Task<OneOf<Success, NotFound, WrongPassword>> DeleteCurrent(string password)
        {
            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return new NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(password, student.PasswordHash))
            {
                return new WrongPassword();
            }

            await this.studentRepository.Delete(student);
            return new Success();
        }

        public Task<OneOf<StudentInfoDto>> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<OneOf<StudentInfoDto, NotFound>> GetCurrentAsync()
        {
            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return new NotFound();
            }

            var studentInfo = this.mapper.Map<StudentInfoDto>(student);
            return studentInfo;
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound>> UpdateCurrentInfoAsync(StudentInfoDto studentInfo)
        {
            var validationResult = await this.studentInfoDtoValidator.ValidateAsync(studentInfo);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return new NotFound();
            }

            this.mapper.Map(studentInfo, student);
            await this.studentRepository.Update(student);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords)
        {
            // TODO: Uncomment when the validation is ready
            // var validationResult = await this.changePasswordDtoValidator.ValidateAsync(passwords);
            //if (!validationResult.IsValid)
            //{
            //    return new ValidationFailed(validationResult.Errors);
            //}

            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return new NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(passwords.CurrentPassword, student.PasswordHash))
            {
                return new WrongPassword();
            }

            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwords.NewPassword);
            await this.studentRepository.Update(student);

            return new Success();
        }

        private string GetCurrentUserEmail()
        {
            var user = this.httpContextAccessor.HttpContext.User;

            var identity = user.Identity;
            if (identity is null)
            {
                throw new HttpRequestException();
            }

            var email = identity.Name;
            if (email is null)
            {
                throw new HttpRequestException();
            }

            return email;
        }
    }
}
