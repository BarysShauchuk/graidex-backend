using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Infrastructure.ResultObjects.Generic;
using Graidex.Application.Infrastructure.ResultObjects.NonGeneric;
using Graidex.Application.Infrastructure.ValidationFailure;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Http;
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

        public async Task<Result> DeleteCurrent(string password)
        {
            var result = new ResultFactory();

            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                throw new Exception(); // TODO: Add custom exception or return NotFound
            }

            if (!BCrypt.Net.BCrypt.Verify(password, student.PasswordHash))
            {
                return result.Failure("Wrong password.");
            }

            await this.studentRepository.Delete(student);
            return result.Success();
        }

        public Task<Result<StudentInfoDto>> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<StudentInfoDto>> GetCurrent()
        {
            var result = new ResultFactory<StudentInfoDto>();

            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                throw new Exception(); // TODO: Add custom exception or return NotFound
            }

            var studentInfo = this.mapper.Map<StudentInfoDto>(student);
            return result.Success(studentInfo);
        }

        public async Task<Result> UpdateCurrentInfo(StudentInfoDto studentInfo)
        {
            var result = new ResultFactory();

            var validationResult = await this.studentInfoDtoValidator.ValidateAsync(studentInfo);
            if (!validationResult.IsValid)
            {
                return result.ValidationFailure(validationResult.Errors);
            }

            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                throw new Exception(); // TODO: Add custom exception or return NotFound
            }

            this.mapper.Map(studentInfo, student);
            await this.studentRepository.Update(student);

            return result.Success();
        }

        public async Task<Result> UpdateCurrentPassword(ChangePasswordDto passwords)
        {
            var result = new ResultFactory();

            // TODO: Uncomment when the validation is ready
            //var validationResult = await this.changePasswordDtoValidator.ValidateAsync(passwords);
            //if (!validationResult.IsValid)
            //{
            //    return result.ValidationFailure(validationResult.Errors);
            //}

            string email = GetCurrentUserEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                throw new Exception(); // TODO: Add custom exception or return NotFound
            }

            if (!BCrypt.Net.BCrypt.Verify(passwords.CurrentPassword, student.PasswordHash))
            {
                return result.Failure("Wrong current password.");
            }

            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwords.NewPassword);
            await this.studentRepository.Update(student);

            return result.Success();
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
