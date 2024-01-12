using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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

    }
}
