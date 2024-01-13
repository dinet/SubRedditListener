using SubRedditListner.DataAccess;

namespace SubRedditListner
{
    public class StatRetriverHostedService : BackgroundService
    {
        private readonly ILogger<StatRetriverHostedService> _logger;
        private readonly ISubredditRepository _subredditRepository;

        public StatRetriverHostedService(ILogger<StatRetriverHostedService> logger, ISubredditRepository subredditRepository)
        {
            _logger = logger;
            _subredditRepository = subredditRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation(
                    $"\n\n ---------------------- Top 10 Posts with most upvotes ----------------- \n" +
                    $"{string.Join("\n", (await _subredditRepository.GetPostsWithMostUpvotesAsync(10))?.ToArray()?? Array.Empty<string>())} \n" +
                    "\n -----------------------------------------------------------------------------\n" +
                    $"\n\n ------------------------- Top 10 Users with most posts ---------------- \n" +
                    $"{string.Join("\n", (await _subredditRepository.GetUsersWithMostPostsAsync(10))?.ToArray() ?? Array.Empty<string>())} \n " +
                     "\n -----------------------------------------------------------------------------\n" +
                    $"-------- Total Posts - {(await _subredditRepository.GetAllItemsAsync()).Count} \n" +
                    $"-------- Most Recent Post - {await _subredditRepository.GetLatestPostAsync()}\n\n"
                    );
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
