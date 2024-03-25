using Graidex.API.Hubs;
using Graidex.Application.Notifications.SubjectRequests.Accepted;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.SubjectRequests
{
    public class NotifyTeacherSubjectRequestAcceptedHandler : INotificationHandler<SubjectRequestAcceptedNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyTeacherSubjectRequestAcceptedHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(SubjectRequestAcceptedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.TeacherEmail is null)
            {
                return;
            }

            await hub.Clients
                .User(UserIdentity.GetTeacherIdentity(notification.TeacherEmail))
                .ReceiveSubjectRequestAcceptedNotification(notification.Data);
        }
    }
}
