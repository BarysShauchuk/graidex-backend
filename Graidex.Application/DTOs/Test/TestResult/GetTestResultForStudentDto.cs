using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetTestResultForStudentDto
    {
        public List<GetResultAnswerForReviewDto> ResultAnswers { get; set; } = new List<GetResultAnswerForReviewDto>();

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public required int TestId { get; set; }

        public int TotalPoints { get; set; }

        public int Grade { get; set; }
    }
}
