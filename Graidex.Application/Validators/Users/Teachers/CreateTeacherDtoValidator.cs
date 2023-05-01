using FluentValidation;
using Graidex.Application.Validators.Extensions;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Validators.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Teachers
{   
    /// <summary>
    /// Represents a validator for <see cref="CreateTeacherDto"/>.
    /// </summary>
    public class CreateTeacherDtoValidator : AbstractValidator<CreateTeacherDto>
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTeacherDtoValidator"/>.
        /// </summary>
        public CreateTeacherDtoValidator()
        {
            RuleFor(x => x.Name).UserNameRules();

            RuleFor(x => x.Surname).UserNameRules();

            RuleFor(x => x.Email).UserEmailRules();

            RuleFor(x => x.Password).UserPasswordRules();
        }
    }
}