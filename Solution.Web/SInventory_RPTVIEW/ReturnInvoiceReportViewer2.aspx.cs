using System;
using System.Data;
using System.Linq;
using Library.BLL.SInventory_BLL;

public partial class SInventory_RPTVIEW_ReturnInvoiceReportViewer2 : System.Web.UI.Page
{
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        string invNo = Server.UrlDecode(Request.QueryString["InvNo"]);
        DataTable code = aInvoiceBll.FindInvoiceCode(invNo);
        if (code.Rows.Count == 0) { NoData(); return; }

        string invColl = aInvoiceBll.InvoiceNoCollectionFormate(code.Rows[0]["ReturnInvoiceNo"].ToString());
        DataTable main = aInvoiceBll.RInvoiceMainDataForReportBLL(invColl);
        if (main.Rows.Count == 0) { NoData(); return; }

        DataRow m = main.Rows[0];
        DataTable lines = aInvoiceBll.RInvoiceDetailDataForReportBLL(invColl);
        DataTable company = aOrderReportBll.CompanyInfoBLL();

        if (company.Rows.Count > 0)
        {
            litCompanyName.Text = Server.HtmlEncode(company.Rows[0]["CompanyName"].ToString());
            litCompanyAddress.Text = Server.HtmlEncode(company.Rows[0]["Address"].ToString());
        }

        litUnitCode.Text = Server.HtmlEncode(m["ComUnitCode"].ToString());
        litUnitName.Text = Server.HtmlEncode(m["CompanyName"].ToString());
        litUnitAddress.Text = Server.HtmlEncode(m["CUAddress"].ToString());
        litInvoiceNo.Text = Server.HtmlEncode(m["InvoiceNo"].ToString());
        litInvoiceDate.Text = Date(m["CreateDate"], "dd-MMM-yyyy hh:mm tt");
        litOrderNo.Text = Server.HtmlEncode(m["OrderNo"].ToString().Trim() == "" ? "No order" : m["OrderNo"].ToString());
        litOrderDate.Text = Date(m["OrderDate"], "dd-MMM-yyyy");

        litCustomerCode.Text = Server.HtmlEncode(m["CustomerCode"].ToString());
        litCustomerName.Text = Server.HtmlEncode(m["CustomerName"].ToString());
        litCustomerAddress.Text = Server.HtmlEncode((m["CMAddress"].ToString() + " " + m["Addrees2"].ToString()).Trim());
        litPaymentType.Text = Server.HtmlEncode(m["PaymentTypeName"].ToString());
        litMia.Text = Server.HtmlEncode((m["MiaCode"].ToString() + " " + m["MiaName"].ToString()).Trim());
        litPrintDate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

        litTotalQty.Text = lines.AsEnumerable().Sum(r => r.IsNull("Quantity") ? 0m : Convert.ToDecimal(r["Quantity"])).ToString("#,##0.##");
        litTpTotal.Text = Money(m["TpTotal"]);
        litTpDiscount.Text = Money(m["TpDiscount"]);
        litTpVat.Text = Money(m["TpVat"]);
        litTpGrandTotal.Text = Money(m["TpGrandTotal"]);

        rptLines.DataSource = lines;
        rptLines.DataBind();
    }

    private void NoData()
    {
        pnlReport.Visible = false;
        lblMessage.Visible = true;
        lblMessage.Text = "No Data Found!!";
    }

    private static string Money(object v)
    {
        decimal d;
        return decimal.TryParse(Convert.ToString(v), out d) ? d.ToString("#,##0.00") : "0.00";
    }

    private static string Date(object v, string fmt)
    {
        DateTime d;
        return DateTime.TryParse(Convert.ToString(v), out d) ? d.ToString(fmt) : "";
    }
}
