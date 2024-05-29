using BookMyHsrp.Libraries.HsrpState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpState.Services
{
    public interface IStateService
    {
        Task<IEnumerable<StateModels.Root>> GetAllStates();
        Task<IEnumerable<StateModels.Cities>> GetCityOfState(int Id);
    }
}
