using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Models
{
    public class CustomerInfoModel
    {
        [Required(ErrorMessage = "State Id Required.")]
        [Range(1, int.MaxValue, ErrorMessage = "State Id should not be 0")]
        public string stateid { get; set; }
        public string oemid { get; set; }
        public string non_homo { get; set; }
        public string order_type { get; set; }

        [Required(ErrorMessage = "Vehicle Registration Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Vehicle Registration must be greater than 5.")]
        public string vehicleregno { get; set; }

        [Required(ErrorMessage = "Chasis Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Chassis Number must be greater than 5.")]
        public string chassisno { get; set; }

        [Required(ErrorMessage = "Engine Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Engine Number must be greater than 5.")]
        public string engineno { get; set; }

        [Required(ErrorMessage = "Bharat Stage Required.")]
        public string bhart_stage { get; set; }

        [Required(ErrorMessage = "Vehicle Registration date Required.")]
        public string veh_reg_date { get; set; }

        [Required(ErrorMessage = "Customer Name Required.")]
        public string customer_name { get; set; }

        [Required(ErrorMessage = "Customer Email Required.")]
        [StringLength(30, MinimumLength = 6,
            ErrorMessage = "Email should be less than 30 characters.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string customer_email { get; set; }


        [Required(ErrorMessage = "Customer Mobile Required.")]
        [StringLength(10, MinimumLength = 10,
            ErrorMessage = "Mobile Number should be 10 digits.")]
        public string customer_mobile { get; set; }

        [Required(ErrorMessage = "Customer Billing Address Required.")]
        public string customer_billing_address { get; set; }


        [Required(ErrorMessage = "Customer State Required.")]
        public string customer_state { get; set; }

        [Required(ErrorMessage = "Customer City Required.")]
        public string customer_city { get; set; }
        public string customer_gstin { get; set; }
        public string rc_file { get; set; }

        [Required(ErrorMessage = "Vehicle Maker Required.")]
        public string maker { get; set; }

        [Required(ErrorMessage = "Vehicle Type Required.")]
        public string vehicle_type { get; set; }

        [Required(ErrorMessage = "Vehicle Fuel Type Required.")]
        public string fuel_type { get; set; }


        [Required(ErrorMessage = "Vehicle Category Required.")]
        public string vehicle_category { get; set; }

        [Required(ErrorMessage = "Oem Vehicle Type Required.")]
        public string OemVehicleType { get; set; }

        public bool isReplacement { get; set; } = false;
        public string front_laser_code { get; set; }
        public string rear_laser_code { get; set; }
        public string fir_number { get; set; }
        public string fir_info { get; set; }
        public string fir_date { get; set; }
        public string front_laser_path { get; set; }
        public string rear_laser_path { get; set; }
        public string fir_copy_name { get; set; }
        public string plate_sticker { get; set; } = "plate";
        public string police_station { get; set; }
        public string file4 { get; set; }
        public string replacement_reason { get; set; }
        public string ReplacementType { get; set; } = "";

    }

    public class CustomerInformationResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public CustomerInformationData data;
    }

    public class CustomerInformationData
    {
        public string VehicleType { get; set; }
        public string VehicleCat { get; set; }
        public string VehicleTypeid { get; set; }
        public string Vehiclecategoryid { get; set; }

        public string PlateOrderType { get; set; }
    }


}
