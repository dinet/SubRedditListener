using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SubRedditListner.DataAccess;
using SubRedditListner.Services;
using SubRedditListner.Services.Models;
using Xunit;
using Shouldly;
using SubRedditListner.Services.Services;

namespace SubRedditListner.UnitTests.SubRedditListner.Services
{
    public class RateLimitedRedditClientTests
    {
        [Fact(Skip = "TODO fix the unit test")]
        public async Task SendAsync_Success()
        {
            // Arrange
            var redditGetClient = Substitute.For<IRedditGetClient>();
            redditGetClient.GetAsync(Arg.Any<string>())
                .Returns(Fixtures.SampleRedditGetResponse);

            var subredditRepository = Substitute.For<ISubredditRepository>();
            subredditRepository.ItemExistsAsync(Arg.Any<string>())
                .Returns(false);

            var logger = Substitute.For<ILogger<SubRedditService>>();

            var subRedditService = new SubRedditService(
                redditGetClient,
                subredditRepository,
                logger
            );
            // Act
            await subRedditService.SendAsync("https://example.com");


            //// Assert
            await redditGetClient.Received().GetAsync(Arg.Any<string>());
            await subredditRepository.Received().AddOrUpdateItemAsync(Arg.Any<SubRedditPost>());
        }

        [Fact(Skip = "TODO fix the unit test")]
        public async Task SendAsync_ExceptionHandling()
        {
            // Arrange
            var redditGetClient = Substitute.For<IRedditGetClient>();
            redditGetClient.GetAsync(Arg.Any<string>())
                .Throws(new Exception("Excetion occured"));

            var subredditRepository = Substitute.For<ISubredditRepository>();
            var logger = Substitute.For<ILogger<SubRedditService>>();

            var subRedditService = new SubRedditService(
                redditGetClient,
                subredditRepository,
                logger
            ); 

            // Act 
            await subRedditService.SendAsync("https://example.com");

            //Assert
            logger.Received().LogError(Arg.Any<string>());
        }
    }

}

