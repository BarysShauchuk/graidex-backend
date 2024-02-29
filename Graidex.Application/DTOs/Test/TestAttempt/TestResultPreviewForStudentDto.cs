using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestAttempt
{
    public class TestResultPreviewForStudentDto
    {
        public int Id { get; set; }
        public bool CanReview { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int? TotalPoints { get; set; }
        public int? Grade { get; set; }
    }
}
