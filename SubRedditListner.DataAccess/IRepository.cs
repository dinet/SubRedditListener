using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubRedditListner.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Retrieves all SubRedditPost items asynchronously from the repository.
        /// </summary>
        /// <returns>A list of SubRedditPost.</returns>
        Task<IList<TEntity>> GetAllItemsAsync();

        /// <summary>
        /// Retrieves a SubRedditPost by its unique identifier asynchronously from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the SubRedditPost.</param>
        /// <returns>A SubRedditPost Post.</returns>
        Task<TEntity> GetItemAsync(string id);

        /// <summary>
        /// Checks if a SubRedditPost with the specified ID exists asynchronously in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the SubRedditPost.</param>
        /// <returns>A  boolean indicating whether the item exists.</returns>
        Task<bool> ItemExistsAsync(string id);
    }
}
