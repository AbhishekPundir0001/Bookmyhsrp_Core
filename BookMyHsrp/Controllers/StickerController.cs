using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class StickerController : Controller
    {

        [Route("/sticker/details")]
        public IActionResult VahanBookingDetails()
        {
            return View();
        }
    }
}
