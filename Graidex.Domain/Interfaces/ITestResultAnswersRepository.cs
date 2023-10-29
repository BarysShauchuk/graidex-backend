using Graidex.Domain.Models.Tests.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface ITestResultAnswersRepository
    {
        public Task CreateAnswersListAsync(TestResultAnswersList list);
        public Task<TestResultAnswersList> GetAnswersListAsync(int testResultId);
        public Task UpdateAnswerAsync(int testResultId, int index, Answer answer);
        public Task<Answer> GetAnswerAsync(int testResultId, int index);
    }
}
