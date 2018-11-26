using System.Threading.Tasks;

namespace GitHub.Unity
{
    /// <summary>
    /// Provides services for logging into a GitHub server.
    /// </summary>
    interface ILoginManager
    {
        /// <summary>
        /// Attempts to log into a GitHub server with a username and password.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The logged in user.</returns>
        /// <exception cref="AuthorizationException">
        /// The login authorization failed.
        /// </exception>
        LoginResultData Login(UriString host, string username, string password);

        LoginResultData ContinueLogin(LoginResultData loginResultData, string twofacode);

        /// <summary>
        /// Logs out of GitHub server.
        /// </summary>
        /// <param name="hostAddress">The address of the server.</param>
        /// <inheritdoc/>
        ITask Logout(UriString hostAddress);

        /// <summary>
        /// Attempts to log into a GitHub server with a token.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        bool LoginWithToken(UriString host, string token);
    }
}
