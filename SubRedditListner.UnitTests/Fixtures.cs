using SubRedditListner.Services.Models;
using static SubRedditListner.Services.Models.RedditGetResponse;

namespace SubRedditListner.UnitTests
{
    public static class Fixtures
    {
        public static readonly RedditGetResponse SampleRedditGetResponse = new RedditGetResponse
        {
            Header = new RedditGetResponseHeader
            {
                RateLimitRemaining = 10,
                RateLimitUsed = 5,
                RateLimitReset = 500
            },
            Content = new RedditGetResponseContent
            {
                data = new Data
                {
                    children = new List<Child>
                        {
                            new Child
                            {
                                data = new ChildData
                                {
                                    id = "123456",
                                    ups = 100,
                                    author = "sample_author1",
                                    title = "Sample Title 1",
                                    created_utc = (float)DateTimeOffset.UtcNow.AddMinutes(-30).ToUnixTimeSeconds()
                                }
                            },
                            new Child
                            {
                                data = new ChildData
                                {
                                    id = "789012",
                                    ups = 50,
                                    author = "sample_author2",
                                    title = "Sample Title 2",
                                    created_utc = (float)DateTimeOffset.UtcNow.AddHours(-2).ToUnixTimeSeconds()
                                }
                            }
                        }
                }
            }

        };

        public static readonly RedditAuthResponse SampleAuthResponse = new RedditAuthResponse
        {
            access_token = "token",
            token_type = "bearer",
            scope = "*",
            expires_in = 1234,
        };
    }
}
