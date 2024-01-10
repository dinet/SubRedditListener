using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubRedditListner
{
    public class ApiSettings
    { 
        public string AccessUrl { get; set; }
        public string BaseUrl { get; set; }
        public string UserAgent { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubRedditName { get; set; }          
    }
}
