using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Interfaces.TestCheckingQueue
{
    public interface ITestCheckingInQueue
    {
        public Task AddAsync(int testResultId);
    }
}
