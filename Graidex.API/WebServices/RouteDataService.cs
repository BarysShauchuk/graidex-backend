using Graidex.Application.Interfaces;
using Microsoft.AspNetCore.Routing;
using System.Collections.Immutable;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace Graidex.API.WebServices
{
    public class RouteDataService : IRouteDataService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public RouteDataService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

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
