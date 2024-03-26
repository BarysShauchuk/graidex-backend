using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Tests.Opens.Student
{
    public class TestOpensStudentData
    {
        public int TestId { get; set; }
        public required string TestTitle { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }
}
