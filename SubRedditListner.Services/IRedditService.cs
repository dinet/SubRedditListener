using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;

namespace SubRedditListner.Services
{
    public interface IRedditService
    {
        public string Post();
        public string RetrieveToken();
    }
}
