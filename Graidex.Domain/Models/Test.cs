using Graidex.Domain.Models.Questions;
using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models
{
    public class Test
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Title { get; set; }

        public DateTime LastUpdate { get; set; }
        public bool IsHidden { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public required virtual Subject Subject { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<TestResult> Results { get; set; } = new List<TestResult>();
    }
}
