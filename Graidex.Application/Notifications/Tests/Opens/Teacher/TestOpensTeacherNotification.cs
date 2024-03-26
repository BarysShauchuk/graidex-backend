using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Tests.Opens.Teacher
{
    public class TestOpensTeacherNotification : INotification
    {
        public string? TeacherEmail { get; set; }
        public required TestOpensTeacherData Data { get; set; }
    }
}
