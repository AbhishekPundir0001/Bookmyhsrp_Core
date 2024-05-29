using Azure;
using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Common.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    [Route("api/v1")]
    public class FileUploadController : Controller
    {
        private readonly FileUploadConnector _fileUploadconnector;
        private readonly string path;
        public FileUploadController(FileUploadConnector fileUploadconnector, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _fileUploadconnector = fileUploadconnector;
            path = dynamicData.Value.RCFilePath;
        }
        //[Route("upload")]
        //public async Task<dynamic> FileUpload([FromForm] FileUploadModel fileUploadModel )
        //{
        //    var data = await _fileUploadconnector.FileUpload(fileUploadModel);
        //    return data;
            
        //}
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadModel fileUploadModel)
        {
            var jsonSerializer = "";
            var response = new ResponseDto();
            var file = fileUploadModel.RcPhoto;  // Get the uploaded file
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(@"D:\PDF", file.FileName); // Set the file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    
                }
                //jsonSerializer = System.Text.Json.JsonSerializer.Serialize(response);
                response.Message = "Success";
                return Ok(response);
                
            }
            else
            {
                return BadRequest("No file uploaded");
            }
            
            
        }
        //public async void FileUploadAllType([FromForm] FileUploadModel fileUploadModel)
        //{
        //    var VehicleRegNo = string.Empty;

        //    var getSession = new RootDto();
        //    if (HttpContext.Session.GetString("UserDetail") != null)
        //    {
        //     var data = HttpContext.Session.GetString("UserSession");
        //      var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(data);

        //        VehicleRegNo = vehicledetails.VehicleRegNo;
        //    }
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    var files = fileUploadModel.RcPhoto.FileName;
        //    //foreach (string str in files)
        //    //{
        //        //HttpPostedFileBase file = Request.Files[str] as HttpPostedFileBase;
        //        ////Checking file is available to save.  
        //        //if (file != null)
        //        //{
        //        //    var InputFileName = Path.GetFileName(file.FileName);
        //        //    var ServerSavePath = Path.Combine(fileuploadPath + InputFileName);
        //        //    //Save file to server folder  
        //        //    file.SaveAs(ServerSavePath);

        //        //}

        //    //}
        //    //var path = "E:\\pdf\\";
        //    //var ca = fileUploadModel.RcPhoto;

        //    //    string filename = ContentDispositionHeaderValue.Parse(data.ContentDisposition).FileName.ToString();

        //    //    filename = this.EnsureCorrectFilename(filename);

        //    //    using (FileStream output = System.IO.File.Create(Path.Combine(path, filename)))
        //    //        await data.CopyToAsync(output);


        //}
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

    }
}
