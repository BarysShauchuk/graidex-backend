using FluentValidation;
using Graidex.Application.DTOs.Test.TestDraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Test
{
    public class UpdateTestTimeDtoValidator : AbstractValidator<UpdateTestTimeDto>
    {
        public UpdateTestTimeDtoValidator() 
        { 
            RuleFor(x => x.StartDateTime)
                .NotEmpty()
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must not be earlier than current time");

            RuleFor(x => x.EndDateTime)
                .NotEmpty()
                .GreaterThan(x => x.StartDateTime).WithMessage("End time must not be earlier than start time");

            RuleFor(x => x.TimeLimit)
                .NotEmpty()
                .LessThanOrEqualTo(x => x.EndDateTime - x.StartDateTime).WithMessage("Time limit must be less than the difference between start and end time");
        }
    }
}
