namespace Graidex.Domain.Models.Users
{
    public class Teacher : User
    {
        public int Id { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
