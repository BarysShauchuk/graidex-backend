using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public class SingleChoiceAnswerChecker : AnswerChecker<SingleChoiceQuestion, SingleChoiceAnswer>
    {
        protected override Task EvaluateAsync(
            SingleChoiceQuestion question, SingleChoiceAnswer answer)
        {
            answer.Feedback = question.DefaultComment;
            if (question.CorrectOptionIndex == answer.ChoiceOptionIndex)
            {
                answer.Points = question.MaxPoints;
            }
            else
            {
                answer.Points = 0;
            }

            return Task.CompletedTask;
        }
    }
}
