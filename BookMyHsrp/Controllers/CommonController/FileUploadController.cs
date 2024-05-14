using BookMyHsrp.Libraries.Common.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    [Route("api/v1")]
    public class FileUploadController : Controller
    {
        private readonly FileUploadConnector _fileUploadconnector;
        public FileUploadController(FileUploadConnector fileUploadconnector)
        {
            _fileUploadconnector = fileUploadconnector;
        }
        //[Route("upload")]
        //public async Task<dynamic> FileUpload([FromForm] FileUploadModel fileUploadModel )
        //{
        //    var data = await _fileUploadconnector.FileUpload(fileUploadModel);
        //    return data;
            
        //}
        [Route("upload")]
        public async void FileUploadAllType([FromForm] FileUploadModel fileUploadModel)
        {
            var VehicleRegNo = string.Empty;

            var getSession = new RootDto();
            if (HttpContext.Session.GetString("UserDetail") != null)
            {
             var data = HttpContext.Session.GetString("UserSession");
              var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(data);

                VehicleRegNo = vehicledetails.VehicleRegNo;
            }
            //var path = "E:\\pdf\\";
            //var ca = fileUploadModel.RcPhoto;
            
            //    string filename = ContentDispositionHeaderValue.Parse(data.ContentDisposition).FileName.ToString();

            //    filename = this.EnsureCorrectFilename(filename);

            //    using (FileStream output = System.IO.File.Create(Path.Combine(path, filename)))
            //        await data.CopyToAsync(output);
            

        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

    }
}
