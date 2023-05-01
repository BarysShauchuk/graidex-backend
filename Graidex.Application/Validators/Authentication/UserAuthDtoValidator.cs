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

            RuleFor(x => x.Password).SetValidator(new UserPasswordValidator());
        }
    }
}
