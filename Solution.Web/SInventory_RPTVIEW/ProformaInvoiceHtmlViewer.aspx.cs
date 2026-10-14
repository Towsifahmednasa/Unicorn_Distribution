using System;
using System.Data;
using System.Linq;
using Library.BLL.SInventory_BLL;

public partial class SInventory_RPTVIEW_ProformaInvoiceHtmlViewer : System.Web.UI.Page
{
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        string invNo = Server.UrlDecode(Request.QueryString["InvNo"]);
        DataTable main = string.IsNullOrEmpty(invNo) ? null : aInvoiceBll.InvoiceMainDatainvNo(invNo);
        if (main == null || main.Rows.Count == 0)
        {
            pnlReport.Visible = false;
            lblMessage.Visible = true;
            lblMessage.Text = "No Data Found!!";
            return;
        }

        DataRow m = main.Rows[0];
        DataTable lines = aInvoiceBll.InvoiceDetailDataInvoID(invNo);
        DataTable company = aOrderReportBll.CompanyInfoBLL();

        if (company.Rows.Count > 0)
        {
            litCompanyName.Text = Server.HtmlEncode(company.Rows[0]["CompanyName"].ToString());
            litCompanyAddress.Text = Server.HtmlEncode(company.Rows[0]["Address"].ToString());
        }

        litPrintDate.Text = DateTime.Now.ToString("hh:mm tt  /  dd-MM-yyyy");
        litCustomerCode.Text = Server.HtmlEncode(m["CustomerCode"].ToString());
        litCustomerName.Text = Server.HtmlEncode(m["CustomerName"].ToString());
        litBillingAddress.Text = Server.HtmlEncode(m["CMAddress"].ToString());
        litDeliveryAddress.Text = Server.HtmlEncode(m["Addrees2"].ToString());
        litBranch.Text = Server.HtmlEncode(m["CompanyName"].ToString());
        litSalesPerson.Text = Server.HtmlEncode(m["MiaName"].ToString());
        litContact.Text = Server.HtmlEncode(m["DeliveryPersonPhNo"].ToString());
        litCreditDays.Text = Server.HtmlEncode(m["RemainingCreditDays"].ToString());
        litOrderNo.Text = Server.HtmlEncode(m["OrderNo"].ToString()) + "<br/>" + Date(m["OrderDate"], "dd-MMM-yyyy hh:mm tt");
        litInvoiceNo.Text = Server.HtmlEncode(m["InvoiceNo"].ToString()) + "<br/>" + Date(m["InvoiceDate"], "dd-MMM-yyyy hh:mm tt");
        litDeliveryDate.Text = Date(m["DeliveryDate"], "dd-MMM-yyyy hh:mm tt");
        litPayment.Text = Server.HtmlEncode(m["PaymentTypeName"].ToString());

        litTotalQty.Text = Sum(lines, "Quantity").ToString("#,##0.##");
        litTotalTp.Text = Sum(lines, "TotalPrice").ToString("#,##0.00");
        litTotalVat.Text = Sum(lines, "TotalPriceVatAmount").ToString("#,##0.00");
        litTotalDiscount.Text = Sum(lines, "DiscountAmount").ToString("#,##0.00");
        litTotalNet.Text = Sum(lines, "NetAmount").ToString("#,##0.00");

        decimal delivery = ToDec(m["DeliveryCharge"]);
        trDelivery.Visible = delivery > 0;
        litDelivery.Text = delivery.ToString("#,##0.00");
        litGrandTotal.Text = ToDec(m["TpGrandTotal"]).ToString("#,##0.00");
        litTotalDue.Text = ToDec(m["TotalDue"]).ToString("#,##0.00");

        rptLines.DataSource = lines;
        rptLines.DataBind();
    }

    private static decimal ToDec(object v)
    {
        decimal d;
        return decimal.TryParse(Convert.ToString(v), out d) ? d : 0m;
    }

    private static decimal Sum(DataTable t, string col)
    {
        return t.AsEnumerable().Sum(r => r.IsNull(col) ? 0m : Convert.ToDecimal(r[col]));
    }

    private static string Date(object v, string fmt)
    {
        DateTime d;
        return DateTime.TryParse(Convert.ToString(v), out d) ? d.ToString(fmt) : "";
    }

    protected static string Date(object v)
    {
        return Date(v, "dd-MMM-yyyy");
    }
}
