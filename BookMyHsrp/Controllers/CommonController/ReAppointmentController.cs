using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class ReAppointmentController : Controller
    {
        [Route("/reappointment/ReAppointment")]
        public IActionResult ReAppointment()
        {
            return View();
        }
    }
}
