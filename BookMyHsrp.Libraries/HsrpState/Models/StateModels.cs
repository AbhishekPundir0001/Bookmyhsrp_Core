using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpState.Models
{
    public class StateModels
    {
        public class Root
        {
            public int HSRP_StateID { get; set; }
            public string HSRPStateName { get; set; }

        }
        public class Cities
        {
            public string District { get; set; }

        }
    }
}
