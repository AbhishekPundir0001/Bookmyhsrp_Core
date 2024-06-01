
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.ResponseWrapper.Models;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.ApiController.ApiHSRPWithColourSticker
{
    [ApiController]
    [Route("api/v1/hsrpWithColorSticker")]
    public class ApiHsrpWithColorStickerController : Controller
    {
        private readonly ILogger<ApiHsrpWithColorStickerController> _logger;

        private readonly HsrpWithColorStickerConnector _hsrpWithColorStickerConnector;
        public ApiHsrpWithColorStickerController(ILogger<ApiHsrpWithColorStickerController> logger,
            HsrpWithColorStickerConnector hsrpWithColorStickerConnector)
        {
            _hsrpWithColorStickerConnector =
                hsrpWithColorStickerConnector ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerConnector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpPost]
        [Route("report/check-vahan-information")]
        
        public async Task<IActionResult> SessionBookingDetails([FromBody] GetSessionBookingDetails requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = true, Message = GetModelErrorMessages() });
            }
            var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(requestDto);
            HttpContext.Session.SetString("SessionDetail", jsonSerializer);
            var result = await _hsrpWithColorStickerConnector.SessionBookingDetails(requestDto);
            if (result.Message == "Vehicle Details didn't match")
            {
                return BadRequest(new { Error = true, result.Message });
            }
            return Ok(
                  new Response<dynamic>(result, false,
                      "Data Received."));

        }

        private string GetModelErrorMessages()
        {
            var errorMessages = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();
            return string.Join(", ", errorMessages);
        }
    }
}
