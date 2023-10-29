using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Answers
{
    /// <summary>
    /// Represents an answer to an open question in the test.
    /// </summary>
    public class OpenAnswer : Answer
    {
        /// <summary>
        /// Gets or sets the response text of the open answer.
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
