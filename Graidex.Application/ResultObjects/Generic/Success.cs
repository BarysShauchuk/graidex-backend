namespace Graidex.Application.ResultObjects.Generic
{
    /// <summary>
    /// Success result object.
    /// </summary>
    /// <typeparam name="T">Type of the value object in case of result success.</typeparam>
    public class Success<T> : Result<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Success{T}"/> class.
        /// </summary>
        /// <param name="value">Value object of the result.</param>
        public Success(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value object of the result.
        /// </summary>
        public T Value { get; set; }
    }
}
