using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using BookMyHsrp.Libraries.Sticker.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;

namespace BookMyHsrp.Libraries.Sticker.Services
{
    public class StickerService: IStickerService
    {
        private readonly FetchDataAndCache _fetchDataAndCache;
        //private readonly AppSettings _appSettings;
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly DapperRepository _databaseHelperDL;
        private readonly DapperRepository _databaseHelperHR;
        private readonly DynamicDataDto _dynamicDataDto;
        private readonly string _connectionString;
        private readonly string _vehicleStatusAPI;
        private readonly string _oemId;
        private readonly string _nonHomo;
        private readonly string _nonHomoOemId;
        string msg = string.Empty;
        // private readonly IHttpContextAccessor _httpContextAccessor;

        public StickerService(IOptionsSnapshot<ConnectionString> connectionStringOptions, FetchDataAndCache fetchDataAndCache, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);

            //_connectionString = connectionStringOptions.Value.DLConnectionString;
            _databaseHelperDL = new DapperRepository(connectionStringOptions.Value.DLConnectionString);
            _databaseHelperHR = new DapperRepository(connectionStringOptions.Value.HRConnectionString);
            //_appSettings = appSettings;
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
            //_httpContextAccessor = httpContextAccessor;
            _fetchDataAndCache = fetchDataAndCache;
            _vehicleStatusAPI = dynamicData.Value.VehicleStatusAPI;
            //_oemId = dynamicData.Value.OemID;
            //_nonHomo = dynamicData.Value.NonHomo;
            //_nonHomoOemId = dynamicData.Value.NonHomoOemId;

        }
        public async Task<dynamic> GetStateDetails(VahanDetailsDto requestDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", requestDto.StateId);
            var result = await _databaseHelper.QueryAsync<dynamic>(
                 StickerQueries.GetStateDetails, parameters);
            return result;
        }

        public async Task<dynamic> isAbleToBook(string VehicleRegNo, string chassisNo, string engineNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo);
            parameters.Add("@ChassisNo", chassisNo.Substring(chassisNo.Length - 5));
            parameters.Add("@EngineNo", engineNo.Substring(engineNo.Length - 5));
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(StickerQueries.GetBookingHistory, parameters);
            return result;
        }

        public async Task<dynamic> RosmertaApi(string VehicleRegNo, string ChassisNo, string EngineNo, string Key)
        {
            string html = string.Empty;
            string decryptedString = string.Empty;
            try
            {
                string vehicleapi = _vehicleStatusAPI;
                string url = @"" + vehicleapi + "?VehRegNo=" + VehicleRegNo + "&ChassisNo=" + ChassisNo + "&EngineNo=" + EngineNo + "&X=" + Key + "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            html = await reader.ReadToEndAsync();
                        }
                    }
                    else
                    {
                        html = "Vehicle Not Found";
                    }


            }
            catch (Exception ev)
            {
                html = "Error While Calling Vahan Service - " + ev.Message;
            }
            return html;

        }

        public async Task<dynamic> InsertVaahanLog(string VehicleRegNo, string chassisNo, string engineNo, VehicleDetails vahanDetailsDto)
        {
            var response = JsonConvert.SerializeObject(vahanDetailsDto);

            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo.ToUpper());
            parameters.Add("@ChassisNo", chassisNo.ToUpper());
            parameters.Add("@EngineNo", engineNo.ToUpper());
            parameters.Add("@Fuel", vahanDetailsDto.fuel);
            parameters.Add("@Norms", vahanDetailsDto.norms);
            parameters.Add("@VehicleCategory", vahanDetailsDto.vchCatg);
            parameters.Add("@VehicleType", vahanDetailsDto.vchType);
            parameters.Add("@Maker", vahanDetailsDto.maker);
            parameters.Add("@ResponseJson", response);
            parameters.Add("@RegistrationDate", vahanDetailsDto.regnDate);
            parameters.Add("@HsrpFrontLasserCode", vahanDetailsDto.hsrpFrontLaserCode);
            parameters.Add("@HsrpRearLasserCode", vahanDetailsDto.hsrpRearLaserCode);
            var insertVahanLog = await _databaseHelperPrimary.QueryAsync<dynamic>(StickerQueries.InsertVahanLog, parameters);
            return insertVahanLog;
        }

        public async Task<dynamic> checkVehicleForStickerPr(string RegNo, string FrontLaser, string RearLaser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegNo", RegNo);
            parameters.Add("@hsrp_front_lasercode", FrontLaser);
            parameters.Add("@hsrp_rear_lasercode", RearLaser);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(StickerQueries.checkVehicleForSticker, parameters);
            return result;
        }

        public async Task<dynamic> checkVehicleForStickerDL(string RegNo, string FrontLaser, string RearLaser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegNo", RegNo);
            parameters.Add("@hsrp_front_lasercode", FrontLaser);
            parameters.Add("@hsrp_rear_lasercode", RearLaser);
            var result = await _databaseHelperDL.QueryAsync<dynamic>(StickerQueries.checkVehicleForStickerDL, parameters);
            return result;
        }

        public async Task<dynamic> checkVehicleForStickerHR(string RegNo, string FrontLaser, string RearLaser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegNo", RegNo);
            parameters.Add("@hsrp_front_lasercode", FrontLaser);
            parameters.Add("@hsrp_rear_lasercode", RearLaser);
            var result = await _databaseHelperHR.QueryAsync<dynamic>(StickerQueries.checkVehicleForStickerDL, parameters);
            return result;
        }

        public async Task<dynamic> GetOemId(string makerName)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@MakerName", makerName);
            var oemId = await _databaseHelper.QueryAsync<dynamic>(
            StickerQueries.GetOemId, parameters);
            return oemId;
        }

        public async Task<dynamic> VehicleSession(string VehicleCatVahan)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleCatType", VehicleCatVahan);
            var vehicleSesion = await _databaseHelper.QueryAsync<dynamic>(StickerQueries.VehicleSession, parameters);
            return vehicleSesion;
        }

        public async Task<dynamic> VehiclePlateEntryLog(string SessionBs, string SessionRN, string SessionRD, string SessionCHN, string SessionEN, string SessionON, string SessionEID, string SessionMn, string SessionBA, string Stateid, string S_OrderType, string VehicleCat,string VehicleType,string S_StateId,string S_Oemid,string SFLCode,string SRLCode, string FuelType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SessionBs", SessionBs);
            parameters.Add("@SessionRN", SessionRN);
            parameters.Add("@SessionRD", SessionRD);
            parameters.Add("@SessionCHN", SessionCHN);
            parameters.Add("@SessionEN", SessionEN);
            parameters.Add("@SessionON", SessionON);
            parameters.Add("@SessionEID", SessionEID);
            parameters.Add("@SessionMn", SessionMn);
            parameters.Add("@SessionBA", SessionBA);

            parameters.Add("@Stateid", Stateid);
            parameters.Add("@S_OrderType", S_OrderType);
            parameters.Add("@VehicleCat", VehicleCat);
            parameters.Add("@VehicleType", VehicleType);
            parameters.Add("@S_StateId", Stateid);
            parameters.Add("@S_Oemid", S_Oemid);
            parameters.Add("@SFLCode", SFLCode);
            parameters.Add("@SRLCode", SRLCode);
            parameters.Add("@FuelType", FuelType);
            var vehicleSesion = await _databaseHelper.QueryAsync<dynamic>(StickerQueries.VehiclePlateEntryLog, parameters);
            return vehicleSesion;
        }

    }
}
