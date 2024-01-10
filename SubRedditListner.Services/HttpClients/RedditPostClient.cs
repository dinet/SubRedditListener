using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SubRedditListner.Services.Models;

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
        public async Task<RedditPostResponse> Post()
        {
            var response = new RedditPostResponse();
            try
            {
                if (_httpClient.DefaultRequestHeaders.Authorization == null)
                {
                    await SetAuthTokenAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private Task SetAuthTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
