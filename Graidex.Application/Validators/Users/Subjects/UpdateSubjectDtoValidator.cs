using FluentValidation;
using Graidex.Application.DTOs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Subjects
{
    /// <summary>
    /// Represents a validator for <see cref="UpdateSubjectDto"/>.
    /// </summary>
    public class UpdateSubjectDtoValidator : AbstractValidator<UpdateSubjectDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSubjectDtoValidator"/>.
        /// </summary>
        public UpdateSubjectDtoValidator()
        {
            RuleFor(x => x.CustomId)
                .NotEmpty()
                .Length(7).WithMessage("Custom ID length must be 7 symbols.")
                .Matches(@"^[A-Z]{3}\d{4}$").WithMessage("Subject custom id should be in format XXX1234.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(0, 50).WithMessage("Title length must be less than 50 symbols.")
                .Matches(@"^[A-Za-z0-9\s\-]+$").WithMessage("Subject title should only contain latin letters, spaces, dashes and numbers.")
                .Matches(@"[\p{L}]+").WithMessage("This field must contain at least one letter.");
        }
    }
}
