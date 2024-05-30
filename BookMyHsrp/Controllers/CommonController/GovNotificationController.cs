using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class GovNotificationController : Controller
    {
        [Route("/govnotification/GovtNotification")]
        public IActionResult GovNotification()
        {
            return View();
        }
    }
}
