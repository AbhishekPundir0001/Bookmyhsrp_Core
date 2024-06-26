using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
	public class FaqController : Controller
	{
		[Route("/faq")]
		public IActionResult Faq()
		{
			return View();
		}
	}
}
