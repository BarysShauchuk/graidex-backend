using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.TestResults.Submitted
{
    public class TestResultSubmittedNotification : INotification
    {
        public string? TeacherEmail { get; set; }
        public required TestResultSubmittedData Data { get; set; }
    }
}
