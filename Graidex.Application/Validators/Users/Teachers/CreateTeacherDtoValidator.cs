using FluentValidation;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Validators.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Teachers
{
    public class CreateTeacherDtoValidator : AbstractValidator<CreateTeacherDto>
    {
        public CreateTeacherDtoValidator()
        {
            // TODO: Implement.
        }
    }
}