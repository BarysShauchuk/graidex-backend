﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models.Tests.ChoiceOptions;

namespace Graidex.Domain.Models.Tests.Questions
{
    /// <summary>
    /// Represents a multiple-chioce question.
    /// </summary>
    public class MultipleChoiceQuestion : Question
    {
        /// <summary>
        /// Gets or sets the collection of choice options for the question.
        /// </summary>
        public virtual List<MultipleChoiceOption> Options { get; set; } = new();

        /// <summary>
        /// Gets or sets the number of points awarded per correct option.
        /// </summary>
        public int PointsPerCorrectAnswer { get; set; }

        /// <summary>
        /// Gets the maximum number of points that can be awarded for the question.
        /// </summary>
        public override int MaxPoints => Options.Count(x => x.IsCorrect) * PointsPerCorrectAnswer;
    }
}
