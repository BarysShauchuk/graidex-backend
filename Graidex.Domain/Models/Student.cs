namespace Graidex.Domain.Models
{
    public class Student : User
    {
        public int Id { get; set; }
        public string CustomId { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

    }
}
