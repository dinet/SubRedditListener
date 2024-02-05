using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubRedditListner.DataAccess
{
    public interface ISubredditRepository : IRepository<SubRedditPost>
    {

        /// <summary>
        /// Retrieves a list of post names with the most upvotes asynchronously from the repository.
        /// </summary>
        /// <param name="resultCount">The number of results to retrieve.</param>
        /// <returns>A list of post names.</returns>
        Task<IList<string>> GetPostsWithMostUpvotesAsync(int resultCount);

        /// <summary>
        /// Retrieves a list of usernames with the most posts asynchronously from the repository.
        /// </summary>
        /// <param name="resultCount">The number of results to retrieve.</param>
        /// <returns>A list of usernames.</returns>
        Task<IList<string>> GetUsersWithMostPostsAsync(int resultCount);

        /// <summary>
        /// Gets the latest post asynchronously in the repository.
        /// </summary>
        /// <returns>The latest post Title  </returns>
        Task<string> GetLatestPostAsync();
    }
}
