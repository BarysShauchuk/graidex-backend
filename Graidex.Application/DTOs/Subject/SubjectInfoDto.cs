using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Subject
{
    /// <summary>
    /// Represents information about subject.
    /// </summary>
    public class SubjectInfoDto
    {   
        /// <summary>
        /// Gets or sets the custom ID for the subject.
        /// </summary>
        public required string CustomId { get; set; }

        /// <summary>
        /// Gets or sets the title for the subject.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the email address of the teacher of the subject.
        /// </summary>
        public required string TeacherEmail { get; set; }

        /// <summary>
        /// Gets or sets the image URL for the subject.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
