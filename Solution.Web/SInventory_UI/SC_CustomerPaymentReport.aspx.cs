using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Library.BLL.SInventory_BLL;
using Library.DAL.MasterSetup_DAL;
using SalesSolution.Web.DataLayer;

public partial class SInventory_UI_SC_CustomerPaymentReport : System.Web.UI.Page
{
    private static CmnCrystaltoView _DAL = new CmnCrystaltoView();
    private DropDownList GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect;
    private static SeedDataDAL _seedRepo = new SeedDataDAL();

    public string CompanyName
    {
        get
        {
            try
            {
                if (Session["CompanyName"] != null && !string.IsNullOrEmpty(Session["CompanyName"].ToString()))
                {
                    return Session["CompanyName"].ToString();
                }
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"];
                if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
                {
                    using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(cs.ConnectionString))
                    {
                        conn.Open();
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT TOP 1 CompanyName FROM tblCompanyInfo", conn))
                        {
                            object obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()))
                            {
                                return obj.ToString();
                            }
                        }
                    }
                }
            }
            catch { }
            return "Unicorn Healthcare Solution Ltd.";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GroupSelect = (DropDownList)IVMarketStructure.FindControl("GroupSelect") as DropDownList;
        ZoneSelect = (DropDownList)IVMarketStructure.FindControl("ZoneSelect") as DropDownList;
        AreaSelect = (DropDownList)IVMarketStructure.FindControl("AreaSelect") as DropDownList;
        TeritorySelect = (DropDownList)IVMarketStructure.FindControl("TeritorySelect") as DropDownList;
        SubTeritory = (DropDownList)IVMarketStructure.FindControl("SubTeritory") as DropDownList;
        MarketSelect = (DropDownList)IVMarketStructure.FindControl("MarketSelect") as DropDownList;
        if (!IsPostBack)
        {

            InvoiceDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            todateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadDropDown();


            SalesCenter.Visible = true;

        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    public void LoadDropDown()
    {
        OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();
        aOtherStockActionBLL.DCLoad(dcDropDownList1);

        try
        {
            using (DataTable dt = _seedRepo.GetProgramTypeListAll())
            {
                ddlProgramType.DataSource = dt;
                ddlProgramType.DataValueField = "ProgramTypeId";
                ddlProgramType.DataTextField = "ProgramTypeName";
                ddlProgramType.DataBind();
                ddlProgramType.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlProgramType.SelectedIndex = 0;
            }


        }


        catch (Exception ex) { }


        try
        {
            using (DataTable dt = _seedRepo.GetChemistTypeListALL())
            {
                ddlChemisType.DataSource = dt;
                ddlChemisType.DataValueField = "CustomerTypeId";
                ddlChemisType.DataTextField = "CustomerType";
                ddlChemisType.DataBind();
                ddlChemisType.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlChemisType.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }

        // Populate Customer
        try
        {
            DataTable dtCustomer = _DAL.GetCustomerListDAL();
            ddlCustomer.DataSource = dtCustomer;
            ddlCustomer.DataTextField = "CustomerName";
            ddlCustomer.DataValueField = "CustomerCode";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("--- Select Customer ---", ""));
        }
        catch { }
    }
    protected void loadGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        loadGridView.PageIndex = e.NewPageIndex;
        this.LoadData();
    }
    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("SC_CustomerPaymentReport.aspx");
    }

    private void LoadData()
    {
        pnlDetails.Visible = true;
        pnlSummary.Visible = false;
        comUnitDetailDataTable = _DAL.GetSC_CustomerFinalPaymentReportDAL_new(Parm(), Parm_2(), OldNewParm());

        if (comUnitDetailDataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = comUnitDetailDataTable;
            loadGridView.DataBind();

            decimal totalPaid = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("PaidAmount") ?? 0);
            decimal totalDiscount = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("Discount") ?? 0);
            decimal totalVat = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("PayVATAmount") ?? 0);
            decimal totalAit = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("AITAmount") ?? 0);
            decimal totalPayAll = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("TotalPay") ?? 0);
            decimal totalAdjustment = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("AdjustmentAmount") ?? 0);

            lblCount.Text = "Total Pay Amount (TP+VAT+AIT) : " + totalPayAll.ToString("N2") + " | Adjustment : " + totalAdjustment.ToString("N2");

            if (loadGridView.FooterRow != null)
            {
                loadGridView.FooterRow.Cells[0].Text = "Total";
                loadGridView.FooterRow.Cells[0].Font.Bold = true;
                
                // Align & Display Sum in Footer (Shifted +1 due to SL column)
                // 16: Paid Amount
                loadGridView.FooterRow.Cells[16].Text = totalPaid.ToString("N2");
                loadGridView.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
                loadGridView.FooterRow.Cells[16].Font.Bold = true;

                // 17: Discount
                loadGridView.FooterRow.Cells[17].Text = totalDiscount.ToString("N2");
                loadGridView.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Right;
                loadGridView.FooterRow.Cells[17].Font.Bold = true;

                // 18: VAT
                loadGridView.FooterRow.Cells[18].Text = totalVat.ToString("N2");
                loadGridView.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
                loadGridView.FooterRow.Cells[18].Font.Bold = true;

                // 19: AIT
                loadGridView.FooterRow.Cells[19].Text = totalAit.ToString("N2");
                loadGridView.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Right;
                loadGridView.FooterRow.Cells[19].Font.Bold = true;

                // 20: Adjustment (Bad Debt / Promotional Expense)
                loadGridView.FooterRow.Cells[20].Text = totalAdjustment.ToString("N2");
                loadGridView.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Right;
                loadGridView.FooterRow.Cells[20].Font.Bold = true;
            }
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
            lblCount.Text = "Total Pay Amount (TP+VAT+AIT) : " + 0.ToString("N2");
        }
    }

    private void LoadSummaryData()
    {
        pnlDetails.Visible = false;
        pnlSummary.Visible = true;
        comUnitDetailDataTable = _DAL.GetSC_CustomerFinalPaymentReportDAL_new(Parm(), Parm_2(), OldNewParm());

        if (comUnitDetailDataTable.Rows.Count > 0)
        {
            summaryGridView.DataSource = comUnitDetailDataTable;
            summaryGridView.DataBind();

            decimal totalPaid = comUnitDetailDataTable.AsEnumerable().Sum(row => row.Field<decimal?>("TotalPay") ?? 0);

            lblSummaryCount.Text = "Total Collection Amount : " + totalPaid.ToString("N2");

            if (summaryGridView.FooterRow != null)
            {
                summaryGridView.FooterRow.Cells[0].Text = "Total";
                summaryGridView.FooterRow.Cells[0].Font.Bold = true;

                // 6: Collection Amount (PaidAmount)
                summaryGridView.FooterRow.Cells[6].Text = totalPaid.ToString("N2");
                summaryGridView.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                summaryGridView.FooterRow.Cells[6].Font.Bold = true;
            }
        }
        else
        {
            summaryGridView.DataSource = null;
            summaryGridView.DataBind();
            lblSummaryCount.Text = "Total Collection Amount : " + 0.ToString("N2");
        }
    }

    protected void fromDateTextBox_TextChanged(object sender, EventArgs e)
    {
        DateTime Fromd = Convert.ToDateTime("01-Apr-2022");
        DateTime inputDateTime;
        if (!DateTime.TryParse(InvoiceDateTextBox.Text, out inputDateTime))
        {
            return; // empty or invalid date typed; validation happens on search
        }
        if (inputDateTime < Fromd)
        {
            InvoiceDateTextBox.Text = "01 April, 2022";
        }
    }

    DataTable comUnitDetailDataTable = new DataTable();
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void SummaryButton_Click(object sender, EventArgs e)
    {
        LoadSummaryData();
    }

    private string OldNewParm()
    {
        string param = "";
        if (dcDropDownList1.SelectedValue != "")
        {
            param = param + " AND I.ComUnitId='" + dcDropDownList1.SelectedValue + "' ";
        }
        if (InvoiceDateTextBox.Text != "" && todateTextBox.Text != "")
        {
            param = param + " AND CONVERT(date,I.UpdateDate) BETWEEN '" + InvoiceDateTextBox.Text + "' AND '" + todateTextBox.Text + "' ";
        }
        return param;
    }

    private string Parm()
    {
        string param = "";
        if (InvoiceDateTextBox.Text != "" && todateTextBox.Text != "")
        {
            param = param + " AND CONVERT(date,custDtl.CustPaymentDate) BETWEEN '" + InvoiceDateTextBox.Text + "' AND '" + todateTextBox.Text + "' ";
        }
        if (ddlPaymentBy.SelectedValue != "")
        {
            param = param + " AND custDtl.CollectionBy='" + ddlPaymentBy.SelectedValue + "' ";
        }
        return param;
    }

    private string Parm_2()
    {
        var sb = new StringBuilder();

        // ----- independent filters (always apply if present) -----
        AppendEq(sb, "CU.ComUnitId", (dcDropDownList1 != null) ? dcDropDownList1.SelectedValue : null);
        AppendEq(sb, "mas.ProgramTypeId", (ddlProgramType != null) ? ddlProgramType.SelectedValue : null);
        AppendEq(sb, "mas.CusttypeId", (ddlChemisType != null) ? ddlChemisType.SelectedValue : null);

        if (ddlCustomer != null && ddlCustomer.SelectedValue != "")
        {
            sb.Append(" AND C.CustomerCode = '").Append(ddlCustomer.SelectedValue).Append("' ");
        }

        // ----- hierarchical: Market > SubTerritory > Territory > Area > Region > Group -----
        int? market = ToInt((MarketSelect != null) ? MarketSelect.SelectedValue : null);
        int? subTer = ToInt((SubTeritory != null) ? SubTeritory.SelectedValue : null);
        int? ter = ToInt((TeritorySelect != null) ? TeritorySelect.SelectedValue : null);
        int? area = ToInt((AreaSelect != null) ? AreaSelect.SelectedValue : null);
        int? region = ToInt((ZoneSelect != null) ? ZoneSelect.SelectedValue : null);
        int? group = ToInt((GroupSelect != null) ? GroupSelect.SelectedValue : null);

        if (market.HasValue)
            sb.Append(" AND C.MarketId = ").Append(market.Value);
        else if (subTer.HasValue)
            sb.Append(" AND bb.SubTerritoryId = ").Append(subTer.Value);
        else if (ter.HasValue)
            sb.Append(" AND cc.TerritoryId = ").Append(ter.Value);
        else if (area.HasValue)
            sb.Append(" AND ddd.AreaId = ").Append(area.Value);
        else if (region.HasValue)
            sb.Append(" AND tblRegion.RegionId = ").Append(region.Value);
        else if (group.HasValue)
            sb.Append(" AND tblRegion.GroupId = ").Append(group.Value);

        return sb.ToString();
    }

    private static int? ToInt(string s)
    {
        if (string.IsNullOrEmpty(s)) return null;
        int v;
        return int.TryParse(s, out v) ? (int?)v : null;
    }

    private static void AppendEq(StringBuilder sb, string column, string selectedValue)
    {
        int? v = ToInt(selectedValue);
        if (v.HasValue)
        {
            sb.Append(" AND ").Append(column).Append(" = ").Append(v.Value);
        }
    }

    protected void dcDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();
        aOtherStockActionBLL.DCLoad(dcDropDownList1);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (loadGridView.Rows.Count > 0)
        {
            // To Export all pages.
            loadGridView.AllowPaging = false;
            this.LoadData();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Customer_Payment_Report_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.UTF8;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
            sb.Append("<head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Customer Payment Report</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]>");
            sb.Append("<style>");
            sb.Append("th { background-color: #1A7343; color: #FFFFFF; font-weight: bold; border: 0.5pt solid #CCCCCC; padding: 5px; text-align: center; }");
            sb.Append("td { border: 0.5pt solid #CCCCCC; padding: 4px; font-family: Calibri, Arial; font-size: 11pt; }");
            sb.Append(".text-cell { mso-number-format:'\\@'; }");
            sb.Append(".num-cell { mso-number-format:'\\#\\,\\#\\#0\\.00'; text-align: right; }");
            sb.Append(".date-cell { mso-number-format:'dd\\/mm\\/yyyy'; text-align: center; }");
            sb.Append(".total-cell { background-color: #E9ECEF; font-weight: bold; border: 0.5pt solid #999999; }");
            sb.Append("</style></head><body>");

            sb.Append("<table border='1' cellspacing='0' cellpadding='4'>");
            
            // Header
            sb.Append("<tr>");
            List<int> visibleColIndices = new List<int>();
            for (int colIndex = 0; colIndex < loadGridView.Columns.Count; colIndex++)
            {
                if (loadGridView.Columns[colIndex].Visible)
                {
                    visibleColIndices.Add(colIndex);
                    sb.Append("<th>" + HttpUtility.HtmlEncode(loadGridView.HeaderRow.Cells[colIndex].Text) + "</th>");
                }
            }
            sb.Append("</tr>");

            // Rows
            int sl = 1;
            foreach (GridViewRow row in loadGridView.Rows)
            {
                sb.Append("<tr>");
                foreach (int colIndex in visibleColIndices)
                {
                    TableCell cell = row.Cells[colIndex];
                    string cellText = HttpUtility.HtmlDecode(cell.Text).Trim();
                    if (cellText == "&nbsp;") cellText = "";

                    // Check column formatting
                    BoundField bf = loadGridView.Columns[colIndex] as BoundField;
                    string dataField = bf != null ? bf.DataField : "";

                    if (loadGridView.Columns[colIndex] is TemplateField)
                    {
                        sb.Append("<td class='text-cell' style='text-align: center;'>" + sl + "</td>");
                    }
                    else if (dataField == "InvoiceAmount" || dataField == "PaidAmount" || dataField == "Discount" || dataField == "PayVATAmount" || dataField == "AITAmount" || dataField == "AdjustmentAmount")
                    {
                        // Clean amount for excel numeric cell
                        decimal val = 0;
                        decimal.TryParse(cellText.Replace(",", ""), out val);
                        sb.Append("<td class='num-cell'>" + val.ToString("0.00") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='text-cell'>" + HttpUtility.HtmlEncode(cellText) + "</td>");
                    }
                }
                sb.Append("</tr>");
                sl++;
            }

            // Footer Total Row in Excel
            if (loadGridView.FooterRow != null)
            {
                sb.Append("<tr class='total-cell'>");
                foreach (int colIndex in visibleColIndices)
                {
                    TableCell cell = loadGridView.FooterRow.Cells[colIndex];
                    string cellText = HttpUtility.HtmlDecode(cell.Text).Trim();
                    if (cellText == "&nbsp;") cellText = "";

                    BoundField bf = loadGridView.Columns[colIndex] as BoundField;
                    string dataField = bf != null ? bf.DataField : "";

                    if (dataField == "InvoiceAmount" || dataField == "PaidAmount" || dataField == "Discount" || dataField == "PayVATAmount" || dataField == "AITAmount" || dataField == "AdjustmentAmount")
                    {
                        decimal val = 0;
                        decimal.TryParse(cellText.Replace(",", ""), out val);
                        sb.Append("<td class='num-cell total-cell'>" + val.ToString("0.00") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='text-cell total-cell'>" + HttpUtility.HtmlEncode(cellText) + "</td>");
                    }
                }
                sb.Append("</tr>");
            }

            sb.Append("</table></body></html>");

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "No Data Found!" + "','Faild');", true);
        }
    }

    protected void btnExportSummary_Click(object sender, EventArgs e)
    {
        if (summaryGridView.Rows.Count > 0)
        {
            // To Export all rows
            summaryGridView.AllowPaging = false;
            this.LoadSummaryData();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Customer_Payment_Summary_Report_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.UTF8;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
            sb.Append("<head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Customer Payment Summary</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]>");
            sb.Append("<style>");
            sb.Append("th { background-color: #1A7343; color: #FFFFFF; font-weight: bold; border: 0.5pt solid #CCCCCC; padding: 5px; text-align: center; }");
            sb.Append("td { border: 0.5pt solid #CCCCCC; padding: 4px; font-family: Calibri, Arial; font-size: 11pt; }");
            sb.Append(".text-cell { mso-number-format:'\\@'; }");
            sb.Append(".num-cell { mso-number-format:'\\#\\,\\#\\#0\\.00'; text-align: right; }");
            sb.Append(".date-cell { mso-number-format:'dd\\/mm\\/yyyy'; text-align: center; }");
            sb.Append(".total-cell { background-color: #E9ECEF; font-weight: bold; border: 0.5pt solid #999999; }");
            sb.Append("</style></head><body>");

            sb.Append("<table border='1' cellspacing='0' cellpadding='4'>");

            // Header
            sb.Append("<tr>");
            List<int> visibleColIndices = new List<int>();
            for (int colIndex = 0; colIndex < summaryGridView.Columns.Count; colIndex++)
            {
                if (summaryGridView.Columns[colIndex].Visible)
                {
                    visibleColIndices.Add(colIndex);
                    sb.Append("<th>" + HttpUtility.HtmlEncode(summaryGridView.HeaderRow.Cells[colIndex].Text) + "</th>");
                }
            }
            sb.Append("</tr>");

            // Rows
            int sl = 1;
            foreach (GridViewRow row in summaryGridView.Rows)
            {
                sb.Append("<tr>");
                foreach (int colIndex in visibleColIndices)
                {
                    TableCell cell = row.Cells[colIndex];
                    string cellText = HttpUtility.HtmlDecode(cell.Text).Trim();
                    if (cellText == "&nbsp;") cellText = "";

                    BoundField bf = summaryGridView.Columns[colIndex] as BoundField;
                    string dataField = bf != null ? bf.DataField : "";

                    if (summaryGridView.Columns[colIndex] is TemplateField)
                    {
                        // SL column
                        sb.Append("<td class='text-cell' style='text-align: center;'>" + sl + "</td>");
                    }
                    else if (dataField == "TotalPay")
                    {
                        decimal val = 0;
                        decimal.TryParse(cellText.Replace(",", ""), out val);
                        sb.Append("<td class='num-cell'>" + val.ToString("0.00") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='text-cell'>" + HttpUtility.HtmlEncode(cellText) + "</td>");
                    }
                }
                sb.Append("</tr>");
                sl++;
            }

            // Footer Total Row in Excel
            if (summaryGridView.FooterRow != null)
            {
                sb.Append("<tr class='total-cell'>");
                foreach (int colIndex in visibleColIndices)
                {
                    TableCell cell = summaryGridView.FooterRow.Cells[colIndex];
                    string cellText = HttpUtility.HtmlDecode(cell.Text).Trim();
                    if (cellText == "&nbsp;") cellText = "";

                    BoundField bf = summaryGridView.Columns[colIndex] as BoundField;
                    string dataField = bf != null ? bf.DataField : "";

                    if (dataField == "TotalPay")
                    {
                        decimal val = 0;
                        decimal.TryParse(cellText.Replace(",", ""), out val);
                        sb.Append("<td class='num-cell total-cell'>" + val.ToString("0.00") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='text-cell total-cell'>" + HttpUtility.HtmlEncode(cellText) + "</td>");
                    }
                }
                sb.Append("</tr>");
            }

            sb.Append("</table></body></html>");

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "No Data Found!" + "','Faild');", true);
        }
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.
    }
    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if (gv.ShowHeader && gv.Rows.Count > 0 && gv.HeaderRow != null)
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        if (gv.ShowFooter && gv.Rows.Count > 0 && gv.FooterRow != null)
        {
            gv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

   
    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        Session["ProformaReport"] = "";
        Session["ProformaReport"] = 1;

        if (InvoiceDateTextBox.Text != "" && todateTextBox.Text != "" && dcDropDownList1.SelectedValue != "")
        {
            if (todateTextBox.Text == "")
            {
                InvoiceDateTextBox.Text = todateTextBox.Text;
            }

            string fromDate = InvoiceDateTextBox.Text;
            string toDate = todateTextBox.Text;
            string districtId = dcDropDownList1.SelectedValue;

            string url = "../SInventory_RPTVIEW/ProformaReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&districtId=" + districtId;
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        //if (CheckBox1.Checked && todateTextBox.Text != "" && InvoiceDateTextBox.Text != "")
        //{
        //    int i = 1;
        //    string fromDate = InvoiceDateTextBox.Text;
        //    string toDate = todateTextBox.Text;
        //    string url = "../SInventory_RPTVIEW/ProformaReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&NationalReport=" + 1;
        //    // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        //    string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        //    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        //}
    }
}