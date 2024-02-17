using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.TestCheckingQueue
{
    public interface ITestCheckingInQueue
    {
        public Task AddAsync(int testResultId);
    }
}
