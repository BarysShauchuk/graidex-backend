using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.OneOfCustomTypes
{
    /// <summary>
    /// Class specifying that the user already exists
    /// </summary>
    /// <param name="Comment">Comment</param>
    public record UserAlreadyExists(string? Comment);
}
