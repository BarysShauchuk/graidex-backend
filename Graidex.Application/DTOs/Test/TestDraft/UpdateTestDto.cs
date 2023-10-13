﻿using Graidex.Domain.Models.Tests.Questions;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graidex.Domain.Models.Tests.Test;

namespace Graidex.Application.DTOs.Test.TestDraft
{
    public class UpdateTestDto
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public int GradeToPass { get; set; }

        public bool IsVisible { get; set; }

        public required DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public TimeSpan TimeLimit { get; set; }

        public bool AutoCheckAfterSubmission { get; set; }

        // TODO: Validate using Enum.IsDefined();
        public ReviewResultOptions ReviewResult { get; set; }
    }
}
