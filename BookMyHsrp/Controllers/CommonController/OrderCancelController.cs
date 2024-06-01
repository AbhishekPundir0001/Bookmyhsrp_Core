using BookMyHsrp.Libraries.GenerateOtp.Services;
using BookMyHsrp.Libraries.OrderCancel.Models;
using BookMyHsrp.Libraries.OrderCancel.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Services;
using BookMyHsrp.Libraries.TrackYoutOrder.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Reflection;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class OrderCancelController : Controller
    {

        private readonly IGenerateOtpService _generateOtpService;
        private readonly IOrderCancelServices _orderCancelServices;

        public OrderCancelController(IGenerateOtpService generateOtpService, IOrderCancelServices orderCancelServices)
        {
            _generateOtpService = generateOtpService;
            _orderCancelServices = orderCancelServices;
        }

        [Route("/ordercancelOTP")]
        [HttpPost]
        public async Task<IActionResult> GenerateOtpOrderCancel([FromBody] OrderCancelModel.OtpModal requestdto)
        {
            var mobile = requestdto.MobileNo;
            var resultGot = await _generateOtpService.GenerateOtp(mobile, requestdto);
            return resultGot;
        }

        [Route("/ordercancel")]
        public IActionResult OrderCancel()
        {
            return View();
        }
        [Route("/DealerWalletdetail")]
        [HttpPost]
        public async Task<IActionResult> DealerName([FromBody] OrderCancelModel.OrderCancel  requestdto)
        {
            var result = await _orderCancelServices.DealerWalletdetail(requestdto);

            if (result.Count > 0 && result != null)
            {
                var input = new
                {
                    isAbleToCancelled = "N",
                    ORDER_NUMBER = requestdto.OrderNo,
                    REG_NUMBER = requestdto.VehicleregNo,
                    MOB_NUMBER = "",
                    OrderStatus = "Success",
                    Emailid = ""
                };
                return Ok(input);
            }
            else
            {
                var cancelorderget = await _orderCancelServices.CancelOrderGet(requestdto);
                if (cancelorderget.Count > 0)
                {
                    return Ok(cancelorderget);
                }
                else
                {

                    throw new Exception("data not found");

                }

            }

        }
     
        [Route("/DealerAddress")]
        [HttpPost]
        public async Task<IActionResult> CancelOrderDetails([FromBody] OrderCancelModel.OrderCancel requestdto)
        {
            var result = await _orderCancelServices.DealerAddress(requestdto);
            // var result = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {

                throw new Exception("data not found");

            }

        }
    

        [Route("/CancelOrderDetails")]
        [HttpPost]
        public async Task<IActionResult> CancelOrderGet([FromBody] OrderCancelModel.OrderCancel requestdto)
        {
            var result = await _orderCancelServices.CancelOrderDetails(requestdto);
            // var result = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if (result != null)
            {
                return Ok(result);
            }
            else
            {

                throw new Exception("data not found");

            }

        }



    }
}
