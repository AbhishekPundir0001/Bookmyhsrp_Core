using BookMyHsrp.Libraries.Receipt.Models;
using BookMyHsrp.Libraries.Receipt.Services;
using BookMyHsrp.Libraries.Sticker.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Abstractions;
using System.Text;

namespace BookMyHsrp.ReportsLogics.Receipt
{
    public class ReceiptConnector
    {
        private readonly IReceiptService _receiptService;

        public ReceiptConnector(IReceiptService receiptService)
        {
            _receiptService = receiptService ?? throw new ArgumentNullException(nameof(receiptService));
        }
    

    public string DownloadReceipt(string receiptPath, ReceiptModels.Receipt requestdto, string QRPath)
        {

            var result =  _receiptService.GetReceipt(requestdto).Result;

            string CompanyName = "ROSMERTA SAFETY SYSTEMS LIMITED";
            string CompanyNamePostFix = "(FORMERLY KNOWN AS ROSMERTA SAFETY SYSTEMS PRIVATE LIMITED)";
            //string AppointmentType = "Dealer";
            string AppointmentType = result[0].AppointmentType;
            string DealerAffixationCenterName = result[0].DealerAffixationCenterName;
            string DealerAffixationCenterAddress = result[0].DealerAffixationCenterAddress;
            string FitmentPersonName = result[0].FitmentPersonName;
            string FitmentPersonMobile = result[0].FitmentPersonMobile;
            string OrderDate = result[0].OrderDate;
            string OrderNo = result[0].OrderNo;
            string OrderStatus = result[0].OrderStatus;
            string SlotBookingDate = result[0].SlotBookingDate;
            string SlotTime = result[0].SlotTime;
            string OwnerName = result[0].OwnerName;
            string VehicleRegNo = result[0].VehicleRegNo;
            string fuelType = result[0].fuelType;
            string VehicleClass = result[0].VehicleClass;
            string VehicleType = result[0].VehicleType;
            string OrderType = result[0].ordertype;
            string oemid = result[0].oemid.ToString();
            decimal GstBasic_Amt = Convert.ToDecimal(result[0].BasicAamount) + Convert.ToDecimal(result[0].FitmentCharge);
            string ConvenienceFee = result[0].ConvenienceFee.ToString();
            string MRDCharges = "0";
            string HomeDeliveryCharge = result[0].HomeDeliveryCharge.ToString();
            string TotalAmount = result[0].TotalAmount.ToString();
            string ReceiptValidUpTo = result[0].ReceiptValidUpTo.ToString();
            double IGSTAmount = Convert.ToDouble(result[0].IGSTAmount);
            double CGSTAmount = Convert.ToDouble(result[0].CGSTAmount);
            double NetAmount = Convert.ToDouble(result[0].NetAmount);
            double SGSTAmount = Convert.ToDouble(result[0].SGSTAmount);
            string StateId = result[0].HSRP_StateID.ToString();

            var Result_GSTIN =  _receiptService.GetGSTIN(StateId).Result;
            string GSTIN = Result_GSTIN[0].GSTIN.ToString();

                string isSuperTag = result[0].isSuperTag.ToString();
                string SuperTagAmount = result[0].SuperTagAmount.ToString();
                string IGSTAmountST = string.Empty;

            string _qrurl = "https://bookmyhsrp.com/TrackOrder.aspx?oid=" + requestdto.OrderNo + "&vr=" + VehicleRegNo + "";
            string _qrPath = _receiptService.QRGenerate(_qrurl, OrderNo, QRPath);
            //string ReceiptPathQRCode = "https://chart.googleapis.com/chart?chs=80x80&cht=qr&chl=https://bookmyhsrp.com/TrackOrder.aspx?oid=" + OrderNo + "%26vr=" + VehicleRegNo + "&chld=L|1&choe=UTF-8"; // latest
            string ReceiptPathQRCode = QRPath+ OrderNo+".jpg";

            if (result[0].IGSTAmountST == null)
            {
                IGSTAmountST = "0.00";
            }
            else
            {
                IGSTAmountST = result[0].IGSTAmountST.ToString();
            }
            string CGSTAmountST = string.Empty;
            if (result[0].CGSTAmountST == null)
            {
                CGSTAmountST = "0.00";
            }
            else
            {
                CGSTAmountST = result[0].CGSTAmountST.ToString();
            }
            string SGSTAmountST = string.Empty;
           if (result[0].SGSTAmountST == null)
            {
                SGSTAmountST = "0.00";
            }
            else
            {
                SGSTAmountST = result[0].SGSTAmountST.ToString();
            }
            string totalamount = string.Empty;
           if (result[0].SuperTagTotalAmount == null)
            {
                totalamount = "0.00";
            }
            else
            {
                totalamount = result[0].SuperTagTotalAmount.ToString();
            }
            string CustomerAddress1 = string.Empty;
            if (result[0].CustomerAddress1 == null)
            {
                CustomerAddress1 = "";
            }
            else
            {
                CustomerAddress1 = result[0].CustomerAddress1.ToString();
            }
            string CustomerCity = string.Empty;
            if (result[0].CustomerCity == null)
            {
                CustomerCity = "";
            }
            else
            {
                CustomerCity = result[0].CustomerCity.ToString();
            }
            string CustomerPin = string.Empty;
            if(result[0].CustomerPin==null)
            {
                CustomerPin = "";
            }else
            {
                CustomerPin = result[0].CustomerPin.ToString();
            }

            string isFramrTag = result[0].IsFrame.ToString();
            string FrameTagAmount = string.Empty;
            if (result[0].FrameTagAmount == null)
            {
                FrameTagAmount = "0.00";
            }
            else
            {
                FrameTagAmount = result[0].FrameTagAmount.ToString();
            }
            string IGSTAmountFrm = string.Empty;
            if(result[0].IGSTAmountFrm == null)
            {
                IGSTAmountFrm = "0.00";
            }
            else
            {
                IGSTAmountFrm = result[0].IGSTAmountFrm.ToString();
            }
            string CGSTAmountFrm = string.Empty;
            if (result[0].CGSTAmountFrm == null)
            {
                CGSTAmountFrm = "0.00";
            }
            else
            {
                CGSTAmountFrm = result[0].CGSTAmountFrm.ToString();
            }
            string SGSTAmountFrm = string.Empty;
            if(result[0].SGSTAmountFrm == null)
            {
                SGSTAmountFrm = "0.00";
            }
            else
            {
                SGSTAmountFrm = result[0].SGSTAmountFrm.ToString();
            }
            string Frametotalamount = string.Empty;
            if (result[0].FrameTagTotalAmount==null)
            {
                Frametotalamount = "0.00";
            }
            else
            {
                Frametotalamount = result[0].FrameTagTotalAmount.ToString();
            }

                StringBuilder sbTable = new StringBuilder();
                sbTable.Clear();

                string ImgPath = receiptPath + "/" + "assetsfile/images/logo";

                //sbTable.Append("html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>\r\n <head>\r\n <style type='text/css'>\r\n\r\ntd {\r\n    border: 1px solid black; border-collapse: collapse; /* Example border styling */\r\n}table {\r\n    border-collapse: collapse;\r\n} </style>");

                sbTable.Append("<table style='width:100%;text-align:center;font-size: 10pt;border-collapse: collapse'  border='1'>");
                sbTable.Append("<tr>");
                sbTable.Append("<td>");

                sbTable.Append("<table style='width:100%;margin-top:0' border='0'>");
                sbTable.Append("<tr><td align='center'><b>Receipt of Payment & Appointment</b></td></tr>");
                sbTable.Append("</table>");

                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");
                sbTable.Append("<table style='width:100%;' border='0'>");
                sbTable.Append("<tr>");
                sbTable.Append("<td align='left'><img src='" + ImgPath + "/BookMyHSRP.png' height='25px'/></td>");
                //sb.Append("<td><span><img src='" + ImgPath + "/BookMyHSRP.png' height='13px'/><span><b>Receipt of Payment & Appointment</b><span text-align:right><img src='" + ImgPath + "/logo.png' height='13px'/></span></td>");
                sbTable.Append("<td style='padding-left:45px;'><img src='" + ImgPath + "/rosmerta-technologies.png' height='25px'/></td>");
                sbTable.Append("<td style='padding-left:90px;'><img src='" + ImgPath + "/logo.png' height='25px'/></td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");

                sbTable.Append("<table style='width:100%;' border='0'>");
                sbTable.Append("<tr>");
                sbTable.Append("<td style='text-align:left;'><b>" + CompanyName + "</b><br/><span style='font-size: 6px;'>" + CompanyNamePostFix + "</span><br/><span style='font-size:14px;'>https://bookmyhsrp.com<br/>HSRP Online Appointment Transaction Receipt<br/>GST No.: " + GSTIN + "</span></td>");
                //sbTable.Append("<td><b>Fitment Location:</b><br/>" + DealerAffixationCenterName + "<br/>" + DealerAffixationCenterAddress + "<br/>" + FitmentPersonName + "<br/>" + FitmentPersonMobile + "</td>");
                if (AppointmentType.ToString().Equals("Dealer"))
                {
                    sbTable.Append("<td><b>Fitment Location:</b><br/><span style='font-size:14px;'>" + DealerAffixationCenterName + "<br/>" + DealerAffixationCenterAddress + "<br/>" + FitmentPersonName + "<br/>" + FitmentPersonMobile + "</span></td>");
                }
                else
                {
                    sbTable.Append("<td><b>Fitment Location:</b><br/><span style='font-size:14px;'>" + DealerAffixationCenterAddress + "<br/>" + FitmentPersonName + "<br/>" + FitmentPersonMobile + "</span></td>");
                }
                sbTable.Append("</tr>");
                sbTable.Append("</table>");

                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");
                sbTable.Append("<table style='width:100%;margin-top:0' border='0'>");
                sbTable.Append("<tr margin-top:-10px;>");
                sbTable.Append("<td style='text-align:left;'>Order Date: <br/><span style='color: blue;font-size:14px;'>" + OrderDate + "</span></td>");
                sbTable.Append("<td>Order ID: <br/><span style='color: blue;font-size:14px;'>" + OrderNo + "</span></td>");
                sbTable.Append("<td>Order Status:<br/><span style='color: blue;font-size:14px;'>" + OrderStatus + "</span></td>");
                //sbTable.Append("<td>Appointment Date & Time: <br/><span style='color: blue;'>01-02-2023 09:00</span></td>");
                //sbTable.Append("<td>Appointment Date & Time: <br/><span style='color: blue;'>"+ SlotBookingDate + " " + SlotTime + "</span></td>");
                if (SlotBookingDate == "01-01-1900" || SlotBookingDate == null)
                {
                    sbTable.Append("<td>Appointment Date & Time: <br/><b>Fitment Date will be informed later via SMS and on Call</b></td>");
                }
                else
                {
                    sbTable.Append("<td>Appointment Date & Time: <br/><span style='color: blue;'>" + SlotBookingDate + " " + SlotTime + "</span></td>");
                }
                sbTable.Append("</tr>");

                sbTable.Append("</table>");
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");

                sbTable.Append("<table style='width:100%;' border='0'>");
                sbTable.Append("<tr>");
                sbTable.Append("<td style='text-align:left;'><b><u>Billing Detail</u></b><br/><b>Billing Name:</b><br/><span style='font-size:12px;'>" + OwnerName + "</span></td>");
                //sbTable.Append("<td><br/><b>Billing Mobile:</b><br/>"+MobileNo+"</td>");
                //sbTable.Append("<td colspan = '2' margin-left: 100;><br/><b>Billing Email ID:</b><br/>"+EmailID+"</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");

                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");

                sbTable.Append("<table style='width:100%;' border='0'>");
                sbTable.Append("<tr>");
                sbTable.Append("<td style='text-align:left;'><b><u>Vehicle Detail</u></b><br/><b>Vehicle Reg No: </b><br/><span style='font-size:14px;'>" + VehicleRegNo + "</span></td>");
                //sbTable.Append("<td><b>Vehicle Reg No: </b><br/>DL10C1996</td>");
                sbTable.Append("<td><br/><b>Vehicle Fuel:</b><br/><span style='font-size:14px;'>" + fuelType + "</span></td>");
                sbTable.Append("<td><br/><b>Vehicle Class:</b><br/><span style='font-size:14px;'>" + VehicleClass + "</span></td>");
                sbTable.Append("<td><br/><b>Vehicle Type:</b><br/><span style='font-size:14px;'>" + VehicleType + "</span></td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");

                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");

                sbTable.Append("<table style='width:100%;' border='0'>");
                sbTable.Append("<tr>");

                if (OrderStatus.ToString().Equals("Success") || OrderStatus.ToString().Equals("Shipped"))
                {
                    //sbTable.Append("<tr>");
                    if (OrderType.ToUpper().Trim() == "DR")
                    {
                        if (oemid == "46")
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Rear Plate) (including Fitment Cost):</span></td>");
                        }
                        else
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Rear Plate) (including Service & Fitment Cost):</span></td>");
                        }
                    }
                    else if (OrderType.ToUpper().Trim() == "DF")
                    {
                        if (oemid == "46")
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Front Plate) <br/> (including Fitment Cost):</span></td>");
                        }
                        else
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Front Plate) <br/> (including Service & Fitment Cost):</span></td>");
                        }
                    }
                    else if (OrderType.ToUpper().Trim() == "OB" || OrderType.ToUpper().Trim() == "DB")
                    {
                        if (oemid == "46")
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Complete set) <br/>  (including Fitment Cost):</span></td>");
                        }
                        else
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Complete set) <br/>  (including Service & Fitment Cost):</span></td>");
                        }
                    }
                    else
                    {
                        if (oemid == "46")
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Complete set) <br/>  (including Fitment Cost):</span></td>");
                        }
                        else
                        {
                            sbTable.Append("<td style='text-align:left;'><b><u>Payment Detail</u></b><br/><span style='font-size:14px;'>HSRP Cost (Complete set) <br/>  (including Service & Fitment Cost):</span></td>");
                        }
                    }

                    sbTable.Append("<td><br/><br/><span style='font-size:14px;text-align:left;'>" + string.Format("{0:0.00}", GstBasic_Amt) + " INR</span></td>");
                    sbTable.Append("<td><img height='70px' width='70px' src='" + ReceiptPathQRCode + "' id='img_qrcode'/></td>");
                    //sbTable.Append("<td></td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("<tr style='line-height:80%;'>");
                    sbTable.Append("<td style='text-align:left;font-size:14px;'>Convenience Fee :</td>");
                    sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'>" + ConvenienceFee + " INR</td>");
                    sbTable.Append("</tr>");

                    if (MRDCharges != "0.00")
                    {
                        sbTable.Append("<tr style='line-height:80%;'>");
                        sbTable.Append("<td style='text-align:left;font-size:14px;'>MRD Charges :</td>");
                        sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'>" + MRDCharges + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    if (AppointmentType.ToString().Equals("Home"))
                    {
                        sbTable.Append("<tr style='line-height:80%;'>");
                        sbTable.Append("<td style='text-align:left;font-size:14px;'>Home Delivery :</td>");
                        sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'>" + HomeDeliveryCharge + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    sbTable.Append("<tr style='line-height:80%;'>");
                    sbTable.Append("<td style='text-align:left;font-size:14px;'>Gross Total :</td>");
                    sbTable.Append("<td style='font-size:14px;text-align:left;'>" + TotalAmount + " INR</td>");
                    //sbTable.Append("<td>Receipt is valid till: <b> " + ReceiptValidUpTo + "</b></td>");
                    if (SlotBookingDate == "01-01-1900" || SlotBookingDate == null)
                    {
                        sbTable.Append("<td></td>");
                    }
                    else
                    {
                        sbTable.Append("<td style='font-size:14px;'>Receipt is valid till: <b> " + ReceiptValidUpTo + "</b></td>");
                    }

                    sbTable.Append("</tr>");

                    if (IGSTAmount != 0)
                    {
                        sbTable.Append("<tr style='line-height:80%;'>");
                        sbTable.Append("<td style='text-align:left;font-size:14px;'>IGST Amount :</td>");
                        sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'>" + string.Format("{0:0.00}", IGSTAmount) + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    if (CGSTAmount != 0)
                    {
                        sbTable.Append("<tr style='line-height:80%;'>");
                        sbTable.Append("<td style='text-align:left;font-size:14px;'>CGST Amount :</td>");
                        sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'>" + string.Format("{0:0.00}", CGSTAmount) + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    if (SGSTAmount != 0)
                    {
                        sbTable.Append("<tr style='line-height:80%;'>");
                        sbTable.Append("<td style='text-align:left;font-size:14px;'>SGST Amount :</td>");
                        sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'>" + string.Format("{0:0.00}", SGSTAmount) + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    sbTable.Append("<tr style='line-height:80%;'>");
                    sbTable.Append("<td style='text-align:left;font-size:14px;'><b>Grand Total :</b></td>");
                    sbTable.Append("<td colspan='2' style='font-size:14px;text-align:left;'><b>" + NetAmount + " INR</b></td>");
                    sbTable.Append("</tr>");
                }

                sbTable.Append("</table>");

                sbTable.Append("</td>");
                sbTable.Append("</tr>");

            //-------------------------------------

            string HindiImgPath = receiptPath + "/" + "HindiLang";//Server.MapPath("~/HindiLang");
            //string HindiImgPath = ConfigurationManager.AppSettings["ReceiptDirectory"].ToString() + "/HindiLang/dealer";

                sbTable.Append("<tr>");
                sbTable.Append("<td>");
                sbTable.Append("<table style='width:100%;' border='0' cellpadding='0' cellspacing='0'>");
                sbTable.Append("<tr>");
                sbTable.Append("<td style='text-align:left;'><u><b>Instructions</b></u></td>");
                sbTable.Append("</tr>");

                if (VehicleRegNo.Trim().ToLower().StartsWith("dl") == true)
                {
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>1. Please show this Receipt and R.C Copy at the time of fitment. </td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>2. Please ensure respective vehicle in which the HSRP has to be Affixed should be available. </td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>3. As per the M.V Act (Transport Ministry), The HSRP will be affixed in a respective vehicle only. The HSRP will not be Handed over to the Vehicle Owner. </td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>4. Changes in appointment date (if any) will only be available for a future date,on paid basis.</td></tr>");

                    if (AppointmentType.ToString().Equals("Dealer"))
                    {
                        sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>5. For Dealer Point Grievance – please contact at +91 8929722203(Calling time 9.00 AM to 6 PM (Monday to Saturday) and Email ID is grievance@bookmyhsrp.com </td></tr>");
                    }
                    if (AppointmentType.ToString().Equals("Home"))
                    {
                        sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>5. For Home Fixation Grievance – please contact at +91 8929722202(Calling time 9.00 AM to 6 PM (Monday to Saturday) and Email ID is homegrievance@bookmyhsrp.com </td></tr>");
                    }

                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>6. Fitment charges have already been paid by you, no extra payment is required to be paid to the fitment person/dealer team.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>7. Please note that you can cancel your order within 4 hrs from Successful  booking. </td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>8. In case of fitment at the dealer's end, the responsibility of company would be to deliver the HSRP to the dealer's address as selected by the vehicle owner at the time of booking.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>9. It is the responsibility of the vehicle owner to bring the vehicle to the selected dealer to get the HSRP affixed on vehicle.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>10. If the HSRP is not affixed within six months from the original affixation date, it shall be destroyed and no refund shall be given in any circumstances.</td></tr>");
                }
                else if ((VehicleRegNo.Trim().ToLower().StartsWith("up") == true) || (VehicleRegNo.Trim().ToLower().StartsWith("mp") == true))
                {
                    //string HindiImgPath = ConfigurationManager.AppSettings["ReceiptDirectory"].ToString();

                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>1. Please show this Receipt and R.C Copy at the time of fitment. <img src='" + HindiImgPath + "/dealer/pt1.jpg' height='10px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>2. Please ensure respective vehicle in which the HSRP has to be Affixed should be available.<br/> <img src='" + HindiImgPath + "/dealer/pt2.jpg' height='10px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>3. As per the M.V Act (Transport Ministry), The HSRP will be affixed in a respective vehicle only. The HSRP will not be Handed over to the Vehicle Owner.<br/><img src='" + HindiImgPath + "/dealer/pt3.jpg' height='10px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>4. Changes in appointment date (if any) will only be available for a future date,on paid basis.<br/> <img src='" + HindiImgPath + "/dealer/pt4.jpg' height='10px'/></td></tr>");
                    if (AppointmentType.ToString().Equals("Dealer"))
                    {
                        sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>5. For Dealer Point Grievance – please contact at +91 8929722203(Calling time 9.00 AM to 6 PM (Monday to Saturday) and Email ID is grievance@bookmyhsrp.com <br/><img src='" + HindiImgPath + "/dealer/pt5.jpg' height='13px'/></td></tr>");
                    }
                    if (AppointmentType.ToString().Equals("Home"))
                    {
                        sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>5. For Home Fixation Grievance – please contact at +91 8929722202(Calling time 9.00 AM to 6 PM (Monday to Saturday) and Email ID is homegrievance@bookmyhsrp.com<br/><img src='" + HindiImgPath + "/home/pt5.jpg' height='13px'/></td></tr>");
                    }
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>6. Fitment charges have already been paid by you, no extra payment is required to be paid to the fitment person/dealer team. <img src='" + HindiImgPath + "/dealer/pt6.jpg' height='10px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>7. Please note that you can cancel your order within 4 hrs from Successful  booking. <img src='" + HindiImgPath + "/dealer/pt7.jpg' height='10px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>8. In case of fitment at the dealer's end, the responsibility of company would be to deliver the HSRP to the dealer's address as selected by the vehicle owner at the time of booking.<br/> <img src='" + HindiImgPath + "/dealer/pt8.jpg' height='10px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>9. It is the responsibility of the vehicle owner to bring the vehicle to the selected dealer to get the HSRP affixed on vehicle. <img src='" + HindiImgPath + "/dealer/pt9.jpg' height='14px'/></td></tr>");
                    sbTable.Append("<tr><td style='line-height:11px;text-align:left;font-size:14px;'>10. If the HSRP is not affixed within six months from the original affixation date, it shall be destroyed and no refund shall be given in any circumstances. <img src='" + HindiImgPath + "/dealer/pt10.jpg' height='10px'/></td></tr>");
                }
                else
                {
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>1. Carry this Receipt and RC Copy at the time of fitment.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>2. Bring you respective vehicle in which the HRSP has to be installed.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>3. Re-Appointment (if any) will only available for future date </td></tr>");

                if (StateId == "18")
                {
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>4. For Dealer Point Grievance – please contact at +91 9205262323(Calling time 9.30AM - 6PM (07 days)) and Email ID is cssupport@rosmertasafety.com </td></tr>");
                }
                else
                {
                    if (AppointmentType.ToString().Equals("Dealer"))
                    {
                        sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>4. For Dealer Point Grievance – please contact at +91 8929722203(Calling time 9.30AM - 6PM (07 days)) and Email ID is grievance@bookmyhsrp.com </td></tr>");
                    }
                    if (AppointmentType.ToString().Equals("Home"))
                    {
                        sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>4. For Home Fixation Grievance – please contact at +91 8929722202(Calling time 9.30 AM to 6 PM (07 days)) and Email ID is homegrievance@bookmyhsrp.com </td></tr>");
                    }
                }
                sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>5. Fitment charges paid, no extra payment required to be paid to fitment person/dealer team.</td></tr>");
                if (VehicleRegNo.Trim().ToLower().StartsWith("od") == true || VehicleRegNo.Trim().ToLower().StartsWith("or") == true)
                {
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>6. Warranty for five years applicable in compliance to S.O.6052 dated 06.12.2018 and Rule 50 of CMVR 1989.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>7. In case of fitment at the dealer's end, the responsibility of company would be to deliver the HSRP to the dealer's address as selected by the vehicle owner at the time of booking.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>8. It is the responsibility of the vehicle owner to bring the vehicle to the selected dealer to get the HSRP affixed on vehicle.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>9. If the HSRP is not affixed within six months from the original affixation date, it shall be destroyed and no refund shall be given in any circumstances.</td></tr>");
                }
                else
                {
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>6. In case of fitment at the dealer's end, the responsibility of company would be to deliver the HSRP to the dealer's address as selected by the vehicle owner at the time of booking.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>7. It is the responsibility of the vehicle owner to bring the vehicle to the selected dealer to get the HSRP affixed on vehicle.</td></tr>");
                    sbTable.Append("<tr><td style='text-align:left;font-size:14px;'>8. If the HSRP is not affixed within six months from the original affixation date, it shall be destroyed and no refund shall be given in any circumstances.</td></tr>");

                }
            }

                sbTable.Append("</table>");

                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                //-------------------------------------


            sbTable.Append("</table>");




            if (isSuperTag == "Y")
            {
                //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                //sbTable.Append("<br/>"); sbTable.Append("<br/>");


                if (VehicleRegNo.Trim().ToLower().StartsWith("dl") == true)
                {
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                }
                else if ((VehicleRegNo.Trim().ToLower().StartsWith("up") == true) || (VehicleRegNo.Trim().ToLower().StartsWith("mp") == true))
                {
                    //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                }
                else
                {
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                    //sbTable.Append("<br/>"); sbTable.Append("<br/>");
                }

                //sbTable.Append("<table style='margin-top: 162px;'>");
                sbTable.Append("<table style='margin-top: 250px;'>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td><b><h4>Receipt of Payment</h4></b></td>");
                    //sbTable.Append("<td rowspan=2><img height='70px' width='70px' src='" + ReceiptPathQRCode + "' id='img_qrcode'></td>");
                    sbTable.Append("<td>mhgj</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr><td rowspan=2><b>" + CompanyName + "</b><br/><span style='font-size: 6px;'>" + CompanyNamePostFix + "</span><br/> https://bookmyhsrp.com <br/>GST No.: CHGF4562DGD</td></tr>");
                    sbTable.Append("</table>");

                    ////----------------------------------------------------

                    sbTable.Append("<table style='font-size:medium;'>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td>Order Date :</td>");
                    sbTable.Append("<td>" + OrderDate + "</td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td><b>Order ID :</b></td>");
                    sbTable.Append("<td><b>" + OrderNo + "</b></td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td>Order Status :</td>");
                    sbTable.Append("<td><b>" + OrderStatus + "</b></td>");
                    sbTable.Append("</tr>");

                    if (OrderStatus.ToString().Equals("Success") || OrderStatus.ToString().Equals("Shipped"))
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td><b>Park+FasTag charges (Do it yourself) :</b></td>");
                        sbTable.Append("<td><b>" + string.Format("{0:0.00}", (Convert.ToDecimal(84.74)).ToString()) + " INR</b></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td width='55%'><b>Dispatch & Convenience Charge for Park+FasTag :</b></td>");
                        sbTable.Append("<td><b>" + string.Format("{0:0.00}", (Convert.ToDecimal(212.72)).ToString()) + " INR</b></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td><b>Gross Total :</b></td>");
                        sbTable.Append("<td><b>" + string.Format("{0:0.00}", (Convert.ToDecimal(SuperTagAmount) - 99).ToString()) + " INR</b></td>");
                        sbTable.Append("</tr>");

                        if (IGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>IGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", IGSTAmountST) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        if (CGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>CGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", CGSTAmountST) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        if (SGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>SGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", SGSTAmountST) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Sub Total :</td>");
                        sbTable.Append("<td>" + ((Convert.ToDecimal(SuperTagAmount) - 99) + Convert.ToDecimal(IGSTAmountST) + Convert.ToDecimal(CGSTAmountST) + Convert.ToDecimal(SGSTAmountST)).ToString() + " INR</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td><b>Park+FasTag Wallet Balance(Recharge):</b></td>");
                        sbTable.Append("<td><b>" + string.Format("{0:0.00}", "99") + " INR</b></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Grand Total :</td>");
                        sbTable.Append("<td>" + totalamount + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    sbTable.Append("<tr>");
                    sbTable.Append("<td>Billing Name :</td>");
                    sbTable.Append("<td>" + OwnerName + "</td>");
                    sbTable.Append("</tr>");

                    if (OrderStatus.ToString().Equals("Success") || OrderStatus.ToString().Equals("Shipped"))
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td><b>Delivery Time</b></td>");
                        sbTable.Append("<td><b>7 to 10 days</b></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Shipping Address :</td>");
                        sbTable.Append("<td><h4><b>" + CustomerAddress1 + " " + CustomerCity + " " + CustomerPin + "</b></h4></td>");
                        sbTable.Append("</tr>");
                    }

                    sbTable.Append("</table>");

                    sbTable.Append("<table>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td>1.The <b>Park+FasTag will be shipped directly to the shipping address</b> and will not delivered with HSRP or Sticker.</td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("</table>");
                }

                if (isFramrTag == "Y")
                {
                    sbTable.Append("<table style='margin-top: 449px;'>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td valign='bottom' width='50%' ><b><h4>Receipt of Payment of Frame</h4></b></td>");
                    //sbTable.Append("<td rowspan=2><img height='70px' width='70px' src='" + ReceiptPathQRCode + "' id='img_qrcode'></td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("<tr><td><p><br/> Rosmerta Safety System Pvt.Ltd. <br/> https://bookmyhsrp.com </p></td></tr>");
                    //sbTable.Append("<td><b>" + CompanyName + "</b><br/><span style='font-size: 6px;'>" + CompanyNamePostFix + "</span><br> https://bookmyhsrp.com </p><br/>GST No.: " + dtGSTIN.Rows[0]["GSTIN"].ToString() + "</td></tr>");
                    sbTable.Append("</table>");

                    sbTable.Append("<table>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td>Order Date :</td>");
                    sbTable.Append("<td>" + OrderDate + "</td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td><b>Order ID :</b></td>");
                    sbTable.Append("<td><b>" + OrderNo + "</b></td>");
                    sbTable.Append("</tr>");

                    sbTable.Append("<tr>");
                    sbTable.Append("<td>Order Status :</td>");
                    sbTable.Append("<td><b>" + OrderStatus + "</b></td>");
                    sbTable.Append("</tr>");

                    if (OrderStatus.ToString().Equals("Success") || OrderStatus.ToString().Equals("Shipped"))
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td><b>Frame charges  :</b></td>");
                        sbTable.Append("<td><b>" + string.Format("{0:0.00}", (Convert.ToDecimal(FrameTagAmount)).ToString()) + " INR</b></td>");
                        sbTable.Append("</tr>");

                        if (IGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>IGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", IGSTAmountFrm) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        if (CGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>CGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", CGSTAmountFrm) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        if (SGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>SGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", SGSTAmountFrm) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Grand Total :</td>");
                        sbTable.Append("<td>" + Frametotalamount + " INR</td>");
                        sbTable.Append("</tr>");
                    }

                    sbTable.Append("<tr>");
                    sbTable.Append("<td>Billing Name :</td>");
                    sbTable.Append("<td>" + OwnerName + "</td>");
                    sbTable.Append("</tr>");

                    if (OrderStatus.ToString().Equals("Success") || OrderStatus.ToString().Equals("Shipped"))
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Fitment Location :</td>");
                        sbTable.Append("<td><h4><b>" + DealerAffixationCenterName + "</b></h4></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Shipping Address :</td>");
                        sbTable.Append("<td><h4><b>" + DealerAffixationCenterAddress + "</b></h4></td>");
                        sbTable.Append("</tr>");
                    }
                    sbTable.Append("</table>");

                    sbTable.Append("<table>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td>1.The <b>Frame will be shipped with HSRP to the shipping address</b></td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("</table>");
                }






            var html =  sbTable.ToString();
            return html;

        }

                return "incorrect" ;
            }
        }
    }
}
