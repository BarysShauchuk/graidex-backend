using Graidex.Application.OneOfCustomTypes;

namespace Graidex.Application.Interfaces
{
    /// <summary>
    /// Interface to retrieve information about the currently logged in user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the email of the currently logged in user.
        /// </summary>
        /// <returns>The email of the currently logged in user.</returns>
        public string GetEmail();

        /// <summary>
        /// Creates a UserNotFound exception with the specified role.
        /// </summary>
        /// <param name="role">The role that the user was not found for.</param>
        /// <returns>A UserNotFound exception with the specified role.</returns>
        public UserNotFound UserNotFound(string role = "User");

        /// <summary>
        /// Gets the roles of the currently logged in user.
        /// </summary>
        /// <returns>An enumerable of roles for the currently logged in user.</returns>
        public IEnumerable<string> GetRoles();

        /// <summary>
        /// Gets the IP address of the user making the request.
        /// </summary>
        /// <returns>The IP address of the user making the request.</returns>
        public string? GetIpAddress();
    }
}