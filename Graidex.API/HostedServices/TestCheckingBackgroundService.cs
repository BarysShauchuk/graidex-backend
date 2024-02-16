using Graidex.API.Extensions;
using Graidex.Application.Interfaces;
using Graidex.Application.Services.TestChecking.TestCheckingQueue;
using Graidex.Application.Services.Tests.TestChecking;
using Graidex.Domain.Models.Tests;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks.Dataflow;

namespace Graidex.API.HostedServices
{
    public class TestCheckingBackgroundService : BackgroundService
    {
        private readonly ILogger<TestCheckingBackgroundService> logger;
        private readonly ITestCheckingOutQueue queue;
        private readonly IServiceProvider serviceProvider;

        public TestCheckingBackgroundService(
            ILogger<TestCheckingBackgroundService> logger,
            ITestCheckingOutQueue queue,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.queue = queue;
            this.serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(TestCheckingBackgroundService)} is running.");

            return ProcessTestCheckingAsync(stoppingToken);
        }

        private async Task ProcessTestCheckingAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    int[] testsToCheck = await this.queue.GetPendingTestsAsync();
                    if (testsToCheck.Length == 0)
                    {
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }

                    var tasks = testsToCheck.Select(testId => this.CheckTestAsync(testId));
                    await Task.WhenAll(tasks).WithAggregateException();
                }
                catch (AggregateException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        logger.LogError(innerEx, "Error occurred executing test checking.");
                    }
                }
                catch (OperationCanceledException) { }
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

        private async Task CheckTestAsync(int testResultId)
        {
            using var scope = this.serviceProvider.CreateScope();
            var testCheckingService =
                scope.ServiceProvider.GetRequiredService<ITestCheckingService>();

            await testCheckingService.CheckTestAttemptAsync(testResultId);
        }
    }
}
