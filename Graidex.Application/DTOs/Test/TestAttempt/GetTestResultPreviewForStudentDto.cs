using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestAttempt
{
    public class GetTestResultPreviewForStudentDto
    {
        public int Id { get; set; }
        public bool ShowToStudent { get; set; }
        public bool RequireTeacherReview { get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        public int? TotalPoints { get; set; }
        public int? Grade { get; set; }
    }
}
