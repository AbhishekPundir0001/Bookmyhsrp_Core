using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Models
{
    public class OrderCancelModel
    {
        public class logDto1
        {
            public string VehicleNo { get; set; } = "";
            public string OrderNo { get; set; } = "";
            public string AppointmentSlot { get; set; } = "";
            public string AppointmentDate { get; set; } = "";
            public string EngineNo { get; set; } = "";
            public string ChassisNo { get; set; } = "";
            public string VehicleMake { get; set; } = "";
            public string FuelType { get; set; } = "";
            public string FitmentAddress { get; set; } = "";
            public string VehicleType { get; set; } = "";
            public string VehicleClass { get; set; } = "";
            public string Reason { get; set; } = "";

        }
        public class logDto
        {
            public string VehicleNo { get; set; } = "";
            public string OrderNo { get; set; } = "";
            public string AppointmentSlot { get; set; } = "";
            public string EngineNo { get; set; } = "";
            public string ChassisNo { get; set; } = "";
            public string VehicleMake { get; set; } = "";
            public string FuelType { get; set; } = "";
            public string FitmentAddress { get; set; } = "";
            public string VehicleType { get; set; } = "";
            public string VehicleClass { get; set; } = "";
            public string Reason { get; set; } = "";

        }
        public class OrderCancel
        {
            public string OrderNo { get; set; } = "";
            public string VehicleregNo { get; set; } = "";

        }
        public class OrderCancelReason
        {
            public string OrderNo { get; set; } = "";
            public string VehicleregNo { get; set; } = "";
            public string Reason { get; set; } = "";

        }
        public class OtpModal
        {
            public string VehicleRegNo { get; set; } = "";
            public string MobileNo { get; set; } = "";
            public string OrderNo { get; set; } = "";

        }
    }
}
