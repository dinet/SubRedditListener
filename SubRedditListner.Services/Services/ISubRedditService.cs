using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SubRedditListner.Services.Services
{
    public interface ISubRedditService
    {
        /// <summary>
        /// Sends an asynchronous HTTP request to the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the HTTP request.</param> 
        /// <returns>Next interval in milliseconds based on the current rate limits</returns> 
        Task<int> SendAsync(string url);
    }

}