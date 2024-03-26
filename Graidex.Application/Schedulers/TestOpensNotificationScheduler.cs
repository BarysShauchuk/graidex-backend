using Graidex.Application.Notifications.Tests.Opens;
using Graidex.Application.Notifications.Tests.Opens.Student;
using Graidex.Application.Notifications.Tests.Opens.Teacher;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Schedulers
{
    public class TestOpensNotificationScheduler : ITestScheduler, IScheduleRefresher
    {
        private readonly SchedulerConfig config;
        private readonly ITestRepository testRepository;
        private readonly IMediator mediator;

        private readonly Dictionary<int, System.Timers.Timer> Timers = [];

        public TestOpensNotificationScheduler(
            IOptions<SchedulerConfig> options,
            IServiceProvider serviceProvider,
            IMediator mediator)
        {
            this.config = options.Value;
            this.testRepository = serviceProvider.CreateScope()
                .ServiceProvider.GetRequiredService<ITestRepository>();

            this.mediator = mediator;
        }

        public Task RefreshSchedule()
        {
            var after = DateTimeOffset.UtcNow;
            var before = after.Add(this.config.RefreshingPeriod);

            var tests = this.testRepository.GetAll()
                .Where(test => test.StartDateTime > after)
                .Where(test => test.StartDateTime < before)
                .Select(test => new
                {
                    id = test.Id, 
                    startDateTime = test.StartDateTime 
                })
                .ToList();

            this.Timers.Clear();
            foreach (var test in tests)
            {
                this.Timers.Add(test.id, CreateTimer(test.id, test.startDateTime));
            }

            return Task.CompletedTask;
        }

        private System.Timers.Timer CreateTimer(int testId, DateTimeOffset notificationTime)
        {
            var timer = new System.Timers.Timer(notificationTime - DateTimeOffset.UtcNow);
            timer.Elapsed += (_, _) => timer.Stop();
            timer.Elapsed +=
                async (_, _) => await this.mediator.Publish(new TestOpensNotification(testId));

            timer.Start();
            return timer;
        }

        public Task HandleTestAdded(Test test)
        {
            return this.HandleTestUpdated(test);
        }

        public Task HandleTestUpdated(Test test)
        {
            HandleTestDeleted(test.Id);

            if (test.StartDateTime < DateTimeOffset.UtcNow.Add(this.config.RefreshingPeriod))
            {
                var timer = this.CreateTimer(test.Id, test.StartDateTime);
                this.Timers.Add(test.Id, timer);
            }

            return Task.CompletedTask;
        }

        public Task HandleTestDeleted(int testId)
        {
            if (!this.Timers.TryGetValue(testId, out var timer))
            {
                return Task.CompletedTask;
            }

            using (timer)
            {
                timer.Stop();
                this.Timers.Remove(testId);
                timer.Close();
            }

            return Task.CompletedTask;
        }
    }
}
