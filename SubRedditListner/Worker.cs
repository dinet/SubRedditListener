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
                _logger.LogInformation($"Worker running at: {_subredditRepository.GetAllItems().Count()}", _subredditRepository.GetAllItems().Count());
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
