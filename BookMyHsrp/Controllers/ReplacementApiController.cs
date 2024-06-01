using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using BookMyHsrp.ReportsLogics.Replacement;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.ReplacementHsrpColorStickerModel;

namespace BookMyHsrp.Controllers
{
    public class ReplacementApiController : Controller
    {

        private readonly ILogger<ReplacementApiController> _logger;

        private readonly ReplacementConnector _replacementConnector;
        public ReplacementApiController(ILogger<ReplacementApiController> logger, ReplacementConnector hsrpWithColorStickerConnector)
        {
            _replacementConnector = hsrpWithColorStickerConnector ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerConnector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("/Replacement/ReValidateData")]
        public async Task<IActionResult> ValidateRequired([FromBody] ReplacementVahanDetailsDto vahanDetailsDto)
        {
            var jsonSerializer = "";
            var result = await _replacementConnector.VahanInformation(vahanDetailsDto);
            if (result.status == "true")
            {
                if (!string.IsNullOrEmpty(result.data.veh_reg_date))
                {
                    IFormatProvider theCultureInfo = new CultureInfo("en-GB", true);
                    var resultDateTime = DateTime.TryParseExact(result.data.veh_reg_date, "yyyy-MM-dd",
                        theCultureInfo,
                        DateTimeStyles.None
                        , out DateTime dt)
                        ? dt
                        : null as DateTime?;
                    if (resultDateTime.HasValue)
                    {
                        result.data.veh_reg_date = resultDateTime.Value.ToString("dd/MM/yyyy");
                    }
                    //else

                }
                var rootDto = new RootDto

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
                    VehRegDate = result.data.veh_reg_date,
                    VehicleCategory = result.data.vehicle_category,
                    VehicleClass = result.data.vehicle_class,
                    FuelType = result.data.fuel,
                    PlateSticker = "Plate",
                    IsReplacement = vahanDetailsDto.isReplacement,
                    Message = result.data.message,
                    StateIdBackup = result.data.StateIdBackup
                };

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
        [Route("/Replacement/ReplacementBooking")]
        public async Task<IActionResult> CustomerInfo([FromBody] ReplacementCustomerInfoModel info)
        {
            var jsonSerializer = "";
            var resultGot = new CustomerInformationResponse();
            var detailsSession = HttpContext.Session.GetString("UserSession");
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                resultGot = await _replacementConnector.CustomerInfo(info, detailsSession);
                if (resultGot.Message == "Success")
                {
                    var getSession = new RootDto();
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
                    getSession.OrderType = resultGot.data.OrderType;

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
