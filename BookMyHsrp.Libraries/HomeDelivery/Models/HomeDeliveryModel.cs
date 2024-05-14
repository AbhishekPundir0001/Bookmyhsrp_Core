using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HomeDelivery.Models
{
    public class HomeDeliveryModel
    {
        public class CheckAvalibility
        {
            public string Status { set; get; }
            public string Message { set; get; }

            public string DeliveryCity { set; get; }
            public string DeliveryState { set; get; }
            public string StateId { set; get; }
            public string DealerAffixationCenterId { set; get; }
            public string DeliveryPoint { set; get; }
            public string StateName { set; get; }
            public string StateShortName { set; get; }
        }
        public class HomeDeliveryCheckAvailabilityRequestdata
        {
            [Required(ErrorMessage = "Pincode Required.")]
            [Range(100000, int.MaxValue, ErrorMessage = "Pincode should not be less than 6 digit")]
            [StringLength(6, MinimumLength = 6,
                ErrorMessage = "Pincode must be 6 digit.")]
            public string PinCode { get; set; }

            [Required(ErrorMessage = "Oem Id Required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Oem Id should not be 0")]
            public string OemId { get; set; }

            [Required(ErrorMessage = "State Id Required.")]
            [Range(1, int.MaxValue, ErrorMessage = "State Id should not be 0")]
            public string StateId { get; set; }
        }
        public class HomeDeliveryNotifyRequestdata
        {
            [Required(ErrorMessage = "Pincode Required.")]
            [Range(100000, int.MaxValue, ErrorMessage = "Pincode should not be less than 6 digit")]
            [StringLength(6, MinimumLength = 6,
                ErrorMessage = "Pincode must be 6 digit.")]
            public string PinCode { get; set; }

            [Required(ErrorMessage = "Vehicle Registration Number Required.")]
            [StringLength(30, MinimumLength = 5,
                ErrorMessage = "Vehicle Registration must be greater than 5.")]
            public string VehicleRegNo { get; set; }

            [Required(ErrorMessage = "Chasis Number Required.")]
            [StringLength(30, MinimumLength = 5,
                ErrorMessage = "Chassis Number must be greater than 5.")]
            public string ChassisNo { get; set; }

            [Required(ErrorMessage = "Engine Number Required.")]
            [StringLength(30, MinimumLength = 5,
                ErrorMessage = "Engine Number must be greater than 5.")]
            public string EngineNo { get; set; }

            [Required(ErrorMessage = "Customer Name Required.")]
            public string OwnerName { get; set; }

            [Required(ErrorMessage = "Customer Email Required.")]
            public string EmailId { get; set; }

            [Required(ErrorMessage = "Customer Mobile Required.")]
            [StringLength(10, MinimumLength = 10,
                ErrorMessage = "Customer Mobile Number should be 10 digits.")]
            public string customerMobileNo { get; set; }

            [Required(ErrorMessage = "Customer Billing Address Required.")]
            public string customerAddress { get; set; }

            [Required(ErrorMessage = "Customer State Required.")]
            public string customerState { get; set; }

            [Required(ErrorMessage = "Customer City Required.")]
            public string customerCity { get; set; }

            [Required(ErrorMessage = "MobileNo. Required.")]
            public string notifyMobileNo { get; set; }
        }


    }
}
