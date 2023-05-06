using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Users;
using System.Data;

namespace Graidex.API.WebServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

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

        public UserNotFound UserNotFound(string role = "User")
        {
            string email = this.GetEmail();
            return new UserNotFound($"{role} with email \"{email}\" is not found.");
        }

        public string GetRole()
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new HttpRequestException();
            }

            if (httpContext.User.IsInRole("Student"))
            {
                return "Student";
            }

            if (httpContext.User.IsInRole("Teacher"))
            {
                return "Teacher";
            }

            else
            {
                return String.Empty;
            }
        }
    }
}
