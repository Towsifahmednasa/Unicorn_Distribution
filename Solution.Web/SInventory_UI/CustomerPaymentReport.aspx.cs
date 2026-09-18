using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SInventory_UI_CustomerPaymentReport : System.Web.UI.Page
{
    private decimal totalAmount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fromDateTextBox.Text = DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy");
            toDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            LoadReportData();
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadReportData();
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        fromDateTextBox.Text = DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy");
        toDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");

        LoadReportData();
    }

    protected void exportButton_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetDataFromDB();

            if (dt != null && dt.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Customer_Payment_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    GridView gvExport = new GridView();
                    gvExport.DataSource = dt;
                    gvExport.DataBind();
                    gvExport.RenderControl(hw);

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                ShowMessageBox("No data available to export.");
            }
        }
        catch (Exception ex)
        {
            if (!(ex is System.Threading.ThreadAbortException))
            {
                ShowMessageBox("Error exporting data: " + ex.Message);
            }
        }
    }

    protected void btnExportPdf_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetDataFromDB();

            if (dt != null && dt.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Customer_Payment_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-word";

                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    GridView gvExport = new GridView();
                    gvExport.DataSource = dt;
                    gvExport.DataBind();
                    gvExport.RenderControl(hw);

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                ShowMessageBox("No data available to export.");
            }
        }
        catch (Exception ex)
        {
            if (!(ex is System.Threading.ThreadAbortException))
            {
                ShowMessageBox("Error exporting report: " + ex.Message);
            }
        }
    }

    private void LoadReportData()
    {
        try
        {
            totalAmount = 0;
            DataTable dt = GetDataFromDB();

            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading report data: " + ex.Message);
        }
    }

    protected void loadGridView_PreRender(object sender, EventArgs e)
    {
        if (loadGridView.Rows.Count > 0)
        {
            loadGridView.UseAccessibleHeader = true;
            loadGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            if (loadGridView.FooterRow != null)
            {
                loadGridView.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal amt = 0;
            if (DataBinder.Eval(e.Row.DataItem, "Amount") != DBNull.Value)
            {
                decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "Amount").ToString(), out amt);
                totalAmount += amt;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Grand Total";
            e.Row.Cells[0].Font.Bold = true;
            e.Row.Cells[6].Text = totalAmount.ToString("N2");
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].Font.Bold = true;
        }
    }

    private DataTable GetDataFromDB()
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

        if (string.IsNullOrEmpty(connStr))
            return dt;

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
                DateTime fDate, tDate;
                if (!DateTime.TryParse(fromDateTextBox.Text.Trim(), out fDate)) fDate = DateTime.Now.AddDays(-30);
                if (!DateTime.TryParse(toDateTextBox.Text.Trim(), out tDate)) tDate = DateTime.Now;

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

    private void ShowMessageBox(string message)
    {
        string script = "alert('" + message.Replace("'", "\\'") + "');";
        ScriptManager.RegisterStartupScript(this, GetType(), "ServerMessage", script, true);
    }
}
