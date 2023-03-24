namespace Graidex.Domain.Models
{
    /// <summary>
    /// Represents a student who uses the application.
    /// </summary>
    public class Student : User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the student.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the custom ID for the student.
        /// </summary>
        /// <remarks>
        /// The custom ID is used for easy search of students, without the need to use database id or name.
        /// </remarks>
        public string CustomId { get; set; }

        /// <summary>
        /// Gets or sets the collection of subjects that the student is assigned to.
        /// </summary>
        public virtual ICollection<Subject> Subjects { get; set; }

    }
}
