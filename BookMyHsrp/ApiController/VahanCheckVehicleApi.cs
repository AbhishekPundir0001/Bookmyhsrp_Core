using System.Net;

namespace BookMyHsrp.ApiController
{
    public class VahanCheckVehicleApi
    {
        //public static string VahanCheckVehicle(string vehRegNo, string chasiNo, string EngineNo, string Key)
        //{
        //    string html = string.Empty;
        //    string decryptedString = string.Empty;
        //    try
        //    {
        //        string vehicleapi = _vehicleStatusAPI;
        //        string url = @"" + vehicleapi + "?VehRegNo=" + VehicleRegNo + "&ChassisNo=" + ChassisNo + "&EngineNo=" + EngineNo + "&X=" + Key + "";
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.AutomaticDecompression = DecompressionMethods.GZip;

        //        using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
        //        using (Stream stream = response.GetResponseStream())
        //        using (StreamReader reader = new StreamReader(stream))
        //        {
        //            html = await reader.ReadToEndAsync();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ValidationException("Vahan is not working");

        //    }
        //    return html;
        //}
    }
}
