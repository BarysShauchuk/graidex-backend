using Azure.Core;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Files;
using Graidex.Application.DTOs.Files.Images;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Users.Teachers;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Graidex.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherAuthenticationService authenticationService;
        private readonly ITeacherService teacherService;
        private readonly IContentTypeProvider contentTypeProvider;

        public TeacherController(
            ITeacherAuthenticationService authenticationService,
            ITeacherService teacherService,
            IContentTypeProvider contentTypeProvider)
        {
            this.authenticationService = authenticationService;
            this.teacherService = teacherService;
            this.contentTypeProvider = contentTypeProvider;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreateTeacherDto request)
        {
            var result = await this.authenticationService.RegisterTeacherAsync(request);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userAlreadyExists => Conflict(userAlreadyExists.Comment));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserAuthDto request)
        {
            var result = await this.authenticationService.LoginTeacherAsync(request);

            return result.Match<ActionResult<string>>(
                token => Ok(token),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong password."));
        }

        [HttpGet("me")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<TeacherInfoDto>> GetMe()
        {
            var result = await this.teacherService.GetCurrentAsync();

            return result.Match<ActionResult<TeacherInfoDto>>(
                teacherInfo => Ok(teacherInfo),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("{teacherEmail}")]
        [Authorize(Roles = "Student", Policy = "StudentOfTeacher")]
        public async Task<ActionResult<TeacherInfoDto>> GetByEmail(string teacherEmail)
        {
            var result = await this.teacherService.GetByEmailAsync(teacherEmail);

            return result.Match<ActionResult<TeacherInfoDto>>(
                teacherInfo => Ok(teacherInfo),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("profile-image/{teacherEmail}")]
        [Authorize(Roles = "Student", Policy = "StudentOfTeacher")]
        public async Task<ActionResult> GetProfileImageByEmail(string teacherEmail)
        {
            var result = await this.teacherService.GetProfileImageByEmailAsync(teacherEmail);

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

        [HttpPut("update-info")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> UpdateInfo(TeacherInfoDto teacher)
        {
            var result = await this.teacherService.UpdateCurrentInfoAsync(teacher);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpPut("update-profile-image")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> UpdateProfileImage(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var imageDto = new UploadImageDto
            {
                FileName = file.FileName,
                Stream = stream
            };

            var result = await this.teacherService.UpdateCurrentProfileImageAsync(imageDto);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("download-profile-image")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> DownloadProfileImage()
        {
            var result = await this.teacherService.DownloadCurrentProfileImageAsync();

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
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> DeleteProfileImage()
        {
            var result = await this.teacherService.DeleteCurrentProfileImageAsync();

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpPut("change-password")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto passwords)
        {
            var result = await this.teacherService.UpdateCurrentPasswordAsync(passwords);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong current password."));
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> Delete(string password)
        {
            var result = await this.teacherService.DeleteCurrent(password);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong password."));
        }
    }
}
