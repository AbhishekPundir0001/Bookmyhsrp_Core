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
        }
        public class Dates
        {
            public string FromDate { get; set; }
            public string EndDate { get; set; }
            public string BlockDate { get; set; }
        }
    }
}
