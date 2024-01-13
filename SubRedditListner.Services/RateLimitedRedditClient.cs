using Microsoft.Extensions.Logging;
using SubRedditListner.DataAccess;
using SubRedditListner.Services.Models;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    public class RateLimitedHttpClient : IRateLimitedHttpClient
    {
        private readonly IRedditGetClient _redditGetClient;
        private readonly ISubredditRepository _subredditRepository;
        private readonly ILogger<RateLimitedHttpClient> _logger;

        public RateLimitedHttpClient(IRedditGetClient redditPostClient, ISubredditRepository subredditRepository, ILogger<RateLimitedHttpClient> logger)
        {
            _redditGetClient = redditPostClient;
            _subredditRepository = subredditRepository;
            _logger = logger;
        }

        public async Task SendAsync(string url, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var response = await _redditGetClient.GetAsync(url);

                    await InsertToDatabase(response);
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
            // If RateLimitRemaining is 0, wait for the next reset time.
            if (response?.Header?.RateLimitRemaining == 0) return Convert.ToInt32(response?.Header?.RateLimitReset * 1000);

            //Substract time taken for the requestt time and database operations to get a better interval estimate.
            int interval = Math.Max(0, (Convert.ToInt32((response?.Header?.RateLimitReset * 1000 / response?.Header?.RateLimitRemaining) ?? 1000) - (int)elapsedMilliseconds));
            _logger.LogInformation($"Next request will be sent after {interval} ms , Processing time: {elapsedMilliseconds}, RateLimitReset: {response?.Header?.RateLimitReset}, RateLimitRemaining: {response?.Header?.RateLimitRemaining}");
            return interval;

        }

        private async Task InsertToDatabase(RedditGetResponse response)
        {
            Parallel.ForEach(response?.Content?.data?.children, async child =>
            {
                //Insert/update items if either of these conditions are met.
                //1.Item already exists in the database.(For updating upvotes and other post stats)
                //2.item Created time is greater than application start time; aka new items(2 mins buffer added as new posts are returned from the api approximately 2 mins after create time)
                if (child?.data != null && (await _subredditRepository.ItemExistsAsync(child.data?.id) ||
                        DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(child?.data?.created_utc) + 360).LocalDateTime >= Process.GetCurrentProcess().StartTime))
                {
                    await _subredditRepository.AddOrUpdateItemAsync(new SubRedditPost()
                    {
                        Id = child.data.id,
                        Created = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(child?.data?.created_utc)).LocalDateTime,
                        Title = child.data.title,
                        Upvotes = child.data.ups,
                        UserId = child.data.author
                    });
                }
            });
        }
    }
}
