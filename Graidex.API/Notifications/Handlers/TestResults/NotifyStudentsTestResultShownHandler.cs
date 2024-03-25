using Graidex.API.Hubs;
using Graidex.Application.Notifications.TestResults.ShownToStudent;
using Graidex.Application.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Notifications.Handlers.TestResults
{
    public class NotifyStudentsTestResultShownHandler : INotificationHandler<TestResultShownToStudentsNotification>
    {
        private readonly IHubContext<NotificationsHub, INotificationsClient> hub;

        public NotifyStudentsTestResultShownHandler(IHubContext<NotificationsHub, INotificationsClient> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(TestResultShownToStudentsNotification notification, CancellationToken cancellationToken)
        {
            if (notification.StudentEmails.Length < 0)
            {
                return;
            }

            var users = notification.StudentEmails
                .Distinct()
                .Select(UserIdentity.GetStudentIdentity);

            await this.hub.Clients
                .Users(users)
                .ReceiveTestResultShownToStudentNotification(notification.Data);
        }
    }
}
