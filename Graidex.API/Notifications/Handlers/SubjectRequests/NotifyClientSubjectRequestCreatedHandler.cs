using Graidex.API.Hubs;
using Graidex.Application.Notifications.SubjectRequests.Created;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.SubjectRequests
{
    public class NotifyStudentSubjectRequestCreatedHandler : INotificationHandler<SubjectRequestCreatedNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyStudentSubjectRequestCreatedHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(SubjectRequestCreatedNotification notification, CancellationToken cancellationToken)
        {
            await hub.Clients
                .User(UserIdentity.GetStudentIdentity(notification.StudentEmail))
                .ReceiveNewSubjectRequestNotification(notification.Data);
        }
    }
}
