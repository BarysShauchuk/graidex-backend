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
        public required List<GetTestResultPreviewForStudentDto> SubmittedTestResults { get; set; }
        public GetTestAttemptPreviewDto? CurrentTestAttempt { get; set; }
        public int NumberOfAvailableTestAttempts { get; set; }
    }
}
