﻿using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public class SingleChoiceAnswerChecker : AnswerChecker<SingleChoiceQuestion, SingleChoiceAnswer>
    {
        protected override void Evaluate(
            SingleChoiceQuestion question, SingleChoiceAnswer answer)
        {
            if (question.CorrectOptionIndex == answer.ChoiceOptionIndex)
            {
                answer.Points = question.MaxPoints;
            }
            else
            {
                answer.Points = 0;
            }
        }
    }
}
