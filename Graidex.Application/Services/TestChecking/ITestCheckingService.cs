using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.Tests.TestChecking
{
    public interface ITestCheckingService
    {
        public void CheckAnswer(Question question, Answer answer);
        public Task CheckAnswerWithAI(Question question, Answer answer, CancellationToken cancellationToken);
        public int CalculateGrade(int points, int maxPoints);
        public void RecalculateTestResultEvaluation(
            TestResult testResult, 
            TestBaseQuestionsList testQuestions, 
            TestResultAnswersList testResultAnswers);
    }
}
