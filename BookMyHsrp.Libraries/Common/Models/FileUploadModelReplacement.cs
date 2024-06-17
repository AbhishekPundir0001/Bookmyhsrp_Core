using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Common.Models
{
    public class FileUploadModelReplacement
    {
        public IFormFile FrontPlatePhoto { get; set; }
        public IFormFile RearPlatePhoto { get; set; }
        public IFormFile FirCopy { get; set; }
        public IFormFile RcCopy { get; set; }
        public string OrderType { get; set; }
        public string ReplacementReason { get; set; }
        public string StateId { get; set; }

    }
}
