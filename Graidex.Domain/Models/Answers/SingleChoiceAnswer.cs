using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{   
    /// <summary>
    /// Represents an answer to a single-choice question in the test.
    /// </summary>
    public class SingleChoiceAnswer : Answer
    {   
        /// <summary>
        /// Gets or sets the single-choice question this answer relates to.
        /// </summary>
        public required virtual SingleChoiceQuestion Question { get; set; }

        /// <summary>
        /// Gets or sets the choice option that was selected as an answer.
        /// </summary>
        public required virtual ChoiceOption Answer { get; set; }
    }
}
