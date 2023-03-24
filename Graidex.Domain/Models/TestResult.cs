using Graidex.Domain.Models.Answers;
using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models
{
    public class TestResult
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public required virtual Test Test { get; set; }

        public required virtual Student Student { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}