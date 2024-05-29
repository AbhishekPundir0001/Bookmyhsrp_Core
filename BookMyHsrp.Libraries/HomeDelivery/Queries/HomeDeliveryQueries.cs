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
        public static string UpdateAvailibility => "INSERT INTO [dbo].[DeliveryPincodeCheckLog] ([deliveryPincode],[vehicleRegNo],[ChassisNo] ,[EngineNo],[OwnerName],[emailid],[ownerno],[billingAddress] ,[state],[city],[deliverymobile]) VALUES( @Pincode ,@RegNo ,@ChassisNo,@EngineNo,@OwnerName,@EmailId,@MobileNo,@BillingAddress,@State,@City,@PincodeMobileNo )";

    }
}
