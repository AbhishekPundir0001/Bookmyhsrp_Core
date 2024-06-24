using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.VerifyPaymentDetail.Models
{
    public class VerifyPaymentDetailModel
    {
        public class GetVerifyPaymentDetailModel
        {
            public bool ParkPlusArea { get; set; }
            public string SuperTagAmount { get; set; }
            public string StateName { get; set; }
            public string HdnGstBasicAmtST { get; set; }
            public decimal BMHHomeCharges { get; set; }
            public string  HdnCGSTAmountST { get; set; }
            public string HdnIGSTAmountST { get; set; }
            public string HdnSGSTAmountST { get; set; }
            public bool DivFrame { get; set; }
            public bool DivSupertag { get; set; }
            public bool CheckShipping { get; set; }
            public bool TrDeliveryCharge { get; set; }
            public bool FrameArea { get; set; }
            public decimal IGSTAmount { get; set; }
            public decimal CGSTAmount { get; set; }
            public decimal MRDCharges { get; set; }
            public decimal SGSTAmount { get; set; }
            public decimal FrontPlateSize { get; set; }
            public decimal RearPlateSize { get; set; }
            public decimal FittmentCharges { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal BMHConvenienceCharges { get; set; }
            public decimal GstBasic_Amt { get; set; }
            public decimal GrossTotal { get; set; }
            public decimal GSTAmount { get; set; }
            public decimal GstRate { get; set; }
            public decimal NetAmount { get; set; }
            public bool HSRPFitmentCost { get; set; }
            public bool HSRPFitmentCostStr { get; set; }
            public bool HSRPFitCostFP { get; set; }
            public bool HSRPFitCostFPStr { get; set; }
            public bool HSRPCompSet { get; set; }
            public bool HSRPCompSetStr { get; set; }
            public bool HSRPFitCost { get; set; }
            public string Message { get; set; }
            public string OwnerName { get; set; }
            public string Mobile { get; set; }
            public string Address { get; set; }
            public string EmailID { get; set; }
            public string PinCode { get; set; }
            public string HdnMRDCharges { get; set; }
        }
        public class PaymentDetails()
        {
            public string  Message { get; set; }
            public string isSuperTag { get; set; }
            public string isFrame { get; set; }
            public string orderNo { get; set; }
            public string Status { get; set; }
            public bool CheckedStatus { get; set; }
            public bool ChkSuperTag { get; set; }
            public bool Checker { get; set; }
            public bool ChkFastTag { get; set; }
            public bool ChkFrame { get; set; }
            public string SlotId { get; set; }
            public string SlotTime { get; set; }
            public string SlotBookingDate { get; set; }
            public string HSRPStateID { get; set; }
            public string RTOLocationID { get; set; }
            public string RTOName { get; set; }
            public string OwnerName { get; set; }
            public string OwnerFatherName { get; set; }
            public string Address1 { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Pin { get; set; }
            public string MobileNo { get; set; }
            public string LandlineNo { get; set; }
            public string EmailID { get; set; }
            public string VehicleClass { get; set; }
            public string VehicleType { get; set; }
            public string ManufacturerName { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string VehicleRegNo { get; set; }
            public string ManufacturingYear { get; set; }
            public string FrontPlateSize { get; set; }
            public string RearPlateSize { get; set; }
            public string TotalAmount { get; set; }
            public string NetAmount { get; set; }
            public string FinalAmount { get; set; }
            public string GrandTotal { get; set; }
            public string SuperTagBasicAmount { get; set; }
            public string FrameBasicAmount { get; set; }
            public string BookingType { get; set; }
            public string BookingClassType { get; set; }
            public string FuelType { get; set; }
            public string DealerId { get; set; }
            public string OEMID { get; set; }
            public string BookedFrom { get; set; }
            public string AppointmentType { get; set; }
            public string BasicAmount { get; set; }
            public string FitmentCharge { get; set; }
            public string ConvenienceFee { get; set; }
            public string HomeDeliveryCharge { get; set; }
            public string GSTAmount { get; set; }
            public string IGSTAmount { get; set; }
            public string FittmentCharges { get; set; }
            public string BMHConvenienceCharges { get; set; }
            public string BMHHomeCharges { get; set; }
            public string CGSTAmount { get; set; }
            public string SGSTAmount { get; set; }
            public string CustomerGSTNo { get; set; }
            public string VehicleRCImage { get; set; }
            public string BharatStage { get; set; }
            public string ShippingAddress1 { get; set; }
            public string ShippingAddress2 { get; set; }
            public string ShippingCity { get; set; }
            public string ShippingState { get; set; }
            public string ShippingPinCode { get; set; }
            public string ShippingLandMark { get; set; }
            public string DealerAffixationAddress { get; set; }
            public string NonHomologVehicle { get; set; }
            public decimal TotalAmountST { get; set; }
            public decimal Basic_AmtST { get; set; }
            public decimal CGSTAmountST { get; set; }
            public decimal IGSTAmountST { get; set; }
            public decimal SGSTAmountST { get; set; }
            public string ErpItemCode { get; set; }
            public decimal TotalAmountFrm { get; set; }
            public decimal Basic_AmtFrm { get; set; }
            public decimal CGSTAmountFrm { get; set; }
            public decimal IGSTAmountFrm { get; set; }

            public decimal SGSTAmountFrm { get; set; } 
            public string FrontHSRPFileName { get; set; }
            public string RearHSRPFileName { get; set; }
            public string FileFIR { get; set; }
            public string FirDate { get; set; }
            public string Firinfo { get; set; }
            public string PoliceStation { get; set; }
            public string Firno { get; set; }
            public string FrontLaserCode { get; set; }
            public string RearLaserCode { get; set; }
            public string ReplacementReason { get; set; }
            public string CustomerName { get; set; }
            public string CustomerMobileNo { get; set; }
            public string CustomerEmailID { get; set; }
            public string CustomerAddress1 { get; set; }
            public string CustomerState { get; set; }
            public string CustomerCity { get; set; }
            public string CustomerPin { get; set; }
            public string val1 { get; set; }
            public string val2 { get; set; }
            public string val3 { get; set; }
            public string val4 { get; set; }
            public string val5 { get; set; }
            public string val6 { get; set; }
            public string hdnMyOrderID { get; set; }
            public string hdnGatewayOrderID { get; set; }
            public string Lateral { get; set; }

        }


    }
}
