using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_StockTransferReport : System.Web.UI.Page
{
    private StockTransferReportBLL _bll = new StockTransferReportBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDowns();
            // Default date range: last 30 days
            fromDateTextBox.Text = DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy");
            toDateTextBox.Text   = DateTime.Now.ToString("dd-MMM-yyyy");
            LoadReport();
        }
    }

    private void BindDropDowns()
    {
        _bll.LoadCompanyUnits(fromLocationDropDownList);
        fromLocationDropDownList.Items.Insert(0, new ListItem("-- All Locations --", "0"));

        _bll.LoadCompanyUnits(toLocationDropDownList);
        toLocationDropDownList.Items.Insert(0, new ListItem("-- All Locations --", "0"));
    }

    private void LoadReport()
    {
        string fromDate       = fromDateTextBox.Text.Trim();
        string toDate         = toDateTextBox.Text.Trim();
        string fromLocationId = fromLocationDropDownList.SelectedValue;
        string toLocationId   = toLocationDropDownList.SelectedValue;

        DataTable dt = _bll.LoadStockTransferReport(fromDate, toDate, fromLocationId, toLocationId);

        loadGridView.DataSource = dt;
        loadGridView.DataBind();

        // Update summary labels
        totalRecordsLabel.Text = dt.Rows.Count.ToString("N0");

        if (dt.Rows.Count > 0 && dt.Columns.Contains("Amount"))
        {
            decimal totalAmt = 0;
            foreach (DataRow row in dt.Rows)
            {
                decimal val;
                if (decimal.TryParse(row["Amount"].ToString(), out val))
                    totalAmt += val;
            }
            totalAmountLabel.Text = totalAmt.ToString("N2");
        }
        else
        {
            totalAmountLabel.Text = "0.00";
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadReport();
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        fromDateTextBox.Text = DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy");
        toDateTextBox.Text   = DateTime.Now.ToString("dd-MMM-yyyy");
        fromLocationDropDownList.SelectedIndex = 0;
        toLocationDropDownList.SelectedIndex   = 0;
        LoadReport();
    }

    protected void gv_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if ((gv.ShowHeader == true && gv.Rows.Count > 0) || gv.ShowHeaderWhenEmpty == true)
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}
