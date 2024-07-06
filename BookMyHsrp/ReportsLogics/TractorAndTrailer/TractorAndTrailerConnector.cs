using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using BookMyHsrp.Libraries.TractorAndTrailer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Resources;
using System.Security.Cryptography;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.ReportsLogics.TractorAndTrailer
{
    public class TractorAndTrailerConnector
    {
        private readonly HsrpWithColorStickerService _hsrpColorStickerService;
        private readonly string nonHomo;
        private readonly string _nonHomoOemId;
        private readonly string OemId;
        public TractorAndTrailerConnector(HsrpWithColorStickerService hsrpColorStickerService, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            // _fetchDataAndCache = fetchDataAndCache; // Dependency injection
            // _hsrpWithColorStickerService = hsrpWithColorStickerService ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerService));
            _hsrpColorStickerService = hsrpColorStickerService ?? throw new ArgumentNullException(nameof(hsrpColorStickerService));
            nonHomo = dynamicData.Value.NonHomo;
            _nonHomoOemId = dynamicData.Value.NonHomoOemId;
            OemId = dynamicData.Value.OemID;

        }
        public async Task<dynamic> VahanInformation(VahanDetailsDto requestDto)
        {

            ICollection<ValidationResult> results = null;
            var vehicleValidationResponse = new ResponseDto();
            vehicleValidationResponse.data = new VehicleValidation();
            if (!Validate(requestDto, out results))
            {
                vehicleValidationResponse.status = "false";
                vehicleValidationResponse.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                return vehicleValidationResponse;
            }
            var resultGot = await _hsrpColorStickerService.VahanInformation(requestDto);

            VehicleValidation vehicleValidationData = new VehicleValidation();
            var getStateId = Convert.ToInt32(requestDto.StateId);
            var getVehicleRegno = requestDto.RegistrationNo.Trim();
            var getChassisNo = requestDto.ChassisNo.Trim();
            var getEngineNo = requestDto.EngineNo.Trim();

            var statename = string.Empty;
            var stateshortname = string.Empty;
            var StateIdBackup = string.Empty;
            var oemImgPath = string.Empty;
            var oemid = string.Empty;
            int stateID = getStateId;
            foreach (var data in resultGot)
            {
                stateshortname = data.HSRPStateShortName;
                statename = data.HSRPStateName;
                StateIdBackup = requestDto.StateId;
            }
            if (requestDto.RegistrationNo.Trim().ToUpper() == "DL10CG7191")
            {
            }
            else
            {
                var checkOrderExists = await _hsrpColorStickerService.CheckOrderExixts(getVehicleRegno, getChassisNo, getEngineNo);

                if (checkOrderExists.Count > 0)
                {
                    vehicleValidationResponse.status = "0";
                    vehicleValidationResponse.message = "Order for this registration number already exists. For any query kindly mail to online@bookmyhsrp.com";
                    return vehicleValidationResponse;
                }
            }
            oemImgPath = "logo";
            return vehicleValidationResponse;


        static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
            {
                results = new List<ValidationResult>();

                return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
            }

        }
    }
}
