using Graidex.Application.DTOs.Subject;
using Graidex.Application.DTOs.SubjectRequest;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Subjects;
using Graidex.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using System.Data;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectRequestController : ControllerBase
    {
        private readonly ISubjectRequestService subjectRequestService;

        public SubjectRequestController(ISubjectRequestService subjectRequestService)
        {
            this.subjectRequestService = subjectRequestService;
        }

        [HttpPost("create/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> Create(int subjectId, OutgoingSubjectRequestDto request)
        {
            var result = await this.subjectRequestService.CreateRequestAsync(subjectId, request);
            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(notFound));
        }

        [HttpGet("all")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> GetAll()
        {
            var result = await this.subjectRequestService.GetAllOfCurrentAsync();

            return result.Match<ActionResult>(
                subjectRequestDtos => Ok(subjectRequestDtos),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("of-teacher/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> GetAllBySubjectId(int subjectId)
        {
            var result = await this.subjectRequestService.GetAllBySubjectIdAsync(subjectId);

            return result.Match<ActionResult>(
                subjectRequestDtos => Ok(subjectRequestDtos),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(notFound));
        }

        [HttpPost("join")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> JoinSubject(int requestId)
        {
            var result = await this.subjectRequestService.JoinSubjectByRequestIdAsync(requestId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(notFound));
        }

        [HttpPost("reject")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> RejectRequest(int requestId)
        {
            var result = await this.subjectRequestService.RejectRequestByIdAsync(requestId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(notFound));
        }

        [HttpDelete("delete/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> Delete(int subjectRequestId)
        {
            var result = await this.subjectRequestService.DeleteByIdAsync(subjectRequestId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(notFound));
        }
    }
}
