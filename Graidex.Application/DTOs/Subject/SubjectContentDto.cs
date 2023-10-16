using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Subject
{
    /// <summary>
    /// Represents DTO for getting the subject content.
    /// </summary>
    public class SubjectContentDto
    {
        /// <summary>
        /// Gets or sets the ID of the content.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the content.
        /// </summary>
        [MaxLength(50)]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the content.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the id of the subject.
        /// </summary>

        public int SubjectId { get; set; }


        public string? ItemType { get; set; }

        /// <summary>
        /// Gets or sets the ordering index for the subject.
        /// </summary>
        public double OrderIndex { get; set; }
    }
}
