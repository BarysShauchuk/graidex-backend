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
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Files;
using Graidex.Application.DTOs.Files.Images;

namespace Graidex.Application.Services.Users.Teachers
{
    public class TeacherService : ITeacherService
    {
        private const string ProfileImagePath = "ProfileImages/Teachers";

        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;
        private readonly IValidator<TeacherInfoDto> teacherInfoDtoValidator;
        private readonly IValidator<ChangePasswordDto> changePasswordDtoValidator;
        private readonly IValidator<UploadImageDto> uploadImageDtoValidator;
        private readonly IFileStorageProvider fileStorage;

        public TeacherService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            IValidator<TeacherInfoDto> teacherInfoDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator,
            IValidator<UploadImageDto> uploadImageDtoValidator,
            IFileStorageProvider fileStorage)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
            this.teacherInfoDtoValidator = teacherInfoDtoValidator;
            this.changePasswordDtoValidator = changePasswordDtoValidator;
            this.uploadImageDtoValidator = uploadImageDtoValidator;
            this.fileStorage = fileStorage;
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

        public async Task<OneOf<Success, UserNotFound>> DeleteCurrentProfileImageAsync()
        {
            string email = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            if (string.IsNullOrEmpty(teacher.ProfileImage))
            {
                return new Success();
            }

            await this.fileStorage.DeleteAsync(teacher.ProfileImage, ProfileImagePath);
            teacher.ProfileImage = null;
            await this.teacherRepository.Update(teacher);

            return new Success();
        }

        public async Task<OneOf<DownloadFileDto, UserNotFound, NotFound>> DownloadCurrentProfileImageAsync()
        {
            string email = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            if (string.IsNullOrEmpty(teacher.ProfileImage))
            {
                return new NotFound();
            }

            var fileName = $"ProfileImage{Path.GetExtension(teacher.ProfileImage)}";

            var stream =
                await this.fileStorage.DownloadAsync(teacher.ProfileImage, ProfileImagePath);

            var downloadImageDto = new DownloadFileDto
            {
                FileName = fileName,
                Stream = stream,
            };

            return downloadImageDto;
        }

        public async Task<OneOf<TeacherInfoDto, UserNotFound>> GetByEmailAsync(string email)
        {
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return new UserNotFound($"Teacher with email \"{email}\" is not found.");
            }

            var teacherInfo = mapper.Map<TeacherInfoDto>(teacher);
            return teacherInfo;
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

        public async Task<OneOf<DownloadFileDto, UserNotFound, NotFound>> GetProfileImageByEmailAsync(string email)
        {
            var teacher = await teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return new UserNotFound($"Teacher with email \"{email}\" is not found.");
            }

            if (string.IsNullOrEmpty(teacher.ProfileImage))
            {
                return new NotFound();
            }

            var fileName = $"{teacher.Email}{Path.GetExtension(teacher.ProfileImage)}";

            var stream =
                await this.fileStorage.DownloadAsync(teacher.ProfileImage, ProfileImagePath);

            var downloadImageDto = new DownloadFileDto
            {
                FileName = fileName,
                Stream = stream,
            };

            return downloadImageDto;
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

        public async Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentProfileImageAsync(UploadImageDto imageDto)
        {
            var validationResult = await this.uploadImageDtoValidator.ValidateAsync(imageDto);
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

            string fileExtension = Path.GetExtension(imageDto.FileName);
            string fileName = $"{Guid.NewGuid()}{fileExtension}";

            if (!string.IsNullOrEmpty(teacher.ProfileImage))
            {
                await this.fileStorage.DeleteAsync(teacher.ProfileImage, ProfileImagePath);
            }

            await this.fileStorage.UploadAsync(imageDto.Stream, fileName, ProfileImagePath);
            teacher.ProfileImage = fileName;
            await this.teacherRepository.Update(teacher);

            return new Success();
        }
    }
}
