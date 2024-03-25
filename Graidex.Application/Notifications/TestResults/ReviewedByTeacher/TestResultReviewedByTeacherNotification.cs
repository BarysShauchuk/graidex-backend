using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.TestResults.ReviewedByTeacher
{
    public class TestResultReviewedByTeacherNotification : INotification
    {
        public string? StudentEmail { get; set; }
        public required TestResultReviewedByTeacherData Data { get; set; }
    }
}
