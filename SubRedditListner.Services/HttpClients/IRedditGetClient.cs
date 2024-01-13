using SubRedditListner.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    /// <summary>
    /// Represents a client for interacting with Reddit posts.
    /// </summary>
    public interface IRedditGetClient
    {
        /// <summary>
        /// Asynchronously retrieves Reddit data based on the provided URL.
        /// </summary>
        /// <param name="url">The URL of the Reddit post.</param>
        /// <returns>A Task representing the asynchronous operation, containing RedditGetResponse.</returns>
        Task<RedditGetResponse> GetAsync(string url);
    }
}
