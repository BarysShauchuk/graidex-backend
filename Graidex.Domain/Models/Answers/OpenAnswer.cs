using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{
    public class OpenAnswer : Answer
    {
        public required virtual OpenQuestion Question { get; set; }

        public required string Answer { get; set; }
    }
}
