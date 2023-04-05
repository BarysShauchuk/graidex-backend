using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.ChoiceOptions
{   
    /// <summary>
    /// Represents a choice option in a multiple-choice question.
    /// </summary>
    public record ChoiceOptionRecord
    {
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        public required ChoiceOption Option { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether specified option is correct.
        /// </summary>
        public bool IsCorrect { get; set; }
    }
}
