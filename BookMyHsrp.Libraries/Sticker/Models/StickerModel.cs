using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.OemMaster.Models.OemMasterModel;

namespace BookMyHsrp.Libraries.Sticker.Models
{
    public class StickerModel
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
            [Required(ErrorMessage = "Front Laser Code is required.")]
            public string HsrpFrontLaserCode { get; set; }
            [Required(ErrorMessage = "Rear Laser Code is required.")]
            public string HsrpRearLaserCode { get; set; }
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
            public string StateidVahan;
            public string StateName;
            public string UploadFlag;
            public VehicleValidation data;
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
            public string RcFileName { get; set; } = "";
            public string MakerVahan { get; set; } = "";
            public string VehicleTypeVahan { get; set; } = "";
            public string FuelTypeVahan { get; set; } = "";
            public string VehicleCatVahan { get; set; } = "";
            public string StateId { get; set; } = "";
            public string StateName { get; set; } = "";
            public string OemVehicleType { get; set; } = "";
            public string OemId { get; set; } = "";
        }
        public class VehicleValidation
        {
            public string fuel { get; set; }
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
