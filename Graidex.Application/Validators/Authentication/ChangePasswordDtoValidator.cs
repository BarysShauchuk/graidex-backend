using FluentValidation;
using Graidex.Application.Validators.Extensions;
using Graidex.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Authentication
{   
    /// <summary>
    /// Represents a validator for <see cref="ChangePasswordDto"/>.
    /// </summary>
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordDtoValidator"/> class.
        /// </summary>
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.NewPassword)
                .UserPasswordRules()
                .NotEqual(x => x.CurrentPassword).WithMessage(x => $"'{nameof(x.NewPassword)}' should not be equal to '{nameof(x.CurrentPassword)}'");
        }
    }
}