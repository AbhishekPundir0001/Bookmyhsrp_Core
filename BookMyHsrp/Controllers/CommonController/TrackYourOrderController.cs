using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.ReceiptValidity.Models;
using BookMyHsrp.Libraries.ReceiptValidity.Services;
using BookMyHsrp.Libraries.TrackYoutOrder.Models;
using BookMyHsrp.Libraries.TrackYoutOrder.Services;
using BookMyHsrp.ReportsLogics.Receipt;
using BookMyHsrp.ReportsLogics.TrackYourOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.TrackYoutOrder.Models.TrackYourOrderModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class TrackYourOrderController : Controller
    {
        public readonly TrackYourOrderConnector _trackyourorderconnector;
        public TrackYourOrderController(TrackYourOrderConnector trackyourorderconnector)
        {
            _trackyourorderconnector = trackyourorderconnector ?? throw new ArgumentNullException(nameof(TrackYourOrderConnector));

        }

        [Route("/trackyourorder/TrackYourOrder")]
        public IActionResult TrackYourOrder()
        {
            return View();
        }

        [Route("/track-your-order")]
        [HttpPost]
        public async Task<IActionResult> TrackYourOrder([FromBody] TrackYourOrder requestdto)
        {
            var response = new ResponseDto();
            var result = await _trackyourorderconnector.TrackYourOrder(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if(result!=null)
            {
                return Ok(result);
            }
            else
            {
                return Json("Not Found");
            }
            
        }
        [Route("/track-your-order-sp")]
        [HttpPost]
        public async Task<IActionResult> SpTrackYourOrder([FromBody] TrackYourOrder requestdto)
        {
            var response = new ResponseDto();
            var result = await _trackyourorderconnector.SpTrackYourOrder(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Json("Not Found");
            }

        }

        [Route("/fitment-date")]
        [HttpPost][HttpPost]
        public async Task<IActionResult> FitmentDate([FromBody] TrackYourOrderModel.TrackYourOrder requestdto)
        {
            var response = new ResponseDto();
            var result = await _trackyourorderconnector.FitmentDate(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)

            return Ok(result);
        }
        [Route("/dealer-name")]
        [HttpPost]
        public async Task<IActionResult> DealerName([FromBody] TrackYourOrderModel.TrackYourOrder requestdto)
        {
            var response = new ResponseDto();
            var result = await _trackyourorderconnector.DealerName(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if (result != null)
            {
                return Ok(result);
            }
            else
            {

                return Json("Not Found");

            }

        }

    }
}
