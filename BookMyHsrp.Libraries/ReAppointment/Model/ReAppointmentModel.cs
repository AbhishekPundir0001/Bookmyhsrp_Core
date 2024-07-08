using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReAppointment.Model
{
    public class ReAppointmentModel
    {
        public class ReAppointment
        {
            public string OrderNo { get; set; } = "";
            public string VehicleregNo { get; set; } = "";

        }
        public class AppointmentSlotSessionResponse
        {
            public string Message { get; set; }
            public string DealerAffixationCenterId { get; set; }
            public string SelectedSlotID { get; set; }
            public string SelectedSlotDate { get; set; }
            public string SelectedSlotTime { get; set; }
            public string Affix { get; set; }
            public string DeliveryPoint { get; set; }
            public string DealerAffixationCenterContactPerson { get; set; }
            public string DealerAffixationCenterContactNo { get; set; }
        }
        public class Message
        {
            public string Status { get; set; } = "";
            public string message { get; set; } = "";

        }

        public class RootDto
        {
            public string StateId { get; set; } = "";
            public string RegistrationDate { get; set; }
            public string Message { get; set; }
            public string PlateSticker { get; set; }
            public string VehicleRegNo { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string StateIdBackup { get; set; }
            public string FrontLaserCode { get; set; }
            public string RearLaserCode { get; set; }
            public string FrontLaserFileName { get; set; }
            public string RearLaserFileName { get; set; }
            public string FrontPlateFileName { get; set; }
            public string RearPlateFileName { get; set; }

            public string Fuel { get; set; }
            public string Maker { get; set; }
            public string NonHomo { get; set; }
            public string Norms { get; set; }
            public string OemImgPath { get; set; }
            public string UploadFlag { get; set; }
            public string OemId { get; set; }
            public string StateName { get; set; }
            public string StateShortName { get; set; }
            public string VehRegDate { get; set; }
            public string VehicleCategory { get; set; }
            public string VehicleClass { get; set; }
            public string VehicleType { get; set; }
            public string VehicleCat { get; set; }
            public string VehicleTypeId { get; set; }
            public string VehicleCategoryId { get; set; }
            public string OrderType { get; set; }
            public string BhartStage { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerMobile { get; set; }
            public string CustomerBillingAddress { get; set; }
            public string CustomerState { get; set; }
            public string CustomerCity { get; set; }
            public string CustomerGstin { get; set; }
            public string RcFile { get; set; }
            public string FuelType { get; set; }
            public string DeliveryPoint { get; set; }
            public string Status { get; set; }


            //Shipping information for Home delivery
            public string ShippingAddress1 { get; set; } = "";
            public string ShippingAddress2 { get; set; } = "";
            public string ShippingCity { get; set; } = "";
            public string ShippingState { get; set; } = "";
            public string ShippingPinCode { get; set; } = "";
            public string ShippingLandMark { get; set; } = "";

            public string DealerAffixationCenterID { get; set; }
            public string DealerAffixationAddress { get; set; }
            public string SelectedDate { get; set; }
            public string SelectedTimeID { get; set; }
            public string SelectedTime { get; set; }


            public string RazorpayPaymentId { get; set; }
            public string RazorpayOrderId { get; set; }
            public string RazorPaySignature { get; set; }
            public string RazorpayNetAmount { get; set; }
            public string HSRPOrderNo { get; set; }
            public VerifyDetailPaymentResponse paymentValidateResponse { get; set; }


            public string OrderStatus { get; set; }
            public string FitmentName { get; set; }
            public string FitmentAddress { get; set; }
            public string ReceiptPath { get; set; }
            public List<ReAppointmentData> reAppointmentData { get; set; }

            public bool IsReplacement { get; set; } = false;
            public string FirNumber { get; set; } = "";
            public string FirInfo { get; set; } = "";
            public string FirDate { get; set; } = "";
            public string FrontLaserPath { get; set; } = "";
            public string RearLaserPath { get; set; } = "";
            public string FirCopyName { get; set; } = "";
            public string PoliceStation { get; set; } = "";
            public string File4 { get; set; } = "";
            public string PlateOrderType { get; set; } = "OB";

            public string ReplacementReason { get; set; } = "";
            public string ReplacementType { get; set; } = "OB";


            public PaymentConfirmationResponse paymentConfirmationResponse { get; set; }
            public bool IsExpressCheckout { get; set; } = false;

        }
        public class RootDtoReAppointment
        {
            public string ReOrderNo { get; set; } = "";
            public string PlateSticker { get; set; } = "";
            public string VehicleRegNo { get; set; } = "";
            public string OldAppointmentDate { get; set; } = "";
            public string OldAppointmentSlot { get; set; } = "";
            public string ReOEMId { get; set; } = "";
            public int ReDealerAffixationCenterid { get; set; } 
            public string ReSessionOwnerName { get; set; } = "";
            public string ReSessionMobileNo { get; set; } = "";
            public string ReSessionBillingAddress { get; set; } = "";
            public string ReSessionEmailID { get; set; } = "";
            public string ReStateName { get; set; } = "";
            public string ReVehicleTypeid { get; set; } = "";
            public string ReDeliveryPoint { get; set; } = "";
            public string ReStateId { get; set; } = "";
            public string BookingType { get; set; } = "";
            public string ReDealerAffixationCenteridid { get; set; } = "";
            public string ReDealerAffixationCenterName { get; set; } = "";

        }

    }
}