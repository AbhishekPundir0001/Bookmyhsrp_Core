using BookMyHsrp.ApiController.ApiHSRPWithColourSticker;
using BookMyHsrp.Libraries.HomeDelivery.Services;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using static BookMyHsrp.Libraries.HomeDelivery.Models.HomeDeliveryModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers
{
    public class HomeDeliveryController : Controller
    {
        private readonly ILogger<HomeDeliveryController> _logger;

        private readonly HomeDeliveryService _homeDeliveryService;
        public HomeDeliveryController(HomeDeliveryService homeDeliveryService, ILogger<HomeDeliveryController> logger)
        {
            _homeDeliveryService = homeDeliveryService ?? throw new ArgumentNullException(nameof(homeDeliveryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [Route("/homedelivery")]
        public IActionResult HomeDelivery()
        {
            return View();
        }
        [Route("/checkavailibility/{pincode}")]
        [HttpGet]
        public async Task<IActionResult> CheckAvalibility(string pincode)
        {
            var check = new CheckAvalibility();
            HttpContext.Session.SetString("MapAddress", "");
            HttpContext.Session.SetString("Affix", "");

            if (pincode.Trim() == "" || pincode == null)
            {
                check.Status = "0";
                check.DeliveryCity = "";
                check.DeliveryState = "";
                check.Message= "Please Enter Delivery Pincode";
            }
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var details = HttpContext.Session.GetString("UserDetail");
            var data = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(details);
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            if(vehicledetails.OemId!=null && vehicledetails.StateId!=null)
            {
                var result = await IsHomeDeliveryAllowed(Convert.ToInt32(vehicledetails.StateId), "Home Delivery");
                if(result==false)
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
                    if(checkpincode.Count>0)
                    {
                        var stateId = vehicledetails.StateId;
                        if(stateId != checkpincode[0].StateId)
                        {
                            var resultCheck = await IsHomeDeliveryAllowed(Convert.ToInt32(checkpincode[0].StateId), "Home Delivery");
                            if(resultCheck == false)
                            {
                                check.Status = "0";
                                check.Message = "Not Available";
                                check.DeliveryCity = "";
                                check.DeliveryState = "";
                                return Json(check);

                            }

                        }

                        check.StateId = checkpincode[0].StateId;
                        check.DealerAffixationCenterId = checkpincode[0].DealerAffixationID;
                        check.DeliveryPoint ="Home";
                        check.StateName = checkpincode[0].StateName;
                        check.StateShortName = checkpincode[0].StateShortName;
                        check.Status = "1";
                        check.Message = "Available";
                        check.DeliveryCity = checkpincode[0].DealerAffixationCenterCity;
                        check.DeliveryState = checkpincode[0].hsrpstate;
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
        public  async Task<dynamic> IsHomeDeliveryAllowed(int stateId, string CheckFor)
        {
            bool IsAllow = false;
            try
            {
                var Result = await _homeDeliveryService.IsHomeDeliveryAllowed(stateId, CheckFor); 
                if (Result.ToString().ToUpper().Trim() == "Y")
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
