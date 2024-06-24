using BookMyHsrp.Libraries.OemMaster.Services;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    [Route("api/v1/oem")]
    public class OemMasterController : Controller
    {
        private readonly ILogger<OemMasterController> logger;
        private readonly IOemMasterService _oemMaster;
        public OemMasterController(IOemMasterService oemMaster)
        {
            _oemMaster = oemMaster;
        }
        [HttpGet]
        [Route("by-vehicle-type/{vehicleType}")]
        public async Task<dynamic> GetAllOemByVehicleType(string vehicleType)
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);

            var resultGot = await _oemMaster.GetAllOemByVehicleType(vehicleType, vehicledetails);
            return resultGot;

        }

    }
}
