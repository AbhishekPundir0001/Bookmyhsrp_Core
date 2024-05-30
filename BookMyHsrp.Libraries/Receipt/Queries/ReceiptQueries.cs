using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Receipt.Queries
{
    public class ReceiptQueries
    {
        //public static string strReceipt => "exec PaymentPlateReceipt @OrderNo";
        public static string strReceipt => "exec PaymentPlateReceipt_MX @OrderNo";

        public static string strGSTIN => "select GSTIN, CompanyName from [HSRPOEM].dbo.HSRPState where HSRP_StateID=@StateId";
    }
}
