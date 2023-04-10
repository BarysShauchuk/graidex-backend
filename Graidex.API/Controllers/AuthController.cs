using Graidex.Application.DTOs.Authentication;
using Graidex.Application.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IConfiguration configuration;

        public AuthController(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            this.authenticationService = authenticationService;
            this.configuration = configuration;
        }

        [HttpPost("register-student")]
        public async Task<ActionResult> RegisterStudent(StudentAuthDto request)
        {
            // TODO: Check ModelState

            var result = await this.authenticationService.RegisterStudent(request);

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

        [HttpPost("login-student")]
        public async Task<ActionResult<string>> LoginStudent(UserAuthDto request)
        {
            // TODO: Check ModelState

            var keyToken = this.configuration.GetSection("AppSettings:Token").Value!;
            var result = await this.authenticationService.LoginStudent(request, keyToken);

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

        [HttpPost("register-teacher")]
        public async Task<ActionResult> RegisterTeacher(TeacherAuthDto request)
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

        [HttpPost("login-teacher")]
        public async Task<ActionResult<string>> LoginTeacher(UserAuthDto request)
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
    }
}
