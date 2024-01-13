using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    /// <summary>
    /// Represents a rate-limited HTTP client for sending asynchronous HTTP requests.
    /// </summary>
    public interface IRateLimitedHttpClient
    {
        /// <summary>
        /// Sends an asynchronous HTTP request to the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the HTTP request.</param>
        /// <param name="cancellationToken">Cancellation Token the stop polling the service.</param>
        /// <returns>A Task representing the asynchronous operation.</returns> 
        Task SendAsync(string url, CancellationToken cancellationToken);
    }

}