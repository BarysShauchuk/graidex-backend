using Graidex.Application.DTOs.Authentication;
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
        private readonly IConfiguration configuration;

        public TeacherController(
            ITeacherAuthenticationService authenticationService,
            IConfiguration configuration)
        {
            this.authenticationService = authenticationService;
            this.configuration = configuration;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(TeacherAuthDto request)
        {
            // TODO: Check ModelState

            var result = await this.authenticationService.RegisterTeacher(request);

            if (result.IsFailure(out var failure))
            {
                return BadRequest(failure.Justification);
            }

            if (result.IsSuccess())
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserAuthDto request)
        {
            // TODO: Check ModelState

            var keyToken = this.configuration.GetSection("AppSettings:Token").Value!;
            var result = await this.authenticationService.LoginTeacher(request, keyToken);

            if (result.IsFailure(out var failure))
            {
                return BadRequest(failure.Justification);
            }

            if (result.IsSuccess(out var success))
            {
                return Ok(success.Value);
            }

            return StatusCode(500);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut("update-info")]
        public async Task<ActionResult> UpdateInfo()
        {
            throw new NotImplementedException();
        }

        [HttpPut("update-password")]
        public async Task<ActionResult> UpdatePassword()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
