using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class ReAppointmentController : Controller
    {
        [Route("/reappointment")]
        public IActionResult ReAppointment()
        {
            return View();
        }
    }
}
