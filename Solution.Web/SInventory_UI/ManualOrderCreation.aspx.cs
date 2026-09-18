using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;


public partial class SInventory_UI_ManualOrderCreation : System.Web.UI.Page
{
    OrderListBLL aOrderListBLL = new OrderListBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownLoad();
            Todate();

            if (!string.IsNullOrEmpty(Request.QueryString["OrderId"]) || !string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                string orderId = !string.IsNullOrEmpty(Request.QueryString["OrderId"]) ? Request.QueryString["OrderId"] : Request.QueryString["ID"];
                hfOrderId.Value = orderId;
                LoadOrderForEdit(orderId);
            }
            else
            {
                InitialGrid();
                OrdNo();
            }
        }
    }

    private void LoadOrderForEdit(string orderId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable dt = aOrderInfoBll.LoadOrderWithDetail(orderId);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow firstRow = dt.Rows[0];

            bool isInvoice = dt.Columns.Contains("IsInvoice") && firstRow["IsInvoice"] != DBNull.Value &&
                (firstRow["IsInvoice"].ToString() == "1" || firstRow["IsInvoice"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase));
            if (isInvoice)
            {
                showMessageBox("Warning: Invoice has ALREADY been generated for this Order (" + (firstRow["OrderCode"] != DBNull.Value ? firstRow["OrderCode"].ToString() : "") + "). Editing is NOT allowed!");
                submitButton.Enabled = false;
                submitButton.CssClass = "btn btn-secondary px-4 shadow-sm fw-bold disabled";
                submitButton.Text = "<i class=\"bx bx-lock me-1\"></i> Order Locked (Invoiced)";
            }
            else
            {
                submitButton.Enabled = true;
                submitButton.CssClass = "btn btn-primary px-4 shadow-sm fw-bold";
                submitButton.Text = "<i class=\"bx bx-check-circle me-1\"></i> Update Order";
            }

            orderNoTextBox.Text = firstRow["OrderCode"] != DBNull.Value ? firstRow["OrderCode"].ToString() : "";

            DateTime subDate;
            if (firstRow["SubmissionDate"] != DBNull.Value && DateTime.TryParse(firstRow["SubmissionDate"].ToString(), out subDate))
            {
                orderDateTextBox.Text = subDate.ToString("dd MMMM, yyyy");
            }

            string custCode = dt.Columns.Contains("CustomerCode") && firstRow["CustomerCode"] != DBNull.Value ? firstRow["CustomerCode"].ToString() : "";
            custCodeTextBox.Text = custCode;
            CustomerInfo(custCode);

            // Select Sales Person if available
            string mioId = dt.Columns.Contains("MIOId") && firstRow["MIOId"] != DBNull.Value ? firstRow["MIOId"].ToString() : "";
            if (!string.IsNullOrEmpty(mioId) && ddlSalesPerson.Items.FindByValue(mioId) != null)
            {
                ddlSalesPerson.SelectedValue = mioId;
                int spId;
                if (int.TryParse(mioId, out spId))
                {
                    LoadDCBySalesPerson(spId);
                }
            }

            // Select DC if available
            string comUnitId = dt.Columns.Contains("ComUnitId") && firstRow["ComUnitId"] != DBNull.Value ? firstRow["ComUnitId"].ToString() : "";
            if (!string.IsNullOrEmpty(comUnitId) && dcDropDownList.Items.FindByValue(comUnitId) != null)
            {
                dcDropDownList.SelectedValue = comUnitId;
            }

            remarksTextBox.Text = dt.Columns.Contains("Remarks") && firstRow["Remarks"] != DBNull.Value ? firstRow["Remarks"].ToString() : "";

            // Populate Grid items
            DataTable aDataTable = new DataTable();
            aDataTable.Columns.Add("SL");
            aDataTable.Columns.Add("ProductCode");
            aDataTable.Columns.Add("ProductName");
            aDataTable.Columns.Add("PackSize");
            aDataTable.Columns.Add("Quantity");
            aDataTable.Columns.Add("ProductId");
            aDataTable.Columns.Add("UnitPrice");
            aDataTable.Columns.Add("VAT");
            aDataTable.Columns.Add("Totaltp");
            aDataTable.Columns.Add("TotaltpVat");
            aDataTable.Columns.Add("IsCampaignProductDropDownList");
            aDataTable.Columns.Add("IsGiftProductDropDownList");

            int sl = 1;
            foreach (DataRow row in dt.Rows)
            {
                if (row["ProductId"] != DBNull.Value && Convert.ToInt32(row["ProductId"]) > 0)
                {
                    DataRow dataRow = aDataTable.NewRow();
                    dataRow["SL"] = sl.ToString();
                    dataRow["ProductCode"] = row.Table.Columns.Contains("ProductCode") && row["ProductCode"] != DBNull.Value ? row["ProductCode"].ToString() : "";
                    dataRow["ProductName"] = row.Table.Columns.Contains("ProductName") && row["ProductName"] != DBNull.Value ? row["ProductName"].ToString() : "";
                    dataRow["PackSize"] = row.Table.Columns.Contains("PackSize") && row["PackSize"] != DBNull.Value ? row["PackSize"].ToString() : "";

                    decimal qty = row.Table.Columns.Contains("Quantity") && row["Quantity"] != DBNull.Value ? Convert.ToDecimal(row["Quantity"]) : 0;
                    decimal unitPrice = row.Table.Columns.Contains("TradePrice") && row["TradePrice"] != DBNull.Value ? Convert.ToDecimal(row["TradePrice"]) : 0;
                    decimal unitVat = row.Table.Columns.Contains("UnitVatAmount") && row["UnitVatAmount"] != DBNull.Value ? Convert.ToDecimal(row["UnitVatAmount"]) : 0;
                    decimal totalTp = row.Table.Columns.Contains("TotalTradePrice") && row["TotalTradePrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalTradePrice"]) : (qty * unitPrice);
                    decimal totalVat = row.Table.Columns.Contains("TotalVatAmount") && row["TotalVatAmount"] != DBNull.Value ? Convert.ToDecimal(row["TotalVatAmount"]) : (qty * unitVat);

                    dataRow["Quantity"] = qty.ToString("N0");
                    dataRow["ProductId"] = row["ProductId"].ToString();
                    dataRow["UnitPrice"] = unitPrice.ToString("F2");
                    dataRow["VAT"] = unitVat.ToString("F2");
                    dataRow["Totaltp"] = totalTp.ToString("F2");
                    dataRow["TotaltpVat"] = totalVat.ToString("F2");

                    bool isCamp = row.Table.Columns.Contains("IsCampaignProduct") && row["IsCampaignProduct"] != DBNull.Value && Convert.ToBoolean(row["IsCampaignProduct"]);
                    bool isGift = row.Table.Columns.Contains("ISGiftProduct") && row["ISGiftProduct"] != DBNull.Value && Convert.ToBoolean(row["ISGiftProduct"]);
                    dataRow["IsCampaignProductDropDownList"] = isCamp ? "True" : "False";
                    dataRow["IsGiftProductDropDownList"] = isGift ? "True" : "False";

                    aDataTable.Rows.Add(dataRow);
                    sl++;
                }
            }

            if (aDataTable.Rows.Count > 0)
            {
                productGridView.DataSource = aDataTable;
                productGridView.DataBind();

                for (int j = 0; j < aDataTable.Rows.Count; j++)
                {
                    DropDownList ddlCamp = (DropDownList)productGridView.Rows[j].Cells[5].FindControl("IsCampaignProductDropDownList");
                    if (ddlCamp != null && ddlCamp.Items.FindByValue(aDataTable.Rows[j]["IsCampaignProductDropDownList"].ToString()) != null)
                    {
                        ddlCamp.SelectedValue = aDataTable.Rows[j]["IsCampaignProductDropDownList"].ToString();
                    }

                    DropDownList ddlGift = (DropDownList)productGridView.Rows[j].Cells[5].FindControl("IsGiftProductDropDownList");
                    if (ddlGift != null && ddlGift.Items.FindByValue(aDataTable.Rows[j]["IsGiftProductDropDownList"].ToString()) != null)
                    {
                        ddlGift.SelectedValue = aDataTable.Rows[j]["IsGiftProductDropDownList"].ToString();
                    }
                }
            }
            else
            {
                InitialGrid();
            }

            if (!isInvoice)
            {
                submitButton.Text = "<i class=\"bx bx-check-circle me-1\"></i> Update Order";
            }
        }
    }
  
    private void DropDownLoad()
    {
        aOrderListBLL.DCLoad(dcDropDownList);
        aOrderListBLL.LoadSalesPerson(ddlSalesPerson);
    }
    private void OrdNo()
    {
        orderNoTextBox.Text = aOrderListBLL.OrdNo();
    }
    private void Todate()
    {
        orderDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd MMMM, yyyy");
    }

    public void CustomerInfo(string custCode)
    {
        DataTable dtcust = aOrderListBLL.CustomerInfo(custCode);
        if (dtcust.Rows.Count > 0)
        {
            custNameLabel.Text = dtcust.Rows[0]["CustomerName"] != DBNull.Value ? dtcust.Rows[0]["CustomerName"].ToString() : "";
            lblAddress.Text = dtcust.Columns.Contains("Address") && dtcust.Rows[0]["Address"] != DBNull.Value ? dtcust.Rows[0]["Address"].ToString() : "N/A";
            lblCellNo.Text = dtcust.Columns.Contains("CellNo") && dtcust.Rows[0]["CellNo"] != DBNull.Value ? dtcust.Rows[0]["CellNo"].ToString() : "N/A";

            string taggedSalesPersons = dtcust.Columns.Contains("TaggedSalesPersonIds") && dtcust.Rows[0]["TaggedSalesPersonIds"] != DBNull.Value
                ? dtcust.Rows[0]["TaggedSalesPersonIds"].ToString().Trim()
                : "";

            string salesPersonId = "";
            if (dtcust.Columns.Contains("SalesPersonId") && dtcust.Rows[0]["SalesPersonId"] != DBNull.Value && dtcust.Rows[0]["SalesPersonId"].ToString() != "0")
            {
                salesPersonId = dtcust.Rows[0]["SalesPersonId"].ToString();
            }
            else if (dtcust.Columns.Contains("MIOEmpInfoId") && dtcust.Rows[0]["MIOEmpInfoId"] != DBNull.Value && dtcust.Rows[0]["MIOEmpInfoId"].ToString() != "0")
            {
                salesPersonId = dtcust.Rows[0]["MIOEmpInfoId"].ToString();
            }

            if (!string.IsNullOrEmpty(taggedSalesPersons))
            {
                string[] spIds = taggedSalesPersons.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (spIds.Length == 1)
                {
                    string singleId = spIds[0].Trim();
                    if (ddlSalesPerson.Items.FindByValue(singleId) != null)
                    {
                        ddlSalesPerson.SelectedValue = singleId;
                        int spId;
                        if (int.TryParse(singleId, out spId))
                        {
                            LoadDCBySalesPerson(spId);
                        }
                    }
                }
                else if (spIds.Length > 1)
                {
                    if (!string.IsNullOrEmpty(salesPersonId) && spIds.Contains(salesPersonId) && ddlSalesPerson.Items.FindByValue(salesPersonId) != null)
                    {
                        ddlSalesPerson.SelectedValue = salesPersonId;
                        int spId;
                        if (int.TryParse(salesPersonId, out spId))
                        {
                            LoadDCBySalesPerson(spId);
                        }
                    }
                    else
                    {
                        ddlSalesPerson.SelectedIndex = 0;
                        LoadDCBySalesPerson(0);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(salesPersonId) && ddlSalesPerson.Items.FindByValue(salesPersonId) != null)
            {
                ddlSalesPerson.SelectedValue = salesPersonId;
                int spId;
                if (int.TryParse(salesPersonId, out spId))
                {
                    LoadDCBySalesPerson(spId);
                }
            }
            else
            {
                ddlSalesPerson.SelectedIndex = 0;
                LoadDCBySalesPerson(0);
            }

            mioCodeLabel.Text = dtcust.Columns.Contains("MIOEmpMastercode") && dtcust.Rows[0]["MIOEmpMastercode"] != DBNull.Value ? dtcust.Rows[0]["MIOEmpMastercode"].ToString() : "";
            mioNameLabel.Text = dtcust.Columns.Contains("MIOEmpName") && dtcust.Rows[0]["MIOEmpName"] != DBNull.Value ? dtcust.Rows[0]["MIOEmpName"].ToString() : "";
            teritory.Text = dtcust.Columns.Contains("TerritoryCode") && dtcust.Rows[0]["TerritoryCode"] != DBNull.Value ? dtcust.Rows[0]["TerritoryCode"].ToString() : "";
            FCBLabel3.Text = dtcust.Columns.Contains("FixedCustomer") && dtcust.Rows[0]["FixedCustomer"] != DBNull.Value ? dtcust.Rows[0]["FixedCustomer"].ToString() : "0";

            pnlCustomerDetails.Visible = true;
        }
        else
        {
            lblAddress.Text = "";
            lblCellNo.Text = "";
            pnlCustomerDetails.Visible = false;
        }
    }

    public void LoadDCBySalesPerson(int salesPersonId)
    {
        dcDropDownList.Items.Clear();

        if (salesPersonId > 0)
        {
            DataTable dtDC = aOrderListBLL.GetDCBySalesPerson(salesPersonId);
            if (dtDC != null && dtDC.Rows.Count > 0)
            {
                dcDropDownList.DataSource = dtDC;
                dcDropDownList.DataTextField = "ComDisplayName";
                dcDropDownList.DataValueField = "ComUnitId";
                dcDropDownList.DataBind();

                if (dtDC.Rows.Count == 1)
                {
                    // Single DC: Auto select!
                    dcDropDownList.SelectedValue = dtDC.Rows[0]["ComUnitId"].ToString();
                }
                else
                {
                    // Multiple DCs: Prompt user to select manually
                    dcDropDownList.Items.Insert(0, new ListItem("-- Select Distribution Center --", ""));
                    dcDropDownList.SelectedIndex = 0;
                }
                return;
            }
        }

        // Fallback: load all DCs with prompt
        aOrderListBLL.DCLoad(dcDropDownList);
    }

    protected void ddlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        int salesPersonId = 0;
        if (int.TryParse(ddlSalesPerson.SelectedValue, out salesPersonId) && salesPersonId > 0)
        {
            LoadDCBySalesPerson(salesPersonId);
        }
        else
        {
            LoadDCBySalesPerson(0);
        }
    }
    private void InitialGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("Totaltp");
        aDataTable.Columns.Add("TotaltpVat");


        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["PackSize"] = "";
        dataRow["Quantity"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["ProductId"] = "";
        dataRow["VAT"] = "";
        dataRow["Totaltp"] = "";
        dataRow["TotaltpVat"] = "";

        aDataTable.Rows.Add(dataRow);

        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();

        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
           
        }
       
    }

    private void AddRowInGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("ProductId");

        aDataTable.Columns.Add("IsCampaignProductDropDownList");
        aDataTable.Columns.Add("IsGiftProductDropDownList");


        aDataTable.Columns.Add("Totaltp");
        aDataTable.Columns.Add("TotaltpVat");


       

        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {

                dataRow = aDataTable.NewRow();
                TextBox vatTextBox =
                    (TextBox)productGridView.Rows[i].Cells[5].FindControl("vatTextBox");
                TextBox tpTextBox =
                    (TextBox)productGridView.Rows[i].Cells[6].FindControl("tpTextBox");

                dataRow["SL"] = Convert.ToString(i + 1);
                TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                dataRow["ProductCode"] = productCodeTextBox.Text.Trim();
                TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                dataRow["ProductName"] = productNameTextBox.Text.Trim();
                TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                dataRow["PackSize"] = packSizeTextBox.Text;
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("unitpriceHiddenField");
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
                dataRow["Quantity"] = quantityTextBox.Text.Trim();
                dataRow["ProductId"] = productidHiddenField.Value.Trim();
                dataRow["UnitPrice"] = (tpTextBox != null && !string.IsNullOrEmpty(tpTextBox.Text.Trim())) ? tpTextBox.Text.Trim() : unitpriceHiddenField.Value;
                dataRow["VAT"] = vatTextBox.Text;

                TextBox TotaltpTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("TotaltpTextBox");
                TextBox TotaltpVatTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("TotaltpVatTextBox");

                dataRow["Totaltp"] = TotaltpTextBox.Text;
                dataRow["TotaltpVat"] = TotaltpVatTextBox.Text;



                DropDownList C =
                (DropDownList)productGridView.Rows[i].Cells[1].FindControl("IsCampaignProductDropDownList");
                DropDownList G =
                (DropDownList)productGridView.Rows[i].Cells[1].FindControl("IsGiftProductDropDownList");



                dataRow["IsCampaignProductDropDownList"] = C.SelectedItem.Text;
                dataRow["IsGiftProductDropDownList"] = G.SelectedItem.Text;

                aDataTable.Rows.Add(dataRow);
            }
        }
        int sl = aDataTable.Rows.Count;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["PackSize"] = "";
        dataRow["Quantity"] = "";
        dataRow["ProductId"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["IsCampaignProductDropDownList"] = "";
        dataRow["IsGiftProductDropDownList"] = "";

        dataRow["Totaltp"] = "";
        dataRow["TotaltpVat"] = "";


        aDataTable.Rows.Add(dataRow);


        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            
        }
        for (int j = 0; j < aDataTable.Rows.Count; j++)
        {
            DropDownList ddlIsCampaignProductDropDownList =
                ((DropDownList)productGridView.Rows[j].Cells[5].FindControl("IsCampaignProductDropDownList"));
            ddlIsCampaignProductDropDownList.SelectedValue = aDataTable.Rows[j]["IsCampaignProductDropDownList"].ToString();


            DropDownList ddIsGiftProductDropDownList =
               ((DropDownList)productGridView.Rows[j].Cells[5].FindControl("IsGiftProductDropDownList"));
            ddIsGiftProductDropDownList.SelectedValue = aDataTable.Rows[j]["IsGiftProductDropDownList"].ToString();
        }
    }
    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        AddRowInGrid();
    }
    protected void ImageButton2_Click(object sender, EventArgs e)
    {
        Control btn = (Control)sender;
        GridViewRow currentRow = (GridViewRow)btn.NamingContainer;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("IsCampaignProductDropDownList");
        aDataTable.Columns.Add("IsGiftProductDropDownList");

        aDataTable.Columns.Add("Totaltp");
        aDataTable.Columns.Add("TotaltpVat");


 


        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            int sl1 = 1;
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (i != rowindex)
                {
                    dataRow = aDataTable.NewRow();
                    TextBox vatTextBox =
                    (TextBox)productGridView.Rows[i].Cells[5].FindControl("vatTextBox");
                    dataRow["SL"] = Convert.ToString(sl1);
                    TextBox productCodeTextBox2 = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                    dataRow["ProductCode"] = productCodeTextBox2.Text.Trim();
                    TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                    dataRow["ProductName"] = productNameTextBox.Text.Trim();
                    TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                    dataRow["PackSize"] = packSizeTextBox.Text;
                    TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                    HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("unitpriceHiddenField");
                    HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
                    TextBox tpTextBox = (TextBox)productGridView.Rows[i].FindControl("tpTextBox");
                    dataRow["Quantity"] = quantityTextBox.Text.Trim();
                    dataRow["ProductId"] = productidHiddenField.Value.Trim();
                    dataRow["UnitPrice"] = (tpTextBox != null && !string.IsNullOrEmpty(tpTextBox.Text.Trim())) ? tpTextBox.Text.Trim() : unitpriceHiddenField.Value.Trim();
                    dataRow["VAT"] = vatTextBox.Text;

                    TextBox TotaltpTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("TotaltpTextBox");
                    TextBox TotaltpVatTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("TotaltpVatTextBox");

                    dataRow["Totaltp"] = TotaltpTextBox.Text;
                    dataRow["TotaltpVat"] = TotaltpVatTextBox.Text;


                    DropDownList C =
                    (DropDownList)productGridView.Rows[i].Cells[1].FindControl("IsCampaignProductDropDownList");
                    DropDownList G =
                    (DropDownList)productGridView.Rows[i].Cells[1].FindControl("IsGiftProductDropDownList");



                    dataRow["IsCampaignProductDropDownList"] = C.SelectedItem.Text;
                    dataRow["IsGiftProductDropDownList"] = G.SelectedItem.Text;


                    dataRow["ProductId"] = productidHiddenField.Value.Trim();
                    aDataTable.Rows.Add(dataRow);
                    sl1 += 1;
                }
            }
        }
        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        if (productGridView.Rows.Count < 1)
        {
            InitialGrid();
        }
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            
        }
        for (int j = 0; j < aDataTable.Rows.Count; j++)
        {
            DropDownList ddlIsCampaignProductDropDownList =
                ((DropDownList)productGridView.Rows[j].Cells[5].FindControl("IsCampaignProductDropDownList"));
            ddlIsCampaignProductDropDownList.SelectedValue = aDataTable.Rows[j]["IsCampaignProductDropDownList"].ToString();


            DropDownList ddIsGiftProductDropDownList =
               ((DropDownList)productGridView.Rows[j].Cells[5].FindControl("IsGiftProductDropDownList"));
            ddIsGiftProductDropDownList.SelectedValue = aDataTable.Rows[j]["IsGiftProductDropDownList"].ToString();
        }
    }
    private void GetProductInGrid(int rowindex, string productCode)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {

            aDataTable = _aDcStockReceiveBll.ProductInfoNew(productCode);
            if (aDataTable.Rows.Count > 0)
            {
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
                TextBox productNameTextBox =
                    (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                productNameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();
                TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                packSizeTextBox.Text = aDataTable.Rows[0]["PackSize"].ToString();
                HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("unitpriceHiddenField");
                unitpriceHiddenField.Value = aDataTable.Rows[0]["UnitPrice"].ToString();
                productidHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();
                TextBox vatTextBox =
                    (TextBox)productGridView.Rows[rowindex].Cells[5].FindControl("vatTextBox");
                TextBox tpTextBox =
                    (TextBox)productGridView.Rows[rowindex].Cells[6].FindControl("tpTextBox");
                tpTextBox.Text = aDataTable.Rows[0]["UnitPrice"].ToString();
                vatTextBox.Text = aDataTable.Rows[0]["VATAmountPerUnit"].ToString();

            }
        }
    }
    protected void productCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();

        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");

        string productCode = productCodeTextBox.Text.Trim();
        GetProductInGrid(rowindex, productCode);
    }


    private void SaveAllData()
    {
        try
        {
            int maxReqId;
            string mioCode = mioCodeLabel.Text;
            string mioName = mioNameLabel.Text;
            int mioId = 0;

            if (ddlSalesPerson.SelectedIndex <= 0 || string.IsNullOrEmpty(ddlSalesPerson.SelectedValue) || ddlSalesPerson.SelectedValue == "0")
            {
                showMessageBox("Please Select Sales Person (MIO)!");
                ddlSalesPerson.Focus();
                return;
            }

            if (ddlSalesPerson.SelectedIndex > 0 && ddlSalesPerson.SelectedItem != null)
            {
                int.TryParse(ddlSalesPerson.SelectedValue, out mioId);
                string spText = ddlSalesPerson.SelectedItem != null ? ddlSalesPerson.SelectedItem.Text : "";
                if (spText.Contains(':'))
                {
                    string[] parts = spText.Split(':');
                    mioCode = parts[0].Trim();
                    mioName = parts.Length > 1 ? parts[1].Trim() : "";
                }
                else
                {
                    mioName = spText.Trim();
                }
            }

            string customerName = !string.IsNullOrEmpty(custNameLabel.Text) ? custNameLabel.Text.Trim() : "";
            if (string.IsNullOrEmpty(customerName))
            {
                DataTable dtc = aOrderListBLL.CustomerInfo(custCodeTextBox.Text.Trim());
                if (dtc != null && dtc.Rows.Count > 0 && dtc.Rows[0]["CustomerName"] != DBNull.Value)
                {
                    customerName = dtc.Rows[0]["CustomerName"].ToString().Trim();
                }
            }

            int comUnitId = 0;
            int.TryParse(dcDropDownList.SelectedValue, out comUnitId);

            string comUnitName = "";
            string comUnitCode = "";
            if (dcDropDownList.SelectedItem != null && !string.IsNullOrEmpty(dcDropDownList.SelectedItem.Text))
            {
                string dcText = dcDropDownList.SelectedItem.Text;
                if (dcText.Contains(':'))
                {
                    string[] dcParts = dcText.Split(':');
                    comUnitCode = dcParts[0].Trim();
                    comUnitName = dcParts.Length > 1 ? dcParts[1].Trim() : "";
                }
                else
                {

                    comUnitName = dcText.Trim();
                }
            }

            DateTime subDate = DateTime.Now;
            if (!string.IsNullOrEmpty(orderDateTextBox.Text))
            {
                if (!DateTime.TryParse(orderDateTextBox.Text, out subDate))
                {
                    DateTime.TryParseExact(orderDateTextBox.Text, new string[] { "dd MMMM, yyyy", "dd-MMM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out subDate);
                }
            }

            bool isFixedCust = false;
            if (!string.IsNullOrEmpty(FCBLabel3.Text))
            {
                string fcbVal = FCBLabel3.Text.Trim();
                if (fcbVal == "1" || fcbVal.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    isFixedCust = true;
                }
            }

            OrderInfoMaster aListMasterDao = new OrderInfoMaster()
            {
                OrderCode = orderNoTextBox.Text,
                ComUnitId = comUnitId,
                ComUnitName = comUnitName,
                ComUnitCode = comUnitCode,
                MIOCode = mioCode,
                MIOName = mioName,
                MIOId = mioId,
                teritory = teritory.Text,
                ManufacId = 1,
                CustomerCode = custCodeTextBox.Text.Trim(),
                SubmissionDate = subDate,
                CustomerName = customerName,
                IsManual = true,
                FCB = isFixedCust,
                Remarks = remarksTextBox.Text.Trim()
            };

            decimal totalPrice = 0;
            decimal totalVat = 0;

            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox");
                TextBox tpTextBox = (TextBox)productGridView.Rows[i].FindControl("tpTextBox");
                HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].FindControl("unitpriceHiddenField");
                TextBox vatTextBox = (TextBox)productGridView.Rows[i].FindControl("vatTextBox");

                decimal qty = 0;
                decimal unitPrice = 0;
                decimal unitVat = 0;

                if (quantityTextBox != null && !string.IsNullOrEmpty(quantityTextBox.Text))
                    decimal.TryParse(quantityTextBox.Text.Trim(), out qty);
                if (tpTextBox != null && !string.IsNullOrEmpty(tpTextBox.Text))
                    decimal.TryParse(tpTextBox.Text.Trim(), out unitPrice);
                else if (unitpriceHiddenField != null && !string.IsNullOrEmpty(unitpriceHiddenField.Value))
                    decimal.TryParse(unitpriceHiddenField.Value.Trim(), out unitPrice);
                if (vatTextBox != null && !string.IsNullOrEmpty(vatTextBox.Text))
                    decimal.TryParse(vatTextBox.Text.Trim(), out unitVat);

                totalPrice += unitPrice * qty;
                totalVat += unitVat * qty;
            }

            aListMasterDao.GrossValue = totalPrice;
            aListMasterDao.TotalVat = totalVat;
            aListMasterDao.TotalDiscount = 0;
            aListMasterDao.TotalNetPayable = totalPrice + totalVat;

            bool isEditMode = !string.IsNullOrEmpty(hfOrderId.Value);
            int orderId = 0;
            if (isEditMode)
            {
                int.TryParse(hfOrderId.Value, out orderId);
                aListMasterDao.OrderId = orderId;
            }

            bool masterSaved = false;
            if (isEditMode)
            {
                masterSaved = aOrderListBLL.UpdateOrderMaster(aListMasterDao);
                maxReqId = orderId;
            }
            else
            {
                masterSaved = aOrderListBLL.SaveOrderMaster(aListMasterDao, out maxReqId);
            }

            if (!masterSaved || maxReqId <= 0)
            {
                showMessageBox("Error saving Order. Please check details and try again.");
                return;
            }

            List<OrderInfoDetail> aOrderInfoDetailList = new List<OrderInfoDetail>();

            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].FindControl("productCodeTextBox");
                TextBox productNameTextBox = (TextBox)productGridView.Rows[i].FindControl("productNameTextBox");
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox");
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].FindControl("productidHiddenField");
                TextBox tpTextBox = (TextBox)productGridView.Rows[i].FindControl("tpTextBox");
                HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].FindControl("unitpriceHiddenField");
                TextBox vatTextBox = (TextBox)productGridView.Rows[i].FindControl("vatTextBox");

                int prodId = 0;
                decimal qty = 0;
                decimal tradePrice = 0;
                decimal vatPerUnit = 0;

                if (productidHiddenField != null && !string.IsNullOrEmpty(productidHiddenField.Value))
                    int.TryParse(productidHiddenField.Value.Trim(), out prodId);
                if (quantityTextBox != null && !string.IsNullOrEmpty(quantityTextBox.Text))
                    decimal.TryParse(quantityTextBox.Text.Trim(), out qty);
                if (tpTextBox != null && !string.IsNullOrEmpty(tpTextBox.Text))
                    decimal.TryParse(tpTextBox.Text.Trim(), out tradePrice);
                else if (unitpriceHiddenField != null && !string.IsNullOrEmpty(unitpriceHiddenField.Value))
                    decimal.TryParse(unitpriceHiddenField.Value.Trim(), out tradePrice);
                if (vatTextBox != null && !string.IsNullOrEmpty(vatTextBox.Text))
                    decimal.TryParse(vatTextBox.Text.Trim(), out vatPerUnit);

                if (prodId <= 0 || qty <= 0)
                    continue;

                OrderInfoDetail aOrderInfoDetail = new OrderInfoDetail();
                aOrderInfoDetail.ProductCode = productCodeTextBox != null ? productCodeTextBox.Text.Trim() : "";
                aOrderInfoDetail.ProductName = productNameTextBox != null ? productNameTextBox.Text.Trim() : "";
                aOrderInfoDetail.ProductId = prodId;
                aOrderInfoDetail.Quantity = qty;
                aOrderInfoDetail.OrderId = maxReqId;
                aOrderInfoDetail.TradePrice = tradePrice;
                aOrderInfoDetail.TotalTradePrice = qty * tradePrice;
                aOrderInfoDetail.VatAmountPerUnit = vatPerUnit;
                aOrderInfoDetail.TotalVatAmount = qty * vatPerUnit;
                aOrderInfoDetail.NetAmount = aOrderInfoDetail.TotalTradePrice + aOrderInfoDetail.TotalVatAmount;

                DropDownList giftTextBox = (DropDownList)productGridView.Rows[i].Cells[2].FindControl("IsGiftProductDropDownList");
                DropDownList campTextBox = (DropDownList)productGridView.Rows[i].Cells[4].FindControl("IsCampaignProductDropDownList");

                aOrderInfoDetail.IsgiftProduct = giftTextBox != null ? giftTextBox.Text : "False";
                aOrderInfoDetail.IsCampaignProduct = campTextBox != null ? campTextBox.Text : "False";

                aOrderInfoDetailList.Add(aOrderInfoDetail);
            }

            string msg = aOrderListBLL.SaveOrderDetail(aOrderInfoDetailList);
            if (isEditMode)
            {
                Response.Redirect("OrderListView.aspx");
            }
            else
            {
                Clear();
                showMessageBox("Order " + aListMasterDao.OrderCode + " Created Successfully!");
            }
        }
        catch (Exception ex)
        {
            showMessageBox("Error: " + ex.Message);
        }
    }

    public bool Validation()
    {
        if (string.IsNullOrEmpty(custCodeTextBox.Text.Trim()))
        {
            showMessageBox("Please Select a valid Customer!");
            custCodeTextBox.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(custNameLabel.Text.Trim()))
        {
            CustomerInfo(custCodeTextBox.Text.Trim());
            if (string.IsNullOrEmpty(custNameLabel.Text.Trim()))
            {
                showMessageBox("Please Select a valid Customer!");
                custCodeTextBox.Focus();
                return false;
            }
        }

        if (ddlSalesPerson.SelectedIndex <= 0 || string.IsNullOrEmpty(ddlSalesPerson.SelectedValue) || ddlSalesPerson.SelectedValue == "0")
        {
            showMessageBox("Please Select Sales Person (MIO)!");
            ddlSalesPerson.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(dcDropDownList.SelectedValue))
        {
            showMessageBox("Please Select Distribution Center!");
            dcDropDownList.Focus();
            return false;
        }

        if (productGridView.Rows.Count == 0)
        {
            showMessageBox("Please Add at least one Product item!");
            return false;
        }

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            TextBox pCode = (TextBox)productGridView.Rows[i].FindControl("productCodeTextBox");
            TextBox pName = (TextBox)productGridView.Rows[i].FindControl("productNameTextBox");
            TextBox pQty = (TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox");

            if (string.IsNullOrEmpty(pCode.Text.Trim()) || string.IsNullOrEmpty(pName.Text.Trim()))
            {
                pName.Focus();
                showMessageBox("Please select valid Product in row #" + (i + 1));
                return false;
            }

            decimal qtyVal = 0;
            if (string.IsNullOrEmpty(pQty.Text.Trim()) || !decimal.TryParse(pQty.Text.Trim(), out qtyVal) || qtyVal <= 0)
            {
                pQty.Focus();
                showMessageBox("Please enter valid Req. Qty in row #" + (i + 1));
                return false;
            }
        }
        return true;
    }
    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hfOrderId.Value))
        {
            OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
            DataTable dtCheck = aOrderInfoBll.LoadOrderWithDetail(hfOrderId.Value);
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                DataRow firstRow = dtCheck.Rows[0];
                if (dtCheck.Columns.Contains("IsInvoice") && firstRow["IsInvoice"] != DBNull.Value)
                {
                    string invVal = firstRow["IsInvoice"].ToString().Trim();
                    if (invVal == "1" || invVal.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        showMessageBox("Invoice has already been generated for this Order! Order cannot be updated.");
                        return;
                    }
                }
            }
        }

        if (Validation())
        {
            SaveAllData();
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    private void Clear()
    {
       // manufacturerDropDownList.SelectedValue = "";
        InitialGrid();
        Todate();
        OrdNo();
        DropDownLoad();
        teritory.Text = string.Empty;
        custNameLabel.Text = string.Empty;
        mioCodeLabel.Text = string.Empty;
        mioNameLabel.Text = string.Empty;
        marketNameLabel.Text = string.Empty;
        custCodeTextBox.Text = string.Empty;
        remarksTextBox.Text = string.Empty;
    }
    protected void productNameTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");

        string productName = productNameTextBox.Text.Trim();
        if (productName.Contains(':'))
        {
            string[] productInfo = productName.Split(':');

            TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");

            productCodeTextBox.Text = productInfo[0];
            //productNameTextBox.Text = productInfo[1];
            string productCode = productCodeTextBox.Text.Trim();
            GetProductInGrid(rowindex, productCode);
        }
        else
        {
            showMessageBox("Input Correct Data!!");
        }
    }
    protected void miaImageButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("OrderRequisitionView.aspx");
    }
    protected void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitialGrid();
    }
    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string empName = custCodeTextBox.Text.Trim();
        if (empName.Contains(':'))
        {
            int pipeIndex = empName.LastIndexOf('|');
            if (pipeIndex >= 0)
            {
                hfCustomerId.Value = empName.Substring(pipeIndex + 1).Trim();
                string mainPart = empName.Substring(0, pipeIndex).Trim();
                string[] parts = mainPart.Split(':');
                custCodeTextBox.Text = parts[0].Trim();
                CustomerInfo(custCodeTextBox.Text);
            }
            else
            {
                string[] parts = empName.Split(':');
                custCodeTextBox.Text = parts[0].Trim();
                CustomerInfo(custCodeTextBox.Text);
            }
        }
        else if (!string.IsNullOrEmpty(empName))
        {
            CustomerInfo(empName);
        }
        else
        {
            custCodeTextBox.Text = "";
            hfCustomerId.Value = "";
            showMessageBox("Input Correct Data !!");
        }
    }

    protected void resetbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManualOrderCreation.aspx");
    }
}