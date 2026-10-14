using System;
using System.Data;
using System.Linq;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_B2BTransferPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        string chalanNo = Request.QueryString["ChalanNo"];
        var dal = new B2BTransferViewDal();
        DataTable header = dal.GetChalanPrintHeader(chalanNo);

        if (string.IsNullOrEmpty(chalanNo) || header.Rows.Count == 0)
        {
            pnlReport.Visible = false;
            lblMessage.Visible = true;
            lblMessage.Text = "No Data Found!!";
            return;
        }

        DataRow h = header.Rows[0];
        DataTable lines = dal.GetChalanPrintLines(chalanNo);
        DataTable company = new StockTransportOrderReportBLL().CompanyInfoBLL();

        if (company.Rows.Count > 0)
        {
            litCompanyName.Text = Server.HtmlEncode(company.Rows[0]["CompanyName"].ToString());
            litCompanyAddress.Text = Server.HtmlEncode(company.Rows[0]["Address"].ToString());
            string contact = company.Rows[0]["ContactNo"].ToString();
            litCompanyContact.Text = contact == "" ? "" : "Contact: " + Server.HtmlEncode(contact);
        }

        litChalanNo.Text = Server.HtmlEncode(h["ChalanNo"].ToString());
        litChalanDate.Text = Date(h["ChalanDate"]);
        litFrom.Text = Server.HtmlEncode(h["FromComUnitName"].ToString());
        litTo.Text = Server.HtmlEncode(h["ToComUnitName"].ToString());
        litTrack.Text = Server.HtmlEncode(h["TrackNo"].ToString());
        litDriver.Text = Server.HtmlEncode(h["DriverName"].ToString());
        litStatus.Text = h["IsDeliver"].ToString() == "True" ? "Received" : "Pending";

        litTotalQty.Text = lines.AsEnumerable().Sum(r => r.IsNull("Quantity") ? 0m : Convert.ToDecimal(r["Quantity"])).ToString("#,##0.##");
        litTotalValue.Text = Money(h["TotalValue"]);
        litTotalVat.Text = Money(h["TotalVat"]);
        litGrandTotal.Text = Money(h["GrandTotal"]);

        rptLines.DataSource = lines;
        rptLines.DataBind();
    }

    private static string Money(object v)
    {
        decimal d;
        return decimal.TryParse(Convert.ToString(v), out d) ? d.ToString("#,##0.00") : "";
    }

    protected static string Date(object v)
    {
        DateTime d;
        return DateTime.TryParse(Convert.ToString(v), out d) ? d.ToString("dd-MMM-yyyy") : "";
    }
}
