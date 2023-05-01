using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Authentication
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.NewPassword)
                .SetValidator(new UserPasswordValidator())
                .NotEqual(x => x.CurrentPassword).WithMessage(x => $"'{nameof(x.NewPassword)}' should not be equal to '{nameof(x.CurrentPassword)}'");
        }
    }
}