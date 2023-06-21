using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Interfaces
{
    /// <summary>
    /// Interface to retrieve information about the route data.
    /// </summary>
    public interface IRouteDataService
    {
        /// <summary>
        /// Gets the route data as an immutable dictionary.
        /// </summary>
        public ImmutableDictionary<string, object?> RouteValues { get; }
    }
}
