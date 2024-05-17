using BookMyHsrp.Libraries.HsrpState.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMyHsrp.Controllers.CommonController
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
        [Route("/city/{Id}")]
        public async Task<dynamic> GetCityOfState(string Id)
        {

            var resultGot = await _allStatesService.GetCityOfState(Id);
            return resultGot;

        }
    }
}
