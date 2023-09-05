using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Questions
{
    public class TestQuestionsList
    {
        public string _id => TestId.ToString();

        public int TestId { get; set; }

        public List<Question> Questions { get; set; } = new();
    }
}
