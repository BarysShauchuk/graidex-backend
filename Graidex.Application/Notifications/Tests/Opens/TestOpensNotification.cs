using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Tests.Opens
{
    public class TestOpensNotification : INotification
    {
        public TestOpensNotification(int testId)
        {
            this.TestId = testId;
        }

        public int TestId { get; set; }
    }
}
