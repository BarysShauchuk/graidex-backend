using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graidex.Domain.Models.Tests.Test;

namespace Graidex.Application.DTOs.Test.TestDraft
{
    public class CreateTestDto
    {
        public required string Title { get; set; }
        public required DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public bool AutoCheckAfterSubmission { get; set; }
        public bool ShuffleQuestions { get; set; }
        public bool IsVisible { get; set; }
        public ShowToStudentOptions ShowToStudent { get; set; }
        public double OrderIndex { get; set; }
    }
}
