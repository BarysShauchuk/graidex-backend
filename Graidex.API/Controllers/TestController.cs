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
