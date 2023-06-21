using Azure.Core;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Users.Students;
using Graidex.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graidex.API.Controllers.Users
{
    [Route("api/[controller]")]
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
            var result = await this.studentService.DeleteCurrent(password);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                wrongPassword => Unauthorized("Wrong password."));
        }

        [HttpPost("add-to-subject/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> AddToSubject(int subjectId, string studentEmail)
        {
            var result = await this.studentService.AddCurrentToSubjectAsync(subjectId, studentEmail);

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
            var result = await this.studentService.RemoveCurrentFromSubjectAsync(subjectId, studentEmail);
            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound("Subject not found."));
        }
    }
}
