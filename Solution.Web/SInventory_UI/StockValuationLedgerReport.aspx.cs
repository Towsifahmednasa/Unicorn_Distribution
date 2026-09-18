using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_StockValuationLedgerReport : System.Web.UI.Page
{
    private readonly CentralStoreBLL aCentralStoreBll = new CentralStoreBLL();

    // Sum accumulators for GridView footer

    private decimal totalOpeningQty = 0;
    private decimal totalOpeningAmt = 0;
    private decimal totalReceivedQty = 0;
    private decimal totalIssuedQty = 0;
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

            LoadDropdowns();
        }
    }

    private void LoadDropdowns()
    {
        try
        {
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
            totalReceivedQty = 0;
            totalIssuedQty = 0;
            totalClosingQty = 0;
            totalClosingAmt = 0;

            DataTable dt = aCentralStoreBll.GetStockValuationLedger(fromDateTextBox.Text, toDateTextBox.Text, categoryId, productId);
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
        Response.Redirect("StockValuationLedgerReport.aspx");
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
            string filename = "WH_Stock_Valuation_Report_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // Clean hyperlink controls before rendering to excel
                    for (int i = 0; i < loadGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < loadGridView.Rows[i].Cells.Count; j++)
                        {
                            TableCell cell = loadGridView.Rows[i].Cells[j];
                            if (cell.Controls.Count > 0)
                            {
                                string text = "";
                                foreach (Control ctrl in cell.Controls)
                                {
                                    HyperLink link = ctrl as HyperLink;
                                    LinkButton lb = ctrl as LinkButton;
                                    Label lbl = ctrl as Label;
                                    LiteralControl lc = ctrl as LiteralControl;

                                    if (link != null)
                                        text += link.Text;
                                    else if (lb != null)
                                        text += lb.Text;
                                    else if (lbl != null)
                                        text += lbl.Text;
                                    else if (lc != null)
                                        text += lc.Text;
                                }
                                if (!string.IsNullOrEmpty(text))
                                {
                                    cell.Controls.Clear();
                                    cell.Text = text;
                                }
                            }
                        }
                    }

                    // Directly render GridView without dummy HtmlForm to avoid capturing MasterPage menu
                    loadGridView.RenderControl(hw);

                    string style = @"<style> 
                        table { border-collapse: collapse; font-family: Calibri, Arial, sans-serif; font-size: 11pt; }
                        th { background-color: #00bcd4; color: #ffffff; font-weight: bold; border: 1px solid #000; text-align: center; }
                        td { border: 1px solid #d3d3d3; padding: 4px; }
                        .text { mso-number-format:\@; } 
                    </style>";
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

            val = DataBinder.Eval(e.Row.DataItem, "Received Qty");
            if (val != null && val != DBNull.Value) totalReceivedQty += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "Issued Qty");
            if (val != null && val != DBNull.Value) totalIssuedQty += Convert.ToDecimal(val);

            val = DataBinder.Eval(e.Row.DataItem, "Closing Qty");
            if (val != null && val != DBNull.Value) totalClosingQty += Convert.ToDecimal(val);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[1].Font.Bold = true;

            e.Row.Cells[5].Text = totalOpeningQty.ToString("N2");
            e.Row.Cells[5].Font.Bold = true;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[6].Text = totalReceivedQty.ToString("N2");
            e.Row.Cells[6].Font.Bold = true;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[7].Text = totalIssuedQty.ToString("N2");
            e.Row.Cells[7].Font.Bold = true;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[8].Text = totalClosingQty.ToString("N2");
            e.Row.Cells[8].Font.Bold = true;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
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
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
    }

    private void ShowMessageBox(string message)
    {
        string sScript = string.Format("alert('{0}');", message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", ""));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}
