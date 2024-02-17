using Graidex.Application.Interfaces;
using OneOf.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.TestCheckingQueue
{
    public class TestCheckingQueue : ITestCheckingInQueue, ITestCheckingOutQueue
    {
        private HashSet<int> PendingTests { get; set; } = new();

        private HashSet<int> ProcessingTests { get; set; } = new();

        public Task AddAsync(int testResultId)
        {
            if (!ProcessingTests.Contains(testResultId))
            {
                PendingTests.Add(testResultId);
            }

            return Task.CompletedTask;
        }

        public Task<int[]> GetPendingTestsAsync()
        {
            ProcessingTests = PendingTests;
            PendingTests = new();

            return Task.FromResult(ProcessingTests.ToArray());
        }
    }
}
