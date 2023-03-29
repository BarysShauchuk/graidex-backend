using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models.ChoiceOptions;

namespace Graidex.Domain.Models.Questions
{
    /// <summary>
    /// Represents a single-choice question in the test.
    /// </summary>
    public class SingleChoiceQuestion : Question
    {   
        /// <summary>
        /// Gets or sets the collection of choice options for the question.
        /// </summary>
        public virtual ICollection<ChoiceOption> Options { get; set; } = new List<ChoiceOption>();

        /// <summary>
        /// Gets or sets the correct choice option index for the question.
        /// </summary>
        public int CorrectOptionIndex { get; set; }
    }
}
