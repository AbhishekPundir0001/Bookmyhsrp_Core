using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ExceptionHandling.Model
{
    public class ExceptionHandling
    {
        public class ExceptionLog
        {
            public string Message { get; set; }
            public string StackTrace { get; set; }
            public string Type { get; set; }
            public string Title { get; set; }
            public string Detail { get; set; }
            public string Instance { get; set; }
            public string StatusText { get; set; }
            public bool IsError { get; set; }
            public int Status { get; set; }

        }
    }
}
