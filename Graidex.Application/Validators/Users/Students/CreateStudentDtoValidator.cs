using FluentValidation;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.Validators.Extensions;
using Graidex.Application.Validators.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Students
{   
    /// <summary>
    /// Represents a validator for <see cref="CreateStudentDto"/>.
    /// </summary>
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStudentDtoValidator"/>.
        /// </summary>
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.Name).UserNameRules();

            RuleFor(x => x.Surname).UserNameRules();

            RuleFor(x => x.Email).UserEmailRules();

            RuleFor(x => x.Password).UserPasswordRules();

            RuleFor(x => x.CustomId).UserCustomIdRules();
        }
    }
}