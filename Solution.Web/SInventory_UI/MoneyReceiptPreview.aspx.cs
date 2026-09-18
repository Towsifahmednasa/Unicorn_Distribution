using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_MoneyReceiptPreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMoneyReceipt();
        }
    }

    private void LoadMoneyReceipt()
    {
        try
        {
            string receiptNo = Request.QueryString["ReceiptNo"];
            string custPayId = Request.QueryString["CustPayId"];
            string invoiceId = Request.QueryString["InvoiceId"];
            string copyType = Request.QueryString["CopyType"];

            if (!string.IsNullOrEmpty(copyType))
            {
                litCopyType.Text = copyType;
            }

            if (!string.IsNullOrEmpty(receiptNo) || !string.IsNullOrEmpty(custPayId) || !string.IsNullOrEmpty(invoiceId))
            {
                FetchDataFromDatabase(receiptNo, custPayId, invoiceId);
            }
            else
            {
                FetchDataFromDatabase("", "", "");
            }
        }
        catch (Exception)
        {
        }
    }

    private void FetchDataFromDatabase(string receiptNo, string custPayId, string invoiceId)
    {
        string connStr = "";
        if (ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"] != null)
        {
            connStr = ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ConnectionString;
        }
        else if (ConfigurationManager.ConnectionStrings["Con"] != null)
        {
            connStr = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
        }

        if (string.IsNullOrEmpty(connStr)) return;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT TOP 1
                    COALESCE(accV.VoucherCode, NULLIF(masPay.RefNo, ''), 'RP' + RIGHT('0000000' + CONVERT(VARCHAR, masPay.CustPayId), 7)) AS ReceiptNo,
                    masPay.PaymentDate,
                    ISNULL(CSTMR.CustomerName, '') + CASE WHEN ISNULL(mkt.MarketName, '') <> '' THEN ' (' + mkt.MarketName + ')' ELSE '' END AS CustomerName,
                    ISNULL(masPay.PayType, 'Cash') AS PayType,
                    masPay.RefDate AS ChequeDate,
                    ISNULL(b.BankName, 'NONE') AS CustomerBank,
                    ISNULL(INV.InvoiceNo, '') AS InvoiceNo,
                    ISNULL(custPay.PaymentAmount, masPay.PaymentAmount) AS PaymentAmount,
                    ISNULL(com.ComUnitName, 'Unicorn Healthcare Solution Ltd') AS CompanyName,
                    ISNULL(com.Address, 'Floor – 8/A, Dynesty Wahid Tower 56/2 Panthapath, Dhaka-1205, Bangladesh .') AS Address
                FROM tblCustomerPay AS masPay WITH(NOLOCK)
                LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS accV WITH(NOLOCK)
                    ON masPay.CustPayId = accV.CommonPK_ID AND accV.TableID = 2017
                LEFT JOIN tblCustPayDetail AS custPay WITH(NOLOCK) ON masPay.CustPayId = custPay.CustPayId
                LEFT JOIN tblInvoice AS INV WITH(NOLOCK) ON custPay.InvoiceId = INV.InvoiceId
                LEFT JOIN tblCustMaster AS CSTMR WITH(NOLOCK) ON masPay.CustomerMasterId = CSTMR.CustomerMasterId
                LEFT JOIN tblMarket AS mkt WITH(NOLOCK) ON CSTMR.MarketId = mkt.MarketId
                LEFT JOIN tblCompanyUnit AS com WITH(NOLOCK) ON masPay.ComUnitId = com.ComUnitId
                LEFT JOIN tblBankInfo AS b WITH(NOLOCK) ON custPay.BankAccId = b.BankId
                WHERE 1=1 ";

            int recNum = 0;
            if (!string.IsNullOrEmpty(receiptNo))
            {
                string cleaned = receiptNo.Replace("RP", "").Replace("MR-", "").Replace("CT", "").Replace("CVB-", "").Replace("CVC-", "").TrimStart('0');
                int.TryParse(cleaned, out recNum);
                sql += " AND (accV.VoucherCode = @ReceiptNo OR masPay.RefNo = @ReceiptNo OR masPay.CustPayId = @ReceiptNoNumeric) ";
            }
            if (!string.IsNullOrEmpty(custPayId))
            {
                sql += " AND masPay.CustPayId = @CustPayId ";
            }
            if (!string.IsNullOrEmpty(invoiceId))
            {
                sql += " AND custPay.InvoiceId = @InvoiceId ";
            }

            sql += " ORDER BY masPay.CustPayId DESC ";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (!string.IsNullOrEmpty(receiptNo))
                {
                    cmd.Parameters.AddWithValue("@ReceiptNo", receiptNo);
                    cmd.Parameters.AddWithValue("@ReceiptNoNumeric", recNum);
                }
                if (!string.IsNullOrEmpty(custPayId))
                {
                    cmd.Parameters.AddWithValue("@CustPayId", custPayId);
                }
                if (!string.IsNullOrEmpty(invoiceId))
                {
                    cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                }

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        litReceiptNo.Text = reader["ReceiptNo"].ToString();
                        
                        if (reader["PaymentDate"] != DBNull.Value)
                        {
                            DateTime dt = Convert.ToDateTime(reader["PaymentDate"]);
                            litReceiptDate.Text = dt.ToString("dd-MMM-yyyy");
                        }

                        litCustomer.Text = reader["CustomerName"].ToString();
                        litChequeNo.Text = string.IsNullOrEmpty(reader["PayType"].ToString()) ? "Cash" : reader["PayType"].ToString();

                        if (reader["ChequeDate"] != DBNull.Value)
                        {
                            DateTime cdt = Convert.ToDateTime(reader["ChequeDate"]);
                            litChequeDate.Text = cdt.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            litChequeDate.Text = "";
                        }

                        litCustomerBank.Text = string.IsNullOrEmpty(reader["CustomerBank"].ToString()) ? "NONE" : reader["CustomerBank"].ToString();
                        litReceivedAgainst.Text = reader["InvoiceNo"].ToString();

                        decimal amt = 0;
                        if (decimal.TryParse(reader["PaymentAmount"].ToString(), out amt))
                        {
                            litAmount.Text = "TK " + amt.ToString("#,##0.00");
                            litAmountInWords.Text = NumberToWords(amt);
                        }

                        if (!string.IsNullOrEmpty(reader["CompanyName"].ToString()))
                        {
                            litCompanyName.Text = reader["CompanyName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(reader["Address"].ToString()))
                        {
                            litCompanyAddress.Text = reader["Address"].ToString();
                        }
                    }
                }
            }
        }
    }

    public static string NumberToWords(decimal number)
    {
        if (number == 0)
            return "Zero Only";

        long intPortion = (long)Math.Floor(number);
        int decimalPortion = (int)((number - intPortion) * 100);

        string words = "Taka " + ConvertIntegerToWords(intPortion);

        if (decimalPortion > 0)
        {
            words += " and " + ConvertIntegerToWords(decimalPortion) + " Paisa";
        }

        words += " Only";
        return words;
    }

    private static string ConvertIntegerToWords(long number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + ConvertIntegerToWords(Math.Abs(number));

        string words = "";

        if ((number / 10000000) > 0)
        {
            words += ConvertIntegerToWords(number / 10000000) + " Crore ";
            number %= 10000000;
        }

        if ((number / 100000) > 0)
        {
            words += ConvertIntegerToWords(number / 100000) + " Lakh ";
            number %= 100000;
        }

        if ((number / 1000) > 0)
        {
            words += ConvertIntegerToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 1000) > 0)
        {
            words += ConvertIntegerToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += ConvertIntegerToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words.Trim();
    }
}
