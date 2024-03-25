using Graidex.API.Hubs;
using Graidex.Application.Notifications.TestResults.ReviewedByTeacher;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.TestResults
{
    public class NotifyStudentTestResultReviewedByTeacherHandler : INotificationHandler<TestResultReviewedByTeacherNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyStudentTestResultReviewedByTeacherHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(TestResultReviewedByTeacherNotification notification, CancellationToken cancellationToken)
        {
            if (notification.StudentEmail is null)
            {
                return;
            }

            await hub.Clients
                .User(UserIdentity.GetStudentIdentity(notification.StudentEmail))
                .ReceiveTestResultReviewedByTeacherNotification(notification.Data);
        }
    }
}
