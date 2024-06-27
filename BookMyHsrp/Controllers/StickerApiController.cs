
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.Sticker.Models;
using BookMyHsrp.ReportsLogics.Sticker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Xml;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;

namespace BookMyHsrp.Controllers
{
    public class StickerApiController : Controller
    {

        private readonly ILogger<StickerApiController> _logger;

        private readonly StickerConnector _StickerConnector;
        public StickerApiController(ILogger<StickerApiController> logger, StickerConnector StickerConnector)
        {
            _StickerConnector = StickerConnector ?? throw new ArgumentNullException(nameof(StickerConnector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("/sticker/validateRequired")]
        public async Task<IActionResult> ValidateRequired([FromBody] VahanDetailsDto vahanDetailsDto)
        {
            #region Validation
            if (string.IsNullOrEmpty(vahanDetailsDto.StateId))
            {
                return BadRequest(new { Error = true, Message = "Please Select  Vehicle Registration State" });
            }
            if (string.IsNullOrEmpty(vahanDetailsDto.RegistrationNo))
            {
                return BadRequest(new { Error = true, Message = "Please Enter  Vehicle Registration No." });
            }
            if(vahanDetailsDto.RegistrationNo.Length<5)
            {
                return BadRequest(new { Error = true, Message = "Please Enter Valid RegNumber No." });
            }
            if (string.IsNullOrEmpty(vahanDetailsDto.ChassisNo))
            {
                return BadRequest(new { Error = true, Message = "Please Enter Chassis No." });
            }
            if (vahanDetailsDto.ChassisNo.Length < 5)
            {
                return BadRequest(new { Error = true, Message = "Please Enter Valid Chassis No." });
            }
            if (string.IsNullOrEmpty(vahanDetailsDto.EngineNo))
            {
                return BadRequest(new { Error = true, Message = "Please Enter Engine No." });
            }
            if (vahanDetailsDto.EngineNo.Length < 5)
            {
                return BadRequest(new { Error = true, Message = "Please Enter Valid Engine No." });
            }

            if (!vahanDetailsDto.HsrpFrontLaserCode.ToString().ToLower().StartsWith("aa"))
            {
                return BadRequest(new { Error = true, Message = "Please enter valid Front Laser Code" });
            }
            if (!vahanDetailsDto.HsrpRearLaserCode.ToString().ToLower().StartsWith("aa"))
            {
                return BadRequest(new { Error = true, Message = "Please enter valid Rear Laser Code" });
            }
            #endregion

            HttpContext.Session.Clear();
            var jsonSerializer = "";
            var result = await _StickerConnector.VahanInformation(vahanDetailsDto);

            if (result.status == "true")
            {
                if (!string.IsNullOrEmpty(result.data.regnDate))
                {
                    IFormatProvider theCultureInfo = new CultureInfo("en-GB", true);
                    var resultDateTime = DateTime.TryParseExact(result.data.regnDate, "yyyy-MM-dd",theCultureInfo,DateTimeStyles.None, out DateTime dt) ? dt : null as DateTime?;
                    if (resultDateTime.HasValue)
                    {
                        result.data.regnDate = resultDateTime.Value.ToString("dd/MM/yyyy");
                    }
                    //else
                }
                var rootDto = new RootDtoSticker
                {
                    Status = result.status,
                    StateId = result.data.stateid,
                    VehicleRegNo = result.data.vehicleregno,
                    ChassisNo = result.data.chassisno,
                    EngineNo = result.data.engineno,
                    Fuel = result.data.fuel,
                    Maker = result.data.maker,
                    NonHomo = result.data.non_homo,
                    Norms = result.data.norms,
                    OemImgPath = result.data.oem_img_path,
                    OemId = result.data.oemid,
                    StateName = result.data.statename,
                    StateShortName = result.data.stateshortname,
                    VehRegDate = result.data.regnDate,
                    VehicleCategory = result.data.vchCatg,
                    VehicleClass = result.data.vchType,
                    FuelType = result.data.fuel,
                    PlateSticker = "Sticker",
                    IsReplacement = vahanDetailsDto.isReplacement,
                    UploadFlag = result.UploadFlag,
                    Message = result.data.message
                };

                var resultDateFormate = await _StickerConnector.DateFormate();
                string DateFormate = resultDateFormate[0].FormattedDate;
                if (resultDateFormate.Count > 0)
                {
                    HttpContext.Session.SetString("DateFormate", DateFormate);
                }

                var GetRootObjectSession = HttpContext.Session.GetString("UserSession");
                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(rootDto);
                HttpContext.Session.SetString("UserSession", jsonSerializer);
            }

            if (jsonSerializer != "")
            {
                return Json(jsonSerializer);
            }
            else
            {
                return BadRequest(new { Error = true, Message = result.message });
            }
        }

        [HttpPost]
        [Route("sticker/customerInfo")]
        public async Task<IActionResult> CustomerInfo([FromBody] CustomerInfoModelSticker info)
        {

            var jsonSerializer = "";
            var resultGot = new CustomerInformationResponseSticker();
            var detailsSession = HttpContext.Session.GetString("UserSession");
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                //var frontLaserfileName = HttpContext.Session.GetString("frontLaserfileName");
                //var rearLaserfileName = HttpContext.Session.GetString("rearLaserfileName");
                //var frontPlatefileName = HttpContext.Session.GetString("frontPlatefileName");
                //var rearPlatefileName = HttpContext.Session.GetString("rearPlatefileName");

                if (info.UploadFlag == "Y")
                {
                    if (info.FrontPlatePath == null || info.FrontPlatePath == "" || info.RearPlatePath == null || info.RearPlatePath == "" || info.FrontLaserCode == null || info.FrontLaserCode == "" || info.RearLaserCode == null || info.RearLaserCode == "")
                    {
                        return BadRequest(new { Error = true, Message = "Please Upload File" });
                    }
                }

                resultGot = await _StickerConnector.CustomerInfo(info, detailsSession);
                if (resultGot.Message == "Success")
                {
                    var getSession = new RootDtoSticker();
                    getSession.CustomerBillingAddress = info.BillingAddress.Replace("'", "");
                    getSession.BhartStage = info.BharatStage;
                    getSession.CustomerName = info.OwnerName;
                    getSession.CustomerEmail = info.EmailId;
                    getSession.CustomerMobile = info.MobileNo;
                    getSession.VehicleType = resultGot.data.VehicleType;
                    getSession.VehicleCat = resultGot.data.VehicleCat;
                    getSession.VehicleTypeId = resultGot.data.VehicleTypeId;
                    getSession.VehicleCategoryId = resultGot.data.Vehiclecategoryid;
                    getSession.FuelType = info.FuelTypeVahan;
                    getSession.Fuel = info.FuelTypeVahan;
                    getSession.Message = resultGot.Message;
                    var GetRootObjectSession = HttpContext.Session.GetString("UserSession");
                    jsonSerializer = System.Text.Json.JsonSerializer.Serialize(getSession);
                    HttpContext.Session.SetString("UserDetail", jsonSerializer);


                }

            }
            if (jsonSerializer != "")
            {
                return Json(jsonSerializer);
            }
            else
            {

                return BadRequest(new { Error = true, Message = resultGot.Message });
            }


        }

    }
}
