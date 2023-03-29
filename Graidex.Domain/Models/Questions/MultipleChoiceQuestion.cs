using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models.ChoiceOptions;

namespace Graidex.Domain.Models.Questions
{   
    /// <summary>
    /// Represents a multiple-chioce question.
    /// </summary>
    public class MultipleChoiceQuestion : Question
    {
        /// <summary>
        /// Gets or sets the collection of choice options for the question.
        /// </summary>
        public virtual ICollection<ChoiceOptionRecord> Options { get; set; } = new List<ChoiceOptionRecord>();

        /// <summary>
        /// Gets or sets the number of points awarded per correct option.
        /// </summary>
        public int PointsPerOption { get; set; }

        /// <summary>
        /// Gets the maximum number of points that can be awarded for the question.
        /// </summary>
        public override int MaxPoints => Options.Count(x => x.IsCorrect) * PointsPerOption;
    }
}
