﻿using FluentValidation;
using Graidex.Application.DTOs.Test.TestDraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Tests.Draft
{
    public class DuplicateTestDraftDtoValidator : AbstractValidator<DuplicateTestDraftDto>
    {
        public DuplicateTestDraftDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1, 50);
        }
    }
}
