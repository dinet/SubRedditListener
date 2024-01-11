using System;
using System.Net.Http;
using System.Threading.Tasks;
using SubRedditListner.Services.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using static SubRedditListner.Services.Models.RedditGetResponse;

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
        public async Task<RedditGetResponse> PostAsync()
        {
            var response = new RedditGetResponse();
            try
            {
                //If token is not set, we need to set it 
                if (_httpClient.DefaultRequestHeaders.Authorization == null)
                {
                    await SetAuthTokenAsync();
                }
                var httpResponse = await _httpClient.GetAsync("/r/funny/new");
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Content = JsonSerializer.Deserialize<RedditGetResponseContent>(await httpResponse.Content.ReadAsStringAsync());
                    response.Header = new RedditGetResponseHeader()
                    {
                        RateLimitRemaining = httpResponse.Headers.TryGetValues("x-ratelimit-remaining", out var rateLimitRemaining) ? Convert.ToInt32(rateLimitRemaining) : 0,
                        RateLimitReset = httpResponse.Headers.TryGetValues("x-ratelimit-reset", out var rateLimitReset) ? Convert.ToInt32(rateLimitReset) : 0,
                        RateLimitUsed = httpResponse.Headers.TryGetValues("x-ratelimit-used", out var rateLimitUsed) ? Convert.ToInt32(rateLimitUsed) : 0
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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.Access_token);
            }
        }
    }
}
