using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        public virtual ICollection<ChoiceOption> Options { get; set; } = new List<ChoiceOption>();
        public virtual ICollection<ChoiceOption> RightOptions { get; set; } = new List<ChoiceOption>();
    }
}
