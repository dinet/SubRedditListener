using Microsoft.Extensions.Logging;
using SubRedditListner.DataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SubredditRepository : ISubredditRepository

{
    private readonly ConcurrentDictionary<string, SubRedditPost> posts = new ConcurrentDictionary<string, SubRedditPost>();

    public ILogger<SubredditRepository> _logger { get; }

    public SubredditRepository(ILogger<SubredditRepository> logger)
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


    public async Task<IList<string>> GetPostsWithMostUpvotesAsync(int resultCount)
    {
        try
        {
            return
              await Task.Run(() => posts?.Values?.OrderByDescending(i => i.Upvotes)
              .Take(resultCount)
              .Select(i => i.Title)
              .ToList() ?? new List<string>());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred in {nameof(GetPostsWithMostUpvotesAsync)}: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return new List<string>();
        }
    }

    public async Task<IList<string>> GetUsersWithMostPostsAsync(int resultCount)
    {
        try
        {
            return
            await Task.Run(() => posts?.Values?.GroupBy(i => i.UserId)
            .OrderByDescending(q => q.Count())
            .Take(resultCount)
            .Select(i => i.Key)
            .ToList() ?? new List<string>());

        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred in {nameof(GetUsersWithMostPostsAsync)}: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return new List<string>();
        }

    }

    public async Task<string> GetLatestPostAsync()
    {
        try
        {
            return await Task.Run(() => posts.Values.OrderByDescending(i => i.Created).FirstOrDefault()?.Title ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred in {nameof(GetLatestPostAsync)}: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return string.Empty;
        }
    }
}
