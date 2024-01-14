using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SubRedditListner.DataAccess;
using SubRedditListner.Services;
using Xunit;
using SubRedditListner.Services.Services;

namespace SubRedditListner.UnitTests.SubRedditListner.Services
{
    public class SubRedditServiceTests
    {
        [Fact]
        public async Task SendAsync_Should_Complete_Successfully()
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
            await redditGetClient.Received(1).GetAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task SendAsync_OnException_Should_ThrowEx()
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

            // Act and assert 
            await Assert.ThrowsAnyAsync<Exception>(async () => await subRedditService.SendAsync("https://example.com"));
             
        }
    }

}

