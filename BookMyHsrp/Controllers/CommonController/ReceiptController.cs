using BookMyHsrp.ReportsLogics.Receipt;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using BookMyHsrp.ReportsLogics.Sticker;
using BookMyHsrp.Libraries.Receipt.Models;
using BookMyHsrp.Dapper;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using iTextSharp.tool.xml;

namespace BookMyHsrp.Controllers.CommonController
{
    public class ReceiptController : Controller
    {
        private readonly ILogger<StickerController> _logger;

        public readonly ReceiptConnector _receiptConnector;

        private readonly IWebHostEnvironment _environment;
        private readonly string receiptPath;



        public ReceiptController(ILogger<StickerController> logger, IWebHostEnvironment environment, ReceiptConnector receiptConnector, IOptionsSnapshot<DynamicDataDto> dynamicDto)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _receiptConnector = receiptConnector ?? throw new ArgumentNullException(nameof(receiptConnector));
            _environment = environment;
            receiptPath = dynamicDto.Value.ReceiptPath;

        }

        public IActionResult Receipt()
        {
            return View();
        }

        public FileStreamResult DownloadReceipt(ReceiptModels.Receipt requestdto)
        {
            string contentPath = _environment.ContentRootPath;
            string html = _receiptConnector.DownloadReceipt(contentPath, requestdto);
            string folderpath = receiptPath;
            string filePath = string.Empty;
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
            //TextReader txtReader = new StringReader(HTMLContent);
            TextReader txtReader = new StringReader(html);

            // Create a Document with specified page size and margins
            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 25, 25, 25, 25);

            // Create a PdfWriter to write the PDF to the MemoryStream
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, ms);
            pdfWriter.CloseStream = false;

            // Open the document
            doc.Open();

            // Parse HTML and convert it to PDF
            XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, doc, txtReader);

            // Close the document
            doc.Close();

            // Reset the stream position to the beginning
            ms.Position = 0;

            // Return the PDF as a file download
            return new FileStreamResult(ms, "application/pdf")
            {
                FileDownloadName = requestdto.OrderNo + ".pdf"
            };
        }

    }
}
