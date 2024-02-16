using FluentValidation;
using Graidex.Application.DTOs.Test.TestDraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Draft
{
    /// <summary>
    /// Represents a validator for <see cref="CreateTestDraftDto"/>.
    /// </summary>
    public class UpdateTestDraftDtoValidator : AbstractValidator<UpdateTestDraftDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSubjectDtoValidator"/>.
        /// </summary>
        public UpdateTestDraftDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1, 50).WithMessage("Test draft title must be from 1 to 50 symbols.");

            RuleFor(x => x.Description)
                .Length(0, 500).WithMessage("Test draft description must be less than 500 symbols.");

            RuleFor(x => x.GradeToPass)
                .NotEmpty()
                .InclusiveBetween(0, 10).WithMessage("The grade to pass must be an integer from 0 and 10.");
        }
    }
}
