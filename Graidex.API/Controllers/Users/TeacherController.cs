using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Graidex.API.Controllers.Users
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherAuthenticationService authenticationService;

        public TeacherController(
            ITeacherAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(TeacherDto request)
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
                notFound => NotFound(),
                wrongPassword => Unauthorized("Wrong password."));
        }

        [HttpGet("me")]
        public async Task<ActionResult<TeacherInfoDto>> GetMe()
        {
            throw new NotImplementedException();
        }

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpGet("{email}")]
        [Authorize(Roles = "Student")] // TODO: Remove student role
        public async Task<ActionResult<TeacherInfoDto>> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut("update-info")]
        public async Task<ActionResult> UpdateInfo(TeacherInfoDto teacher)
        {
            throw new NotImplementedException();
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto passwords)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(string password)
        {
            throw new NotImplementedException();
        }
    }
}
