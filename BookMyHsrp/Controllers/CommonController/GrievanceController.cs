using BookMyHsrp.Libraries.GenerateOtp.Services;
using BookMyHsrp.Libraries.Grievance.Models;
using BookMyHsrp.Libraries.Grievance.Services;
using BookMyHsrp.Libraries.OrderCancel.Models;
using BookMyHsrp.Libraries.OrderCancel.Services;
using iTextSharp.tool.xml.html;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.Grievance.Models.GrievanceModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class GrievanceController : Controller
    {
        private readonly IGrievanceServices _grievanceservices;

        public GrievanceController(IGrievanceServices grievanceServices)
        {
            _grievanceservices = grievanceServices;
        }

        [Route("/grievance")]
        public IActionResult Grievance()
        {
            return View();
        }



        [Route("/grievanceinsert")]
        [HttpPost]
        public async Task<IActionResult> Grievance([FromBody] GrievanceInsert requestdto)
        {
            var result = await _grievanceservices.getRecord(requestdto.VehicleRegNo);
            Responsedto response = new Responsedto { } ;

            if (result.Count > 0)
            {
                response.message = "Request Already Raised";
                return Ok(response);
            }
            else
            {
                var resultGot = await _grievanceservices.greivanceinsert(requestdto.VehicleRegNo, requestdto.OrderNo, requestdto.MobileNo, requestdto.EmailId, requestdto.Query, requestdto.CustomerName);
            
                    response.message = resultGot;
                    return Ok(response);

                
            }
        }

    }
}
