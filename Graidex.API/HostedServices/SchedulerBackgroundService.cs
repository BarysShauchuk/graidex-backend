using Graidex.Application.Schedulers;
using Microsoft.Extensions.Options;

namespace Graidex.API.HostedServices
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly SchedulerConfig config;
        private readonly IEnumerable<IScheduleRefresher> scheduleRefreshers;

        public SchedulerBackgroundService(
            IOptions<SchedulerConfig> options,
            IEnumerable<IScheduleRefresher> scheduleRefreshers)
        {
            this.config = options.Value;
            this.scheduleRefreshers = scheduleRefreshers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(config.RefreshingPeriod);

            do
            {
                await Task.WhenAll(this.scheduleRefreshers.Select(x => x.RefreshSchedule()));
            }
            while (!stoppingToken.IsCancellationRequested && 
                    await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
