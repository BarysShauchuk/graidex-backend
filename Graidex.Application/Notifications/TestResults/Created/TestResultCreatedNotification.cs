using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.TestResults.Created
{
    public class TestResultCreatedNotification : INotification
    {
        public string? TeacherEmail { get; set; }
        public required TestResultCreatedData Data { get; set; }
    }
}
