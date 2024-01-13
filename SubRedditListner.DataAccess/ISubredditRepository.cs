using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubRedditListner.DataAccess
{
    public interface ISubredditRepository
    {
        /// <summary>
        /// Adds or updates a SubRedditPost asynchronously in the repository.
        /// </summary>
        /// <param name="subRedditPost">The SubRedditPost to add or update.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task AddOrUpdateItemAsync(SubRedditPost subRedditPost);

        /// <summary>
        /// Retrieves all SubRedditPost items asynchronously from the repository.
        /// </summary>
        /// <returns>A list of SubRedditPost.</returns>
        Task<IList<SubRedditPost>> GetAllItemsAsync();

        /// <summary>
        /// Retrieves a SubRedditPost by its unique identifier asynchronously from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the SubRedditPost.</param>
        /// <returns>A SubRedditPost Post.</returns>
        Task<SubRedditPost> GetItemAsync(string id);

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
        /// Checks if a SubRedditPost with the specified ID exists asynchronously in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the SubRedditPost.</param>
        /// <returns>A  boolean indicating whether the item exists.</returns>
        Task<bool> ItemExistsAsync(string id);

        /// <summary>
        /// Gets the latest post asynchronously in the repository.
        /// </summary>
        /// <returns>The latest post Title  </returns>
        Task<string> GetLatestPostAsync();
    }
}
