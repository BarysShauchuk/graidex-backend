using FluentValidation;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.Questions.ConcreteQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Questions
{
    public class TestBaseQuestionDtosListValidator : AbstractValidator<List<TestBaseQuestionDto>>
    {
        public TestBaseQuestionDtosListValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("Questions list cannot be empty.");

            RuleForEach(x => x).SetInheritanceValidator(x =>
            {
                x.Add(new TestBaseOpenQuestionDtoValidator());
                x.Add(new TestBaseSingleChoiceQuestionDtoValidator());
                x.Add(new TestBaseMultipleChoiceQuestionDtoValidator());
            });
        }
    }
}
