﻿using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models.Users
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
        [MaxLength(15)]
        public string? CustomId { get; set; }
    }
}
