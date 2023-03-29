using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.ChoiceOptions
{
    public class ChoiceOption
    {
        /// <summary>
        /// Gets or sets the text of the choice option.
        /// </summary>
        public required string Text { get; set; }
    }
}
