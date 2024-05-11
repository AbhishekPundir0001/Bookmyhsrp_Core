using System.ComponentModel.DataAnnotations;

namespace BookMyHsrp.Models
{
    public class TrackOrder
    {
        [Required(ErrorMessage = "Order No. Required.")]
        public string OrderNo { get; set; }


        [Required(ErrorMessage = "Vehicle Registration Number Required.")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessage = "Vehicle Registration must be greater than 5.")]
        public string VehicleRegno { get; set; }
    }
    public class TrackOrderResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public TrackOrderData data;
    }

    public class TrackOrderData
    {
        public string orderNo { get; set; }
        public string vehicleRegNo { get; set; }

        public bool bookedByAkhsrp { get; set; } = true;
        public string appointmentdate { get; set; }
        public string orderStatus { get; set; }
        public string fitmentName { get; set; }
        public string fitmentAddress { get; set; }
        public string receiptPath { get; set; }
        public List<ReAppointmentData> reAppointmentData { get; set; }

    }

    public class ReAppointmentData
    {
        public string previousDate { get; set; }
        public string rescheduledDate { get; set; }
        public string rescheduledOn { get; set; }
        public string orderNo { get; set; }
        public string peceiptPath { get; set; }

    }

}
