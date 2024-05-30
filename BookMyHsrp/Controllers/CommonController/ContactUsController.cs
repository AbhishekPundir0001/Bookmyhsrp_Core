using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    [ResponseCache(Duration = 60 * 60 * 24 * 30, Location = ResponseCacheLocation.Any)]
    public class ContactUsController : Controller
    {
        [Route("/contactUs/ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
