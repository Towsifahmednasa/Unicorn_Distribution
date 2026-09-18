using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;
using System.Drawing;

public partial class SInventory_UI_DeliverySalesReturn : System.Web.UI.Page
{
    private RequisitionBLL aRequisitionBll = new RequisitionBLL();
    private InvoiceBLL aInvoiceBll = new InvoiceBLL();

    private void WriteLog(string msg)
    {
        try
        {
            string path = @"C:\Users\Programmer25\.gemini\antigravity\brain\c46b17a5-f998-4767-a8c2-8bf583fb477f\debug.log";
            File.AppendAllText(path, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg + Environment.NewLine);
        }
        catch { }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            aInvoiceBll.PaymentTypeLoadBLL(payTypeDDL);
        }
    }

    private void GetCustInfo(string custCode)
    {
        WriteLog("GetCustInfo called for customer: " + custCode);
        if (!string.IsNullOrEmpty(custCode))
        {
            custCodeTextBox.Text = custCode;
            DataTable aDataTable = new DataTable();
            CustomerReturnBLL aCustomerReturnBll = new CustomerReturnBLL();
            aDataTable = aCustomerReturnBll.LoadCustomerInfoByCode(custCode);
            WriteLog("GetCustInfo: Customer query rows returned: " + aDataTable.Rows.Count);
            if (aDataTable.Rows.Count > 0)
            {
                hdCustomerMasterId.Value = aDataTable.Rows[0]["CustomerMasterId"].ToString();
                custNameTextBox.Text = aDataTable.Rows[0]["CustomerName"].ToString();
                custAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                hdMiaId.Value = aDataTable.Rows[0]["MIOId"].ToString();
                WriteLog("GetCustInfo: Successfully populated customer details.");
            }
        }
    }

    public void LoadInvoice(string invoiceId)
    {
        WriteLog("LoadInvoice called for InvoiceId: " + invoiceId);
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable dtinvoice = aOrderInfoBll.LoadInvoice(invoiceId);
        WriteLog("LoadInvoice: OrderInfoBLL.LoadInvoice returned rows: " + dtinvoice.Rows.Count);
        if (dtinvoice.Rows.Count > 0)
        {
            GetCustInfo(dtinvoice.Rows[0]["CustomerCode"].ToString());
            orderNoTextBox.Text = dtinvoice.Rows[0]["OrderNo"].ToString();
            deliverypersonNameTextBox.Text = dtinvoice.Columns.Contains("DeliveryPersonName") ? dtinvoice.Rows[0]["DeliveryPersonName"].ToString() : (dtinvoice.Columns.Contains("EmpName") ? dtinvoice.Rows[0]["EmpName"].ToString() : "");
            deliverypersonMobileTextBox.Text = dtinvoice.Columns.Contains("DeliveryPersonPhNo") ? dtinvoice.Rows[0]["DeliveryPersonPhNo"].ToString() : (dtinvoice.Columns.Contains("CellNumber") ? dtinvoice.Rows[0]["CellNumber"].ToString() : "");
            remarksTextBox.Text = dtinvoice.Rows[0]["Remarks"].ToString();
            invoiceNoHiddenField.Value = dtinvoice.Rows[0]["InvoiceNo"].ToString();
            
            invoiceNoLabel.Text = dtinvoice.Rows[0]["InvoiceNo"].ToString();
            deliveryInvoiceNoLabel.Text = dtinvoice.Rows[0]["DelivaryInvoiceNo"].ToString();
            
            if (dtinvoice.Columns.Contains("UpdateDate") && dtinvoice.Rows[0]["UpdateDate"] != DBNull.Value)
            {
                updateDateLabel.Text = Convert.ToDateTime(dtinvoice.Rows[0]["UpdateDate"].ToString()).ToString("dd-MMM-yyyy");
            }
            else if (dtinvoice.Columns.Contains("UpdateDatetime") && dtinvoice.Rows[0]["UpdateDatetime"] != DBNull.Value)
            {
                updateDateLabel.Text = Convert.ToDateTime(dtinvoice.Rows[0]["UpdateDatetime"].ToString()).ToString("dd-MMM-yyyy");
            }

            if (dtinvoice.Columns.Contains("InvoiceDate") && dtinvoice.Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                invDateTextBox.Text = Convert.ToDateTime(dtinvoice.Rows[0]["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy");
            }

            orderDateTextBox.Text = Convert.ToDateTime(dtinvoice.Rows[0]["OrderDate"].ToString())
                .ToString("dd-MMM-yyyy");
            invoiceHiddenField.Value = invoiceId;

            hdComUnitId.Value = dtinvoice.Rows[0]["ComUnitId"].ToString();
            string comUnitId = dtinvoice.Rows[0]["ComUnitId"].ToString();
            CustomerReturnBLL aCustomerReturnBll = new CustomerReturnBLL();
            if (!string.IsNullOrEmpty(comUnitId))
            {
                DataTable dtCom = aCustomerReturnBll.GetCompanyUnitById(comUnitId);
                if (dtCom.Rows.Count > 0)
                {
                    comUnitNameTextBox.Text = dtCom.Rows[0]["ComUnitCode"].ToString() + " : " + dtCom.Rows[0]["ComUnitName"].ToString();
                }
            }

            if (dtinvoice.Columns.Contains("Inv_DANameId") && dtinvoice.Rows[0]["Inv_DANameId"] != DBNull.Value)
            {
                string daId = dtinvoice.Rows[0]["Inv_DANameId"].ToString();
                if (!string.IsNullOrEmpty(daId))
                {
                    DataTable dtDa = aCustomerReturnBll.GetDANameById(daId);
                    if (dtDa.Rows.Count > 0)
                    {
                        daNameLabel.Text = dtDa.Rows[0]["Name"].ToString();
                    }
                }
            }

            string orderId = dtinvoice.Rows[0]["OrderId"].ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                DataTable dtOrder = aCustomerReturnBll.GetOrderSenderNameByOrderId(orderId);
                if (dtOrder.Rows.Count > 0)
                {
                    salesPersonNameLabel.Text = dtOrder.Rows[0]["OrderSenderName"].ToString();
                }
            }
            WriteLog("LoadInvoice: Successfully populated invoice header details.");
        }
    }

    private bool Validation()
    {
        int totalQty = 0;
        
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            string txtQty = ((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text.Trim();
            if (!string.IsNullOrEmpty(txtQty))
            {
                int qty;
                if (!int.TryParse(txtQty, out qty) || qty < 0)
                {
                    ShowMessageBox("Please enter a valid return quantity!");
                    return false;
                }
                
                decimal availableQty = Convert.ToDecimal(loadGridView.Rows[i].Cells[3].Text);
                if (qty > availableQty)
                {
                    ShowMessageBox("Return quantity cannot be greater than Available Quantity!");
                    return false;
                }
                
                totalQty += qty;
            }
        }

        if (totalQty <= 0)
        {
            ShowMessageBox("Please specify return quantity for at least one product!");
            return false;
        }

        int count = 0;
        for (int j = 0; j < paymentTypeRadioButtonList.Items.Count; j++)
        {
            if (paymentTypeRadioButtonList.Items[j].Selected)
            {
                count++;
            }
        }

        if (count == 0)
        {
            ShowMessageBox("Please select payment type !!!");
            return false;
        }

        return true;
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        WriteLog("saveButton_Click initiated.");
        if (Validation())
        {
            decimal totalamnt = 0;
            int totalqty = 0;
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                totalqty += string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text) ? 0 :
                    Convert.ToInt32(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text);
                totalamnt += string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text) ? 0 :
                    Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text);
            }

            decimal deductionCharge = 0;
            if (!string.IsNullOrEmpty(txtDeductionCharge.Text))
            {
                decimal.TryParse(txtDeductionCharge.Text.Trim(), out deductionCharge);
            }

            if (deductionCharge > totalamnt && totalamnt > 0)
            {
                ShowMessageBox("Deduction charge cannot be greater than Total Product Return Amount!");
                return;
            }

            decimal netReturn = totalamnt - deductionCharge;
            if (netReturn < 0) netReturn = 0;

            CustomerReturnMasterDAO aCustomerReturnMasterDao = new CustomerReturnMasterDAO()
            {
               InvoiceId = Convert.ToInt32(invoiceHiddenField.Value),
               TotalAmount = totalamnt,
               TotalQty = totalqty,
               DeductionCharge = deductionCharge,
               NetReturnAmount = netReturn,
               ActionStatus = "Posted",
               EntryBy = Session["LoginName"].ToString(),
               EntryDate = DateTime.Now,
            };

            CustomerReturnBLL aCustomerReturnBll = new CustomerReturnBLL();
            int id = 0;
            if (totalqty > 0)
            {
                id = aCustomerReturnBll.SaveCustomerReturn(aCustomerReturnMasterDao);   
            }
             
            if (id > 0)
            {
                for (int i = 0; i < loadGridView.Rows.Count; i++)
                {
                    CustomerReturnDetailDAO aCustomerReturnDetailDao = new CustomerReturnDetailDAO()
                    {
                        CustReturnId = id,
                        InvoiceDetailId = Convert.ToInt32(loadGridView.DataKeys[i][1].ToString()),
                        ReturnAmount = string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text) ? 0 :
                                        Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text),
                        ReturnQty = string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text) ? 0 :
                                       Convert.ToInt32(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text)
                    };
                    if (aCustomerReturnDetailDao.ReturnQty > 0)
                    {
                        int detailId = aCustomerReturnBll.SaveCustomerReturnDetail(aCustomerReturnDetailDao);
                        aCustomerReturnBll.InsertDCStoreStockReturn(aCustomerReturnDetailDao.InvoiceDetailId, aCustomerReturnDetailDao.ReturnQty, detailId);
                    }
                }
                
                aCustomerReturnBll.PostDeliverySalesReturn(id);

                Clear();
                ShowMessageBox("Data Saved and Posted Successfully !!!");
            }
        }
    }

    private void Clear()
    {
        hdComUnitId.Value = "";
        hdCustomerMasterId.Value = "";
        custNameTextBox.Text = "";
        custAddressTextBox.Text = "";
        comUnitNameTextBox.Text = "";
        hdMiaId.Value = "";
        custCodeTextBox.Text = "";
        orderNoTextBox.Text = "";
        loadGridView.DataSource = null;
        loadGridView.DataBind();

        delInvNoTextBox0.Text = "";
        payamntTextBox.Text = "";
        orderDateTextBox.Text = "";
        deliverypersonMobileTextBox.Text = "";
        deliverypersonNameTextBox.Text = "";
        remarksTextBox.Text = "";

        invoiceNoLabel.Text = "";
        deliveryInvoiceNoLabel.Text = "";
        updateDateLabel.Text = "";
        salesPersonNameLabel.Text = "";
        daNameLabel.Text = "";
        invDateTextBox.Text = "";

        txtTotalReturnAmount.Text = "0.00";
        txtDeductionCharge.Text = "0.00";
        txtNetReturnAmount.Text = "0.00";
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            string txt = delInvNoTextBox0.Text.Trim();
            WriteLog("Search button clicked. Search term: '" + txt + "'");
            CustomerReturnBLL aCustomerReturnBll = new CustomerReturnBLL();
            DataTable dt = aCustomerReturnBll.LoadInvoiceNew(txt);
            WriteLog("Search: LoadInvoiceNew returned rows: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                uiel.Visible = true;
                payamntTextBox.Text = dt.Rows[0]["Amnt"].ToString();
                LoadInvoice(dt.Rows[0]["InvoiceId"].ToString());
                loadGridView.DataSource = dt;
                loadGridView.DataBind();
                txtTotalReturnAmount.Text = "0.00";
                txtDeductionCharge.Text = "0.00";
                txtNetReturnAmount.Text = "0.00";
                WriteLog("Search: Databound successfully.");
            }
            else
            {
                WriteLog("Search: No rows returned from LoadInvoiceNew.");
                loadGridView.DataSource = null;
                loadGridView.DataBind();
                ShowMessageBox("Already Returned or Payment Exist or Invalid Delivery Invoice No. !!!");
            }
        }
        catch (Exception ex)
        {
            WriteLog("Search: EXCEPTION: " + ex.ToString());
            ShowMessageBox("Error: " + ex.Message);
        }
    }

    protected void retQtyTextBox_OnTextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            decimal qty = string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text) ? 0 :
                Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text);
            if (qty > Convert.ToDecimal(loadGridView.Rows[i].Cells[3].Text))
            {
                ShowMessageBox("Cannot be greater then Avialable Quantity");
                ((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text = string.Empty;
                ((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text = "0.00";
            }
            else
            {
                decimal tp = Convert.ToDecimal(loadGridView.DataKeys[i][2].ToString()) +
                             Convert.ToDecimal(loadGridView.DataKeys[i][3].ToString());
                ((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text = (qty * tp).ToString("F2");
            }
        }
        CalculateReturnTotals();
    }

    protected void txtDeductionCharge_TextChanged(object sender, EventArgs e)
    {
        CalculateReturnTotals();
    }

    private void CalculateReturnTotals()
    {
        decimal totalProductReturn = 0;
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            TextBox txtRetAmnt = (TextBox)loadGridView.Rows[i].FindControl("retamountTextBox");
            if (txtRetAmnt != null && !string.IsNullOrEmpty(txtRetAmnt.Text))
            {
                decimal val;
                if (decimal.TryParse(txtRetAmnt.Text, out val))
                {
                    totalProductReturn += val;
                }
            }
        }

        txtTotalReturnAmount.Text = totalProductReturn.ToString("0.00");

        decimal deductionCharge = 0;
        if (!string.IsNullOrEmpty(txtDeductionCharge.Text))
        {
            decimal.TryParse(txtDeductionCharge.Text.Trim(), out deductionCharge);
        }

        if (deductionCharge > totalProductReturn && totalProductReturn > 0)
        {
            ShowMessageBox("Deduction charge cannot be greater than Total Product Return Amount!");
            deductionCharge = 0;
            txtDeductionCharge.Text = "0.00";
        }

        decimal netReturn = totalProductReturn - deductionCharge;
        if (netReturn < 0) netReturn = 0;

        txtNetReturnAmount.Text = netReturn.ToString("0.00");
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected DropDownList payTypeDDL = new DropDownList();
}
