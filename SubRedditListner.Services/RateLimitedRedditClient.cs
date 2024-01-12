using Microsoft.Extensions.Logging;
using SubRedditListner.DataAccess;
using SubRedditListner.Services.Models;
using System;
using System.Diagnostics;
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

        public async Task SendAsync(string url)
        {
            while (true)
            {
                try
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var response = await _redditPostClient.GetAsync(url);

                    InsertToDatabase(response);
                    stopWatch.Stop();
                    // Calculate interval and log rate limit information

                    int interval = CalculateNextInterval(response, stopWatch.ElapsedMilliseconds);
                    // Delay before making the next request
                    await Task.Delay(interval);
                }
                catch (Exception ex)
                {
                    // Log any exceptions
                    _logger.LogError($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
                }
            }
        }

        private int CalculateNextInterval(RedditGetResponse response, long elapsedMilliseconds)
        {
            //Substract time taken for the request and database operations to get a better interval estimation.
            int interval = Math.Max(0, response.GetIntervalInMiliSeconds() - (int)elapsedMilliseconds);
            _logger.LogInformation($"elapsedms: {elapsedMilliseconds}, Interval: {interval}, RateLimitReset: {response?.Header?.RateLimitReset}, RateLimitRemaining: {response?.Header?.RateLimitRemaining}");
            return interval;

        }

        private void InsertToDatabase(RedditGetResponse response)
        {
            Parallel.ForEach(response?.Content?.data?.children, child =>
            {
                //Insert/update items if either these conditions are met.
                //1.Item already exists in the database.
                //2.item Created is greater than application start time(2 mins buffer added as new posts are approximately 2 mins delayed from create time)
                if (child?.data != null && (_subredditRepository.ItemExists(child.data?.id) ||
                        DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(child?.data?.created_utc) + 360).LocalDateTime >= Process.GetCurrentProcess().StartTime))
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
