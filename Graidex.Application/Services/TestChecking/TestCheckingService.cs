using Graidex.Application.Services.TestChecking;
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
    public class TestCheckingService : ITestCheckingService
    {
        private readonly ITestBaseQuestionsRepository testQuestionsRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly ITestResultAnswersRepository testResultAnswersRepository;
        private readonly IAnswerCheckHandler answerCheckHandler;

        public TestCheckingService(
            ITestBaseQuestionsRepository testQuestionsRepository,
            ITestResultRepository testResultRepository,
            ITestResultAnswersRepository testResultAnswersRepository,
            IAnswerCheckHandler answerCheckHandler)
        {
            this.testQuestionsRepository = testQuestionsRepository;
            this.testResultRepository = testResultRepository;
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

            var studentAnswers
                =  await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);
            
            var testQuestions
                = await this.testQuestionsRepository.GetQuestionsListAsync(testResult.TestId);

            var tasks = new List<Task<(int points, int maxPoints)>>();
            foreach (var answer in studentAnswers.Answers)
            {
                var question = testQuestions.Questions[answer.QuestionIndex];
                tasks.Add(this.EvaluateAnswerAsync(question, answer));
            }

            var results = await Task.WhenAll(tasks);

            int points = results.Sum(x => x.points);
            int maxPoints = results.Sum(x => x.maxPoints);

            testResult.TotalPoints = points;
            testResult.Grade = (int)Math.Round(points*10d / maxPoints);
            testResult.IsAutoChecked = true;
            
            await this.testResultRepository.Update(testResult);
            await this.testResultAnswersRepository.UpdateAnswersListAsync(studentAnswers);
        }

        private async Task<(int points, int maxPoints)> EvaluateAnswerAsync(
            Question question, Answer answer)
        {
            await this.answerCheckHandler.EvaluateAsync(question, answer);
            return (answer.Points, question.MaxPoints);
        }

        public async Task RecalculateTestResultEvaluation(int testResultId)
        {
            await Task.Delay(1);
            // TODO: Implement or remove and use CheckTestAttemptAsync
        }
    }
}
