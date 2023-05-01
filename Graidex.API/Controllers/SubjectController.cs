using Graidex.Application.DTOs.Subject;
using Graidex.Application.Services.Subjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<SubjectDto>> Create(CreateSubjectDto request)
        {
            var result = await this.subjectService.CreateForCurrentAsync(request);

            return result.Match<ActionResult<SubjectDto>>(
                subjectDto => Ok(subjectDto),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpGet("all")]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpGet("{id}")]
        [Authorize(Roles = "Student, Teacher", Policy = "")]
        public async Task<ActionResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpPut("update")]
        [Authorize(Roles = "Teacher", Policy = "")]
        public async Task<ActionResult> Update(CreateSubjectDto request)
        {
            throw new NotImplementedException();
        }

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpPost("add")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> AddStudents(List<string> studentEmails)
        {
            throw new NotImplementedException();
        }

        [ApiExplorerSettings(IgnoreApi = true)] // TODO: Implement method and remove this attribute
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher", Policy = "")]
        public async Task<ActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
