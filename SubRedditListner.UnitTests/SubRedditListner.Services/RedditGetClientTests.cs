﻿using System.Net;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using global::SubRedditListner.Services;
using RichardSzalay.MockHttp;
using System.Net.Http.Json;
using NSubstitute.ExceptionExtensions;
using System.Runtime.InteropServices;


namespace SubRedditListner.UnitTests.SubRedditListner.Services
{

    public class RedditGetClientTests
    {
        private readonly MockHttpMessageHandler _handler = new();
        private readonly RedditGetClient _client;
        private readonly string _baseAddress = "https://www.example.com";
        private readonly IRedditAuthClient _redditAuthClient;
        private readonly ILogger<RedditGetClient> _logger;

        public RedditGetClientTests()
        {
            var httpClient = new HttpClient(_handler)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _redditAuthClient = Substitute.For<IRedditAuthClient>();
            _logger = Substitute.For<ILogger<RedditGetClient>>();
            _client = new RedditGetClient(httpClient, _redditAuthClient, _logger);
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            // Arrange 
            var expectedResponse = Fixtures.SampleRedditGetResponse;

            _handler
            .Expect(HttpMethod.Get, $"/test")
            .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse.Content));
            // Act
            var actualResponse = await _client.GetAsync("/test");

            // Assert 
            Assert.NotNull(actualResponse.Content);
        }


        [Fact]
        public async Task GetAsync_Exception_LogError()
        {
            // Arrange
            var expectedResponse = Fixtures.SampleRedditGetResponse;

            _handler
            .Expect(HttpMethod.Get, $"/this_will_throw_ex")
            .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse.Content));

            // Act and Assert 
            await Assert.ThrowsAnyAsync<Exception>(async () => await _client.GetAsync(""));
        }
    }

}
