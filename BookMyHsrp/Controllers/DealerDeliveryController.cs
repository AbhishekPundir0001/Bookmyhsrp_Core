using BookMyHsrp.Libraries.HomeDelivery.Services;
using BookMyHsrp.ReportsLogics.AppointmentSlot;
using BookMyHsrp.ReportsLogics.DealerDelivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
using static BookMyHsrp.Libraries.DealerDelivery.Models.DealerDeliveryModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookMyHsrp.Controllers
{
    public class DealerDeliveryController : Controller
    {
        private readonly ILogger<DealerDeliveryController> _logger;
        private readonly DealerDeliveryConnector _dealerDeliveryConnector;
        public DealerDeliveryController(ILogger<DealerDeliveryController> logger, DealerDeliveryConnector dealerDeliveryConnector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dealerDeliveryConnector = dealerDeliveryConnector;
        }
        [Route("/dealerDelivery")]
        public IActionResult DealerDelivery()
        {
          
            return View();
        }
        [Route("/get-stateId")]
        [HttpGet]
        public IActionResult GetStateId()
        {
            var sessiondata = HttpContext.Session.GetString("UserSession");
            var sessiondata1 = HttpContext.Session.GetString("UserDetail");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(sessiondata);
            var stateId = vehicledetails.StateId;
            return Json(stateId);
        }
        [Route("/bind-dealers")]
        [HttpGet]
        public async Task<IActionResult> BindDealers()
        {
            var jsonSerializer = "";
            var userDetail = HttpContext.Session.GetString("UserDetail");
            var sessiondata = HttpContext.Session.GetString("UserSession");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(userDetail);
            var sessionDetail = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(sessiondata);
            vehicledetails.DeliveryPoint="Dealer";
            var stateId = sessionDetail.StateId;
            var setdata = new SetSessionDealer();
            setdata.data = new DealerData();
            setdata.dealerAppointment = new List<DealerAppointmentData>();
            var result =await _dealerDeliveryConnector.GetDealersData(vehicledetails, sessionDetail);
            if(result.Status!=0)
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
                setdata.data.BMHHomeCharges = result.data.BMHHomeCharges ;
                setdata.data.MRDCharges = result.data.MRDCharges ;
                setdata.data.Message = result.data.Message ;
                setdata.data.GrossTotal = result.data.GrossTotal ;
                setdata.data.GST = result.data.GST ;
                setdata.data.IGSTAmount = result.data.IGSTAmount ;
                setdata.data.CGSTAmount = result.data.CGSTAmount;
                setdata.data.SGSTAmount = result.data.SGSTAmount;
                setdata.data.TotalAmount = result.data.TotalAmount ;
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
            if(jsonSerializer!=null)
            {
                HttpContext.Session.SetString("DealerDetails", jsonSerializer);
                return Json(setdata);
            }
            else
            {

                return BadRequest(new { Error = true, Message = result.message });
            }
        }
        [HttpGet]
        [Route("check-affixation-Id/{Id}")]
        public async Task<IActionResult> AppointmentSlot(string Id)
        {

            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var jsonSerializer = "";
            var response = new DealerAppointmentResponseLatestDate();
            if (HttpContext.Session.GetString("UserDetail") != null)
            {
                var dealerRequestData = new DealerAppointmentRequestData();
                {
                    dealerRequestData.StateID = vehicledetails.StateId;
                    dealerRequestData.OemID = vehicledetails.OemId;
                    dealerRequestData.VehicleCat = vehicledetails.VehicleCategory;
                    dealerRequestData.VehicleType = vehicledetails.VehicleType;
                    dealerRequestData.VehicleClass = vehicledetails.VehicleClass;
                    dealerRequestData.Vehiclecategoryid = vehicledetails.VehicleCategoryId;
                    dealerRequestData.VehicleFuelType = vehicledetails.FuelType;
                    dealerRequestData.DeliveryPoint = vehicledetails.DeliveryPoint;
                    dealerRequestData.StateName = vehicledetails.StateName;
                    dealerRequestData.CustomerState = vehicledetails.StateName;
                    dealerRequestData.PlateSticker = vehicledetails.PlateSticker;
                    dealerRequestData.NonHomo = vehicledetails.NonHomo;
                    dealerRequestData.VehicleTypeID = userdetails.VehicleTypeId;
                    dealerRequestData.PlateOrderType = vehicledetails.OrderType;
                    dealerRequestData.ReplacementType = vehicledetails.ReplacementType;

                }
                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(dealerRequestData);
                var result = await _dealerDeliveryConnector.CheckAppointmentSlot(jsonSerializer, Id);
                var setSession = new AppointmentSlotSessionResponse()
                {
                    Message = result.Message,
                    DealerAffixationCenterId = result.DealerAffixationCenterId,
                    SelectedSlotID = result.SelectedSlotID,
                    SelectedSlotDate = result.SelectedSlotDate,
                    SelectedSlotTime = result.SelectedSlotTime,
                    Affix = result.Affix,
                    DeliveryPoint = "Dealer",
                    DealerAffixationCenterContactPerson = result.DealerAffixationCenterContactPerson,
                    DealerAffixationCenterContactNo=result.DealerAffixationCenterContactNo
                };
                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(setSession);
                HttpContext.Session.SetString("AppointmentSlotId", jsonSerializer);



            }

            return Json(jsonSerializer);
        }
    }
}
