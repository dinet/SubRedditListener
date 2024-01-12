using System;
using System.Net.Http;
using System.Threading.Tasks;
using SubRedditListner.Services.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using Microsoft.Extensions.Logging;
using static SubRedditListner.Services.Models.RedditGetResponse;
using System.Linq;

namespace SubRedditListner.Services
{
    public class RedditPostClient : IRedditPostClient
    {
        private readonly HttpClient _httpClient;
        private readonly IRedditAuthClient _authClient;
        private readonly ILogger<RedditPostClient> _logger;

        public RedditPostClient(HttpClient httpClient, IRedditAuthClient authClient, ILogger<RedditPostClient> logger)
        {
            _httpClient = httpClient;
            _authClient = authClient;
            _logger = logger;
        }

        public async Task<RedditGetResponse> GetAsync(string url)
        {
            var response = new RedditGetResponse();
            try
            {
                // Set token if it's not already set.
                if (_httpClient.DefaultRequestHeaders.Authorization == null)
                {
                    await SetAuthTokenAsync();
                }

                // Make HTTP request to Reddit API
                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    // Deserialize JSON response
                    var jsonString = await httpResponse.Content.ReadAsStringAsync();
                    response.Content = JsonSerializer.Deserialize<RedditGetResponseContent>(jsonString);

                    // Extract rate limit information from headers
                    response.Header = new RedditGetResponseHeader()
                    {
                        RateLimitRemaining = httpResponse.Headers.TryGetValues("x-ratelimit-remaining", out var rateLimitRemaining) ? Convert.ToDouble(rateLimitRemaining.FirstOrDefault()) : 0,
                        RateLimitReset = httpResponse.Headers.TryGetValues("x-ratelimit-reset", out var rateLimitReset) ? Convert.ToDouble(rateLimitReset.FirstOrDefault()) : 0,
                        RateLimitUsed = httpResponse.Headers.TryGetValues("x-ratelimit-used", out var rateLimitUsed) ? Convert.ToDouble(rateLimitUsed.FirstOrDefault()) : 0
                    };
                }
                else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Retry with a new token if unauthorized
                    await SetAuthTokenAsync();
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
            return response;
        }

        private async Task SetAuthTokenAsync()
        {
            _logger.LogInformation($"Attempting to retrieve token.");
            var tokenResponse = await _authClient.RetrieveToken();
            if (tokenResponse != null)
            {
                // Set the obtained token in the request headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.access_token);
            }
        }
    }
}
