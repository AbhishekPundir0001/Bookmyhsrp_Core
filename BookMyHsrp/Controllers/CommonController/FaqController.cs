using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
	public class FaqController : Controller
	{
		[Route("/faq/Faq")]
		public IActionResult Faq()
		{
			return View();
		}
	}
}
