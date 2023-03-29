using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.ChoiceOptions
{
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
