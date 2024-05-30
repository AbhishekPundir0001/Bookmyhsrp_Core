using BookMyHsrp.Libraries.OrderCancel.Models;
using BookMyHsrp.Libraries.OrderCancel.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Services;
using BookMyHsrp.Libraries.TrackYoutOrder.Models;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class OrderCancelController(IOrderCancelServices orderCancelServices) : Controller
    {
         
        private readonly IOrderCancelServices _orderCancelService = orderCancelServices ?? throw new ArgumentNullException(nameof(OrderCancelServices));

        [Route("/ordercancel/OrderCancel")]
        public IActionResult OrderCancel()
        {
            return View();
        }
        [Route("/DealerWalletdetail")]
        [HttpPost]
        public async Task<IActionResult> DealerName([FromBody] OrderCancelModel.OrderCancel  requestdto)
        {
            var response = new ResponseDto();
            var result = await _orderCancelService.DealerWalletdetail(requestdto);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {

                throw new Exception("data not found");

            }

        }
        [Route("/CancelOrderGet")]
        [HttpPost]
        public async Task<IActionResult> CancelOrderGet([FromBody] OrderCancelModel.OrderCancel requestdto)
        {
            var response = new ResponseDto();
            var result = await _orderCancelService.CancelOrderGet(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
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
