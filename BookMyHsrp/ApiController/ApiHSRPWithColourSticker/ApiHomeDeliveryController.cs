using BookMyHsrp.Libraries.ResponseWrapper.Models;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.ApiController.ApiHSRPWithColourSticker
{
    [ApiController]
    [Route("api/v1/homeDelivery")]
    public class ApiHomeDeliveryController : Controller     
    {
        private readonly ILogger<ApiHomeDeliveryController> _logger;

        private readonly HsrpWithColorStickerConnector _hsrpWithColorStickerConnector;
        public ApiHomeDeliveryController(ILogger<ApiHomeDeliveryController> logger,
            HsrpWithColorStickerConnector hsrpWithColorStickerConnector)
        {
            _hsrpWithColorStickerConnector =
                hsrpWithColorStickerConnector ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerConnector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpPost]
        [Route("/checkAvailability")]
        //public async Task<IActionResult> CheckAvailability([FromBody] VahanDetailsDto requestDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new { Error = true, Message = GetModelErrorMessages() });
        //    }

        //    var result = await _hsrpWithColorStickerConnector.VahanInformation(requestDto);
        //    return Ok(
        //          new Response<dynamic>(result, false,
        //              "Data Received."));

        //}
        private string GetModelErrorMessages()
        {
            var errorMessages = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();
            return string.Join(", ", errorMessages);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
