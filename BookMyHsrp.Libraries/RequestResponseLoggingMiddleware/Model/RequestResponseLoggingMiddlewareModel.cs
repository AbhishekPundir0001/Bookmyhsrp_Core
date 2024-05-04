using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.RequestResponseLoggingMiddleware.Model
{
    public class RequestResponseLoggingMiddlewareModel
    {
        public class RequestResponseLog
        {
            public DateTime Timestamp { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
            public string UserAgent { get; set; }
            public string IPAddress { get; set; }
            public string RequestUrl { get; set; }

            public long TimeTakenMs { get; set; }
            // Add other fields as needed
        }
    }
}
