using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SubRedditListner.UnitTests.SubRedditListner.DataAccess
{
    public class SubRedditRepositoryTests
    {
        [Fact(Skip = "To Be implemented")]
        public async Task GetUsersWithMostPostsAsync_ReturnsTopUsers()
        {
            //// Arrange
            var posts = Fixtures.SampleRedditGetResponse;

            var logger = Substitute.For<ILogger<SubredditRepository>>;
        }
    }
}
