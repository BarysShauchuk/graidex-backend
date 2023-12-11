using AutoMapper;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestResult;
using Graidex.Application.Factories;
using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Graidex.Application.Services.Tests
{
    public class TestResultService : ITestResultService
    {
        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITestRepository testRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly ITestBaseQuestionsRepository testBaseQuestionsRepository;
        private readonly ITestResultAnswersRepository testResultAnswersRepository;
        private readonly IAnswerFactory answerFactory;
        private readonly IMapper mapper;
        private const int ExtraMinutesForSubmission = 1;

        public TestResultService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITestRepository testRepository,
            ITestResultRepository testResultRepository,
            ITestBaseQuestionsRepository testBaseQuestionsRepository,
            ITestResultAnswersRepository testResultAnswersRepository,
            IAnswerFactory answerFactory,
            IMapper mapper)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.testRepository = testRepository;
            this.testResultRepository = testResultRepository;
            this.testBaseQuestionsRepository = testBaseQuestionsRepository;
            this.testResultAnswersRepository = testResultAnswersRepository;
            this.answerFactory = answerFactory;
            this.mapper = mapper;
        }

        public async Task<OneOf<Success, UserNotFound, NotFound, OutOfAttempts>> StartTestAttemptAsync(int testId)
        {
            string email = this.currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            bool attemptAlreadyStarted = this.testResultRepository
                .GetAll()
                .Any(x => x.StudentId == student.Id 
                && x.TestId == test.Id);

            if (attemptAlreadyStarted)
            {
                return new OutOfAttempts("No more attempts available");
            }

            var testResult = new TestResult
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow + test.TimeLimit,
                TestId = testId,
                StudentId = student.Id
            };

            await this.testResultRepository.Add(testResult);

            // TODO: Add null check

            var questions = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testId);
            var answers = questions.Questions.Select(this.answerFactory.CreateAnswer).ToList();

            // TODO: Shuffle answers

            var answersList = new TestResultAnswersList
            {
                TestResultId = testResult.Id,
                Answers = answers
            };

            ShuffleList(answersList.Answers, testResult.Id);

            await this.testResultAnswersRepository.CreateAnswersListAsync(answersList);

            return new Success();
        }

        private static void ShuffleList<T>(IList<T> list, int? seed = null)
        {
            Random random = seed.HasValue ? new Random(seed.Value) : new Random();

            int n = list.Count;
            T temp;
            while (n > 1)
            {
                int k = random.Next(n);
                n--;
                temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        public async Task<OneOf<List<GetAnswerDto>, NotFound>> GetAllQuestionsWithSavedAnswersAsync(int testResultId)
        {
            var testAttempt = await this.testResultRepository.GetById(testResultId);
            if (testAttempt is null)
            {
                return new NotFound();
            }

            var questions
                = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testAttempt.TestId);

            var answers
                = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);

            var questionsWithAnswers = answers.Answers
                .Select(answer => new GetAnswerDto
                {
                    Question = mapper.Map<TestAttemptQuestionForStudentDto>(questions.Questions[answer.QuestionIndex]),
                    Answer = mapper.Map<GetAnswerForStudentDto>(answer)
                })
                .ToList();

            return questionsWithAnswers;
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable>> UpdateTestAttemptByIdAsync(int testResultId, int index, GetAnswerForStudentDto answerDto)
        {
            var testAttempt = await this.testResultRepository.GetById(testResultId);
            if (testAttempt is null)
            {
                return new NotFound();
            }

            var test = await this.testRepository.GetById(testAttempt.TestId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow > test.EndDateTime.AddMinutes(ExtraMinutesForSubmission)
                || DateTime.UtcNow > (testAttempt.StartTime + test.TimeLimit).AddMinutes(ExtraMinutesForSubmission)
                || DateTime.UtcNow > testAttempt.EndTime.AddMinutes(ExtraMinutesForSubmission))
            {
                return new ItemImmutable("This test attempt is already finished");
            }

            // TODO: Add answers update logic
            // Get answer from repo, validate types, update answer

            // TODO: Add validation 

            await this.testResultAnswersRepository.UpdateAnswerAsync(testResultId, index, mapper.Map<Answer>(answerDto));

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId, int index, GetAnswerForStudentDto answerDto)
        {
            var updateResult = await this.UpdateTestAttemptByIdAsync(testResultId, index, answerDto);
            if (updateResult.IsT1)
            {
                return new NotFound();
            }

            if (updateResult.IsT2)
            {
                return new Success();
            }

            var testAttempt = await this.testResultRepository.GetById(testResultId);
            if (testAttempt is null)
            {
                return new NotFound();
            }

            testAttempt.EndTime = DateTime.UtcNow;

            // TODO: Add validation 

            await this.testResultAnswersRepository.UpdateAnswerAsync(testResultId, index, mapper.Map<Answer>(answerDto));

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }

        public async Task<OneOf<GetTestResultForTeacherDto, NotFound, ItemImmutable>> GetTestResultByIdAsync(int testResultId)
        {
            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                return new NotFound();
            }

            var student = await this.studentRepository.GetById(testResult.StudentId);
            if (student is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow < testResult.EndTime) 
            {
                return new ItemImmutable("The test attempt is not finished yet");
            }

            var questions
                = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResult.TestId);

            var answers
                = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);

            var questionsWithAnswers = answers.Answers
                .Select(answer => new GetResultAnswerForTeacherDto
                {
                    Question = mapper.Map<TestBaseQuestionDto>(questions.Questions[answer.QuestionIndex]),
                    Answer = mapper.Map<GetResultAnswerDto>(answer)
                })
                .ToList();

            var testResultDto = this.mapper.Map<GetTestResultForTeacherDto>(testResult);
            testResultDto.ResultAnswers = questionsWithAnswers;
            testResultDto.StudentEmail = student.Email;

            return testResultDto;
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable>> LeaveFeedBackOnAnswerAsync(int testResultId, int index, LeaveFeedbackForAnswerDto feedbackDto)
        {
            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow < testResult.EndTime)
            {
                return new ItemImmutable("The test attempt is not finished yet");
            }

            // TODO: Add validation
            // TODO: Add grade and total points recalculation

            await this.testResultAnswersRepository.UpdateAnswerAsync(testResultId, index, mapper.Map<Answer>(feedbackDto));

            await this.testResultRepository.Update(testResult);

            return new Success();
        }
    }
}
