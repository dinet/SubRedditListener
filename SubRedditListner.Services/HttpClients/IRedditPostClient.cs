using SubRedditListner.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    public interface IRedditPostClient
    {  
        Task<RedditGetResponse> GetAsync(string url);
    }
}
