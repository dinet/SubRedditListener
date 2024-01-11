using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    public class RateLimitedHttpClient : IRateLimitedHttpClient
    {
        private readonly IRedditPostClient _redditPostClient;

        public RateLimitedHttpClient(IRedditPostClient redditPostClient)
        {
            _redditPostClient = redditPostClient;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            CancellationToken cancellationToken = default;
            int interval = 100;
            while (true)
            {
                var response = await _redditPostClient.PostAsync();
                interval = response.GetIntervalInMiliSeconds();
                Console.WriteLine($"Interval : {interval}, RateLimitReset:{response.Header.RateLimitReset}, RateLimitRemaining:{response.Header.RateLimitRemaining}");
                await Task.Delay(interval, cancellationToken);
               
            }
        }
    }
}
