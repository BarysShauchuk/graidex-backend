using FluentValidation;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.TestResult
{
    public class GetMultipleChoiceAnswerForStudentDtoValidator : AbstractValidator<GetMultipleChoiceAnswerForStudentDto>
    {
        public GetMultipleChoiceAnswerForStudentDtoValidator() 
        {
            RuleFor(x => x.ChoiceOptionIndexes)
                .NotNull()
                .ForEach(y => y.GreaterThanOrEqualTo(0).WithMessage("Choice option index must be greater than or equal to 0"));
        }
    }
}
