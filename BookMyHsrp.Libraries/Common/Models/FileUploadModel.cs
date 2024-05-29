using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Common.Models
{
    public class FileUploadModel
    {
        public IFormFile RcPhoto { get; set; }
    }
}
