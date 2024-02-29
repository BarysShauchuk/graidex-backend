using Graidex.Application.Services.TestChecking;
using Graidex.Application.Services.TestChecking.TestCheckingQueue;
using Graidex.Application.Services.Tests.TestChecking;
using Graidex.Domain.Exceptions;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTestingController : ControllerBase
    {
        private readonly GraidexDbContext dbContext;
        private readonly GraidexMongoDbClient mongoDbClient;
        private readonly ISubjectRepository subjectRepository;
        private readonly IConfiguration configuration;
        private readonly IAnswerCheckHandler answerCheckersContainer;
        private readonly ITestCheckingInQueue testCheckingQueue;

        public ApplicationTestingController(
            GraidexDbContext dbContext,
            GraidexMongoDbClient mongoDbClient,
            ISubjectRepository subjectRepository,
            IConfiguration configuration,
            IAnswerCheckHandler answerCheckHandler,
            ITestCheckingInQueue testCheckingQueue)
        {
            this.dbContext = dbContext;
            this.mongoDbClient = mongoDbClient;
            this.subjectRepository = subjectRepository;
            this.configuration = configuration;
            this.answerCheckersContainer = answerCheckHandler;
            this.testCheckingQueue = testCheckingQueue;
        }

        [HttpDelete("drop-data")]
        public IActionResult DropData()
        {
            this.dbContext.Database.EnsureDeleted();
            this.mongoDbClient.DropDatabase(
                configuration
                .GetRequiredSection("AppSettings")
                .GetRequiredSection("MongoDb")
                .GetValue<string>("DatabaseName"));

            return Ok();
        }

        [HttpPut("insert-subject-content-order-indexes")]
        public async Task<IActionResult> InsertSubjectContentOrderIndexes()
        {
            var subjects = this.dbContext.Subjects.Select(x => x.Id).AsAsyncEnumerable();
            await foreach (var id in subjects)
            {
                await this.subjectRepository.RefreshSubjectContentOrderingById(id);
            }

            return Ok();
        }

        [HttpPost("test-1")]
        public ActionResult Test()
        {
            var efDateTime =
                this.dbContext.Tests.FirstOrDefault(x => x.Id == 31)!
                .StartDateTime;
            var efDateTime2 =
                efDateTime
                .ToUniversalTime();

            var dateTimeOff = DateTimeOffset.Now;
            DateTime dateTime = dateTimeOff.DateTime;

            return Ok(new { efdt = efDateTime, efdt2 = efDateTime2 });
        }

        [HttpPost("test-2")]
        public async Task<ActionResult> Test2()
        {
            Question question = new OpenQuestion 
            {
                Text = "Test question",
            };

            Answer answer = new OpenAnswer
            {
                Text = "Test answer",
            };

            await this.answerCheckersContainer.EvaluateAsync(question, answer);

            return Ok();
        }

        [HttpPost("test-3")]
        public async Task<ActionResult> Test3()
        {
            Question question = new SingleChoiceQuestion
            {
                Text = "Test question",
            };

            Answer answer = new SingleChoiceAnswer
            {
                
            };

            await this.answerCheckersContainer.EvaluateAsync(question, answer);

            return Ok();
        }
    }
}
