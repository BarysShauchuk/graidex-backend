using FluentValidation;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Questions
{
    public class TestBaseMultipleChoiceQuestionDtoValidator : AbstractValidator<TestBaseMultipleChoiceQuestionDto>
    {
        public TestBaseMultipleChoiceQuestionDtoValidator()
        {
            RuleFor(x => x.Options)
                .NotEmpty()
                .WithMessage("Options list cannot be empty.");

            RuleForEach(x => x.Options.Select(y => y.Option.Text))
                .NotEmpty()
                .WithMessage("Choice option cannot be empty.");

            RuleFor(x => x.PointsPerCorrectAnswer)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Points per correct answer must be greater than or equal to 0.");
        }
    }
}
