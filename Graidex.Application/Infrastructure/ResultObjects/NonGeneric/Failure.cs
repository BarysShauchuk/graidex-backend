using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Infrastructure.ResultObjects.NonGeneric
{
    /// <summary>
    /// Failure result object.
    /// </summary>
    public class Failure : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Failure"/> class.
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
