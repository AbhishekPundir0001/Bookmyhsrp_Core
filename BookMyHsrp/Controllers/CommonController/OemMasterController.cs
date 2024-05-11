using BookMyHsrp.Libraries.OemMaster.Services;
using Microsoft.AspNetCore.Mvc;

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

            var resultGot = await _oemMaster.GetAllOemByVehicleType(vehicleType);
            return resultGot;

        }

    }
}
