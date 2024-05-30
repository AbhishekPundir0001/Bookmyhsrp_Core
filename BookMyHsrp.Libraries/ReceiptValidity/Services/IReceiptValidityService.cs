using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReceiptValidity.Services
{
    public interface IReceiptValidityService
    {
        Task<dynamic> CheckReceiptValidity(dynamic dto);
    }
}
