using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubRedditListner.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly ConcurrentDictionary<string, TEntity> posts = new ConcurrentDictionary<string, TEntity>();

        private readonly ILogger<Repository<TEntity>> _logger;

        public Repository(ILogger<Repository<TEntity>> logger)
        {
            _logger = logger;
        }

        public async Task<TEntity> GetItemAsync(string id)
        {
            try
            {
                return await Task.Run(() => posts.TryGetValue(id, out TEntity? subredditPost) ? subredditPost : new TEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in GetItemAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new TEntity();
            }
        }

        public async Task<IList<TEntity>> GetAllItemsAsync()
        {
            try
            {
                return await Task.Run(() => posts.Values.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in GetAllItemsAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new List<TEntity>();
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
