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
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(x => x.StartDateTime)
                .NotEmpty()
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must not be earlier than current time");

            RuleFor(x => x.EndDateTime)
                .NotEmpty()
                .GreaterThan(x => x.StartDateTime).WithMessage("End time must not be earlier than start time");

            RuleFor(x => x.TimeLimit)
                .NotEmpty()
                .LessThanOrEqualTo(x => x.EndDateTime - x.StartDateTime).WithMessage("Time limit must be less than the difference between start and end time")
                .LessThanOrEqualTo(TimeSpan.FromDays(14));

            RuleFor(x => x.ShowToStudent)
                .IsInEnum().WithMessage("ShowToStudent must be either 1-'SetManually' or 2-'AfterSubmission'");
        }
    }
}
