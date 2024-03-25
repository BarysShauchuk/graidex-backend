using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.TestResults.ShownToStudent
{
    public class TestResultShownToStudentsNotification : INotification
    {
        public string[] StudentEmails { get; set; } = [];
        public required TestResultShownToStudentData Data { get; set; }

    }
}
