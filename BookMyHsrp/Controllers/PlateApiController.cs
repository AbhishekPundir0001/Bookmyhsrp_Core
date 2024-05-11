using BookMyHsrp.ApiController.ApiHSRPWithColourSticker;
using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.ResponseWrapper.Models;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using System.Web.Helpers;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
namespace BookMyHsrp.Controllers
{


    public class PlateApiController : Controller
    {
        private readonly ILogger<ApiHsrpWithColorStickerController> _logger;

        private readonly HsrpWithColorStickerConnector _hsrpWithColorStickerConnector;
        public PlateApiController(ILogger<ApiHsrpWithColorStickerController> logger,
            HsrpWithColorStickerConnector hsrpWithColorStickerConnector)
        {
            _hsrpWithColorStickerConnector =
                hsrpWithColorStickerConnector ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerConnector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        [HttpPost]
        [Route("/plate/validateRequired")]
        public async Task<IActionResult> ValidateRequired([FromBody] VahanDetailsDto vahanDetailsDto)
        {
            var jsonSerializer = "";
            var result = await _hsrpWithColorStickerConnector.VahanInformation(vahanDetailsDto);
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

                { Status = result.status,
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
                    Message = result.data.message
                };

                var GetRootObjectSession = HttpContext.Session.GetString("UserSession");
                 jsonSerializer = System.Text.Json.JsonSerializer.Serialize(rootDto);
                HttpContext.Session.SetString("UserSession", jsonSerializer);

                //if (!string.IsNullOrEmpty(UpdateRootObjectSession))

                //{

                //    var jsonDeSerializer = System.Text.Json.JsonSerializer.Deserialize<RootDto>(UpdateRootObjectSession);
                //    jsonDeSerializer = rootDto;
                //    var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(jsonDeSerializer);
                //    HttpContext.Session.SetString("UserSession", jsonSerializer);

                //}
                //else
                //{
                //    var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(rootDto);
                //    HttpContext.Session.SetString("UserSession", jsonSerializer);
                //}



            }
            if(jsonSerializer!="")
            {
                return Json(jsonSerializer);
            }
            else
            {

                return BadRequest(new { Error = true, Message = result.message });
            }
            
        }
        [HttpPost]
        [Route("report/SetSessionBookingDetail")]

        public async Task<IActionResult> SetSessionBookingDetail(HsrpColorStickerModel requestDto)
        {
                var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(requestDto);
                HttpContext.Session.SetString("SessionDetail", jsonSerializer);
                var result = await _hsrpWithColorStickerConnector.SessionBookingDetails(requestDto);
                if (result.Message == "Vehicle Details didn't match")
                {
                    return BadRequest(new { Error = true, result.Message });
                }
                return Ok(
                      new Response<dynamic>(result, false,
                          "Data Received."));

            }
        //    public async Task<JsonResult> CustomerInfo(CustomerInfoModel info)
        //{

        //    var resultGot = new CustomerInformationResponse();
        //    if (HttpContext.Session.GetString("UserDetail") != null)
        //    {
        //        resultGot = await await _hsrpWithColorStickerConnector.CustomerInfo(vahanDetailsDto)
        //    //    var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(requestDto);
        //    //HttpContext.Session.SetString("SessionDetail", jsonSerializer);
        //    //var result = await _hsrpWithColorStickerConnector.SessionBookingDetails(requestDto);
        //    //if (result.Message == "Vehicle Details didn't match")
        //    //{
        //    //    return BadRequest(new { Error = true, result.Message });
        //    //}
        //    //return Ok(
        //    //      new Response<dynamic>(result, false,
        //    //          "Data Received."));

        //}




    }
}
