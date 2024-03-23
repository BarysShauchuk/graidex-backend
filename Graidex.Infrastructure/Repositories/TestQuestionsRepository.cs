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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Graidex.Infrastructure.Repositories
{
    public class TestQuestionsRepository : ITestBaseQuestionsRepository
    {
        private readonly GraidexMongoDbClient client;

        public TestQuestionsRepository(GraidexMongoDbClient client)
        {
            this.client = client;
        }

        public async Task<Question> GetQuestionAsync(int testBaseId, int questionIndex)
        {
            return await this.client.TestQuestionsLists
                .Find(x => x.TestBaseId == testBaseId)
                .Project(x => x.Questions[questionIndex])
                .FirstOrDefaultAsync();
        }

        public async Task<TestBaseQuestionsList> GetQuestionsListAsync(int testBaseId)
        {
            return await this.client.TestQuestionsLists
                .Find(x => x.TestBaseId == testBaseId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateQuestionsListAsync(TestBaseQuestionsList testBaseQuestionsList)
        {
            await this.client.TestQuestionsLists
                .ReplaceOneAsync(
                    x => x.TestBaseId == testBaseQuestionsList.TestBaseId,
                    testBaseQuestionsList,
                    new ReplaceOptions { IsUpsert = true });
        }
    }
}
