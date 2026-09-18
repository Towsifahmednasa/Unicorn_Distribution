using System;
using System.Web.UI;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_SalesOperationRollback : System.Web.UI.Page
{
    private SalesOperationRollbackBLL aRollbackBll = new SalesOperationRollbackBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Clear();
        }
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\\'");
        message = message.Replace("\r\n", " ");
        sScript = string.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void Clear()
    {
        txtCode.Text = string.Empty;
        txtRemarks.Text = string.Empty;
    }

    protected void RollbackType_CheckedChanged(object sender, EventArgs e)
    {
        txtCode.Text = string.Empty;
        if (rdoProforma.Checked)
        {
            lblCodeLabel.Text = "Proforma Invoice No:";
        }
        else if (rdoDelivery.Checked)
        {
            lblCodeLabel.Text = "Delivery Invoice No:";
        }
        else if (rdoPayment.Checked)
        {
            lblCodeLabel.Text = "Money Receipt No:";
        }
    }

    protected void btnRollback_Click(object sender, EventArgs e)
    {
        string code = txtCode.Text.Trim();
        string remarks = txtRemarks.Text.Trim();

        if (string.IsNullOrEmpty(code))
        {
            showMessageBox("Please enter the Code/No!");
            return;
        }

        if (string.IsNullOrEmpty(remarks))
        {
            showMessageBox("Please enter the reason/remarks for rollback!");
            return;
        }

        if (Session["LoginName"] == null)
        {
            showMessageBox("User session is invalid. Please log in again.");
            return;
        }

        string user = Session["LoginName"].ToString();
        string result = "";

        if (rdoProforma.Checked)
        {
            result = aRollbackBll.RollbackProforma(code, user, remarks);
        }
        else if (rdoDelivery.Checked)
        {
            result = aRollbackBll.RollbackDeliveryInvoice(code, user, remarks);
        }
        else if (rdoPayment.Checked)
        {
            result = aRollbackBll.RollbackCustomerPayment(code, user, remarks);
        }

        if (result == "SUCCESS")
        {
            showMessageBox("Rollback operation successfully completed!");
            Clear();
        }
        else
        {
            showMessageBox(result);
        }
    }
}
