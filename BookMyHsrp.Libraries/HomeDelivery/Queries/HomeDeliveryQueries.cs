using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HomeDelivery.Queries
{
    public class HomeDeliveryQueries
    {
        public static string CheckAvalibility => "exec sp_IsHomeDeliveryAllowed @StateId , @CheckFor ";
        public static string CheckPincode => "exec CheckHomeDeliveryAvailability @OemId ,@StateId ,@PinCode ";

    }
}
