using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.BookingSummary.Queries
{
    public class BookingSummaryQueries
    {
        public static string BookingSummaryConfirmation => "select a.OemID, c.Name OemName, a.DealerID, a.StateID, a.RTOLocationID,b.RTOLocationName,a.DealerAffixationCenterName, a.DealerAffixationCenterAddress  from [HSRPOEM].dbo.DealerAffixationCenter a, [HSRPOEM].dbo.rtolocation b, [HSRPOEM].dbo.OEMMaster c where a.rtolocationid = b.RTOLocationID and a.OemID = c.OEMID and DealerAffixationID = @DealerAffixationId ";

    }
}
