using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class PlateController : Controller
    {
        [Route("/plate/details")]
        public IActionResult VahanBookingDetails(string BookingType)
        {
            HttpContext.Session.SetString("UserDetails", "null");

            ViewBag.BookingType = BookingType;
            return View();
        }
    }
}
