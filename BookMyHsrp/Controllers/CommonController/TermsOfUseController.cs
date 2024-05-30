using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    [ResponseCache(Duration = 60 * 60 * 24 * 30, Location = ResponseCacheLocation.Any)]

    public class TermsOfUseController : Controller
    {
        [Route("/termsOfUse/TermsOfUse")]
        public IActionResult TermsOfUse()
        {
            return View();
        }
    }
}
