using BookMyHsrp.Libraries.HsrpState.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.Common
{

    [Route("api/v1/state")]
    public class GetAllStatesController : ControllerBase
    {
        private readonly IStateService _allStatesService;
        public GetAllStatesController(IStateService allStatesService)
        {

            _allStatesService = allStatesService;
        }
       
        [HttpGet]
        public async Task<dynamic> GetAllStates()
        {

            var resultGot = await _allStatesService.GetAllStates();
            return resultGot;

        }
    }
}
