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
        await Task.Run(() => posts[subRedditPost.Id] = subRedditPost);
    }

    public async Task<SubRedditPost> GetItemAsync(string id)
    {
        return await Task.Run(() => posts.TryGetValue(id, out SubRedditPost? subredditPost) ? subredditPost : new SubRedditPost());
    }

    public async Task<IList<SubRedditPost>> GetAllItemsAsync()
    {
        return await Task.Run(() => posts.Values.ToList());
    }

    public async Task<bool> ItemExistsAsync(string id)
    {
        return await Task.Run(() => posts.ContainsKey(id));
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
            _logger.LogError($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return new List<string>();
        }
    }

    public async Task<IList<string>> GetUsersWithMostPostsAsync(int resultCount)
    {
        return
            await Task.Run(() => posts?.Values?.GroupBy(i => i.UserId)
            .OrderByDescending(q => q.Count())
            .Take(resultCount)
            .Select(i => i.Key)
            .ToList() ?? new List<string>());
    }

    public async Task<string> GetLatestPostAsync()
    {
        return await Task.Run(() => posts.Values.OrderByDescending(i => i.Created).FirstOrDefault()?.Title ?? string.Empty);
    }
}
