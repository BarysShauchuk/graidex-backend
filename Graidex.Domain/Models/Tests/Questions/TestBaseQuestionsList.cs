using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Questions
{
    public class TestBaseQuestionsList
    {
        public string _id => TestBaseId.ToString();

        public int TestBaseId { get; set; }

        public List<Question> Questions { get; set; } = new();
    }
}
