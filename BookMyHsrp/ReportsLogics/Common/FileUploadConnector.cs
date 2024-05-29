using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace BookMyHsrp.ReportsLogics.Common
{
    public class FileUploadConnector 
    {
//        private readonly string pathDrive;
//        private readonly IWebHostEnvironment _hostingEnvironment;

//        public FileUploadConnector(IOptionsSnapshot<DynamicDataDto> dynamicdto , IWebHostEnvironment hostingEnvironment)
//        {
//            pathDrive = dynamicdto.Value.RCFilePath;
//            _hostingEnvironment = hostingEnvironment;
//        }
//        public async Task<dynamic> FileUpload(dynamic fileUploadModels)
//        {
//            var data = FileUploadAllType(fileUploadModels);
//            return data;
//        }
//       public async  FileUploadAllType(dynamic fileUploadModels)
//        {
//            var path = "E:\\pdf\\";
//            foreach (IFormFile source in fileUploadModels)
//            {
//                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString();

//                filename = this.EnsureCorrectFilename(filename);

//                using (FileStream output = System.IO.File.Create(Path.Combine(path,filename)))
//                    await source.CopyToAsync(output);
//            }
            
//        }

//        private string EnsureCorrectFilename(string filename)
//        {
//            if (filename.Contains("\\"))
//                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

//            return filename;
}

      
                   
    }

