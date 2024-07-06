using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers
{
    public class ReAppointmentSlotController : Controller
    {
        [Route("/reappointmentslot")]
        public IActionResult ReAppointmentSlot()
        {
            return View();
        }
    }
}
