namespace Graidex.Application.Infrastructure.ResultObjects.Generic
{
    /// <summary>
    /// Failure result object.
    /// </summary>
    /// <typeparam name="T">Type of the value object in case of result success.</typeparam>
    public class Failure<T> : Result<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Failure{T}"/> class.
        /// </summary>
        /// <param name="justification">Justification of the failure.</param>
        public Failure(string justification)
        {
            Justification = justification;
        }

        /// <summary>
        /// Gets or sets the justification of the failure.
        /// </summary>
        public string Justification { get; set; }
    }
}
