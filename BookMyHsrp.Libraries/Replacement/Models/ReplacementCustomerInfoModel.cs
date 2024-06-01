using BookMyHsrp.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Models
{
    public class ReplacementCustomerInfoModel
    {
        [Required(ErrorMessage = "State Id Required.")]
        [Range(1, int.MaxValue, ErrorMessage = "State Id should not be 0")]
        public string StateId { get; set; }
        public string StateName{ get; set; }

        public string OemId { get; set; }
        public string NonHomo { get; set; }
        public string OrderType { get; set; }

        [Required(ErrorMessage = "Vehicle Registration Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Vehicle Registration must be greater than 5.")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "Chasis Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Chassis Number must be greater than 5.")]
        public string ChassisNo { get; set; }

        [Required(ErrorMessage = "Engine Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Engine Number must be greater than 5.")]
        public string EngineNo { get; set; }

        [Required(ErrorMessage = "Bharat Stage Required.")]
        public string BharatStage { get; set; }

        [Required(ErrorMessage = "Vehicle Registration date Required.")]
        public string RegistrationDate { get; set; }

        [Required(ErrorMessage = "Customer Name Required.")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Customer Email Required.")]
        [StringLength(30, MinimumLength = 6,
            ErrorMessage = "Email should be less than 30 characters.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Customer Mobile Required.")]
        [StringLength(10, MinimumLength = 10,
            ErrorMessage = "Mobile Number should be 10 digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Customer Billing Address Required.")]
        public string BillingAddress { get; set; }


        public string RcFileName { get; set; }

        [Required(ErrorMessage = "Vehicle Maker Required.")]
        public string MakerVahan { get; set; }

        [Required(ErrorMessage = "Vehicle Type Required.")]
        public string VehicleTypeVahan { get; set; }

        [Required(ErrorMessage = "Vehicle Fuel Type Required.")]
        public string FuelTypeVahan { get; set; }


        [Required(ErrorMessage = "Vehicle Category Required.")]
        public string VehicleCatVahan { get; set; }

        public bool isReplacement { get; set; } = false;
        public string FrontLaserCode { get; set; }
        public string RearLaserCode { get; set; }
        public string FirNumber { get; set; }
        public string FirInfo { get; set; }
        public string FirDate { get; set; }
        public string FrontLaserPath { get; set; }
        public string RearLaserPath { get; set; }
        public string FirCopyName { get; set; }
        public string PlateSticker { get; set; } = "plate";
        public string PoliceStation { get; set; }
        public string File4 { get; set; }
        public string ReplacementReason { get; set; }
        public string ReplacementType { get; set; } = "";

    }

    public class ReplacementCustomerInformationResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public CustomerInformationData data;
    }

    public class ReplacementCustomerInformationData
    {
        public string NonHomoVehicleType { get; set; }
        public string Message { get; set; }
        public string VehicleType { get; set; }
        public string VehicleClass_imgPath { get; set; }
        public string VehicleType_imgPath { get; set; }
        public string IsOTPVerify { get; set; }
        public string OTPno { get; set; }
        public string OrderType { get; set; }
        public string OEMImgPath { get; set; }
        public string VehicleTypeId { get; set; }
        public string VehicleCat { get; set; }
        public string Vehiclecategoryid { get; set; }
        public string Vehiclecategory { get; set; }
        public string VehicleFuelType { get; set; }
        public string VehicleClass { get; set; }
        public string SessionBharatStage { get; set; }
        public string RegDate { get; set; }
        public string OwnerName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string FilePath { get; set; }
        public string BillingAddress { get; set; }
        public string SessionState { get; set; }
        public string SessionCity { get; set; }
        public string SessionGST { get; set; }
        public string VehicleTypeid { get; set; }

        public string PlateOrderType { get; set; }
    }
    public class ReplacementSession
    {
        public string IsOTPVerify { get; set; }
        public string OTPno { get; set; }
        public string VehicleType_imgPath { get; set; }
        public string OEMImgPath { get; set; }
        public string OrderType { get; set; }

    }
    public class ReplacementSetCustomerData
    {
        public string NonHomoVehicleType { get; set; }
        public string Message { get; set; }
        public string VehicleType { get; set; }
        public string VehicleClass_imgPath { get; set; }
        public string VehicleTypeId { get; set; }
        public string VehicleCat { get; set; }
        public string Vehiclecategoryid { get; set; }
        public string Vehiclecategory { get; set; }
        public string VehicleFuelType { get; set; }
        public string VehicleClass { get; set; }
        public string SessionBharatStage { get; set; }
        public string RegDate { get; set; }
        public string OwnerName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string FilePath { get; set; }
        public string BillingAddress { get; set; }
        public string SessionState { get; set; }
        public string SessionCity { get; set; }
        public string SessionGST { get; set; }

    }
}
