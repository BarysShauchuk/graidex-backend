using FluentValidation;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Validators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Teachers
{   
    /// <summary>
    /// Represents a validator for <see cref="TeacherInfoDto"/>.
    /// </summary>
    public class TeacherInfoDtoValidator : AbstractValidator<TeacherInfoDto>
    {   
        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherInfoDtoValidator"/>.
        /// </summary>
        public TeacherInfoDtoValidator()
        {
            RuleFor(x => x.Name).UserNameRules();

            RuleFor(x => x.Surname).UserNameRules();
        }
    }
}