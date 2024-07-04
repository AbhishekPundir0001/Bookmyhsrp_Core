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
        public class Message
        {
            public string Status { get; set; } = "";
            public string message { get; set; } = "";

        }

        public class RootDtoReAppointment
        {
            public string ReOrderNo { get; set; } = "";
            public string PlateSticker { get; set; } = "";
            public string VehicleRegNo { get; set; } = "";
            public string OldAppointmentDate { get; set; } = "";
            public string OldAppointmentSlot { get; set; } = "";
            public int ReOEMId { get; set; }
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
            public int ReDealerAffixationCenteridid { get; set; }
            public string ReDealerAffixationCenterName { get; set; } = "";

        }

    }
}