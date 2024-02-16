using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public class MultipleChoiceAnswerChecker : AnswerChecker<MultipleChoiceQuestion, MultipleChoiceAnswer>
    {
        protected override Task EvaluateAsync(MultipleChoiceQuestion question, MultipleChoiceAnswer answer)
        {
            answer.Points = 0;

            foreach (int choice in answer.ChoiceOptionIndexes)
            {
                if (question.Options[choice].IsCorrect)
                {
                    answer.Points += question.PointsPerCorrectAnswer;
                }
            }

            if (answer.Points < 0)
            {
                answer.Points = 0;
            }
            else if (answer.Points > question.MaxPoints)
            {
                answer.Points = question.MaxPoints;
            }

            answer.Feedback = question.DefaultComment;

            return Task.CompletedTask;
        }
    }
}
