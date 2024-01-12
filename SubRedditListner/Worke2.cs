using SubRedditListner.Services;
using System.Threading;

namespace SubRedditListner
{
    public class Worker2 : BackgroundService
    {
        private readonly ILogger<Worker2> _logger;
        private readonly IRateLimitedHttpClient _rateLimitedHttpClient;

        public Worker2(ILogger<Worker2> logger, IRateLimitedHttpClient rateLimitedHttpClient)
        {
            _logger = logger;
            _rateLimitedHttpClient = rateLimitedHttpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
                await _rateLimitedHttpClient.SendAsync();
            //}
        }
    }
}
