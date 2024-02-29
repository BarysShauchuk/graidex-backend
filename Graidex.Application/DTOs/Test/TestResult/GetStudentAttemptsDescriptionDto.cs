using Graidex.Application.DTOs.Test.TestAttempt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetStudentAttemptsDescriptionDto
    {
        public required List<TestResultPreviewForStudentDto> SubmittedTestResults { get; set; }
        public int? CurrentTestResultId { get; set; }
        public required int NumberOfAvailableTestAttempts { get; set; }
    }
}
