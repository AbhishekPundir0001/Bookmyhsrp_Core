using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class HomeDeliveryController : Controller
    {
        [Route("/homeDelivery/homedelivery")]
        public IActionResult HomeDelivery()
        {
            return View();
        }
    }
}
