using BookMyHsrp.Libraries.Common.Models;
using BookMyHsrp.ReportsLogics.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
            var path = "E:\\pdf\\";
            var data = fileUploadModel.RcPhoto;
            
                string filename = ContentDispositionHeaderValue.Parse(data.ContentDisposition).FileName.ToString();

                filename = this.EnsureCorrectFilename(filename);

                using (FileStream output = System.IO.File.Create(Path.Combine(path, filename)))
                    await data.CopyToAsync(output);
            

        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

    }
}
