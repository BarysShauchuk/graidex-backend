using Graidex.Application.Infrastructure.ResultObjects.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Infrastructure.ResultObjects.NonGeneric
{
    /// <summary>
    /// Base class for all result objects.
    /// </summary>
    public abstract class Result
    {
        /// <summary>
        /// Checks whether this is <see cref="Success"/> result.
        /// </summary>
        /// <returns><see langword="true"/> if this is <see cref="Success{TValue}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsSuccess()
        {
            return this is Success;
        }

        /// <summary>
        /// Checks whether this is <see cref="Failure"/> result.
        /// </summary>
        /// <param name="failure">Output parameter to hold the <see cref="Failure{TValue}"/> object, if available.</param>
        /// <returns><see langword="true"/> if this is <see cref="Failure{TValue}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsFailure([NotNullWhen(true)] out Failure? failure)
        {
            if (this is Failure failureResult)
            {
                failure = failureResult;
                return true;
            }

            failure = null;
            return false;
        }

        /// <summary>
        /// Checks whether this is <see cref="Failure"/> result.
        /// </summary>
        /// <returns><see langword="true"/> if this is <see cref="Failure{TValue}"/> result, <see langword="false"/> otherwise.</returns>
        public bool IsFailure()
        {
            return this is Failure;
        }
    }
}
