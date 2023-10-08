using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.Questions.ChoiceOptions
{
    public class MultipleChoiceOptionDto
    {
        public required ChoiceOptionDto Option { get; set; }

        public bool IsCorrect { get; set; }
    }
}
