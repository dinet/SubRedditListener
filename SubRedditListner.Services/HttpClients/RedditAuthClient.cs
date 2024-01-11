using SubRedditListner.Services.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

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
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                                                    {
                                                        {"grant_type", "client_credentials" }
                                                    });

            var response = await _httpClient.PostAsync("access_token", content);
            return JsonSerializer.Deserialize<RedditAuthResponse>(await response.Content.ReadAsStringAsync());

        }
    }
}
