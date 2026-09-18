using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_BatchLifecycleReport : System.Web.UI.Page
{
    private readonly DCStockReportBLL aDcStockReportBll = new DCStockReportBLL();

    // Footer accumulators
    private int totalReceiveQty = 0;
    private int totalIssueQty = 0;
    private int finalClosingBalance = 0;

    private string ConnectionString
    {
        get
        {
            return ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ConnectionString;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] == null || string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            LoadFilters();
        }
    }

    private void LoadFilters()
    {
        try
        {
            aDcStockReportBll.LoadCompanyUnit(dcDropDownList);
            dcDropDownList.Items.Insert(0, new ListItem("--- All Distribution Centers ---", ""));

            LoadProductDropdown();
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading filters: " + ex.Message);
        }
    }

    private void LoadProductDropdown()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"
                    SELECT DISTINCT 
                        DCS.ProductCode, 
                        DCS.ProductCode + ' - ' + ISNULL(P.ProductName, DCS.ProductName) AS ProductDisplayName
                    FROM tblDCStore DCS
                    LEFT JOIN tblProduct P ON P.ProductCode = DCS.ProductCode
                    WHERE DCS.ProductCode IS NOT NULL AND LTRIM(RTRIM(DCS.ProductCode)) <> ''
                    ORDER BY ProductDisplayName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    productDropDownList.DataSource = dt;
                    productDropDownList.DataTextField = "ProductDisplayName";
                    productDropDownList.DataValueField = "ProductCode";
                    productDropDownList.DataBind();
                    productDropDownList.Items.Insert(0, new ListItem("--- Select Product ---", ""));
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading Products: " + ex.Message);
        }
    }

    protected void productDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string productCode = productDropDownList.SelectedValue;
        if (!string.IsNullOrEmpty(productCode))
        {
            LoadBatchDropdown(productCode);
        }
        else
        {
            batchDropDownList.Items.Clear();
            batchDropDownList.Items.Insert(0, new ListItem("--- Select Batch ---", ""));
        }
    }

    protected void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(productDropDownList.SelectedValue))
        {
            LoadBatchDropdown(productDropDownList.SelectedValue);
        }
    }

    private void LoadBatchDropdown(string productCode)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                int depotId = 0;
                if (!string.IsNullOrEmpty(dcDropDownList.SelectedValue))
                {
                    int.TryParse(dcDropDownList.SelectedValue, out depotId);
                }

                string query = @"
                    SELECT DISTINCT BatchNo 
                    FROM tblDCStore 
                    WHERE ProductCode = @ProductCode 
                      AND BatchNo IS NOT NULL 
                      AND LTRIM(RTRIM(BatchNo)) <> ''
                      AND (@DepotId = 0 OR ComUnitId = @DepotId)
                    ORDER BY BatchNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductCode", productCode);
                    cmd.Parameters.AddWithValue("@DepotId", depotId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        batchDropDownList.DataSource = dt;
                        batchDropDownList.DataTextField = "BatchNo";
                        batchDropDownList.DataValueField = "BatchNo";
                        batchDropDownList.DataBind();
                        batchDropDownList.Items.Insert(0, new ListItem("--- Select Batch ---", ""));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error loading Batches: " + ex.Message);
        }
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(productDropDownList.SelectedValue))
        {
            ShowMessageBox("Please select a Product.");
            return;
        }

        if (string.IsNullOrEmpty(batchDropDownList.SelectedValue))
        {
            ShowMessageBox("Please select a Batch No.");
            return;
        }

        try
        {
            // Reset footer accumulators
            totalReceiveQty = 0;
            totalIssueQty = 0;
            finalClosingBalance = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_Get_BatchLifecycleReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int? comUnitId = null;
                    if (!string.IsNullOrEmpty(dcDropDownList.SelectedValue))
                    {
                        comUnitId = Convert.ToInt32(dcDropDownList.SelectedValue);
                    }

                    cmd.Parameters.AddWithValue("@ComUnitId", (object)comUnitId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProductCode", productDropDownList.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@BatchNo", batchDropDownList.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@TransactionType", txnTypeDropDownList.SelectedValue);

                    if (!string.IsNullOrEmpty(fromDateTextBox.Text.Trim()))
                    {
                        DateTime fromDt;
                        if (DateTime.TryParse(fromDateTextBox.Text.Trim(), out fromDt))
                            cmd.Parameters.AddWithValue("@FromDate", fromDt);
                        else
                            cmd.Parameters.AddWithValue("@FromDate", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FromDate", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(toDateTextBox.Text.Trim()))
                    {
                        DateTime toDt;
                        if (DateTime.TryParse(toDateTextBox.Text.Trim(), out toDt))
                            cmd.Parameters.AddWithValue("@ToDate", toDt);
                        else
                            cmd.Parameters.AddWithValue("@ToDate", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ToDate", DBNull.Value);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        loadGridView.DataSource = dt;
                        loadGridView.DataBind();
                    }
                }
            }
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
        Response.Redirect("BatchLifecycleReport.aspx");
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
            string filename = "Batch_Lifecycle_Statement_" + productDropDownList.SelectedValue + "_" + batchDropDownList.SelectedValue + "_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls";
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
            string type = DataBinder.Eval(e.Row.DataItem, "TransactionType").ToString();

            if (type.StartsWith("Opening Balance"))
            {
                e.Row.CssClass = "row-opening";
            }

            object inVal = DataBinder.Eval(e.Row.DataItem, "ReceiveQty");
            if (inVal != null && inVal != DBNull.Value) totalReceiveQty += Convert.ToInt32(inVal);

            object outVal = DataBinder.Eval(e.Row.DataItem, "IssueQty");
            if (outVal != null && outVal != DBNull.Value) totalIssueQty += Convert.ToInt32(outVal);

            object balVal = DataBinder.Eval(e.Row.DataItem, "CumulativeBatchBalance");
            if (balVal != null && balVal != DBNull.Value) finalClosingBalance = Convert.ToInt32(balVal);

            Label lblType = (Label)e.Row.FindControl("lblType");
            if (lblType != null)
            {
                if (type.StartsWith("Opening Balance"))
                {
                    lblType.Text = "<span class='status-opening'><span class='status-dot'></span>Opening Balance (B/F)</span>";
                }
                else if (type == "Receive from WH")
                {
                    lblType.Text = "<span class='status-receive'><span class='status-dot'></span>Receive from WH</span>";
                }
                else if (type == "Sales")
                {
                    lblType.Text = "<span class='status-sales'><span class='status-dot'></span>Sales</span>";
                }
                else if (type == "Sales Return")
                {
                    lblType.Text = "<span class='status-return'><span class='status-dot'></span>Sales Return</span>";
                }
                else if (type == "B2B In" || type == "B2B Out")
                {
                    lblType.Text = "<span class='status-b2b'><span class='status-dot'></span>" + type + "</span>";
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[1].Font.Bold = true;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[7].Text = totalReceiveQty.ToString("N0");
            e.Row.Cells[7].Font.Bold = true;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[8].Text = totalIssueQty.ToString("N0");
            e.Row.Cells[8].Font.Bold = true;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Cells[9].Text = finalClosingBalance.ToString("N0");
            e.Row.Cells[9].Font.Bold = true;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
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
