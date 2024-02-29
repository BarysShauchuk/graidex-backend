using Graidex.Application.Services.TestChecking;
using Graidex.Application.Services.TestChecking.TestCheckingQueue;
using Graidex.Domain.Exceptions;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests.TestChecking
{
    public class TestCheckingService : ITestCheckingService, ITestResultRecalculationService
    {
        private readonly ITestBaseQuestionsRepository testQuestionsRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly ITestRepository testRepository;
        private readonly ITestResultAnswersRepository testResultAnswersRepository;
        private readonly IAnswerCheckHandler answerCheckHandler;

        public TestCheckingService(
            ITestBaseQuestionsRepository testQuestionsRepository,
            ITestResultRepository testResultRepository,
            ITestRepository testRepository,
            ITestResultAnswersRepository testResultAnswersRepository,
            IAnswerCheckHandler answerCheckHandler)
        {
            this.testQuestionsRepository = testQuestionsRepository;
            this.testResultRepository = testResultRepository;
            this.testRepository = testRepository;
            this.testResultAnswersRepository = testResultAnswersRepository;
            this.answerCheckHandler = answerCheckHandler;
        }

        public async Task CheckTestAttemptAsync(int testResultId)
        {
            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                throw new EntityNotFoundException(
                    $"{nameof(TestResult)} with id={testResultId} wasn't found.", 
                    typeof(TestResult));
            }

            var test = await this.testRepository.GetById(testResult.TestId);
            if (test is null)
            {
                throw new EntityNotFoundException(
                    $"{nameof(Test)} with id={testResult.TestId} wasn't found.", 
                    typeof(Test));
            }

            var studentAnswers
                =  await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);
            
            var testQuestions
                = await this.testQuestionsRepository.GetQuestionsListAsync(testResult.TestId);

            var tasks = new List<Task>();
            foreach (var answer in studentAnswers.Answers)
            {
                var question = testQuestions.Questions[answer.QuestionIndex];
                tasks.Add(this.answerCheckHandler.EvaluateAsync(question, answer));
            }

            await Task.WhenAll(tasks);

            this.RecalculateTestResultEvaluation(testResult, testQuestions, studentAnswers);

            testResult.IsAutoChecked = true;

            if (test.ReviewResult == Test.ReviewResultOptions.AfterAutoCheck)
            {
                testResult.CanReview = true;
            }
            
            await this.testResultRepository.Update(testResult);
            await this.testResultAnswersRepository.UpdateAnswersListAsync(studentAnswers);
        }

        public void RecalculateTestResultEvaluation(TestResult testResult, TestBaseQuestionsList testQuestions, TestResultAnswersList testResultAnswers)
        {
            int points = testResultAnswers.Answers.Sum(x => x.Points);
            int maxPoints = testQuestions.Questions.Sum(x => x.MaxPoints);

            testResult.TotalPoints = points;
            testResult.Grade = (int)Math.Round(points * 10d / maxPoints);
        }
    }
}
