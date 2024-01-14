using Microsoft.Extensions.Options;
using SubRedditListner.Configurations;
using SubRedditListner.DataAccess;

namespace SubRedditListner
{
    public class StatRetriverBackgroundService : BackgroundService
    {
        private readonly ILogger<StatRetriverBackgroundService> _logger;
        private readonly ISubredditRepository _subredditRepository;
        private readonly IOptions<ApiConfig> _apiConfig;

        public StatRetriverBackgroundService(ILogger<StatRetriverBackgroundService> logger, ISubredditRepository subredditRepository, IOptions<ApiConfig> apiConfig)
        {
            _logger = logger;
            _subredditRepository = subredditRepository;
            _apiConfig = apiConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    _logger.LogInformation(
                               $"\n\n ---------------------- Top 10 Posts with most upvotes ----------------- \n" +
                               $"{string.Join("\n", (await _subredditRepository.GetPostsWithMostUpvotesAsync(10))?.ToArray() ?? Array.Empty<string>())} \n" +
                               "\n -----------------------------------------------------------------------------\n" +
                               $"\n\n ------------------------- Top 10 Users with most posts ---------------- \n" +
                               $"{string.Join("\n", (await _subredditRepository.GetUsersWithMostPostsAsync(10))?.ToArray() ?? Array.Empty<string>())} \n " +
                                "\n -----------------------------------------------------------------------------\n" +
                               $"-------- Total Posts - {(await _subredditRepository.GetAllItemsAsync()).Count} \n" +
                               $"-------- Most Recent Post - {await _subredditRepository.GetLatestPostAsync()}\n\n"
                               );
                }
                catch (Exception ex)
                {
                    _logger.LogError("Internall Error occured");
                }
                await Task.Delay(_apiConfig.Value.StatRetrivalInterval, stoppingToken);
            }
        }
    }
}
