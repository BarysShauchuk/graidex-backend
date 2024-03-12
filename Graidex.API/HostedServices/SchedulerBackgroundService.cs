using Graidex.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.HostedServices
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private readonly IHubContext<NotificationsHub, INotificationsClient> notificationsHubContext;

        public SchedulerBackgroundService(
            ILogger<SchedulerBackgroundService> logger,
            IHubContext<NotificationsHub, INotificationsClient> notificationsHubContext)
        {
            _logger = logger;
            this.notificationsHubContext = notificationsHubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Period);

            while (!stoppingToken.IsCancellationRequested 
                && await timer.WaitForNextTickAsync(stoppingToken)) 
            {
                await DoWork();
            }
        }

        private async Task DoWork()
        {
            // TODO: Implement

            var dateTime = DateTime.Now;
            _logger.LogInformation($"Server time: {dateTime}");

            await notificationsHubContext.Clients.All
                .ReceiveApplicationTestNotification($"Server time: {dateTime}");
        }
    }
}
