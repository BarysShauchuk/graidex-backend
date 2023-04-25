using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Infrastructure.ResultObjects.NonGeneric
{
    /// <summary>
    /// Factory for creating <see cref="Result"/> objects.
    /// </summary>
    public class ResultFactory
    {
        /// <summary>
        /// Creates new instance of <see cref="Success"/> object.
        /// </summary>
        /// <returns>New instance of <see cref="Success"/> object.</returns>
        public Result Success()
        {
            return new Success();
        }

        /// <summary>
        /// Creates new instance of <see cref="Failure"/> object.
        /// </summary>
        /// <param name="justification">Justification of the failure.</param>
        /// <returns>New instance of <see cref="Failure"/> object.</returns>
        public Result Failure(string justification = "")
        {
            return new Failure(justification);
        }
    }
}
