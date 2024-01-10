using System.Net.Http;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    public interface IRateLimitedHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}