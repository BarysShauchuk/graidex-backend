using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graidex.Domain.Models
{   
    /// <summary>
    /// Represents a test in the application.
    /// </summary>
    public class Test : TestBase
    {
        /// <summary>
        /// Possible participation restriction rules for the test.
        /// </summary>
        public enum ParticipationRestriction
        {
            /// <summary> 
            /// Only the students in the restriction group can take the test. 
            /// </summary>
            Group,

            /// <summary> 
            /// All students of subject except the ones in the restriction group can take the test. 
            /// </summary>
            AllButGroup
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
        /// Gets or sets the restriction rule for the test.
        /// </summary>
        public ParticipationRestriction RestrictionRule { get; set; }

        /// <summary>
        /// Gets or sets the collection of students for the restriction group.
        /// </summary>
        public virtual ICollection<Student> RestrictionGroup { get; set; } = new List<Student>();
    }
}
