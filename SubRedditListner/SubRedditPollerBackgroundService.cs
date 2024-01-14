using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SubRedditListner.Configurations;
using SubRedditListner.DataAccess;
using SubRedditListner.Services;
using SubRedditListner.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubRedditListner
{
    public class SubRedditPollerBackgroundService : BackgroundService
    {
        private ILogger<SubRedditPollerBackgroundService> _logger;
        private ISubRedditService _service;
        private readonly IOptions<ApiConfig> _apiConfig;

        public SubRedditPollerBackgroundService(ILogger<SubRedditPollerBackgroundService> logger, ISubRedditService service, IOptions<ApiConfig> apiConfig)
        {
            _apiConfig = apiConfig;
            _logger = logger;
            _service = service;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    int nextInterval = await _service.SendAsync($"/r/{_apiConfig.Value.SubRedditName}/new");
                    await Task.Delay(nextInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
                }
            }

        }
    }
}
