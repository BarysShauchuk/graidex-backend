using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Answers
{
    /// <summary>
    /// Represents an answer to a single-choice question in the test.
    /// </summary>
    public class SingleChoiceAnswer : Answer
    {
        /// <summary>
        /// Gets or sets the choice option index that was selected as an answer.
        /// </summary>
        public int ChoiceOptionIndex { get; set; } = -1;
    }
}
