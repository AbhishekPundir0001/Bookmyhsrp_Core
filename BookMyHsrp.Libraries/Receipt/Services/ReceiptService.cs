using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using BookMyHsrp.Libraries.OemMaster.Models;
using BookMyHsrp.Libraries.OemMaster.Queries;
using BookMyHsrp.Libraries.Receipt.Models;
using BookMyHsrp.Libraries.Receipt.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using QRCoder;
using System.Drawing;

namespace BookMyHsrp.Libraries.Receipt.Services
{
    public class ReceiptService : IReceiptService
    {
        //private readonly FetchDataAndCache _fetchDataAndCache;
        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;

        public ReceiptService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }


        public async Task<dynamic> GetReceipt(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", dto.OrderNo);

            var receipts =  await _databaseHelper.QueryAsync<dynamic>(ReceiptQueries.strReceipt, parameters);
            return receipts;
        }

        public async Task<dynamic> GetGSTIN(string StateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", StateId);

            var receipts = await _databaseHelper.QueryAsync<dynamic>(ReceiptQueries.strGSTIN, parameters);
            return receipts;
        }

        public string QRGenerate(string inputText, string OrderNo, string QRPath)
        {
            if (!Directory.Exists(QRPath))
            {
                Directory.CreateDirectory(QRPath);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator oQRCodeGenerator = new QRCodeGenerator();
                QRCodeData oQRCodeData = oQRCodeGenerator.CreateQrCode(inputText, QRCodeGenerator.ECCLevel.Q);
                QRCode qQRCode = new QRCode(oQRCodeData);

                using (Bitmap oBitmap = qQRCode.GetGraphic(20))
                {
                    //oBitmap.Save(ms, ImageFormat.Png);
                    //ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    oBitmap.Save(QRPath + "\\" + OrderNo + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }

            return QRPath + "\\" + OrderNo + ".jpg";
        }


    }
}
