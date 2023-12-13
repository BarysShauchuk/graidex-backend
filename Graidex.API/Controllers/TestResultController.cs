using Amazon.Auth.AccessControlPolicy;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestResult;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Tests;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly ITestResultService testResultService;

        public TestResultController(ITestResultService testResultService)
        {
            this.testResultService = testResultService;
        }

        [HttpPost("start-test-attempt/{testId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfVisibleTest")]
        public async Task<ActionResult> StartTestAttempt(int testId)
        {
            var result = await this.testResultService.StartTestAttemptAsync(testId);

            return result.Match<ActionResult>(
                GetTestAttemptForStudentDto => Ok(GetTestAttemptForStudentDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(),
                outOfAttempts => BadRequest(outOfAttempts.Comment));
        }

        [HttpGet("get-all-questions-with-answers/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfAttempt")]
        public async Task<ActionResult> GetAllQuestionsWithSavedAnswers(int testResultId)
        {
            var result = await this.testResultService.GetAllQuestionsWithSavedAnswersAsync(testResultId);

            return result.Match<ActionResult>(
                testAttemptForStudentDto => Ok(testAttemptForStudentDto),
                notFound => NotFound());
        }

        [HttpPut("update-test-attempt/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfAttempt")]
        public async Task<ActionResult> UpdateTestAttempt(int testResultId, int index, GetAnswerForStudentDto answerDto)
        {
            var result = await this.testResultService.UpdateTestAttemptByIdAsync(testResultId, index, answerDto);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound(),
                itemImmutable => BadRequest(itemImmutable.Comment));
        }

        [HttpPut("submit-test-attempt/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfAttempt")]
        public async Task<ActionResult> SubmitTestAttempt(int testResultId, int index, GetAnswerForStudentDto answerDto)
        {
            var result = await this.testResultService.SubmitTestAttemptByIdAsync(testResultId, index, answerDto);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound());
        }

        [HttpPut("get-test-result/{testResultId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> GetTestResultForTeacherAttempt(int testResultId)
        {
            var result = await this.testResultService.GetTestResultByIdAsync(testResultId);

            return result.Match<ActionResult>(
                testResultDto => Ok(testResultDto),
                notFound => NotFound(),
                itemImmutable => BadRequest(itemImmutable.Comment));
        }

        [HttpPut("leave-feedback-on-answer/{testResultId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> LeaveFeedBack(int testResultId, int index, LeaveFeedbackForAnswerDto feedbackDto)
        {
            var result = await this.testResultService.LeaveFeedBackOnAnswerAsync(testResultId, index, feedbackDto);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound(),
                itemImmutable => BadRequest(itemImmutable.Comment));
        }
    }
}
