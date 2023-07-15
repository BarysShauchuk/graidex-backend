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
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.Interfaces;
using Graidex.Application.DTOs.Files;

namespace Graidex.Application.Services.Users.Students
{
    public class StudentService : IStudentService
    {
        private const string ProfileImagePath = "ProfileImages/Students";

        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;
        private readonly IValidator<StudentInfoDto> studentInfoDtoValidator;
        private readonly IValidator<ChangePasswordDto> changePasswordDtoValidator;
        private readonly IValidator<UploadImageDto> uploadImageDtoValidator;
        private readonly IFileStorageProvider fileStorage;

        public StudentService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            IValidator<StudentInfoDto> studentInfoDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator,
            IValidator<UploadImageDto> uploadImageDtoValidator,
            IFileStorageProvider fileStorage)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
            this.studentInfoDtoValidator = studentInfoDtoValidator;
            this.changePasswordDtoValidator = changePasswordDtoValidator;
            this.uploadImageDtoValidator = uploadImageDtoValidator;
            this.fileStorage = fileStorage;
        }

        public async Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrentAsync(string password)
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

        public async Task<OneOf<Success, UserNotFound>> DeleteCurrentProfileImageAsync()
        {
            string email = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            if (string.IsNullOrEmpty(student.ProfileImage))
            {
                return new Success();
            }

            await this.fileStorage.DeleteAsync(student.ProfileImage, ProfileImagePath);
            student.ProfileImage = null;
            await this.studentRepository.Update(student);

            return new Success();
        }

        public async Task<OneOf<DownloadImageDto, UserNotFound, NotFound>> DownloadCurrentProfileImageAsync()
        {
            string email = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            if (string.IsNullOrEmpty(student.ProfileImage))
            {
                return new NotFound();
            }

            var fileName = $"ProfileImage{Path.GetExtension(student.ProfileImage)}";

            var stream = 
                await this.fileStorage.DownloadAsync(student.ProfileImage, ProfileImagePath);

            var downloadImageDto = new DownloadImageDto
            {
                FileName = fileName,
                Stream = stream,
            };

            return downloadImageDto;
        }

        public async Task<OneOf<List<StudentDto>, NotFound>> GetAllOfSubjectAsync(int subjectId)
        {
            var subject = await subjectRepository.GetById(subjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            var students = subject.Students;
            var studentDtos = this.mapper.Map<List<StudentDto>>(students);
            return studentDtos;
        }

        public async Task<OneOf<StudentInfoDto, UserNotFound>> GetByEmailAsync(string email)
        {
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return new UserNotFound($"Student with email \"{email}\" is not found.");
            }

            var studentInfo = mapper.Map<StudentInfoDto>(student);
            return studentInfo;
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

        public async Task<OneOf<Success, UserNotFound, NotFound>> RemoveFromSubjectAsync(int subjectId, string studentEmail)
        {
            var student = await studentRepository.GetByEmail(studentEmail);
            if (student is null)
            {
                return new UserNotFound($"Student with email \"{studentEmail}\" is not found.");
            }

            var subject = await subjectRepository.GetById(subjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            if (!subject.Students.Any(s => s.Id == student.Id))
            {
                return new UserNotFound(
                    $"Student with email \"{studentEmail}\" is not on the subject with id \"{subjectId}\"");
            }

            subject.Students.Remove(student);
            await this.subjectRepository.Update(subject);
            return new Success();
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

        public async Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentProfileImageAsync(UploadImageDto imageDto)
        {
            var validationResult = await this.uploadImageDtoValidator.ValidateAsync(imageDto);
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
            
            string fileExtension = Path.GetExtension(imageDto.FileName);
            string fileName = $"{Guid.NewGuid()}{fileExtension}";

            if (!string.IsNullOrEmpty(student.ProfileImage))
            {
                await this.fileStorage.DeleteAsync(student.ProfileImage, ProfileImagePath);
            }
            
            await this.fileStorage.UploadAsync(imageDto.Stream, fileName, ProfileImagePath);
            student.ProfileImage = fileName;
            await this.studentRepository.Update(student);

            return new Success();
        }

    }
}
