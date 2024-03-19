﻿using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using Graidex.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graidex.Domain.Models.Tests
{
    /// <summary>
    /// Represents a result of the test.
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the test result.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the test require teacher review.
        /// </summary>
        public bool RequireTeacherReview { get; set; } = true;

        /// <summary>
        /// Gets or sets the flag indicating whether the test result can be viewed by student.
        /// </summary>
        public bool ShowToStudent { get; set; }

        /// <summary>
        /// Gets or sets the time of the start of the test.
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the end of the test.
        /// </summary>
        public DateTimeOffset EndTime { get; set; }

        /// <summary>
        /// Gets or sets id of the test the result belongs to.
        /// </summary>
        public required int TestId { get; set; }

        /// <summary>
        /// Gets or sets id of the student who took the test.
        /// </summary>
        public required int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the total amount of points earned.
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// Gets or sets the grade earned.
        /// </summary>
        public int Grade { get; set; }
    }
}