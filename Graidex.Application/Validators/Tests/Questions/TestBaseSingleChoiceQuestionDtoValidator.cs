using FluentValidation;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Questions
{
    public class TestBaseSingleChoiceQuestionDtoValidator : AbstractValidator<TestBaseSingleChoiceQuestionDto>
    {
        public TestBaseSingleChoiceQuestionDtoValidator()
        {
            RuleFor(x => x.MaxPoints)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max points must be greater than or equal to 0.");

            RuleFor(x => x.Options)
                .NotEmpty()
                .WithMessage("Options list cannot be empty.");

            RuleForEach(x => x.Options.Select(y => y.Text))
                .NotEmpty()
                .WithMessage("Choice option cannot be empty.");

            RuleFor(x => x.CorrectOptionIndex)
                .Must((entity, index) => index >= 0 && index < entity.Options.Count)
                .WithMessage("Correct option index must be greater than or equal to 0 and less than the number of options.");
        }
    }
}
