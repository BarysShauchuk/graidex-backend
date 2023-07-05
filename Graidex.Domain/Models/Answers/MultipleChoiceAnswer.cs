using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{   
    /// <summary>
    /// Represents an answer to a multiple-choice question in the test.
    /// </summary>
    public class MultipleChoiceAnswer : IAnswer<MultipleChoiceQuestion>
    {   
        /// <summary>
        /// Gets or sets the multiple-choice question this answer relates to.
        /// </summary>
        public required MultipleChoiceQuestion Question { get; set; }

        /// <summary>
        /// Gets or sets the collection of choice option indexes that were selected as an answer.
        /// </summary>
        public virtual ICollection<int> ChoiceOptionIndexes { get; set; } = new List<int>();

        /// <inheritdoc />
        public int Points { get; set; }
    }
}
