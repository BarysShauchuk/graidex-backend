using Graidex.Application.Services.TestChecking;
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

        public ApplicationTestingController(
            GraidexDbContext dbContext,
            GraidexMongoDbClient mongoDbClient,
            ISubjectRepository subjectRepository,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.mongoDbClient = mongoDbClient;
            this.subjectRepository = subjectRepository;
            this.configuration = configuration;
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
    }
}
