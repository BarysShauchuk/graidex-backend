using Amazon.Auth.AccessControlPolicy;
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
        [Authorize(Roles = "Student", Policy = "StudentOfTest")]
        public async Task<ActionResult> StartTestAttempt(int testId)
        {
            var result = await this.testResultService.StartTestAttemptAsync(testId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound(),
                outOfAttempts => BadRequest(outOfAttempts.Comment));
        }

        [HttpPut("update-test-attempt/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfAttempt")]
        public async Task<ActionResult> UpdateTestAttempt(int testResultId)
        {
            var result = await this.testResultService.UpdateTestAttemptByIdAsync(testResultId);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound(),
                attemptFinished => BadRequest(attemptFinished.Comment));
        }

        [HttpPut("submit-test-attempt/{testResultId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfAttempt")]
        public async Task<ActionResult> SubmitTestAttempt(int testResultId)
        {
            var result = await this.testResultService.SubmitTestAttemptByIdAsync(testResultId);

            return result.Match<ActionResult>(
                success => Ok(),
                notFound => NotFound());
        }
    }
}
