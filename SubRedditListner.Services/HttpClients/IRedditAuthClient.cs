using SubRedditListner.Services.Models;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    /// <summary>
    /// Represents a client for retrieving Reddit authentication tokens.
    /// </summary>
    public interface IRedditAuthClient
    {
        /// <summary>
        /// Retrieves a Reddit authentication token asynchronously.
        /// </summary>
        /// <returns>
        /// Retrives Reddit auth token.
        /// </returns>
        Task<RedditAuthResponse?> RetrieveToken();
    }
}
