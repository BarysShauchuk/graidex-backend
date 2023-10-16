using Graidex.Domain.Interfaces;
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
    }
}
