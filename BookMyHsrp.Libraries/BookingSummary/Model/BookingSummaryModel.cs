using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.BookingSummary.Model
{
    public class BookingSummaryModel
    {
        public class BookingDetails
        {
            public string DealerAffixationCenterId { get; set; }
            public string BharatStage { get; set; }
            public string SlotTime { get; set; }
            public int OemID { get; set; }
            public string OemName { get; set; }
            public int DealerID { get; set; }
            public int StateID { get; set; }
            public int RTOLocationID { get; set; }
            
            public string RTOLocationName { get; set; }
            public string SlotDate { get; set; }
            public string DealerAffixationCenterName { get; set; }
            public string DealerAffixationCenterAddress { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string FuelType { get; set; }
            public string PlateSticker { get; set; }
            public string VehicleCategory { get; set; }
            public string VehicleClass { get; set; }
            public string VehicleRegNo { get; set; }
            public string CustomerMobile { get; set; }
            public string VehicleType { get; set; }
            public string CustomerBillingAddress { get; set; }

        }
        public class BookingDate
        {
            public string  Date { get; set; }
            public string  SlotTime { get; set; }
        }
    }
}
