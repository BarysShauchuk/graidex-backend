using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Infrastructure.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(GraidexDbContext? context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            // TODO: Add seed data
        }
    }
}
