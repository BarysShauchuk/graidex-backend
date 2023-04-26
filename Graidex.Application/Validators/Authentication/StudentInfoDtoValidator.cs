using FluentValidation;
using Graidex.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Authentication
{
    public class StudentInfoDtoValidator : AbstractValidator<StudentInfoDto>
    {
        public StudentInfoDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[\p{L}-]+$")
                .WithMessage(x => $"'{nameof(x.Name)}' must contain only letters");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[\p{L}-]+$")
                .WithMessage(x => $"'{nameof(x.Name)}' must contain only letters");

            // TODO: Add a rule for a specific format of the custom id?
            RuleFor(x => x.CustomId)
                .Length(0, 15);
        }
    }
}