using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.AppointmentSlot.Model
{
    public class AppointmentSlotModel
    {
        public class AppointmentSlotSessionResponse
        {
            public string Message { get; set; }
            public string DealerAffixationCenterId { get; set; }
            public string SelectedSlotID { get; set; }
            public string SelectedSlotDate { get; set; }
            public string SelectedSlotTime { get; set; }
            public string Affix { get; set; }
            public string DeliveryPoint { get; set; }
            public string DealerAffixationCenterContactPerson  { get; set; }
            public string DealerAffixationCenterContactNo { get; set; }
        }
        public class Dates
        {
            public string FromDate { get; set; }
            public string EndDate { get; set; }
            public string BlockDate { get; set; }
        }
        public class AppointmentSlotCheckEc
        {
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }
        public class CheckTimeSlot
        {
            public string Date { get; set; }
            public string StartDate { get; set; }
        }
        public class TimeSlotList
        {
            public int TimeSlotID { get; set; }
            public int SlotID { get; set; }
            public string SlotName { get; set; }
            public int RTOCodeID { get; set; }
            public int VehicleTypeID { get; set; }
            public int AvaiableCount { get; set; }
            public int BookedCount { get; set; }
            public string AvaiableStatus { get; set; }
        }
       
    }
}
