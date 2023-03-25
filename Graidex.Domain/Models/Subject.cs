using Graidex.Domain.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models
{   
    /// <summary>
    /// Represents a study subject.
    /// </summary>
    public class Subject
    {   
        /// <summary>
        /// Gets or sets the unique identifier for the subject.
        /// </summary>
        public int Id { get; set; }

        [MaxLength(15)]
        public required string CustomId { get; set; }

        /// <summary>
        /// Gets or sets the title for the subject.
        /// </summary>
        [MaxLength(50)]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the teacher that manages the subject.                                                              
        /// </summary>
        public required virtual Teacher Teacher { get; set; }

        /// <summary>
        /// Gets or sets the collection of students that are assigned to the subject.
        /// </summary>
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        /// <summary>
        /// Gets or sets the collection of tests created for the subject.
        /// </summary>
        public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}
