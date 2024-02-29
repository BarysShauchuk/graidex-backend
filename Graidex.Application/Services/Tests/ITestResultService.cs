﻿using Graidex.Application.OneOfCustomTypes;
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
        public Task<OneOf<GetTestAttemptForStudentDto, UserNotFound, NotFound, OutOfAttempts>> StartTestAttemptAsync(int testId);
        public Task<OneOf<GetTestAttemptForStudentDto, NotFound>> GetAllQuestionsWithSavedAnswersAsync(int testResultId);
        public Task<OneOf<Success, NotFound, ItemImmutable, ValidationFailed>> UpdateTestAttemptByIdAsync(int testResultId, int index, GetAnswerForStudentDto answerDto);
        public Task<OneOf<Success, NotFound, ValidationFailed>> SubmitTestAttemptByIdAsync(int testResultId, int index, GetAnswerForStudentDto answerDto);
        public Task<OneOf<GetTestResultForTeacherDto, NotFound, ItemImmutable>> GetTestResultByIdAsync(int testResultId);
        public Task<OneOf<Success, NotFound, ItemImmutable>> LeaveFeedBackOnAnswerAsync(int testResultId, List<LeaveFeedbackForAnswerDto> feedbackDtos);
        public Task<OneOf<GetStudentAttemptsDescriptionDto, UserNotFound, NotFound>> GetStudentAttemptsDescription(int testId);
    }
}
