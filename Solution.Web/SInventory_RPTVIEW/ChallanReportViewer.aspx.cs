using System;
using System.Data;
using System.Linq;
using Library.BLL.SInventory_BLL;

public partial class SInventory_RPTVIEW_ChallanReportViewer : System.Web.UI.Page
{
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        string reqId = Request.QueryString["reqId"];
        DataTable main = aOrderReportBll.StockTransportOrderReportMainDataBLL(reqId);

        if (main.Rows.Count == 0)
        {
            pnlReport.Visible = false;
            lblMessage.Visible = true;
            lblMessage.Text = "No Data Found!!";
            return;
        }

        DataRow m = main.Rows[0];
        DataTable lines = aOrderReportBll.StockTransportOrderReportDetailDataBLL(reqId);
        DataTable company = aOrderReportBll.CompanyInfoBLL();

        if (company.Rows.Count > 0)
        {
            litCompanyName.Text = Server.HtmlEncode(company.Rows[0]["CompanyName"].ToString());
            litCompanyAddress.Text = Server.HtmlEncode(company.Rows[0]["Address"].ToString());
        }

        litIssueOffice.Text = Server.HtmlEncode(m["WearhouseName"].ToString());
        litIssueAddress.Text = Server.HtmlEncode(m["WHAddress"].ToString());
        litDestination.Text = Server.HtmlEncode(m["ComUnitName"].ToString());
        litDestAddress.Text = Server.HtmlEncode(m["CUAddress"].ToString());
        litChalanNo.Text = Server.HtmlEncode(m["IssueChalanNo"].ToString());
        litChalanDate.Text = Date(m["IssuChalanDate"]);
        litPrintDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

        string truck = (m["TruckNo"].ToString() + " " + m["DriverName"].ToString()).Trim();
        trTruck.Visible = truck != "";
        litTruck.Text = Server.HtmlEncode(truck);

        litTotalQty.Text = Sum(lines, "Quantity").ToString("#,##0.##");
        litTotalPrice.Text = Sum(lines, "PriceAmount").ToString("#,##0.00");
        litTotalVat.Text = Sum(lines, "VATAmount").ToString("#,##0.00");
        litGrandTotal.Text = Sum(lines, "TotalPriceAmount").ToString("#,##0.00");

        rptLines.DataSource = lines;
        rptLines.DataBind();
    }

    private static decimal Sum(DataTable t, string col)
    {
        return t.AsEnumerable().Sum(r => r.IsNull(col) ? 0m : Convert.ToDecimal(r[col]));
    }

    protected static string Date(object v)
    {
        DateTime d;
        return DateTime.TryParse(Convert.ToString(v), out d) ? d.ToString("dd-MMM-yyyy") : "";
    }
}
