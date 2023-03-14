namespace Graidex.Domain.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsHidden { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TestResult> Results { get; set; }

        public Test(
            int id,
            string title,
            DateTime lastUpdate,
            bool isHidden,
            DateTime startTime,
            DateTime endTime,
            TimeSpan timeLimit,
            Subject subject,
            ICollection<Question> questions,
            ICollection<TestResult> results)
        {
            Id = id;
            Title = title;
            LastUpdate = lastUpdate;
            IsHidden = isHidden;
            StartTime = startTime;
            EndTime = endTime;
            TimeLimit = timeLimit;
            Subject = subject;
            Questions = questions;
            Results = results;
        }
    }
}
