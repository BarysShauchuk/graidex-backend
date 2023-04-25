namespace Graidex.Application.Infrastructure.ResultObjects.Generic
{
    /// <summary>
    /// Factory for creating Result objects.
    /// </summary>
    /// <typeparam name="T">Type of the value object of the result.</typeparam>
    public class ResultFactory<T>
    {
        /// <summary>
        /// Creates new instance of <see cref="Success{T}"/> object.
        /// </summary>
        /// <param name="value">Value object of the result.</param>
        /// <returns>New instance of <see cref="Success{T}"/> object.</returns>
        public Result<T> Success(T value)
        {
            return new Success<T>(value);
        }

        /// <summary>
        /// Creates new instance of <see cref="Failure{T}"/> object.
        /// </summary>
        /// <param name="justification">Justification of the failure.</param>
        /// <returns>New instance of <see cref="Failure{T}"/> object.</returns>
        public Result<T> Failure(string justification = "")
        {
            return new Failure<T>(justification);
        }
    }
}
