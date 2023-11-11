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

namespace Graidex.Application.Services.Tests
{
    public interface ITestResultService
    {
        public Task<OneOf<Success, UserNotFound, NotFound, OutOfAttempts>> StartTestAttemptAsync(int testId);
        public Task<OneOf<List<TestAttemptQuestionForStudentDto>, ItemImmutable, NotFound>> GetAllQuestionsAsync(int testResultId);
        public Task<OneOf<List<GetAnswerDto>, NotFound>> GetAllQuestionsWithSavedAnswersAsync(int testResultId);
        public Task<OneOf<Success, NotFound, ItemImmutable>> UpdateTestAttemptByIdAsync(int testResultId, int questionIndex, GetAnswerForStudentDto answerDto);
        public Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId, int questionIndex, GetAnswerForStudentDto answerDto);
    }
}
