using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubRedditListner.Services.Services
{
    public class RedditService : IRedditService
    {
        IRedditPostClient _redditPostClient;
        public RedditService(IRedditPostClient redditPostClient)
        {
            _redditPostClient = redditPostClient;
        }

        public Task TryCallRedditApiAsync()
        {
        
        }
    }
}
