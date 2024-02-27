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

        public Task<OneOf<GetTestDraftDto, ValidationFailed>> CreateTestDraftForSubjectAsync(int subjectId, CreateTestDraftDto createTestDraftDto);

        public Task<OneOf<GetTestDraftDto, NotFound>> CreateTestDraftFromTestAsync(int testId);

        public Task<OneOf<GetTestDraftDto, NotFound>> DuplicateTestDraftAsync(int draftId);

        public Task<OneOf<GetTestDraftDto, NotFound>> GetTestDraftByIdAsync(int draftId);

        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestDraftByIdASync(int draftId, UpdateTestDraftDto updateTestDraftDto);

        public Task<OneOf<Success, NotFound>> DeleteTestDraftByIdAsync(int draftId);

        public Task<OneOf<GetTestDto, ValidationFailed, NotFound>> CreateTestForDraftAsync(int draftId, CreateTestDto createTestDto);

        public Task<OneOf<GetTestDto, NotFound>> GetTestByIdAsync(int testId);

        public Task<OneOf<GetVisibleTestDto, NotFound>> GetVisibleTestByIdAsync(int testId);

        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestByIdAsync(int testId, UpdateTestDto updateTestDto);
        public Task<OneOf<Success, ValidationFailed, NotFound, ItemImmutable>> UpdateTestTimeByIdAsync(int testId, UpdateTestTimeDto updateTestTimeDto);

        public Task<OneOf<Success, NotFound, ItemImmutable>> DeleteTestByIdAsync(int testId);

        // public Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt);

        // public Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt);

        public Task<OneOf<List<TestBaseQuestionDto>, NotFound>> GetTestQuestionsAsync(int testId);
        public Task<OneOf<Success, ValidationFailed, ItemImmutable, NotFound>> UpdateTestQuestionsAsync(int testId, List<TestBaseQuestionDto> testQuestions);

        public Task<OneOf<List<TestBaseQuestionDto>, NotFound>> GetTestDraftQuestionsAsync(int testId);
        public Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestDraftQuestionsAsync(int testId, List<TestBaseQuestionDto> testQuestions);

        public Task<OneOf<Success, NotFound>> AddStudentsToTestAsync(int testId, List<String> studentEmails);
    }
}
