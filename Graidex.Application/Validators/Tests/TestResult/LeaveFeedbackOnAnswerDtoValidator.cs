using FluentValidation;
using Graidex.Application.DTOs.Test.TestResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.TestResult
{
    public class LeaveFeedbackOnAnswerDtoValidator : AbstractValidator<LeaveFeedbackForAnswerDto>
    {
        public LeaveFeedbackOnAnswerDtoValidator() 
        {
            RuleFor(x => x.Points)
                .NotEmpty();

            RuleFor(x => x.Feedback)
                .NotEmpty()
                .MaximumLength(500).WithMessage("Feedback should not exceed 500 characters");
        }
    }
}
