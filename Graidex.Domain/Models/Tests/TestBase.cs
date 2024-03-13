using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests
{
    public abstract class TestBase : SubjectContent
    {
        /// <summary>
        /// Gets or sets the description of the test.
        /// </summary>
        [MaxLength(500)]
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the minimum grade to pass this test.
        /// </summary>
        public int GradeToPass { get; set; }

        /// <summary>
        /// Gets or sets maximum amount of points that can be earned in the test.
        /// </summary>
        public int MaxPoints { get; set; }
    }
}
