using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubRedditListner
{
    public class ApiConfig
    { 
        public string TokenUrl { get; set; }
        public string BaseUrl { get; set; }
        public string AgentName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubRedditName { get; set; }          
    }
}
