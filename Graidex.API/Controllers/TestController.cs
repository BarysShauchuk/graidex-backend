using Graidex.Application.DTOs.Subject;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Application.Services.Tests;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.Services.Tests;
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
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpPost("create-draft-from-test/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult<GetTestDraftDto>> CreateDraftFromTest(int testId)
        {
            var result = await this.testService.CreateTestDraftFromTestAsync(testId);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpPost("duplicate-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<GetTestDraftDto>> DuplicateDraft(int draftId)
        {
            var result = await this.testService.DuplicateTestDraftAsync(draftId);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpGet("get-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<GetTestDraftDto>> GetDraftById(int draftId)
        {
            var result = await this.testService.GetTestDraftByIdAsync(draftId);

            return result.Match<ActionResult<GetTestDraftDto>>(
                getTestDraftDto => Ok(getTestDraftDto),
                userNotFound => NotFound(userNotFound.Comment),
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
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpDelete("delete-draft/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult> DeleteDraft(int draftId)
        {
            var result = await this.testService.DeleteTestDraftByIdAsync(draftId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpPost("create-test/{draftId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfDraft")]
        public async Task<ActionResult<GetTestDto>> CreateTest(int draftId, CreateTestDto createTestDto)
        {
            var result = await this.testService.CreateTestForDraftAsync(draftId, createTestDto);

            return result.Match<ActionResult<GetTestDto>>(
                getTestDto => Ok(getTestDto),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpGet("get-test/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult<GetTestDto>> GetTestById(int testId)
        {
            var result = await this.testService.GetTestByIdAsync(testId);

            return result.Match<ActionResult<GetTestDto>>(
                getTestDto => Ok(getTestDto),
                userNotFound => NotFound(userNotFound.Comment),
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
                userNotFound => NotFound(userNotFound.Comment),
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
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(),
                testImmutable => BadRequest(testImmutable.Comment));
        }

        [HttpPut("update-questions/{testId}")]
        [Authorize(Roles = "Teacher")] // TODO: Add TestOfTeacher policy
        public async Task<ActionResult> UpdateQuestions(int testId, List<TestQuestionDto> questions)
        {
            var result = await this.testService.UpdateTestQuestionsAsync(testId, questions);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound());
        }

        [HttpGet("questions-of-teacher/{testId}")]
        [Authorize(Roles = "Teacher")] // TODO: Add TestOfTeacher policy
        public async Task<ActionResult<List<TestQuestionDto>>> GetQuestions(int testId)
        {
            var result = await this.testService.GetTestQuestionsAsync(testId);

            return result.Match<ActionResult>(
                questions => Ok(questions),
                notFound => NotFound());
        }
    }
}
