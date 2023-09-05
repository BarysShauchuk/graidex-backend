using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.OneOfCustomTypes;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests
{
    public interface ITestService
    {
        // public Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt);
        // public Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt);

        public Task<OneOf<List<TestQuestionDto>, NotFound>> GetTestQuestionsAsync(int testId);
        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestQuestionsAsync(int testId, List<TestQuestionDto> testQuestions);
    }
}
