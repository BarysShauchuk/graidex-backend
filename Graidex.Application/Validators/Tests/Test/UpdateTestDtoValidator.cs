using FluentValidation;
using Graidex.Application.DTOs.Test.TestDraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Test
{
    /// <summary>
    /// Represents a validator for <see cref="UpdateTestDto"/>.
    /// </summary>
    public class UpdateTestDtoValidator : AbstractValidator<UpdateTestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTestDtoValidator"/>.
        /// </summary>
        public UpdateTestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1, 50).WithMessage("Test title must be from 1 to 50 symbols.");

            RuleFor(x => x.Description)
                .Length(0, 500).WithMessage("Test description must be less than 500 symbols.");

            RuleFor(x => x.GradeToPass)
                .NotEmpty()
                .InclusiveBetween(0, 10).WithMessage("The grade to pass must be an integer from 0 and 10.");

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
                .NotEmpty()
                .IsInEnum().WithMessage("Review result must be 1-'SetManually', 2-'AfterSubmission' or 3-'AfterAutoCheck'");
        }
    }
}
