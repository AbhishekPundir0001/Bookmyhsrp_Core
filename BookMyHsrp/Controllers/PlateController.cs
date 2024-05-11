using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class PlateController : Controller
    {
        [Route("/plate/details")]
        public IActionResult VahanBookingDetails()
        {
            HttpContext.Session.SetString("UserDetails", "null");

            return View();
        }
    }
}
