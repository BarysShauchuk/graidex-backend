using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models
{
    public class TestBase
    {
        /// <summary>
        /// Gets or sets the unique identifier for the test.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the test.
        /// </summary>
        [MaxLength(50)]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last update of the test.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test is hidden or not.
        /// </summary>
        public bool IsVisible { get; protected set; } = false;

        /// <summary>
        /// Gets or sets id of the subject the test is created for.
        /// </summary>
        public required int SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the collection of questions for the test.
        /// </summary>
        public List<Question> Questions { get; set; } = new List<Question>();

        /// <summary>
        /// Gets or sets the minimum grade to pass this test.
        /// </summary>
        public int GradeToPass { get; set; }
    }
}
