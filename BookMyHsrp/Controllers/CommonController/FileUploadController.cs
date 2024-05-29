using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Common.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.ReplacementModel;

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
        public async Task<IActionResult> FileUploadAllType([FromForm] FileUploadModel fileUploadModel)
        {
            var VehicleRegNo = string.Empty;

            var getSession = new RootDtoSticker();
            if (HttpContext.Session.GetString("UserDetail") != null)
            {
                var data = HttpContext.Session.GetString("UserSession");
                var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(data);

                VehicleRegNo = vehicledetails.VehicleRegNo;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var response = new ResponseDto();
            var file = fileUploadModel.RcPhoto;  // Get the uploaded file
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(path, file.FileName); // Set the file path
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

        [Route("uploadSticker")]
        public async Task<IActionResult> uploadcsvfilesticker([FromForm] FileUploadModelSticker fileUploadModel)
        {
            var response = new ResponseSticker();
            try
            {
                if (fileUploadModel.FrontLaserPhoto == null)
                {
                    response.Message = "Please upload File";
                }
                else 
                {

                    var VehicleRegNo = string.Empty;
                    var getSession = new RootDtoSticker();
                    //if (HttpContext.Session.GetString("UserDetail") != null)
                    //{
                    //    var data = HttpContext.Session.GetString("UserSession");
                    //    var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(data);

                    //    VehicleRegNo = vehicledetails.VehicleRegNo;
                    //}
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var _frontLaserPhoto = fileUploadModel.FrontLaserPhoto;  // Get the uploaded file
                    var _rearLaserPhoto = fileUploadModel.RearLaserPhoto;
                    var _fronPlatePhoto = fileUploadModel.FrontPlatePhoto;
                    var _rearPlatePhoto = fileUploadModel.RearPlatePhoto;

                    double sz = fileUploadModel.FrontLaserPhoto.Length / 1024;
                    double sz2 = fileUploadModel.RearLaserPhoto.Length / 1024;
                    double sz3 = fileUploadModel.FrontPlatePhoto.Length / 1024;
                    double sz4 = fileUploadModel.RearPlatePhoto.Length / 1024;
                    sz = sz / 1024;
                    sz2 = sz2 / 1024;
                    sz3 = sz3 / 1024;
                    sz4 = sz4 / 1024;


                    if (!IsImage(Path.GetExtension(_frontLaserPhoto.FileName)) || !IsImage(Path.GetExtension(_rearLaserPhoto.FileName)) || !IsImage(Path.GetExtension(_fronPlatePhoto.FileName)) || !IsImage(Path.GetExtension(_rearPlatePhoto.FileName)))
                    {
                        //return BadRequest("Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!");
                        response.Message = "Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!";
                    }
                    else if (sz > 1 || sz2 > 1 || sz3 > 1 || sz4 > 1)
                    {
                        //return BadRequest("Error! File size can not be max 1.5 MB!!");
                        response.Message = "Error! File size can not be max 1.5 MB!!";
                    }
                    else
                    {
                        if (_frontLaserPhoto != null && _frontLaserPhoto.Length > 0 && _rearLaserPhoto != null && _rearLaserPhoto.Length > 0 && _fronPlatePhoto != null && _fronPlatePhoto.Length > 0 && _rearPlatePhoto != null && _rearPlatePhoto.Length > 0)
                        {
                            //string _dt = DateTime.Now.ToString("dd-MM-yyyy");
                            //DateTime.Now.ToString("yyyyMMddHHmmssfff")
                            var _dateFormate = HttpContext.Session.GetString("DateFormate");
                            var frontLaserfilePath = Path.Combine(path, "Front" + _dateFormate + "_" + RandomString(4) + Path.GetExtension(_frontLaserPhoto.FileName)); // Set the file path
                            var rearLaserfilePath = Path.Combine(path, "Rear" + _dateFormate + "_" + RandomString(4) + Path.GetExtension(_rearLaserPhoto.FileName));
                            var frontPlatefilePath = Path.Combine(path, "File1" + _dateFormate + "_" + RandomString(4) + Path.GetExtension(_fronPlatePhoto.FileName));
                            var rearPlatefilePath = Path.Combine(path, "File2" + _dateFormate + "_" + RandomString(4) + Path.GetExtension(_rearPlatePhoto.FileName));

                            using (var stream = new FileStream(frontLaserfilePath, FileMode.Create))
                            {
                                await _frontLaserPhoto.CopyToAsync(stream);
                            }

                            using (var stream = new FileStream(rearLaserfilePath, FileMode.Create))
                            {
                                await _rearLaserPhoto.CopyToAsync(stream);
                            }

                            using (var stream = new FileStream(frontPlatefilePath, FileMode.Create))
                            {
                                await _fronPlatePhoto.CopyToAsync(stream);
                            }

                            using (var stream = new FileStream(rearPlatefilePath, FileMode.Create))
                            {
                                await _rearPlatePhoto.CopyToAsync(stream);
                            }

                            HttpContext.Session.SetString("frontLaserfileName", frontLaserfilePath);
                            HttpContext.Session.SetString("rearLaserfileName", rearLaserfilePath);
                            HttpContext.Session.SetString("frontPlatefileName", frontPlatefilePath);
                            HttpContext.Session.SetString("rearPlatefileName", rearPlatefilePath);

                            response.Message = "File Uploaded Successfully";
                        }

                    }



                }
            }
            catch (Exception ex)
            {
                return BadRequest("No file uploaded"+ ex);
            }
            var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(response);
            return Json(jsonSerializer);

        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }


        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        public bool IsImage(string Ext)
        {
            foreach (string st in ".jpg|.jpeg|.bmp|.png|.pdf".Split('|'))
            {
                if (Ext.ToLower() == st)
                    return true;
            }
            return false;
        }

    }
}
