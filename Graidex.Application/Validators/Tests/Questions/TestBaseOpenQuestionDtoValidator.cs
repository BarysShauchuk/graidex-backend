using FluentValidation;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Questions
{
    public class TestBaseOpenQuestionDtoValidator : AbstractValidator<TestBaseOpenQuestionDto>
    {
        public TestBaseOpenQuestionDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Question cannot be empty.");

            RuleFor(x => x.MaxPoints)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max points must be greater than or equal to 0.");
        }
    }
}
