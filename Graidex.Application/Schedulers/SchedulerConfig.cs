using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Schedulers
{
    public class SchedulerConfig
    {
        private int refreshingPeriodInMinutes = 5;
        public int RefreshingPeriodInMinutes
        {
            get => this.refreshingPeriodInMinutes;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Refreshing period must be greater than 0");
                }

                this.refreshingPeriodInMinutes = value;
            }
        }

        public TimeSpan RefreshingPeriod => TimeSpan.FromMinutes(this.RefreshingPeriodInMinutes);
    }
}
