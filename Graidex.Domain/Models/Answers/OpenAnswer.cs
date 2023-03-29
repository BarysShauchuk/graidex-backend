using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{   
    /// <summary>
    /// Represents an answer to an open question in the test.
    /// </summary>
    public class OpenAnswer : Answer
    {   
        /// <summary>
        /// Gets or sets the open question this answer relates to.
        /// </summary>
        public required OpenQuestion Question { get; set; }

        /// <summary>
        /// Gets the maximum number of points that can be awarded for this answer.
        /// </summary>
        public override int MaxPoints => Question.MaxPoints;

        /// <summary>
        /// Gets or sets the response text of the open answer.
        /// </summary>
        public required string Text { get; set; }
    }
}
