using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests.TestChecking
{
    public interface ITestCheckingService
    {
        public Task CheckTestAttemptAsync(int testResultId);
    }
}
