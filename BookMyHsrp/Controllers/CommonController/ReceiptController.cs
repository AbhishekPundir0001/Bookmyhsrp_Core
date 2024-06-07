using BookMyHsrp.ReportsLogics.Receipt;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using BookMyHsrp.ReportsLogics.Sticker;
using BookMyHsrp.Libraries.Receipt.Models;
using BookMyHsrp.Dapper;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using iTextSharp.text.pdf.qrcode;
using QRCoder;
using System.Drawing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Components.Forms;
using System.Drawing.Imaging;
using QRCode = QRCoder.QRCode;
using StackExchange.Redis;
using BookMyHsrp.Libraries.Receipt.Services;
using static BookMyHsrp.Libraries.Receipt.Models.ReceiptModels;

namespace BookMyHsrp.Controllers.CommonController
{
    public class ReceiptController : Controller
    {
        private readonly ILogger<StickerController> _logger;

        public readonly ReceiptConnector _receiptConnector;

        private readonly IWebHostEnvironment _environment;
        private readonly string receiptPath;
        private readonly IReceiptService _receiptService;
        private readonly string QRPath;


        public ReceiptController(ILogger<StickerController> logger, IWebHostEnvironment environment, ReceiptConnector receiptConnector, IOptionsSnapshot<DynamicDataDto> dynamicDto, IReceiptService receiptService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _receiptConnector = receiptConnector ?? throw new ArgumentNullException(nameof(receiptConnector));
            _environment = environment;
            receiptPath = dynamicDto.Value.ReceiptPath;
            QRPath = dynamicDto.Value.QRPath;
            _receiptService = receiptService ?? throw new ArgumentNullException(nameof(receiptService));

        }

        public IActionResult Receipt()  
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult DownloadReceipt([FromBody] ReceiptModels.Receipt requestdto)
        {
            string contentPath = _environment.ContentRootPath;
            string folderpath = receiptPath;
            string html = _receiptConnector.DownloadReceipt(contentPath, requestdto, QRPath);

            string filePath = string.Empty; // Define a variable to store the file path

            if (html == "incorrect")
            {
                return Ok("Cannot find pdf");
            }
            else
            {
                if (requestdto.OrderNo.Substring(0, 2) == "BM")
                {
                    string MonthYears = DateTime.Now.ToString("MMM-yyyy");
                    string filename = requestdto.OrderNo + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    string PdfFolder = folderpath;
                    filePath = Path.Combine(PdfFolder, "Plate", MonthYears, filename); // Define the file path for saving
                    if (!Directory.Exists(PdfFolder + "\\" + "Plate"))
                    {
                        Directory.CreateDirectory(PdfFolder + "\\" + "Plate");
                    }

                    if (!Directory.Exists(PdfFolder + "\\" + "Plate" + "\\" + MonthYears))
                    {
                        Directory.CreateDirectory(PdfFolder + "\\" + "Plate" + "\\" + MonthYears);
                    }
                }
                else
                {
                    string MonthYears = DateTime.Now.ToString("MMM-yyyy");
                    string filename = requestdto.OrderNo + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                    string PdfFolder = folderpath;
                    filePath = Path.Combine(PdfFolder, "Sticker", MonthYears, filename); // Define the file path for saving
                    if (!Directory.Exists(PdfFolder + "\\" + "Sticker" + "\\" + MonthYears))
                    {
                        Directory.CreateDirectory(PdfFolder + "\\" + "Sticker" + "\\" + MonthYears);
                    }
                }


            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(html);

                // Create a Document with specified page size and margins
                Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

                // Create a PdfWriter to write the PDF to the file stream
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                // Open the document
                doc.Open();

                // Parse HTML and convert it to PDF
                XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, doc, txtReader);

                // Close the document
                doc.Close();

                // Reset the stream position to the beginning
                ms.Position = 0;

            // Return the PDF file path

            //var response = new ResponseSticker();
            //response.Message = filePath;


            //var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(response);
            //return Json(jsonSerializer);

                if (System.IO.File.Exists(filePath))
                {
                    // Read the file contents into a byte array
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file as a byte array with Content-Type application/pdf
                return File(fileBytes, "application/pdf", requestdto.OrderNo+ ".pdf");
            }
            else
            {
                // Return a 404 Not Found response if the file doesn't exist
                return NotFound();
            }



        }
    }
}



