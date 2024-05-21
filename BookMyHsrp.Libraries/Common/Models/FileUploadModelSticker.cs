using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Common.Models
{
    public class FileUploadModelSticker
    {
        public IFormFile FrontLaserPhoto { get; set; }
        public IFormFile RearLaserPhoto { get; set; }
        public IFormFile FrontPlatePhoto { get; set; }
        public IFormFile RearPlatePhoto { get; set; }
    }
}
