using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class ReprintReceiptController : Controller
    {
        public IActionResult ReprintReceipt()
        {
            return View();
        }
    }
}
