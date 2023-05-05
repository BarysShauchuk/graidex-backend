using Graidex.Application.OneOfCustomTypes;

namespace Graidex.Application.Services.Users
{
    public interface ICurrentUserService
    {
        public string GetEmail();
        public UserNotFound UserNotFound(string role = "User");
        public string GetRole();
    }
}