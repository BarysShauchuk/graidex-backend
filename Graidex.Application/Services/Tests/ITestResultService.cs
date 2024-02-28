using Graidex.Application.OneOfCustomTypes;
using OneOf.Types;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.TestResult;

namespace Graidex.Application.Services.Tests
{
    public interface ITestResultService
    {
        public Task<OneOf<GetTestAttemptForStudentDto, UserNotFound, NotFound, ConditionFailed>> StartTestAttemptAsync(int testId);
        public Task<OneOf<GetTestAttemptForStudentDto, NotFound, ConditionFailed>> GetAllQuestionsWithSavedAnswersAsync(int testResultId);
        public Task<OneOf<Success, NotFound, ItemImmutable, ValidationFailed>> UpdateTestAttemptByIdAsync(int testResultId, int index, GetAnswerForStudentDto answerDto);
        public Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId);
        public Task<OneOf<Success, ConditionFailed>> AddTestResultsToCheckingQueueAsync(int testId, IEnumerable<int> testResultIds);
        public Task<OneOf<GetTestResultForTeacherDto, NotFound, ConditionFailed>> GetTestResultByIdAsync(int testResultId);
        public Task<OneOf<Success, NotFound, ItemImmutable>> LeaveFeedBackOnAnswerAsync(int testResultId, int index, LeaveFeedbackForAnswerDto feedbackDto);
    }
}
