using SubRedditListner.DataAccess;

namespace SubRedditListner
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISubredditRepository _subredditRepository;

        public Worker(ILogger<Worker> logger, ISubredditRepository subredditRepository)
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
                    "\n -----------------------------------------------------------------------------\n"+
                    $"\n\n ------------------------- Top 10 Users with most posts ---------------- \n" +
                    $"{string.Join("\n", _subredditRepository.GetUsersWithMostPosts().ToArray())}"); 
                await Task.Delay(1000, stoppingToken);
            }
        }


    }
}
