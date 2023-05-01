using FluentValidation;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.Validators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Students
{   
    /// <summary>
    /// Represents a validator for <see cref="StudentInfoDto"/>.
    /// </summary>
    public class StudentInfoDtoValidator : AbstractValidator<StudentInfoDto>
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentInfoDtoValidator"/>.
        /// </summary>
        public StudentInfoDtoValidator()
        {
            RuleFor(x => x.Name).UserNameRules();

            RuleFor(x => x.Surname).UserNameRules();

            RuleFor(x => x.CustomId).UserCustomIdRules();
        }
    }
}