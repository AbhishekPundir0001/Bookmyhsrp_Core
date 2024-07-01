using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class PrivacyPolicyController : Controller
    {
        [Route("/privacypolicy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
