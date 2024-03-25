using Graidex.Application.OneOfCustomTypes;
using OneOf.Types;
using OneOf;
using Graidex.Application.DTOs.Test.TestAttempt;
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
        
        public Task<OneOf<Success, ConditionFailed>> SetShowTestResultsToStudentsAsync(int testId, IEnumerable<int> testResultIds, bool show);
        
        public Task<OneOf<Success, NotFound>> CheckTestResultsWithAIAsync(int testId, IEnumerable<int> testResultIds, CancellationToken cancellationToken);
        
        public Task<OneOf<GetTestResultForTeacherDto, NotFound, ConditionFailed>> GetTestResultByIdAsync(int testResultId);
        
        public Task<OneOf<List<GetTestResultListedForTeacherDto>, NotFound>> GetAllTestResultsByTestIdAsync(int testId);
        
        public Task<OneOf<GetTestResultForStudentDto, NotFound, ConditionFailed>> GetTestResultForStudentByIdAsync(int testResultId);
        
        public Task<OneOf<Success, ValidationFailed, NotFound, ConditionFailed>> LeaveFeedBackOnAnswerAsync(int testResultId, List<LeaveFeedbackForAnswerDto> feedbackDtos);
        
        public Task<OneOf<GetStudentAttemptsDescriptionDto, UserNotFound, NotFound>> GetStudentAttemptsDescription(int testId);
    }
}
