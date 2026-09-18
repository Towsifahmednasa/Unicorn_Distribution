using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.MasterSetup_DAL;

public partial class SInventory_UI_SalesCollectionReport : System.Web.UI.Page
{
    private static CmnCrystaltoView _DAL = new CmnCrystaltoView();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd MMMM, yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadDropDowns();
        }
    }

    private void LoadDropDowns()
    {
        try
        {
            // 1. Delivery Agent (DA)
            DataTable dtDA = _DAL.GetDAListDAL();
            ddlDA.DataSource = dtDA;
            ddlDA.DataTextField = "DAName";
            ddlDA.DataValueField = "DAId";
            ddlDA.DataBind();
            ddlDA.Items.Insert(0, new ListItem("ALL", ""));

            // 2. Customer
            DataTable dtCustomer = _DAL.GetCustomerListDAL();
            ddlCustomer.DataSource = dtCustomer;
            ddlCustomer.DataTextField = "CustomerName";
            ddlCustomer.DataValueField = "CustomerCode";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("ALL", ""));

            // 3. Sales Person (MIO)
            DataTable dtSalesPerson = _DAL.GetOrderSenderListDAL();
            ddlSalesPerson.DataSource = dtSalesPerson;
            ddlSalesPerson.DataTextField = "EmpName";
            ddlSalesPerson.DataValueField = "EmpMasterCode";
            ddlSalesPerson.DataBind();
            ddlSalesPerson.Items.Insert(0, new ListItem("ALL", ""));

            // 4. Region (Zone)
            DataTable dtRegion = _DAL.GetRegionListDAL();
            ddlRegion.DataSource = dtRegion;
            ddlRegion.DataTextField = "RegionName";
            ddlRegion.DataValueField = "RegionId";
            ddlRegion.DataBind();
            ddlRegion.Items.Insert(0, new ListItem("ALL", ""));

            // 5. Product Name
            DataTable dtProduct = _DAL.GetProductListDAL();
            ddlProduct.DataSource = dtProduct;
            ddlProduct.DataTextField = "ProductName";
            ddlProduct.DataValueField = "ProductCode";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("ALL", ""));
        }
        catch (Exception ex)
        {
            showMessageBox("Error loading filters: " + ex.Message);
        }
    }

    private void LoadData()
    {
        try
        {
            string parm = BuildParmString();
            DataTable dt = _DAL.GetSalesCollectionReportDAL(parm);

            if (dt != null && dt.Rows.Count > 0)
            {
                loadGridView.DataSource = dt;
                loadGridView.DataBind();

                // Calculate sums
                decimal totalQty = 0;
                decimal totalAmount = 0;
                decimal totalBank = 0;
                decimal totalbKash = 0;
                decimal totalHand = 0;

                foreach (DataRow row in dt.Rows)
                {
                    totalQty += Convert.ToDecimal(row["Quantity"] == DBNull.Value ? 0 : row["Quantity"]);
                    totalAmount += Convert.ToDecimal(row["TotalAmount"] == DBNull.Value ? 0 : row["TotalAmount"]);
                    totalBank += Convert.ToDecimal(row["BankAmount"] == DBNull.Value ? 0 : row["BankAmount"]);
                    totalbKash += Convert.ToDecimal(row["bKashAmount"] == DBNull.Value ? 0 : row["bKashAmount"]);
                    totalHand += Convert.ToDecimal(row["HandCash"] == DBNull.Value ? 0 : row["HandCash"]);
                }

                // Bind sums to Footer
                if (loadGridView.FooterRow != null)
                {
                    loadGridView.FooterRow.Cells[0].Text = "TOTAL SUMMARY";
                    loadGridView.FooterRow.Cells[0].Font.Bold = true;

                    loadGridView.FooterRow.Cells[9].Text = totalQty.ToString("N0");
                    loadGridView.FooterRow.Cells[9].Font.Bold = true;
                    loadGridView.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                    loadGridView.FooterRow.Cells[11].Text = totalAmount.ToString("N2");
                    loadGridView.FooterRow.Cells[11].Font.Bold = true;
                    loadGridView.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;

                    loadGridView.FooterRow.Cells[12].Text = totalBank.ToString("N2");
                    loadGridView.FooterRow.Cells[12].Font.Bold = true;
                    loadGridView.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;

                    loadGridView.FooterRow.Cells[13].Text = totalbKash.ToString("N2");
                    loadGridView.FooterRow.Cells[13].Font.Bold = true;
                    loadGridView.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;

                    loadGridView.FooterRow.Cells[14].Text = totalHand.ToString("N2");
                    loadGridView.FooterRow.Cells[14].Font.Bold = true;
                    loadGridView.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                }

                // Bind Summary Highlights
                lblTotalBank.Text = totalBank.ToString("N2");
                lblTotalbKash.Text = totalbKash.ToString("N2");
                lblTotalHandCash.Text = totalHand.ToString("N2");
                lblGrandTotal.Text = (totalBank + totalbKash + totalHand).ToString("N2");
            }
            else
            {
                loadGridView.DataSource = null;
                loadGridView.DataBind();

                lblTotalBank.Text = "0.00";
                lblTotalbKash.Text = "0.00";
                lblTotalHandCash.Text = "0.00";
                lblGrandTotal.Text = "0.00";
            }
        }
        catch (Exception ex)
        {
            showMessageBox("Error loading data: " + ex.Message);
        }
    }

    private string BuildParmString()
    {
        StringBuilder sb = new StringBuilder();

        // Date filter (Collection/Invoice Date)
        if (!string.IsNullOrEmpty(txtDateFrom.Text) && !string.IsNullOrEmpty(txtDateTo.Text))
        {
            sb.Append(" AND CONVERT(date, I.InvoiceDate) BETWEEN '" + Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "'");
        }

        // Delivery Agent
        if (!string.IsNullOrEmpty(ddlDA.SelectedValue))
        {
            sb.Append(" AND EXISTS (SELECT 1 FROM tblCustPayDetail cpd WITH (NOLOCK) WHERE cpd.InvoiceId = I.InvoiceId AND cpd.DANameId = " + ddlDA.SelectedValue + ")");
        }

        // Customer
        if (!string.IsNullOrEmpty(ddlCustomer.SelectedValue))
        {
            sb.Append(" AND C.CustomerCode = '" + ddlCustomer.SelectedValue + "'");
        }

        // Sales Person (MIO)
        if (!string.IsNullOrEmpty(ddlSalesPerson.SelectedValue))
        {
            sb.Append(" AND EGI.EmpMasterCode = '" + ddlSalesPerson.SelectedValue + "'");
        }

        // Region
        if (!string.IsNullOrEmpty(ddlRegion.SelectedValue))
        {
            sb.Append(" AND rg.RegionId = " + ddlRegion.SelectedValue);
        }

        // Product Name
        if (!string.IsNullOrEmpty(ddlProduct.SelectedValue))
        {
            sb.Append(" AND dtl.ProductCode = '" + ddlProduct.SelectedValue + "'");
        }

        // Payment Channel
        if (!string.IsNullOrEmpty(ddlPaymentChannel.SelectedValue))
        {
            string channel = ddlPaymentChannel.SelectedValue;
            if (channel == "Bank")
            {
                sb.Append(" AND EXISTS (SELECT 1 FROM tblCustPayDetail cpd WITH (NOLOCK) WHERE cpd.InvoiceId = I.InvoiceId AND cpd.BankAccId > 0 AND cpd.BankAccId <> 53)");
            }
            else if (channel == "bKash")
            {
                sb.Append(" AND EXISTS (SELECT 1 FROM tblCustPayDetail cpd WITH (NOLOCK) WHERE cpd.InvoiceId = I.InvoiceId AND cpd.BankAccId = 53)");
            }
            else if (channel == "Hand Cash")
            {
                sb.Append(" AND EXISTS (SELECT 1 FROM tblCustPayDetail cpd WITH (NOLOCK) WHERE cpd.InvoiceId = I.InvoiceId AND cpd.CashAccId > 0)");
            }
        }

        return sb.ToString();
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        loadGridView.PageIndex = 0;
        LoadData();
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        txtDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd MMMM, yyyy");
        txtDateTo.Text = DateTime.Now.ToString("dd MMMM, yyyy");
        ddlDA.SelectedIndex = 0;
        ddlCustomer.SelectedIndex = 0;
        ddlSalesPerson.SelectedIndex = 0;
        ddlPaymentChannel.SelectedIndex = 0;
        ddlRegion.SelectedIndex = 0;
        ddlProduct.SelectedIndex = 0;
        LoadData();
    }

    protected void loadGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        loadGridView.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void gv_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if ((gv.ShowHeader == true && gv.Rows.Count > 0) || (gv.ShowHeaderWhenEmpty == true))
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (loadGridView.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Sales_Collection_Report_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".csv");
            Response.Charset = "";
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.Default;

            loadGridView.AllowPaging = false;
            LoadData();

            StringBuilder sb = new StringBuilder();
            List<int> visibleColIndices = new List<int>();

            for (int colIndex = 0; colIndex < loadGridView.Columns.Count; colIndex++)
            {
                if (loadGridView.Columns[colIndex].Visible)
                {
                    visibleColIndices.Add(colIndex);
                    sb.Append(HttpUtility.HtmlDecode(loadGridView.HeaderRow.Cells[colIndex].Text) + ',');
                }
            }
            sb.Append("\r\n");

            foreach (GridViewRow row in loadGridView.Rows)
            {
                foreach (int colIndex in visibleColIndices)
                {
                    TableCell cell = row.Cells[colIndex];
                    string cellText = HttpUtility.HtmlDecode(cell.Text).Trim();
                    if (cellText.Contains(","))
                    {
                        sb.Append(String.Format("\"{0}\",", cellText));
                    }
                    else
                    {
                        sb.Append(cellText + ',');
                    }
                }
                sb.Append("\r\n");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            showMessageBox("No Data Found to Export!");
        }
    }

    private void showMessageBox(string message)
    {
        string sScript = String.Format("alert('{0}');", message.Replace("'", "\\'"));
        ScriptManager.RegisterStartupScript(this, GetType(), "alert", sScript, true);
    }
}
