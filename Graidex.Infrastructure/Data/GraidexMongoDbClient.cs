using Graidex.Domain.Models.Tests.Questions;
using Graidex.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graidex.Infrastructure.Data
{
    public class GraidexMongoDbClient : MongoClient
    {
        public GraidexMongoDbClient(
            string? connectionString,
            IOptions<MongoDbConfig> options, 
            ILogger<GraidexMongoDbClient> logger)
            :base(connectionString)
        {
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.Database = this.GetDatabase(options.Value.DatabaseName);

            this.TestQuestionsLists = this.Database
                .GetCollection<TestBaseQuestionsList>(nameof(TestQuestionsLists));

            this.Configure();

            logger.LogInformation(
                $"MongoDbClient initialized for database: {options.Value.DatabaseName}");
        }

        public void Configure()
        {
            BsonClassMap.RegisterClassMap<OpenQuestion>();
            BsonClassMap.RegisterClassMap<SingleChoiceQuestion>();
            BsonClassMap.RegisterClassMap<MultipleChoiceQuestion>();
        }

        public IMongoDatabase Database { get; }

        public IMongoCollection<TestBaseQuestionsList> TestQuestionsLists { get; }
    }
}
