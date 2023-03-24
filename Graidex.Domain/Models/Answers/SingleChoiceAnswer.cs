using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{
    public class SingleChoiceAnswer : Answer
    {
        public required virtual SingleChoiceQuestion Question { get; set; }
        public required virtual ChoiceOption Answer { get; set; }
    }
}
