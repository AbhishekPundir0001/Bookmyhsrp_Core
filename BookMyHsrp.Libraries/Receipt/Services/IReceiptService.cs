using BookMyHsrp.Libraries.HsrpState.Models;
using BookMyHsrp.Libraries.Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Receipt.Services
{
    public interface IReceiptService
    {
        Task<dynamic> GetReceipt(dynamic dto);
        Task<dynamic> GetGSTIN(string StateId);
        string QRGenerate(string inputText, string OrderNo, string QRPath);

    }
}
