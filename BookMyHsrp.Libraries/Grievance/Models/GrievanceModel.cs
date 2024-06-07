using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Grievance.Models
{
    public  class GrievanceModel
    {
        public class GrievanceInsert
        {
            public string VehicleRegNo { get; set; } = "";
            public string OrderNo { get; set; } = "";
            public string MobileNo { get; set; } = "";
            public string EmailId { get; set; } = "";
            public string Query { get; set; } = "";
            public string CustomerName { get; set; } = "";

        }
        public class Responsedto
        {
            public string status { get; set; } = "";

            public string message { get; set; } = "";

        }
    }
}
