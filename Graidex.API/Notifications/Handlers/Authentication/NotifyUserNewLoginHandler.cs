using Graidex.API.Hubs;
using Graidex.Application.Notifications.Authentication.Login;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.Authentication
{
    public class NotifyUserNewLoginHandler : INotificationHandler<NewLoginNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyUserNewLoginHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(NewLoginNotification notification, CancellationToken cancellationToken)
        {
            await 
                this.hub.Clients
                .User(UserIdentity.Get(notification.Email, notification.Role))
                .ReceiveLoginNotification(notification.Data);
        }
    }
}
