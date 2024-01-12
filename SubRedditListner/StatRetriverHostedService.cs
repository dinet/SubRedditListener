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
                    $"{string.Join("\n", _subredditRepository.GetPostsWithMostUpvotes().ToArray())} " +
                    "\n -----------------------------------------------------------------------------\n" +
                    $"\n\n ------------------------- Top 10 Users with most posts ---------------- \n" +
                    $"{string.Join("\n", _subredditRepository.GetUsersWithMostPosts().ToArray())} \n\n" +
                    $"-------- Total Posts - {_subredditRepository.GetAllItems().Count}"
                    );
                await Task.Delay(1000, stoppingToken);
            }
        }


    }
}
