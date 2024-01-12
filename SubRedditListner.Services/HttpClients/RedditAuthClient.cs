using SubRedditListner.Services.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SubRedditListner.Services
{
    public class RedditAuthClient : IRedditAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RedditAuthClient> _logger;

        public RedditAuthClient(HttpClient httpClient, ILogger<RedditAuthClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<RedditAuthResponse?> RetrieveToken()
        {
            var authResponse = new RedditAuthResponse();
            try
            {
                // Prepare content for token request
                HttpContent content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                // Send token request to Reddit API
                var response = await _httpClient.PostAsync("access_token", content);

                // Read token and  Deserialize token response
                var tokenString = await response.Content.ReadAsStringAsync();

                authResponse = JsonSerializer.Deserialize<RedditAuthResponse>(tokenString);
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }

            return authResponse;
        }
    }
}
