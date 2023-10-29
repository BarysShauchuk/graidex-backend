using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Answers
{
    /// <summary>
    /// Represents an answer to a multiple-choice question in the test.
    /// </summary>
    public class MultipleChoiceAnswer : Answer
    {
        /// <summary>
        /// Gets or sets the collection of choice option indexes that were selected as an answer.
        /// </summary>
        public List<int> ChoiceOptionIndexes { get; set; } = new();
    }
}
