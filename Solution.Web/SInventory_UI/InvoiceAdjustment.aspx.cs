using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

// Invoice Adjustment: write off part of a delivered invoice as Bad Debts or Promotional Expense.
// Saves tblInvoiceBadDebtsMaster/Details and posts Dr Expense / Cr Accounts Receivable
// (sp_Sales_InvoiceAdjustmentPosting) in one TransactionScope, same shape as CustomerPayment.
public partial class SInventory_UI_InvoiceAdjustment : System.Web.UI.Page
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
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        // autocomplete returns "InvoiceId:InvoiceNo"; a hand-typed value is just the number
        string invoiceNo = invoiceNoTextBox.Text.Trim();
        if (invoiceNo.Contains(":")) invoiceNo = invoiceNo.Substring(invoiceNo.IndexOf(':') + 1).Trim();
        invoiceNoTextBox.Text = invoiceNo;

        if (invoiceNo == "")
        {
            ShowError("Please input Invoice No.");
            return;
        }

        DataTable header = aDal.GetInvoiceHeader(invoiceNo, 0); // no unit filter: invoice no is unique, user may span units
        if (header.Rows.Count == 0)
        {
            ClearDetails();
            ShowError("Invoice not found.");
            return;
        }

        DataRow h = header.Rows[0];
        if (h["DelivaryInvoiceNo"] == DBNull.Value || h["DelivaryInvoiceNo"].ToString() == "")
        {
            ClearDetails();
            ShowError("Invoice is not delivered yet. Only delivered invoices can be adjusted.");
            return;
        }

        hfInvoiceId.Value = h["InvoiceId"].ToString();
        hfCustomerMasterId.Value = h["CustomerMasterId"].ToString();
        hfComUnitId.Value = h["ComUnitId"].ToString();
        customerLabel.Text = h["CustomerName"] + " (" + h["CustomerCode"] + ")";
        invoiceDateLabel.Text = Convert.ToDateTime(h["InvoiceDate"]).ToString("dd-MMM-yyyy");
        deliveryInvoiceLabel.Text = h["DelivaryInvoiceNo"].ToString();
        deliveredAmountLabel.Text = Convert.ToDecimal(h["DeliveredAmount"]).ToString("N2");
        paidAmountLabel.Text = Convert.ToDecimal(h["PaidAmount"]).ToString("N2");
        prevAdjustmentLabel.Text = Convert.ToDecimal(h["PrevAdjustment"]).ToString("N2");

        LoadDetails();
    }

    private void LoadDetails()
    {
        detailGridView.DataSource = aDal.GetInvoiceDetails(Convert.ToInt32(hfInvoiceId.Value));
        detailGridView.DataBind();
        detailPanel.Visible = detailGridView.Rows.Count > 0;
        if (!detailPanel.Visible) ShowError("No invoice lines found.");
    }

    private void ClearDetails()
    {
        hfInvoiceId.Value = "";
        detailGridView.DataSource = null;
        detailGridView.DataBind();
        detailPanel.Visible = false;
        remarksTextBox.Text = "";
    }

    protected void detailGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        DataRowView row = (DataRowView)e.Row.DataItem;
        TextBox adj = (TextBox)e.Row.FindControl("adjustmentTextBox");
        decimal max = MaxAdjustable(row.Row);
        adj.Attributes["data-max"] = max.ToString("0.00", CultureInfo.InvariantCulture);

        if (max <= 0)
        {
            adj.Enabled = false;
            adj.Text = "0.00";
            if (row["DeliveryStatus"].ToString() == "Reject")
                e.Row.CssClass = "table-danger";
        }
    }

    // DeliveryNetAmount less what was already adjusted on this line; 0 for rejected lines.
    private static decimal MaxAdjustable(DataRow r)
    {
        if (r["DeliveryStatus"].ToString() == "Reject") return 0;
        decimal max = Convert.ToDecimal(r["DeliveryNetAmount"]) - Convert.ToDecimal(r["PrevAdjustment"]);
        return max > 0 ? max : 0;
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        int invoiceId;
        if (!int.TryParse(hfInvoiceId.Value, out invoiceId) || invoiceId <= 0)
        {
            ShowError("Please search an invoice first.");
            return;
        }

        // Fresh read inside the save so a stale page cannot exceed the current cap.
        Dictionary<int, DataRow> lines = new Dictionary<int, DataRow>();
        foreach (DataRow r in aDal.GetInvoiceDetails(invoiceId).Rows)
            lines[Convert.ToInt32(r["InvoiceDetailId"])] = r;

        InvoiceAdjustment m = new InvoiceAdjustment
        {
            InvoiceId = invoiceId,
            InvoiceNo = invoiceNoTextBox.Text.Trim(),
            CustomerMasterId = ToInt(hfCustomerMasterId.Value),
            ComUnitId = ToInt(hfComUnitId.Value),
            AdjustmentType = adjustmentTypeRadioButtonList.SelectedValue,
            AdjustmentDate = DateTime.Now,
            Remarks = remarksTextBox.Text.Trim(),
            EntryBy = Session["LoginName"] != null ? Session["LoginName"].ToString() : Session["UserId"].ToString()
        };

        foreach (GridViewRow gr in detailGridView.Rows)
        {
            TextBox adj = (TextBox)gr.FindControl("adjustmentTextBox");
            decimal amount;
            if (!adj.Enabled || !decimal.TryParse(adj.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount) || amount <= 0)
                continue;

            int detailId = Convert.ToInt32(detailGridView.DataKeys[gr.RowIndex].Value);
            DataRow line;
            if (!lines.TryGetValue(detailId, out line))
            {
                ShowError("Invoice line not found. Please search again.");
                return;
            }

            decimal max = MaxAdjustable(line);
            if (amount > max)
            {
                ShowError("Adjustment for " + line["ProductCode"] + " cannot exceed " + max.ToString("N2") + ".");
                return;
            }

            m.Details.Add(new InvoiceAdjustmentDetail
            {
                InvoiceDetailId = detailId,
                ProductCode = line["ProductCode"].ToString(),
                DeliveryNetAmount = Convert.ToDecimal(line["DeliveryNetAmount"]),
                AdjustmentAmount = amount
            });
            m.TotalAdjustment += amount;
        }

        if (m.Details.Count == 0)
        {
            ShowError("Please input at least one Adjustment Amount.");
            return;
        }

        try
        {
            TransactionOptions txOpt = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(2)
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {
                int badDebtsId = aDal.SaveMaster(m);
                if (badDebtsId <= 0) throw new Exception("Adjustment master save failed");

                foreach (InvoiceAdjustmentDetail d in m.Details)
                {
                    if (!aDal.SaveDetail(badDebtsId, invoiceId, d)) throw new Exception("Adjustment detail save failed");
                }

                aDal.PostJournal(badDebtsId);
                if (!aDal.IsJournalPosted(badDebtsId)) throw new Exception("Accounts voucher posting failed");

                scope.Complete();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup",
                "ShowSuccesalert('" + m.AdjustmentType + " of " + m.TotalAdjustment.ToString("N2") + " saved and posted to accounts.','Success');", true);

            remarksTextBox.Text = "";
            searchButton_Click(null, null); // reload with updated previous-adjustment figures
        }
        catch (Exception ex)
        {
            ShowError("Invoice adjustment failed: " + ex.Message);
        }
    }

    private static int ToInt(string s)
    {
        int v;
        return int.TryParse(s, out v) ? v : 0;
    }

    private void ShowError(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + message.Replace("'", "\\'") + "','Faild');", true);
    }
}
