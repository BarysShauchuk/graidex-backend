using Graidex.Application.OneOfCustomTypes;

namespace Graidex.Application.Interfaces
{
    public interface ICurrentUserService
    {
        public string GetEmail();
        public UserNotFound UserNotFound(string role = "User");
        public string GetRole();
    }
}