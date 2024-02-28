using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public class OpenAnswerChecker : AnswerChecker<OpenQuestion, OpenAnswer>
    {
        protected override Task EvaluateAsync(OpenQuestion question, OpenAnswer answer)
        {
            answer.Feedback = question.DefaultFeedback;
            answer.Points = 0;

            // TODO [v2/?]: Implement AI-check here

            return Task.CompletedTask;
        }
    }
}
