using Graidex.API.Hubs;
using Graidex.Application.Notifications.TestResults.Created;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.TestResults
{
    public class NotifyTeacherTestResultCreatedHandler : INotificationHandler<TestResultCreatedNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyTeacherTestResultCreatedHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(TestResultCreatedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.TeacherEmail is null)
            {
                return;
            }

            await hub.Clients
                .User(UserIdentity.GetTeacherIdentity(notification.TeacherEmail))
                .ReceiveStudentStartedTestNotification(notification.Data);
        }
    }
}
