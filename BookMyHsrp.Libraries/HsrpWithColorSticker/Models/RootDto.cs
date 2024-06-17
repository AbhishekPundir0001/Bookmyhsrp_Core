using BookMyHsrp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Models
{
    public class RootDto
    {
            public string StateId { get; set; }
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
    public class PaymentConfirmationResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public PaymentConfirmationResponseData data;
    }
    public class VerifyDetailPaymentResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string SuperTagApplyed { get; set; }
        public string SuperTagMessage { get; set; }
        public string FrameApplyed { get; set; }
        public VerifyDetailPaymentData data { get; set; }
    }
    public class PaymentConfirmationResponseData
    {
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string VehicleRegNo { get; set; }
        public string TrackingID { get; set; }
        public string PaymentStatus { get; set; }
        public string Amount { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }
        public string AppointmentDateTime { get; set; }
        public string Validity { get; set; }
        public string FitmentCenterName { get; set; }
        public string FitmentAddress { get; set; }
        public string FitmentPersonName { get; set; }
        public string FitmentPersonMobile { get; set; }
        public string AppointmentType { get; set; }
        public string SuperTagAmt { get; set; }
        public string FrameTagAmt { get; set; }
        public string ReceiptPath { get; set; }
    }


    public class VerifyDetailPaymentData
    {
        public string BMHHomeCharges { get; set; }
        public string GstBasicAmtWithFittmentCharges { get; set; }
        public string FastTagBasicAmt { get; set; }
        public string FrameBasicAmt { get; set; }
        public string BMHConvenienceCharges { get; set; }
        public string BMHHomeCharge { get; set; }

        public string GrossTotal { get; set; }

        //public string GSTAmount = "0.00";
        public string IGSTAmount { get; set; }
        public string CGSTAmount { get; set; }
        public string SGSTAmount { get; set; }
        public string TotalAmount { get; set; }

    }
}
