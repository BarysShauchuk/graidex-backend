using Graidex.Application.DTOs.Subject;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Application.Services.Tests;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests;
using Graidex.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        [HttpPost("create-draft/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult<GetTestDraftDto>> CreateDraft(int subjectId, CreateTestDraftDto request)
        {
            var result = await this.testService.CreateTestDraftForSubjectAsync(subjectId, request);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                validationFailed => BadRequest(validationFailed.Errors));
        }

        [HttpPost("create-draft-from-test/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult<GetTestDraftDto>> CreateDraftFromTest(int testId, CreateTestDraftFromTestDto createTestDraftFromTestDto)
        {
            var result = await this.testService.CreateTestDraftFromTestAsync(testId, createTestDraftFromTestDto);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound());
        }

        [HttpPost("duplicate-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<GetTestDraftDto>> DuplicateDraft(int draftId, DuplicateTestDraftDto duplicateTestDraftDto)
        {
            var result = await this.testService.DuplicateTestDraftAsync(draftId, duplicateTestDraftDto);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound());
        }

        [HttpGet("get-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<GetTestDraftDto>> GetDraftById(int draftId)
        {
            var result = await this.testService.GetTestDraftByIdAsync(draftId);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                notFound => NotFound());
        }

        [HttpPut("update-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult> UpdateDraft(int draftId, UpdateTestDraftDto updateTestDraftDto)
        {
            var result = await this.testService.UpdateTestDraftByIdASync(draftId, updateTestDraftDto);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound());
        }

        [HttpDelete("delete-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult> DeleteDraft(int draftId)
        {
            var result = await this.testService.DeleteTestDraftByIdAsync(draftId);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound());
        }

        [HttpPost("create-test/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<GetTestDto>> CreateTest(int draftId, CreateTestDto createTestDto)
        {
            var result = await this.testService.CreateTestFromTestDraftAsync(draftId, createTestDto);

            return result.Match<ActionResult<GetTestDto>>(
                getTestDto => Ok(getTestDto),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpGet("get-test/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult<GetTestDto>> GetTestById(int testId)
        {
            var result = await this.testService.GetTestByIdAsync(testId);

            return result.Match<ActionResult<GetTestDto>>(
                getTestDto => Ok(getTestDto),
                notFound => NotFound());
        }

        [HttpGet("get-visible-test/{testId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfVisibleTest")]
        public async Task<ActionResult<GetVisibleTestDto>> GetVisibleTestById(int testId)
        {
            var result = await this.testService.GetVisibleTestByIdAsync(testId);

            return result.Match<ActionResult<GetVisibleTestDto>>(
                getTestDto => Ok(getTestDto),
                notFound => NotFound());
        }

        [HttpPut("update-test/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> UpdateTest(int testId, UpdateTestDto updateTestDto)
        {
            var result = await this.testService.UpdateTestByIdAsync(testId, updateTestDto);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound(),
                testImmutable => BadRequest(testImmutable.Comment));
        }

        [HttpDelete("delete-test/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> DeleteTest(int testId)
        {
            var result = await this.testService.DeleteTestByIdAsync(testId);
             
            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound(),
                testImmutable => BadRequest(testImmutable.Comment));
        }

        [HttpPut("update-test-questions/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> UpdateTestQuestions(int testId, List<TestBaseQuestionDto> questions)
        {
            var result = await this.testService.UpdateTestQuestionsAsync(testId, questions);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                testImmutable => BadRequest(testImmutable.Comment),
                notFound => NotFound());
        }

        [HttpGet("test-questions-of-teacher/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult<List<TestBaseQuestionDto>>> GetTestQuestions(int testId)
        {
            var result = await this.testService.GetTestQuestionsAsync(testId);

            return result.Match<ActionResult>(
                questions => Ok(questions),
                notFound => NotFound());
        }

        [HttpGet("test-draft-questions/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<List<GetTestDraftDto>>> GetTestDraftQuestions(int draftId)
        {
            var drafts = await this.testService.GetTestDraftQuestionsAsync(draftId);

            return drafts.Match<ActionResult>(
                questions => Ok(questions),
                notFound => NotFound());
        }

        [HttpPut("update-test-draft-questions/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult> UpdateTestDraftQuestions(int draftId, List<TestBaseQuestionDto> questions)
        {
            var result = await this.testService.UpdateTestDraftQuestionsAsync(draftId, questions);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound());
        }

        [HttpPut("add-students/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> AddStudentsToTest(int testId, List<String> studentEmails)
        {
            var result = await this.testService.AddStudentsToTestAsync(testId, studentEmails);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound());
        }

        [HttpDelete("remove-students/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> RemoveStudentsFromTest(int testId, List<String> studentEmails)
        {
            var result = await this.testService.RemoveStudentsFromTestAsync(testId, studentEmails);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound(),
                testImmutable => BadRequest(testImmutable.Comment));
        }
    }
}
