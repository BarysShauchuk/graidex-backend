namespace Graidex.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
