using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using OneOf.Types;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Interfaces;

namespace Graidex.Application.Services.Users.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;
        private readonly IValidator<TeacherInfoDto> teacherInfoDtoValidator;
        private readonly IValidator<ChangePasswordDto> changePasswordDtoValidator;

        public TeacherService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            IValidator<TeacherInfoDto> teacherInfoDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
            this.teacherInfoDtoValidator = teacherInfoDtoValidator;
            this.changePasswordDtoValidator = changePasswordDtoValidator;
        }

        public async Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrent(string password)
        {
            string email = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, teacher.PasswordHash))
            {
                return new WrongPassword();
            }

            await teacherRepository.Delete(teacher);
            return new Success();
        }

        public Task<OneOf<TeacherInfoDto>> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<OneOf<TeacherInfoDto, UserNotFound>> GetCurrentAsync()
        {
            string email = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var teacherInfo = mapper.Map<TeacherInfoDto>(teacher);
            return teacherInfo;
        }

        public async Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentInfoAsync(TeacherInfoDto teacherInfo)
        {
            var validationResult = await teacherInfoDtoValidator.ValidateAsync(teacherInfo);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            mapper.Map(teacherInfo, teacher);
            await teacherRepository.Update(teacher);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, UserNotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords)
        {
            var validationResult = await changePasswordDtoValidator.ValidateAsync(passwords);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            string email = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            if (!BCrypt.Net.BCrypt.Verify(passwords.CurrentPassword, teacher.PasswordHash))
            {
                return new WrongPassword();
            }

            teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwords.NewPassword);
            await teacherRepository.Update(teacher);

            return new Success();
        }
    }
}
