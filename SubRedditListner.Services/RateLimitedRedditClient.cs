using System;
using System.Collections.Generic;
using System.Text;

namespace SubRedditListner.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public class RateLimitedHttpClient : IRateLimitedHttpClient
    {
        private readonly IRedditPostClient _redditPostClient;
        private SemaphoreSlim semaphoreSlim;

        public RateLimitedHttpClient(IRedditPostClient redditPostClient)
        {
            _redditPostClient = redditPostClient;
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                HttpResponseMessage response = await _redditPostClient.Post();

                // Update semaphore based on rate-limit headers
                UpdateSemaphoreFromHeaders(response.Headers);

                return response;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private void UpdateSemaphoreFromHeaders(HttpHeaders headers)
        {
            if (headers.TryGetValues("x-ratelimit-remaining", out var remainingValues) &&
                headers.TryGetValues("x-ratelimit-reset", out var resetValues))
            {
                if (int.TryParse(remainingValues?.FirstOrDefault(), out var remaining) &&
                    int.TryParse(resetValues?.FirstOrDefault(), out var resetTimestamp))
                {
                    // Calculate time until reset
                    var timeUntilReset = TimeSpan.FromSeconds(resetTimestamp - DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                    // Update semaphore count based on remaining requests and time until reset
                    semaphoreSlim = new SemaphoreSlim(remaining, remaining);

                    // You might also want to log or handle other aspects based on your application's needs
                    Console.WriteLine($"Updated rate limit: Remaining={remaining}, Reset in {timeUntilReset}");
                }
            }
        }
    }
}
