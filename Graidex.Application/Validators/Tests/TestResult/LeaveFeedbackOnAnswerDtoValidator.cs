using FluentValidation;
using Graidex.Application.DTOs.Test.TestResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.TestResult
{
    public class LeaveFeedbackOnAnswerDtoListValidator : AbstractValidator<List<LeaveFeedbackForAnswerDto>>
    {
        public LeaveFeedbackOnAnswerDtoListValidator() 
        {
            RuleForEach(list => list)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.QuestionIndex)
                        .GreaterThanOrEqualTo(0);

                    item.RuleFor(x => x.Points)
                        .GreaterThanOrEqualTo(0);

                    item.RuleFor(x => x.Feedback)
                        .MaximumLength(500);
                });
        }
    }
}
