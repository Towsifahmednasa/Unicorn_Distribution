using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

// List of saved Invoice Adjustments (tblInvoiceBadDebtsMaster) with per-row line detail.
public partial class SInventory_UI_InvoiceAdjustmentList : System.Web.UI.Page
{
    private readonly InvoiceAdjustmentDAL aDal = new InvoiceAdjustmentDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] == null || string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                Response.Redirect("~/Login.aspx");
            }
            fromDateTextBox.Text = DateTime.Now.AddDays(-30).ToString("dd MMMM, yyyy");
            toDateTextBox.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadList();
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadList();
    }

    private void LoadList()
    {
        DateTime fromDate, toDate;
        if (!DateTime.TryParse(fromDateTextBox.Text.Trim(), out fromDate) || !DateTime.TryParse(toDateTextBox.Text.Trim(), out toDate))
        {
            ShowError("Please select From Date and To Date.");
            return;
        }

        DataTable dt = aDal.GetAdjustmentList(fromDate, toDate);
        listGridView.DataSource = dt;
        listGridView.DataBind();

        decimal total = 0;
        foreach (DataRow r in dt.Rows) total += Convert.ToDecimal(r["TotalAdjustment"]);
        totalLabel.Text = total.ToString("N2");
    }

    protected void listGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "ViewDetail") return;

        int badDebtsId = Convert.ToInt32(e.CommandArgument);
        foreach (GridViewRow row in listGridView.Rows)
        {
            if (Convert.ToInt32(listGridView.DataKeys[row.RowIndex].Value) != badDebtsId) continue;
            detailIdLabel.Text = badDebtsId.ToString();
            detailInvoiceLabel.Text = row.Cells[6].Text;
            detailCustomerLabel.Text = row.Cells[5].Text + " (" + row.Cells[4].Text + ")";
            detailTypeLabel.Text = row.Cells[2].Text;
            detailTotalLabel.Text = row.Cells[8].Text;
        }

        detailGridView.DataSource = aDal.GetAdjustmentDetails(badDebtsId);
        detailGridView.DataBind();
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowDetailModal", "$('#detailModal').modal('show');", true);
    }

    private void ShowError(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + message.Replace("'", "\\'") + "','Faild');", true);
    }
}
