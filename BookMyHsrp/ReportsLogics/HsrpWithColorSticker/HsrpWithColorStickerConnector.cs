using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.ReportsLogics.HsrpWithColorSticker
{
    public class HsrpWithColorStickerConnector 
    {
        //private readonly FetchDataAndCache _fetchDataAndCache; // instance of ReportHelper
        private readonly IHsrpWithColorStickerService _hsrpWithColorStickerService;
        public HsrpWithColorStickerConnector(IHsrpWithColorStickerService hsrpWithColorStickerService)
        {
            // _fetchDataAndCache = fetchDataAndCache; // Dependency injection
            _hsrpWithColorStickerService = hsrpWithColorStickerService ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerService));
        }
        public async Task<dynamic> VahanInformation(dynamic requestDto)
        {
            var resultGot = await _hsrpWithColorStickerService.VahanInformation(requestDto);
            return resultGot;
           
        }
        public async Task<dynamic> SessionBookingDetails(dynamic requestDto)
        {
            var resultGot = await _hsrpWithColorStickerService.SessionBookingDetails(requestDto);
            return resultGot;

        }

    }
}
