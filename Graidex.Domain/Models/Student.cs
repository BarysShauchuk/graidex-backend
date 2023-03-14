namespace Graidex.Domain.Models
{
    public class Student : User
    {
        public int Id { get; set; }
        public int CustomId { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

        public Student(
            string email,
            string name,
            string surname,
            string password,
            int id,
            int customId,
            ICollection<Subject> subjects) : base(email, name, surname, password)
        {   
            Id = id;
            CustomId = customId;
            Subjects = subjects;
        }
    }
}
