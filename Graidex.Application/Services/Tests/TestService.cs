using AutoMapper;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.Interfaces;
using Graidex.Application.Interfaces.TestCheckingQueue;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests.Questions;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository testRepository;
        private readonly ITestQuestionsRepository testQuestionsRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly IMapper mapper;
        private readonly ITestCheckingInQueue testCheckingQueue;

        public TestService(
            ITestRepository testRepository,
            ITestQuestionsRepository testQuestionsRepository,
            ITestResultRepository testResultRepository,
            IMapper mapper,
            ITestCheckingInQueue testCheckingQueue)
        {
            this.testRepository = testRepository;
            this.testQuestionsRepository = testQuestionsRepository;
            this.testResultRepository = testResultRepository;
            this.mapper = mapper;
            this.testCheckingQueue = testCheckingQueue;
        }

        public async Task<OneOf<List<TestQuestionDto>, NotFound>> GetTestQuestionsAsync(int testId)
        {
            bool testExists = this.testRepository.GetAll().Any(x => x.Id == testId);
            if (!testExists)
            {
                return new NotFound();
            }

            var questionsList = await this.testQuestionsRepository.GetQuestionsListAsync(testId);
            if (questionsList is null)
            {
                return new List<TestQuestionDto>();
            }

            var questions = questionsList.Questions.Select(mapper.Map<TestQuestionDto>).ToList();

            return questions;
        }

        public Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt)
        {
            throw new NotImplementedException();
        }

        public Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt)
        {
            // Validation, checks, logic, etc.

            //var testResult = mapper.Map<TestResult>(testAttempt);
            //await this.testResultRepository.Add(testResult);
            //await this.testCheckingQueue.AddAsync(testResult.Id);
            //return new Success();

            throw new NotImplementedException();
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestQuestionsAsync(int testId, List<TestQuestionDto> testQuestions)
        {
            bool testExists = this.testRepository.GetAll().Any(x => x.Id == testId);
            if (!testExists)
            {
                return new NotFound();
            }

            var testQuestionsList = new TestQuestionsList
            {
                TestId = testId,
                Questions = testQuestions.Select(mapper.Map<Question>).ToList()
            };

            await this.testQuestionsRepository.UpdateQuestionsListAsync(testQuestionsList);

            return new Success();
        }
    }
}
