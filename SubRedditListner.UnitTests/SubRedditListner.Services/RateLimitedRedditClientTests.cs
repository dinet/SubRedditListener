using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SubRedditListner.DataAccess;
using SubRedditListner.Services;
using SubRedditListner.Services.Models;
using Xunit;
using Shouldly;

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

            var logger = Substitute.For<ILogger<RateLimitedHttpClient>>();

            var rateLimitedHttpClient = new RateLimitedHttpClient(
                redditGetClient,
                subredditRepository,
                logger
            );
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            // Act
            await rateLimitedHttpClient.SendAsync("https://example.com", token);


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
            var logger = Substitute.For<ILogger<RateLimitedHttpClient>>();

            var rateLimitedHttpClient = new RateLimitedHttpClient(
                redditGetClient,
                subredditRepository,
                logger
            );
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            // Act 
            await rateLimitedHttpClient.SendAsync("https://example.com", token);

            //Assert
            logger.Received().LogError(Arg.Any<string>());
        }
    }

}

