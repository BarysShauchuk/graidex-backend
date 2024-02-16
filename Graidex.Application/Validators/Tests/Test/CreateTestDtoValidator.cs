using FluentValidation;
using Graidex.Application.DTOs.Test.TestDraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graidex.Domain.Models.Tests.Test;

namespace Graidex.Application.Validators.Tests.Test
{
    /// <summary>
    /// Represents a validator for <see cref="CreateTestDto"/>.
    /// </summary>
    public class CreateTestDtoValidator : AbstractValidator<CreateTestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTestDtoValidator"/>.
        /// </summary>
        public CreateTestDtoValidator() 
        {
            RuleFor(x => x.StartDateTime)
                .NotEmpty()
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must not be earlier than current time");

            RuleFor(x => x.EndDateTime)
                .NotEmpty()
                .GreaterThan(x => x.StartDateTime).WithMessage("End time must not be earlier than start time");

            RuleFor(x => x.TimeLimit)
                .NotEmpty()
                .LessThan(x => x.EndDateTime - x.StartDateTime).WithMessage("Time limit must be less than the difference between start and end time");

            RuleFor(x => x.ReviewResult)
                .IsInEnum().WithMessage("Review result must be 1-'SetManually', 2-'AfterSubmission' or 3-'AfterAutoCheck'");
        }
    }
}
