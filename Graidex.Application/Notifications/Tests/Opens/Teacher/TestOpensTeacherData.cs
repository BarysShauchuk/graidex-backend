using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Tests.Opens.Teacher
{
    public class TestOpensTeacherData
    {
        public int TestId { get; set; }
        public required string TestTitle { get; set; }
        public bool IsVisible { get; set; }
        public int StudentCount { get; set; }
    }
}
