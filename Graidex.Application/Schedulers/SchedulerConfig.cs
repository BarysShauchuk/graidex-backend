using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Schedulers
{
    public class SchedulerConfig
    {
        public required int RefreshingPeriodInMinutes { get; set; }
        public TimeSpan RefreshingPeriod => TimeSpan.FromMinutes(this.RefreshingPeriodInMinutes);
    }
}
