using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.SubjectRequests.Created
{
    public class SubjectRequestCreatedNotification : INotification
    {
        public required string StudentEmail { get; set; }
        public required SubjectRequestCreatedData Data { get; set; }
    }
}
