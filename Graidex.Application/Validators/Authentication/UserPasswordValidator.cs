using FluentValidation;
using FluentValidation.Validators;
using Graidex.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Authentication
{
    public class UserPasswordValidator : AbstractValidator<String>
    {
        public UserPasswordValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .Length(8, 16).WithMessage("Password length must be from 8 to 16 symbols")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\?\*\.\$]+").WithMessage("Password must contain at least one of [!?*.$].");
        }
    }
}