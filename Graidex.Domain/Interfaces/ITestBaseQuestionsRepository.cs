using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface ITestBaseQuestionsRepository
    {
        public Task<TestBaseQuestionsList> GetQuestionsListAsync(int testBaseId);
        public Task UpdateQuestionsListAsync(TestBaseQuestionsList testBaseQuestionsList);
    }
}
