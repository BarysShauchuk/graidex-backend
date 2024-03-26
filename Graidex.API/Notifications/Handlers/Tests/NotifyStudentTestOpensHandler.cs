using Graidex.API.Hubs;
using Graidex.Application.Notifications.Tests.Opens.Student;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.Tests
{
    public class NotifyStudentTestOpensHandler : INotificationHandler<TestOpensStudentNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyStudentTestOpensHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(TestOpensStudentNotification notification, CancellationToken cancellationToken)
        {
            if (notification.StudentEmails.Length == 0)
            {
                return;
            }

            await this.hub.Clients
                .Users(notification.StudentEmails.Select(UserIdentity.GetStudentIdentity))
                .ReceiveTestOpensNotification(notification.Data);
        }
    }
}
