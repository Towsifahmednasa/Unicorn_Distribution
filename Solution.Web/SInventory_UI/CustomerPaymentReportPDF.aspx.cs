using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SInventory_UI_CustomerPaymentReportPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReport();
        }
    }

    private void LoadReport()
    {
        string fromDateStr = Request.QueryString["FromDate"];
        string toDateStr = Request.QueryString["ToDate"];

        DateTime fDate, tDate;
        if (!DateTime.TryParse(fromDateStr, out fDate)) fDate = DateTime.Now.AddDays(-30);
        if (!DateTime.TryParse(toDateStr, out tDate)) tDate = DateTime.Now;

        litPeriod.Text = fDate.ToString("dd-MMM-yyyy") + " to " + tDate.ToString("dd-MMM-yyyy");

        DataTable dt = GetData(fDate, tDate);
        rptPayments.DataSource = dt;
        rptPayments.DataBind();

        decimal grandTotal = 0;
        if (dt != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["Amount"] != DBNull.Value)
                {
                    decimal val = 0;
                    decimal.TryParse(row["Amount"].ToString(), out val);
                    grandTotal += val;
                }
            }
        }
        litGrandTotal.Text = grandTotal.ToString("#,##0.00");
    }

    private DataTable GetData(DateTime fDate, DateTime tDate)
    {
        DataTable dt = new DataTable();
        string connStr = "";
        if (ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"] != null)
        {
            connStr = ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ConnectionString;
        }
        else if (ConfigurationManager.ConnectionStrings["Con"] != null)
        {
            connStr = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
        }

        if (string.IsNullOrEmpty(connStr)) return dt;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT 
                    COALESCE(accV.VoucherCode, masPay.RefNo, 'CT' + RIGHT('00000000' + CONVERT(VARCHAR, masPay.CustPayId), 8)) AS VoucherNo,
                    masPay.PaymentDate                                                                                            AS PaymentDate,
                    CASE 
                        WHEN masPay.PayType LIKE '%Cash%' THEN 'General Cash'
                        WHEN ISNULL(masPay.PayType, '') = 'EFT' THEN 'General Cash'
                        ELSE 'General Cash'
                    END                                                                                                           AS BankCash,
                    ISNULL(masPay.PayType, 'Cash Deposit')                                                                        AS ChequeNo,
                    CASE 
                        WHEN ISNULL(NULLIF(masPay.Remarks, ''), '') <> '' THEN masPay.Remarks
                        ELSE ISNULL(masPay.PayType, 'Cash') + ' by ' + ISNULL(CSTMR.CustomerName, 'Customer')
                    END                                                                                                           AS Narration,
                    ISNULL(masPay.PaymentAmount, ISNULL(dtl.TotalDetailAmount, 0))                                                AS Amount,
                    ISNULL(masPay.CreateBy, 'admin')                                                                              AS UserName
                FROM tblCustomerPay AS masPay WITH(NOLOCK)
                LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS accV WITH(NOLOCK)
                    ON masPay.CustPayId = accV.CommonPK_ID AND accV.TableID = 2017
                LEFT JOIN tblCustMaster AS CSTMR WITH(NOLOCK) ON masPay.CustomerMasterId = CSTMR.CustomerMasterId
                LEFT JOIN tblCompanyUnit AS com WITH(NOLOCK) ON masPay.ComUnitId = com.ComUnitId
                LEFT JOIN (
                    SELECT 
                        CustPayId, 
                        SUM(ISNULL(PaymentAmount, 0)) AS TotalDetailAmount
                    FROM tblCustPayDetail WITH(NOLOCK)
                    GROUP BY CustPayId
                ) AS dtl ON masPay.CustPayId = dtl.CustPayId
                WHERE masPay.PaymentDate >= @FromDate 
                  AND masPay.PaymentDate <= @ToDate 
                ORDER BY masPay.CustPayId DESC";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FromDate", fDate.ToString("yyyy-MM-dd 00:00:00"));
                cmd.Parameters.AddWithValue("@ToDate", tDate.ToString("yyyy-MM-dd 23:59:59"));

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        return dt;
    }
}
