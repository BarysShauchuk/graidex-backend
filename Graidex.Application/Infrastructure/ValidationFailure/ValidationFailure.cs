using Graidex.Application.Infrastructure.ResultObjects.Generic;
using Graidex.Application.Infrastructure.ResultObjects.NonGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Infrastructure.ValidationFailure
{
    /// <summary>
    /// Validation failure result.
    /// </summary>
    /// <typeparam name="T">Type of the result.</typeparam>
    public class ValidationFailure<T> : Failure<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFailure{T}"/> class.
        /// </summary>
        /// <param name="justification">Justification of the failure.</param>
        /// <param name="errors">List of validation errors.</param>
        public ValidationFailure(
            string justification,
            List<FluentValidation.Results.ValidationFailure> errors)
            : base(justification)
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets or sets the list of validation errors.
        /// </summary>
        public List<FluentValidation.Results.ValidationFailure> Errors { get; set; }
    }

    /// <summary>
    /// Validation failure result.
    /// </summary>
    public class ValidationFailure : Failure
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFailure"/> class.
        /// </summary>
        /// <param name="justification">Justification of the failure.</param>
        /// <param name="errors">List of validation errors.</param>
        public ValidationFailure(
            string justification,
            List<FluentValidation.Results.ValidationFailure> errors)
            : base(justification)
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets or sets the list of validation errors.
        /// </summary>
        public List<FluentValidation.Results.ValidationFailure> Errors { get; set; }
    }
}
