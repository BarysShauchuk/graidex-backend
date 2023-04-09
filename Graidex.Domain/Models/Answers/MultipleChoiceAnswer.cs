﻿using Graidex.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Answers
{   
    /// <summary>
    /// Represents an answer to a multiple-choice question in the test.
    /// </summary>
    public class MultipleChoiceAnswer : Answer
    {   
        /// <summary>
        /// Gets or sets the multiple-choice question this answer relates to.
        /// </summary>
        public required MultipleChoiceQuestion Question { get; set; }

        /// <summary>
        /// Gets the maximum number of points that can be awarded for this answer.
        /// </summary>
        public override int MaxPoints => Question.MaxPoints;

        /// <summary>
        /// Gets or sets the collection of choice option indexes that were selected as an answer.
        /// </summary>
        public virtual ICollection<int> ChoiceOptionIndexes { get; set; } = new List<int>();
    }
}