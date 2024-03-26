using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Tests.Opens.Student
{
    public class TestOpensStudentNotification : INotification
    {
        public string[] StudentEmails { get; set; } = [];
        public required TestOpensStudentData Data { get; set; }
    }
}
