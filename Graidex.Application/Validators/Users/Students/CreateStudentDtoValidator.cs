using FluentValidation;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.Validators.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users.Students
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            // TODO: Implement.
        }
    }
}