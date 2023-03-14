namespace Graidex.Domain.Models
{
    public class Teacher : User
    {
        public int Id { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

        public Teacher(
            string email,
            string name,
            string surname,
            string password,
            int id,
            ICollection<Subject> subjects) : base(email, name, surname, password)
        {
            Id = id;
            Subjects = subjects;
        }
    }
}
