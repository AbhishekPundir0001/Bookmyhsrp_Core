using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class SafeSecureTransactionController : Controller
    {
        [Route("/safesecuretransaction")]
        public IActionResult SafeSecureTransaction()
        {
            return View();
        }
    }
}
