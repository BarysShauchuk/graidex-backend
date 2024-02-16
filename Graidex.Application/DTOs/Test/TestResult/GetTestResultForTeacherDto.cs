using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetTestResultForTeacherDto
    {   
        public List<GetResultAnswerForTeacherDto> ResultAnswers { get; set; } = new List<GetResultAnswerForTeacherDto>();

        public bool IsAutoChecked { get; set; }

        public bool CanReview { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public required int TestId { get; set; }

        public string StudentEmail{ get; set; } = string.Empty;

        public int TotalPoints { get; set; }

        public int Grade { get; set; }
    }
}
