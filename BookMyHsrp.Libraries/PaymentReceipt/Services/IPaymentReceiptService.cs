using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.PaymentReceipt.Services
{
    public interface IPaymentReceiptService
    {
        Task<dynamic> UpdateStatusOfPayment(string OrderNo);
    }
}
