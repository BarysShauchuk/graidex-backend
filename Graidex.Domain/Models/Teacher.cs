namespace Graidex.Domain.Models
{
    public class Teacher : User
    {
        public int Id { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
