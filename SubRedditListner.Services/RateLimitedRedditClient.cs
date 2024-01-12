using Microsoft.Extensions.Logging;
using SubRedditListner.DataAccess;
using SubRedditListner.Services.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    public class RateLimitedHttpClient : IRateLimitedHttpClient
    {
        private readonly IRedditPostClient _redditPostClient;
        private readonly ISubredditRepository _subredditRepository;
        private readonly ILogger<RateLimitedHttpClient> _logger;
        public RateLimitedHttpClient(IRedditPostClient redditPostClient, ISubredditRepository subredditRepository, ILogger<RateLimitedHttpClient> logger)
        {
            _redditPostClient = redditPostClient;
            _subredditRepository = subredditRepository;
            _logger = logger;
        }

        public async Task SendAsync()
        {

            while (true)
            {
                try
                {
                    var response = await _redditPostClient.GetAsync();
                    int interval = Math.Max(0, response.GetIntervalInMiliSeconds() - 300);
                    InsertToDatabase(response);
                    _logger.LogDebug($"Interval : {interval}, RateLimitReset:{response?.Header?.RateLimitReset}, RateLimitRemaining:{response?.Header?.RateLimitRemaining}");
                    await Task.Delay(interval);
                }
                catch (Exception)
                {
                }
            }
        }

        private void InsertToDatabase(RedditGetResponse response)
        {
            Parallel.ForEach(response?.Content?.data?.children, child =>
            {
                if (_subredditRepository.ItemExists(child.data.id) || DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(child.data.created_utc)).DateTime >= Process.GetCurrentProcess().StartTime)
                {
                    _subredditRepository.AddOrUpdateItem(new SubRedditPost()
                    {
                        Id = child.data.id,
                        Title = child.data.title,
                        Upvotes = child.data.ups,
                        UserId = child.data.author
                    });
                }
            });
        }
    }
}
