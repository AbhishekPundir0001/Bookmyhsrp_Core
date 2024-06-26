using BookMyHsrp.Libraries.Receipt.Models;
using BookMyHsrp.Libraries.ReceiptValidity.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
{
    public class ReprintReceiptController : Controller
    {
        [Route("/reprintreceipt")]
        public IActionResult ReprintReceipt()
        {
            return View();
        }
        //public ValidateReceipt(ReceiptValidityModel.ReceiptValidity requestdto)
        //{
            
        //}
    }
}
