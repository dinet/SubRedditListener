using Microsoft.Extensions.Logging;
using NSubstitute;
using RichardSzalay.MockHttp;
using SubRedditListner.Services;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Shouldly;

namespace SubRedditListner.UnitTests.SubRedditListner.Services
{
    public class RedditAuthClientTests
    {
        private readonly MockHttpMessageHandler _handler = new();
        private readonly RedditAuthClient _client;
        private readonly string _baseAddress = "https://www.example.com";
        private readonly IRedditAuthClient _redditAuthClient;
        private readonly ILogger<RedditAuthClient> _logger;

        public RedditAuthClientTests()
        {
            var httpClient = new HttpClient(_handler)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _logger = Substitute.For<ILogger<RedditAuthClient>>();
            _client = new RedditAuthClient(httpClient, _logger);
        }

        [Fact]
        public async Task GetAsync_Should_Complete_Successfully()
        {
            // Arrange 
            var expectedResponse = Fixtures.SampleAuthResponse;

            _handler
            .Expect(HttpMethod.Post, $"")
            .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse));

            // Act
            var actualResponse = await _client.RetrieveToken();

            // Assert 
            actualResponse.access_token.ShouldBe(expectedResponse.access_token);
            actualResponse.expires_in.ShouldBe(expectedResponse.expires_in);
        }


        [Fact]
        public async Task GetAsync_On_Exception_Should_Throw_Ex()
        {
            // Arrange 
            var expectedResponse = Fixtures.SampleAuthResponse;

            _handler
            .Expect(HttpMethod.Post, $"/this_will_throw_ex")
            .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse));

            // Act and Assert 
            await Assert.ThrowsAnyAsync<Exception>(async () => await _client.RetrieveToken());
        }

    }
}
