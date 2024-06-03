using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Common.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Net;
using System.Text;
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
                if (fileUploadModel.FrontLaserPhoto == null || fileUploadModel.RearLaserPhoto == null || fileUploadModel.FrontPlatePhoto ==null || fileUploadModel.RearPlatePhoto == null)
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
                return BadRequest("No file uploaded" + ex);
            }
            var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(response);
            return Json(jsonSerializer);

        }


        [Route("uploadFileReplacement")]
        public async Task<IActionResult> uploadFileRepla([FromForm] FileUploadModelReplacement fileUploadModel)
        {
            //var _fronPlatePhoto = fileUploadModel.FrontPlatePhoto;
            //var frontPlatefilePath = Path.Combine(path, "File1"  + "_" + RandomString(4) + Path.GetExtension(_fronPlatePhoto.FileName));

            //using (var stream = new FileStream(frontPlatefilePath, FileMode.Create))
            //{
            //    await _fronPlatePhoto.CopyToAsync(stream);
            //}
            //var data = new { Message = "File Uploaded Successfully" };

            //var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(data);
            //return Json(jsonSerializer);

            #region File Upload
            string msg = string.Empty;
            string fileName = string.Empty;
            string GetExtention = string.Empty;
            string fileName2 = string.Empty;
            string GetExtention2 = string.Empty;
            string fileName3 = string.Empty;
            string GetExtention3 = string.Empty;
            string fileName4 = string.Empty;
            string GetExtention4 = string.Empty;
            double sz = 0;
            double sz2 = 0;
            double sz3 = 0;
            double sz4 = 0;

            string ReplacementReason = fileUploadModel.ReplacementReason;
            string OrderType = string.Empty;

            OrderType = fileUploadModel.OrderType;
            string stateid = fileUploadModel.StateId;
            string folderPath = path;

            //Set the File Name.
            if (fileUploadModel.FrontPlatePhoto != null)
            {
                fileName = Path.GetFileName(fileUploadModel.FrontPlatePhoto.FileName);            //Front
                GetExtention = Path.GetExtension(fileUploadModel.FrontPlatePhoto.FileName);
                sz = fileUploadModel.FrontPlatePhoto.Length / 1024;
            }
            if (fileUploadModel.RearPlatePhoto != null)
            {
                fileName2 = Path.GetFileName(fileUploadModel.RearPlatePhoto.FileName);          //Rear
                GetExtention2 = Path.GetExtension(fileUploadModel.RearPlatePhoto.FileName);
                sz2 = fileUploadModel.RearPlatePhoto.FileName.Length / 1024;
            }
            if (fileUploadModel.FirCopy != null)
            {
                fileName3 = Path.GetFileName(fileUploadModel.FirCopy.FileName);        //FIR
                GetExtention3 = Path.GetExtension(fileUploadModel.FirCopy.FileName);
                sz3 = fileUploadModel.FirCopy.FileName.Length / 1024;
            }
            if (fileUploadModel.RcCopy != null)
            {
                fileName4 = Path.GetFileName(fileUploadModel.RcCopy.FileName);             //RC
                GetExtention4 = Path.GetExtension(fileUploadModel.RcCopy.FileName);
                sz4 = fileUploadModel.RcCopy.FileName.Length / 1024;
            }

            sz = sz / 1024;
            sz2 = sz2 / 1024;
            sz3 = sz3 / 1024;
            sz4 = sz4 / 1024;
            if (OrderType == "BDB")
            {
                if (ReplacementReason == "LT" || ReplacementReason == "BD" || ReplacementReason == "CA" || ReplacementReason == "RE")
                {
                    if (Convert.ToInt32(stateid) != 25)
                    {
                        if (!IsImage(Path.GetExtension(fileUploadModel.FirCopy.FileName)) || !IsImage(Path.GetExtension(fileUploadModel.RcCopy.FileName)))
                            fileName = "Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!";
                        else if (sz3 > 1 || sz4 > 1)
                            fileName = "Error! File size can not be max 1.5 MB!!";
                        else
                        {
                            fileName3 = "FIR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention3;
                            var _firCopy = fileUploadModel.FirCopy;
                            var firCopy = Path.Combine(path, fileName3);
                            using (var stream = new FileStream(firCopy, FileMode.Create))
                            {
                                await _firCopy.CopyToAsync(stream);
                            }

                            fileName4 = "RC_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention4;
                            var _rcCopy = fileUploadModel.RcCopy;
                            var rcCopy = Path.Combine(path, fileName4);
                            using (var stream = new FileStream(rcCopy, FileMode.Create))
                            {
                                await _rcCopy.CopyToAsync(stream);
                            }
                            msg = "File Uploaded Successfully";
                        }
                    }

                    if (Convert.ToInt32(stateid) == 25)
                    {
                        if (ReplacementReason == "LT")
                        {
                            if (fileName3 == "" || fileName3 == "")
                            {
                                fileName = "Error! Please Upload FIR File!!";
                            }
                            else
                            {
                                fileName3 = "FIR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention3;
                                var _firCopy = fileUploadModel.FirCopy;
                                var firCopy = Path.Combine(path, fileName3);
                                using (var stream = new FileStream(firCopy, FileMode.Create))
                                {
                                    await _firCopy.CopyToAsync(stream);
                                }
                                msg = "File Uploaded Successfully";
                            }
                        }

                        if (ReplacementReason == "CA" || ReplacementReason == "RE")
                        {
                            if (fileName4 == "" || fileName4 == "")
                            {
                                fileName = "Error! Please Upload RC File Code!!";
                            }
                            else
                            {
                                fileName4 = "RC_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention4;
                                var _rcCopy = fileUploadModel.RcCopy;
                                var rcCopy = Path.Combine(path, fileName4);
                                using (var stream = new FileStream(rcCopy, FileMode.Create))
                                {
                                    await _rcCopy.CopyToAsync(stream);
                                }
                                msg = "File Uploaded Successfully";
                            }
                        }

                        if (ReplacementReason == "BD")
                        {
                            if (OrderType == "BDF")
                            {
                                if (fileName == "" || fileName == "")
                                {
                                    fileName = "Error! Please Upload Front Laser Code!!";
                                }
                                else
                                {
                                    fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                                    var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                                    var frontPlateCopy = Path.Combine(path, fileName);
                                    using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                                    {
                                        await _frontPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";
                                }
                            }
                            else if (OrderType == "BDR")
                            {
                                if (fileName2 == "" || fileName2 == "")
                                {
                                    fileName = "Error! Please Upload Rear Laser Code!!";
                                }
                                else
                                {
                                    fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                                    var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                                    var rearPlateCopy = Path.Combine(path, fileName2);
                                    using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                                    {
                                        await _rearPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";
                                }
                            }
                            else if (OrderType == "BDB")
                            {
                                if (fileName == "" || fileName == "")
                                {
                                    fileName = "Error! Please Upload Front Laser Code!!";
                                }
                                else if (fileName2 == "" || fileName2 == "")
                                {
                                    fileName = "Error! Please Upload Rear Laser Code!!";
                                }
                                else
                                {
                                    fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                                    var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                                    var frontPlateCopy = Path.Combine(path, fileName);
                                    using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                                    {
                                        await _frontPlateCopy.CopyToAsync(stream);
                                    }

                                    fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                                    var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                                    var rearPlateCopy = Path.Combine(path, fileName2);
                                    using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                                    {
                                        await _rearPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";
                                }

                            }

                        }

                    }

                }
                else
                {
                    fileName = "Error! Please Select Reason For Replacement.!!";
                }


            }
            else if (OrderType == "BDR")
            {

                if (ReplacementReason == "LT" || ReplacementReason == "BD" || ReplacementReason == "CA" || ReplacementReason == "RE")
                {
                    if (Convert.ToInt32(stateid) != 25)
                    {
                        if (fileUploadModel.FrontPlatePhoto == null || fileUploadModel.FirCopy == null || fileUploadModel.RcCopy == null)
                            fileName = "Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!";
                        else if (!IsImage(Path.GetExtension(fileUploadModel.FrontPlatePhoto.FileName)) || !IsImage(Path.GetExtension(fileUploadModel.FirCopy.FileName)) || !IsImage(Path.GetExtension(fileUploadModel.RcCopy.FileName)))
                            fileName = "Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!";
                        else if (sz > 1 || sz3 > 1 || sz4 > 1)
                            fileName = "Error! File size can not be max 1.5 MB!!";
                        else
                        {
                            //Save the File in Folder.
                            //fileName = "Front" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileName.Replace(" ", "");
                            fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                            var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                            var frontPlateCopy = Path.Combine(path, fileName);
                            using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                            {
                                await _frontPlateCopy.CopyToAsync(stream);
                            }

                            fileName3 = "FIR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention3;
                            var _firCopy = fileUploadModel.FirCopy;
                            var firCopy = Path.Combine(path, fileName3);
                            using (var stream = new FileStream(firCopy, FileMode.Create))
                            {
                                await _firCopy.CopyToAsync(stream);
                            }

                            fileName4 = "RC_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention4;
                            var _rcCopy = fileUploadModel.RcCopy;
                            var rcCopy = Path.Combine(path, fileName4);
                            using (var stream = new FileStream(firCopy, FileMode.Create))
                            {
                                await _firCopy.CopyToAsync(stream);
                            }
                            msg = "File Uploaded Successfully";
                        }
                    }

                    if (Convert.ToInt32(stateid) == 25)
                    {
                        if (ReplacementReason == "LT")
                        {
                            if (fileName3 == "" || fileName3 == "")
                            {
                                fileName = "Error! Please Upload FIR!!";
                            }
                            else
                            {
                                fileName3 = "FIR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention3;
                                var _firCopy = fileUploadModel.FirCopy;
                                var firCopy = Path.Combine(path, fileName3);
                                using (var stream = new FileStream(firCopy, FileMode.Create))
                                {
                                    await _firCopy.CopyToAsync(stream);
                                }
                                msg = "File Uploaded Successfully";
                            }
                        }

                        if (ReplacementReason == "CA" || ReplacementReason == "RE")
                        {
                            if (fileName4 == "" || fileName4 == "")
                            {
                                fileName = "Error! Please Upload FIR!!";
                            }
                            else
                            {
                                fileName4 = "RC_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention4;
                                var _rcCopy = fileUploadModel.RcCopy;
                                var rcCopy = Path.Combine(path, fileName4);
                                using (var stream = new FileStream(rcCopy, FileMode.Create))
                                {
                                    await _rcCopy.CopyToAsync(stream);
                                }
                                msg = "File Uploaded Successfully";
                            }
                        }

                        if (ReplacementReason == "BD")
                        {
                            if (OrderType == "BDF")
                            {
                                if (fileName == "" || fileName == "")
                                {
                                    fileName = "Error! Please Upload FIR!!";
                                }
                                else
                                {
                                    fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                                    var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                                    var frontPlateCopy = Path.Combine(path, fileName);
                                    using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                                    {
                                        await _frontPlateCopy.CopyToAsync(stream);
                                    }
                                    fileName = "File Uploaded Successfully";
                                }
                            }
                            else if (OrderType == "BDR")
                            {
                                if (fileName2 == "" || fileName2 == "")
                                {
                                    fileName = "Error! Please Upload Rear File!!";
                                }
                                else
                                {
                                    fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                                    var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                                    var rearPlateCopy = Path.Combine(path, fileName2);
                                    using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                                    {
                                        await _rearPlateCopy.CopyToAsync(stream);
                                    }
                                }
                            }
                            else if (OrderType == "BDB")
                            {
                                if (fileName == "" || fileName == "")
                                {
                                    fileName = "Error! Please Upload Front Laser File!!";
                                }
                                else if (fileName2 == "" || fileName2 == "")
                                {
                                    fileName = "Error! Please Upload Rear Laser File!!";
                                }
                                else
                                {
                                    fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                                    var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                                    var frontPlateCopy = Path.Combine(path, fileName);
                                    using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                                    {
                                        await _frontPlateCopy.CopyToAsync(stream);
                                    }
                                    fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                                    var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                                    var rearPlateCopy = Path.Combine(path, fileName2);
                                    using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                                    {
                                        await _rearPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";

                                }

                            }

                        }

                    }

                }
                else
                {
                    fileName = "Error! Please Select Reason For Replacement.!!";
                }


            }
            else if (OrderType == "BDF")
            {
                
                if (ReplacementReason == "LT" || ReplacementReason == "BD" || ReplacementReason == "CA" || ReplacementReason == "RE")
                {
                    if (Convert.ToInt32(stateid) != 25)
                    {
                        if(fileUploadModel.RearPlatePhoto == null || fileUploadModel.FirCopy==null || fileUploadModel.RcCopy == null)
                            fileName = "Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!";
                        else if (!IsImage(Path.GetExtension(fileUploadModel.RearPlatePhoto.FileName)) || !IsImage(Path.GetExtension(fileUploadModel.FirCopy.FileName)) || !IsImage(Path.GetExtension(fileUploadModel.RcCopy.FileName)))
                            fileName = "Error! Invalid image file format, file should be .jpg|.jpeg|.bmp|.png|.pdf!!";
                        else if (sz2 > 1 || sz3 > 1 || sz4 > 1)
                            fileName = "Error! File size can not be max 1.5 MB!!";
                        else
                        {
                            //Save the File in Folder.
                            fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                            var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                            var rearPlateCopy = Path.Combine(path, fileName2);
                            using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                            {
                                await _rearPlateCopy.CopyToAsync(stream);
                            }

                            fileName3 = "FIR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention3;
                            var _firCopy = fileUploadModel.FirCopy;
                            var firCopy = Path.Combine(path, fileName3);
                            using (var stream = new FileStream(firCopy, FileMode.Create))
                            {
                                await _firCopy.CopyToAsync(stream);
                            }

                            fileName4 = "RC_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention4;
                            var _rcCopy = fileUploadModel.RcCopy;
                            var rcCopy = Path.Combine(path, fileName4);
                            using (var stream = new FileStream(rcCopy, FileMode.Create))
                            {
                                await _rcCopy.CopyToAsync(stream);
                            }
                            msg = "File Uploaded Successfully";
                        }
                    }

                    if (Convert.ToInt32(stateid) == 25)
                    {
                        if (ReplacementReason == "LT")
                        {
                            if (fileName3 == "" || fileName3 == null)
                            {
                                fileName = "Error! Please Upload FIR File!!";
                            }
                            else
                            {
                                fileName3 = "FIR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention3;
                                var _firCopy = fileUploadModel.FirCopy;
                                var firCopy = Path.Combine(path, fileName3);
                                using (var stream = new FileStream(firCopy, FileMode.Create))
                                {
                                    await _firCopy.CopyToAsync(stream);
                                }
                                msg = "File Uploaded Successfully";
                            }

                        }

                        if (ReplacementReason == "CA" || ReplacementReason == "RE")
                        {
                            if (fileName4 == "" || fileName4 == null)
                            {
                                fileName = "Error! Please Upload RC File!!";
                            }
                            else
                            {
                                fileName4 = "RC_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention4;
                                var _rcCopy = fileUploadModel.RcCopy;
                                var rcCopy = Path.Combine(path, fileName4);
                                using (var stream = new FileStream(rcCopy, FileMode.Create))
                                {
                                    await _rcCopy.CopyToAsync(stream);
                                }
                                msg = "File Uploaded Successfully";
                            }


                        }

                        if (ReplacementReason == "BD")
                        {
                            if (OrderType == "BDF")
                            {
                                if (fileName == "" || fileName == null)
                                {
                                    fileName = "Error! Please Upload Front Laser Code!!";
                                }
                                else
                                {
                                    fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                                    var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                                    var frontPlateCopy = Path.Combine(path, fileName);
                                    using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                                    {
                                        await _frontPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";
                                }
                            }
                            else if (OrderType == "BDR")
                            {
                                if (fileName2 == "" || fileName2 == null)
                                {
                                    fileName = "Error! Please Upload Rear Laser Code!!";
                                }
                                else
                                {
                                    fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                                    var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                                    var rearPlateCopy = Path.Combine(path, fileName2);
                                    using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                                    {
                                        await _rearPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";
                                }
                            }
                            else if (OrderType == "BDB")
                            {

                                if (fileName == "" || fileName == "")
                                {
                                    fileName = "Error! Please Upload Front Laser Code!!";
                                }
                                else if (fileName2 == "" || fileName2 == "")
                                {
                                    fileName = "Error! Please Upload Rear Laser Code!!";
                                }
                                else
                                {
                                    fileName = "Front_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention;
                                    var _frontPlateCopy = fileUploadModel.FrontPlatePhoto;
                                    var frontPlateCopy = Path.Combine(path, fileName);
                                    using (var stream = new FileStream(frontPlateCopy, FileMode.Create))
                                    {
                                        await _frontPlateCopy.CopyToAsync(stream);
                                    }

                                    fileName2 = "Rear_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RandomString(4) + GetExtention2;
                                    var _rearPlateCopy = fileUploadModel.RearPlatePhoto;
                                    var rearPlateCopy = Path.Combine(path, fileName2);
                                    using (var stream = new FileStream(rearPlateCopy, FileMode.Create))
                                    {
                                        await _rearPlateCopy.CopyToAsync(stream);
                                    }
                                    msg = "File Uploaded Successfully";
                                }
                            }

                        }

                    }

                }
                else
                {
                    fileName = "Error! Please Select Reason For Replacement.!!";
                }


            }
            else
            {
                fileName = "Error! Please Select Damage Plate.";
            }
            #endregion

            var data = new {    Message = msg,
                                FrontPlatePhoto=fileName,
                                RearPlatePhoto=fileName2,
                                FirCopy=fileName3,
                                RcCopy=fileName4
                          };
            var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(data);
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
