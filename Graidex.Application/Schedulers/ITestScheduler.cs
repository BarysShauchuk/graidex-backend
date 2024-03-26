using Graidex.Domain.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Schedulers
{
    public interface ITestScheduler
    {
        public Task HandleTestAdded(Test test);
        public Task HandleTestUpdated(Test test);
        public Task HandleTestDeleted(int testId);
    }
}
