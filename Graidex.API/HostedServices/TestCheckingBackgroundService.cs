using Graidex.API.Extensions;
using Graidex.Application.Interfaces;
using Graidex.Application.Interfaces.TestCheckingQueue;
using Graidex.Application.Services.Tests.TestChecking;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks.Dataflow;

namespace Graidex.API.HostedServices
{
    public class TestCheckingBackgroundService : BackgroundService
    {
        private readonly ILogger<TestCheckingBackgroundService> logger;
        private readonly ITestCheckingOutQueue queue;
        private readonly ITestCheckingService testCheckingService;

        public TestCheckingBackgroundService(
            ILogger<TestCheckingBackgroundService> logger,
            ITestCheckingOutQueue queue,
            ITestCheckingService testCheckingService)
        {
            this.logger = logger;
            this.queue = queue;
            this.testCheckingService = testCheckingService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(TestCheckingBackgroundService)} is running.");

            return ProcessTestCheckingAsync(stoppingToken);
        }

        private async Task ProcessTestCheckingAsync(CancellationToken stoppingToken)
        {
            List<Task> tasks = new();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var testsToCheck = await this.queue.GetPendingTestsAsync();
                    if (testsToCheck.Length == 0)
                    {
                        continue;
                    }

                    tasks.Clear();
                    foreach (var testId in testsToCheck)
                    {
                        tasks.Add(testCheckingService.CheckTestAttemptAsync(testId));
                    }

                    await Task.WhenAll(tasks).WithAggregateException();
                }
                catch (AggregateException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        logger.LogError(innerEx, "Error occurred executing test checking.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred executing test checking.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                $"{nameof(TestCheckingBackgroundService)} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
