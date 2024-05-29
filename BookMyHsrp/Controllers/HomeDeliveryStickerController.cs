using BookMyHsrp.Libraries.HomeDelivery.Services;
using BookMyHsrp.Libraries.HomeDeliverySticker.Services;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HomeDelivery.Models.HomeDeliveryModel;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;
using BookMyHsrp.Libraries.ResponseWrapper.Models;

namespace BookMyHsrp.Controllers
{
    public class HomeDeliveryStickerController : Controller
    {

        private readonly ILogger<HomeDeliveryStickerController> _logger;

        private readonly IHomeDeliveryStickerService _homeDeliveryService;
        public HomeDeliveryStickerController(IHomeDeliveryStickerService homeDeliveryService, ILogger<HomeDeliveryStickerController> logger)
        {
            _homeDeliveryService = homeDeliveryService ?? throw new ArgumentNullException(nameof(homeDeliveryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("/homedeliverysticker")]
        public IActionResult HomeDeliverySticker()
        {
            return View();
        }

        [HttpGet]
        [Route("/checkavailibilityforsticker/{pincode}")]
        public async Task<IActionResult> CheckAvalibility(string pincode)
        {
            var check = new CheckAvalibility();
            HttpContext.Session.SetString("MapAddress", "");

            if (pincode.Trim() == "" || pincode == null)
            {
                check.Status = "0";
                check.DeliveryCity = "";
                check.DeliveryState = "";
                check.Message = "Please Enter Delivery Pincode";
            }
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var details = HttpContext.Session.GetString("UserDetail");
            var data = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(details);
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            if (vehicledetails.OemId != null && vehicledetails.StateId != null)
            {
                var result = await IsHomeDeliveryAllowed(Convert.ToInt32(vehicledetails.StateId), "Home Delivery");
                if (result == false)
                {
                    check.Status = "0";
                    check.Message = "Not Available";
                    check.DeliveryCity = "";
                    check.DeliveryState = "";
                    return Json(check);
                }
                else
                {
                    var checkpincode = await _homeDeliveryService.CheckPinCode(vehicledetails, pincode);
                    if (checkpincode.Count > 0)
                    {
                        var stateId = vehicledetails.StateId;
                        int stateIdNew = 0;
                        var DealerAffixationID = "";
                        var StateName = "";
                        var StateShortName = "";
                        var DealerAffixationCenterCity = "";
                        var hsrpstate = "";
                        foreach (var state in checkpincode)
                        {
                            stateIdNew = state.StateID;

                            StateName = state.StateName;
                            DealerAffixationCenterCity = state.DealerAffixationCenterCity;
                            hsrpstate = state.hsrpstate;
                        }
                        if (stateId != checkpincode[0].StateId)
                        {

                            var resultCheck = await IsHomeDeliveryAllowed(stateIdNew, "Home Delivery");
                            if (resultCheck == false)
                            {
                                check.Status = "0";
                                check.Message = "Not Available";
                                check.DeliveryCity = "";
                                check.DeliveryState = "";
                                return Json(check);

                            }

                        }

                        check.StateId = stateIdNew;
                        check.DeliveryPoint = "Home";
                        check.StateName = StateName;
                        check.StateShortName = StateShortName;
                        check.Status = "1";
                        check.Message = "Available";
                        check.DeliveryCity = DealerAffixationCenterCity;
                        check.DeliveryState = hsrpstate;
                        check.Pincode = pincode;
                    }
                    else
                    {
                        check.Status = "0";
                        check.Message = "Not Available";
                        check.DeliveryCity = "";
                        check.DeliveryState = "";
                        return Json(check);

                    }
                }
            }
            else
            {

                check.Status = "0";
                check.Message = "Session Expires..";
                check.DeliveryCity = "";
                check.DeliveryState = "";
                return Json(check);

            }

            return Json(check);

        }

        [Route("/update-for-avalibility-sticker")]
        [HttpPost]
        public async Task<IActionResult> UpdateAvailibility([FromBody] UpdateAvalibility updateAvalibility)
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);


            var result = await _homeDeliveryService.UpdateAvailibility(updateAvalibility, vehicledetails, userdetails);
            return Ok(new Response<dynamic>(result.Message, false, ""));

        }

        public async Task<dynamic> IsHomeDeliveryAllowed(int stateId, string CheckFor)
        {
            bool IsAllow = false;
            try
            {
                var Result = await _homeDeliveryService.IsHomeDeliveryAllowed(stateId, CheckFor);
                var checking = "";
                foreach (var check in Result)
                {
                    checking = check.HomeDeliveryAllowed;
                }
                if (checking.ToString().ToUpper().Trim() == "Y")
                {
                    IsAllow = true;
                }
            }
            catch (Exception ex)
            {

                return IsAllow;
            }
            return IsAllow;
        }

    }
}
