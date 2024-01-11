using SubRedditListner.Services.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SubRedditListner.Services
{
    public interface IRedditAuthClient
    {
        public Task<RedditAuthResponse?> RetrieveToken();
    }
}
