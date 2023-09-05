using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestDraft;
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
        public Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt);
        public Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt);
        public Task<OneOf<GetTestDraftDto, ValidationFailed, UserNotFound>> CreateTestDraftForSubjectAsync(int subjectId, CreateTestDraftDto createTestDraftDto);
        public Task<OneOf<GetTestDraftDto, UserNotFound, NotFound>> CreateTestDraftFromTestAsync(int testId);
        public Task<OneOf<GetTestDraftDto, UserNotFound, NotFound>> DuplicateTestDraftAsync(int draftId);
        public Task<OneOf<GetTestDraftDto, UserNotFound, NotFound>> GetTestDraftByIdAsync(int draftId);
        public Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> UpdateTestDraftByIdASync(int draftId, UpdateTestDraftDto updateTestDraftDto);
        public Task<OneOf<Success, UserNotFound, NotFound>> DeleteTestDraftByIdAsync(int draftId);
        public Task<OneOf<GetTestDto, ValidationFailed, UserNotFound, NotFound>> CreateTestForDraftAsync(int draftId, CreateTestDto createTestDto);
        public Task<OneOf<GetTestDto, UserNotFound, NotFound>> GetTestByIdAsync(int testId);
        public Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound, TestImmutable>> UpdateTestByIdAsync(int testId, UpdateTestDto updateTestDto);
        public Task<OneOf<Success, UserNotFound, NotFound, TestImmutable>> DeleteTestByIdAsync(int testId);
        // public Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt);
        // public Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt);

        public Task<OneOf<List<TestQuestionDto>, NotFound>> GetTestQuestionsAsync(int testId);
        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestQuestionsAsync(int testId, List<TestQuestionDto> testQuestions);
    }
}
