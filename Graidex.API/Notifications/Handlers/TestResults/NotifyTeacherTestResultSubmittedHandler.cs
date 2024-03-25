using Graidex.API.Hubs;
using Graidex.Application.Notifications.TestResults.Submitted;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.TestResults
{
    public class NotifyTeacherTestResultSubmittedHandler : INotificationHandler<TestResultSubmittedNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyTeacherTestResultSubmittedHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(TestResultSubmittedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.TeacherEmail is null)
            {
                return;
            }

            await hub.Clients
                .User(UserIdentity.GetTeacherIdentity(notification.TeacherEmail))
                .ReceiveStudentSubmittedTestNotification(notification.Data);
        }
    }
}
