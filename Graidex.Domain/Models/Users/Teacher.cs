namespace Graidex.Domain.Models.Users
{   
    /// <summary>
    /// Represents a teacher who uses the application.
    /// </summary>
    public class Teacher : User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the teacher.
        /// </summary>
        public int Id { get; set; }
    }
}
