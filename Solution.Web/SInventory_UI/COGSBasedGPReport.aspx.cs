using ClosedXML.Excel;
using Library.BLL.SInventory_BLL;
using Library.DAL.MasterSetup_DAL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SInventory_UI_COGSBasedGPReport : System.Web.UI.Page
{
    private static CmnCrystaltoView _DAL = new CmnCrystaltoView();
    private DropDownList GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect;

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
            InvoiceDateTextBox.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd MMMM, yyyy");
            todateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadDropDown();
            LoadData();
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

            // Populate Customer Type
            DataTable dtCustType = _DAL.GetCustomerTypeListDAL();
            ddlCustomerType.DataSource = dtCustType;
            ddlCustomerType.DataTextField = "CustomerType";
            ddlCustomerType.DataValueField = "CustomerTypeId";
            ddlCustomerType.DataBind();
            ddlCustomerType.Items.Insert(0, new ListItem("--- Select Customer Type ---", ""));

            // Populate Product Name
            DataTable dtProduct = _DAL.GetProductListDAL();
            ddlProduct.DataSource = dtProduct;
            ddlProduct.DataTextField = "ProductName";
            ddlProduct.DataValueField = "ProductCode";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("--- Select Product ---", ""));

            // Populate Order Sender (Sales Executive)
            DataTable dtSender = _DAL.GetOrderSenderListDAL();
            ddlOrderSender.DataSource = dtSender;
            ddlOrderSender.DataTextField = "EmpName";
            ddlOrderSender.DataValueField = "EmpMasterCode";
            ddlOrderSender.DataBind();
            ddlOrderSender.Items.Insert(0, new ListItem("--- Select Sales Executive ---", ""));
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
        Response.Redirect("COGSBasedGPReport.aspx");
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    private string GetCacheKey()
    {
        string userId = Session["UserId"] != null ? Session["UserId"].ToString() : "Guest";
        return "COGSBasedGPReportData_" + userId;
    }

    private DataTable _DAL_GetGPReportDAL()
    {
        try
        {
            return _DAL.GetCOGSBasedGPReportDAL(Parm());
        }
        catch (Exception ex)
        {
            showMessageBox(ex.Message);
            return new DataTable();
        }
    }

    private void LoadData()
    {
        DataTable dt = _DAL_GetGPReportDAL();

        if (dt != null && dt.Rows.Count > 0)
        {
            Cache.Insert(GetCacheKey(), dt, null,
                DateTime.Now.AddMinutes(10),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        if (dt != null && dt.Rows.Count > 0)
        {
            loadGridView.DataSource = dt;
            loadGridView.DataBind();

            decimal totalRealization = dt.AsEnumerable().Sum(row => row.Field<decimal?>("RealizationAmount") ?? 0);
            lblCount.Text = "Total Realization Amount : " + totalRealization.ToString("N2");
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
            lblCount.Text = "Total Realization Amount : " + 0.ToString("N2");
        }
    }

    private string Parm()
    {
        string param = "";

        if (dcDropDownList1.SelectedValue != "")
        {
            param = param + " AND ISNULL(CPY.ComUnitId, ISNULL(INV.ComUnitId, mas.ComUnitId))='" + dcDropDownList1.SelectedValue + "' ";
        }

        if (ddlCustomer.SelectedValue != "")
        {
            param = param + " AND CMS.CustomerCode='" + ddlCustomer.SelectedValue + "' ";
        }

        if (ddlCustomerType.SelectedValue != "")
        {
            param = param + " AND CMS.CustomerTypeId='" + ddlCustomerType.SelectedValue + "' ";
        }

        if (ddlProduct.SelectedValue != "")
        {
            param = param + " AND INVD.ProductCode='" + ddlProduct.SelectedValue + "' ";
        }

        if (ddlOrderSender.SelectedValue != "")
        {
            param = param + " AND ISNULL(mas.OrderSenderCode, INV.OrderSenderCode)='" + ddlOrderSender.SelectedValue + "' ";
        }

        if (InvoiceDateTextBox.Text != "" && todateTextBox.Text != "")
        {
            param = param + " AND CONVERT(date, CPY.PaymentDate) BETWEEN '" + InvoiceDateTextBox.Text + "' AND '" + todateTextBox.Text + "' ";
        }
        else if (InvoiceDateTextBox.Text != "" && todateTextBox.Text == "")
        {
            param = param + " AND CONVERT(date, CPY.PaymentDate) BETWEEN '" + InvoiceDateTextBox.Text + "' AND '" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ";
        }

        if (GroupSelect != null && GroupSelect.SelectedValue != "")
        {
            param = param + " AND CMS.GroupId='" + GroupSelect.SelectedValue + "' ";
        }

        if (ZoneSelect != null && ZoneSelect.SelectedValue != "")
        {
            param = param + " AND CMS.RegionId='" + ZoneSelect.SelectedValue + "' ";
        }

        if (AreaSelect != null && AreaSelect.SelectedValue != "")
        {
            param = param + " AND CMS.AreaId='" + AreaSelect.SelectedValue + "' ";
        }

        if (TeritorySelect != null && TeritorySelect.SelectedValue != "")
        {
            param = param + " AND CMS.TerritoryId='" + TeritorySelect.SelectedValue + "' ";
        }

        if (SubTeritory != null && SubTeritory.SelectedValue != "")
        {
            param = param + " AND CMS.SubTerritoryId='" + SubTeritory.SelectedValue + "' ";
        }

        if (MarketSelect != null && MarketSelect.SelectedValue != "")
        {
            param = param + " AND CMS.MarketId='" + MarketSelect.SelectedValue + "' ";
        }

        return param;
    }

    protected void fromDateTextBox_TextChanged(object sender, EventArgs e)
    {
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        try
        {
            loadGridView.UseAccessibleHeader = true;
            loadGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        catch { }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (Cache[GetCacheKey()] != null)
            {
                dt = (DataTable)Cache[GetCacheKey()];
            }
            else
            {
                dt = _DAL_GetGPReportDAL();
            }

            if (dt.Rows.Count > 0)
            {
                // Select only display columns
                DataTable dtExport = new DataTable();
                dtExport.Columns.Add("Month", typeof(string));
                dtExport.Columns.Add("Date", typeof(string));
                dtExport.Columns.Add("Sales Center", typeof(string));
                dtExport.Columns.Add("BU Wise", typeof(string));
                dtExport.Columns.Add("Particulars", typeof(string));
                dtExport.Columns.Add("SKU", typeof(string));
                dtExport.Columns.Add("Qty", typeof(decimal));
                dtExport.Columns.Add("Name of Sales Executive", typeof(string));
                dtExport.Columns.Add("Realization Amount", typeof(decimal));
                dtExport.Columns.Add("VAT", typeof(decimal));
                dtExport.Columns.Add("Tax", typeof(decimal));
                dtExport.Columns.Add("COGS Amount", typeof(decimal));
                dtExport.Columns.Add("Net Amount", typeof(decimal));
                dtExport.Columns.Add("GP", typeof(decimal));
                dtExport.Columns.Add("GP %", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    dtExport.Rows.Add(
                        row["Month"],
                        row["Date"],
                        row["ComUnitName"],
                        row["BUWise"],
                        row["Particulars"],
                        row["SKU"],
                        row["Qty"],
                        row["OrderSenderName"],
                        row["RealizationAmount"],
                        row["VATAmount"],
                        row["TaxAmount"],
                        row["COGSAmount"],
                        row["NetAmount"],
                        row["GP"],
                        row["GPPercent"].ToString() + "%"
                    );
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtExport, "GP_Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=COGS_based_GP_Calculation_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            showMessageBox(ex.Message);
        }
    }
}
