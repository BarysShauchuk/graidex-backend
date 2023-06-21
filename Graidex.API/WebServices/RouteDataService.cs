using Graidex.Application.Interfaces;
using Microsoft.AspNetCore.Routing;
using System.Collections.Immutable;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace Graidex.API.WebServices
{
    /// <summary>
    /// Service to access route data.
    /// </summary>
    public class RouteDataService : IRouteDataService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteDataService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public RouteDataService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public ImmutableDictionary<string, object?> RouteValues
        {
            get
            {
                var values = this.httpContext.GetRouteData().Values;
                return values.ToImmutableDictionary();
            }
        }

        private HttpContext httpContext
        {
            get
            {
                var context = this.httpContextAccessor.HttpContext;
                if (context is null)
                {
                    throw new HttpRequestException();
                }

                return context;
            }
        }
    }
}
