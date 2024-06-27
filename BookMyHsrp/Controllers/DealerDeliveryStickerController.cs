using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class DealerDeliveryStickerController : Controller
    {
        [Route("/dealerDeliverySticker")]
        public IActionResult DealerDeliverySticker()
        {

            return View();
        }
    }
}
