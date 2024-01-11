using SubRedditListner.Services.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;

namespace SubRedditListner.Services
{
    public class RedditAuthClient : IRedditAuthClient
    {
        private readonly HttpClient _httpClient;
        public RedditAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RedditAuthResponse?> RetrieveToken()
        {
            HttpContent content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _httpClient.PostAsync("access_token", content);
            var tokenString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RedditAuthResponse>(tokenString);
        }
    }
}
