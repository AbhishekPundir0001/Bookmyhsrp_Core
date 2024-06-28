using BookMyHsrp.ReportsLogics.DealerDelivery;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.DealerDelivery.Models.DealerDeliveryModel;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
namespace BookMyHsrp.Controllers
{
    public class DealerDeliveryStickerController : Controller
    {
        private readonly ILogger<DealerDeliveryStickerController> _logger;
        private readonly DealerDeliveryStickerConnector _dealerDeliveryStickerConnector;
        public DealerDeliveryStickerController(ILogger<DealerDeliveryStickerController> logger, DealerDeliveryStickerConnector dealerDeliveryStickerConnector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dealerDeliveryStickerConnector = dealerDeliveryStickerConnector;
        }
        [Route("/dealerDeliverySticker")]
        public IActionResult DealerDeliverySticker()
        {

            return View();
        }

        [Route("/bind-dealers-sticker")]
        [HttpGet]
        public async Task<IActionResult> BindDealers()
        {
            var jsonSerializer = "";
            var userDetail = HttpContext.Session.GetString("UserDetail");
            var sessiondata = HttpContext.Session.GetString("UserSession");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(userDetail);
            var sessionDetail = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(sessiondata);
            vehicledetails.DeliveryPoint = "Dealer";
            var stateId = sessionDetail.StateId;
            var setdata = new SetSessionDealer();
            setdata.data = new DealerData();
            setdata.dealerAppointment = new List<DealerAppointmentData>();
            var result = await _dealerDeliveryStickerConnector.GetDealersData(vehicledetails, sessionDetail);
            if (result.Status != 0)
            {

                setdata.data.TotalAmountWithGST = result.data.TotalAmountWithGST;
                setdata.data.Message = result.Message;
                setdata.data.FrontPlateSize = result.data.FrontPlateSize;
                setdata.data.TotalAmountWithGST = result.data.TotalAmountWithGST;
                setdata.data.FrontPlateSize = result.data.FrontPlateSize;
                setdata.data.RearPlateSize = result.data.RearPlateSize;
                setdata.data.GstBasicAmount = result.data.GstBasicAmount;
                setdata.data.FittmentCharges = result.data.FittmentCharges;
                setdata.data.BMHConvenienceCharges = result.data.BMHConvenienceCharges;
                setdata.data.BMHHomeCharges = result.data.BMHHomeCharges;
                setdata.data.MRDCharges = result.data.MRDCharges;
                setdata.data.Message = result.data.Message;
                setdata.data.GrossTotal = result.data.GrossTotal;
                setdata.data.GST = result.data.GST;
                setdata.data.IGSTAmount = result.data.IGSTAmount;
                setdata.data.CGSTAmount = result.data.CGSTAmount;
                setdata.data.SGSTAmount = result.data.SGSTAmount;
                setdata.data.TotalAmount = result.data.TotalAmount;
                foreach (var item in result.dealerAppointment)
                {
                    var appointment = new DealerAppointmentData();
                    appointment.DealerAffixationID = item.DealerAffixationID;
                    appointment.Dealerid = item.Dealerid;
                    appointment.DealerName = item.DealerName;
                    appointment.DealerAffixationCenterName = item.DealerAffixationCenterName;
                    appointment.City = item.City;
                    appointment.DealerAffixationCenterContactNo = item.DealerAffixationCenterContactNo;
                    appointment.Pincode = item.Pincode;
                    appointment.Country = item.Country;
                    appointment.StateName = item.StateName;
                    appointment.WebsiteId = item.WebsiteId;
                    appointment.DealerAffixationCenterLat = item.DealerAffixationCenterLat;
                    appointment.DealerAffixationCenterLon = item.DealerAffixationCenterLon;
                    appointment.RoundOff_netamount = item.RoundOff_netamount;
                    appointment.EarliestDateAvailable = item.EarliestDateAvailable;
                    appointment.EarliestTimeSlotAvailable = item.EarliestTimeSlotAvailable;
                    appointment.Address = item.Address;
                    appointment.cnt = item.cnt;
                    appointment.Distance = item.Distance;
                    setdata.dealerAppointment.Add(appointment);
                }

                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(setdata);
            }
            if (jsonSerializer != null)
            {
                HttpContext.Session.SetString("DealerDetails", jsonSerializer);
                return Json(setdata);
            }
            else
            {

                return BadRequest(new { Error = true, Message = result.message });
            }
        }



    }
}
