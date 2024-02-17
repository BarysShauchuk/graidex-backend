using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Infrastructure.Repositories
{
    public class TestResultAnswersRepository : ITestResultAnswersRepository
    {
        private readonly GraidexMongoDbClient client;

        public TestResultAnswersRepository(GraidexMongoDbClient client)
        {
            this.client = client;
        }

        public async Task CreateAnswersListAsync(TestResultAnswersList list)
        {
            await this.client.TestResultAnswersLists.InsertOneAsync(list);
        }

        public async Task<TestResultAnswersList> GetAnswersListAsync(int testResultId)
        {
            return await this.client.TestResultAnswersLists
                .Find(x => x.TestResultId == testResultId)
                .FirstOrDefaultAsync();
        }

        public async Task<Answer> GetAnswerAsync(int testResultId, int index)
        {
            return await this.client.TestResultAnswersLists
                .Find(x => x.TestResultId == testResultId)
                .Project(x => x.Answers[index])
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAnswerAsync(int testResultId, int index, Answer answer)
        {
            await this.client.TestResultAnswersLists.UpdateOneAsync(
                x => x.TestResultId == testResultId,
                Builders<TestResultAnswersList>.Update.Set(x => x.Answers[index], answer));
        }

        public async Task UpdateAnswersListAsync(TestResultAnswersList testResultAnswersList)
        {
            await this.client.TestResultAnswersLists
                .ReplaceOneAsync(
                    x => x.TestResultId == testResultAnswersList.TestResultId,
                    testResultAnswersList);
        }
    }
}
