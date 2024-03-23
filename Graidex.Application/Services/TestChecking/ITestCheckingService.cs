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
    public interface ITestCheckingService
    {
        public void CheckAnswer(Question question, Answer answer);
        public int CalculateGrade(int points, int maxPoints);
        public void RecalculateTestResultEvaluation(
            TestResult testResult, 
            TestBaseQuestionsList testQuestions, 
            TestResultAnswersList testResultAnswers);
    }
}
