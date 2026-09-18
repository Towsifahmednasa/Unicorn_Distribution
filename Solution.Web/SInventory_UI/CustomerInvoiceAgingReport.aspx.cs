using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_CustomerInvoiceAgingReport : System.Web.UI.Page
{
    private readonly CustomerInvoiceAgingBLL aAgingBll = new CustomerInvoiceAgingBLL();

    // Key: Column Index, Value: Accumulated Sum
    private Dictionary<int, decimal> footerTotals = new Dictionary<int, decimal>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] == null || string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            asOnDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadFilters();
        }
    }

    private void LoadFilters()
    {
        try
        {
            aAgingBll.LoadDepotDropdown(comUnitDropDownList);
            if (comUnitDropDownList.Items.Count > 0)
            {
                comUnitDropDownList.Items[0].Text = "--- All DCs / Depots ---";
            }

            aAgingBll.LoadCustomerDropdown(customerDropDownList);
            if (customerDropDownList.Items.Count > 0)
            {
                customerDropDownList.Items[0].Text = "--- All Customers ---";
            }

            aAgingBll.LoadSalesPersonDropdown(salesPersonDropDownList);
            if (salesPersonDropDownList.Items.Count > 0)
            {
                salesPersonDropDownList.Items[0].Text = "--- All Sales Persons ---";
            }
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading filters: " + ex.Message);
        }
    }

    protected void comUnitDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int? comUnitId = null;
            if (!string.IsNullOrEmpty(comUnitDropDownList.SelectedValue))
            {
                comUnitId = Convert.ToInt32(comUnitDropDownList.SelectedValue);
            }

            customerDropDownList.Items.Clear();
            aAgingBll.LoadCustomerDropdown(customerDropDownList, comUnitId);
            if (customerDropDownList.Items.Count > 0)
            {
                customerDropDownList.Items[0].Text = "--- All Customers ---";
            }
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error filtering customers: " + ex.Message);
        }
    }

    private DateTime? ParseDate(string dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr)) return null;

        string[] formats = { "dd MMMM, yyyy", "dd MMMM yyyy", "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "d-MMM-yyyy", "dd-MMM-yyyy" };
        DateTime parsed;
        if (DateTime.TryParseExact(dateStr.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            return parsed;
        }

        DateTime fallback;
        if (DateTime.TryParse(dateStr.Trim(), out fallback))
        {
            return fallback;
        }

        return null;
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(asOnDateTextBox.Text))
        {
            ShowMessageBox("Please select an As On Date.");
            return;
        }

        try
        {
            DateTime? asOnDate = ParseDate(asOnDateTextBox.Text);
            DateTime? fromDate = ParseDate(fromDateTextBox.Text);
            DateTime? toDate = ParseDate(toDateTextBox.Text);

            int? comUnitId = null;
            if (!string.IsNullOrEmpty(comUnitDropDownList.SelectedValue))
            {
                comUnitId = Convert.ToInt32(comUnitDropDownList.SelectedValue);
            }

            int? customerMasterId = null;
            if (!string.IsNullOrEmpty(customerDropDownList.SelectedValue))
            {
                customerMasterId = Convert.ToInt32(customerDropDownList.SelectedValue);
            }

            int? salesPersonId = null;
            if (!string.IsNullOrEmpty(salesPersonDropDownList.SelectedValue))
            {
                salesPersonId = Convert.ToInt32(salesPersonDropDownList.SelectedValue);
            }

            bool onlyDue = false;

            // Reset footer totals
            footerTotals.Clear();

            DataTable dt = aAgingBll.GetCustomerInvoiceAging(asOnDate, fromDate, toDate, comUnitId, customerMasterId, salesPersonId, onlyDue);

            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error retrieving report: " + ex.Message);
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerInvoiceAgingReport.aspx");
    }

    protected void exportButton_Click(object sender, EventArgs e)
    {
        if (loadGridView.Rows.Count == 0)
        {
            ShowMessageBox("No data available to export.");
            return;
        }

        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Customer_Invoice_Aging_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // Report Title Header in Excel
                    hw.Write("<div style='text-align:center; font-size:16px; font-weight:bold; margin-bottom:10px;'>Customer Invoice Aging & Outstanding Report</div>");
                    hw.Write("<div style='text-align:center; font-size:12px; margin-bottom:15px;'>As On Date: " + HttpUtility.HtmlEncode(asOnDateTextBox.Text) + "</div>");

                    // Disable Paging and re-render table
                    loadGridView.AllowPaging = false;
                    loadGridView.RenderControl(hw);

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            // Expected for Response.End()
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error exporting to Excel: " + ex.Message);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Required for GridView rendering during Excel Export
    }

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Accumulate financial totals from DataRow
            decimal invoiceAmt, paidAmt, outstandingAmt;

            string invText = e.Row.Cells[4].Text.Replace(",", "").Trim();
            if (decimal.TryParse(invText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out invoiceAmt))
            {
                footerTotals[4] = (footerTotals.ContainsKey(4) ? footerTotals[4] : 0) + invoiceAmt;
            }

            string paidText = e.Row.Cells[5].Text.Replace(",", "").Trim();
            if (decimal.TryParse(paidText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out paidAmt))
            {
                footerTotals[5] = (footerTotals.ContainsKey(5) ? footerTotals[5] : 0) + paidAmt;
            }

            string outText = e.Row.Cells[7].Text.Replace(",", "").Trim();
            if (decimal.TryParse(outText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outstandingAmt))
            {
                footerTotals[7] = (footerTotals.ContainsKey(7) ? footerTotals[7] : 0) + outstandingAmt;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "";
            e.Row.Cells[1].Text = "Grand Total:";
            e.Row.Cells[1].Font.Bold = true;
            e.Row.Cells[1].CssClass = "text-left font-weight-bold";

            e.Row.Cells[2].Text = "";
            e.Row.Cells[3].Text = "";

            e.Row.Cells[4].Text = footerTotals.ContainsKey(4) ? footerTotals[4].ToString("#,##0.00") : "0.00";
            e.Row.Cells[4].Font.Bold = true;
            e.Row.Cells[4].CssClass = "text-right font-weight-bold";

            e.Row.Cells[5].Text = footerTotals.ContainsKey(5) ? footerTotals[5].ToString("#,##0.00") : "0.00";
            e.Row.Cells[5].Font.Bold = true;
            e.Row.Cells[5].CssClass = "text-right font-weight-bold";

            e.Row.Cells[6].Text = "";

            e.Row.Cells[7].Text = footerTotals.ContainsKey(7) ? footerTotals[7].ToString("#,##0.00") : "0.00";
            e.Row.Cells[7].Font.Bold = true;
            e.Row.Cells[7].CssClass = "text-right font-weight-bold text-danger";

            e.Row.Cells[8].Text = "";
        }
    }

    protected void loadGridView_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if ((gv.ShowHeader && gv.Rows.Count > 0) || gv.ShowHeaderWhenEmpty)
        {
            // Force GridView to use <thead> instead of <tbody> for proper styling
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        if (gv.ShowFooter && gv.Rows.Count > 0)
        {
            gv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    private void ShowMessageBox(string message)
    {
        string cleanMsg = message.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
        string script = string.Format("alert('{0}');", cleanMsg);
        ScriptManager.RegisterStartupScript(this, GetType(), "alertScript", script, true);
    }
}
