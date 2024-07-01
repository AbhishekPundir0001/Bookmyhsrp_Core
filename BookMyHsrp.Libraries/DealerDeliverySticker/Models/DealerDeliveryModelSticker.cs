using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.DealerDeliverySticker.Models
{
    public class DealerDeliveryModelSticker
    {
        public class SetSessionDealerSticker
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public DealerDataSticker data { get; set; }
            public List<DealerAppointmentDataSticker> dealerAppointment { get; set; }
        }
        public class DealerDataSticker
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public string TotalAmountWithGST { get; set; }
            public int FrontPlateSize { get; set; }
            public int RearPlateSize { get; set; }
            public decimal GstBasicAmount { get; set; }
            public decimal FittmentCharges { get; set; }
            public decimal BMHConvenienceCharges { get; set; }
            public decimal BMHHomeCharges { get; set; }
            public decimal MRDCharges { get; set; }
            public decimal GrossTotal { get; set; }
            public decimal GstAmount { get; set; }
            public decimal GST { get; set; }
            public decimal IGSTAmount { get; set; }
            public decimal CGSTAmount { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal SGSTAmount { get; set; }

        }
        public class DealerAppointmentDataSticker
        {
            public long DealerAffixationID { get; set; }
            public int Dealerid { get; set; }
            public string DealerName { get; set; }
            public string DealerAffixationCenterName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string DealerAffixationCenterContactNo { get; set; }
            public string Pincode { get; set; }
            public string Country { get; set; }
            public string StateName { get; set; }
            public int WebsiteId { get; set; }
            public string DealerAffixationCenterLat { get; set; }
            public string DealerAffixationCenterLon { get; set; }
            public string EarliestDateAvailable { get; set; }
            public decimal RoundOff_netamount { get; set; }
            public bool ExpressCheckout { get; set; } = false;
            public string Distance { get; set; }
            public string EarliestTimeSlotAvailable { get; set; }
            public int cnt { get; set; }
            public string TotalAmountWithGST { get; set; }
        }
        public class DealerAppointmentResponseLatestDateSticker
        {
            public string status { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }
        public class DealerAppointmentRequestDataSticker
        {
            [Required(ErrorMessage = "State Id Required.")]
            [Range(1, int.MaxValue, ErrorMessage = "State Id should not be 0.")]
            public string StateID { get; set; }

            [Required(ErrorMessage = "Oem Id Required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Oem Id should not be 0.")]
            public string OemID { get; set; }

            [Required(ErrorMessage = "Vehicle Category Required.")]
            public string VehicleCat { get; set; }

            [Required(ErrorMessage = "Vehicle Type Required.")]
            public string VehicleType { get; set; }

            [Required(ErrorMessage = "Vehicle Class Required.")]
            public string VehicleClass { get; set; }

            public string Vehiclecategoryid { get; set; }

            [Required(ErrorMessage = "Fuel Type Required.")]
            public string VehicleFuelType { get; set; }


            [Required(ErrorMessage = "Dealer Point Required.")]
            public string DeliveryPoint { get; set; }


            [Required(ErrorMessage = "State Name Required.")]
            public string StateName { get; set; }

            [Required(ErrorMessage = "Choose service Plate or Sticker.")]
            public string PlateSticker { get; set; }

            public string NonHomo { get; set; }
            public string VehicleTypeID { get; set; }
            public string PlateOrderType { get; set; } = "OB";
            public string ReplacementType { get; set; } = "";
            public string CustomerState { get; set; }
        }
    }

}
