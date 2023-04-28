using Graidex.Application.Infrastructure.ResultObjects.NonGeneric;
using Graidex.Application.Infrastructure.ResultObjects.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Graidex.Application.Infrastructure.ValidationFailure
{
    /// <summary>
    /// Static class for <see cref="ValidationFailure"/> extensions.
    /// </summary>
    public static class ValidationFailureExtensions
    {
        /// <summary>
        /// Creates a new instance of <see cref="ValidationFailure{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value object in case of result success.</typeparam>
        /// <param name="resultFactory"><see cref="ResultFactory{T}"> object used to create the result.</param>
        /// <param name="errors">List of validation errors.</param>
        /// <param name="justification">Justification of the failure.</param>
        /// <returns>A new instance of <see cref="ValidationFailure{T}"/>.</returns>
        public static Result<T> ValidationFailure<T>(
            this ResultFactory<T> resultFactory,
            List<FluentValidation.Results.ValidationFailure> errors,
            string justification = "Validation failed")
        {
            return new ValidationFailure<T>(justification, errors);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationFailure"/>.
        /// </summary>
        /// <param name="resultFactory"><see cref="ResultFactory"> object used to create the result.</param>
        /// <param name="errors">List of validation errors.</param>
        /// <param name="justification">Justification of the failure.</param>
        /// <returns>A new instance of <see cref="ValidationFailure"/>.</returns>
        public static Result ValidationFailure(
            this ResultFactory resultFactory,
            List<FluentValidation.Results.ValidationFailure> errors,
            string justification = "Validation failed")
        {
            return new ValidationFailure(justification, errors);
        }

        /// <summary>
        /// Checks if the result is a <see cref="ValidationFailure{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value object in case of result success.</typeparam>
        /// <param name="result">Result to check.</param>
        /// <param name="failure">Validation failure result.</param>
        /// <returns><see langword="true"/> if this is <see cref="ValidationFailure{T}"/> result, <see langword="false"/> otherwise.</returns>
        public static bool IsValidationFailure<T>(
            this Result<T> result,
            [NotNullWhen(true)] out ValidationFailure<T>? failure)
        {
            if (result is ValidationFailure<T> failureResult)
            {
                failure = failureResult;
                return true;
            }

            failure = null;
            return false;
        }

        /// <summary>
        /// Checks if the result is a <see cref="ValidationFailure{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value object in case of result success.</typeparam>
        /// <param name="result">Result to check.</param>
        /// <returns><see langword="true"/> if this is <see cref="ValidationFailure{T}"/> result, <see langword="false"/> otherwise.</returns>
        public static bool IsValidationFailure<T>(this Result<T> result)
        {
            return result is ValidationFailure<T>;
        }

        /// <summary>
        /// Checks if the result is a <see cref="ValidationFailure"/>.
        /// </summary>
        /// <param name="result">Result to check.</param>
        /// <param name="failure">Validation failure result.</param>
        /// <returns><see langword="true"/> if this is <see cref="ValidationFailure"/> result, <see langword="false"/> otherwise.</returns>
        public static bool IsValidationFailure(
            this Result result,
            [NotNullWhen(true)] out ValidationFailure? failure)
        {
            if (result is ValidationFailure failureResult)
            {
                failure = failureResult;
                return true;
            }

            failure = null;
            return false;
        }

        /// <summary>
        /// Checks if the result is a <see cref="ValidationFailure"/>.
        /// </summary>
        /// <param name="result">Result to check.</param>
        /// <returns><see langword="true"/> if this is <see cref="ValidationFailure"/> result, <see langword="false"/> otherwise.</returns>
        public static bool IsValidationFailure(this Result result)
        {
            return result is ValidationFailure;
        }
    }
}
