﻿using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Infrastructure.ValidationFailure;
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
            var result = await this.authenticationService.RegisterTeacher(request);

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
            var result = await this.authenticationService.LoginTeacher(request);

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
