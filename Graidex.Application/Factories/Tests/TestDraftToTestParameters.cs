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
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public bool AutoCheckAfterSubmission { get; set; }
        public ReviewResultOptions ReviewResult { get; set; }
        public int MaxPoints { get; set; }
    }
}
