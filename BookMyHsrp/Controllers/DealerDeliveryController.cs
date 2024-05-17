using BookMyHsrp.Libraries.HomeDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers
{
    public class DealerDeliveryController : Controller
    {
        private readonly ILogger<DealerDeliveryController> _logger;
        public DealerDeliveryController(ILogger<DealerDeliveryController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [Route("/dealerDelivery")]
        public IActionResult DealerDelivery()
        {
          
            return View();
        }
        [Route("/get-stateId")]
        [HttpGet]
        public IActionResult GetStateId()
        {
            var sessiondata = HttpContext.Session.GetString("UserSession");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(sessiondata);
            var stateId = vehicledetails.StateId;
            return Json(stateId);
        }
    }
}
