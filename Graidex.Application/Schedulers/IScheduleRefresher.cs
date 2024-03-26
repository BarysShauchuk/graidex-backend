using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Schedulers
{
    public interface IScheduleRefresher
    {
        public Task RefreshSchedule();
    }
}
