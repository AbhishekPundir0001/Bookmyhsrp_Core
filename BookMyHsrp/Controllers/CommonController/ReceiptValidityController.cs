using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BookMyHsrp.Dapper;
using BookMyHsrp.ReportsLogics.Receipt;
using Microsoft.Extensions.Options;
using BookMyHsrp.Libraries.Receipt.Models;
using BookMyHsrp.Libraries.Receipt.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Models;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class ReceiptValidityController(IReceiptValidityService receiptValidityService) : Controller
    {

        private readonly IReceiptValidityService _receiptValidityService = receiptValidityService ?? throw new ArgumentNullException(nameof(receiptValidityService));


        [Route("/ReceiptValidity")]
        public IActionResult ReceiptValidity()
        {
            return View();

        }


        [HttpPost]
        [Route("/receipt-validaity")]
        public async Task<IActionResult> ValidateReceipt([FromBody]ReceiptValidityModel.ReceiptValidity requestdto)
        {
            var response = new ResponseDto();
            var result =await _receiptValidityService.CheckReceiptValidity(requestdto);
            
            if (result.Count>0)
            {
                return Json(result);
            }
            else
            {
                return Json(null);


            }
        }


    }
}
