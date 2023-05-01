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
    public class TeacherService : ITeacherService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IValidator<TeacherInfoDto> teacherInfoDtoValidator;
        private readonly IValidator<ChangePasswordDto> changePasswordDtoValidator;

        public TeacherService(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IValidator<TeacherInfoDto> teacherInfoDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.teacherInfoDtoValidator = teacherInfoDtoValidator;
            this.changePasswordDtoValidator = changePasswordDtoValidator;
        }

        public async Task<OneOf<Success, NotFound, WrongPassword>> DeleteCurrent(string password)
        {
            string email = GetCurrentUserEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return new NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(password, teacher.PasswordHash))
            {
                return new WrongPassword();
            }

            await this.teacherRepository.Delete(teacher);
            return new Success();
        }

        public Task<OneOf<TeacherInfoDto>> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<OneOf<TeacherInfoDto, NotFound>> GetCurrentAsync()
        {
            string email = GetCurrentUserEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return new NotFound();
            }

            var teacherInfo = this.mapper.Map<TeacherInfoDto>(teacher);
            return teacherInfo;
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound>> UpdateCurrentInfoAsync(TeacherInfoDto teacherInfo)
        {
            var validationResult = await this.teacherInfoDtoValidator.ValidateAsync(teacherInfo);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = GetCurrentUserEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return new NotFound();
            }

            this.mapper.Map(teacherInfo, teacher);
            await this.teacherRepository.Update(teacher);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords)
        {
            var validationResult = await this.changePasswordDtoValidator.ValidateAsync(passwords);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = GetCurrentUserEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return new NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(passwords.CurrentPassword, teacher.PasswordHash))
            {
                return new WrongPassword();
            }

            teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwords.NewPassword);
            await this.teacherRepository.Update(teacher);

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
