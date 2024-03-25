using Graidex.Application.Services.AI;
using Graidex.Application.Services.TestChecking;
using Graidex.Application.Services.TestChecking.AnswerCheckers;
using Graidex.Application.Services.TestChecking.AnswerCheckers.AnswerCheckersAI;
using Graidex.Domain.Exceptions;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests.TestChecking
{
    public class TestCheckingService : ITestCheckingService
    {
        private readonly Dictionary<(Type, Type), IAnswerChecker> answerCheckers = [];
        private readonly Dictionary<(Type, Type), IAnswerCheckerAI> answerCheckersAI = [];
        public TestCheckingService(
            IEnumerable<IAnswerChecker> answerCheckers,
            IEnumerable<IAnswerCheckerAI> answerCheckersAI)
        {   
            this.answerCheckers = answerCheckers
                .ToDictionary(x => (x.QuestionType, x.AnswerType));

            this.answerCheckersAI = answerCheckersAI
                .ToDictionary(x => (x.QuestionType, x.AnswerType));
        }

        public void CheckAnswer(Question question, Answer answer)
        {
            answer.Feedback = question.DefaultFeedback;

            if (!this.answerCheckers.TryGetValue(
                (question.GetType(), answer.GetType()), out var answerChecker))
            {
                return;
            }

            answerChecker.Evaluate(question, answer);
        }

        public async Task CheckAnswerWithAI(Question question, Answer answer, CancellationToken cancellationToken)
        {
            answer.Feedback = question.DefaultFeedback;

            if (!this.answerCheckersAI.TryGetValue(
                               (question.GetType(), answer.GetType()), out var answerCheckerAI))
            {
                return;
            }

            await answerCheckerAI.EvaluateAsync(question, answer, cancellationToken);
        }

        public int CalculateGrade(int points, int maxPoints)
        {
            return (int)Math.Round(points * 10d / maxPoints);
        }

        public void RecalculateTestResultEvaluation(TestResult testResult, TestBaseQuestionsList testQuestions, TestResultAnswersList testResultAnswers)
        {
            int points = testResultAnswers.Answers.Sum(x => x.Points);
            int maxPoints = testQuestions.Questions.Sum(x => x.MaxPoints);

            testResult.TotalPoints = points;
            testResult.Grade = this.CalculateGrade(points, maxPoints);
        }
    }
}
