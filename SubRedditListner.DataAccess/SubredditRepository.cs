using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace SubRedditListner.DataAccess
{
    public class SubredditRepository : ISubredditRepository
    {
        private readonly ConcurrentDictionary<string, SubRedditPost> posts = new ConcurrentDictionary<string, SubRedditPost>();

        public void AddOrUpdateItem(SubRedditPost subRedditPost)
        {
            posts[subRedditPost.Id] = subRedditPost;
        }

        public SubRedditPost GetItem(string id)
        {
            return posts.TryGetValue(id, out var subredditPost) ? subredditPost : new SubRedditPost();
        }

        public IList<SubRedditPost> GetAllItems()
        {
            return posts.Values.ToList();
        }

        public IList<string> GetPostsWithMostUpvotes()
        {
            return posts?.Values?.OrderBy(i => i.Upvotes).Take(10).Select(i => i.Title).ToList() ?? new List<string>();
        }

        public IList<string> GetUsersWithMostPosts()
        {
            return posts?.Values?.GroupBy(i => i.UserId).OrderByDescending(q => q.Count()).Take(10).Select(i => i.Key).ToList() ?? new List<string>();
        }
    }
}
