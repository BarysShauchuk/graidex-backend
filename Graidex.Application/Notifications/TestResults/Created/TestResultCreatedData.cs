using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.TestResults.Created
{
    public class TestResultCreatedData
    {
        public int TestId { get; set; }
        public required string StudentEmail { get; set; }
        public DateTimeOffset EndDateTime { get; set; }

        // TODO: Add more properties as needed
    }
}
