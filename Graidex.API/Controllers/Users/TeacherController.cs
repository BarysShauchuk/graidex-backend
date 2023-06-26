using Azure.Core;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Users.Teachers;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graidex.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherAuthenticationService authenticationService;
        private readonly ITeacherService teacherService;

        public TeacherController(
            ITeacherAuthenticationService authenticationService,
            ITeacherService teacherService)
        {
            this.authenticationService = authenticationService;
            this.teacherService = teacherService;
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
