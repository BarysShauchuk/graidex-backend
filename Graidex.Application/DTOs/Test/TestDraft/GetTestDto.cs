using Graidex.Domain.Models.Tests.Questions;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graidex.Domain.Models.Tests.Test;

namespace Graidex.Application.DTOs.Test.TestDraft
{
    public class GetTestDto
    {
        public int Id { get; set; }

        public required int SubjectId { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public int GradeToPass { get; set; }

        public int MaxPoints { get; set; }

        public string? ItemType { get; set; }

        public bool IsVisible { get; set; }

        public required DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }

        public TimeSpan TimeLimit { get; set; }
        
        public required List<String> AllowedStudents { get; set; }

        public bool AutoCheckAfterSubmission { get; set; }

        public bool ShuffleQuestions { get; set; }

        public ShowToStudentOptions ShowToStudent { get; set; }

        public double OrderIndex { get; set; }
    }
}
