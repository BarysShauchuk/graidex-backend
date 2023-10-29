using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Answers
{
    public class TestResultAnswersList
    {
        public string _id => TestResultId.ToString();

        public int TestResultId { get; set; }

        public List<Answer> Answers { get; set; } = new();
    }
}
