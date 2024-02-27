using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.OneOfCustomTypes
{
    /// <summary>
    /// Class for specifying that a condition was failed.
    /// </summary>
    /// <param name="Comment">Comment.</param>
    public record ConditionFailed(string Comment);
}
