using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
   
    [ResponseCache(Duration = 60 * 60 * 24 * 30, Location = ResponseCacheLocation.Any)]

    public class HsrpWithColourStickerController : Controller
    {
        [Route("/hsrpWithColorSticker/VahanBookingDetails")]
        public IActionResult VahanBookingDetails()
        {
            return View();
        }
    }
}
