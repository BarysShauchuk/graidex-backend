﻿using Azure.Core;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Users.Student;
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
        private readonly IConfiguration configuration;

        public StudentController(
            IStudentAuthenticationService authenticationService,
            IStudentService studentService,
            IConfiguration configuration)
        {
            this.authenticationService = authenticationService;
            this.studentService = studentService;
            this.configuration = configuration;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(StudentDto request)
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserAuthDto request)
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

        [HttpGet("{email}")]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<StudentInfoDto>> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut("update-info")]
        public async Task<ActionResult> UpdateInfo(StudentInfoDto student)
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
