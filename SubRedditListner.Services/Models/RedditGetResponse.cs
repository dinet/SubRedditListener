using System.Collections.Generic;

namespace SubRedditListner.Services.Models
{
    public class RedditGetResponse
    {
        public RedditGetResponseContent Content { get; set; }
        public RedditGetResponseHeader Header { get; set; }

        public class RedditGetResponseHeader
        {
            public double RateLimitRemaining { get; set; }
            public double RateLimitUsed { get; set; }
            public double RateLimitReset { get; set; }
        }
        public class RedditGetResponseContent
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public IList<Child> children { get; set; }
        }

        public class Child
        {
            public ChildData data { get; set; }
        }

        public class ChildData
        {
            public string id { get; set; }
            public int ups { get; set; }
            public string author { get; set; }
            public string title { get; set; }
            public float created_utc { get; set; }
        }
    }
}
