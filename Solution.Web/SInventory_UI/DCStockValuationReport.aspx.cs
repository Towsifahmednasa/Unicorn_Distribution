using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_DCStockValuationReport : System.Web.UI.Page
{
    private readonly DCStockReportBLL aDcStockReportBll = new DCStockReportBLL();
    private readonly CentralStoreBLL aCentralStoreBll = new CentralStoreBLL();

    // Sum accumulators for GridView footer
    private decimal totalOpeningQty = 0;
    private decimal totalOpeningAmt = 0;
    private decimal totalRcvFromWH = 0;
    private decimal totalB2BRcv = 0;
    private decimal totalReturnRcv = 0;
    private decimal totalSalesIssue = 0;
    private decimal totalB2BIssue = 0;
    private decimal totalClosingQty = 0;
    private decimal totalClosingAmt = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] == null || string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            fromDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            toDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");

            LoadFilters();
        }
    }

    private void LoadFilters()
    {
        try
        {
            aDcStockReportBll.LoadCompanyUnit(dcDropDownList);
            dcDropDownList.Items.Insert(0, new ListItem("--- Select DC ---", ""));

            aCentralStoreBll.LoadCategoryDropdown(categoryDropDownList);
            categoryDropDownList.Items.Insert(0, new ListItem("--- Select Group ---", ""));

            aCentralStoreBll.LoadProductDropdown(productDropDownList);
            productDropDownList.Items.Insert(0, new ListItem("--- Select Product ---", ""));
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading filters: " + ex.Message);
        }
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(fromDateTextBox.Text) || string.IsNullOrEmpty(toDateTextBox.Text))
        {
            ShowMessageBox("Please select a valid date range.");
            return;
        }

        try
        {
            int? comUnitId = null;
            if (!string.IsNullOrEmpty(dcDropDownList.SelectedValue))
            {
                comUnitId = Convert.ToInt32(dcDropDownList.SelectedValue);
            }

            int? categoryId = null;
            if (!string.IsNullOrEmpty(categoryDropDownList.SelectedValue))
            {
                categoryId = Convert.ToInt32(categoryDropDownList.SelectedValue);
            }

            int? productId = null;
            if (!string.IsNullOrEmpty(productDropDownList.SelectedValue))
            {
                productId = Convert.ToInt32(productDropDownList.SelectedValue);
            }

            // Reset footer accumulators
            totalOpeningQty = 0;
            totalOpeningAmt = 0;
            totalRcvFromWH = 0;
            totalB2BRcv = 0;
            totalReturnRcv = 0;
            totalSalesIssue = 0;
            totalB2BIssue = 0;
            totalClosingQty = 0;
            totalClosingAmt = 0;

            DataTable dt = aDcStockReportBll.GetDCStockValuationLedger(fromDateTextBox.Text, toDateTextBox.Text, comUnitId, categoryId, productId);
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
        Response.Redirect("DCStockValuationReport.aspx");
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
            string filename = "DC_Stock_Valuation_Report_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    HtmlForm form = new HtmlForm();
                    loadGridView.Parent.Controls.Add(form);
                    form.Controls.Add(loadGridView);
                    form.RenderControl(hw);

                    string style = @"<style> .text { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
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

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object val;

            val = DataBinder.Eval(e.Row.DataItem, "Opening Qty");
            if (val != null && val != DBNull.Value) totalOpeningQty += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "Receive From WH");
            if (val != null && val != DBNull.Value) totalRcvFromWH += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "B2B Receive");
            if (val != null && val != DBNull.Value) totalB2BRcv += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "Return Rcv");
            if (val != null && val != DBNull.Value) totalReturnRcv += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "Sales");
            if (val != null && val != DBNull.Value) totalSalesIssue += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "B2B Issue");
            if (val != null && val != DBNull.Value) totalB2BIssue += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "Closing Qty");
            if (val != null && val != DBNull.Value) totalClosingQty += Convert.ToDecimal(val);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[1].Font.Bold = true;

            e.Row.Cells[7].Text = totalOpeningQty.ToString("N2");
            e.Row.Cells[7].Font.Bold = true;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[8].Text = totalRcvFromWH.ToString("N2");
            e.Row.Cells[8].Font.Bold = true;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[9].Text = totalB2BRcv.ToString("N2");
            e.Row.Cells[9].Font.Bold = true;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[10].Text = totalReturnRcv.ToString("N2");
            e.Row.Cells[10].Font.Bold = true;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[11].Text = totalSalesIssue.ToString("N2");
            e.Row.Cells[11].Font.Bold = true;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[12].Text = totalB2BIssue.ToString("N2");
            e.Row.Cells[12].Font.Bold = true;
            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[13].Text = totalClosingQty.ToString("N2");
            e.Row.Cells[13].Font.Bold = true;
            e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    protected void loadGridView_PreRender(object sender, EventArgs e)
    {
        if (loadGridView.HeaderRow != null)
        {
            loadGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    private void ShowMessageBox(string message)
    {
        string sScript = string.Format("alert('{0}');", message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", ""));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}
