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
        public required SingleChoiceQuestion Question { get; set; }

        /// <summary>
        /// Gets the maximum number of points that can be awarded for this answer.
        /// </summary>
        public override int MaxPoints => Question.MaxPoints;

        /// <summary>
        /// Gets or sets the choice option index that was selected as an answer.
        /// </summary>
        public required int ChoiceOptionIndex { get; set; }
    }
}
