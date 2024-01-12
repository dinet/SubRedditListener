using System;
using System.Net.Http;
using System.Threading.Tasks;
using SubRedditListner.Services.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using static SubRedditListner.Services.Models.RedditGetResponse;
using System.Linq;

namespace SubRedditListner.Services
{
    public class RedditPostClient : IRedditPostClient
    {
        private readonly HttpClient _httpClient;
        private readonly IRedditAuthClient _authClient;
        public RedditPostClient(HttpClient httpClient, IRedditAuthClient authClient)
        {
            _httpClient = httpClient;
            _authClient = authClient;
        }
        public async Task<RedditGetResponse> GetAsync(string url)
        {
            var response = new RedditGetResponse();
            try
            {
                //Set token if its not already set. 
                if (_httpClient.DefaultRequestHeaders.Authorization == null)
                {
                    await SetAuthTokenAsync();
                }
                var httpResponse = await _httpClient.GetAsync(url);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var jsonString = await httpResponse.Content.ReadAsStringAsync();
                    response.Content = JsonSerializer.Deserialize<RedditGetResponseContent>(jsonString);
                    response.Header = new RedditGetResponseHeader()
                    {
                        RateLimitRemaining = httpResponse.Headers.TryGetValues("x-ratelimit-remaining", out var rateLimitRemaining) ? Convert.ToDouble(rateLimitRemaining.FirstOrDefault()) : 0,
                        RateLimitReset = httpResponse.Headers.TryGetValues("x-ratelimit-reset", out var rateLimitReset) ? Convert.ToDouble(rateLimitReset.FirstOrDefault()) : 0,
                        RateLimitUsed = httpResponse.Headers.TryGetValues("x-ratelimit-used", out var rateLimitUsed) ? Convert.ToDouble(rateLimitUsed.FirstOrDefault()) : 0
                    };
                }
                else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await SetAuthTokenAsync();
                }
            }
            catch (Exception ex)
            {
                //Handle exceptions
            }
            return response;
        }

        private async Task SetAuthTokenAsync()
        {
            var tokenResponse = await _authClient.RetrieveToken();
            if (tokenResponse != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.access_token);
            }
        }
    }
}
