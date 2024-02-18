using FluentValidation;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.Validators.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.TestResult
{
    public class GetAnswerForStudentDtoValidator : AbstractValidator<GetAnswerForStudentDto>
    {
        public GetAnswerForStudentDtoValidator() 
        {
            RuleFor(x => x).SetInheritanceValidator(x =>
            {
                x.Add(new GetOpenAnswerForStudentDtoValidator());
                x.Add(new GetSingleChoiceAnswerForStudentDtoValidator());
                x.Add(new GetMultipleChoiceAnswerForStudentDtoValidator());
            });
        }
    }
}
