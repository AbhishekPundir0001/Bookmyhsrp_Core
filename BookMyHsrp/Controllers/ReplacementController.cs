using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class ReplacementController : Controller
    {
        [Route("/replacement/details")]
        public IActionResult VahanBookingDetails()
        {
            return View();
        }
    }
}
