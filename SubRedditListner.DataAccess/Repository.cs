using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubRedditListner.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ConcurrentDictionary<string, SubRedditPost> posts = new ConcurrentDictionary<string, SubRedditPost>();

        private readonly ILogger<SubredditRepository> _logger;

        public Repository(ILogger<SubredditRepository> logger)
        {
            _logger = logger;
        }

        public async Task AddOrUpdateItemAsync(SubRedditPost subRedditPost)
        {
            try
            {
                await Task.Run(() => posts[subRedditPost.Id] = subRedditPost);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in AddOrUpdateItemAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        public async Task<SubRedditPost> GetItemAsync(string id)
        {
            try
            {
                return await Task.Run(() => posts.TryGetValue(id, out SubRedditPost? subredditPost) ? subredditPost : new SubRedditPost());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in GetItemAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new SubRedditPost();
            }
        }

        public async Task<IList<SubRedditPost>> GetAllItemsAsync()
        {
            try
            {
                return await Task.Run(() => posts.Values.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in GetAllItemsAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new List<SubRedditPost>();
            }
        }

        public async Task<bool> ItemExistsAsync(string id)
        {
            try
            {
                return await Task.Run(() => posts.ContainsKey(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in {nameof(ItemExistsAsync)}: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
