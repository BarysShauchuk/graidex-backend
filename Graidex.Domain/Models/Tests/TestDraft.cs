using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests
{
    public class TestDraft : TestBase
    {
        public TestDraft()
        {
            this.IsVisible = false;
            this.ItemType ??= nameof(TestDraft);
        }

        /// <summary>
        /// Gets or sets the date and time of the last update of the test.
        /// </summary>
        public DateTime LastUpdate { get; set; }
    }
}
