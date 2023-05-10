﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Subject
{   
    /// <summary>
    /// Represents dto for updating a subject.
    /// </summary>
    public class UpdateSubjectDto
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
        /// Gets or sets the image URL for the subject.
        /// </summary>
        public string? ImageUrl { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set;}
    }
}
