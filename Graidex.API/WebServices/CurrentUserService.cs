using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;

namespace Graidex.API.WebServices
{
    /// <summary>
    /// Represents a service for retrieving information about the currently authenticated user.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public string GetEmail()
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new HttpRequestException();
            }

            var user = httpContext.User;

            var identity = user.Identity;
            if (identity is null)
            {
                throw new HttpRequestException();
            }

            var email = identity.Name;
            if (email is null)
            {
                throw new HttpRequestException();
            }

            return email;
        }

        /// <inheritdoc/>
        public UserNotFound UserNotFound(string role = "User")
        {
            string email = this.GetEmail();
            return new UserNotFound($"{role} with email \"{email}\" is not found.");
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetRoles()
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new HttpRequestException();
            }

            var roleClaims = httpContext.User.FindAll(ClaimTypes.Role);
            if (roleClaims.IsNullOrEmpty()) 
            {
                throw new HttpRequestException();
            }
            
            return roleClaims.Select(roleClaim => roleClaim.Value);
        }
    }
}
