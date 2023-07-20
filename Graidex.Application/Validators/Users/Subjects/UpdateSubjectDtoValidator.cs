﻿using FluentValidation;
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
                .Length(1, 15).WithMessage("Custom ID length must be from 1 to 15 symbols.")
                .Matches(@"^[\x20-\x7E]+$").WithMessage("Subject custom id should only contain printable ASCII characters.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1, 50).WithMessage("Title length must be less than 50 symbols.")
                .Matches(@"^[\x20-\x7E]+$").WithMessage("Subject title should only contain printable ASCII characters.")
                .Matches(@"[\p{L}]+").WithMessage("This field must contain at least one letter.");
        }
    }
}
