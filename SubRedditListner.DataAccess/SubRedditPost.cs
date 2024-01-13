using System;

namespace SubRedditListner.DataAccess
{
    public class SubRedditPost
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public int Upvotes { get; set; }
        public DateTime Created { get; set; }
    }
}
