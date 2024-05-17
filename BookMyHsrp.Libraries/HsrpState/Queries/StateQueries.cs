using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpState.Queries
{
    public class StateQueries
    {
        public static readonly string GetAllStates = "select HSRP_StateID,HSRPStateName from BMHSRPStates where isBookMyHSRP='Y' order by HSRPStateName";
        public static readonly string GetCities = "select HSRP_StateID,HSRPStateName from BMHSRPStates where isBookMyHSRP='Y' order by HSRPStateName";
    }
}
