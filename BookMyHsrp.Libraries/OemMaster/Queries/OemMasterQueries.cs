using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OemMaster.Queries
{
    public class OemMasterQueries
    {
        public static readonly string GetAllOemByVehicleType = "select Distinct OER.VehicleType,Concat(OER.Vehicletypenew,'-',OER.VehicleType) [Vehicletypenew] ,OM.IsVehicleTypeEnable from hsrpoem.dbo.oemrates OER join [hsrpoem].dbo.oemmaster OM on OM.OemId=OER.OemId   where OER.OemId=@OemId and OER.VehicleClass=@VehicleType and OER.OrderType='OB' ";
    }
}
