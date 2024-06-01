using iTextSharp.text.pdf.qrcode;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace BookMyHsrp.ReportsLogics.Receipt
{
    public class QRGenerator
    {
        //public void OnPostGenerate(string qrcode)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
        //        QRCode qrCode = new QRCode(qrCodeData);
        //        using (Bitmap bitMap = qrCode.GetGraphic(20))
        //        {
        //            bitMap.Save(ms, ImageFormat.Png);
        //            this.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}
    }
}
