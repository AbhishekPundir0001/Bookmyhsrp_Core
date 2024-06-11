using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return View("Home/index.cshtml");
        }
    }
}
