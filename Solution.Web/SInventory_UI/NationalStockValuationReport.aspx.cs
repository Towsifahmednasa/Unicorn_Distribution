using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_NationalStockValuationReport : System.Web.UI.Page
{
    private readonly CentralStoreBLL aCentralStoreBll = new CentralStoreBLL();

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

            fromDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            toDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");

            LoadFilters();
        }
    }

    private void LoadFilters()
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

            // Reset footer totals
            footerTotals.Clear();

            DataTable dt = aCentralStoreBll.GetNationalStockValuation(fromDateTextBox.Text, toDateTextBox.Text, categoryId, productId);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                // Add SL No column programmatically to the DataTable so it auto-generates first
                if (!dt.Columns.Contains("SL No"))
                {
                    dt.Columns.Add("SL No", typeof(int));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["SL No"] = i + 1;
                    }
                    dt.Columns["SL No"].SetOrdinal(0);
                }
            }

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
        Response.Redirect("NationalStockValuationReport.aspx");
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
            // Prepare GridView for export by stripping HTML tags (e.g. hyperlinks on batch popup links)
            foreach (GridViewRow row in loadGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        string cellText = row.Cells[i].Text;
                        if (!string.IsNullOrEmpty(cellText) && cellText.Contains("<"))
                        {
                            row.Cells[i].Text = System.Text.RegularExpressions.Regex.Replace(cellText, "<.*?>", "");
                        }

                        // Also check controls inside cell if any
                        if (row.Cells[i].Controls.Count > 0)
                        {
                            for (int c = row.Cells[i].Controls.Count - 1; c >= 0; c--)
                            {
                                Control ctrl = row.Cells[i].Controls[c];
                                if (ctrl is LinkButton)
                                {
                                    LiteralControl lit = new LiteralControl(((LinkButton)ctrl).Text);
                                    row.Cells[i].Controls.Add(lit);
                                    row.Cells[i].Controls.Remove(ctrl);
                                }
                                else if (ctrl is HyperLink)
                                {
                                    LiteralControl lit = new LiteralControl(((HyperLink)ctrl).Text);
                                    row.Cells[i].Controls.Add(lit);
                                    row.Cells[i].Controls.Remove(ctrl);
                                }
                            }
                        }
                    }
                }
            }

            string filename = "National_Stock_Valuation_Report_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    loadGridView.RenderControl(hw);

                    string style = @"<style> .text { mso-number-format:\@; } table, th, td { border: 1px solid black; border-collapse: collapse; } </style>";
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
            // AutoGenerated column order:
            // 0: SL No
            // 1: Product Code
            // 2: Product Name
            // 3: Pack Size
            // 4: Category
            // 5: Avg Rate (No Sum)
            // 6+: Numeric columns (Sum)
            string productCode = "";
            string productName = "";
            try
            {
                productCode = DataBinder.Eval(e.Row.DataItem, "Product Code").ToString().Trim();
                productName = DataBinder.Eval(e.Row.DataItem, "Product Name").ToString().Trim();
            }
            catch
            {
                productCode = e.Row.Cells[1].Text.Trim();
                productName = e.Row.Cells[2].Text.Trim();
            }

            for (int i = 5; i < e.Row.Cells.Count; i++)
            {
                string text = e.Row.Cells[i].Text.Trim();
                if (!string.IsNullOrEmpty(text) && text != "&nbsp;")
                {
                    decimal val;
                    if (decimal.TryParse(text, out val))
                    {
                        e.Row.Cells[i].Text = val.ToString("N2");
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;

                        if (i >= 6)
                        {
                            if (footerTotals.ContainsKey(i))
                            {
                                footerTotals[i] += val;
                            }
                            else
                            {
                                footerTotals[i] = val;
                            }

                            if (val != 0)
                            {
                                string locationName = "";
                                try
                                {
                                    locationName = ((DataRowView)e.Row.DataItem).Row.Table.Columns[i].ColumnName.Trim();
                                }
                                catch
                                {
                                    if (loadGridView.HeaderRow != null)
                                    {
                                        locationName = loadGridView.HeaderRow.Cells[i].Text.Trim();
                                    }
                                }

                                string encodedProductCode = HttpUtility.JavaScriptStringEncode(productCode);
                                string encodedProductName = HttpUtility.JavaScriptStringEncode(productName);
                                string encodedLocationName = HttpUtility.JavaScriptStringEncode(locationName);

                                e.Row.Cells[i].Text = string.Format("<a href=\"javascript:void(0);\" onclick=\"showBatchDetails('{0}', '{1}', '{2}');\" style=\"text-decoration:underline; font-weight:bold;\">{3}</a>", 
                                    encodedProductCode, 
                                    encodedProductName,
                                    encodedLocationName, 
                                    val.ToString("N2"));
                            }
                        }
                    }
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[2].Text = "Total";
                e.Row.Cells[2].Font.Bold = true;
            }

            for (int i = 6; i < e.Row.Cells.Count; i++)
            {
                if (footerTotals.ContainsKey(i))
                {
                    e.Row.Cells[i].Text = footerTotals[i].ToString("N2");
                    e.Row.Cells[i].Font.Bold = true;
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[i].CssClass = "footer-total";
                }
            }
        }
    }

    private decimal popupTotalQty = 0;

    protected void btnHidden_Click(object sender, EventArgs e)
    {
        try
        {
            string productCode = hfProductCode.Value.Trim();
            string productName = hfProductName.Value.Trim();
            string locationName = hfLocationName.Value.Trim();

            lblModalProduct.Text = productCode + " - " + productName;
            lblModalLocation.Text = locationName;

            popupTotalQty = 0; // Reset sum for current popup view
            DataTable dtBatches = aCentralStoreBll.GetNationalStockValuationBatchDetails(productCode, locationName, fromDateTextBox.Text, toDateTextBox.Text);

            gvBatchDetails.DataSource = dtBatches;
            gvBatchDetails.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showBatchModal", "$('#batchModal').modal('show');", true);
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error opening batch details: " + ex.Message);
        }
    }

    protected void gvBatchDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal val = 0;
            if (DataBinder.Eval(e.Row.DataItem, "Qty") != null && decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "Qty").ToString(), out val))
            {
                popupTotalQty += val;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            if (e.Row.Cells.Count > 4)
            {
                e.Row.Cells[3].Text = "Total";
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[4].Text = popupTotalQty.ToString("N2");
                e.Row.Cells[4].Font.Bold = true;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].CssClass = "footer-total";
            }
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


