using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using BookMyHsrp.Libraries.Sticker.Services;
using BookMyHsrp.Libraries.TrackYoutOrder.Models;
using BookMyHsrp.Libraries.TrackYoutOrder.Services;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.TrackYoutOrder.Models.TrackYourOrderModel;

namespace BookMyHsrp.ReportsLogics.TrackYourOrder
{
    public class TrackYourOrderConnector
    {
        private readonly ITrackYourOrderService _trackYourOrderService;

        public TrackYourOrderConnector(ITrackYourOrderService trackYourOrderService)
        {
            _trackYourOrderService = trackYourOrderService ?? throw new ArgumentNullException(nameof(trackYourOrderService));
        }

          
            public async Task<dynamic> TrackYourOrder(dynamic requestdto)
            {
                
                var result = await _trackYourOrderService.GetTrackYourOrderStatus(requestdto);
            if (result.Count> 0)
            {
                return result;

            }
            else
            {
                return null;
                
            }

        }
        public async Task<dynamic> FitmentDate([FromBody] TrackYourOrderModel.TrackYourOrder requestdto)
        {
            var response = new ResponseDto();
            var result = await _trackYourOrderService.GetFitmentDate(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            return result;
        }

        public async Task<dynamic> SpTrackYourOrder([FromBody] TrackYourOrderModel.TrackYourOrder requestdto)
        {
            var response = new ResponseDto();
            var result = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if(result.Count > 0)
            {
                return result;

            }
            else
            {
                return null;

            }


        }
        public async Task<dynamic> DealerName([FromBody] GetDealerId requestdto)
            {
                var response = new ResponseDto();
                var result = await _trackYourOrderService.GetDealerName(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            if (result.Count > 0)
            {
                return result;

            }
            else
            {
                return null;

            }

        }

    }
}
