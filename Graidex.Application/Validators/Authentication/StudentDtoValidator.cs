using FluentValidation;
using Graidex.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Authentication
{
    public class StudentDtoValidator : AbstractValidator<StudentDto>
    {
        public StudentDtoValidator()
        {
            RuleFor(x => x.AuthInfo).SetValidator(new UserAuthDtoValidator());

            RuleFor(x => x.StudentInfo).SetValidator(new StudentInfoDtoValidator());
        }
    }
}