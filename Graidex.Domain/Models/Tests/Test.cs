﻿using Graidex.Domain.Models.Tests.Questions;
using Graidex.Domain.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graidex.Domain.Models.Tests
{
    /// <summary>
    /// Represents a test in the application.
    /// </summary>
    public class Test : TestBase
    {
        public Test()
        {
            this.ItemType ??= nameof(Test);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the test is visible or hidden.
        /// </summary>
        public new bool IsVisible
        {
            get => base.IsVisible;
            set => base.IsVisible = value;
        }

        /// <summary>
        /// Gets or sets the date and time of the start of the test.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the end of the test.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time limit of the test.
        /// </summary>
        public TimeSpan TimeLimit { get; set; }

        /// <summary>
        /// Gets or sets the collection of students allowed to take the test.
        /// </summary>
        public virtual ICollection<Student> AllowedStudents { get; set; } = new List<Student>();
    }
}