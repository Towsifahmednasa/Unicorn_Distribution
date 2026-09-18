using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.MasterSetup_DAL;
using Library.DAO.SInventory_Entities;
using SalesSolution.Web.DataLayer;

public partial class SInventory_UI_InvoiceCreationForCustomer : System.Web.UI.Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    static CommonDataLoad _dataLoad = new CommonDataLoad();
    OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["OrderId"] != null)
            {
                 
                orderHiddenField.Value = Session["OrderId"].ToString();


               

                LoadAllDataByOrder(orderHiddenField.Value.ToString());

                ddlDAName.Items.Clear();
                try
                {
                    RouteInformationDAL _DalRoute = new RouteInformationDAL();
                    using (DataTable dt = _DalRoute.GetDANameDDL(Convert.ToInt32(hdComUnitId.Value)))
                    {
                        ddlDAName.DataSource = dt;
                        ddlDAName.DataValueField = "Value";
                        ddlDAName.DataTextField = "TextField";
                        ddlDAName.DataBind();
                        ddlDAName.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                        ddlDAName.SelectedIndex = 0;
                    }
                }
                catch (Exception ex) { }


                CustomerCreditAmount();
             

                //DataTable dtwar = aInvoiceBll.GetWarning(hdCustomerMasterId.Value, custCodeTextBox.Text);
                //if (dtwar.Rows.Count > 0)
                //{
                //    string a = dtwar.Rows[0]["Details"].ToString();
                //    warningLabel.Text = "This customer has Dues(time  more than 30 days)"+ a;
                //}
                //else
                //{
                //    warningLabel.Text = "";
                //}
                AdjustmentAmount();

                //shuvo
                for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
                {
                    TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");
                    TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox");
                    decimal tpTextBox = 0;
                    try
                    {
                        tpTextBox = Convert.ToDecimal(
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim());
                    }
                    catch
                    {

                    }
                    decimal dpAmtTextBox = 0;
                    try
                    {

                        dpAmtTextBox = Convert.ToDecimal(
                                           ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox"))
                                               .Text.Trim());

                    }
                    catch
                    {

                    }

                    decimal amTextBox = 0;
                    try
                    {

                        amTextBox = Convert.ToDecimal(
                                           ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("amTextBox"))
                                               .Text.Trim());

                    }
                    catch
                    {

                    }

                    decimal tpVat = 0;
                    try
                    {

                        tpVat = Convert.ToDecimal(tpVatTextBox.Text);

                    }
                    catch
                    {

                    }

                    decimal rS = (tpTextBox - (dpAmtTextBox + amTextBox)) + tpVat;
                    npTextBox.Text = rS.ToString();
                }
                TotalValueCalculation();
            }
            aInvoiceBll.PaymentTypeLoadBLL(payTypeDDL);
            payTypeDDL.SelectedIndex = 1;
            Todate();

            Session["OrderId"] = null;
            Session["RootNameId"] = null;
        }
    }

    public void AdjustmentAmount()
    {
        decimal divider = Convert.ToDecimal(gridLineItemGridView.Rows.Count);
        decimal amount =string.IsNullOrEmpty(crAmountTextBox.Text)?0:Convert.ToDecimal(crAmountTextBox.Text);
        decimal count = 0;
        for(int i =0;i<gridLineItemGridView.Rows.Count;i++)
        {
            if(gridLineItemGridView.Rows[i].BackColor == Color.Red)
            {
                divider = divider - 1;
               
            }
        }
        if (divider>0)
        {
            decimal peramount = amount / divider;
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {

                if (gridLineItemGridView.Rows[i].BackColor == Color.Red)
                {
                 
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("amTextBox")).Text = "0";
                }

                else { 
                ((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("amTextBox")).Text = peramount.ToString("F");
                }
            }
        }
       
    }

    private void CustomerCreditAmount()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable custTable = new DataTable();
        custTable = aOrderInfoBll.GetCustomerCredit(hdCustomerMasterId.Value);
        decimal crAmount = 0;
        if (custTable != null && custTable.Rows.Count > 0 && custTable.Rows[0]["Amount"] != DBNull.Value)
        {
            decimal.TryParse(custTable.Rows[0]["Amount"].ToString(), out crAmount);
        }
        crAmountTextBox.Text = crAmount.ToString("0.00");

        decimal gTotal = 0;
        decimal.TryParse(grandTotalTextBox.Text.Trim(), out gTotal);
        rcvAmountTextBox.Text = (gTotal - crAmount).ToString("0.00");
    }
    protected void backLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("InvoiceCreationByOrder.aspx");
    }

    public void Todate()
    {
        invDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
    }
    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if ((gv.ShowHeader == true && gv.Rows.Count > 0)
            || (gv.ShowHeaderWhenEmpty == true))
        {
            //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    private bool Validation()
    {
        //if (warningLabel.Text == "This customer has Dues(time  more than 30 days)")
        //{
        //    showMessageBox("This customer has Dues(time  more than 30 days)!!");
        //    return false;
        //}
        if (orderNoTextBox.Text == "")
        {
            showMessageBox("Please Input Order Number!!");
            return false;
        }
        if (orderDateTextBox.Text == "")
        {
            showMessageBox("Please Input Order Date!!");
            return false;
        }

        if (payTypeDDL.Text == "")
        {
            showMessageBox("Please Input Payment Type!!");
            return false;
        }
        if (Convert.ToDecimal(rcvAmountTextBox.Text) < 0)
        {
            showMessageBox("Invalid Receivable Amount!!");
            return false;
        }
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim() == "" || ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim() == "")
            {
                showMessageBox("Please Remove Blank Row!!");
                return false;
            }
        }

        if (ddlDAName.SelectedValue == "")
        {
            showMessageBox("Please Select DA Name!!");
            ddlDAName.Focus();
            return false;
        }
        return true;
    }

    private void GetCustInfo(string custCode, string   OrderNO)
    {
        if (!string.IsNullOrEmpty(custCode))
        {
            custCodeTextBox.Text = custCode;
            DataTable aDataTable = new DataTable();
            aDataTable = aInvoiceBll.CustomerMaster( OrderNO);
            if (aDataTable.Rows.Count > 0)
            {
                hdComUnitId.Value = aDataTable.Rows[0]["ComUnitId"].ToString();
                hdCustomerMasterId.Value = aDataTable.Rows[0]["CustomerMasterId"].ToString();
                custNameTextBox.Text = aDataTable.Rows[0]["CustomerName"].ToString();
                custAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                districtNameTextBox.Text = aDataTable.Rows[0]["ASMEmpName"].ToString();
                areaNameTextBox.Text = aDataTable.Rows[0]["AreaName"].ToString();
                comUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitCode"].ToString() + ":" + aDataTable.Rows[0]["ComUnitName"].ToString();
               miaCodeTextBox.Text = aDataTable.Rows[0]["MIOEmpMastercode"].ToString();
                 hdMiaId.Value = aDataTable.Rows[0]["MIOEmpInfoId"].ToString();
                marketNameTextBox.Text = aDataTable.Rows[0]["MarketName"].ToString();
                 miaNameTextBox.Text = aDataTable.Rows[0]["MIOEmpName"].ToString();
                custCategoryTextBox.Text = aDataTable.Rows[0]["Type"].ToString();//Green/Blue/Pnk
                //hdMiaId.Value = aDataTable.Rows[0]["MiaId"].ToString();
                cusTypeTextBox.Text = aDataTable.Rows[0]["CustomerType"].ToString();//FCB/Institue/Genral
            }
            else
            {

            }
        }
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

        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                    Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "FGDsa02" && Qty >= 4)
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

        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                    Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "AELss02" && Qty >= 3)
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
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                    Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
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
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                    Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "AIDs04" && Qty >= 5)
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
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                    Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "sOAD04" && Qty >= 2)
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
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                    Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "MNSs03" && Qty >= 5)
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
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "ANB0s85" && Qty >= 9898884)
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
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderHiddenField.Value);
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
                int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
                if (ProductCode == "ANBs098" && Qty >= 989894)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public void LoadAllDataByOrder(string orderId)
    {
        if (OrderExists(orderHiddenField.Value) == false)
        {
            OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
            DataTable aTable = new DataTable();
            aTable = aOrderInfoBll.LoadOrderWithDetail(orderId);
            orderNoTextBox.Text = aTable.Rows[0]["OrderCode"].ToString();
            orderIdHiddenField.Value = aTable.Rows[0]["OrderId"].ToString();
            orderDateTextBox.Text = Convert.ToDateTime(aTable.Rows[0]["SubmissionDate"].ToString()).ToString("dd-MMM-yyyy");

            if (aTable.Columns.Contains("Remarks") && aTable.Rows[0]["Remarks"] != DBNull.Value)
            {
                string orderRemarks = aTable.Rows[0]["Remarks"].ToString().Trim();
                remarksTextBox.Text = orderRemarks;
                if (!string.IsNullOrEmpty(orderRemarks))
                {
                    divOrderRemarksAlert.Visible = true;
                    lblOrderRemarksSpecial.Text = orderRemarks;
                }
                else
                {
                    divOrderRemarksAlert.Visible = false;
                }
            }

            if (aTable.Columns.Contains("DeliveryCharge") && aTable.Rows[0]["DeliveryCharge"] != DBNull.Value)
            {
                decimal orderDelCharge = 0;
                if (decimal.TryParse(aTable.Rows[0]["DeliveryCharge"].ToString(), out orderDelCharge) && orderDelCharge > 0)
                {
                    deliveryChargeTextBox.Text = orderDelCharge.ToString("0.00");
                }
                else
                {
                    deliveryChargeTextBox.Text = "0.00";
                }
            }
            else
            {
                deliveryChargeTextBox.Text = "0.00";
            }

            GetCustInfo(aTable.Rows[0]["CustomerCode"].ToString(), orderNoTextBox.Text.Trim());
            int numberOfRecords = aTable.Rows.Count;


            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                AddFunc();
                ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value =
                    aTable.Rows[i]["OrderDetailId"].ToString();
                GetProduct(i, aTable.Rows[i]["ProductCode"].ToString());

                // Apply custom/edited TradePrice and UnitVatAmount from Order if present
                if (aTable.Columns.Contains("TradePrice") && aTable.Rows[i]["TradePrice"] != DBNull.Value)
                {
                    TextBox unitPriceTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox");
                    decimal orderTradePrice = 0;
                    if (decimal.TryParse(aTable.Rows[i]["TradePrice"].ToString(), out orderTradePrice))
                    {
                        unitPriceTextBox.Text = orderTradePrice.ToString("0.00");
                    }
                }

                if (aTable.Columns.Contains("UnitVatAmount") && aTable.Rows[i]["UnitVatAmount"] != DBNull.Value)
                {
                    TextBox upVatTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox");
                    decimal orderUnitVat = 0;
                    if (decimal.TryParse(aTable.Rows[i]["UnitVatAmount"].ToString(), out orderUnitVat))
                    {
                        upVatTextBox.Text = orderUnitVat.ToString("0.00");
                    }
                }

                /////SMC Low Stock Method /////
                decimal cstock = 0;
                cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text);
                if (cstock < Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()))
                {
                    GetQty(cstock, i);
                   // GetQty(Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()), i);
                }
                ///////////////////
                else
                {
                    GetQty(Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()), i);
                }

                if (aTable.Columns.Contains("DiscountPercent") && aTable.Rows[i]["DiscountPercent"] != DBNull.Value)
                {
                    decimal ordDisPer = 0;
                    if (decimal.TryParse(aTable.Rows[i]["DiscountPercent"].ToString(), out ordDisPer) && ordDisPer > 0)
                    {
                        TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox");
                        dpTextBox.Text = ordDisPer.ToString("0.00");
                        TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox");
                        decimal dAmt = 0;
                        if (aTable.Columns.Contains("DiscountAmount") && aTable.Rows[i]["DiscountAmount"] != DBNull.Value)
                        {
                            decimal.TryParse(aTable.Rows[i]["DiscountAmount"].ToString(), out dAmt);
                        }
                        else
                        {
                            decimal curTp = 0;
                            decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim(), out curTp);
                            dAmt = (curTp / 100) * ordDisPer;
                        }
                        dpAmtTextBox.Text = dAmt.ToString("0.00");
                        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");
                        decimal tpVal = 0;
                        decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim(), out tpVal);
                        decimal vatVal = 0;
                        decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim(), out vatVal);
                        npTextBox.Text = ((tpVal - dAmt) + vatVal).ToString("0.00");
                    }
                }

                //if (((TextBox) gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text == "0")
                //{
                    ((TextBox) gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text =
                       // "true";
                    aTable.Rows[i]["IsCampaignProduct"].ToString();


                ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("bQtyTextBox")).Text =
                   aTable.Rows[i]["ISGiftProduct"].ToString();
                ((HiddenField)gridLineItemGridView.Rows[i].Cells[10].FindControl("CampaignTypeHiddenField")).Value =
                  aTable.Rows[i]["CampaignName"].ToString();

            }

         //   else
            {
                decimal totalprice = Convert.ToDecimal(tpTptalTextBox.Text);

                {
                    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
                    {
                        //string ProductCode1 =
                        //    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;

                        if ((((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("sdTextBox")).Text == "1"))
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }
                        if (
                            aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 500]-Feb-24 ")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }
                        if (
                              aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 1g IM]-Feb-24 ")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }



                        
                        if (
                              aTable.Rows[i]["CampaignName"].ToString() == "Special Rate [Triforce 1g IM]-Feb-24 ")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }




                        if (
                          aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 1g IM]-Feb-24 ")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }
                        if (
                              aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 500]-Feb-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        } 
                     ///----------------------------
                        
                        
                        if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Aminobost]-Apr-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }   if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Aminobost]-Apr-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }   if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Triforce 1g IV]-Apr-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }   if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Triforce 1g IV]-Apr-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }  if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Triforce 1g IV]-Apr-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }  if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Aminobost]-Apr-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }  if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Triforce 1g IM]-Feb-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }  if (
                              aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Triforce 500]-Feb-24")
                        {
                            totalprice -=
                                Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].FindControl("tpTextBox")).Text);
                        }
                    }
                }
                decimal percentage = 0;
                string PaymentType = "";
                string SubmissionDate = "";
                int CustTypeId = 0;


                // Frinds Hospital Start
                if (custCategoryTextBox.Text == "INSTITUTION"  ||  custCategoryTextBox.Text == "RCP")
                {
                    DataTable dttradepolicy = aOrderInfoBll.GetParcentFromOrderDetails(((HiddenField)gridLineItemGridView.Rows[0].Cells[1].FindControl("orderdetailIdHiddenField")).Value);
                    if (dttradepolicy.Rows.Count > 0)
                    {
                        if (dttradepolicy.Columns.Contains("DiscountPercent") && dttradepolicy.Rows[0]["DiscountPercent"] != DBNull.Value)
                            decimal.TryParse(dttradepolicy.Rows[0]["DiscountPercent"].ToString(), out percentage);
                        if (dttradepolicy.Columns.Contains("PaymentType") && dttradepolicy.Rows[0]["PaymentType"] != DBNull.Value)
                            PaymentType = dttradepolicy.Rows[0]["PaymentType"].ToString();
                        if (dttradepolicy.Columns.Contains("CustTypeId") && dttradepolicy.Rows[0]["CustTypeId"] != DBNull.Value)
                            int.TryParse(dttradepolicy.Rows[0]["CustTypeId"].ToString(), out CustTypeId);
                        if (dttradepolicy.Columns.Contains("SubmissionDate") && dttradepolicy.Rows[0]["SubmissionDate"] != DBNull.Value)
                            SubmissionDate = dttradepolicy.Rows[0]["SubmissionDate"].ToString();
                    }
                }
                else
                {
                    DataTable dttOrdr = aOrderInfoBll.GetParcentFromOrderDetails(((HiddenField)gridLineItemGridView.Rows[0].Cells[1].FindControl("orderdetailIdHiddenField")).Value);
                    if (dttOrdr.Rows.Count > 0)
                    {
                        if (dttOrdr.Columns.Contains("DiscountPercent") && dttOrdr.Rows[0]["DiscountPercent"] != DBNull.Value)
                            decimal.TryParse(dttOrdr.Rows[0]["DiscountPercent"].ToString(), out percentage);
                        if (dttOrdr.Columns.Contains("PaymentType") && dttOrdr.Rows[0]["PaymentType"] != DBNull.Value)
                            PaymentType = dttOrdr.Rows[0]["PaymentType"].ToString();
                        if (dttOrdr.Columns.Contains("CustTypeId") && dttOrdr.Rows[0]["CustTypeId"] != DBNull.Value)
                            int.TryParse(dttOrdr.Rows[0]["CustTypeId"].ToString(), out CustTypeId);
                        if (dttOrdr.Columns.Contains("SubmissionDate") && dttOrdr.Rows[0]["SubmissionDate"] != DBNull.Value)
                            SubmissionDate = dttOrdr.Rows[0]["SubmissionDate"].ToString();
                    }
                    DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(orderHiddenField.Value);
                    if (custCategoryTextBox.Text == "FCB FY-23-24")
                    {
                        //2
                        if (!string.IsNullOrEmpty(SubmissionDate))
                        {
                            DataTable dttradepolicy = aOrderInfoBll.GetTradeTerm(totalprice.ToString(), CustTypeId, PaymentType, SubmissionDate);
                            if (dttradepolicy.Rows.Count > 0)
                            {
                                percentage = 0;
                            }
                        }
                    }
                    else
                    {
                        //1
                        if (!string.IsNullOrEmpty(SubmissionDate))
                        {
                            DataTable dttradepolicy = aOrderInfoBll.GetTradeTerm(totalprice.ToString(), CustTypeId, PaymentType, SubmissionDate);
                            if (dttradepolicy.Rows.Count > 0)
                            {
                                if (dttradepolicy.Columns.Contains("DiscountPerc") && dttradepolicy.Rows[0]["DiscountPerc"] != DBNull.Value)
                                    decimal.TryParse(dttradepolicy.Rows[0]["DiscountPerc"].ToString(), out percentage);
                            }
                        }
                    }
                }
                // Frinds Hospital end

             

                // Campaing 

                decimal totaldiscount = 0;
                //  totaldiscount = (percentage * Convert.ToDecimal(tpTptalTextBox.Text)) / 100;
                disTotalTextBox.Text = totaldiscount.ToString();
                for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
                {
                    // ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text = ((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("IsCampaignProduct")).Text;
                    decimal cstock = 0;
                    decimal tqty = 0;
                    TextBox txtTqty = (TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox");
                    if (txtTqty != null && !string.IsNullOrEmpty(txtTqty.Text))
                        decimal.TryParse(txtTqty.Text, out tqty);

                    TextBox txtStock = (TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox");
                    if (txtStock != null && !string.IsNullOrEmpty(txtStock.Text))
                        decimal.TryParse(txtStock.Text, out cstock);

                    DataTable dtdata = aInvoiceBll.LoadProductQty(orderIdHiddenField.Value,
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);
                    if (dtdata.Rows.Count > 0 && dtdata.Rows[0][0] != DBNull.Value)
                    {
                        decimal.TryParse(dtdata.Rows[0][0].ToString(), out tqty);
                    }
                    if (cstock < tqty)
                    {
                        gridLineItemGridView.Rows[i].BackColor = Color.Red;
                        /////////SMC Low Order Method//////////
                        //if (cstock != 0)
                        {
                            // showMessageBox("Stock Not Avaialable");
                            // saveButton.Visible = false;
                            GetQty(cstock, i);
                        }
                        ///////////////////////
                    }
                    //////// SMC Low Stock Method////////
                    //else
                    /////////////////
                    {
                        DataTable dtproductvat =
                            aOrderInfoBll.ProductVat(
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);
                        DataTable dtdiscount =
                           aOrderInfoBll.ProductDiscount(
                               ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text,
                               hdCustomerMasterId.Value, invDateTextBox.Text);
                        decimal percamount = 0;
                        if (dtdiscount.Rows.Count > 0)
                        {
                            percamount = Convert.ToDecimal(dtdiscount.Rows[0]["DiscountPercentage"].ToString());
                        }
                        decimal totalamount = 0;
                        totalamount = Convert.ToDecimal(tpTptalTextBox.Text);
                        decimal productamount = 0;
                        productamount =
                            Convert.ToDecimal(
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim());
                        decimal productperc = 0;
                        //productperc = (productamount*100)/totalamount;
                        decimal mainper = 0;
                        //  mainper = (percentage * productperc) / 100;


                        // Frinds Hospital Start
                       // if (aTable.Rows[0]["CustomerCode"].ToString() == "158964" || aTable.Rows[0]["CustomerCode"].ToString() == "158399")
                        bool isSpDis = aTable.Columns.Contains("IsSpDis") && aTable.Rows[0]["IsSpDis"] != DBNull.Value && (aTable.Rows[0]["IsSpDis"].ToString() == "1" || aTable.Rows[0]["IsSpDis"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase));
                        if (isSpDis)
                        {
                            DataTable aTable2 = new DataTable();
                            aTable2 = aOrderInfoBll.LoadOrderWithDetailFrindsHospital(orderId, ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value);
                            if (aTable2 != null && aTable2.Rows.Count > 0 && aTable2.Rows[0]["DiscountAmount"] != DBNull.Value)
                            {
                                decimal disAmt = 0;
                                decimal.TryParse(aTable2.Rows[0]["DiscountAmount"].ToString(), out disAmt);
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text = disAmt.ToString();
                            }
                        }
                        

                        else
                        {
                            if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("sdTextBox")).Text == "1")
                            {

                                ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                              0.ToString();
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                   0.ToString("F");
                            }
                              if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("bQtyTextBox")).Text == "True")
                            {

                                ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                              0.ToString();
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                   0.ToString("F");

                                ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("tpTextBox")).Text =
                          0.ToString();

                                ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("tpVatTextBox")).Text =
                          0.ToString();

                                ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("npTextBox")).Text =
                          0.ToString();
                            }
                              if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("sdTextBox")).Text != "1"
                                  && ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("bQtyTextBox")).Text != "True")
                            {
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                   percentage.ToString();
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                    (Convert.ToDecimal(
                                        ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                     (percentage / 100)).ToString("F");
                            }
                        }

                        if (aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 500]-Feb-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                        if (aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 500]-Feb-24 ")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                        if (aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 1g IM]-Feb-24 ")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                        if (aTable.Rows[i]["CampaignName"].ToString() == "Special Rate [Triforce 1g IM]-Feb-24 ")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                        if (aTable.Rows[i]["CampaignName"].ToString() == "Bonus Campaign | Special Rate [Triforce 1g IM]-Feb-24 ")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }


                        ///---------------------------------
                        ///



                        if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Triforce 1g IM]-Feb-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }

                        if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Aminobost]-Apr-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Aminobost]-Apr-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                            if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Triforce 1g IV]-Apr-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                              if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Bonus Campaign | Special Rate [Triforce 1g IV]-Apr-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                                     if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Triforce 1g IV]-Apr-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
                                        if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Aminobost]-Apr-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
       if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Triforce 1g IM]-Feb-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
          if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Special Rate [Triforce 500]-Feb-24")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text =
                                    aTable.Rows[i]["DiscountPercent"].ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text =
                                (Convert.ToDecimal(
                                    ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) *
                                 (Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text) / 100)).ToString("F");
                        }
          if (aTable.Rows[i]["CampaignName"].ToString().Trim() == "Ezevent Flat Rate Campaign | Dec-25"  ||  aTable.Rows[i]["CampaignName"].ToString().Trim() == "Esomium 20 Flat Rate Campaign | Mar-26" || aTable.Rows[i]["CampaignName"].ToString().Trim() == "Seacoral D Flat Rate Campaign | Apr-26")
                        {

                            // tpTextBox = মূল দাম (Total Price)
                            // dpAmtTextBox = ডিসকাউন্ট এমাউন্ট
                            // dpTextBox = ডিসকাউন্ট পারসেন্টেজ

                            var tpTxt = (TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("tpTextBox");
                            var dpAmtTxt = (TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpAmtTextBox");
                            var dpTxt = (TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox");

                            decimal tp = 0m, dpAmt = 0m;

                            decimal.TryParse(tpTxt.Text.Trim(), out tp);
                            decimal.TryParse(aTable.Rows[i]["DiscountAmount"].ToString().Trim(), out dpAmt);

                            decimal dpPercent = 0m;
                            if (tp > 0)
                            {
                                dpPercent = (dpAmt / tp) * 100m;
                            }

                            // tpTextBox = মূল দাম
                            tpTxt.Text = tp.ToString("0.00");

                            // dpAmtTextBox = ডিসকাউন্ট
                            dpAmtTxt.Text = dpAmt.ToString("0.00");

                            // dpTextBox = ডিসকাউন্ট শতাংশ
                            dpTxt.Text = dpPercent.ToString("0.00");

                        }

                        ////---------------------
                        // Frinds Hospital End

                        // Resectin FOC Offer Discount End//

                        //////////Modified Version/////////////
                        //decimal vat = 0;
                        //vat = Convert.ToDecimal(dtproductvat.Rows[0]["VATPercentage"].ToString());
                        //((TextBox) gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text =
                        //    (Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text)*
                        //     (percamount/100)).ToString();

                        decimal withdiscount = 0;
                        withdiscount =
                            (Convert.ToDecimal(
                                ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim()) -
                             Convert.ToDecimal(
                                 (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text)));
                        //-
                        // Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text));
                        decimal vatamount = 0;
                        //vatamount = (Convert.ToDecimal(
                        //    ((TextBox) gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim())*vat)/100;
                        TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox");
                        //tpVatTextBox.Text = vatamount.ToString("F");
                        //((TextBox) gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text =
                        //    (Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text)*
                        //     (percamount/100)).ToString();

                        //amTextBox
                        //ddddddddddddd
                        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");


                        

                        //AdjustmentAmount();

                        decimal tpTextBox = 0;
                        try
                        {
                            tpTextBox = Convert.ToDecimal(
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim());
                        }
                        catch
                        {

                        }
                        decimal dpAmtTextBox = 0;
                        try
                        {

                            dpAmtTextBox = Convert.ToDecimal(
                                               ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox"))
                                                   .Text.Trim());

                        }
                        catch
                        {

                        }

                        decimal amTextBox = 0;
                        try
                        {

                            amTextBox = Convert.ToDecimal(
                                               ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("amTextBox"))
                                                   .Text.Trim());

                        }
                        catch
                        {

                        }

                        decimal tpVat = 0;
                        try
                        {

                            tpVat = Convert.ToDecimal(tpVatTextBox.Text);

                        }
                        catch
                        {

                        }

                       decimal rS = (tpTextBox - (dpAmtTextBox + amTextBox)) + tpVat;
                        npTextBox.Text = rS.ToString();

                        if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("sdTextBox")).Text == "True"

                            && ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("bQtyTextBox")).Text == "True")
                        {

                            ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("npTextBox")).Text = 0.ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("tpVatTextBox")).Text = 0.ToString();
                            ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("tpTextBox")).Text = 0.ToString();
                            //(TextBox) gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox").te = 0;
                        }

                        TotalValueCalculation();
                    }
                }
                GetDiscounttotalValue();
            }
            if (gridLineItemGridView.Rows.Count == aTable.Rows.Count)
            {
                
            }
            else
            {
                Response.Redirect("InvoiceCreationByOrder.aspx");

            }
        }
        else
        {
            showMessageBox("Order Information already Exists !!");
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
            tqty = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text);

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

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string custCode = custCodeTextBox.Text.Trim();
        GetCustInfo(custCode,orderNoTextBox.Text.Trim());
    }

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
        aDataTable.Columns.Add("UnitVAT");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");
        aDataTable.Columns.Add("NetPrice");
        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("ISGiftProduct");
        aDataTable.Columns.Add("IsCampaignProduct");
        aDataTable.Columns.Add("CampaignType");
        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["SpecialAmount"] = "";
        dataRow["OrderDetailsId"] = "";
        dataRow["StockQty"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["UnitVAT"] = "";
        dataRow["Quantity"] = "";
        dataRow["TotalPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["DiscountPercentage"] = "";
        dataRow["DiscountAmount"] = "";
        dataRow["NetPrice"] = "";
        dataRow["BonusQty"] = "";
        dataRow["TotalQty"] = "";
        dataRow["ISGiftProduct"] = "";
        dataRow["IsCampaignProduct"] = "";
        dataRow["CampaignType"] = "";
        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();

        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;

    }

    public void AddFunc()
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
        aDataTable.Columns.Add("SpecialAmount");
       
        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("ISGiftProduct");
        aDataTable.Columns.Add("IsCampaignProduct");
        aDataTable.Columns.Add("CampaignType");
        
        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value.Trim();
                dataRow["IsCampaignProduct"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim();
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
               // dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                dataRow["ISGiftProduct"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();

                dataRow["CampaignType"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("CampaignTypeHiddenField")).Value.Trim();
                    //((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }
        }

        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "";
        dataRow["OrderDetailsId"] = "";
        dataRow["ProductName"] = "";
        dataRow["StockQty"] = "";
        dataRow["UnitPrice"] = "";
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
        dataRow["ISGiftProduct"] = "";
        dataRow["IsCampaignProduct"] = "";
        dataRow["CampaignType"] = "";
        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
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
                aDataTable = aInvoiceBll.ProductInfo(hdComUnitId.Value, productCode);
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
                        (TextBox)gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
                    unitPriceTextBox.Text = aDataTable.Rows[0]["UnitPrice"].ToString();
                    TextBox upVatTextBox =
                        (TextBox)gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");
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
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    public void GetQty(decimal qty, int rowindex)
    {

        TextBox qtyTextBox1 = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[6].FindControl("qtyTextBox");
        qtyTextBox1.Text = qty.ToString();

        TextBox unitPriceTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
        TextBox upVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");

        TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[7].FindControl("tpTextBox");
        tpTextBox.Text = Convert.ToString(Convert.ToDecimal(unitPriceTextBox.Text.Trim()) * qty);

        TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[8].FindControl("tpVatTextBox");
        tpVatTextBox.Text = Convert.ToString(Convert.ToDecimal(upVatTextBox.Text.Trim()) * qty);

        TextBox codeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

        decimal discountPer = 0;
        //discountPer = aInvoiceBll.ProductDiscount(codeTextBox.Text.Trim(), qtyTextBox1.Text.Trim());

        TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[9].FindControl("dpTextBox");
        dpTextBox.Text = Convert.ToString(discountPer);
        TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[10].FindControl("dpAmtTextBox");
        if (discountPer == 0)
        {
            dpAmtTextBox.Text = "0";
        }
        else
        {
            dpAmtTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) / 100) * discountPer);
        }

        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[11].FindControl("npTextBox");
        npTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) - Convert.ToDecimal(dpAmtTextBox.Text.Trim())) + Convert.ToDecimal(tpVatTextBox.Text.Trim()));

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text = qtyTextBox1.Text.Trim();
        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";

        TotalValueCalculation();
    }
    protected void qtyTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox qtyTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        decimal qty = 0;
        TextBox qtyTextBox1 = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[6].FindControl("qtyTextBox");
        qty = Convert.ToDecimal(qtyTextBox1.Text.Trim());

        TextBox unitPriceTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
        TextBox upVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");

        TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[7].FindControl("tpTextBox");
        tpTextBox.Text = Convert.ToString(Convert.ToDecimal(unitPriceTextBox.Text.Trim()) * qty);

        TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[8].FindControl("tpVatTextBox");
        tpVatTextBox.Text = Convert.ToString(Convert.ToDecimal(upVatTextBox.Text.Trim()) * qty);

        TextBox codeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

        decimal discountPer = 0;
        discountPer = aInvoiceBll.ProductDiscount(codeTextBox.Text.Trim(), qtyTextBox1.Text.Trim());

        TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[9].FindControl("dpTextBox");
        dpTextBox.Text = Convert.ToString(discountPer);
        TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[10].FindControl("dpAmtTextBox");
        if (discountPer == 0)
        {
            dpAmtTextBox.Text = "0";
        }
        else
        {
            dpAmtTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) / 100) * discountPer);
        }

        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[11].FindControl("npTextBox");
        npTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) - Convert.ToDecimal(dpAmtTextBox.Text.Trim())) + Convert.ToDecimal(tpVatTextBox.Text.Trim()));

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text = qtyTextBox1.Text.Trim();
        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        TotalValueCalculation();
    }

    protected void dpTextBox_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox dpBox = (TextBox)sender;
            GridViewRow currentRow = (GridViewRow)dpBox.Parent.Parent;
            int rowindex = currentRow.RowIndex;

            decimal dp = 0;
            decimal.TryParse(dpBox.Text.Trim(), out dp);

            TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("tpTextBox");
            TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("dpAmtTextBox");
            TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("tpVatTextBox");
            TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("npTextBox");

            decimal tp = 0;
            if (tpTextBox != null) decimal.TryParse(tpTextBox.Text.Trim(), out tp);

            decimal vat = 0;
            if (tpVatTextBox != null) decimal.TryParse(tpVatTextBox.Text.Trim(), out vat);

            decimal dpAmt = (tp * dp) / 100;
            if (dpAmtTextBox != null)
            {
                dpAmtTextBox.Text = dpAmt.ToString("F2");
            }

            if (npTextBox != null)
            {
                decimal np = (tp - dpAmt) + vat;
                npTextBox.Text = np.ToString("F2");
            }

            TotalValueCalculation();
        }
        catch { }
    }

    protected void dpAmtTextBox_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox dpAmtBox = (TextBox)sender;
            GridViewRow currentRow = (GridViewRow)dpAmtBox.Parent.Parent;
            int rowindex = currentRow.RowIndex;

            decimal dpAmt = 0;
            decimal.TryParse(dpAmtBox.Text.Trim(), out dpAmt);

            TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("tpTextBox");
            TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("dpTextBox");
            TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("tpVatTextBox");
            TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].FindControl("npTextBox");

            decimal tp = 0;
            if (tpTextBox != null) decimal.TryParse(tpTextBox.Text.Trim(), out tp);

            decimal vat = 0;
            if (tpVatTextBox != null) decimal.TryParse(tpVatTextBox.Text.Trim(), out vat);

            decimal dp = 0;
            if (tp > 0)
            {
                dp = (dpAmt / tp) * 100;
            }

            if (dpTextBox != null)
            {
                dpTextBox.Text = dp.ToString("F2");
            }

            if (npTextBox != null)
            {
                decimal np = (tp - dpAmt) + vat;
                npTextBox.Text = np.ToString("F2");
            }

            TotalValueCalculation();
        }
        catch { }
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
    public void SaveReturnAmount(int invid)
    {
        ReturnAmountDAO amountDao = new ReturnAmountDAO()
        {
            CustomerId = Convert.ToInt32(hdCustomerMasterId.Value),
            Amount = Convert.ToDecimal(crAmountTextBox.Text)*(-1),
          //  ReturnInvoiceId = invid,
            InvoiceId = Convert.ToInt32(invid)
        };
        bool status = aInvoiceBll.SaveDataForReturnAmount(amountDao);

    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        if (ValiMethod() == true)
        {

        if (Validation() == true)
        {
            if (OrderExists(orderHiddenField.Value) == false)
            {
                {
                    try
                    {
                        int invId = SaveInvoice();
                        if (invId > 0)
                        {
                                ProformaOrInvoiceReturnBLL aInvoiceReturnBll2 = new ProformaOrInvoiceReturnBLL();
                                DataTable invoiceid = aInvoiceReturnBll2.SelectInvoiceID(Convert.ToInt32(orderHiddenField.Value));
                                int invoicePK = Convert.ToInt32(invoiceid.Rows[0]["InvoiceId"]);

                                bool dtl = SaveInvoiceDetail(invoicePK);
                            if (dtl == true)
                            {
                                aInvoiceBll.UpdateInvoiceHeaderTotals(invoicePK);
                                SaveReturnAmount(invoicePK);
                                aOrderInfoBll.UpdateInvoiceStatus(orderIdHiddenField.Value);

                                    //if (ValiMethodAfterSave())
                                    //{
                                    //}
                                    //else
                                    //{
                                    //    ProformaOrInvoiceReturnBLL aInvoiceReturnBll = new ProformaOrInvoiceReturnBLL();
                                    //    aInvoiceReturnBll.DeleteProforma(invTextBox.Text.Trim());
                                    //    //aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
                                    //    //Update return amt
                                    //    // update order status
                                    //    Clear();
                                    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Please generate Invoice again!" + "','Faild');", true);
                                    //}
                                }
                            else
                            {
                                //aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
                                Clear();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Please generate Invoice again!" + "','Faild');", true);

                                }
                        }
                            ProformaOrInvoiceReturnBLL aIn = new ProformaOrInvoiceReturnBLL();
                            DataTable dtInvo = aOrderInfoBll.GetInvoNOGetByInvoID(orderIdHiddenField.Value.ToString());


                            if (dtInvo.Rows.Count > 0)
                            {
                                if (dtInvo.Rows[0]["InvoiceId"].ToString() != "")
                                {
                                    DataTable dtMarket = _dataLoad.Check_anomalyInvoiceDetails(dtInvo.Rows[0]["InvoiceId"].ToString(), orderIdHiddenField.Value.ToString());

                                    if (dtMarket.Rows.Count > 0)
                                    {
                                      //aIn.DeleteProforma(dtInvo.Rows[0]["InvoiceNo"].ToString().Trim());
                                    }

                                }
                            }
                            Clear();

                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "ShowSuccesalert('" + "Invoice Created Successsfully!" + "','Success');", true);


                        }
                    catch (Exception ex)
                    {
                        aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
                        string errorMsg = ex.Message.Replace("'", " ");
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('Invoice generation failed: " + errorMsg + "','Faild');", true);
                    }
                }
            }
            else
            {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Invoice already Generated !!" + "','Faild');", true);
                   
            }
        }

        }
        else
        {
            Response.Redirect("InvoiceCreationByOrder.aspx"); 
        }
    }

    private bool ValiMethod()
    {

        bool chk = true;
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        string OrderId = orderHiddenField.Value.ToString();
        int cont = 0;
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string OrderDtlsId =
                    ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
            DataTable   aTable = aOrderInfoBll.LoadOrderWithDetailIDCheck(OrderId, OrderDtlsId);

            if(aTable.Rows.Count==0)
            {
                 
                chk = false;
                break;

            }
        }

        //if (cont == 0)
        //{
        //    chk = false;
        //}

        return chk;
    }


    private bool ValiMethodAfterSave()
    {

        bool chk = true;
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        string OrderId = orderHiddenField.Value.ToString();
        int cont = 0;
        DataTable dtOrder = aOrderInfoBll.LoadDetalIdByMasCheck(OrderId);

        for (int i = 0; i < dtOrder.Rows.Count; i++)
        {
       int OrderDtlsId = 0;
            try
            {
                OrderDtlsId = Convert.ToInt32(dtOrder.Rows[i]["OrderDetailsId"].ToString());
            }
            catch
            {
                chk = false;
                break;
            }
            DataTable aTable = aOrderInfoBll.LoadInvoiceWithDetailIDCheck(OrderId, OrderDtlsId);

            if (aTable.Rows.Count == 0)
            {

                chk = false;
                break;

            }
        }

        //if (cont == 0)
        //{
        //    chk = false;
        //}

        return chk;
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
                    hdCustomerMasterId.Value, invDateTextBox.Text);

                InvoiceDetail aInvoiceDetail = new InvoiceDetail();
                aInvoiceDetail.ProductCode =
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                string product = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text;
                if (!string.IsNullOrEmpty(product) && product.Contains(':'))
                {
                    string[] proNameAndPackSize = product.Split(':');
                    aInvoiceDetail.ProductName = proNameAndPackSize[0].Trim();
                    aInvoiceDetail.PackSize = proNameAndPackSize.Length > 1 ? proNameAndPackSize[1].Trim() : "";
                }
                else
                {
                    aInvoiceDetail.ProductName = (product ?? "").Trim();
                    aInvoiceDetail.PackSize = "";
                }

                decimal uPrice = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim(), out uPrice);
                aInvoiceDetail.UnitPrice = uPrice;

                decimal uVat = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(), out uVat);
                aInvoiceDetail.UnitVatAmount = uVat;

                decimal qVal = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim(), out qVal);
                aInvoiceDetail.Quantity = qVal;

                decimal dPerc = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim(), out dPerc);
                aInvoiceDetail.DiscountPercentage = dPerc;

                decimal dAmt = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim(), out dAmt);
                aInvoiceDetail.DiscountAmount = dAmt;

                aInvoiceDetail.BonusQuantity = 0;
                aInvoiceDetail.SpecialAmount = 0;
                
                try
                {
                    aInvoiceDetail.IsgiftProduct = gridLineItemGridView.DataKeys[i][0] != null ? gridLineItemGridView.DataKeys[i][0].ToString() : "0";
                }
                catch
                {
                    aInvoiceDetail.IsgiftProduct = "0";
                }

                string id =
                    ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                int ordDtlId = 0;
                int.TryParse(id, out ordDtlId);
                aInvoiceDetail.OrderDetailsId = ordDtlId;
                aInvoiceDetail.InvoiceId = invoiceId;

                decimal tpVatVal = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim(), out tpVatVal);
                aInvoiceDetail.TotalPriceVatAmount = tpVatVal;

                TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");
                decimal npVal = 0;
                decimal.TryParse(npTextBox != null ? npTextBox.Text.Trim() : "0", out npVal);
                aInvoiceDetail.NetAmount = npVal;

                if (dtdiscount != null && dtdiscount.Rows.Count > 0)
                {
                    decimal spVal = 0;
                    decimal.TryParse(dtdiscount.Rows[0]["DiscountPercentage"].ToString(), out spVal);
                    aInvoiceDetail.SpecialAmountPer = spVal;
                }
                else
                {
                    aInvoiceDetail.SpecialAmountPer = 0;
                }

                try
                {
                    decimal amVal = 0;
                    decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("amTextBox")).Text.Trim(), out amVal);
                    aInvoiceDetail.AdjustmentAmount = amVal;
                }
                catch(Exception ex)
                {
                    aInvoiceDetail.AdjustmentAmount = 0;
                }

                decimal cstock = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim(), out cstock);

                decimal tqty = 0;
                decimal.TryParse(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text.Trim(), out tqty);
                try
                {
                    DataTable dtdata = aInvoiceBll.LoadProductQty(orderIdHiddenField.Value,
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        decimal.TryParse(dtdata.Rows[0][0].ToString(), out tqty);
                    }
                }
                catch { }

                try
                {
                    aInvoiceDetail.CampaignType = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("CampaignTypeHiddenField")).Value.Trim();
                }
                catch
                {
                    aInvoiceDetail.CampaignType = "";
                }

                try
                {
                    aInvoiceDetail.IsCampaignProductforInv =
                        Convert.ToBoolean(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim());
                }
                catch (Exception)
                {
                    aInvoiceDetail.IsCampaignProductforInv = false;
                }

                try
                {
                    aInvoiceDetail.ISGiftProductforInv =
                        Convert.ToBoolean(((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim());
                }
                catch (Exception)
                {
                    aInvoiceDetail.ISGiftProductforInv = false;
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
        int invoiceId = 0;
        DataTable aDataTable = new DataTable();
        aDataTable = aInvoiceBll.CustomerMaster(orderNoTextBox.Text.Trim());

        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable dtFixedCustomer = aOrderInfoBll.GetFixedCustomer(orderHiddenField.Value);

        string invoiceNo = string.Empty;
        string ComUnitCode = "";
        if (!string.IsNullOrEmpty(comUnitNameTextBox.Text))
        {
            string[] forComUCode = comUnitNameTextBox.Text.Split(':');
            ComUnitCode = forComUCode[0].Trim();
        }

        Invoice aInvoice = new Invoice();
        
        DateTime invDate;
        if (!DateTime.TryParse(invDateTextBox.Text.Trim(), out invDate))
            invDate = DateTime.Today;
        aInvoice.InvoiceDate = invDate;
        aInvoice.OrderNo = orderNoTextBox.Text.Trim();

        DateTime ordDate;
        if (!DateTime.TryParse(orderDateTextBox.Text.Trim(), out ordDate))
            ordDate = DateTime.Today;
        aInvoice.OrderDate = ordDate;

        int custMasterId = 0;
        int.TryParse(hdCustomerMasterId.Value, out custMasterId);
        aInvoice.CustomerMasterId = custMasterId;

        int comUnitId = 0;
        int.TryParse(hdComUnitId.Value, out comUnitId);
        aInvoice.ComUnitId = comUnitId;

        int miaId = 0;
        int.TryParse(hdMiaId.Value, out miaId);
        aInvoice.MiaId = miaId;

        int payTypeId = 1;
        int.TryParse(payTypeDDL.SelectedValue, out payTypeId);
        aInvoice.PaymentTypeId = payTypeId;

        decimal tpTotal = 0;
        decimal.TryParse(tpTptalTextBox.Text.Trim(), out tpTotal);
        aInvoice.TpTotal = tpTotal;

        decimal disTotal = 0;
        decimal.TryParse(disTotalTextBox.Text.Trim(), out disTotal);
        aInvoice.TpDiscount = disTotal;

        decimal vatTotal = 0;
        decimal.TryParse(vatTotalTextBox.Text.Trim(), out vatTotal);
        aInvoice.TpVat = vatTotal;

        decimal grandTotal = 0;
        decimal.TryParse(grandTotalTextBox.Text.Trim(), out grandTotal);
        aInvoice.TpGrandTotal = grandTotal;

        int userId = 0;
        if (Session["UserId"] != null)
        {
            int.TryParse(Session["UserId"].ToString(), out userId);
        }
        aInvoice.UserId = userId;
        aInvoice.ComUnitCode = ComUnitCode;

        int ordId = 0;
        int.TryParse(orderIdHiddenField.Value, out ordId);
        aInvoice.OrderId = ordId;

        int daNameId = 0;
        int.TryParse(ddlDAName.SelectedValue, out daNameId);
        aInvoice.Inv_DANameId = daNameId;

        decimal pdVal = 0;
        decimal.TryParse(pdTextBox.Text.Trim(), out pdVal);
        aInvoice.TotalSpecialAmount = pdVal;

        aInvoice.cusType = cusTypeTextBox.Text;
        aInvoice.OldTradePolicy = false;
        aInvoice.Remarks = remarksTextBox.Text;
        aInvoice.CustomerDeliveryAddress = custDeliveryAddressTextBox.Text.Trim();

        if (aDataTable != null && aDataTable.Rows.Count > 0)
        {
            aInvoice.MIACode = aDataTable.Rows[0]["MIOEmpMastercode"] != null ? aDataTable.Rows[0]["MIOEmpMastercode"].ToString() : "";
            aInvoice.MIAName = aDataTable.Rows[0]["MIOEmpName"] != null ? aDataTable.Rows[0]["MIOEmpName"].ToString() : "";
            aInvoice.MarketCode = aDataTable.Rows[0]["MarketCode"] != null ? aDataTable.Rows[0]["MarketCode"].ToString() : "";
            aInvoice.MarketName = aDataTable.Rows[0]["MarketName"] != null ? aDataTable.Rows[0]["MarketName"].ToString() : "";
            aInvoice.AreaCode = aDataTable.Rows[0]["AreaCode"] != null ? aDataTable.Rows[0]["AreaCode"].ToString() : "";
            aInvoice.DisCode = aDataTable.Rows[0]["ASMEmpMasterCode"] != null ? aDataTable.Rows[0]["ASMEmpMasterCode"].ToString() : "";
            aInvoice.FEName = aDataTable.Rows[0]["ASMEmpName"] != null ? aDataTable.Rows[0]["ASMEmpName"].ToString() : "";
            aInvoice.RegionCode = aDataTable.Rows[0]["RegionCode"] != null ? aDataTable.Rows[0]["RegionCode"].ToString() : "";
            aInvoice.DZSMName = aDataTable.Rows[0]["RSMEmpName"] != null ? aDataTable.Rows[0]["RSMEmpName"].ToString() : "";
            aInvoice.Type = aDataTable.Rows[0]["Type"] != null ? aDataTable.Rows[0]["Type"].ToString() : "";
        }

        bool isFixed = false;
        if (dtFixedCustomer != null && dtFixedCustomer.Rows.Count > 0 && dtFixedCustomer.Rows[0]["FixedCustomer"] != DBNull.Value)
        {
            bool.TryParse(dtFixedCustomer.Rows[0]["FixedCustomer"].ToString(), out isFixed);
        }
        aInvoice.FixedCustomer = isFixed;

        aInvoice.DpNAme = deliverypersonNameTextBox.Text.Trim();
        aInvoice.DpMob = deliverypersonMobileTextBox.Text.Trim();
        aInvoice.ProductOffer = "False";
        aInvoice.Createdate = DateTime.Now;

        decimal delCharge = 0;
        decimal.TryParse(deliveryChargeTextBox.Text.Trim(), out delCharge);
        aInvoice.DeliveryCharge = delCharge;

        decimal crAmt = 0;
        decimal.TryParse(crAmountTextBox.Text.Trim(), out crAmt);
        aInvoice.AdjustAmount = crAmt;

        decimal rcvAmt = 0;
        decimal.TryParse(rcvAmountTextBox.Text.Trim(), out rcvAmt);
        aInvoice.ReceivableAmount = rcvAmt;

        if (crAmt > 0)
        {
            aInvoice.IsAdjustInvoice = true;
            aInvoice.AdjustInvoiceNo_ReturnInvoiceNo = adjustInvoiceNoTextBox.Text.Trim();
        }
        else
        {
            aInvoice.IsAdjustInvoice = false;
        }

        aInvoiceBll.SaveInvoice(aInvoice, out invoiceId, out invoiceNo);
        invTextBox.Text = invoiceNo;

        return invoiceId;
    }
    private void Clear()
    {
        tpTptalTextBox.Text = "";
        vatTotalTextBox.Text = "";
        disTotalTextBox.Text = "";
        deliveryChargeTextBox.Text = "";
        grandTotalTextBox.Text = "";
        hdComUnitId.Value = "";
        hdCustomerMasterId.Value = "";
        custNameTextBox.Text = "";
        custAddressTextBox.Text = "";
        districtNameTextBox.Text = "";
        areaNameTextBox.Text = "";
        comUnitNameTextBox.Text = "";
        miaCodeTextBox.Text = "";
        marketNameTextBox.Text = "";
        miaNameTextBox.Text = "";
        custCategoryTextBox.Text = "";
        hdMiaId.Value = "";
        custCodeTextBox.Text = "";
        orderNoTextBox.Text = "";
        pdTextBox.Text = "";
        orderHiddenField.Value = "";
        try
        {
            InitialGrid();
        }
        catch
        {

        }
    }

    protected void deliveryChargeTextBox_TextChanged(object sender, EventArgs e)
    {
        TotalValueCalculation();
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
                decimal cstock = 0;
                decimal tqty = 0;
                tqty = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text);
                cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text);
                DataTable dtdata = aInvoiceBll.LoadProductQty(orderIdHiddenField.Value,
                ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);
                /////SMC Low Stock Method/////
                //tqty = Convert.ToDecimal(dtdata.Rows[0][0].ToString());
                /////////////
                if (cstock >= tqty)
                {
                    tpTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text) : 0;
                    vatTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text) : 0;
                    //Offer Applied
                    //if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "ANB08" && tqty >= 3)
                    //{
                    //    disTotal = 0;
                    //    //(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;

                    //}
                    //else
                    {
                        disTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;

                    }

                    gTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text) : 0;
                  //  sptotatl += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text) : 0;
                }
            }
        }

        decimal deliveryCharge = 0;
        if (!string.IsNullOrEmpty(deliveryChargeTextBox.Text))
        {
            decimal.TryParse(deliveryChargeTextBox.Text.Trim(), out deliveryCharge);
        }
        gTotal += deliveryCharge;

        tpTptalTextBox.Text = tpTotal.ToString("0.00");
        vatTotalTextBox.Text = vatTotal.ToString("0.00");
        disTotalTextBox.Text = disTotal.ToString("0.00");
        grandTotalTextBox.Text = gTotal.ToString("0.00");
        pdTextBox.Text = sptotatl.ToString("0.00");

        decimal crAmt = 0;
        decimal.TryParse(crAmountTextBox.Text.Trim(), out crAmt);
        rcvAmountTextBox.Text = (gTotal - crAmt).ToString("0.00");

    }

    protected void printButton_Click(object sender, EventArgs e)
    {
        if (invTextBox.Text == "")
        {
            string text6 = "Please Enter Invoice NO!";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showpop6('" + text6 + "')</script>", false);
            invTextBox.CssClass = "form-control form-control-sm is-invalid";
            invTextBox.Focus();
        }
        else
        {

            ProformaOrInvoiceReturnBLL aIn = new ProformaOrInvoiceReturnBLL();
            DataTable dtInvo = aOrderInfoBll.GetInvoNOGetByInvoNo(invTextBox.Text.Trim());

            if (dtInvo.Rows.Count > 0)
            {
                if (dtInvo.Rows[0]["InvoiceId"].ToString() != "")
                {
                    DataTable dtMarket = _dataLoad.Check_anomalyInvoiceDetails(dtInvo.Rows[0]["InvoiceId"].ToString(), dtInvo.Rows[0]["OrderId"].ToString().ToString());

                    if (dtMarket.Rows.Count > 0)
                    {
                        aIn.DeleteProforma(dtInvo.Rows[0]["InvoiceNo"].ToString().Trim());
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Please generate Invoice again!" + "','Faild');", true);
                    }

                    else
                    {
                        string url = "../SInventory_RPTVIEW/ProformaReportPrintViewer.aspx?InvNo=" + Server.UrlEncode(dtInvo.Rows[0]["InvoiceNo"].ToString().Trim());
                        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
                        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                    }

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Invoice Number Not Found!" + "','Faild');", true);
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
                custCodeTextBox.Text = custInfo[0];
                string custCode = custCodeTextBox.Text.Trim();
                GetCustInfo(custCode,orderNoTextBox.Text.Trim());
            }
        }
    }
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