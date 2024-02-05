using Microsoft.Extensions.Logging;
using SubRedditListner.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SubredditRepository : Repository<SubRedditPost>, ISubredditRepository

{
    private readonly ILogger<SubredditRepository> _logger;
    public SubredditRepository(ILogger<SubredditRepository> logger) : base(logger)
    {
        _logger = logger;
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
