using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestDraft
{
    public class UpdateTestDraftDto
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public int GradeToPass { get; set; }

        public bool IsVisible { get; set; }

        public double OrderIndex { get; set; }
    }
}
