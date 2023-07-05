using Graidex.Application.Interfaces;
using Graidex.Application.Interfaces.TestCheckingQueue;
using OneOf.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests.TestChecking
{
    public class TestCheckingQueue : ITestCheckingInQueue, ITestCheckingOutQueue
    {
        private HashSet<int> PendingTests { get; set; } = new();

        private HashSet<int> ProcessingTests { get; set; } = new();

        public Task AddAsync(int testResultId)
        {
            if (!this.ProcessingTests.Contains(testResultId))
            {
                this.PendingTests.Add(testResultId);
            }

            return Task.CompletedTask;
        }

        public Task<int[]> GetPendingTestsAsync()
        {
            this.ProcessingTests = this.PendingTests;
            this.PendingTests = new();

            return Task.FromResult(this.ProcessingTests.ToArray());
        }
    }
}
