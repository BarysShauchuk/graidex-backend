using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Interfaces
{
    public interface IContentTypeProvider
    {
        string? GetContentType(string fileName);
    }
}
