using BookMyHsrp.Libraries.GenerateOtp.Services;
using BookMyHsrp.Libraries.OrderCancel.Models;
using BookMyHsrp.Libraries.OrderCancel.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Services;
using BookMyHsrp.Libraries.TrackYoutOrder.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net.NetworkInformation;
using System.Reflection;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.OrderCancel.Models.OrderCancelModel;
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
        public async Task<IActionResult> CancelOrderDetails(string Dealerid)
        {
            var result = await _orderCancelServices.DealerAddress(Dealerid);
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


        [Route("/OrderCancelConfirmation")]
        [HttpPost]
        public async Task<IActionResult> OrderCancelConfirmation([FromBody] OrderCancelModel.OrderCancelReason requestdto)
        {
            var hsrporderstatus = await _orderCancelServices.OrderStatusCancel(requestdto);
            var HsrpOrderStatus = "";

            if (hsrporderstatus.Count > 0)
            {
                HsrpOrderStatus = hsrporderstatus[0].OrderStatus;
            }
        
            if (HsrpOrderStatus == "New Order" || HsrpOrderStatus == "")
            {
                try
                {
                    var query = await _orderCancelServices.checkcancelrecord(requestdto);
                    if (query.Count > 0)
                    {
                        var result = await _orderCancelServices.CancelOrderDetails(requestdto);
                        var dealerid = result[0].Dealerid;
                        var _appointmenttype = result[0].AppointmentType;
                        var FullAddress = string.Empty;
                        if (_appointmenttype == "Home")
                        {
                            FullAddress = result[0]["ShippingAddress1"] + " " + result[0]["ShippingAddress2"] + " " + result[0]["ShippingCity"] + " " + result[0]["ShippingState"] + " " + result[0]["ShippingPinCode"];
                        }
                        else
                        {
                            var dealeraddress = await _orderCancelServices.DealerAddress(dealerid);
                            var dAddress = dealeraddress[0].DealerAffixationCenterName;
                            var dCity = dealeraddress[0].DealerAffixationCenterAddress;
                            var dPincode = dealeraddress[0].DealerAffixationCenterPinCode;
                            FullAddress = dAddress + " " + dCity + " " + dPincode;

                        }
                        logDto logDto = new logDto
                        {
                            OrderNo = result[0].OrderNo,
                            VehicleNo = result[0].VehicleNo,
                            AppointmentSlot = result[0].SlotBookingDateNew,
                            EngineNo = result[0].EngineNo,
                            ChassisNo = result[0].ChassisNo,
                            VehicleMake = result[0].ManufacturerName,
                            VehicleClass = result[0].VehicleClass,
                            VehicleType = result[0].VehicleType,
                            FuelType = result[0].fuelType,
                            FitmentAddress = FullAddress,
                            Reason = requestdto.Reason
                        };

                        await _orderCancelServices.updatecancelledlog(logDto);
                    }
                    else
                    {
                        var result = await _orderCancelServices.CancelOrderDetails(requestdto);
                        dynamic dealerid = result[0].Dealerid;
                        var _appointmenttype = result[0].AppointmentType;
                        string FullAddress = string.Empty;
                        if (_appointmenttype == "Home")
                        {
                            FullAddress = result[0]["ShippingAddress1"] + " " + result[0]["ShippingAddress2"] + " " + result[0]["ShippingCity"] + " " + result[0]["ShippingState"] + " " + result[0]["ShippingPinCode"];
                        }
                        else
                        {
                            var dealeraddress = await _orderCancelServices.DealerAddress(dealerid);
                            var dAddress = dealeraddress[0].DealerAffixationCenterName;
                            var dCity = dealeraddress[0].DealerAffixationCenterAddress;
                            var dPincode = dealeraddress[0].DealerAffixationCenterPinCode;
                            FullAddress = dAddress + " " + dCity + " " + dPincode;

                        }
                        logDto1 logDto1 = new logDto1
                        {
                            OrderNo = result[0].OrderNo,
                            VehicleNo = result[0].VehicleRegNo,
                            AppointmentSlot = result[0].SlotBookingDateNew,
                            AppointmentDate = result[0].AppointmentDate,
                            EngineNo = result[0].EngineNo,
                            ChassisNo = result[0].ChassisNo,
                            VehicleMake = result[0].ManufacturerName,
                            VehicleClass = result[0].VehicleClass,
                            VehicleType = result[0].VehicleType,
                            FuelType = result[0].fuelType,
                            FitmentAddress = FullAddress,
                            Reason = requestdto.Reason
                        };

                        await _orderCancelServices.updatecancelledlog2(logDto1);

                    }
                    await _orderCancelServices.voidOrder(requestdto);
                    await _orderCancelServices.OrderStatusUpdate(requestdto);

                   var checkbook =  await _orderCancelServices.checkBookApp(requestdto);
                    if(checkbook.Count > 0)
                    {
                        var updatecheckbook = await _orderCancelServices.updateBookApp(requestdto);
                    }
                    var Smsqry = await _orderCancelServices.Smsqry(requestdto);
                    if (Smsqry.Count > 0)
                    {
                        string qry = "Your order has been successfully cancelled. Amount will be refunded with in 7 working days.\n(Team Rosmerta)";
                        var SMSResponse =await _generateOtpService.OrdercancelSMSSend(Smsqry[0].LandLineNo, qry, "1007835448341198264", "DLHSRP");
                        await _orderCancelServices.SMSLogSaveQuery(requestdto, Smsqry[0].LandLineNo, qry, SMSResponse);
                    }
                    var finalresult = await _orderCancelServices.cancellationfinalpage(requestdto);
                    return Ok(finalresult);
                }
                catch { Exception e;
                    return Ok("HI");
                }
            }
            else{
                return Ok("HI");
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
        [Route("/CancelOrderStatus")]
        [HttpPost]
        public async Task<IActionResult> OrderStatusCancel([FromBody] OrderCancelModel.OrderCancel requestdto)
        {
            var result = await _orderCancelServices.OrderStatusCancel(requestdto);
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
