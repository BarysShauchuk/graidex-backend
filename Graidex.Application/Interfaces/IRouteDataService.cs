using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Interfaces
{
    public interface IRouteDataService
    {
        public ImmutableDictionary<string, object?> RouteValues { get; }
    }
}
