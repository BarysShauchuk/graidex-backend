using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graidex.Domain.Models.Tests.Test;

namespace Graidex.Application.Factories.Tests
{
    public class TestDraftToTestParameters
    {
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public bool AutoCheckAfterSubmission { get; set; }
        public bool ShuffleQuestions { get; set; }
        public ShowToStudentOptions ReviewResult { get; set; }
    }
}
