using Library.BLL.SInventory_BLL;
using Library.DAL.MasterSetup_DAL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;
using SalesSolution.Web.DataLayer;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SInventory_UI_CustomerPayment : System.Web.UI.Page
{
    OrderInfoDAL aDal = new OrderInfoDAL();
    private CustPaymentBLL aCustPaymentBll = new CustPaymentBLL();
    OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
    private static SeedDataDAL _seedRepo = new SeedDataDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            paymentDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["SalesCenter"] != null)
            {

                aCustPaymentBll.LoadSC(salesCenterDropDownList, Session["UserId"].ToString());


                salesCenterDropDownList.SelectedValue = Session["SalesCenter"].ToString();

                salesCenterDropDownList_SelectedIndexChanged(null, null);

                // Route
                if (Session["Route"] != null)
                {
                    rootDropDownList.SelectedValue = Session["Route"].ToString();
                    rootDropDownList_SelectedIndexChanged(null, null);

                }

                // Territory
                if (Session["Territory"] != null)
                {
                    ddlTerritoryName.SelectedValue = Session["Territory"].ToString();

                    DataTable aTable = aDal.LoadPaymentInvSP(GenerateParam());

                    if (aTable != null && aTable.Rows.Count > 0)
                    {
                        DataView dv = aTable.DefaultView;
                        dv.Sort = "InvoiceDate ASC, InvoiceId ASC";
                        aTable = dv.ToTable();
                    }

                    orderGridView.DataSource = aTable;
                    orderGridView.DataBind();
                    CollectionByChange();


                    aCustPaymentBll.PaymentTypeLoadBLL(payTypeDDL);

                    payTypeDDL.SelectedIndex = 1;
                }
            }
            else
            {

                DropDownList();
            }
         
            
        }
    }

    public void Clear()
    {
        //orderGridView.DataSource = null;
        //orderGridView.DataBind();

        //salesCenterDropDownList.SelectedIndex = 0;
        marketDropDownList.SelectedValue = "";
        //customerDropDownList.SelectedIndex = 0;
      //  paymentDtTextBox.Text = string.Empty;
 
        refDtTextBox.Text = string.Empty;
        refNameTextBox.Text = string.Empty;
        paymentDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        remarksTextBox.Text = string.Empty;
        checkNoTextBox.Text = string.Empty;
        checkDateTextBox.Text = string.Empty;
        txtDisburseAmount.Text = string.Empty;
        payTypeDDL.SelectedIndex = 1;
        //customerTextBox.Text = string.Empty;

        txtCustomerSearch.Text = string.Empty;
        hfCustomerSearchId.Value = string.Empty;


    }

    public void DropDownList()
    {
        try
        {
            aCustPaymentBll.LoadSC(salesCenterDropDownList, Session["UserId"].ToString());

           
                salesCenterDropDownList.SelectedIndex = 1;
             
            aCustPaymentBll.PaymentTypeLoadBLL(payTypeDDL);

            payTypeDDL.SelectedIndex = 1;
            salesCenterDropDownList_SelectedIndexChanged(null, null);

        }
        catch(Exception ex)
        {

        }

    }

    protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlDAName.Items.Clear();
            try
            {
                using (DataTable dt = _seedRepo.GetRouteInfoforCustPayment(Convert.ToInt32(salesCenterDropDownList.SelectedValue)))
                {
                    rootDropDownList.DataSource = dt;

                    rootDropDownList.DataValueField = "DistributionRouteId";
                    rootDropDownList.DataTextField = "DistributionRouteName";
                    rootDropDownList.DataBind();
                    rootDropDownList.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    rootDropDownList.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }
        }
        catch (Exception ex)
        {

        }
    }

    protected void txtCustomerSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string inputText = txtCustomerSearch.Text.Trim();
            if (!string.IsNullOrEmpty(inputText))
            {
                if (inputText.Contains('|'))
                {
                    string[] parts = inputText.Split('|');
                    hfCustomerSearchId.Value = parts[1].Trim();
                    txtCustomerSearch.Text = parts[0].Trim();
                }
                else
                {
                    hfCustomerSearchId.Value = "";
                }
            }
            else
            {
                hfCustomerSearchId.Value = "";
            }
        }
        catch (Exception ex)
        {
            hfCustomerSearchId.Value = "";
        }
    }
 
    

    protected void customerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
      
    }

    protected void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aCustPaymentBll.LoadCustomerMaster(customerDropDownList, marketDropDownList.SelectedValue);
        orderGridView.DataSource = null;
        orderGridView.DataBind();
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox) orderGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox) orderGridView.Rows[i].FindControl("chkSelect");
            TextBox amountBox = (TextBox)orderGridView.Rows[i].FindControl("payAmountTextBox");
            TextBox discountBox = (TextBox)orderGridView.Rows[i].FindControl("discountTextBox");
            TextBox aitBox = (TextBox)orderGridView.Rows[i].FindControl("aitTextBox");
            DropDownList ddlBank = (DropDownList)orderGridView.Rows[i].FindControl("ddlBank");
            DropDownList ddlCash = (DropDownList)orderGridView.Rows[i].FindControl("ddlCash");
            CheckBox cbBank = (CheckBox)orderGridView.Rows[i].FindControl("isBank");
            CheckBox cbCash = (CheckBox)orderGridView.Rows[i].FindControl("isCash");

            var paymentTypeName = orderGridView.DataKeys[i]["PaymentTypeName"].ToString();

            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
                Label lblDue = (Label)orderGridView.Rows[i].FindControl("lblDue");
                if (amountBox != null && lblDue != null)
                {
                    amountBox.Text = lblDue.Text;
                }
                if (discountBox != null)
                {
                    discountBox.Text = "0.00";
                }
                if (aitBox != null)
                {
                    aitBox.Text = "0.00";
                }

                if (paymentTypeName.Trim() == "Cash")
                {
                    if (cbCash != null) cbCash.Checked = true;
                    if (cbBank != null) cbBank.Checked = false;
                    if (ddlBank != null) ddlBank.Items.Clear();
                    SetBankOrCashAccount(i, "Cash");
                }
                else
                {
                    if (cbCash != null) cbCash.Checked = false;
                    if (cbBank != null) cbBank.Checked = true;
                    if (ddlCash != null) ddlCash.Items.Clear();
                    SetBankOrCashAccount(i, "Bank");
                }

                PayAmountChange(i);
            }
            else
            {
                ChkBoxRows.Checked = false;
                if (amountBox != null) amountBox.Text = "";
                if (discountBox != null) discountBox.Text = "";
                if (aitBox != null) aitBox.Text = "";
                if (cbCash != null) cbCash.Checked = false;
                if (cbBank != null) cbBank.Checked = false;
                if (ddlBank != null) ddlBank.Items.Clear();
                if (ddlCash != null) ddlCash.Items.Clear();
            }
        }

        decimal disburseAmt = 0;
        if (decimal.TryParse(txtDisburseAmount.Text.Trim(), out disburseAmt) && disburseAmt > 0)
        {
            ApplyDisbursement();
        }
        else
        {
            CalculateTotal();
        }
    }

    public bool Validation()
    {
        if (string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) || salesCenterDropDownList.SelectedValue == "0")
        {
            showMessageBox("Please select Sales Center!!");
            salesCenterDropDownList.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(hfCustomerSearchId.Value) && string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()))
        {
            showMessageBox("Please select Customer!!");
            txtCustomerSearch.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(paymentDateTextBox.Text))
        {
            showMessageBox("Please select Payment Date!!");
            return false;
        }

        int CK = 0;
        for (int j = 0; j < orderGridView.Rows.Count; j++)
        {
            CheckBox cbReject = (CheckBox)orderGridView.Rows[j].FindControl("chkSelect");
            if (cbReject.Checked)
            {
                CK = CK + 1;
            }
        }
        if (CK == 0)
        {
            showMessageBox("Please Select Invoice from List!!");
            return false;
        }
        int count = 0;
        if (orderGridView.Rows.Count > 0)
        {
            for (int i = 0; i < orderGridView.Rows.Count; i++)
            {
                if (((CheckBox)orderGridView.Rows[i].FindControl("chkSelect")).Checked)
                {
                    if (((TextBox)orderGridView.Rows[i].FindControl("payAmountTextBox")).Text == "")
                    {
                        showMessageBox("Please fill out Pay Amount !!");
                        return false;
                    }

                    DropDownList ddlBank = (DropDownList)orderGridView.Rows[i].FindControl("ddlBank");
                    DropDownList ddlCash = (DropDownList)orderGridView.Rows[i].FindControl("ddlCash");
                    CheckBox cbBank = (CheckBox)orderGridView.Rows[i].FindControl("isBank");
                    CheckBox cbCash = (CheckBox)orderGridView.Rows[i].FindControl("isCash");

                    bool isBankSelected = cbBank != null && cbBank.Checked && ddlBank != null && ddlBank.SelectedValue != "" && ddlBank.SelectedValue != "0";
                    bool isCashSelected = cbCash != null && cbCash.Checked && ddlCash != null && ddlCash.SelectedValue != "" && ddlCash.SelectedValue != "0";

                    if (!isBankSelected && !isCashSelected)
                    {
                        showMessageBox("Please select Bank or Cash account for checked invoice at row " + (i + 1) + ".");
                        return false;
                    }

                    count++;
                }
            }
        }

        return true;
    }
    List<string> custPayIdList = new List<string>();
    protected void saveButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            CustomerMaster aCustomerMaster;

            bool save = false;

            List<CustPaymentDetail> aCustPaymentDetails = new List<CustPaymentDetail>();

            for (int i = 0; i < orderGridView.Rows.Count; i++)
            {
                CheckBox ChkBoxRows = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");
                CheckBox chkAdjust = (CheckBox)orderGridView.Rows[i].FindControl("chkAdjust");
                TextBox payAmountTextBox = (TextBox)orderGridView.Rows[i].FindControl("payAmountTextBox");
                TextBox aitTextBox = (TextBox)orderGridView.Rows[i].FindControl("aitTextBox");
                HiddenField hfCustomerMasterId = (HiddenField)orderGridView.Rows[i].FindControl("hfCustomerMasterId");

                HiddenField hfMarketId = (HiddenField)orderGridView.Rows[i].FindControl("hfMarketId");
                HiddenField hfTP_Pay = (HiddenField)orderGridView.Rows[i].FindControl("hfTP_Pay");
                HiddenField hfVat_Pay = (HiddenField)orderGridView.Rows[i].FindControl("hfVat_Pay");
                DropDownList ddlCollectionBy = (DropDownList)orderGridView.Rows[i].FindControl("ddlCollectionBy");

                HiddenField hfDistributionRouteId = (HiddenField)orderGridView.Rows[i].FindControl("hfDistributionRouteId");
                decimal prevamount = 0;
                decimal TotalDelivery = 0;
                Label lbl_PrvAmount = (Label)orderGridView.Rows[i].FindControl("lblPaymentAmount");
                Label lblTotalDelivery = (Label)orderGridView.Rows[i].FindControl("lblTotalDelivery");
                if (lbl_PrvAmount != null && lbl_PrvAmount.Text != "")
                {
                     prevamount = Convert.ToDecimal(lbl_PrvAmount.Text);
                }
                
                
                if (lblTotalDelivery != null && lblTotalDelivery.Text != "")
                {
                    TotalDelivery = Convert.ToDecimal(lblTotalDelivery.Text);
                }

                if (ChkBoxRows != null && ChkBoxRows.Checked)
                {
                    CustPayment aCustPayment = new CustPayment();

                    aCustPayment.CustomerMasterId = string.IsNullOrEmpty(hfCustomerMasterId.Value) ? 0 : Convert.ToInt32(hfCustomerMasterId.Value);
                    aCustPayment.MarketId = string.IsNullOrEmpty(hfMarketId.Value) ? 0 : Convert.ToInt32(hfMarketId.Value);
                    aCustPayment.DistributionRouteId = string.IsNullOrEmpty(hfDistributionRouteId.Value) ? 0 : Convert.ToInt32(hfDistributionRouteId.Value);
                    aCustPayment.ComUnitId = string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) ? 0 : Convert.ToInt32(salesCenterDropDownList.SelectedValue);
                 
                    DateTime pDate;
                    if (!string.IsNullOrEmpty(paymentDateTextBox.Text.Trim()) && DateTime.TryParse(paymentDateTextBox.Text.Trim(), out pDate))
                    {
                        aCustPayment.PaymentDate = pDate;
                    }
                    else
                    {
                        aCustPayment.PaymentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
                    }

                    aCustPayment.PaymentAmount = Convert.ToDecimal(payAmountTextBox.Text);
                    aCustPayment.PayType = payTypeDDL.SelectedItem.Text;

                    aCustPayment.RefNo = !string.IsNullOrEmpty(checkNoTextBox.Text.Trim())
                                         ? checkNoTextBox.Text.Trim()
                                         : refNameTextBox.Text.Trim();

                    if (!string.IsNullOrEmpty(checkDateTextBox.Text.Trim()))
                    {
                        DateTime cDate;
                        if (DateTime.TryParse(checkDateTextBox.Text.Trim(), out cDate))
                        {
                            aCustPayment.RefDate = cDate;
                        }
                    }
                    else if (refDtTextBox.Text != "")
                    {
                        aCustPayment.RefDate = Convert.ToDateTime(refDtTextBox.Text);
                    }

                    aCustPayment.Remarks = remarksTextBox.Text.Trim();

                      
                    DataTable aTable = aDal.CheckInvoiceCustpayment(aCustPayment.PaymentAmount, Convert.ToInt32(orderGridView.DataKeys[i][0].ToString()));

                    if (aTable.Rows.Count == 0)
                    {
                        string ptStatus = "";
                        aCustPayment.CreateBy = Session["LoginName"].ToString();
                        aCustPayment.CreateDate = DateTime.Now;


                        decimal _TP_Pay=0;
                        decimal _Vat_Pay = 0;
                        _TP_Pay = Convert.ToDecimal(hfTP_Pay.Value);
                        _Vat_Pay = Convert.ToDecimal(hfVat_Pay.Value);

                        decimal payAmount = Convert.ToDecimal(payAmountTextBox.Text);
                        decimal aitAmount = (aitTextBox != null && !string.IsNullOrEmpty(aitTextBox.Text)) ? Convert.ToDecimal(aitTextBox.Text) : 0;
                        TextBox discountTextBox = (TextBox)orderGridView.Rows[i].FindControl("discountTextBox");
                        decimal discountAmount = (discountTextBox != null && !string.IsNullOrEmpty(discountTextBox.Text)) ? Convert.ToDecimal(discountTextBox.Text) : 0;
                        decimal totalSettled = payAmount + aitAmount + discountAmount;

                        if ((totalSettled + prevamount) == TotalDelivery)
                        {
                            ptStatus = "Full";
                            save = aCustPaymentBll.UpdateInvoiceFinalPayment(Convert.ToInt32(orderGridView.DataKeys[i][0].ToString()), totalSettled, ptStatus, Session["LoginName"].ToString());
                        }
                        else
                        {
                            ptStatus = "Partial";
                            save = aCustPaymentBll.UpdateInvoiceFinalPayment(Convert.ToInt32(orderGridView.DataKeys[i][0].ToString()), totalSettled, ptStatus, Session["LoginName"].ToString());
                        }

                        CustPaymentDetail aCustPaymentDetail = new CustPaymentDetail();
                        aCustPaymentDetail.InvoiceId = Convert.ToInt32(orderGridView.DataKeys[i]["InvoiceId"].ToString());
                        aCustPaymentDetail.PaymentAmount = payAmount;
                        aCustPaymentDetail.TPAmount      = _TP_Pay;
                        aCustPaymentDetail.VATAmount     = _Vat_Pay;
                        aCustPaymentDetail.AIT           = aitAmount;
                        aCustPaymentDetail.Discount      = discountAmount;
                        aCustPaymentDetail.IsAdjust      = chkAdjust.Checked;

                        aCustPaymentDetail.DANameId = string.IsNullOrWhiteSpace(ddlDAName.SelectedValue)
                                                      ? 0
                                                      : Convert.ToInt32(ddlDAName.SelectedValue);

                        DropDownList ddlColBy = (DropDownList)orderGridView.Rows[i].FindControl("ddlCollectionBy");
                        aCustPaymentDetail.CollectionBy = (ddlColBy != null && !string.IsNullOrEmpty(ddlColBy.SelectedValue)) ? ddlColBy.SelectedValue : "DIC";

                        DropDownList ddlBankVal = (DropDownList)orderGridView.Rows[i].FindControl("ddlBank");
                        DropDownList ddlCashVal = (DropDownList)orderGridView.Rows[i].FindControl("ddlCash");
                        CheckBox cbBankVal = (CheckBox)orderGridView.Rows[i].FindControl("isBank");
                        CheckBox cbCashVal = (CheckBox)orderGridView.Rows[i].FindControl("isCash");

                        int bankAccIdVal = 0;
                        int cashAccIdVal = 0;

                        if (cbBankVal != null && cbBankVal.Checked && ddlBankVal != null && !string.IsNullOrEmpty(ddlBankVal.SelectedValue))
                        {
                            int.TryParse(ddlBankVal.SelectedValue, out bankAccIdVal);
                        }

                        if (cbCashVal != null && cbCashVal.Checked && ddlCashVal != null && !string.IsNullOrEmpty(ddlCashVal.SelectedValue))
                        {
                            int.TryParse(ddlCashVal.SelectedValue, out cashAccIdVal);
                        }

                        aCustPaymentDetail.BankAccId = bankAccIdVal;
                        aCustPaymentDetail.CashAccId = cashAccIdVal;

                        aCustPaymentDetails.Add(aCustPaymentDetail);

                    }
                    else
                    {
                        save = false;
                    }

                    if (save)
                    { 
                    if (aCustPaymentBll.SaveCustPayment(aCustPayment, aCustPaymentDetails))
                    {

                            custPayIdList.Add(aCustPayment.CustPayId.ToString());

                            foreach (var aDetail in aCustPaymentDetails)
                            {
                                aCustPaymentBll.PaymentJournalPosting(aCustPayment.CustPayId, aDetail.InvoiceId);
                            }

                        }

                    }
                }
            }

            if (save)
            {

                //MoneyRecitPrint();
                hfCustPayId.Value = string.Join(",", custPayIdList);
                //LoadGridView();
                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "ShowSuccesalert('" + "Operation successful!" + "','Success');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#moneyModal').modal('show');", true);


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Already Exist!" + "','Faild');", true);
            }


        }
    }


    protected void btnMoneyReceiptPrint_Click(object sender, EventArgs e)
    {
        MoneyRecitPrint(); // Print Receipt
        LoadGridView();    // Refresh grid
        Clear();
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseMoneyModal", "$('#moneyModal').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();", true);
    }

    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        LoadGridView();    // Just save/refresh
        Clear();
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseMoneyModal", "$('#moneyModal').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();", true);
    }

    protected void payAmountTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox qtyTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        PayAmountChange(rowindex);

        CalculateTotal();

    }

    protected void aitTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox aitTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)aitTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        PayAmountChange(rowindex);

        CalculateTotal();
    }

    protected void discountTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox discountTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)discountTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        PayAmountChange(rowindex);

        CalculateTotal();
    }

    private void PayAmountChange(int rowindex)
    {
        decimal prevamount = 0;
        TextBox payAmountTextBox = (TextBox)orderGridView.Rows[rowindex].FindControl("payAmountTextBox");
        TextBox discountTextBox = (TextBox)orderGridView.Rows[rowindex].FindControl("discountTextBox");
        TextBox aitTextBox = (TextBox)orderGridView.Rows[rowindex].FindControl("aitTextBox");
        Label lbl_PrvAmount = (Label)orderGridView.Rows[rowindex].FindControl("lblPaymentAmount");
        Label lblTotalDelivery = (Label)orderGridView.Rows[rowindex].FindControl("lblTotalDelivery");
        Label lblDue = (Label)orderGridView.Rows[rowindex].FindControl("lblDue");

        decimal payamount = string.IsNullOrEmpty(payAmountTextBox.Text) ? 0 : Convert.ToDecimal(payAmountTextBox.Text);
        decimal discountamount = (discountTextBox != null && !string.IsNullOrEmpty(discountTextBox.Text)) ? Convert.ToDecimal(discountTextBox.Text) : 0;
        decimal aitamount = (aitTextBox != null && !string.IsNullOrEmpty(aitTextBox.Text)) ? Convert.ToDecimal(aitTextBox.Text) : 0;
        decimal mainamount = payamount + discountamount + aitamount;
        decimal delamount = 0;
        decimal Dueamount = 0;


        HiddenField hfInvoiceId = (HiddenField)orderGridView.Rows[rowindex].FindControl("hfInvoiceId");
        HiddenField hfTP_Pay = (HiddenField)orderGridView.Rows[rowindex].FindControl("hfTP_Pay");
        HiddenField hfVat_Pay = (HiddenField)orderGridView.Rows[rowindex].FindControl("hfVat_Pay");


        DataTable aTable = aDal.LoadPaymentInvSPTPVATAmt(hfInvoiceId.Value, mainamount);

        if (aTable.Rows.Count > 0)
        {
            decimal _tpFinal = 0;
            decimal _VatFinal = 0;
            decimal _tpPay = Convert.ToDecimal(aTable.Rows[0]["TP_Pay"].ToString());
            decimal _vatPay = Convert.ToDecimal(aTable.Rows[0]["Vat_Pay"].ToString());

            if (_vatPay > 0)
            {
                _VatFinal = _vatPay - mainamount;

                if (_VatFinal > 0)
                {
                    _VatFinal = mainamount;
                    _tpFinal = 0;
                }
                else
                {
                    _VatFinal = _vatPay;
                    _tpFinal = mainamount - _vatPay;
                }

            }

            if (_vatPay == 0)
            {
                if (_tpPay > 0)
                {
                    _tpFinal = mainamount;
                    _VatFinal = 0;
                }
                else
                {
                    _VatFinal = 0;
                    _tpFinal = mainamount - _vatPay;
                }
            }

            hfTP_Pay.Value = _tpFinal.ToString();
            hfVat_Pay.Value = _VatFinal.ToString();
        }



        prevamount = Convert.ToDecimal(lbl_PrvAmount.Text);
        delamount = Convert.ToDecimal(lblTotalDelivery.Text);
        Dueamount = Convert.ToDecimal(lblDue.Text);



        if ((mainamount + prevamount) > delamount)
        {
            payAmountTextBox.Text = "0";
            if (discountTextBox != null) discountTextBox.Text = "0.00";
            if (aitTextBox != null) aitTextBox.Text = "0.00";
            payAmountTextBox.Focus();
            showMessageBox("Cannot Be Greater then Invoice Quantity ");

        }
    }

    public void CalculateTotal()
    {
        decimal prevamount = 0;

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");
            TextBox payAmountTextBox = (TextBox)orderGridView.Rows[i].FindControl("payAmountTextBox");

            if (chkBoxRows != null && chkBoxRows.Checked && payAmountTextBox != null)
            {
                if (payAmountTextBox.Text.Trim() != "")
                {
                    if (payAmountTextBox.Text != "0")
                    {
                        prevamount = prevamount + Convert.ToDecimal(payAmountTextBox.Text.Trim());
                    }
                }
            }
        }

        lblCount.Text = "Total Pay Amount : " + prevamount.ToString(CultureInfo.InvariantCulture);
    }

    protected void chkAdjust_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox isAdjust = (CheckBox)sender;
        GridViewRow currentRow = (GridViewRow)isAdjust.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox payAmountTextBox = (TextBox)orderGridView.Rows[rowindex].FindControl("payAmountTextBox");

        if (isAdjust.Checked)
        {
            payAmountTextBox.Text = orderGridView.Rows[rowindex].Cells[orderGridView.Columns.Count - 1].Text.Trim();
        }
        else
        {
            payAmountTextBox.Text = "";
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerPayment.aspx");
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
       lblCount.Text = "Total Pay Amount: 0";
        LoadGridView();

    }


    private string Parm()
    {
        StringBuilder sb = new StringBuilder();

        if (!string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue))
        {
            sb.Append(" AND INV.ComUnitId = '").Append(salesCenterDropDownList.SelectedValue).Append("' ");
        }

        if (!string.IsNullOrEmpty(hfCustPayId.Value))
        {
            sb.Append(" AND custPay.CustPayId IN (").Append(hfCustPayId.Value).Append(") ");
        }

        sb.Append(" AND CONVERT(date, custPay.custPaymentDate) = CONVERT(date, GETDATE()) ");

        return sb.ToString();
    }

    private string GenerateParameter()
    {
        StringBuilder param = new StringBuilder();
        List<int> invIds = new List<int>();

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            var chkSelect = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");
            var hfInvoiceId = (HiddenField)orderGridView.Rows[i].FindControl("hfInvoiceId");

            if (chkSelect != null && hfInvoiceId != null && chkSelect.Checked)
            {
                int invId = 0;
                if (int.TryParse(hfInvoiceId.Value, out invId) && invId > 0)
                {
                    invIds.Add(invId);
                }
            }
        }

        if (invIds.Count > 0)
        {
            param.Append(" AND INV.InvoiceId IN (").Append(string.Join(",", invIds)).Append(") ");
        }

        param.Append(Parm());

        return param.ToString();
    }

    private void MoneyRecitPrint()
    {
        Session["InvoiceIds"] = "";
        string invoiceIds = GenerateParameter();
        if (string.IsNullOrEmpty(invoiceIds))
        {
            // Validation Message show code (optional)
            // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('Please select at least one invoice.','Faild');", true);
            return;
        }

        Session["InvoiceIds"] = invoiceIds;
       // LoadGridView();

        // First hide the modal (if open)
       
        string url = "../SInventory_RPTVIEW/MoneyReceiptViewerForAfterPayment.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" +
                         url +
                         "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top='+Mtop+', left='+Mleft );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    private void LoadGridView()
    {
        orderGridView.DataSource = null;
        orderGridView.DataBind();

        if (string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) || salesCenterDropDownList.SelectedValue == "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('Please select Sales Center!','Faild');", true);
            salesCenterDropDownList.Focus();
            return;
        }

        if (string.IsNullOrEmpty(hfCustomerSearchId.Value) && string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('Please select Customer!','Faild');", true);
            txtCustomerSearch.Focus();
            return;
        }

        Session["SalesCenter"] = salesCenterDropDownList.SelectedValue;
        Session["Route"] = 0;
        Session["Territory"] = 0;
        lblCount.Text = "Total Pay Amount: 0";

        DataTable aTable = aDal.LoadPaymentInvSP(GenerateParam());

        if (aTable != null && aTable.Rows.Count > 0)
        {
            DataView dv = aTable.DefaultView;
            dv.Sort = "InvoiceDate ASC, InvoiceId ASC";
            aTable = dv.ToTable();
        }

        orderGridView.DataSource = aTable;
        orderGridView.DataBind();
        CollectionByChange();
    }
    public string GenerateParam()
    {
        string param = "";

        // Territory
        if (!string.IsNullOrEmpty(ddlTerritoryName.SelectedValue) &&
            ddlTerritoryName.SelectedValue != "0")
        {
            param += " AND ord.TerritoryId = '" + ddlTerritoryName.SelectedValue + "'";
        }

        // Sales Center
        if (!string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) &&
            salesCenterDropDownList.SelectedValue != "0")
        {
            param += " AND ord.ComUnitId = '" + salesCenterDropDownList.SelectedValue + "'";
        }

        // Route
        if (!string.IsNullOrEmpty(rootDropDownList.SelectedValue) &&
            rootDropDownList.SelectedValue != "0")
        {
            param += " AND ord.DistributionRouteId = '" + rootDropDownList.SelectedValue + "'";
        }

        // Customer
        if (!string.IsNullOrEmpty(hfCustomerSearchId.Value))
        {
            param += " AND ord.CustomerMasterId = '" + hfCustomerSearchId.Value + "'";
        }
        else if (!string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()))
        {
            string custCode = txtCustomerSearch.Text.Trim();
            if (custCode.Contains(":"))
            {
                custCode = custCode.Split(':')[0].Trim();
            }
            param += " AND (ord.CustomerCode = '" + custCode.Replace("'", "''") + "' OR ord.CustomerName LIKE '%" + custCode.Replace("'", "''") + "%')";
        }

        return param;
    }
    protected void Unnamed_Click(object sender, EventArgs e)
    {

    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox qtyTextBox = (CheckBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        CheckBox cb = (CheckBox)orderGridView.Rows[rowindex].FindControl("chkSelect");
        TextBox amountBox = (TextBox)orderGridView.Rows[rowindex].FindControl("payAmountTextBox");
        DropDownList ddlBank = (DropDownList)orderGridView.Rows[rowindex].FindControl("ddlBank");
        DropDownList ddlCash = (DropDownList)orderGridView.Rows[rowindex].FindControl("ddlCash");
        CheckBox cbBank = (CheckBox)orderGridView.Rows[rowindex].FindControl("isBank");
        CheckBox cbCash = (CheckBox)orderGridView.Rows[rowindex].FindControl("isCash");

        var paymentTypeName = orderGridView.DataKeys[rowindex]["PaymentTypeName"].ToString();

        TextBox discountBox = (TextBox)orderGridView.Rows[rowindex].FindControl("discountTextBox");
        TextBox aitBox = (TextBox)orderGridView.Rows[rowindex].FindControl("aitTextBox");

        if (cb != null && cb.Checked)
        {
            Label lblDue = (Label)orderGridView.Rows[rowindex].FindControl("lblDue");
            if (amountBox != null && lblDue != null)
            {
                amountBox.Text = lblDue.Text;
            }
            if (discountBox != null)
            {
                discountBox.Text = "0.00";
            }
            if (aitBox != null)
            {
                aitBox.Text = "0.00";
            }

            if (paymentTypeName.Trim() == "Cash")
            {
                if (cbCash != null) cbCash.Checked = true;
                if (cbBank != null) cbBank.Checked = false;
                if (ddlBank != null) ddlBank.Items.Clear();
                SetBankOrCashAccount(rowindex, "Cash");
            }
            else
            {
                if (cbCash != null) cbCash.Checked = false;
                if (cbBank != null) cbBank.Checked = true;
                if (ddlCash != null) ddlCash.Items.Clear();
                SetBankOrCashAccount(rowindex, "Bank");
            }
        }
        else
        {
            if (amountBox != null) amountBox.Text = "";
            if (discountBox != null) discountBox.Text = "";
            if (aitBox != null) aitBox.Text = "";
            if (cbCash != null) cbCash.Checked = false;
            if (cbBank != null) cbBank.Checked = false;
            if (ddlBank != null) ddlBank.Items.Clear();
            if (ddlCash != null) ddlCash.Items.Clear();
        }

        PayAmountChange(rowindex);

        decimal disburseAmt = 0;
        if (decimal.TryParse(txtDisburseAmount.Text.Trim(), out disburseAmt) && disburseAmt > 0)
        {
            ApplyDisbursement();
        }
        else
        {
            CalculateTotal();
        }
    }

    protected void btnDisburse_Click(object sender, EventArgs e)
    {
        ApplyDisbursement();
    }

    private void ApplyDisbursement()
    {
        decimal totalDisburse = 0;
        if (!decimal.TryParse(txtDisburseAmount.Text.Trim(), out totalDisburse) || totalDisburse < 0)
        {
            return;
        }

        decimal remaining = totalDisburse;

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");
            TextBox amountBox = (TextBox)orderGridView.Rows[i].FindControl("payAmountTextBox");
            Label lblDue = (Label)orderGridView.Rows[i].FindControl("lblDue");

            if (chk != null && chk.Checked && amountBox != null && lblDue != null)
            {
                decimal due = 0;
                decimal.TryParse(lblDue.Text.Trim(), out due);

                if (remaining > 0)
                {
                    if (remaining >= due)
                    {
                        amountBox.Text = due.ToString("0.00");
                        remaining -= due;
                    }
                    else
                    {
                        amountBox.Text = remaining.ToString("0.00");
                        remaining = 0;
                    }
                }
                else
                {
                    amountBox.Text = "0.00";
                }

                PayAmountChange(i);
            }
            else if (amountBox != null && (chk == null || !chk.Checked))
            {
                amountBox.Text = "0.00";
            }
        }

        CalculateTotal();
    }

    protected void rootDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDAName.Items.Clear();
        try
        {
                  RouteInformationDAL _DalRoute = new RouteInformationDAL();
            using (DataTable dt = _DalRoute.GeteRouteInformationDA_DDLId(rootDropDownList.SelectedValue))
            {
                ddlDAName.DataSource = dt;
                ddlDAName.DataValueField = "DANameId";
                ddlDAName.DataTextField = "DAName";
                ddlDAName.DataBind();
                ddlDAName.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlDAName.SelectedIndex = 0;
            }
        }
        catch (Exception ex) { }


        try
        {
            RouteInformationDAL _DalRoute = new RouteInformationDAL();
            using (DataTable dt = _DalRoute.GetforPaymentTerritoryByRouteInformationDA_DDLId(rootDropDownList.SelectedValue))
            {
                ddlTerritoryName.DataSource = dt;
                ddlTerritoryName.DataValueField = "TerritoryId";
                ddlTerritoryName.DataTextField = "TerritoryName";
                ddlTerritoryName.DataBind();
                ddlTerritoryName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select From List", String.Empty));
                ddlTerritoryName.SelectedIndex = 0;
            }
        }
        catch (Exception ex) { }
        //try
        //{
        //          RouteInformationDAL _DalRoute = new RouteInformationDAL();
        //    using (DataTable dt = _DalRoute.GeteRouteInformationDA_DDLId(rootDropDownList.SelectedValue))
        //    {
        //        ddlDAName.DataSource = dt;
        //        ddlDAName.DataValueField = "DANameId";
        //        ddlDAName.DataTextField = "DAName";
        //        ddlDAName.DataBind();
        //        ddlDAName.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
        //        ddlDAName.SelectedIndex = 0;
        //    }
        //}
        //catch (Exception ex) { }
    }

    private string GetDefaultCollectionBy()
    {
        string roleTypeName = Session["RoleTypeName"] != null ? Session["RoleTypeName"].ToString().Trim() : "";
        string roleTypeId = Session["RoleTypeId"] != null ? Session["RoleTypeId"].ToString().Trim() : "";
        string userType = Session["UserType"] != null ? Session["UserType"].ToString().Trim() : "";
        string desigName = Session["DesigName"] != null ? Session["DesigName"].ToString().Trim() : "";

        // 1. Check for X. MIO
        if (roleTypeName.Equals("X. MIO", StringComparison.OrdinalIgnoreCase) ||
            desigName.Equals("X. MIO", StringComparison.OrdinalIgnoreCase) ||
            userType.Equals("X. MIO", StringComparison.OrdinalIgnoreCase))
        {
            return "X. MIO";
        }

        // 2. Check for MIO
        if (roleTypeId == "1" ||
            roleTypeName.IndexOf("MIO", StringComparison.OrdinalIgnoreCase) >= 0 ||
            desigName.IndexOf("MIO", StringComparison.OrdinalIgnoreCase) >= 0 ||
            userType.IndexOf("MIO", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return "MIO";
        }

        // 3. Check for DIC
        if (roleTypeId == "8" || roleTypeId == "13" ||
            roleTypeName.IndexOf("DIC", StringComparison.OrdinalIgnoreCase) >= 0 ||
            desigName.IndexOf("DIC", StringComparison.OrdinalIgnoreCase) >= 0 ||
            userType.IndexOf("DIC", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return "DIC";
        }

        // Fallback: check if ddlDAName has an explicit selection
        if (ddlDAName != null && ddlDAName.SelectedItem != null && !string.IsNullOrEmpty(ddlDAName.SelectedItem.Text))
        {
            if (ddlDAName.SelectedItem.Text.IndexOf("MIO", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ddlDAName.SelectedItem.Text.Trim();
            }
        }

        return "DIC";
    }

    protected void orderGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlCollectionBy = (DropDownList)e.Row.FindControl("ddlCollectionBy");
            if (ddlCollectionBy != null)
            {
                string defColBy = GetDefaultCollectionBy();
                ListItem item = ddlCollectionBy.Items.FindByValue(defColBy);
                if (item != null)
                {
                    ddlCollectionBy.ClearSelection();
                    item.Selected = true;
                }
            }
        }
    }

    protected void ddlDAName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CollectionByChange();
    }

    private void CollectionByChange()
    {
        string defaultCollectionBy = GetDefaultCollectionBy();

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            DropDownList ddlCollectionBy = (DropDownList)orderGridView.Rows[i].FindControl("ddlCollectionBy");
            if (ddlCollectionBy != null)
            {
                ListItem item = ddlCollectionBy.Items.FindByValue(defaultCollectionBy);
                if (item != null)
                {
                    ddlCollectionBy.ClearSelection();
                    item.Selected = true;
                }
            }
        }
    }

    protected void isBank_CheckedChanged(object sender, EventArgs e)
    {
        int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
        CheckBox cb = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("chkSelect");

        if (cb != null && cb.Checked)
        {
            SetBankOrCashAccount(selRowIndex, "Bank");
        }
        else
        {
            CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("isBank");
            CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("isCash");

            DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].FindControl("ddlBank");
            DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].FindControl("ddlCash");

            if (cbBank != null) cbBank.Checked = false;
            if (ddlBank != null) ddlBank.Items.Clear();

            if (cbCash != null) cbCash.Checked = false;
            if (ddlCash != null) ddlCash.Items.Clear();

            showMessageBox("You should select row item first !!");
        }
    }

    protected void isCash_CheckedChanged(object sender, EventArgs e)
    {
        int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
        CheckBox cb = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("chkSelect");

        if (cb != null && cb.Checked)
        {
            SetBankOrCashAccount(selRowIndex, "Cash");
        }
        else
        {
            CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("isBank");
            CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("isCash");

            DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].FindControl("ddlBank");
            DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].FindControl("ddlCash");

            if (cbBank != null) cbBank.Checked = false;
            if (ddlBank != null) ddlBank.Items.Clear();

            if (cbCash != null) cbCash.Checked = false;
            if (ddlCash != null) ddlCash.Items.Clear();

            showMessageBox("You should select row item first !!");
        }
    }

    private void SetBankOrCashAccount(int selRowIndex, string status)
    {
        CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("isBank");
        CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].FindControl("isCash");

        DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].FindControl("ddlBank");
        DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].FindControl("ddlCash");

        string companyId = orderGridView.DataKeys[selRowIndex]["CompanyId"].ToString();

        if (status == "Cash")
        {
            if (cbBank != null) cbBank.Checked = false;
            if (ddlBank != null) ddlBank.Items.Clear();
        }

        if (status == "Bank")
        {
            if (cbCash != null) cbCash.Checked = false;
            if (ddlCash != null) ddlCash.Items.Clear();
        }

        if (cbBank != null && cbBank.Checked && ddlBank != null)
        {
            aCustPaymentBll.LoadBankAccount(ddlBank, companyId);
        }

        if (cbCash != null && cbCash.Checked && ddlCash != null)
        {
            aCustPaymentBll.LoadCashAccount(ddlCash, companyId);
            if (ddlCash.Items.Count > 0)
            {
                ddlCash.SelectedIndex = 0;
            }
        }
    }
}