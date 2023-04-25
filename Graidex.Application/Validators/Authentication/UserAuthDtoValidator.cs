using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Authentication
{
    public class UserAuthDtoValidator : AbstractValidator<UserAuthDto>
    {
        public UserAuthDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 16)
                .Matches(@"[A-Z]+").WithMessage(x => $"'{nameof(x.Password)}' must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage(x => $"'{nameof(x.Password)}' must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage(x => $"'{nameof(x.Password)}' must contain at least one number.")
                .Matches(@"[\!\?\*\.\$]+").WithMessage(x => $"'{nameof(x.Password)}' must contain at least one of [!?*.$].");
        }
    }
}
