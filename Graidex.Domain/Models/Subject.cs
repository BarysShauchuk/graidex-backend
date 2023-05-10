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

        /// <summary>
        /// Gets or sets the custom ID for the subject.
        /// </summary>
        /// <remarks>
        /// The custom ID is used for easy search of subjects, without the need to use database id or title.
        /// </remarks>
        [MaxLength(15)]
        public required string CustomId { get; set; }

        /// <summary>
        /// Gets or sets the title for the subject.
        /// </summary>
        [MaxLength(50)]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets id of the teacher that manages the subject.                                                              
        /// </summary>
        public required int TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the collection of students that are assigned to the subject.
        /// </summary>
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        /// <summary>
        /// Gets or sets the image url for the subject.
        /// </summary>
        public string? ImageUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
