using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using SubRedditListner.DataAccess;
using Xunit;

namespace SubRedditListner.UnitTests.SubRedditListner.DataAccess
{
    public class SubRedditRepositoryTests
    {
        #region AddOrUpdateItemAsync
        [Fact]
        public async Task AddOrUpdateItemAsync_ShouldAddNewItem()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var post = new SubRedditPost { Id = "1", Title = "Test Post", Upvotes = 10 };

            // Act
            await repository.AddOrUpdateItemAsync(post);

            // Assert
            (await repository.ItemExistsAsync("1")).ShouldBeTrue();
        }
        #endregion

        #region GetItemAsync

        [Fact]
        public async Task GetItemAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var post = new SubRedditPost { Id = "1", Title = "Test Post", Upvotes = 10 };
            await repository.AddOrUpdateItemAsync(post);

            // Act
            var retrievedPost = await repository.GetItemAsync("1");

            // Assert
            post.ShouldBeEquivalentTo(retrievedPost);
        }
        #endregion

        #region GetAllItemsAsync

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnAllItems()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var post1 = new SubRedditPost { Id = "1", Title = "Test Post 1", Upvotes = 10 };
            var post2 = new SubRedditPost { Id = "2", Title = "Test Post 2", Upvotes = 20 };
            await repository.AddOrUpdateItemAsync(post1);
            await repository.AddOrUpdateItemAsync(post2);

            // Act
            var allPosts = await repository.GetAllItemsAsync();

            // Assert
            allPosts.Count().ShouldBe(2);
        }
        #endregion

        #region ItemExistsAsync

        [Fact]
        public async Task ItemExistsAsync_ShouldReturnCorrectResults()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var post1 = new SubRedditPost { Id = "1", Title = "Test Post 1", Upvotes = 10 };
            var post2 = new SubRedditPost { Id = "2", Title = "Test Post 2", Upvotes = 20 };
            await repository.AddOrUpdateItemAsync(post1);
            await repository.AddOrUpdateItemAsync(post2);

            // Act
            var result = await repository.ItemExistsAsync("1");

            // Assert
            result.ShouldBe(true);
        }
        #endregion

        #region GetPostsWithMostUpvotesAsync
        [Fact]
        public async Task GetPostsWithMostUpvotesAsync_ShouldReturnPostsInDescendingOrder()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var posts = new Dictionary<string, SubRedditPost>
        {
            { "1", new SubRedditPost { Id = "1", Title = "Post 1", Upvotes = 10 } },
            { "2", new SubRedditPost { Id = "2", Title = "Post 2", Upvotes = 20 } },
            { "3", new SubRedditPost { Id = "3", Title = "Post 3", Upvotes = 15 } }
        };

            foreach (var post in posts.Values)
            {
                await repository.AddOrUpdateItemAsync(post);
            }

            // Act
            var result = await repository.GetPostsWithMostUpvotesAsync(2);

            // Assert
            result.ShouldBe(new List<string> { "Post 2", "Post 3" });
        }

        [Fact]
        public async Task GetPostsWithMostUpvotesAsync_ShouldReturnEmptyListIfNoPosts()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);

            // Act
            var result = await repository.GetPostsWithMostUpvotesAsync(2);

            // Assert
            result.ShouldBeEmpty();
        }
        #endregion

        #region GetUsersWithMostPostsAsync
        [Fact]
        public async Task GetUsersWithMostPostsAsync_ShouldReturnUsersInDescendingOrder()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var posts = new List<SubRedditPost>
        {
            new() { Id = "1", UserId = "user1" },
            new() { Id = "2", UserId = "user1" },
            new() { Id = "3", UserId = "user2" },
            new() { Id = "4", UserId = "user2" },
            new() { Id = "5", UserId = "user2" },
            new() { Id = "6", UserId = "user3" },
        };

            foreach (var post in posts)
            {
                await repository.AddOrUpdateItemAsync(post);
            }

            // Act
            var result = await repository.GetUsersWithMostPostsAsync(2);

            // Assert
            result.ShouldBe(new List<string> { "user2", "user1" });
        }
        #endregion

        #region   GetLatestPostAsync

        [Fact]
        public async Task GetLatestPostAsync_ShouldReturnLatestPostTitle()
        {
            // Arrange
            var logger = Substitute.For<ILogger<SubredditRepository>>();
            var repository = new SubredditRepository(logger);
            var posts = new List<SubRedditPost>
        {
            new() { Id = "1", Title = "Post 1", Created = DateTime.Now },
            new() { Id = "2", Title = "Post 2", Created = DateTime.Now.AddMinutes(5) },
            new() { Id = "3", Title = "Post 3", Created = DateTime.Now.AddMinutes(10) },
        };

            foreach (var post in posts)
            {
                await repository.AddOrUpdateItemAsync(post);
            }

            // Act
            var result = await repository.GetLatestPostAsync();

            // Assert
            result.ShouldBe("Post 3");
        }
        #endregion
    }
}
