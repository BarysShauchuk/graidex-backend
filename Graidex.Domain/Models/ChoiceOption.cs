using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models
{
    public class ChoiceOption
    {
        public int Id { get; set; }

        public required virtual Question Question { get; set; }
    }
}
