using AutoMapper;
using Graidex.Application.DTOs.Test.TestAttempt;
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
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + test.TimeLimit,
                TestId = testId,
                StudentId = student.Id
            };

            await this.testResultRepository.Add(testResult);

            var questions = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testId);
            var answers = questions.Questions.Select(this.answerFactory.CreateAnswer).ToList();

            // TODO: Shuffle answers

            var answersList = new TestResultAnswersList
            {
                TestResultId = testResult.Id,
                Answers = answers
            };

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

        // TODO: Change return type. Rename method?
        public async Task GetAllQuestionsWithSavedAnswersAsync(int testResultId)
        {
            var questions
                = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResultId);

            var answers
                = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);
            
            var questionsWithAnswers = answers.Answers
                .Select(answer => new GetAnswerDto
                {
                    Question = questions.Questions[answer.QuestionIndex], // Mappings here
                    Answer = answer // And here
                })
                .ToList();

            throw new NotImplementedException();
        }

        // Example of DTO, should be removed. All properties should be DTOs as well.
        // Real DTO can be named differently.
        private class GetAnswerDto
        {
            public required Question Question { get; set; }
            public required Answer Answer { get; set; }
        }

        public async Task<OneOf<Success, NotFound, AttemptFinished>> UpdateTestAttemptByIdAsync(int testResultId)
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

            if (DateTime.Now > test.EndDateTime 
                || DateTime.Now > testAttempt.StartTime + test.TimeLimit 
                || DateTime.Now > testAttempt.EndTime)
            {
                return new AttemptFinished("This test attempt is already finished");
            }

            // TODO: Add answers update logic
            // Get answer from repo, validate types, update answer

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId)
        {
            var updateResult = await this.UpdateTestAttemptByIdAsync(testResultId);
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

            testAttempt.EndTime = DateTime.Now;

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }
    }
}
