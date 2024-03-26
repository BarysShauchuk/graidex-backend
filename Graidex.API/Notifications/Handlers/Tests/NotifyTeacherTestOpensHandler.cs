using Graidex.API.Hubs;
using Graidex.Application.Notifications.Tests.Opens.Teacher;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.Tests
{
    public class NotifyTeacherTestOpensHandler : INotificationHandler<TestOpensTeacherNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyTeacherTestOpensHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(TestOpensTeacherNotification notification, CancellationToken cancellationToken)
        {
            if (notification.TeacherEmail is null)
            {
                return;
            }

            await this.hub.Clients
                .User(UserIdentity.GetTeacherIdentity(notification.TeacherEmail))
                .ReceiveTestOpensNotification(notification.Data);
        }
    }
}
