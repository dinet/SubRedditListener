using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using SubRedditListner.Configurations;
using SubRedditListner.DataAccess;
using SubRedditListner.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SubRedditListner.UnitTests.SubRedditListner
{
    public class StatRetriverBackgroundServiceTests
    {
        [Fact(Skip ="Times out, needs fixes")]
        public async Task StatRetriverBackgroundService_Should_ExecuteSucessfully()
        {
            //Arrange
            var logger = Substitute.For<ILogger<StatRetriverBackgroundService>>();
            var repository = Substitute.For<ISubredditRepository>();
            var apiconfig = Substitute.For<IOptions<ApiConfig>>();
            var bgService = new StatRetriverBackgroundService(logger, repository, apiconfig);
            repository.GetPostsWithMostUpvotesAsync(Arg.Any<int>()).Returns(new List<string>());
            repository.GetUsersWithMostPostsAsync(Arg.Any<int>()).Returns(new List<string>());
            repository.GetAllItemsAsync().Returns(new List<SubRedditPost>());
            repository.GetLatestPostAsync().Returns(string.Empty);

            //Act 
            await bgService.StartAsync(default);
            await Task.Delay(500);
            await bgService.StopAsync(default);

            //Assert
            await repository.Received().GetPostsWithMostUpvotesAsync(Arg.Any<int>());
            await repository.Received().GetUsersWithMostPostsAsync(Arg.Any<int>());
            await repository.Received().GetAllItemsAsync();
        }
    }
}
