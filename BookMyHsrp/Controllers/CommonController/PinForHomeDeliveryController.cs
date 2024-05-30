using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class PinForHomeDeliveryController : Controller
    {
        [Route("/pinforhomedelivery/PinForHomeDelivery")]
        public IActionResult PinForHomeDelivery()
        {
            return View();
        }
    }
}
