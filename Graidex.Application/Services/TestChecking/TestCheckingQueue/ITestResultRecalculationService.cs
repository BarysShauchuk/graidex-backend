using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Domain.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.TestCheckingQueue
{
    public interface ITestResultRecalculationService
    {
        public void RecalculateTestResultEvaluation(TestResult testResult, TestBaseQuestionsList testQuestions, TestResultAnswersList testResultAnswers);
    }
}
