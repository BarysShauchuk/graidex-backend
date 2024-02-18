using FluentValidation;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.TestResult
{
    public class GetSingleChoiceAnswerForStudentDtoValidator : AbstractValidator<GetSingleChoiceAnswerForStudentDto>
    {
        public GetSingleChoiceAnswerForStudentDtoValidator() 
        {
            RuleFor(x => x.ChoiceOptionIndex)
                .NotNull()
                .GreaterThanOrEqualTo(-1).WithMessage("Choice option index must be greater than or equal to -1");
        }
    }
}
