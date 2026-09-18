using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Library.BLL.SInventory_BLL;
using Library.DAL.MasterSetup_DAL;

public partial class SInventory_UI_InvoiceSummaryReport : System.Web.UI.Page
{
    private static CmnCrystaltoView _DAL = new CmnCrystaltoView();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InvoiceDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            todateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadDropDown();
        }
    }

    private void LoadDropDown()
    {
        try
        {
            OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();
            aOtherStockActionBLL.DCLoad(dcDropDownList1);

            // Populate Customer
            DataTable dtCustomer = _DAL.GetCustomerListDAL();
            ddlCustomer.DataSource = dtCustomer;
            ddlCustomer.DataTextField = "CustomerName";
            ddlCustomer.DataValueField = "CustomerCode";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("--- Select Customer ---", ""));
        }
        catch { }
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("InvoiceSummaryReport.aspx");
    }

    protected void loadGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        loadGridView.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        if (loadGridView.HeaderRow != null)
        {
            loadGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    private void LoadData()
    {
        try
        {
            int? comUnitId = null;
            if (dcDropDownList1.SelectedValue != "")
            {
                comUnitId = Convert.ToInt32(dcDropDownList1.SelectedValue);
            }

            string customerCode = ddlCustomer.SelectedValue;
            string proformaNo = txtProformaNo.Text.Trim();
            string deliveryNo = txtDeliveryNo.Text.Trim();

            DataTable dt = _DAL.GetInvoiceSummaryReportDAL(
                InvoiceDateTextBox.Text,
                todateTextBox.Text,
                comUnitId,
                customerCode,
                proformaNo,
                deliveryNo
            );

            loadGridView.DataSource = dt;
            loadGridView.DataBind();

            // Calculate summaries
            decimal totalReceivable = 0;
            decimal totalPaid = 0;
            decimal totalDue = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    totalReceivable += Convert.ToDecimal(r["Receiveable Amount"] == DBNull.Value ? 0 : r["Receiveable Amount"]);
                    totalPaid += Convert.ToDecimal(r["Total Payment"] == DBNull.Value ? 0 : r["Total Payment"]);
                    totalDue += Convert.ToDecimal(r["Due Amount"] == DBNull.Value ? 0 : r["Due Amount"]);
                }
            }

            lblTotalReceivable.Text = "Total Receivable: " + totalReceivable.ToString("N2");
            lblTotalPaid.Text = "Total Paid: " + totalPaid.ToString("N2");
            lblTotalDue.Text = "Total Due: " + totalDue.ToString("N2");
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading report data: " + ex.Message);
        }
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string argument = e.CommandArgument != null ? e.CommandArgument.ToString() : "";
            if (string.IsNullOrEmpty(argument)) return;

            if (e.CommandName == "PrintProforma")
            {
                string url = "../SInventory_RPTVIEW/ProformaReportPrintViewer.aspx?InvNo=" + Server.UrlEncode(argument);
                string script = string.Format("openReportWindow('{0}');", url);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintPI", script, true);
            }
            else if (e.CommandName == "PrintChallan")
            {
                if (string.IsNullOrEmpty(argument))
                {
                    ShowMessageBox("Delivery Challan is not generated for this invoice yet.");
                    return;
                }
                string url = "../SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx?InvNo=" + Server.UrlEncode(argument);
                string script = string.Format("openReportWindow('{0}');", url);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintChallan", script, true);
            }
            else if (e.CommandName == "PrintReceipt")
            {
                Session["InvoiceIds"] = " AND INV.InvoiceId = " + argument;
                string url = "../SInventory_RPTVIEW/MoneyReceiptViewer2.aspx";
                string script = string.Format("openReportWindow('{0}');", url);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintMR", script, true);
            }
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error during report print generation: " + ex.Message);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            int? comUnitId = null;
            if (dcDropDownList1.SelectedValue != "")
            {
                comUnitId = Convert.ToInt32(dcDropDownList1.SelectedValue);
            }

            string customerCode = ddlCustomer.SelectedValue;
            string proformaNo = txtProformaNo.Text.Trim();
            string deliveryNo = txtDeliveryNo.Text.Trim();

            DataTable dt = _DAL.GetInvoiceSummaryReportDAL(
                InvoiceDateTextBox.Text,
                todateTextBox.Text,
                comUnitId,
                customerCode,
                proformaNo,
                deliveryNo
            );

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessageBox("No data available to export.");
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Invoice Summary");
                worksheet.Cell(1, 1).Value = "Invoice Summary Report";
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Range("A1:K1").Merge();

                worksheet.Cell(2, 1).Value = "Report Period: " + InvoiceDateTextBox.Text + " to " + todateTextBox.Text;
                worksheet.Cell(2, 1).Style.Font.Italic = true;
                worksheet.Range("A2:K2").Merge();

                // Add header row
                string[] headers = new string[] {
                    "SL", "Business Unit", "Customer Code", "Customer Name", 
                    "Proforma Invoice No", "Proforma Date", "Delivery Invoice No", 
                    "Delivery Date", "Receivable Amount", "Total Payment", "Last Payment Date", "Due Amount"
                };

                for (int col = 0; col < headers.Length; col++)
                {
                    var cell = worksheet.Cell(4, col + 1);
                    cell.Value = headers[col];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.Green;
                    cell.Style.Font.FontColor = XLColor.White;
                }
                // Add data rows
                int startRow = 5;
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    DataRow dr = dt.Rows[row];
                    worksheet.Cell(startRow + row, 1).Value = row + 1;
                    worksheet.Cell(startRow + row, 2).Value = dr["Business Unit"].ToString();
                    worksheet.Cell(startRow + row, 3).Value = dr["Customer Code"].ToString();
                    worksheet.Cell(startRow + row, 4).Value = dr["Customer Name"].ToString();
                    worksheet.Cell(startRow + row, 5).Value = dr["Proforma Invoice No"].ToString();
                    if (dr["Proforma Invoice Date"] != DBNull.Value)
                    {
                        worksheet.Cell(startRow + row, 6).Value = Convert.ToDateTime(dr["Proforma Invoice Date"]);
                        worksheet.Cell(startRow + row, 6).Style.DateFormat.Format = "dd-mmm-yyyy";
                    }
                    worksheet.Cell(startRow + row, 7).Value = dr["Delivery Invoice No"].ToString();
                    if (dr["Delivery Invoice Date"] != DBNull.Value)
                    {
                        worksheet.Cell(startRow + row, 8).Value = Convert.ToDateTime(dr["Delivery Invoice Date"]);
                        worksheet.Cell(startRow + row, 8).Style.DateFormat.Format = "dd-mmm-yyyy";
                    }

                    worksheet.Cell(startRow + row, 9).Value = Convert.ToDecimal(dr["Receiveable Amount"] == DBNull.Value ? 0 : dr["Receiveable Amount"]);
                    worksheet.Cell(startRow + row, 9).Style.NumberFormat.Format = "#,##0.00";

                    worksheet.Cell(startRow + row, 10).Value = Convert.ToDecimal(dr["Total Payment"] == DBNull.Value ? 0 : dr["Total Payment"]);
                    worksheet.Cell(startRow + row, 10).Style.NumberFormat.Format = "#,##0.00";

                    if (dr["Last Payment Date"] != DBNull.Value)
                    {
                        worksheet.Cell(startRow + row, 11).Value = Convert.ToDateTime(dr["Last Payment Date"]);
                        worksheet.Cell(startRow + row, 11).Style.DateFormat.Format = "dd-mmm-yyyy";
                    }

                    worksheet.Cell(startRow + row, 12).Value = Convert.ToDecimal(dr["Due Amount"] == DBNull.Value ? 0 : dr["Due Amount"]);
                    worksheet.Cell(startRow + row, 12).Style.NumberFormat.Format = "#,##0.00";
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=InvoiceSummaryReport_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            if (!(ex is System.Threading.ThreadAbortException))
            {
                ShowMessageBox("Error exporting to Excel: " + ex.Message);
            }
        }
    }

    private void ShowMessageBox(string message)
    {
        string sScript = string.Format("alert('{0}');", message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", ""));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}
