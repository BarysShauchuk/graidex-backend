namespace Graidex.Domain.Models.Users
{
    public class Teacher : User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the teacher.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the collection of subjects that the teacher manages.
        /// </summary>
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
