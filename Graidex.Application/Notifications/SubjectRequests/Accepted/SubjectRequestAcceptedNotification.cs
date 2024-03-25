using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.SubjectRequests.Accepted
{
    public class SubjectRequestAcceptedNotification : INotification
    {
        public string? TeacherEmail { get; set; }
        public required SubjectRequestAcceptedData Data { get; set; }
    }
}
