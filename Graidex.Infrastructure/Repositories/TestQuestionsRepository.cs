using Graidex.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Graidex.Infrastructure.Configuration;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Infrastructure.Data;
using MongoDB.Bson.Serialization;

namespace Graidex.Infrastructure.Repositories
{
    public class TestQuestionsRepository : ITestQuestionsRepository
    {
        private readonly GraidexMongoDbClient client;

        public TestQuestionsRepository(GraidexMongoDbClient client)
        {
            this.client = client;
        }

        public async Task<TestQuestionsList> GetQuestionsListAsync(int testId)
        {
            return await this.client.TestQuestionsLists
                .Find(x => x.TestId == testId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateQuestionsListAsync(TestQuestionsList testQuestionsList)
        {
            await this.client.TestQuestionsLists
                .ReplaceOneAsync(
                    x => x.TestId == testQuestionsList.TestId,
                    testQuestionsList,
                    new ReplaceOptions { IsUpsert = true });
        }
    }
}
