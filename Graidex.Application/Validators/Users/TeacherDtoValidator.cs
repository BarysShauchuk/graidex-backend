using FluentValidation;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Validators.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Users
{
    public class TeacherDtoValidator : AbstractValidator<TeacherDto>
    {
        public TeacherDtoValidator()
        {
            RuleFor(x => x.AuthInfo).SetValidator(new UserAuthDtoValidator());

            RuleFor(x => x.TeacherInfo).SetValidator(new TeacherInfoDtoValidator());
        }
    }
}