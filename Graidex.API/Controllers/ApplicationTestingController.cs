using Graidex.Domain.Interfaces;
using Graidex.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTestingController : ControllerBase
    {
        private readonly GraidexDbContext dbContext;
        private readonly GraidexMongoDbClient mongoDbClient;
        private readonly IConfiguration configuration;

        public ApplicationTestingController(
            GraidexDbContext dbContext,
            GraidexMongoDbClient mongoDbClient,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.mongoDbClient = mongoDbClient;
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
    }
}
