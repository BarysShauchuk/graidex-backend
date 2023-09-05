using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface ITestQuestionsRepository
    {
        public Task<TestQuestionsList> GetQuestionsListAsync(int testId);
        public Task UpdateQuestionsListAsync(TestQuestionsList testQuestionsList);
    }
}
