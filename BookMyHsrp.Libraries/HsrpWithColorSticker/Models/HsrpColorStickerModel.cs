using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Models
{
    public class HsrpColorStickerModel
    {
        public class VahanDetails
        {

            public string RegistrationNo { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string StateId { get; set; }




        }
        public class VehicleDetails
        {
            public string message { get; set; }
            public string fuel { get; set; }
            public string maker { get; set; }
            public string vchType { get; set; }
            public string norms { get; set; }
            public string vchCatg { get; set; }
            public string regnDate { get; set; }
            public string hsrpFrontLaserCode { set; get; }
            public string hsrpRearLaserCode { set; get; }
            public string stateCd { set; get; }
            public string offCd { set; get; }
        }

        public class ResponseDto
        {
            public dynamic ResponseData { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
            public string NonHomo { get; set; }




        }

        public class GetSessionBookingDetails
        {
            public string BharatStage { get; set; }
            public string RegistrationDate { get; set; }
            public string RegistrationNo { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string OwnerName { get; set; }
            public string EmailId { get; set; }
            public string MobileNo { get; set; }
            public string FilePath { get; set; } = "";
            public string BillingAddress { get; set; } = "";
            public string RcFileName { get; set; } = "";
            public string MakerVahan { get; set; } = "";
            public string VehicleTypeVahan { get; set; } = "";
            public string FuelTypeVahan { get; set; } = "";
            public string VehicleCatVahan { get; set; } = "";
            public string StateId { get; set; } = "";
            public string StateName { get; set; } = "";
            public string OemVehicleType { get; set; } = "";

        }
    }
}
