using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using BookMyHsrp.ReportsLogics.Replacement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.ReplacementHsrpColorStickerModel;

namespace BookMyHsrp.Controllers
{
    public class ReplacementApiController : Controller
    {

        private readonly ILogger<ReplacementApiController> _logger;

        private readonly ReplacementConnector _replacementConnector;
        public ReplacementApiController(ILogger<ReplacementApiController> logger, ReplacementConnector hsrpWithColorStickerConnector)
        {
            _replacementConnector = hsrpWithColorStickerConnector ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerConnector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("/Replacement/ReValidateData")]
        public async Task<IActionResult> ValidateRequired([FromBody] ReplacementVahanDetailsDto vahanDetailsDto)
        {
            var jsonSerializer = "";
            var result = await _replacementConnector.VahanInformation(vahanDetailsDto);
            if (result.status == "true")
            {
                if (!string.IsNullOrEmpty(result.data.veh_reg_date))
                {
                    IFormatProvider theCultureInfo = new CultureInfo("en-GB", true);
                    var resultDateTime = DateTime.TryParseExact(result.data.veh_reg_date, "yyyy-MM-dd",
                        theCultureInfo,
                        DateTimeStyles.None
                        , out DateTime dt)
                        ? dt
                        : null as DateTime?;
                    if (resultDateTime.HasValue)
                    {
                        result.data.veh_reg_date = resultDateTime.Value.ToString("dd/MM/yyyy");
                    }
                    //else
                }
                var rootDto = new RootDto
                {
                    Status = result.status,
                    StateId = result.data.stateid,
                    VehicleRegNo = result.data.vehicleregno,
                    ChassisNo = result.data.chassisno,
                    EngineNo = result.data.engineno,
                    Fuel = result.data.fuel,
                    Maker = result.data.maker,
                    NonHomo = result.data.non_homo,
                    Norms = result.data.norms,
                    OemImgPath = result.data.oem_img_path,
                    OemId = result.data.oemid,
                    StateName = result.data.statename,
                    StateShortName = result.data.stateshortname,
                    VehRegDate = result.data.veh_reg_date,
                    VehicleCategory = result.data.vehicle_category,
                    VehicleClass = result.data.vehicle_class,
                    FuelType = result.data.fuel,
                    PlateSticker = "Plate",
                    IsReplacement = vahanDetailsDto.isReplacement,
                    Message = result.data.message,
                    StateIdBackup = result.data.StateIdBackup
                };

                var GetRootObjectSession = HttpContext.Session.GetString("UserSession");
                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(rootDto);
                HttpContext.Session.SetString("UserSession", jsonSerializer);

            }
            if (jsonSerializer != "")
            {
                return Json(jsonSerializer);
            }
            else
            {

                return BadRequest(new { Error = true, Message = result.message });
            }
        }

