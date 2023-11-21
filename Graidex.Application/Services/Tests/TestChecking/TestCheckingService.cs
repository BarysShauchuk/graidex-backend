using Graidex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests.TestChecking
{
    public class TestCheckingService : ITestCheckingService
    {
        public async Task CheckTestAttemptAsync(int testResultId)
        {
            await Task.Delay(1_000);
        }

        public async Task RecalculateTestResultEvaluation(int testResultId)
        {
            await Task.Delay(1);
        }
    }
}
