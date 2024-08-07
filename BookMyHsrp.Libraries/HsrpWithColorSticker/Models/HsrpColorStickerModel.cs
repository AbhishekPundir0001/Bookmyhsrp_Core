﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.OemMaster.Models.OemMasterModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Models
{
    public class HsrpColorStickerModel
    {
        public class VahanDetailsDto
        {
            [Required(ErrorMessage = "Registration No is required.")]
            //[StringLength(5 , ErrorMessage = "Registration No is  Not Valid")]
            public string RegistrationNo { get; set; }
            [Required(ErrorMessage = "Chassis No is required.")]
            //[StringLength(5, ErrorMessage = "Chassis No is Not Valid")]
            public string ChassisNo { get; set; }
            [Required(ErrorMessage = "EngineNo No is required.")]
           // [StringLength(5, ErrorMessage = "EngineNo No is Not Valid")]
            public string EngineNo { get; set; }
            [Required(ErrorMessage = "State is required.")]
            public string StateId { get; set; }
            public string StateName { get; set; }
            public bool isReplacement { get; set; } = false;



        }
        public class VehicleDetails
        {
            public string message { get; set; }
            public string fuel { get; set; }
            public string maker { get; set; }
            public string vchType { get; set; }
            public string norms { get; set; }
            public string vchCatg { get; set; }
            public string regnDate { get; set; }
            public string hsrpFrontLaserCode { set; get; }
            public string hsrpRearLaserCode { set; get; }
            public string stateCd { set; get; }
            public string offCd { set; get; }
            public VehicleValidation data;
        }

        
        public class ResponseDto
        {
            public string Message;
            public string status;
            public string message;
            public VehicleValidation data;
        }

        public class ResponseSticker
        {
            public string Message { get; set; }

        }

        public class GetSessionBookingDetails
        {
            public string BharatStage { get; set; }
            public string RegistrationDate { get; set; }
            public string VehicleRegNo { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string OwnerName { get; set; }
            public string EmailId { get; set; }
            public string CustomerMobile { get; set; }
            public string FilePath { get; set; } = "";
            public string BillingAddress { get; set; } = "";
            public string ReplacementType { get; set; } = "";
            public string RcFileName { get; set; } = "";
            public string MakerVahan { get; set; } = "";
            public string VehicleTypeVahan { get; set; } = "";
            public string FuelTypeVahan { get; set; } = "";
            public string VehicleCatVahan { get; set; } = "";
            public string StateId { get; set; } = "";
            public string StateName { get; set; } = "";
            public string OemVehicleType { get; set; } = "";
            public string OemId { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public string CustomerEmail { get; set; } = "";
            public string CustomerBillingAddress { get; set; } = "";
            public string CustomerCity { get; set; } = "";
            public string OrderType { get; set; } = "";
            public string VehicleCategory { get; set; } = "";
            public string VehicleType { get; set; } = "";
            public string VehicleClass { get; set; } = "";
            public string StateIdBackup { get; set; } = "";
            public string VehicleCategoryId { get; set; } = "";
            public string FuelType { get; set; } = "";
            public string DeliveryPoint { get; set; } = "";
            public string PlateSticker { get; set; } = "";
            public string NonHomo { get; set; } = "";
            public string VehicleTypeId { get; set; } = "";
            public string DealerAffixationCenterId { get; set; }
            public string SelectedSlotID { get; set; }
            public string SelectedSlotDate { get; set; }
            public string SelectedSlotTime { get; set; }
            public string Affix { get; set; }
        }
        public class VehicleValidation
        {
            public string fuel { get; set; }
            public string StateIdBackup { get; set; }
            public string maker { get; set; }
            public string non_homo { get; set; } = "N";
            public string upload_flag { get; set; } = "N";
            public string norms { get; set; }
            public string oem_img_path { get; set; }
            public string oemid { get; set; }
            public string stateid { get; set; }
            public string statename { get; set; }
            public string stateshortname { get; set; }
            public string veh_reg_date { get; set; }
            public string vehicle_category { get; set; }
            public string vehicle_class { get; set; }
            public string engineno { get; set; }
            public string chassisno { get; set; }
            public string vehicleregno { get; set; }
            public string message { get; set; }
            public string vchType { get; set; }
            public string vchCatg { get; set; }
            public string regnDate { get; set; }
            public string hsrpFrontLaserCode { set; get; }
            public string hsrpRearLaserCode { set; get; }
            public string stateCd { set; get; }
            public string offCd { set; get; }

            public bool show_damage_both { get; set; } = true;
            public List<OemVehicleTypeList> oemvehicletypelist { get; set; }

        }
      

    }
}
