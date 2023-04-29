using Azure.Core;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Infrastructure.ValidationFailure;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Users;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graidex.API.Controllers.Users
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentAuthenticationService authenticationService;
        private readonly IStudentService studentService;

        public StudentController(
            IStudentAuthenticationService authenticationService,
            IStudentService studentService)
        {
            this.authenticationService = authenticationService;
            this.studentService = studentService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(StudentDto request)
        {
            var result = await this.authenticationService.RegisterStudent(request);

            if (result.IsValidationFailure(out var validationFailure))
            {
                return BadRequest(validationFailure.Errors);
            }

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
        public async Task<ActionResult<string>> Login(UserAuthDto request)
        {
            var result = await this.authenticationService.LoginStudent(request);

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

        [HttpGet("me")]
        public async Task<ActionResult<StudentInfoDto>> GetMe()
        {
            var result = await this.studentService.GetCurrent();

            if (result.IsValidationFailure(out var validationFailure))
            {
                return BadRequest(validationFailure.Errors);
            }

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

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpGet("{email}")]
        [Authorize(Roles = "Teacher")] // TODO: Remove student role
        public async Task<ActionResult<StudentInfoDto>> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut("update-info")]
        public async Task<ActionResult> UpdateInfo(StudentInfoDto student)
        {
            var result = await this.studentService.UpdateCurrentInfo(student);

            if (result.IsValidationFailure(out var validationFailure))
            {
                return BadRequest(validationFailure.Errors);
            }

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

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto passwords)
        {
            var result = await this.studentService.UpdateCurrentPassword(passwords);

            if (result.IsValidationFailure(out var validationFailure))
            {
                return BadRequest(validationFailure.Errors);
            }

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

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(string password)
        {
            var result = await this.studentService.DeleteCurrent(password);

            if (result.IsValidationFailure(out var validationFailure))
            {
                return BadRequest(validationFailure.Errors);
            }

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
    }
}
