using FluentValidation;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.TestResult
{
    public class GetOpenAnswerForStudentDtoValidator : AbstractValidator<GetOpenAnswerForStudentDto>
    {
        public GetOpenAnswerForStudentDtoValidator() 
        {
            RuleFor(x => x.Text)
                .Length(0, 1000).WithMessage("Answer length should be less than 1000 characters");
        }
    }
}
