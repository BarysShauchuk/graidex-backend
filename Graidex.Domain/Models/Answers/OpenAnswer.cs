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
        public required virtual OpenQuestion Question { get; set; }

        /// <summary>
        /// Gets or sets the text of the answer.
        /// </summary>
        public required string Answer { get; set; }
    }
}
