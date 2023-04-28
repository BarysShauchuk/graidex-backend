using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Infrastructure.ResultObjects.Generic
{
    /// <summary>
    /// Base class for all result objects.
    /// </summary>
    /// <typeparam name="T">Type of the value object in case of result success.</typeparam>
    public abstract class Result<T>
    {
        /// <summary>
        /// Checks whether this is <see cref="Success{T}"/> result.
        /// </summary>
        /// <param name="success">Output parameter to hold the <see cref="Success{T}"/> object, if available.</param>
        /// <returns><see langword="true"/> if this is <see cref="Success{T}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsSuccess([NotNullWhen(true)] out Success<T>? success)
        {
            if (this is Success<T> successResult)
            {
                success = successResult;
                return true;
            }

            success = null;
            return false;
        }

        /// <summary>
        /// Checks whether this is <see cref="Success{T}"/> result.
        /// </summary>
        /// <returns><see langword="true"/> if this is <see cref="Success{T}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsSuccess()
        {
            return this is Success<T>;
        }

        /// <summary>
        /// Checks whether this is <see cref="Failure{T}"/> result.
        /// </summary>
        /// <param name="failure">Output parameter to hold the <see cref="Failure{T}"/> object, if available.</param>
        /// <returns><see langword="true"/> if this is <see cref="Failure{T}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsFailure([NotNullWhen(true)] out Failure<T>? failure)
        {
            if (this is Failure<T> failureResult)
            {
                failure = failureResult;
                return true;
            }

            failure = null;
            return false;
        }

        /// <summary>
        /// Checks whether this is <see cref="Failure{T}"/> result.
        /// </summary>
        /// <returns><see langword="true"/> if this is <see cref="Failure{T}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsFailure()
        {
            return this is Failure<T>;
        }
    }
}
