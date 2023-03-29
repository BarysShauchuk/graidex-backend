using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Infrastructure.Data
{   
    /// <summary>
    /// Static class with methods used to populate the database with seed data.
    /// </summary>
    public static class SeedData
    {   
        /// <summary>
        /// Applies pending migration to database.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        /// <exception cref="ArgumentNullException">The GraidexDbContext object is null.</exception>
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
