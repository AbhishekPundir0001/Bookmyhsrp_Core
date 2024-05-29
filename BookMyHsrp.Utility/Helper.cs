using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Utility
{
    public class Helper
    {
        public static string ConvertDateFromStandardToAbbreviatedFormat(string originalDateString)
        {
            DateTime parsedDate = DateTime.ParseExact(originalDateString,
                "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string convertedDateString = parsedDate.ToString("dd-MMM-yyyy");
            return convertedDateString;
        }
        public static string ConvertDateFormatMMddyyyyToyyyymmdd(string inputDate)
        {
            try
            {
                // Parse the input date
                DateTime parsedDate = DateTime.ParseExact(inputDate, "MM-dd-yyyy", null);

                // Convert it to the desired format "yyyy-MM-dd"
                return parsedDate.ToString("yyyy-MM-dd");
            }
            catch (FormatException)
            {
                // Handle invalid date format
                return "Invalid date format";
            }
        }

        public static string ConvertDateFormatddMMyyyytoyyyyMMdd(string inputDate)
        {
            try
            {
                // Parse the input date in the "dd-MM-yyyy" format
                DateTime parsedDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", null);

                // Convert it to the desired format "yyyy-MM-dd"
                return parsedDate.ToString("yyyy-MM-dd");
            }
            catch (FormatException)
            {
                // Handle invalid date format
                return "Invalid date format";
            }
        }

        public static string ConvertDateFormatddMMyyyytoMMddyyyy(string inputDate)
        {
            try
            {
                // Parse the input date in the "dd-MM-yyyy" format
                DateTime parsedDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", null);

                // Convert it to the desired format "MM-dd-yyyy"
                return parsedDate.ToString("MM-dd-yyyy");
            }
            catch (FormatException)
            {
                // Handle invalid date format
                return "Invalid date format";
            }
        }

        public static string GetDateTimeString() =>
            DateTime.Now.ToString("dd-MM-yyy hh:mm:ss");
        public static string GetExcelName(string fileName) => $"{fileName}-{GetDateTimeString()}.xlsx";
        public static bool HasProperty(dynamic obj, string name)
        {
            return obj.GetType().GetProperty(name) != null;
        }
    }

}

