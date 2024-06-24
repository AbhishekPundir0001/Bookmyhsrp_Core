using BookMyHsrp.Libraries.HsrpState.Models;
using BookMyHsrp.Libraries.OemMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OemMaster.Services
{
    public interface IOemMasterService
    {
        Task<IEnumerable<OemMasterModel.OemVehicleTypeList>> GetAllOemByVehicleType(string vehicleType,dynamic vehicledetails);
    }
}
