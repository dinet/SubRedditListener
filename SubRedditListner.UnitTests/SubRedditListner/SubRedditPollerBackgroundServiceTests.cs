using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using SubRedditListner.Configurations;
using SubRedditListner.Services.Services;
using Xunit;

namespace SubRedditListner.UnitTests.SubRedditListner
{
    public class SubRedditPollerBackgroundServiceTests
    {
        [Fact]
        public async Task SubRedditPollerBackgroundService_Should_ExecuteSucessfully()
        {
            //Arrange
            var logger = Substitute.For<ILogger<SubRedditPollerBackgroundService>>();
            var redditService = Substitute.For<ISubRedditService>();
            var apiconfig = Substitute.For<IOptions<ApiConfig>>();
            var bgService = new SubRedditPollerBackgroundService(logger, redditService, apiconfig);
            redditService.SendAsync(Arg.Any<string>()).Returns(10);

            //Act 
            await bgService.StartAsync(default);
            await Task.Delay(500);
            await bgService.StopAsync(default);

            //Assert
            await redditService.Received().SendAsync(Arg.Any<string>());
        }
    }
}
