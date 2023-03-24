using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{
    public class MultipleChoiceAnswer : Answer
    {
        public required virtual MultipleChoiceQuestion Question { get; set; }

        public virtual ICollection<ChoiceOption> Answers { get; set; } = new List<ChoiceOption>();
    }
}
