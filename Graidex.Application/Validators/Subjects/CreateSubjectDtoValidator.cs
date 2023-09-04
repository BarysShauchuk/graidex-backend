﻿using FluentValidation;
using Graidex.Application.DTOs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Subjects
{
    /// <summary>
    /// Represents a validator for <see cref="CreateSubjectDto"/>.
    /// </summary>
    public class CreateSubjectDtoValidator : AbstractValidator<CreateSubjectDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSubjectDtoValidator"/>.
        /// </summary>
        public CreateSubjectDtoValidator()
        {
            RuleFor(x => x.CustomId)
                .NotEmpty()
                .Length(1, 15).WithMessage("Custom ID length must be from 1 to 15 symbols.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1, 50).WithMessage("Title length must be less than 50 symbols.");
        }
    }
}
