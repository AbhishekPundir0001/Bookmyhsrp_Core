using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class PrivacyPolicyController : Controller
    {
        [Route("/privacypolicy/PrivacyPolicy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
