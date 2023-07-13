using Azure.Core;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Files;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Users.Students;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.Net.Mime;
using System.Text;

namespace Graidex.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentAuthenticationService authenticationService;
        private readonly IStudentService studentService;
        private readonly IContentTypeProvider contentTypeProvider;

        public StudentController(
            IStudentAuthenticationService authenticationService,
            IStudentService studentService,
            IContentTypeProvider contentTypeProvider)
        {
            this.authenticationService = authenticationService;
            this.studentService = studentService;
            this.contentTypeProvider = contentTypeProvider;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreateStudentDto request)
        {
            var result = await this.authenticationService.RegisterStudentAsync(request);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userAlreadyExists => Conflict(userAlreadyExists.Comment));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserAuthDto request)
        {
            var result = await this.authenticationService.LoginStudentAsync(request);

            return result.Match<ActionResult<string>>(
                token => Ok(token),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong password."));
        }

        [HttpGet("me")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentInfoDto>> GetMe()
        {
            var result = await this.studentService.GetCurrentAsync();

            return result.Match<ActionResult<StudentInfoDto>>(
                studentInfo => Ok(studentInfo),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("{studentEmail}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfStudent")]
        public async Task<ActionResult<StudentInfoDto>> GetByEmail(string studentEmail)
        {
            var result = await this.studentService.GetByEmailAsync(studentEmail);

            return result.Match<ActionResult<StudentInfoDto>>(
                studentInfo => Ok(studentInfo),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpPut("update-info")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> UpdateInfo(StudentInfoDto student)
        {
            var result = await this.studentService.UpdateCurrentInfoAsync(student);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpPut("update-profile-image")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> UpdateProfileImage(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var imageDto = new UploadImageDto
            {
                FileName = file.FileName,
                Stream = stream
            };

            var result = await this.studentService.UpdateCurrentProfileImageAsync(imageDto);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("download-profile-image")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> DownloadProfileImage()
        {
            var result = await this.studentService.DownloadCurrentProfileImageAsync();

            return result.Match<ActionResult>(
                file => 
                {
                    this.contentTypeProvider.TryGetContentType(
                        file.FileName, 
                        out var contentType);

                    return File(
                        fileStream: file.Stream,
                        contentType: contentType ?? "image/?",
                        fileDownloadName: file.FileName);
                },
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound("Profile image not found."));
        }

        [HttpDelete("delete-profile-image")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> DeleteProfileImage()
        {
            var result = await this.studentService.DeleteCurrentProfileImageAsync();

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment));
        }

            [HttpPut("change-password")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto passwords)
        {
            var result = await this.studentService.UpdateCurrentPasswordAsync(passwords);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong current password."));
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Delete(string password)
        {
            var result = await this.studentService.DeleteCurrentAsync(password);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong password."));
        }

        [HttpPost("add-to-subject/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> AddToSubject(int subjectId, string studentEmail)
        {
            var result = await this.studentService.AddToSubjectAsync(subjectId, studentEmail);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound("Subject not found."));
        }

        [HttpGet("all-of-subject/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult<List<StudentDto>>> GetAllOfSubject(int subjectId)
        {
            var result = await this.studentService.GetAllOfSubjectAsync(subjectId);

            return result.Match<ActionResult<List<StudentDto>>>(
                students => Ok(students),
                notFound => NotFound("Subject not found."));
        }

        [HttpDelete("remove-from-subject/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> RemoveFromSubject(int subjectId, string studentEmail)
        {
            var result = await this.studentService.RemoveFromSubjectAsync(subjectId, studentEmail);
            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound("Subject not found."));
        }

        [HttpDelete("remove-me-from-subject/{subjectId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfSubject")]
        public async Task<ActionResult> RemoveMeFromSubject(int subjectId)
        {
            var result = await this.studentService.RemoveCurrentFromSubjectAsync(subjectId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound("Subject not found."));
        }
    }
}
