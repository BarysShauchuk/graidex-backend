using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.TestResult;
using Graidex.Application.Services.Tests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                getTestAttemptForStudentDto => Ok(getTestAttemptForStudentDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpGet("get-all-questions-with-answers/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfTestResult")]
        public async Task<ActionResult> GetAllQuestionsWithSavedAnswers(int testResultId)
        {
            var result = await this.testResultService.GetAllQuestionsWithSavedAnswersAsync(testResultId);

            return result.Match<ActionResult>(
                testAttemptForStudentDto => Ok(testAttemptForStudentDto),
                notFound => NotFound(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpPut("update-test-attempt/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfTestResult")]
        public async Task<ActionResult> UpdateTestAttempt(int testResultId, int index, GetAnswerForStudentDto answerDto)
        {
            var result = await this.testResultService.UpdateTestAttemptByIdAsync(testResultId, index, answerDto);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound(),
                itemImmutable => BadRequest(itemImmutable.Comment),
                validationFailed => BadRequest(validationFailed.Errors));
        }

        [HttpPut("submit-test-attempt/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfTestResult")]
        public async Task<ActionResult> SubmitTestAttempt(int testResultId)
        {
            var result = await this.testResultService.SubmitTestAttemptByIdAsync(testResultId);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound());
        }

        [HttpPut("add-test-results-to-checking-queue/{testId}")]
        [Authorize (Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> AddTestResultsToCheckingQueue(int testId, IEnumerable<int> testResultIds)
        {
            var result = await this.testResultService.AddTestResultsToCheckingQueueAsync(testId, testResultIds);

            return result.Match<ActionResult>(
                success => Ok(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpGet("get-test-result/{testResultId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTestResult")]
        public async Task<ActionResult> GetTestResultForTeacherAttempt(int testResultId)
        {
            var result = await this.testResultService.GetTestResultByIdAsync(testResultId);

            return result.Match<ActionResult>(
                testResultDto => Ok(testResultDto),
                notFound => NotFound(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpGet("get-all-test-results/{testId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTest")]
        public async Task<ActionResult> GetAllTestResultsByTestId(int testId)
        {
            var result = await this.testResultService.GetAllTestResultsByTestIdAsync(testId);

            return result.Match<ActionResult>(
                testResultListedDtos => Ok(testResultListedDtos),
                notFound => NotFound());
        }
        
        [HttpGet("get-test-result-for-student/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfTestResult")]
        public async Task<ActionResult> GetTestResultForStudentAttempt(int testResultId)
        {
            var result = await this.testResultService.GetTestResultForStudentByIdAsync(testResultId);

            return result.Match<ActionResult>(
                getTestResultForStudentDto => Ok(getTestResultForStudentDto),
                notFound => NotFound(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpPut("leave-feedback-on-answer/{testResultId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfTestResult")]
        public async Task<ActionResult> LeaveFeedBack(int testResultId, List<LeaveFeedbackForAnswerDto> feedbackDtos)
        {
            var result = await this.testResultService.LeaveFeedBackOnAnswerAsync(testResultId, feedbackDtos);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                notFound => NotFound(),
                conditionFailed => BadRequest(conditionFailed.Comment));
        }

        [HttpGet("get-student-attempts-description/{testId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfVisibleTest")]
        public async Task<ActionResult> GetStudentAttemptsDescription(int testId)
        {
            var result = await this.testResultService.GetStudentAttemptsDescription(testId);

            return result.Match<ActionResult>(
                getStudentAttemptsDescriptionDto => Ok(getStudentAttemptsDescriptionDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }
    }
}
