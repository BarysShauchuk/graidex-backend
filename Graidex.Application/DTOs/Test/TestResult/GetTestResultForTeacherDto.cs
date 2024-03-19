using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetTestResultForTeacherDto
    {   
        public List<GetResultAnswerForReviewDto> ResultAnswers { get; set; } = new List<GetResultAnswerForReviewDto>();

        public bool RequireTeacherReview { get; set; }

        public bool ShowToStudent { get; set; }

        public bool CanReview { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public required int TestId { get; set; }

        public string StudentEmail{ get; set; } = string.Empty;

        public int TotalPoints { get; set; }

        public int Grade { get; set; }
    }
}
