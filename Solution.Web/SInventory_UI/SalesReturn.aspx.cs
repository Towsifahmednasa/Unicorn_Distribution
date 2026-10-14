using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Bibliography;
using Library.BLL.SInventory_BLL;
using Library.DAL.MasterSetup_DAL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_SalesReturn : System.Web.UI.Page
{

    private RequisitionBLL aRequisitionBll = new RequisitionBLL();
    private ReturnInvoiceBLL aInvoiceBll = new ReturnInvoiceBLL();
    private OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
    private static BonusCampaignNewDAL _BonusCampaignNewDAL = new BonusCampaignNewDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDistributionCenter();
            LoadAllProduct();
            orderDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            orderNoTextBox.Text = "Return From Market";
            IssueSaveToken();
        }
    }

    private void LoadDistributionCenter()
    {
        salesCenterDropDownList.Items.Clear();
        string userId = Session["UserId"] != null ? Session["UserId"].ToString() : string.Empty;
        if (!string.IsNullOrEmpty(userId))
        {
            aOrderInfoBll.LoadSC(salesCenterDropDownList, userId);
        }
        if (salesCenterDropDownList.Items.Count <= 1)
        {
            salesCenterDropDownList.Items.Clear();
            aOrderInfoBll.LoadSC(salesCenterDropDownList);
        }

        // Set default selected value from Session if available
        if (Session["DICCompanyUnitId"] != null && salesCenterDropDownList.Items.FindByValue(Session["DICCompanyUnitId"].ToString()) != null)
        {
            salesCenterDropDownList.SelectedValue = Session["DICCompanyUnitId"].ToString();
        }
        else if (Session["ComUnitId"] != null && salesCenterDropDownList.Items.FindByValue(Session["ComUnitId"].ToString()) != null)
        {
            salesCenterDropDownList.SelectedValue = Session["ComUnitId"].ToString();
        }
        else if (Session["SalesCenter"] != null && salesCenterDropDownList.Items.FindByValue(Session["SalesCenter"].ToString()) != null)
        {
            salesCenterDropDownList.SelectedValue = Session["SalesCenter"].ToString();
        }
        else if (salesCenterDropDownList.Items.Count > 1 && salesCenterDropDownList.SelectedIndex <= 0)
        {
            salesCenterDropDownList.SelectedIndex = 1;
        }

        if (salesCenterDropDownList.SelectedIndex > 0)
        {
            hdComUnitId.Value = salesCenterDropDownList.SelectedValue;
            comUnitNameTextBox.Text = salesCenterDropDownList.SelectedItem.Text;
            Session["DICCompanyUnitId"] = salesCenterDropDownList.SelectedValue;
        }
    }

    protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (salesCenterDropDownList.SelectedIndex > 0 && !string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) && salesCenterDropDownList.SelectedValue != "0")
        {
            hdComUnitId.Value = salesCenterDropDownList.SelectedValue;
            comUnitNameTextBox.Text = salesCenterDropDownList.SelectedItem.Text;
            Session["DICCompanyUnitId"] = salesCenterDropDownList.SelectedValue;
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }


    protected void ListImageButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalesReturnList.aspx");
    }
    private void LoadAllProduct()
    {
        DataTable aTable = aInvoiceBll.GetRectinProduct();

        if (aTable.Rows.Count > 0)
        {
            productGridView.DataSource = aTable;
            productGridView.DataBind();
        }
        else
        {
            productGridView.DataSource = null;
            productGridView.DataBind();
        }

        //aRequisitionBll.LoadAllInvoice(ddlInvoice);
    }
    protected void productCodeTextBox_TextChanged(object sender, EventArgs e)
    {
      //  ProformaOrInvoiceReturnBLL aInvoiceReturnBll = new ProformaOrInvoiceReturnBLL();
      ////  if (ProformaCheckBox.Checked)
      //  {
      //      bool chk = aInvoiceReturnBll.chkProformaOther(ddlInvoice.Text.Trim());
      //      if (chk)
      //      {
      //      }
      //      else
      //      {
      //          showMessageBox("No Proforma Found !!");
      //          //ddlInvoice.Text = string.Empty;
      //      }
      //  }
       
        //DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();

        //TextBox TextBox = (TextBox)sender;
        //GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        //int rowindex = 0;
        //rowindex = currentRow.RowIndex;
        //TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
        //string productName = productCodeTextBox.Text.Trim();
        //if (productName.Contains(':'))
        //{
           // string[] productInfo = productName.Split(':');

            //TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");

           // productCodeTextBox.Text = productInfo[0];
            //productNameTextBox.Text = productInfo[1];
          //  string productCode = ddlInvoice.Text.Trim();
           // GetProductInGrid(rowindex, productCode);
        //}
        //else
        //{
        //    showMessageBox("Input Correct Data!!");
        //    ((TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox")).Text = string.Empty;
        //}


        //string productCode = productCodeTextBox.Text.Trim();
        //GetProductInGrid(rowindex, productCode);

    }

    

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var chkBoxHeader = (CheckBox)productGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)productGridView.Rows[i].Cells[0].FindControl("chkSelect");
            
            if (chkBoxHeader.Checked == true)
            {
                chkBoxRows.Checked = true;
            }
            else
            {
                chkBoxRows.Checked = false;
            }
        }
    }

    protected void custNameTextBox_TextChanged(object sender, EventArgs e)
    {

        string custName = custNameTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(custName))
        {
            if (custName.Contains(':'))
            {

                string[] custInfo = custName.Split(':');
                custCodeTextBox.Text = custInfo[0].Trim();
                string custCode = custCodeTextBox.Text.Trim();
                GetCustInfo(custCode);
            }
            else
            {
                GetCustInfo(custName);
            }
        }
    }

    private void GetCustInfo(string custCode)
    {
        if (!string.IsNullOrEmpty(custCode))
        {
            custCodeTextBox.Text = custCode;
            DataTable aDataTable = new DataTable();
            aDataTable = aInvoiceBll.CustomerMaster(custCode);
            if (aDataTable.Rows.Count > 0)
            {
                string custComUnitId = aDataTable.Rows[0]["ComUnitId"].ToString();
                
                // If no DC selected in dropdown or currently 0, sync from customer
                if ((salesCenterDropDownList.SelectedIndex <= 0 || string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) || salesCenterDropDownList.SelectedValue == "0") && !string.IsNullOrEmpty(custComUnitId))
                {
                    if (salesCenterDropDownList.Items.FindByValue(custComUnitId) != null)
                    {
                        salesCenterDropDownList.SelectedValue = custComUnitId;
                    }
                }

                if (salesCenterDropDownList.SelectedIndex > 0 && !string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) && salesCenterDropDownList.SelectedValue != "0")
                {
                    hdComUnitId.Value = salesCenterDropDownList.SelectedValue;
                    comUnitNameTextBox.Text = salesCenterDropDownList.SelectedItem.Text;
                    Session["DICCompanyUnitId"] = salesCenterDropDownList.SelectedValue;
                }
                else
                {
                    hdComUnitId.Value = custComUnitId;
                    comUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitCode"].ToString() + ":" + aDataTable.Rows[0]["ComUnitName"].ToString();
                }

                int customerMasterId = 0;
                int.TryParse(aDataTable.Rows[0]["CustomerMasterId"].ToString(), out customerMasterId);
                hdCustomerMasterId.Value = customerMasterId.ToString();
                LoadCustomerInvoices(customerMasterId);
                custNameTextBox.Text = aDataTable.Rows[0]["CustomerName"].ToString();
                custAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                //districtNameTextBox.Text = aDataTable.Rows[0]["DistrictName"].ToString();
                areaNameTextBox.Text = aDataTable.Rows[0]["AreaName"].ToString();
                //miaCodeTextBox.Text = aDataTable.Rows[0]["MiaCode"].ToString();
                marketNameTextBox.Text = aDataTable.Rows[0]["MarketName"].ToString();
                lblMioInfo.Text = aDataTable.Rows[0]["EmpName"].ToString();
                hfMIOId.Value = aDataTable.Rows[0]["MioId"].ToString();
                hfMioEmpId.Value = aDataTable.Rows[0]["EmpInfoId"].ToString();
                hfTerriId.Value = aDataTable.Rows[0]["TerritoryId"].ToString();
                hfSapCode.Value = aDataTable.Rows[0]["SAP_MIOCode"].ToString();
                //miaNameTextBox.Text = aDataTable.Rows[0]["MiaName"].ToString();
                //custCategoryTextBox.Text = aDataTable.Rows[0]["CategoryName"].ToString();
            }
            else
            {
            }
        }
    }

    private void LoadCustomerInvoices(int customerMasterId)
    {
        ddlInvoiceNo.DataSource = aInvoiceBll.GetCustomerInvoices(customerMasterId);
        ddlInvoiceNo.DataTextField = "InvoiceNo";
        ddlInvoiceNo.DataValueField = "InvoiceId";
        ddlInvoiceNo.DataBind();
        ddlInvoiceNo.Items.Insert(0, new ListItem("-- Select Invoice --", "0"));
    }

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string custCode = custCodeTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(custCode))
        {
            if (custCode.Contains(':'))
            {
                string[] custInfo = custCode.Split(':');
                custCode = custInfo[0].Trim();
            }
            GetCustInfo(custCode);
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
        string sScript = String.Format("showpop6('{0}', 'Error');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (salesCenterDropDownList.SelectedIndex <= 0 || string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) || salesCenterDropDownList.SelectedValue == "0")
        {
            if (string.IsNullOrEmpty(hdComUnitId.Value) || hdComUnitId.Value == "0")
            {
                ShowMessageBox("Please Select Distribution Center (DC)!!");
                salesCenterDropDownList.Focus();
                return false;
            }
        }

        if (orderNoTextBox.Text == "")
        {
            ShowMessageBox("Please Input Order Number!!");
            orderNoTextBox.Focus();
            return false;
        }
        if (ddlInvoiceNo.SelectedValue == "" || ddlInvoiceNo.SelectedValue == "0")
        {
            ShowMessageBox("Please Select Invoice No!!");
            ddlInvoiceNo.Focus();
            return false;
        }
        if (orderDateTextBox.Text == "")
        {
            ShowMessageBox("Please Input Order Date!!");
            orderDateTextBox.Focus();

            return false;
        }

        //if (ddlInvoice.Text == "")
        //{
        //    ShowMessageBox("Please Select Invoice No!!");
        //    return false;
        //}

        if (custCodeTextBox.Text == "")
        {
            ShowMessageBox("Please Select Customer !!");
            custCodeTextBox.Focus();
            return false;
        }

        if (lblMioInfo.Text == "")
        {
            ShowMessageBox("MIO Name can not be empty !!");
            return false;
        }

        //for (int i = 0; i < gridLineItemGridView.Rows.Count - 1; i++)
        //{
        //    string a = ((TextBox)gridLineItemGridView.Rows[i].FindControl("expDateTextBox")).Text.Trim();
        //    string b = ((TextBox) gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Text.Trim();
        //    if (((TextBox)gridLineItemGridView.Rows[i].FindControl("expDateTextBox")).Text.Trim() == "" ||
        //        ((TextBox)gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Text.Trim() == "")
        //    {
        //        ShowMessageBox("Please Input Batch No and Expry Date!!!");
        //        return false;
        //    }
        //} 


        for (int i = 0; i < gridLineItemGridView.Rows.Count ; i++)
        {
            string a = ((TextBox)gridLineItemGridView.Rows[i].FindControl("expDateTextBox")).Text.Trim();
            string b = ((TextBox) gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Text.Trim();
            if (((TextBox)gridLineItemGridView.Rows[i].FindControl("expDateTextBox")).Text.Trim() == "")
            {
                ShowMessageBox("Please Input Expry Date!!!");
                ((TextBox)gridLineItemGridView.Rows[i].FindControl("expDateTextBox")).Focus();
                return false;
            }  
            
            
            if (
                ((TextBox)gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Text.Trim() == "")
            {


                ShowMessageBox("Please Input Batch No!!!");
                ((TextBox)gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Focus();

                return false;
            }
        }
        //for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        //{
        //    if (((Label)gridLineItemGridView.Rows[i].Cells[15].FindControl("statusLabel")).Text == "Reject" ||
        //        ((Label)gridLineItemGridView.Rows[i].Cells[15].FindControl("statusLabel")).Text == "Parial")
        //    {
        //        if (
        //            ((DropDownList)gridLineItemGridView.Rows[i].Cells[17].FindControl("reasonReturnDropDownList"))
        //                .SelectedIndex == 0)
        //        {
        //            showMessageBox("Please Select Reason !!");
        //            return false;
        //        }
        //    }
        //}
        return true;

    }

    // One-time save token: each rendered form carries a token that is consumed only after a
    // successful commit. A double-click / browser retry re-posts the same token, finds it
    // already consumed and is rejected without touching the database. ASP.NET's per-session
    // request serialization makes the check-then-consume race-free for same-session requests.
    private void IssueSaveToken()
    {
        string token = Guid.NewGuid().ToString("N");
        Session["SRTOKEN_" + token] = true;
        ViewState["SaveToken"] = token;
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation() == true)
        {
            string saveToken = ViewState["SaveToken"] as string;
            if (string.IsNullOrEmpty(saveToken) || Session["SRTOKEN_" + saveToken] == null)
            {
                showMessageBox("This sales return has already been submitted. Please start a new return.");
                return;
            }

            try
            {
                string generatedInvNo;
                var txOpt = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(5)
                };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    int invId = SaveInvoiceNew();
                    if (invId <= 0)
                        throw new Exception("Return invoice header save failed");

                    if (!SaveReturnAmount(invId))
                        throw new Exception("Return amount save failed");

                    if (!SaveInvoiceDetailNew(invId))
                        throw new Exception("Return invoice detail save failed");

                    // Auto post AR Reversal and COGS/Inventory Reversal vouchers.
                    // sp_SalesReturnPosting runs with SET NOCOUNT ON, so the DAL's rows-affected
                    // bool is meaningless; verify the posting outcome directly instead.
                    aInvoiceBll.PostSalesReturn(invId);
                    if (!aInvoiceBll.IsSalesReturnPosted(invId))
                        throw new Exception("Journal posting failed");

                    scope.Complete();
                }

                // Commit succeeded: consume this form's token and arm a fresh one.
                Session.Remove("SRTOKEN_" + saveToken);
                IssueSaveToken();

                generatedInvNo = invTextBox.Text.Trim();
                Clear(keepInvoiceNo: true);
                invTextBox.Text = generatedInvNo;

                showSuccessMessageBox("Return Invoice Generated and Accounts Vouchers Posted Successfully! Invoice No: " + generatedInvNo);
            }
            catch (Exception exception)
            {
                // Transaction rolled back; token stays valid so the user can retry.
                showMessageBox("Sales return failed: " + exception.Message);
            }
        }
    }

    private bool SaveInvoiceDetailNew(int invoiceId)
    {

        List<InvoiceDetail> aInvoiceDetailsList = new List<InvoiceDetail>();

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                GridViewRow row = gridLineItemGridView.Rows[i];
                TextBox codeTextBox = row.FindControl("codeTextBox") as TextBox;
                TextBox nameTextBox = row.FindControl("nameTextBox") as TextBox;
                TextBox unitPriceTextBox = row.FindControl("unitPriceTextBox") as TextBox;
                TextBox upVatTextBox = row.FindControl("upVatTextBox") as TextBox;
                TextBox qtyTextBox = row.FindControl("qtyTextBox") as TextBox;
                TextBox tpTextBox = row.FindControl("tpTextBox") as TextBox;
                TextBox dpTextBox = row.FindControl("dpTextBox") as TextBox;
                TextBox dpAmtTextBox = row.FindControl("dpAmtTextBox") as TextBox;
                TextBox tpVatTextBox = row.FindControl("tpVatTextBox") as TextBox;
                TextBox npTextBox = row.FindControl("npTextBox") as TextBox;
                TextBox bQtyTextBox = row.FindControl("bQtyTextBox") as TextBox;
                TextBox tQtyTextBox = row.FindControl("tQtyTextBox") as TextBox;
                TextBox mfgDateTextBox = row.FindControl("mfgDateTextBox") as TextBox;
                TextBox expDateTextBox = row.FindControl("expDateTextBox") as TextBox;
                TextBox batchNoTextBox = row.FindControl("batchNoTextBox") as TextBox;
                TextBox cogsTextBox = row.FindControl("cogsTextBox") as TextBox;
                HiddenField orderdetailIdHiddenField = row.FindControl("orderdetailIdHiddenField") as HiddenField;

                string prodCode = codeTextBox != null ? codeTextBox.Text.Trim() : string.Empty;
                if (string.IsNullOrEmpty(prodCode)) continue;

                InvoiceDetail aInvoiceDetail = new InvoiceDetail();
                aInvoiceDetail.ProductCode = prodCode;

                string product = nameTextBox != null ? nameTextBox.Text : string.Empty;
                string[] proNameAndPackSize = product.Split(':');
                aInvoiceDetail.ProductName = proNameAndPackSize[0].Trim();

                DateTime expDate = DateTime.Today;
                if (expDateTextBox != null && !string.IsNullOrEmpty(expDateTextBox.Text.Trim()))
                {
                    DateTime.TryParse(expDateTextBox.Text.Trim(), out expDate);
                }
                aInvoiceDetail.ExpDate = expDate;
                aInvoiceDetail.ReceiveDate = DateTime.Today;

                decimal uPrice = 0, uVat = 0, qty = 0, dp = 0, dpAmt = 0, bQty = 0, tp = 0, tQty = 0, tpVat = 0, np = 0, cogs = 0;
                if (unitPriceTextBox != null) decimal.TryParse(unitPriceTextBox.Text.Trim(), out uPrice);
                if (cogsTextBox != null) decimal.TryParse(cogsTextBox.Text.Trim(), out cogs);
                if (upVatTextBox != null) decimal.TryParse(upVatTextBox.Text.Trim(), out uVat);
                if (qtyTextBox != null) decimal.TryParse(qtyTextBox.Text.Trim(), out qty);
                if (dpTextBox != null) decimal.TryParse(dpTextBox.Text.Trim(), out dp);
                if (dpAmtTextBox != null) decimal.TryParse(dpAmtTextBox.Text.Trim(), out dpAmt);
                if (bQtyTextBox != null) decimal.TryParse(bQtyTextBox.Text.Trim(), out bQty);
                if (tpTextBox != null) decimal.TryParse(tpTextBox.Text.Trim(), out tp);
                if (tQtyTextBox != null) decimal.TryParse(tQtyTextBox.Text.Trim(), out tQty);
                if (tpVatTextBox != null) decimal.TryParse(tpVatTextBox.Text.Trim(), out tpVat);
                if (npTextBox != null) decimal.TryParse(npTextBox.Text.Trim(), out np);

                aInvoiceDetail.UnitPrice = uPrice;
                aInvoiceDetail.CostPrice = cogs;
                aInvoiceDetail.UnitVatAmount = uVat;
                aInvoiceDetail.Quantity = qty;
                aInvoiceDetail.DiscountPercentage = dp;
                aInvoiceDetail.DiscountAmount = dpAmt;
                aInvoiceDetail.BonusQuantity = bQty;
                aInvoiceDetail.TotalPrice = tp;
                aInvoiceDetail.TotalQuantity = tQty > 0 ? tQty : qty;
                aInvoiceDetail.SpecialAmount = 0;
                aInvoiceDetail.TotalPriceVatAmount = tpVat;
                aInvoiceDetail.NetAmount = np > 0 ? np : (tp + tpVat);

                int orderDetailId = 0;
                if (orderdetailIdHiddenField != null) int.TryParse(orderdetailIdHiddenField.Value, out orderDetailId);
                aInvoiceDetail.OrderDetailsId = orderDetailId;
                aInvoiceDetail.InvoiceId = invoiceId;
                aInvoiceDetail.SpecialAmountPer = 0;

                aInvoiceDetail.DCStoreId = 0;
                aInvoiceDetail.PackSize = mfgDateTextBox != null ? mfgDateTextBox.Text.Trim() : "";
                aInvoiceDetail.BatchNo = batchNoTextBox != null ? batchNoTextBox.Text.Trim() : "";
                DateTime? expDateVal = null;
                DateTime parsedExp;
                if (expDateTextBox != null && DateTime.TryParse(expDateTextBox.Text.Trim(), out parsedExp))
                {
                    expDateVal = parsedExp;
                }

                DateTime? mfgDateVal = null;
                DateTime parsedMfg;
                if (mfgDateTextBox != null && DateTime.TryParse(mfgDateTextBox.Text.Trim(), out parsedMfg))
                {
                    mfgDateVal = parsedMfg;
                }

                string batchVal = batchNoTextBox != null ? batchNoTextBox.Text.Trim() : string.Empty;

                int comUnitId = 0;
                if (salesCenterDropDownList.SelectedIndex > 0 && !string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) && salesCenterDropDownList.SelectedValue != "0")
                {
                    int.TryParse(salesCenterDropDownList.SelectedValue, out comUnitId);
                }
                else if (Session["DICCompanyUnitId"] != null && Session["DICCompanyUnitId"].ToString() != "" && Session["DICCompanyUnitId"].ToString() != "0")
                {
                    int.TryParse(Session["DICCompanyUnitId"].ToString(), out comUnitId);
                }
                else if (!string.IsNullOrEmpty(hdComUnitId.Value) && hdComUnitId.Value != "0")
                {
                    int.TryParse(hdComUnitId.Value, out comUnitId);
                }

                string retInvNo = invTextBox.Text.Trim();

                aInvoiceDetailsList.Add(aInvoiceDetail);
                int detailid = aInvoiceBll.SaveReturnInvoice(aInvoiceDetail);
                if (detailid <= 0)
                    throw new Exception("Detail save failed for product " + prodCode);

                // Automatically receive return stock into tblDCStore for the active depot with manual COGS and detail ID
                decimal retQty = aInvoiceDetail.TotalQuantity > 0 ? aInvoiceDetail.TotalQuantity : aInvoiceDetail.Quantity;
                if (retQty > 0)
                {
                    if (comUnitId <= 0)
                        throw new Exception("No Distribution Center resolved for return stock of product " + prodCode);
                    if (!aInvoiceBll.InsertDCStoreStockSalesReturn(prodCode, aInvoiceDetail.ProductName, batchVal, expDateVal, mfgDateVal, retQty, comUnitId, retInvNo, cogs, detailid))
                        throw new Exception("Return stock receive failed for product " + prodCode);
                }
            }
        }

        // No detail row saved means an empty/invalid grid; never leave a header-only return behind.
        return aInvoiceDetailsList.Count > 0;
    }


    public void SaveDCStoreFreeze(int detail, int i)
    {
        //DataTable dtdcinfo = aInvoiceBll.DCInfoWithDCId(gridLineItemGridView.DataKeys[i]["DCStoreId"].ToString());

        DCStockNew aDcStockNew = new DCStockNew();
        {
            aDcStockNew.DCStoreId = Convert.ToInt32(0);
            aDcStockNew.InvoiceDetailId = Convert.ToInt32(0);
            //aDcStockNew.StorageLocation = dtdcinfo.Rows[0]["StorageLocation"].ToString();
            aDcStockNew.ProductCode = ((TextBox)gridLineItemGridView.Rows[i].FindControl("codeTextBox")).Text;
            aDcStockNew.ProductName = ((TextBox)gridLineItemGridView.Rows[i].FindControl("nameTextBox")).Text;
            //aDcStockNew.PackSize = ((TextBox)gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Text;
            aDcStockNew.ComUnitId = Convert.ToInt32(hdComUnitId.Value);
            aDcStockNew.BatchNo = ((TextBox)gridLineItemGridView.Rows[i].FindControl("batchNoTextBox")).Text;
            aDcStockNew.TotalQuantity =
                Convert.ToDecimal(
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text);
            aDcStockNew.ExpDate = Convert.ToDateTime(((TextBox)gridLineItemGridView.Rows[i].FindControl("expDateTextBox")).Text);
            aDcStockNew.ReceiveDate = DateTime.Today;
            aDcStockNew.ChalanNo = "NA";
            aDcStockNew.ChalanDate = DateTime.Today;
            //aDcStockNew.ComUnitId = Convert.ToInt32(dtdcinfo.Rows[0]["ComUnitId"].ToString());
            aDcStockNew.StockQty =
                Convert.ToDecimal(
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[15].FindControl("tQtyTextBox")).Text);
            ;
            aDcStockNew.DamageQty = 0;
            aDcStockNew.StockRcvDate = DateTime.Today;
            //
            aDcStockNew.ReqId = Convert.ToInt32(0);
            aDcStockNew.ReqChildId = Convert.ToInt32(0);
            aDcStockNew.StockInTransfarId = Convert.ToInt32(0);
            //
        };
        //if (((Label) gridLineItemGridView.Rows[i].Cells[15].FindControl("statusLabel")).Text =="Partial")
        {
            aInvoiceBll.SaveDCStoreFreeze(aDcStockNew, detail.ToString());
        }
    }

    private int SaveInvoiceNew()
    {

        int invoiceId = 0;
        DataTable aDataTable = new DataTable();
        aDataTable = aInvoiceBll.CustomerMaster(custCodeTextBox.Text);
        string invoiceNo = string.Empty;

        int comUnitId = 0;
        if (salesCenterDropDownList.SelectedIndex > 0 && !string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) && salesCenterDropDownList.SelectedValue != "0")
        {
            int.TryParse(salesCenterDropDownList.SelectedValue, out comUnitId);
        }
        else if (Session["DICCompanyUnitId"] != null && Session["DICCompanyUnitId"].ToString() != "" && Session["DICCompanyUnitId"].ToString() != "0")
        {
            int.TryParse(Session["DICCompanyUnitId"].ToString(), out comUnitId);
        }
        else if (!string.IsNullOrEmpty(hdComUnitId.Value) && hdComUnitId.Value != "0")
        {
            int.TryParse(hdComUnitId.Value, out comUnitId);
        }

        string ComUnitCode = string.Empty;
        if (salesCenterDropDownList.SelectedItem != null && salesCenterDropDownList.SelectedIndex > 0)
        {
            ComUnitCode = salesCenterDropDownList.SelectedItem.Text;
        }
        else if (!string.IsNullOrEmpty(comUnitNameTextBox.Text))
        {
            ComUnitCode = comUnitNameTextBox.Text;
        }

        SalesReturnDaoMas aInvoice = new SalesReturnDaoMas();
        // {
        aInvoice.InvoiceDate = Convert.ToDateTime(orderDateTextBox.Text.Trim());
        aInvoice.OrderNo = orderNoTextBox.Text.Trim();
        aInvoice.OrderDate = Convert.ToDateTime(orderDateTextBox.Text.Trim());
        aInvoice.CustomerMasterId = Convert.ToInt32(hdCustomerMasterId.Value);
        aInvoice.ComUnitId = comUnitId;
        aInvoice.MiaId = Convert.ToInt32(0);
        aInvoice.PaymentTypeId = Convert.ToInt32(1);

        decimal tpVal = 0, disVal = 0, vatVal = 0, grandVal = 0, pdVal = 0, dedVal = 0, netVal = 0;
        decimal.TryParse(tpTptalTextBox.Text.Trim(), out tpVal);
        decimal.TryParse(disTotalTextBox.Text.Trim(), out disVal);
        decimal.TryParse(vatTotalTextBox.Text.Trim(), out vatVal);
        decimal.TryParse(grandTotalTextBox.Text.Trim(), out grandVal);
        decimal.TryParse(pdTextBox.Text.Trim(), out pdVal);
        decimal.TryParse(txtDeductionCharge.Text.Trim(), out dedVal);
        decimal.TryParse(txtNetReturnAmount.Text.Trim(), out netVal);

        if (netVal <= 0 && grandVal > 0)
        {
            netVal = grandVal - dedVal;
            if (netVal < 0) netVal = 0;
        }

        aInvoice.TpTotal = tpVal;
        aInvoice.TpDiscount = disVal;
        aInvoice.TpVat = vatVal;
        aInvoice.TpGrandTotal = grandVal;
        aInvoice.DeductionCharge = dedVal;
        aInvoice.NetReturnAmount = netVal;
        aInvoice.UserId = Convert.ToInt32(Session["UserId"].ToString());
        aInvoice.ComUnitCode = ComUnitCode;
        int selectedInvoiceId = 0;
        int.TryParse(ddlInvoiceNo.SelectedValue, out selectedInvoiceId);
        aInvoice.InvoiceId = selectedInvoiceId;
        aInvoice.OrderId = Convert.ToInt32(0);
        aInvoice.OrderNo = "No order";
        aInvoice.Remarks = orderNoTextBox.Text.Trim();
        aInvoice.IsSalesReturnWithoutOrder = "Return From Market";

        aInvoice.MIACode = "";
        aInvoice.MIAName = "";
        aInvoice.MarketCode = "";
        aInvoice.MarketName = "";
        aInvoice.AreaCode = "";
        aInvoice.DisCode = "";
        aInvoice.FEName = "";
        aInvoice.RegionCode = "";
        aInvoice.DZSMName = "";
        ///

        aInvoice.FixedCustomer = true;//Convert.ToBoolean(aDataTable.Rows[0]["FixedCustomer"].ToString());
        aInvoice.Type = true; //Convert.ToString(aDataTable.Rows[0]["Type"].ToString());

        //aInvoice.DpNAme = deliverypersonNameTextBox.Text.Trim();
        aInvoice.DpMob = "";
        aInvoice.createBy = Session["LoginName"].ToString();
        aInvoice.Createdate = DateTime.Now;
        {
            aInvoice.ProductOffer = false;
        }
        aInvoice.Issubdeport = false;


        aInvoice.MIOId_new = string.IsNullOrEmpty(hfMIOId.Value) ? (int?)null : int.Parse(hfMIOId.Value);
        aInvoice.Terri_Id_new = string.IsNullOrEmpty(hfTerriId.Value) ? (int?)null : int.Parse(hfTerriId.Value);
        aInvoice.MioEmpId_new = string.IsNullOrEmpty(hfMioEmpId.Value) ? (int?)null : int.Parse(hfMioEmpId.Value);
        aInvoice.Mio_SapCode_New = hfSapCode.Value;
        ////////////////Product Multiple Offer End ///////////
        //if (CheckBox1.Checked)
        //{
        //    aInvoiceBll.SaveInvoice(aInvoice, out invoiceId, out invoiceNo, "0", invoiceHiddenField.Value);
        //}
        //else
        //{
        invoiceId = _BonusCampaignNewDAL.SaveReturnSalesMaster(aInvoice, Session["UserId"].ToString());

        //aInvoiceBll.SaveInvoice(aInvoice, out invoiceId, out invoiceNo, "0", "0");
        //}


        DataTable dtInvo = new DataTable();
        dtInvo = aInvoiceBll.getInvo(invoiceId.ToString());
        if (dtInvo.Rows.Count == 0)
            throw new Exception("Generated return invoice not found (Id " + invoiceId + ")");
        invTextBox.Text = dtInvo.Rows[0]["ReturnInvoiceNo"].ToString();

        return invoiceId;
    }
    protected void printButton_Click(object sender, EventArgs e)
    {
        string invNo = invTextBox.Text.Trim();
        if (string.IsNullOrEmpty(invNo))
        {
            showMessageBox("Please Enter or Generate a Return Invoice No to Print!");
            return;
        }

        string url = "../SInventory_RPTVIEW/ReturnInvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invNo);
        string fullURL = "window.open('" + url + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void addButton_Click(object sender, EventArgs e)
    {
        // 1. Validate if any product is selected with valid quantity
        int selectedCount = 0;
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)productGridView.Rows[i].FindControl("chkSelect");
            if (chkBoxRows != null && chkBoxRows.Checked)
            {
                TextBox qtyBox = (TextBox)productGridView.Rows[i].FindControl("transferQtyTextBox");
                decimal qty = 0;
                if (qtyBox == null || !decimal.TryParse(qtyBox.Text.Trim(), out qty) || qty <= 0)
                {
                    showMessageBox("Please enter a valid Return Quantity for selected products!");
                    return;
                }
                selectedCount++;
            }
        }

        if (selectedCount == 0)
        {
            showMessageBox("Please select at least one product to return!");
            return;
        }

        // 2. Clear existing line items
        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();

        // 3. Populate line items directly from productGridView
        int rowIndex = 0;
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)productGridView.Rows[i].FindControl("chkSelect");
            if (chkBoxRows != null && chkBoxRows.Checked)
            {
                TextBox qtyBox = (TextBox)productGridView.Rows[i].FindControl("transferQtyTextBox");
                decimal qty = 0;
                if (qtyBox != null) decimal.TryParse(qtyBox.Text.Trim(), out qty);

                string prodCode = HttpUtility.HtmlDecode(productGridView.Rows[i].Cells[1].Text.Trim());

                // Add a blank row to gridLineItemGridView
                AddFunc();

                // Populate product info and quantity
                GetProduct(rowIndex, prodCode);
                GetQty(qty, rowIndex);

                rowIndex++;
            }
        }

        // 4. Calculate Discounts and Totals
        decimal totalprice = 0;
        if (!string.IsNullOrEmpty(tpTptalTextBox.Text))
        {
            decimal.TryParse(tpTptalTextBox.Text, out totalprice);
        }

        decimal percentage = 0;
        try
        {
            if (!string.IsNullOrEmpty(custCodeTextBox.Text))
            {
                OrderInfoDAL OrderInfoDALs = new OrderInfoDAL();
                DataTable dtFixedCustomer = OrderInfoDALs.GetFixedCustomerffff(custCodeTextBox.Text.Trim());
                if (dtFixedCustomer != null && dtFixedCustomer.Rows.Count > 0 && Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]))
                {
                    percentage = 0;
                }
                else
                {
                    OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
                    DataTable dttradepolicy = aOrderInfoBll.GetParcentFromOrderDetailsMasterID(orderIdHiddenField.Value);
                    string PaymentType = "";
                    int CustTypeId = 0;
                    string SubmissionDate = "";
                    if (dttradepolicy != null && dttradepolicy.Rows.Count > 0)
                    {
                        PaymentType = dttradepolicy.Rows[0]["PaymentType"].ToString();
                        CustTypeId = Convert.ToInt32(dttradepolicy.Rows[0]["CustTypeId"].ToString());
                        SubmissionDate = dttradepolicy.Rows[0]["SubmissionDate"].ToString();
                    }
                    DataTable dttradepolicy22 = aOrderInfoBll.GetTradeTerm(totalprice.ToString(), CustTypeId, PaymentType, SubmissionDate);
                    if (dttradepolicy22 != null && dttradepolicy22.Rows.Count > 0)
                    {
                        percentage = Convert.ToDecimal(dttradepolicy22.Rows[0]["DiscountPerc"].ToString());
                    }
                }
            }
        }
        catch
        {
            percentage = 0;
        }

        decimal totaldiscount = (percentage * totalprice) / 100;
        disTotalTextBox.Text = totaldiscount.ToString("F2");

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            TextBox sdBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("sdTextBox");
            if (sdBox != null && string.IsNullOrEmpty(sdBox.Text.Trim())) sdBox.Text = "0.00";

            TextBox dpBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpTextBox");
            if (dpBox != null) dpBox.Text = percentage.ToString();

            TextBox dpAmtBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpAmtTextBox");
            TextBox tpBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox");
            if (dpAmtBox != null && tpBox != null)
            {
                decimal itemTp = 0;
                decimal.TryParse(tpBox.Text.Trim(), out itemTp);
                dpAmtBox.Text = ((itemTp * percentage) / 100).ToString("F2");
            }

            TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("npTextBox");
            TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("tpVatTextBox");
            if (npTextBox != null && tpBox != null && dpAmtBox != null && tpVatTextBox != null)
            {
                decimal itemTp = 0, itemDp = 0, itemVat = 0;
                decimal.TryParse(tpBox.Text.Trim(), out itemTp);
                decimal.TryParse(dpAmtBox.Text.Trim(), out itemDp);
                decimal.TryParse(tpVatTextBox.Text.Trim(), out itemVat);
                npTextBox.Text = ((itemTp - itemDp) + itemVat).ToString("F2");
            }
        }

        TotalValueCalculation();
        GetDiscounttotalValue();
    }

  


    protected void DeleteImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton productCodeTextBox = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)productCodeTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");

        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");
        aDataTable.Columns.Add("SpecialAmount");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");

        DataRow dataRow = null;
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            if (i != rowindex)
            {


                dataRow = aDataTable.NewRow();
                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value.Trim();
                dataRow["SpecialAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }

        }

        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();

    }
    public bool OrderExists(string orderId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderExistsBll(orderId);
        if (aTable.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Moticare()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        OrderInfoDAL sss = new OrderInfoDAL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = sss.GetFixedCustomerffff(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty = Convert.ToInt32(0);
                if (ProductCode == "FGD02" && Qty >= 4)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool Nervaid75mgCapsuleFOC()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        OrderInfoDAL sss = new OrderInfoDAL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = sss.GetFixedCustomerffff(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty = Convert.ToInt32(0);
                if (ProductCode == "AEL02" && Qty >= 3)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool FlexidolFOC()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty = Convert.ToInt32(0);
                if (ProductCode == "AIssD01" && Qty >= 422222)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool Ezepain()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty = Convert.ToInt32(0);
                if (ProductCode == "AID04" && Qty >= 5)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool MaxiventFOC()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty =
                    Convert.ToInt32(0);
                if (ProductCode == "OAD04" && Qty >= 2)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool SeacoralDTablet()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty =Convert.ToInt32(0);
                if (ProductCode == "MNS03" && Qty >= 5)
                {
                    return true;
                }
            }
        }
        return false;
    }
    //private bool ChkProductOffer()
    //{
    //    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
    //    {
    //        string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
    //        int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
    //        if (ProductCode == "AID01" && Qty >= 2)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    private bool CefimaxFOC()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty = Convert.ToInt32(0);
                if (ProductCode == "ANB085" && Qty >= 9898884)
                {
                    return true;
                }
            }
        }

        return false;
    }
    private bool CiprodylFOC()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        DataTable aTable = new DataTable();
        aTable = aInvoiceBll.LoadOrderWithDetail(orderHiddenField.Value);
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(aTable.Rows[0]["CustomerCode"].ToString());
        if (Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]) == true)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                int Qty = Convert.ToInt32(0);
                if (ProductCode == "ANB098" && Qty >= 989894)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    public void LoadAllDataByOrder(string orderId)
    {
        try
        {
            //if (OrderExists(orderHiddenField.Value) == false)
            //{
            OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
            DataTable aTable = new DataTable();
            aTable = aInvoiceBll.LoadOrderWithDetail(orderId);
            //orderNoTextBox.Text = aTable.Rows[0]["OrderCode"].ToString();
            //orderIdHiddenField.Value = aTable.Rows[0]["OrderId"].ToString();
            // orderDateTextBox.Text = Convert.ToDateTime(aTable.Rows[0]["SubmissionDate"].ToString()).ToString("dd-MMM-yyyy");
            GetCustInfo(aTable.Rows[0]["CustomerCode"].ToString());

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                // if (gridLineItemGridView.Rows.Count == 0)
                {
                    AddFunc();
                }

                ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value =
                    //   "1";
                    aTable.Rows[i]["OrderDetailId"].ToString();
                GetProduct(i, aTable.Rows[i]["ProductCode"].ToString());
                /////SMC Low Stock Method /////
                decimal cstock = 0;
                cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text);
                if (cstock < Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()))
                {
                    GetQty(Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()), i);
                }
                ///////////////////
                else
                {
                    GetQty(Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()), i);
                }


            }
            decimal totalprice = 0;
            if (!string.IsNullOrEmpty(tpTptalTextBox.Text))
            {
                decimal.TryParse(tpTptalTextBox.Text, out totalprice);
            }

            // Campaing 2
            decimal percentage = 0;
            string PaymentType = "";
            string SubmissionDate = "";
            int CustTypeId = 0;
            OrderInfoDAL OrderInfoDALs = new OrderInfoDAL();

            try
            {
                if (aTable != null && aTable.Rows.Count > 0 && aTable.Rows[0]["CustomerCode"] != DBNull.Value)
                {
                    DataTable dtFixedCustomer = OrderInfoDALs.GetFixedCustomerffff(aTable.Rows[0]["CustomerCode"].ToString());
                    if (dtFixedCustomer != null && dtFixedCustomer.Rows.Count > 0 && Convert.ToBoolean(dtFixedCustomer.Rows[0]["FixedCustomer"]))
                    {
                        percentage = 0;
                    }
                    else
                    {
                        DataTable dttradepolicy = aOrderInfoBll.GetParcentFromOrderDetailsMasterID(orderIdHiddenField.Value);
                        if (dttradepolicy != null && dttradepolicy.Rows.Count > 0)
                        {
                            PaymentType = dttradepolicy.Rows[0]["PaymentType"].ToString();
                            CustTypeId = Convert.ToInt32(dttradepolicy.Rows[0]["CustTypeId"].ToString());
                            SubmissionDate = dttradepolicy.Rows[0]["SubmissionDate"].ToString();
                        }
                        DataTable dttradepolicy22 = aOrderInfoBll.GetTradeTerm(totalprice.ToString(), CustTypeId, PaymentType, SubmissionDate);
                        if (dttradepolicy22 != null && dttradepolicy22.Rows.Count > 0)
                        {
                            percentage = Convert.ToDecimal(dttradepolicy22.Rows[0]["DiscountPerc"].ToString());
                        }
                    }
                }
            }
            catch
            {
                percentage = 0;
            }

            decimal totaldiscount = (percentage * totalprice) / 100;
            disTotalTextBox.Text = totaldiscount.ToString("F2");

            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                TextBox sdBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("sdTextBox");
                if (sdBox != null) sdBox.Text = "0.00";

                TextBox dpBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpTextBox");
                if (dpBox != null) dpBox.Text = percentage.ToString();

                TextBox dpAmtBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpAmtTextBox");
                TextBox tpBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox");
                if (dpAmtBox != null && tpBox != null)
                {
                    decimal itemTp = 0;
                    decimal.TryParse(tpBox.Text.Trim(), out itemTp);
                    dpAmtBox.Text = ((itemTp * percentage) / 100).ToString("F2");
                }
            }

            TotalValueCalculation();
            GetDiscounttotalValue();
        }
        catch (Exception ex)
        {
            showMessageBox("Error: " + ex.Message.Replace("'", ""));
        }
    }

    private void GetOrderDetailValue(string orderId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderId);

        for (int i = 0; i < aTable.Rows.Count; i++)
        {
            ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value =
                aTable.Rows[i]["OrderDetailId"].ToString();
        }
    }

    private void GetDiscounttotalValue()
    {
        decimal disTotal = 0;
        decimal tqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            //tqty = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text);

            //if (((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "ANB08" && tqty >= 3)
            //{
            //    disTotal = 0;
            //    //(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;
            //}
            //else
            {
                disTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "")
                    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text)
                    : 0;
            }
        }


        disTotalTextBox.Text = disTotal.ToString();
    }

    //FOC Bonus Qty Start//
    private bool B()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
            if (ProductCode == "AID01" && Qty >= 4)
            {

                return true;
            }
        }
        return false;
    }
    private bool B2()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
            if (ProductCode == "AEL02" && Qty >= 3)
            {

                return true;
            }
        }
        return false;
    }
    private bool B4()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
            if (ProductCode == "AID04" && Qty >= 5)
            {

                return true;
            }
        }
        return false;
    }
    private bool B3()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
            if (ProductCode == "OAD04" && Qty >= 2)
            {

                return true;
            }
        }
        return false;
    }
    private bool B5()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
            if (ProductCode == "MNS03" && Qty >= 5)
            {

                return true;
            }
        }
        return false;
    }
    private bool B6()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
            if (ProductCode == "FGD02" && Qty >= 4)
            {

                return true;
            }
        }
        return false;
    }
    //FOC Bonus Qty End//



    private void InitialGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("SpecialAmount");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("CostPrice");
        aDataTable.Columns.Add("UnitVAT");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");
        aDataTable.Columns.Add("NetPrice");
        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";

        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["SpecialAmount"] = "";
        dataRow["OrderDetailsId"] = "";
        dataRow["StockQty"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["CostPrice"] = "";
        dataRow["UnitVAT"] = "";
        dataRow["Quantity"] = "";
        dataRow["TotalPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["DiscountPercentage"] = "";
        dataRow["DiscountAmount"] = "";
        dataRow["NetPrice"] = "";
        dataRow["BonusQty"] = "";
        dataRow["TotalQty"] = "";
        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }

    public void AddFunc()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");

        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("CostPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");
        aDataTable.Columns.Add("SpecialAmount");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("codeTextBox")).Text.Trim();
                HiddenField hfOrd = (HiddenField)gridLineItemGridView.Rows[i].FindControl("orderdetailIdHiddenField");
                dataRow["OrderDetailsId"] = hfOrd != null ? hfOrd.Value.Trim() : "";
                TextBox sdBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("sdTextBox");
                dataRow["SpecialAmount"] = sdBox != null ? sdBox.Text.Trim() : "";
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("unitPriceTextBox")).Text.Trim();
                TextBox cogsBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("cogsTextBox");
                dataRow["CostPrice"] = cogsBox != null ? cogsBox.Text.Trim() : "";
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("upVatTextBox")).Text.Trim();
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpVatTextBox")).Text.Trim();
                TextBox dpBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpTextBox");
                dataRow["DiscountPercentage"] = dpBox != null ? dpBox.Text.Trim() : "";
                TextBox dpAmtBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpAmtTextBox");
                dataRow["DiscountAmount"] = dpAmtBox != null ? dpAmtBox.Text.Trim() : "";
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("npTextBox")).Text.Trim();
                TextBox bQtyBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("bQtyTextBox");
                dataRow["BonusQty"] = bQtyBox != null ? bQtyBox.Text.Trim() : "";
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].FindControl("tQtyTextBox")).Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }
        }
        //else
        {
            int sl = aDataTable.Rows.Count;
            dataRow = aDataTable.NewRow();

            dataRow["SL"] = Convert.ToString(sl + 1);
            dataRow["ProductCode"] = "";
            dataRow["OrderDetailsId"] = "";
            dataRow["ProductName"] = "";
            dataRow["StockQty"] = "";
            dataRow["UnitPrice"] = "";
            dataRow["CostPrice"] = "";
            dataRow["UnitVAT"] = "";
            dataRow["Quantity"] = "";
            dataRow["TotalPrice"] = "";
            dataRow["VAT"] = "";
            dataRow["DiscountPercentage"] = "";
            dataRow["DiscountAmount"] = "";
            dataRow["SpecialAmount"] = "";
            dataRow["NetPrice"] = "";
            dataRow["BonusQty"] = "";
            dataRow["TotalQty"] = "";
            aDataTable.Rows.Add(dataRow);
        }
       

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }


    protected void addImageButton_Click6(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "FGD02");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "FGD02";
        dataRow["ProductName"] = "Moticare 10mg:FGD02 ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty6();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty6();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }
    protected void addImageButton_Click5(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "MNS03");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "MNS03";
        dataRow["ProductName"] = "Seacoral D Tablet:6 X 10s ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty5();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty5();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }
    protected void addImageButton_Click(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "AID01");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "AID01";
        dataRow["ProductName"] = "Flexidol Tablet~100mg :10 X 10s ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }
    protected void addImageButton_Click2(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "AEL02");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "AEL02";
        dataRow["ProductName"] = "Nervaid 75mg Capsule:6 X 10s ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty2();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty2();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }
    protected void addImageButton_Click3(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "OAD04");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "OAD04";
        dataRow["ProductName"] = "Maxivent 400 mg Tablet:3 X 10s ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty3();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty3();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }
    protected void addImageButton_Click4(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "AID04");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "AID04";
        dataRow["ProductName"] = "Ezepain 90 mg Tablet :3 X 10s ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty4();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty4();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }

    private int Qty()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "AID01")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
    }
    private int Qty5()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "MNS03")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
    }
    private int Qty6()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "FGD02")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
    }
    private int Qty4()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "AID04")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
    }
    private int Qty2()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "AEL02")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
    }
    private int Qty3()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "OAD04")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
    }
    protected void removeImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton productCodeTextBox = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)productCodeTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            int sl1 = 1;
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                if (i != rowindex)
                {
                    dataRow = aDataTable.NewRow();

                    dataRow["SL"] = Convert.ToString(sl1);
                    dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                    dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                    dataRow["StockQty"] =
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                    dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                    dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                    dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                    dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                    dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                    dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                    dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                    dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                    dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                    dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                    aDataTable.Rows.Add(dataRow);
                    sl1 += 1;
                }
            }
        }

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
    }

    protected void codeTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productCodeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

        string productCode = productCodeTextBox.Text.Trim();
        GetProduct(rowindex, productCode);
    }

    private void GetProduct(int rowindex, string productCode)
    {
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {
            if (ProductCodeValidation(productCode, rowindex) == true)
            {

                string effectiveComUnitId = (salesCenterDropDownList.SelectedIndex > 0 && !string.IsNullOrEmpty(salesCenterDropDownList.SelectedValue) && salesCenterDropDownList.SelectedValue != "0")
                    ? salesCenterDropDownList.SelectedValue
                    : ((Session["DICCompanyUnitId"] != null && Session["DICCompanyUnitId"].ToString() != "" && Session["DICCompanyUnitId"].ToString() != "0")
                        ? Session["DICCompanyUnitId"].ToString()
                        : (!string.IsNullOrEmpty(hdComUnitId.Value) && hdComUnitId.Value != "0" ? hdComUnitId.Value : "0"));

                if (rbTypePrice.Items[0].Selected)
                {
                    aDataTable = aInvoiceBll.ProductInfo_Old(effectiveComUnitId, productCode);

                    if (aDataTable.Rows.Count == 0)
                    {
                        aDataTable = aInvoiceBll.ProductInfo(effectiveComUnitId, productCode);
                    }
                }
                else
                {
                    aDataTable = aInvoiceBll.ProductInfo(effectiveComUnitId, productCode);
                }
              
                ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text = productCode;
                if (aDataTable.Rows.Count > 0)
                {
                    TextBox nameTextBox =
                        (TextBox)gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox");
                    nameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();
                    TextBox currentStockTextBox =
                        (TextBox)gridLineItemGridView.Rows[rowindex].Cells[3].FindControl("currentStockTextBox");
                    currentStockTextBox.Text = aDataTable.Rows[0]["StockQty"].ToString();
                    TextBox unitPriceTextBox =
                        (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("unitPriceTextBox");
                    if (unitPriceTextBox != null)
                        unitPriceTextBox.Text = aDataTable.Rows[0]["UnitPrice"].ToString();

                    TextBox cogsTextBox =
                        (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("cogsTextBox");
                    if (cogsTextBox != null && aDataTable.Columns.Contains("CostPrice"))
                        cogsTextBox.Text = aDataTable.Rows[0]["CostPrice"].ToString();

                    TextBox upVatTextBox =
                        (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("upVatTextBox");
                    if (upVatTextBox != null)
                        upVatTextBox.Text = aDataTable.Rows[0]["VAT"].ToString();
                }
                else
                {
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox")).Text = "";
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text = "";
                    showMessageBox("No Any Stock or Product of " + productCode);
                }
            }
            else
            {
                ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox")).Text = "";
                ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text = "";
                showMessageBox(productCode + " No: Product Already Inserted!!!");
            }
        }
    }

    private bool ProductCodeValidation(string productCode, int rowindex)
    {

        //for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        //{
        //    if (rowindex!=i)
        //    {
        //        if (((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim() ==
        //            productCode.Trim())
        //        {
        //            return false;
        //        }
        //    }
        //}

        return true;
    }
    protected void showMessageBox(string message)
    {
        message = message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
        string sScript = String.Format("showpop6('{0}', 'Error');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void showSuccessMessageBox(string message)
    {
        message = message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
        string sScript = String.Format("showpop5('{0}', 'Success');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    public void GetQty(decimal qty, int rowindex)
    {
        GridViewRow row = gridLineItemGridView.Rows[rowindex];
        TextBox qtyTextBox1 = row.FindControl("qtyTextBox") as TextBox;
        if (qtyTextBox1 != null) qtyTextBox1.Text = qty.ToString();

        RecalculateRow(rowindex);
        TotalValueCalculation();
    }

    protected void unitPriceTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox unitPriceBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)unitPriceBox.Parent.Parent;
        int rowindex = currentRow.RowIndex;

        RecalculateRow(rowindex);
        TotalValueCalculation();
    }

    protected void qtyTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox qtyTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        int rowindex = currentRow.RowIndex;

        RecalculateRow(rowindex);
        TotalValueCalculation();
    }

    private void RecalculateRow(int rowindex)
    {
        GridViewRow row = gridLineItemGridView.Rows[rowindex];
        TextBox qtyBox = row.FindControl("qtyTextBox") as TextBox;
        TextBox unitPriceBox = row.FindControl("unitPriceTextBox") as TextBox;
        TextBox upVatBox = row.FindControl("upVatTextBox") as TextBox;
        TextBox tpBox = row.FindControl("tpTextBox") as TextBox;
        TextBox tpVatBox = row.FindControl("tpVatTextBox") as TextBox;
        TextBox codeBox = row.FindControl("codeTextBox") as TextBox;
        TextBox dpBox = row.FindControl("dpTextBox") as TextBox;
        TextBox dpAmtBox = row.FindControl("dpAmtTextBox") as TextBox;
        TextBox npBox = row.FindControl("npTextBox") as TextBox;
        TextBox tQtyBox = row.FindControl("tQtyTextBox") as TextBox;
        TextBox bQtyBox = row.FindControl("bQtyTextBox") as TextBox;

        decimal qty = 0, unitPrice = 0, upVat = 0, bQty = 0;
        if (qtyBox != null) decimal.TryParse(qtyBox.Text.Trim(), out qty);
        if (unitPriceBox != null) decimal.TryParse(unitPriceBox.Text.Trim(), out unitPrice);
        if (upVatBox != null) decimal.TryParse(upVatBox.Text.Trim(), out upVat);
        if (bQtyBox != null) decimal.TryParse(bQtyBox.Text.Trim(), out bQty);

        decimal tp = unitPrice * qty;
        if (tpBox != null) tpBox.Text = tp.ToString("F2");

        decimal tpVat = upVat * qty;
        if (tpVatBox != null) tpVatBox.Text = tpVat.ToString("F2");

        decimal discountPer = 0;
        if (codeBox != null && qtyBox != null)
        {
            discountPer = aInvoiceBll.ProductDiscount(codeBox.Text.Trim(), qtyBox.Text.Trim());
        }
        if (dpBox != null) dpBox.Text = discountPer.ToString();

        decimal dpAmt = 0;
        if (discountPer > 0)
        {
            dpAmt = (tp / 100) * discountPer;
        }
        if (dpAmtBox != null) dpAmtBox.Text = dpAmt.ToString("F2");

        decimal np = (tp - dpAmt) + tpVat;
        if (npBox != null) npBox.Text = np.ToString("F2");

        if (tQtyBox != null) tQtyBox.Text = (qty + bQty).ToString();
        if (bQtyBox != null && string.IsNullOrEmpty(bQtyBox.Text.Trim())) bQtyBox.Text = "0";
    }
    protected void bQtyTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox bQtyTextBox1 = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)bQtyTextBox1.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text =
            Convert.ToString(
                Convert.ToDecimal(
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text.Trim()) +
                Convert.ToDecimal(
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text.Trim()));

    }
    public bool SaveReturnAmount(int invid)
    {
        ReturnAmountDAO amountDao = new ReturnAmountDAO()
        {
            CustomerId = Convert.ToInt32(hdCustomerMasterId.Value),
            Amount = Convert.ToDecimal(grandTotalTextBox.Text),
            //  ReturnInvoiceId = invid,
            InvoiceId = Convert.ToInt32(invid)
        };
        return aInvoiceBll.SaveDataForReturnAmount(amountDao);
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        //if (Validation() == true)
        //{
        //    if (OrderExists(orderHiddenField.Value) == false)
        //    {
        //        {
        //            try
        //            {
        //                int invId = SaveInvoice();
        //                if (invId > 0)
        //                {
        //                    bool dtl = SaveInvoiceDetail(invId);
        //                    if (dtl == true)
        //                    {
        //                        SaveReturnAmount(invId);
        //                        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        //                        aOrderInfoBll.UpdateInvoiceStatus(orderIdHiddenField.Value);
        //                        Clear();
        //                        showMessageBox("Proforma Invoice Created Successsfully!!");
        //                    }
        //                    else
        //                    {
        //                        aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
        //                        Clear();
        //                        showMessageBox("Internet  error code 1231!! please generate Invoice again");

        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                // aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
        //                Clear();
        //                showMessageBox("Internet error code 1231!!  please generate Invoice again");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        showMessageBox("Invoice already Generated !!");
        //    }
        //}
    }

    private bool SaveInvoiceDetail(int invoiceId)
    {

        List<InvoiceDetail> aInvoiceDetailsList = new List<InvoiceDetail>();

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
                DataTable dtdiscount =
                aOrderInfoBll.ProductDiscount(
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text,
                    hdCustomerMasterId.Value, DateTime.Today.ToString());

                InvoiceDetail aInvoiceDetail = new InvoiceDetail();
                aInvoiceDetail.ProductCode =
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                string product = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text;
                string[] proNameAndPackSize = product.Split(':');
                aInvoiceDetail.ProductName = proNameAndPackSize[0];
                aInvoiceDetail.PackSize = proNameAndPackSize[1];
                aInvoiceDetail.UnitPrice = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text);
                aInvoiceDetail.UnitVatAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text);
                aInvoiceDetail.Quantity = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text);
                aInvoiceDetail.DiscountPercentage = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text);
                aInvoiceDetail.DiscountAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text);
                aInvoiceDetail.BonusQuantity = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim());
                aInvoiceDetail.SpecialAmount = 0;
                //Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim());
                string id =
                    ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aInvoiceDetail.OrderDetailsId = Convert.ToInt32(id);
                aInvoiceDetail.InvoiceId = invoiceId;
                aInvoiceDetail.TotalPriceVatAmount =
                    Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text);
                TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");
                aInvoiceDetail.NetAmount = Convert.ToDecimal(npTextBox.Text);
                if (dtdiscount.Rows.Count > 0)
                {
                    aInvoiceDetail.SpecialAmountPer = Convert.ToDecimal(dtdiscount.Rows[0]["DiscountPercentage"].ToString());
                }
                else
                {
                    aInvoiceDetail.SpecialAmountPer = 0;
                }
                decimal cstock = 0;
                decimal tqty = 0;
                tqty = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text);
                cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text);
                DataTable dtdata = aInvoiceBll.LoadProductQty(orderIdHiddenField.Value,
                ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);
                tqty = Convert.ToDecimal(dtdata.Rows[0][0].ToString());
                /////SMC Low Order Method /////
                //if (tqty>cstock)
                //{
                //    aInvoiceBll.UpdateOrder("Undelivered", aInvoiceDetail.OrderDetailsId.ToString());
                //}
                //else
                //{
                //    aInvoiceDetailsList.Add(aInvoiceDetail);    
                //}
                ///////////////////


                if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "OAD04" && MaxiventFOC())
                {
                    aInvoiceDetail.Campaign = "Bonus Campaign";

                }

                //// Resectin FOC Offer Discount Start end//
                if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "AEL02" && Nervaid75mgCapsuleFOC())
                {
                    aInvoiceDetail.Campaign = "Bonus Campaign";
                }



                if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "FGD02" && Moticare())
                {
                    aInvoiceDetail.Campaign = "Bonus Campaign";
                }

                if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "MNS03" && SeacoralDTablet())
                {
                    aInvoiceDetail.Campaign = "Bonus Campaign";
                }

                if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "AID04" && Ezepain())
                {
                    aInvoiceDetail.Campaign = "Bonus Campaign";
                }



                if (cstock == 0)
                {
                    aInvoiceBll.UpdateOrder("Undelivered", aInvoiceDetail.OrderDetailsId.ToString());
                }
                else
                {
                    aInvoiceDetailsList.Add(aInvoiceDetail);
                }
            }

            aInvoiceBll.SaveInvoiceDetails(aInvoiceDetailsList, hdComUnitId.Value);
        }

        return true;
    }

    private int SaveInvoice()
    {

        //int invoiceId = 0;
        //DataTable aDataTable = new DataTable();
        //aDataTable = aInvoiceBll.CustomerMaster(custCodeTextBox.Text);
        //string invoiceNo = string.Empty;
        //string[] forComUCode = comUnitNameTextBox.Text.Split(':');
        //string ComUnitCode = forComUCode[0];
        //Invoice aInvoice = new Invoice();
        //// {
        //aInvoice.InvoiceDate = Convert.ToDateTime(DateTime.Today.ToString());
        //aInvoice.OrderNo = orderNoTextBox.Text.Trim();
        //aInvoice.OrderDate = Convert.ToDateTime(orderDateTextBox.Text.Trim());
        //aInvoice.CustomerMasterId = Convert.ToInt32(hdCustomerMasterId.Value);
        //aInvoice.ComUnitId = Convert.ToInt32(hdComUnitId.Value);
        //aInvoice.MiaId = Convert.ToInt32(hdMiaId.Value);
        //aInvoice.PaymentTypeId = Convert.ToInt32(1);
        //aInvoice.TpTotal = Convert.ToDecimal(tpTptalTextBox.Text.Trim());
        //aInvoice.TpDiscount = Convert.ToDecimal(disTotalTextBox.Text.Trim());
        //aInvoice.TpVat = Convert.ToDecimal(vatTotalTextBox.Text.Trim());
        //aInvoice.TpGrandTotal = Convert.ToDecimal(grandTotalTextBox.Text.Trim());
        //aInvoice.UserId = Convert.ToInt32(Session["UserId"].ToString());
        //aInvoice.ComUnitCode = ComUnitCode;
        //aInvoice.OrderId = Convert.ToInt32(orderIdHiddenField.Value);
        //aInvoice.TotalSpecialAmount = Convert.ToDecimal(pdTextBox.Text);
        //aInvoice.OldTradePolicy = false;
        //////// SMC Low Stock Method////////
        //aInvoice.Remarks = remarksTextBox.Text;
        //aInvoice.MIACode = aDataTable.Rows[0]["MIACode"].ToString();
        //aInvoice.MIAName = aDataTable.Rows[0]["MIAName"].ToString();
        //aInvoice.MarketCode = aDataTable.Rows[0]["MarketCode"].ToString();
        //aInvoice.MarketName = aDataTable.Rows[0]["MarketName"].ToString();
        //aInvoice.AreaCode = aDataTable.Rows[0]["AreaCode"].ToString();
        //aInvoice.DisCode = aDataTable.Rows[0]["DistrictCode"].ToString();
        //aInvoice.FEName = aDataTable.Rows[0]["FEName"].ToString();
        //aInvoice.RegionCode = aDataTable.Rows[0]["RegionCode"].ToString();
        //aInvoice.DZSMName = aDataTable.Rows[0]["DZSMName"].ToString();
        //aInvoice.FixedCustomer = Convert.ToBoolean(aDataTable.Rows[0]["FixedCustomer"].ToString());
        //aInvoice.Type = Convert.ToString(aDataTable.Rows[0]["Type"].ToString());
        //aInvoice.DpNAme = deliverypersonNameTextBox.Text.Trim();
        //aInvoice.DpMob = deliverypersonMobileTextBox.Text.Trim();
        //aInvoice.ProductOffer = "False";

        //aInvoice.Createdate = DateTime.Now;
        //aInvoice.AdjustAmount = Convert.ToDecimal(crAmountTextBox.Text);
        //aInvoice.ReceivableAmount = Convert.ToDecimal(rcvAmountTextBox.Text);
        //if (Convert.ToDecimal(crAmountTextBox.Text) > 0)
        //{
        //    aInvoice.IsAdjustInvoice = true;
        //}
        //else
        //{
        //    aInvoice.IsAdjustInvoice = false;

        //}
        //////////////////Product Multiple Offer End ///////////

        //aInvoiceBll.SaveInvoice(aInvoice, out invoiceId, out invoiceNo);

        //invTextBox.Text = invoiceNo;

        return 1;
    }
    private void Clear(bool keepInvoiceNo = false)
    {
        tpTptalTextBox.Text = "0.00";
        vatTotalTextBox.Text = "0.00";
        disTotalTextBox.Text = "0.00";
        grandTotalTextBox.Text = "0.00";
        pdTextBox.Text = "0.00";
        txtDeductionCharge.Text = "0.00";
        txtNetReturnAmount.Text = "0.00";

        if (!keepInvoiceNo)
        {
            invTextBox.Text = string.Empty;
        }

        hdComUnitId.Value = string.Empty;
        hdCustomerMasterId.Value = string.Empty;
        hfMIOId.Value = string.Empty;
        hfMioEmpId.Value = string.Empty;
        hfTerriId.Value = string.Empty;
        hfSapCode.Value = string.Empty;
        orderHiddenField.Value = string.Empty;
        hdMiaId.Value = string.Empty;

        custCodeTextBox.Text = string.Empty;
        custNameTextBox.Text = string.Empty;
        custAddressTextBox.Text = string.Empty;
        lblMioInfo.Text = string.Empty;
        ddlInvoiceNo.Items.Clear();
        districtNameTextBox.Text = string.Empty;
        areaNameTextBox.Text = string.Empty;
        comUnitNameTextBox.Text = string.Empty;
        miaCodeTextBox.Text = string.Empty;
        marketNameTextBox.Text = string.Empty;
        miaNameTextBox.Text = string.Empty;
        custCategoryTextBox.Text = string.Empty;

        orderDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        orderNoTextBox.Text = "Return From Market";

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();

        LoadDistributionCenter();
        LoadAllProduct();
    }

    private void TotalValueCalculation()
    {
        decimal tpTotal = 0;
        decimal vatTotal = 0;
        decimal disTotal = 0;
        decimal gTotal = 0;
        decimal sptotatl = 0;
        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                TextBox tpBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox");
                TextBox vatBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("tpVatTextBox");
                TextBox dpAmtBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("dpAmtTextBox");
                TextBox npBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("npTextBox");
                TextBox sdBox = (TextBox)gridLineItemGridView.Rows[i].FindControl("sdTextBox");

                decimal val = 0;
                if (tpBox != null && decimal.TryParse(tpBox.Text.Trim(), out val)) tpTotal += val;
                if (vatBox != null && decimal.TryParse(vatBox.Text.Trim(), out val)) vatTotal += val;
                if (dpAmtBox != null && decimal.TryParse(dpAmtBox.Text.Trim(), out val)) disTotal += val;
                if (npBox != null && decimal.TryParse(npBox.Text.Trim(), out val)) gTotal += val;
                if (sdBox != null && decimal.TryParse(sdBox.Text.Trim(), out val)) sptotatl += val;
            }
        }

        tpTptalTextBox.Text = tpTotal.ToString("F2");
        vatTotalTextBox.Text = vatTotal.ToString("F2");
        disTotalTextBox.Text = disTotal.ToString("F2");
        grandTotalTextBox.Text = gTotal.ToString("F2");
        pdTextBox.Text = sptotatl.ToString("F2");

        decimal dedVal = 0;
        decimal.TryParse(txtDeductionCharge.Text.Trim(), out dedVal);
        decimal netReturn = gTotal - dedVal;
        if (netReturn < 0) netReturn = 0;
        txtNetReturnAmount.Text = netReturn.ToString("F2");
    }

    protected void txtDeductionCharge_TextChanged(object sender, EventArgs e)
    {
        decimal grandTotal = 0;
        decimal deduction = 0;
        decimal.TryParse(grandTotalTextBox.Text.Trim(), out grandTotal);
        decimal.TryParse(txtDeductionCharge.Text.Trim(), out deduction);
        decimal netReturn = grandTotal - deduction;
        if (netReturn < 0) netReturn = 0;
        txtNetReturnAmount.Text = netReturn.ToString("F2");
    }

    //protected void printButton_Click(object sender, EventArgs e)
    //{
    //    string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invTextBox.Text.Trim());
    //    // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
    //    string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
    //    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    //}

    protected void nameTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        string product = ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox")).Text;

        if (!string.IsNullOrEmpty(product))
        {
            if (product.Contains(':'))
            {
                string[] proNameAndPackSize = product.Split(':');


                TextBox productCodeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");
                productCodeTextBox.Text = proNameAndPackSize[1];
                string productCode = productCodeTextBox.Text.Trim();
                GetProduct(rowindex, productCode);
            }

        }


    }
}