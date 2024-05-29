using BookMyHsrp.Libraries.DealerDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
using static BookMyHsrp.Libraries.DealerDelivery.Models.DealerDeliveryModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.ReportsLogics.DealerDelivery
{
    public class DealerDeliveryConnector 
    {
        private  readonly IDealerDeliveryService _dealerDeliveryService;
        public DealerDeliveryConnector(IDealerDeliveryService dealerDeliveryService)
        {
            _dealerDeliveryService = dealerDeliveryService ?? throw new ArgumentNullException(nameof(dealerDeliveryService)); ;


        } 
        public async Task<dynamic> GetDealersData(dynamic vehicledetails,dynamic sessiondetails)
        {

            var response = new SetSessionDealer();
            response.data =new DealerData();
            response.dealerAppointment = new List<DealerAppointmentData>();
            var Ordertype = vehicledetails.OrderType==null?"": vehicledetails.OrderType;    
            var OemId= sessiondetails.OemId;
            decimal netamount;
            var StateId= sessiondetails.StateId;
            var VehicleCat= sessiondetails.VehicleCategory;
            if(VehicleCat=="2WN")
            {
                VehicleCat = "2W";
            }
            var VehicleType= vehicledetails.VehicleType;
            var VehicleClass= sessiondetails.VehicleClass;
            
            if (OemId != null && StateId != null && VehicleCat != null && VehicleType != null && VehicleClass != null)
            {
                try
                {
                    //var getDealers = "";
                    if (sessiondetails.StateIdBackup == "27")
                    {
                        if (OemId == "272" && VehicleType == "Scooter_2W")
                        {
                           var getDealers = await _dealerDeliveryService.GetDealersForRajasthan(OemId, StateId, VehicleType, VehicleCat, VehicleClass, Ordertype);
                        }
                        else
                        {
                         var getDealers = await _dealerDeliveryService.GetDealersForRajasthanElse(OemId, StateId, VehicleType, VehicleCat, VehicleClass, Ordertype);

                        }

                    }
                    else
                    {
                        if (OemId == "272" && VehicleType == "Scooter_2W")
                        {
                           var getDealers = await _dealerDeliveryService.GetDealers(OemId, StateId, VehicleType, VehicleCat, VehicleClass, Ordertype);
                        }
                        else
                        {
                           var getDealers = await _dealerDeliveryService.GetDealersElse(OemId, StateId, VehicleType, VehicleCat, VehicleClass, Ordertype);
                            if (getDealers.Count > 0)
                            {
                                response = await CheckOrderType(getDealers, vehicledetails, sessiondetails);
                             }
                        }
                    }
                    //if (int.TryParse(getDealers, out int dealersCount))
                    //{
                    //    if (dealersCount > 0)
                    //    {
                    //        if (vehicledetails.StateIdBackup == "27")
                    //        {
                    //            Ordertype = vehicledetails.Ordertype == null ? "" : vehicledetails.Ordertype;
                    //            var checkOemRate = _dealerDeliveryService.CheckOemRate(Ordertype, VehicleType, vehicledetails.StateIdBackup);
                    //            if (checkOemRate.Count > 0)
                    //            {

                    //                foreach (var data in checkOemRate)
                    //                {
                    //                    netamount = data.NetAmount;
                    //                    response.data.TotalAmountWithGST = netamount.ToString();
                    //                    response.data.FrontPlateSize = data.FrontPlateSize;
                    //                    response.data.RearPlateSize = data.RearPlateSize;
                    //                    response.data.GstBasicAmount = data.GstBasic_Amt;
                    //                    response.data.FittmentCharges = data.FittmentCharges;
                    //                    response.data.BMHConvenienceCharges = data.BMHConvenienceCharges;
                    //                    response.data.BMHHomeCharges = data.BMHHomeCharges;
                    //                    response.data.MRDCharges = data.MRDCharges;
                    //                    response.data.GrossTotal = data.GrossTotal;
                    //                    response.data.GST = data.GST;
                    //                    response.data.IGSTAmount = data.IGSTAmount;
                    //                    response.data.CGSTAmount = data.CGSTAmount;
                    //                    response.data.SGSTAmount = data.SGSTAmount;
                    //                    response.data.TotalAmount = data.TotalAmount;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                response.data.TotalAmountWithGST = "Rate Not Found";
                    //                response.Status = "0";
                    //             //   response.data = getDealers[].FrontSizePlate;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            var checkOemRate = _dealerDeliveryService.CheckOemRateQuery(OemId, Ordertype, VehicleClass, VehicleType, vehicledetails.VehicleCategoryId, vehicledetails.FuelType, vehicledetails.DeliveryPoint, StateId, sessiondetails.StateName);
                    //            if (checkOemRate.Count > 2)
                    //            {
                    //                if (checkOemRate.Count > 0)
                    //                {
                    //                    foreach (var data in checkOemRate)
                    //                    {
                    //                        netamount = checkOemRate.TotalAmount;
                    //                        TotalAmountWithGST = netamount.ToString();
                    //                    }


                    //                }
                    //                else
                    //                {
                    //                    //response.TotalAmountWithGST = "Rate Not Found";
                    //                    response.Status = "0";
                    //                }
                    //            }
                    //            else
                    //            {
                    //               // response.TotalAmountWithGST = "Rate Not Found";
                    //                response.Status = "0";
                    //            }
                    //        }
                    //    }

                    //}
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
               
            }
            return response;
        }
        public async Task<dynamic> CheckOrderType(dynamic dealers,dynamic vehicledetails, dynamic sessiondetails)
        {
            
            var response = new SetSessionDealer();
            response.data = new DealerData();
            response.dealerAppointment= new List<DealerAppointmentData>();
            var Ordertype = vehicledetails.OrderType == null ? "" : vehicledetails.OrderType;
            var OemId = sessiondetails.OemId;
            decimal netamount;
            var StateId = sessiondetails.StateId;
            var VehicleCat = sessiondetails.VehicleCategory;
            var VehicleType = vehicledetails.VehicleType;
            var VehicleClass = sessiondetails.VehicleClass;
            string TotalAmountWithGST = "0.00";
            if (dealers.Count > 0)
            {
                foreach (var dealer in dealers)
                {
                    var appointment = new DealerAppointmentData();
                    appointment.DealerAffixationID = dealer.DealerAffixationID;
                    appointment.Dealerid = dealer.Dealerid;
                    appointment.DealerName = dealer.DealerName;
                    appointment.DealerAffixationCenterName = dealer.DealerAffixationCenterName;
                    appointment.City = dealer.City;
                    appointment.DealerAffixationCenterContactNo = dealer.DealerAffixationCenterContactNo;
                    appointment.Pincode = dealer.Pincode;
                    appointment.Country = dealer.Country;
                    appointment.StateName = dealer.StateName;
                    appointment.WebsiteId = dealer.WebsiteId;
                    appointment.DealerAffixationCenterLat = dealer.DealerAffixationCenterLat;
                    appointment.DealerAffixationCenterLon = dealer.DealerAffixationCenterLon;
                    appointment.RoundOff_netamount = dealer.RoundOff_netamount;
                    appointment.EarliestDateAvailable = dealer.EarliestDateAvailable;
                    appointment.EarliestTimeSlotAvailable = dealer.EarliestTimeSlotAvailable;
                    appointment.cnt = dealer.cnt;
                    appointment.Distance = dealer.Distance;
                    appointment.Address = dealer.Address;
                    response.dealerAppointment.Add(appointment);

                }
            }
            if (vehicledetails.StateIdBackup == "27")
            {
                Ordertype = vehicledetails.Ordertype == null ? "" : vehicledetails.Ordertype;
                var checkOemRate = _dealerDeliveryService.CheckOemRate(Ordertype, VehicleType, vehicledetails.StateIdBackup);
                if (checkOemRate.Count > 0)
                {

                    foreach (var data in checkOemRate)
                    {
                        netamount = data.NetAmount;
                        response.data.TotalAmountWithGST = netamount.ToString();
                        response.data.FrontPlateSize = data.FrontPlateSize;
                        response.data.RearPlateSize = data.RearPlateSize;
                        response.data.GstBasicAmount = data.GstBasic_Amt;
                        response.data.FittmentCharges = data.FittmentCharges;
                        response.data.BMHConvenienceCharges = data.BMHConvenienceCharges;
                        response.data.BMHHomeCharges = data.BMHHomeCharges;
                        response.data.MRDCharges = data.MRDCharges;
                        response.data.GrossTotal = data.GrossTotal;
                        response.data.GST = data.GST;
                        response.data.IGSTAmount = data.IGSTAmount;
                        response.data.CGSTAmount = data.CGSTAmount;
                        response.data.SGSTAmount = data.SGSTAmount;
                        response.data.TotalAmount = data.TotalAmount;
                    }
                }
                else
                {
                    response.data.TotalAmountWithGST = "Rate Not Found";
                    response.Status = "0";
                    //   response.data = getDealers[].FrontSizePlate;
                }
            }
            else
            {
                var checkOemRate = await _dealerDeliveryService.CheckOemRateQuery(OemId, Ordertype, VehicleClass, VehicleType, vehicledetails.VehicleCategoryId, vehicledetails.FuelType, vehicledetails.DeliveryPoint, StateId, sessiondetails.StateName);
                if (checkOemRate.Count > 0)
                {
                    if (checkOemRate.Count > 0)
                    {
                        foreach (var data in checkOemRate)
                        {
                            netamount = data.TotalAmount;
                            response.data.TotalAmountWithGST = netamount.ToString();
                            response.data.FrontPlateSize = data.FrontPlateSize;
                            response.data.RearPlateSize = data.RearPlateSize;
                            response.data.GstBasicAmount = data.GstBasic_Amt;
                            response.data.FittmentCharges = data.FittmentCharges;
                            response.data.BMHConvenienceCharges = data.BMHConvenienceCharges;
                            response.data.BMHHomeCharges = data.BMHHomeCharges;
                            response.data.MRDCharges = data.MRDCharges;
                            response.data.Message = data.message;
                            response.data.GrossTotal = data.GrossTotal;
                            response.data.GST = data.gst;
                            response.data.IGSTAmount = data.IGSTAmount;
                            response.data.CGSTAmount = data.CGSTAmount;
                            response.data.SGSTAmount = data.SGSTAmount;
                            response.data.TotalAmount = data.TotalAmount;
                        }
                       }
                    else
                    {
                        //response.TotalAmountWithGST = "Rate Not Found";
                        response.Status = "0";
                    }
                }
                else
                {
                    // response.TotalAmountWithGST = "Rate Not Found";
                    response.Status = "0";
                }
            }
            return response;
        }
        public async Task<dynamic> CheckAppointmentSlot(dynamic jsonSerializer, string Id)
        {
            var setSession = new AppointmentSlotSessionResponse();
            setSession.DealerAffixationCenterId = Id;
            try
            {
                var getAffixationId = await _dealerDeliveryService.GetAffixationId(Id);
                if (getAffixationId.Count > 0)
                {
                    setSession.SelectedSlotID = "1";
                    setSession.SelectedSlotDate = "1900-01-01";
                    setSession.SelectedSlotTime = "NULL";
                    setSession.Affix = "Affixval";
                    setSession.Message = "Affix";
                }
                else
                {
                    setSession.Message = "Success";
                    setSession.Affix = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return setSession;
        }




    }
}