        [HttpPost]
        [Route("/Replacement/ReplacementBooking")]
        public async Task<IActionResult> CustomerInfo([FromBody] ReplacementCustomerInfoModel info)
        {
            #region Validation
            if (Convert.ToInt32(info.StateId) != 25)
            {
                if (info.ReplacementReason == "BD")
                {
                    #region Replacement Code 
                    if (info.RcCopy == "" || info.RcCopy == null)
                    {
                        return BadRequest(new { Error = true, Message = "Please Upload Rc File" });
                    }

                    if (info.OrderType == "BDF")
                    {
                        if (info.RearLaserPath == "" || info.RearLaserPath == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Rear Plate Photo" });
                        }

                        if (info.RearLaserCode == "" || info.RearLaserCode == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Provide Rear Laser Code" });
                        }

                    }

                    if (info.OrderType == "BDR")
                    {
                        if (info.FrontLaserPath == "" || info.FrontLaserPath == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Front Plate Photo" });
                        }

                        if (info.FrontLaserCode == "" || info.FrontLaserCode == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Provide Rear Laser Code" });
                        }

                    }
                    #endregion
                }
            }
            else if (Convert.ToInt32(info.StateId) == 25)
            {
                if (info.ReplacementReason == "LT")
                {
                    #region check FIRDateValidation
                    try
                    {
                        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                        DateTime FIR_date = DateTime.ParseExact(info.FirDate, "dd/MM/yyyy", theCultureInfo);
                        DateTime _regdate = DateTime.ParseExact(info.RegistrationDate, "dd/MM/yyyy", theCultureInfo);
                        string Day = System.DateTime.Now.Day.ToString();
                        string month = System.DateTime.Now.Month.ToString();
                        if (Day.Length == 1)
                        {
                            Day = "0" + Day;
                        }

                        if (month.Length == 1)
                        {
                            month = "0" + month;
                        }
                        string ENDDate = Day + "/" + month + "/" + System.DateTime.Now.Year.ToString();
                        DateTime _ENDDate = DateTime.ParseExact(ENDDate, "dd/MM/yyyy", theCultureInfo);

                        string txt_total_days = ((_ENDDate - FIR_date).TotalDays).ToString();
                        int diffResult = int.Parse(txt_total_days.ToString());

                        if (diffResult <= 0)
                        {
                            return BadRequest(new { Error = true, Message = "Fir Date should only be before Today's Date" });
                        }

                        string diffRegAndFir_Days = ((FIR_date - _regdate).TotalDays).ToString();
                        int diffResultRegAndFir = int.Parse(diffRegAndFir_Days.ToString());
                        if (diffResultRegAndFir <= 0)
                        {
                            return BadRequest(new { Error = true, Message = "Fir Date should only be After Registration Date" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { Error = true, Message = "Check FIR date Format DD/MM/YYYY" });
                    }

                    #endregion

                    if (info.FirDate == "" || info.FirDate == null)
                    {
                        return BadRequest(new { Error = true, Message = "Please Upload FIR File" });
                    }
                    if (info.FirDate == "" || info.FirDate == null)
                    {
                        return BadRequest(new { Error = true, Message = "Please enter FIR Date" });
                    }
                }
                if (info.ReplacementReason == "BD")
                {
                    if (info.OrderType == "BDF")
                    {
                        if (info.FrontLaserPath == "" || info.FrontLaserPath == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Front Laser File" });
                        }
                    }
                    else if (info.OrderType == "BDR")
                    {
                        if (info.RearLaserPath == "" || info.RearLaserPath == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Rear Laser File" });
                        }
                    }
                    else if (info.OrderType == "BDB")
                    {
                        if (info.FrontLaserPath == "" || info.FrontLaserPath == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Front Laser File" });
                        }
                        else if (info.RearLaserPath == "" || info.RearLaserPath == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Rear Laser File" });
                        }
                    }
                }

                if (info.ReplacementReason == "CA" || info.ReplacementReason == "RE")
                {
                    if (info.RcCopy == "" || info.RcCopy == null)
                    {
                        return BadRequest(new { Error = true, Message = "Please Upload RC File" });
                    }
                }

            }

            if (Convert.ToInt32(info.StateId) != 25)
            {
                if (info.ReplacementReason == "LT" || info.ReplacementReason == "BD")
                {
                    #region check FIRDateValidation
                    try
                    {
                        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                        DateTime FIR_date = DateTime.ParseExact(info.FirDate, "dd/MM/yyyy", theCultureInfo);
                        DateTime _regdate = DateTime.ParseExact(info.RegistrationDate.Replace('-','/'), "dd/MM/yyyy", theCultureInfo);
                        string Day = System.DateTime.Now.Day.ToString();
                        string month = System.DateTime.Now.Month.ToString();
                        if (Day.Length == 1)
                        {
                            Day = "0" + Day;
                        }

                        if (month.Length == 1)
                        {
                            month = "0" + month;
                        }
                        string ENDDate = Day + "/" + month + "/" + System.DateTime.Now.Year.ToString();
                        DateTime _ENDDate = DateTime.ParseExact(ENDDate, "dd/MM/yyyy", theCultureInfo);

                        string txt_total_days = ((_ENDDate - FIR_date).TotalDays).ToString();
                        int diffResult = int.Parse(txt_total_days.ToString());

                        if (diffResult <= 0)
                        {
                            return BadRequest(new { Error = true, Message = "Fir Date should only be before Today's Date" });
                        }

                        string diffRegAndFir_Days = ((FIR_date - _regdate).TotalDays).ToString();
                        int diffResultRegAndFir = int.Parse(diffRegAndFir_Days.ToString());
                        if (diffResultRegAndFir <= 0)
                        {
                            return BadRequest(new { Error = true, Message = "Fir Date should only be After Registration Date" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { Error = true, Message = "Check FIR date Format DD/MM/YYYY" });
                    }

                    #endregion

                    if (info.FirNumber == null || info.FirNumber == "")
                    {
                        return BadRequest(new { Error = true, Message = "Please provide FIR No." });
                    }

                    if (info.FirDate == null || info.FirDate == "")
                    {
                        return BadRequest(new { Error = true, Message = "Please Provide FIR Date." });
                    }

                    if (info.FirCopy == "" || info.FirCopy == null)
                    {
                        return BadRequest(new { Error = true, Message = "Please Upload FIR Copy." });
                    }

                    if (Convert.ToInt32(info.StateId) != 25)
                    {
                        if (info.RcCopy == "" || info.RcCopy == null)
                        {
                            return BadRequest(new { Error = true, Message = "Please Upload Rc File" });
                        }
                    }

                    #region Replacement Code 

                    if (Convert.ToInt32(info.StateId) != 25)
                    {
                        if (info.OrderType == "BDF")
                        {
                            if (info.RearLaserPath == "" || info.RearLaserPath == null)
                            {
                                return BadRequest(new { Error = true, Message = "Please Upload Rear Plate Photo" });
                            }

                            if (info.RearLaserCode == "" || info.RearLaserCode == null)
                            {
                                return BadRequest(new { Error = true, Message = "Please Provide Rear Laser Code" });
                            }

                        }

                        if (info.OrderType == "BDR")
                        {
                            if (info.FrontLaserPath == "" || info.FrontLaserPath == null)
                            {
                                return BadRequest(new { Error = true, Message = "Please Upload Front Plate Photo" });
                            }

                            if (info.FrontLaserCode == "" || info.FrontLaserCode == null)
                            {
                                return BadRequest(new { Error = true, Message = "Please Provide Front Laser Code" });
                            }
                        }
                    }
                    #endregion
                }
            }

            if (info.BillingAddress.Contains('\''))
            {
                return BadRequest(new { Error = true, Message = "Apostrophe (') is not allowed in address" });
            }
            #endregion

            var jsonSerializer = "";
            var resultGot = new CustomerInformationResponse();
            var detailsSession = HttpContext.Session.GetString("UserSession");
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                resultGot = await _replacementConnector.CustomerInfo(info, detailsSession);
                if (resultGot.Message == "Success")
                {
                    var getSession = new RootDto();
                    getSession.CustomerBillingAddress = info.BillingAddress.Replace("'", "");
                    getSession.BhartStage = info.BharatStage;
                    getSession.CustomerName = info.OwnerName;
                    getSession.CustomerEmail = info.EmailId;
                    getSession.CustomerMobile = info.MobileNo;
                    getSession.VehicleType = resultGot.data.VehicleType;
                    getSession.VehicleCat = resultGot.data.VehicleCat;
                    getSession.VehicleTypeId = resultGot.data.VehicleTypeId;
                    getSession.VehicleCategoryId = resultGot.data.Vehiclecategoryid;
                    getSession.FuelType = info.FuelTypeVahan;
                    getSession.Fuel = info.FuelTypeVahan;
                    getSession.Message = resultGot.Message;
                    getSession.OrderType = resultGot.data.OrderType;

                    var GetRootObjectSession = HttpContext.Session.GetString("UserSession");
                    jsonSerializer = System.Text.Json.JsonSerializer.Serialize(getSession);
                    HttpContext.Session.SetString("UserDetail", jsonSerializer);
                }
            }
            if (jsonSerializer != "")
            {
                return Json(jsonSerializer);
            }
            else
            {
                return BadRequest(new { Error = true, Message = resultGot.Message });
            }



        }
    }
}
