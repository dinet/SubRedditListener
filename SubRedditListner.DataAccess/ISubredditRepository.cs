using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace SubRedditListner.DataAccess
{
    public interface ISubredditRepository
    {
        void AddOrUpdateItem(SubRedditPost subRedditPost);
        IList<SubRedditPost> GetAllItems();
        SubRedditPost GetItem(string id );
        IList<string> GetPostsWithMostUpvotes();
        IList<string> GetUsersWithMostPosts();
    }
}
