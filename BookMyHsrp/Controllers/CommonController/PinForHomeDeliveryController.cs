using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class PinForHomeDeliveryController : Controller
    {
        [Route("/pinforhomedelivery")]
        public IActionResult PinForHomeDelivery()
        {
            return View();
        }
    }
}
