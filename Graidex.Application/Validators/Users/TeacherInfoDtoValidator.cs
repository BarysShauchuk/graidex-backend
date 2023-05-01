using FluentValidation;
using Graidex.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users
{
    public class TeacherInfoDtoValidator : AbstractValidator<TeacherInfoDto>
    {
        public TeacherInfoDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[\p{L}\- ]+$").WithMessage(x => $"'{nameof(x.Name)}' must contain only letters, dashes or space symbols")
                .Matches(@"[\p{L}]+").WithMessage(x => $"'{nameof(x.Name)}' must contain at least one letter.");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[\p{L}\- ]+$").WithMessage(x => $"'{nameof(x.Surname)}' must contain only letters, dashes or space symbols")
                .Matches(@"[\p{L}]+").WithMessage(x => $"'{nameof(x.Surname)}' must contain at least one letter.");
        }
    }
}