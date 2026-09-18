using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MaterialConversionReceive : System.Web.UI.Page
{
    private readonly MaterialConversionReceiveDal aReceiveDal = new MaterialConversionReceiveDal();
    private readonly WarehouseStockInBll aWarehouseStockInBll = new WarehouseStockInBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
        if (scriptManager != null)
        {
            scriptManager.RegisterPostBackControl(submitButton);
        }

        if (!IsPostBack)
        {
            LoadDropdownlist();

            string editIdValue = Request.QueryString["ID"];
            if (string.IsNullOrEmpty(editIdValue) && Session["IDInformation"] != null)
            {
                editIdValue = Session["IDInformation"].ToString();
                Session["IDInformation"] = null;
            }

            int stockInMasterId;
            if (!string.IsNullOrEmpty(editIdValue) && int.TryParse(editIdValue, out stockInMasterId))
            {
                LoadExistingStockInData(stockInMasterId);
            }
        }
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if ((gv.ShowHeader && gv.Rows.Count > 0) || gv.ShowHeaderWhenEmpty)
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void stockInDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateTime value;
        if (!DateTime.TryParse(stockInDateTextBox.Text, out value))
        {
            stockInDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
    }

    protected void challanDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateTime value;
        if (!DateTime.TryParse(challanDateTextBox.Text, out value))
        {
            challanDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
    }

    protected void referenceDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateTime value;
        if (!DateTime.TryParse(referenceDateTextBox.Text, out value))
        {
            referenceDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
    }

    protected void showMessageBox(string message)
    {
        ShowAlertMessage("warning", "Validation error", message);
    }

    protected void showSuccessMessage(string message, bool isUpdate)
    {
        ShowAlertMessage("success", isUpdate ? "Updated" : "Saved", message);
    }

    protected void showErrorMessage(string message)
    {
        ShowAlertMessage("error", "Operation failed", message);
    }

    private void ShowAlertMessage(string type, string title, string message)
    {
        string safeType = HttpUtility.JavaScriptStringEncode(type ?? "info");
        string safeTitle = HttpUtility.JavaScriptStringEncode(title ?? string.Empty);
        string safeMessage = HttpUtility.JavaScriptStringEncode(message ?? string.Empty);
        string script;

        if (string.Equals(safeType, "success", StringComparison.OrdinalIgnoreCase))
        {
            script = string.Format("showMaterialSuccess('{0}', '{1}');", safeTitle, safeMessage);
        }
        else
        {
            script = string.Format("showMaterialAlert('{0}', '{1}', '{2}');", safeType, safeTitle, safeMessage);
        }

        ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString("N"), script, true);
    }

    private void LoadDropdownlist()
    {
        aReceiveDal.LoadCompany(ddlCompany);
    }

    private void ResetSummary()
    {
        totalQtyTextBox.Text = string.Empty;
        totalVatTextBox.Text = string.Empty;
        grandTotalTextBox.Text = string.Empty;
    }

    private static string FormatDateValue(object value)
    {
        return value == null || value == DBNull.Value
            ? string.Empty
            : Convert.ToDateTime(value).ToString("dd-MMM-yyyy");
    }

    private static string FormatTextValue(object value)
    {
        return value == null || value == DBNull.Value ? string.Empty : value.ToString();
    }

    private static bool TrySetDropDownValue(DropDownList dropDown, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        ListItem item = dropDown.Items.FindByValue(value);
        if (item == null)
        {
            return false;
        }

        dropDown.SelectedValue = value;
        return true;
    }

    private static Dictionary<int, DataRow> BuildSavedDetailLookup(DataTable savedDetailTable)
    {
        Dictionary<int, DataRow> savedRows = new Dictionary<int, DataRow>();

        foreach (DataRow row in savedDetailTable.Rows)
        {
            int detailId;
            if (!int.TryParse(FormatTextValue(row["MaterialConversionDetailId"]), out detailId) || detailId <= 0)
            {
                continue;
            }

            if (!savedRows.ContainsKey(detailId))
            {
                savedRows.Add(detailId, row);
            }
        }

        return savedRows;
    }

    private void MarkAndPopulateSavedRows(Dictionary<int, DataRow> savedRows)
    {
        int checkedCount = 0;

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            GridViewRow row = loadGridView.Rows[i];
            CheckBox checkBox = (CheckBox)row.FindControl("chkSelect");
            object detailKey = loadGridView.DataKeys[i]["MaterialConversionDetailId"];
            int detailId = detailKey == null || detailKey == DBNull.Value
                ? 0
                : Convert.ToInt32(detailKey);

            if (checkBox == null || !savedRows.ContainsKey(detailId))
            {
                if (checkBox != null)
                {
                    checkBox.Checked = false;
                }

                continue;
            }

            checkBox.Checked = true;
            checkedCount++;

            DataRow savedRow = savedRows[detailId];

            ((TextBox)row.FindControl("conversionCodeTextBox")).Text = FormatTextValue(savedRow["MaterialConversionCode"]);
            ((TextBox)row.FindControl("productCodeTextBox")).Text = FormatTextValue(savedRow["ProductCode"]);
            ((TextBox)row.FindControl("productNameTextBox")).Text = FormatTextValue(savedRow["ProductName"]);
            ((TextBox)row.FindControl("packSizeTextBox")).Text = FormatTextValue(savedRow["PackSize"]);
            ((TextBox)row.FindControl("batchTextBox")).Text = FormatTextValue(savedRow["Batch_No"]);
            ((TextBox)row.FindControl("mfgDateTextBox")).Text = FormatDateValue(savedRow["Mfg_Date"]);
            ((TextBox)row.FindControl("expDateDateTextBox")).Text = FormatDateValue(savedRow["Exp_Date"]);
            ((TextBox)row.FindControl("reqQtyTextBox")).Text = FormatTextValue(savedRow["FinalOutputQuantityInt"]);
            ((TextBox)row.FindControl("costPriceTextBox")).Text = FormatTextValue(savedRow["CostPerUnit"]);
            ((TextBox)row.FindControl("vatTextBox")).Text = FormatTextValue(savedRow["Vat"]);
            ((TextBox)row.FindControl("totalValueTextBox")).Text = FormatTextValue(savedRow["TotalCostOfThisBatch"]);

            HiddenField productIdHiddenField = (HiddenField)row.FindControl("productidHiddenField");
            if (productIdHiddenField != null)
            {
                productIdHiddenField.Value = FormatTextValue(savedRow["ProductId"]);
            }
        }

        if (loadGridView.HeaderRow != null)
        {
            CheckBox headerCheckBox = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");
            if (headerCheckBox != null)
            {
                headerCheckBox.Checked = checkedCount > 0 && checkedCount == loadGridView.Rows.Count;
            }
        }
    }

    private void LoadExistingStockInData(int stockInMasterId)
    {
        DataTable masterTable = aReceiveDal.LoadStockInMasterForEdit(stockInMasterId);
        if (masterTable.Rows.Count == 0)
        {
            showMessageBox("No stock-in data found for edit.");
            return;
        }

        DataRow masterRow = masterTable.Rows[0];
        stockInIdHiddenField.Value = stockInMasterId.ToString();

        string companyId = FormatTextValue(masterRow["CompanyId"]);
        if (!string.IsNullOrEmpty(companyId))
        {
            TrySetDropDownValue(ddlCompany, companyId);
            aReceiveDal.GetProductionUnit(ddlProductionUnit, companyId);
            aWarehouseStockInBll.LoadWhInformation(warehouseDropDownList, companyId);
        }

        TrySetDropDownValue(warehouseDropDownList, FormatTextValue(masterRow["WarehouseId"]));
        TrySetDropDownValue(ddlProductionUnit, FormatTextValue(masterRow["ProductionUnitId"]));

        stockInDateTextBox.Text = FormatDateValue(masterRow["WHStockInDate"]);
        challanNoTextBox.Text = FormatTextValue(masterRow["ChallanNo"]);
        challanDateTextBox.Text = FormatDateValue(masterRow["ChallanDate"]);
        referenceNoTextBox.Text = FormatTextValue(masterRow["ReferenceNo"]);
        referenceDateTextBox.Text = FormatTextValue(masterRow["ReferenceDate"]);
        remarksTextBox.Text = FormatTextValue(masterRow["Remarks"]);

        totalQtyTextBox.Text = FormatTextValue(masterRow["TotalQuantity"]);
        totalVatTextBox.Text = FormatTextValue(masterRow["TotalVat"]);
        grandTotalTextBox.Text = FormatTextValue(masterRow["TotalValue"]);

        DataTable savedDetailTable = aReceiveDal.LoadStockInDetailForEdit(stockInMasterId);
        DataTable listTable = new DataTable();
        if (ddlProductionUnit.SelectedValue != "")
        {
            listTable = aReceiveDal.LoadMaterialConversionListForEdit(GenerateParameter(), stockInMasterId);
        }

        if (listTable.Rows.Count == 0)
        {
            listTable = savedDetailTable;
        }

        loadGridView.DataSource = listTable;
        loadGridView.DataBind();

        MarkAndPopulateSavedRows(BuildSavedDetailLookup(savedDetailTable));
        if (loadGridView.Rows.Count > 0)
        {
            CalculateQuantityVatAndValue();
        }

        submitButton.Text = "Update Information";
    }

    private void LoadPendingMaterialConversionList()
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();
        ResetSummary();

        if (ddlProductionUnit.SelectedValue != "")
        {
            DataTable aTable = aReceiveDal.LoadMaterialConversionList(GenerateParameter());

            if (aTable.Rows.Count > 0)
            {
                loadGridView.DataSource = aTable;
                loadGridView.DataBind();
                CalculateQuantityVatAndValue();
            }
        }
    }

    protected void mfgDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)textBox.Parent.Parent;
        TextBox mfgDateTextBox = (TextBox)currentRow.FindControl("mfgDateTextBox");

        DateTime value;
        if (!DateTime.TryParse(mfgDateTextBox.Text, out value))
        {
            mfgDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
        else
        {
            DateTime dd = Convert.ToDateTime(mfgDateTextBox.Text);
            ((TextBox)currentRow.FindControl("expDateDateTextBox")).Text = dd.AddYears(2).ToString("dd-MMM-yyyy");
        }
    }

    protected void expDateDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)textBox.Parent.Parent;
        TextBox mfgDateTextBox = (TextBox)currentRow.FindControl("mfgDateTextBox");
        TextBox expDateDateTextBox = (TextBox)currentRow.FindControl("expDateDateTextBox");

        if (mfgDateTextBox.Text != "")
        {
            DateTime value;

            if (!DateTime.TryParse(expDateDateTextBox.Text, out value))
            {
                expDateDateTextBox.Text = "";
                showMessageBox("Please insert valid date");
            }

            DateTime mDate = Convert.ToDateTime(mfgDateTextBox.Text);
            DateTime exDate = Convert.ToDateTime(expDateDateTextBox.Text);

            if (exDate < mDate)
            {
                expDateDateTextBox.Text = string.Empty;
                showMessageBox("Date Error!!");
            }
        }
        else
        {
            showMessageBox("Please Insert Manufacturing Date!!");
            expDateDateTextBox.Text = string.Empty;
        }
    }

    protected void reqQtyTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)textBox.Parent.Parent;
        CalculateTotalValue(currentRow.RowIndex);
    }

    protected void costPriceTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)textBox.Parent.Parent;
        CalculateTotalValue(currentRow.RowIndex);
    }

    private void CalculateTotalValue(int rowIndex)
    {
        GridViewRow row = loadGridView.Rows[rowIndex];
        TextBox quantityTextBox = (TextBox)row.FindControl("reqQtyTextBox");
        TextBox costPriceTextBox = (TextBox)row.FindControl("costPriceTextBox");
        TextBox vatTextBox = (TextBox)row.FindControl("vatTextBox");
        TextBox totalValueTextBox = (TextBox)row.FindControl("totalValueTextBox");

        decimal totalValue = 0;
        decimal totalVat = 0;
        string vat = vatTextBox.Text;
        string quantity = quantityTextBox.Text;
        string price = costPriceTextBox.Text;

        if (quantity != "" && price != "" && vat != "")
        {
            totalVat = totalVat + (Convert.ToDecimal(quantity) * Convert.ToDecimal(vat));
            totalValue = totalValue + (totalVat + (Convert.ToDecimal(quantity) * Convert.ToDecimal(price)));
            totalValueTextBox.Text = totalValue.ToString(CultureInfo.InvariantCulture);
            CalculateQuantityVatAndValue();
        }
        else
        {
            totalValueTextBox.Text = "";
            CalculateQuantityVatAndValue();
        }
    }

    private void CalculateQuantityVatAndValue()
    {
        decimal totalQuantity = 0;
        decimal value = 0;
        decimal totalVat = 0;

        if (loadGridView.Rows.Count > 0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                GridViewRow row = loadGridView.Rows[i];
                CheckBox cb = (CheckBox)row.FindControl("chkSelect");

                if (cb.Checked)
                {
                    if (!string.IsNullOrEmpty(((TextBox)row.FindControl("reqQtyTextBox")).Text))
                    {
                        int qty = Convert.ToInt32(((TextBox)row.FindControl("reqQtyTextBox")).Text);
                        totalQuantity += qty;
                    }

                    if (!string.IsNullOrEmpty(((TextBox)row.FindControl("vatTextBox")).Text) &&
                        !string.IsNullOrEmpty(((TextBox)row.FindControl("reqQtyTextBox")).Text))
                    {
                        decimal qty = Convert.ToDecimal(((TextBox)row.FindControl("reqQtyTextBox")).Text);
                        decimal vat = Convert.ToDecimal(((TextBox)row.FindControl("vatTextBox")).Text);
                        totalVat += qty * vat;
                    }

                    if (!string.IsNullOrEmpty(((TextBox)row.FindControl("totalValueTextBox")).Text))
                    {
                        decimal costPrice = Convert.ToDecimal(((TextBox)row.FindControl("totalValueTextBox")).Text);
                        value += costPrice;
                    }
                }
            }

            totalQtyTextBox.Text = totalQuantity.ToString(CultureInfo.InvariantCulture);
            totalVatTextBox.Text = totalVat.ToString(CultureInfo.InvariantCulture);
            grandTotalTextBox.Text = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (stockInIdHiddenField.Value == "")
            {
                SaveAllData();
            }
            else
            {
                UpdateAllData();
            }
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private bool Validation()
    {
        if (warehouseDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select a Warehouse !!!");
            warehouseDropDownList.Focus();
            warehouseDropDownList.BackColor = Color.GhostWhite;
            return false;
        }

        if (stockInDateTextBox.Text == "")
        {
            showMessageBox("Please insert stock in date !!!");
            stockInDateTextBox.Focus();
            stockInDateTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        if (challanNoTextBox.Text == "")
        {
            showMessageBox("Please insert challan number !!!");
            challanNoTextBox.Focus();
            challanNoTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        if (challanDateTextBox.Text == "")
        {
            showMessageBox("Please insert challan date !!!");
            challanDateTextBox.Focus();
            challanDateTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        int selectedCount = 0;

        if (loadGridView.Rows.Count > 0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                GridViewRow row = loadGridView.Rows[i];
                CheckBox cb = (CheckBox)row.FindControl("chkSelect");
                if (!cb.Checked)
                {
                    continue;
                }

                selectedCount++;

                if (((TextBox)row.FindControl("productCodeTextBox")).Text == "")
                {
                    ((TextBox)row.FindControl("productCodeTextBox")).Focus();
                    showMessageBox("Please fill out product code!!");
                    return false;
                }

                if (((TextBox)row.FindControl("batchTextBox")).Text == "")
                {
                    ((TextBox)row.FindControl("batchTextBox")).Focus();
                    showMessageBox("Please fill out batch!!");
                    return false;
                }

                if (((TextBox)row.FindControl("expDateDateTextBox")).Text == "")
                {
                    ((TextBox)row.FindControl("expDateDateTextBox")).Focus();
                    showMessageBox("Please fill out exp. Date!!");
                    return false;
                }

                if (((TextBox)row.FindControl("mfgDateTextBox")).Text == "")
                {
                    ((TextBox)row.FindControl("mfgDateTextBox")).Focus();
                    showMessageBox("Please fill out Mfg. Date!!");
                    return false;
                }

                if (((TextBox)row.FindControl("reqQtyTextBox")).Text == "")
                {
                    ((TextBox)row.FindControl("reqQtyTextBox")).Focus();
                    showMessageBox("Please fill out Req.Qty!!");
                    return false;
                }
            }
        }

        if (selectedCount == 0)
        {
            showMessageBox("Please select at least one item.");
            return false;
        }

        return true;
    }

    private void SaveAllData()
    {
        try
        {
            int maxRowId;
            WarehouseStockInMasterDao aStockInMasterDao = new WarehouseStockInMasterDao();

            DateTime stockInDate = DateTime.Today;
            DateTime.TryParse(stockInDateTextBox.Text.Trim(), out stockInDate);
            aStockInMasterDao.WHStockInDate = stockInDate;

            int whId = 0;
            int.TryParse(warehouseDropDownList.SelectedValue, out whId);
            aStockInMasterDao.WarehouseId = whId;

            int supId = 0;
            int.TryParse(ddlSupplier.SelectedValue, out supId);
            aStockInMasterDao.SupplierId = supId;

            decimal totalVat = 0;
            decimal.TryParse(totalVatTextBox.Text.Trim(), out totalVat);
            aStockInMasterDao.TotalVat = totalVat;

            decimal totalQty = 0;
            decimal.TryParse(totalQtyTextBox.Text.Trim(), out totalQty);
            aStockInMasterDao.TotalQuantity = Convert.ToInt32(totalQty);

            decimal grandTotal = 0;
            decimal.TryParse(grandTotalTextBox.Text.Trim(), out grandTotal);
            aStockInMasterDao.TotalValue = grandTotal;

            aStockInMasterDao.ChallanNo = challanNoTextBox.Text.Trim();

            DateTime challanDate = DateTime.Today;
            DateTime.TryParse(challanDateTextBox.Text.Trim(), out challanDate);
            aStockInMasterDao.ChallanDate = challanDate;

            aStockInMasterDao.ReferenceNo = referenceNoTextBox.Text.Trim();
            aStockInMasterDao.ReferenceDate = referenceDateTextBox.Text != "" ? referenceDateTextBox.Text.Trim() : "";
            aStockInMasterDao.Remarks = remarksTextBox.Text.Trim();
            aStockInMasterDao.Status = "Posted";
            aStockInMasterDao.EntryBy = Session["LoginName"] != null ? Session["LoginName"].ToString() : (Session["UserId"] != null ? Session["UserId"].ToString() : "Admin");
            aStockInMasterDao.EntryDate = DateTime.Now;
            aStockInMasterDao.MaterialConversionMasterId = GetSelectedMaterialConversionMasterId();

            bool stockInSave = aWarehouseStockInBll.SaveStockInMasterInfoForMaterialConversion(aStockInMasterDao, out maxRowId);
            if (!stockInSave)
            {
                showErrorMessage("Failed to save stock-in master information.");
                return;
            }

            List<WharehouseStockInDetailsDao> aDetailList = GetSelectedDetails(maxRowId);
            string msg = aWarehouseStockInBll.SaveWarehouseStockInDetailWithMaterialConversion(aDetailList);

            Clear();
            showSuccessMessage(msg, false);
        }
        catch (Exception ex)
        {
            showErrorMessage("Error saving stock-in: " + ex.Message);
        }
    }

    private void UpdateAllData()
    {
        try
        {
            WarehouseStockInMasterDao aStockInMasterDao = new WarehouseStockInMasterDao();

            int masterId = 0;
            int.TryParse(stockInIdHiddenField.Value, out masterId);
            aStockInMasterDao.WHStockInMasterID = masterId;

            DateTime stockInDate = DateTime.Today;
            DateTime.TryParse(stockInDateTextBox.Text.Trim(), out stockInDate);
            aStockInMasterDao.WHStockInDate = stockInDate;

            int whId = 0;
            int.TryParse(warehouseDropDownList.SelectedValue, out whId);
            aStockInMasterDao.WarehouseId = whId;

            decimal totalVat = 0;
            decimal.TryParse(totalVatTextBox.Text.Trim(), out totalVat);
            aStockInMasterDao.TotalVat = totalVat;

            decimal totalQty = 0;
            decimal.TryParse(totalQtyTextBox.Text.Trim(), out totalQty);
            aStockInMasterDao.TotalQuantity = Convert.ToInt32(totalQty);

            decimal grandTotal = 0;
            decimal.TryParse(grandTotalTextBox.Text.Trim(), out grandTotal);
            aStockInMasterDao.TotalValue = grandTotal;

            aStockInMasterDao.ChallanNo = challanNoTextBox.Text.Trim();

            DateTime challanDate = DateTime.Today;
            DateTime.TryParse(challanDateTextBox.Text.Trim(), out challanDate);
            aStockInMasterDao.ChallanDate = challanDate;

            aStockInMasterDao.ReferenceNo = referenceNoTextBox.Text.Trim();

            int supId = 0;
            int.TryParse(ddlSupplier.SelectedValue, out supId);
            aStockInMasterDao.SupplierId = supId;

            aStockInMasterDao.ReferenceDate = referenceDateTextBox.Text != "" ? referenceDateTextBox.Text.Trim() : "";
            aStockInMasterDao.Remarks = remarksTextBox.Text.Trim();
            aStockInMasterDao.Status = "Posted";
            aStockInMasterDao.UpdateBy = Session["LoginName"] != null ? Session["LoginName"].ToString() : (Session["UserId"] != null ? Session["UserId"].ToString() : "Admin");
            aStockInMasterDao.UpdateDate = DateTime.Now;
            aStockInMasterDao.MaterialConversionMasterId = GetSelectedMaterialConversionMasterId();

            bool stockInSave = aWarehouseStockInBll.UpdateStockInMasterInfoForMaterialConversion(aStockInMasterDao);
            if (!stockInSave)
            {
                showErrorMessage("Failed to update stock-in master information.");
                return;
            }

            aWarehouseStockInBll.DeleteWhStockInDetailById(masterId);
            List<WharehouseStockInDetailsDao> aDetailList = GetSelectedDetails(masterId);
            string msg = aWarehouseStockInBll.SaveWarehouseStockInDetailWithMaterialConversion(aDetailList);

            Clear();
            showSuccessMessage(msg, true);
        }
        catch (Exception ex)
        {
            showErrorMessage("Error updating stock-in: " + ex.Message);
        }
    }

    private List<WharehouseStockInDetailsDao> GetSelectedDetails(int masterId)
    {
        List<WharehouseStockInDetailsDao> aDetailList = new List<WharehouseStockInDetailsDao>();

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            GridViewRow row = loadGridView.Rows[i];
            CheckBox cb = (CheckBox)row.FindControl("chkSelect");
            if (!cb.Checked)
            {
                continue;
            }

            HiddenField productidHiddenField = (HiddenField)row.FindControl("productidHiddenField");
            TextBox batchTextBox = (TextBox)row.FindControl("batchTextBox");
            TextBox expDateTextBox = (TextBox)row.FindControl("expDateDateTextBox");
            TextBox mfgDateTextBox = (TextBox)row.FindControl("mfgDateTextBox");
            TextBox quantityTextBox = (TextBox)row.FindControl("reqQtyTextBox");
            TextBox costPriceTextBox = (TextBox)row.FindControl("costPriceTextBox");
            TextBox vatTextBox = (TextBox)row.FindControl("vatTextBox");
            TextBox totalValueTextBox = (TextBox)row.FindControl("totalValueTextBox");

            int materialConversionDetailId = 0;
            if (loadGridView.DataKeys[i] != null && loadGridView.DataKeys[i]["MaterialConversionDetailId"] != null)
            {
                int.TryParse(loadGridView.DataKeys[i]["MaterialConversionDetailId"].ToString(), out materialConversionDetailId);
            }

            int pId = 0;
            if (productidHiddenField != null) int.TryParse(productidHiddenField.Value, out pId);

            DateTime expDate = DateTime.Today;
            if (expDateTextBox != null) DateTime.TryParse(expDateTextBox.Text.Trim(), out expDate);

            DateTime mfgDate = DateTime.Today;
            if (mfgDateTextBox != null) DateTime.TryParse(mfgDateTextBox.Text.Trim(), out mfgDate);

            decimal qty = 0;
            if (quantityTextBox != null) decimal.TryParse(quantityTextBox.Text.Trim(), out qty);

            decimal price = 0;
            if (costPriceTextBox != null) decimal.TryParse(costPriceTextBox.Text.Trim(), out price);

            decimal vat = 0;
            if (vatTextBox != null) decimal.TryParse(vatTextBox.Text.Trim(), out vat);

            decimal totalAmt = 0;
            if (totalValueTextBox != null) decimal.TryParse(totalValueTextBox.Text.Trim(), out totalAmt);

            WharehouseStockInDetailsDao aWharehouseStockInDetailsDao = new WharehouseStockInDetailsDao
            {
                WHStockInMasterID = masterId,
                ProductId = pId,
                Batch = batchTextBox != null ? batchTextBox.Text.Trim() : "",
                ExpDate = expDate,
                MfgDate = mfgDate,
                Qty = qty,
                Price = price,
                VAT = vat,
                TotalAmount = totalAmt,
                MaterialConversionDetailId = materialConversionDetailId
            };

            aDetailList.Add(aWharehouseStockInDetailsDao);
        }

        return aDetailList;
    }

    private int GetSelectedMaterialConversionMasterId()
    {
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            GridViewRow row = loadGridView.Rows[i];
            CheckBox cb = (CheckBox)row.FindControl("chkSelect");
            if (!cb.Checked)
            {
                continue;
            }

            object dataKeyValue = loadGridView.DataKeys[i]["MaterialConversionMasterId"];
            if (dataKeyValue != null)
            {
                return Convert.ToInt32(dataKeyValue);
            }
        }

        return 0;
    }

    private void Clear()
    {
        LoadDropdownlist();
        stockInDateTextBox.Text = string.Empty;
        challanNoTextBox.Text = string.Empty;
        challanDateTextBox.Text = string.Empty;
        referenceNoTextBox.Text = string.Empty;
        referenceDateTextBox.Text = string.Empty;
        remarksTextBox.Text = string.Empty;
        stockInIdHiddenField.Value = string.Empty;

        loadGridView.DataSource = null;
        loadGridView.DataBind();
        ddlProductionUnit.Items.Clear();
        warehouseDropDownList.Items.Clear();
        ResetSummary();

        submitButton.Text = "Submit Information";
    }

    protected void totalValueTextBox_OnTextChanged(object sender, EventArgs e)
    {
        CalculateQuantityVatAndValue();
    }

    public string GenerateParameter()
    {
        string param = " AND CM.MaterialConversionMasterId IS NOT NULL ";

        if (ddlCompany.SelectedValue != "")
        {
            param = param + "  AND CM.BusinessUnitId = '" + ddlCompany.SelectedValue + "' ";
        }

        if (ddlProductionUnit.SelectedValue != "")
        {
            param = param + " AND CM.ProductionUnitId ='" + ddlProductionUnit.SelectedValue + "'";
        }

        return param;
    }

    protected void vatTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)textBox.Parent.Parent;
        CalculateTotalValue(currentRow.RowIndex);
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            chkBoxRows.Checked = chkBoxHeader.Checked;
        }

        CalculateQuantityVatAndValue();
    }

    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProductionUnit.Items.Clear();
        warehouseDropDownList.Items.Clear();
        loadGridView.DataSource = null;
        loadGridView.DataBind();
        ResetSummary();

        if (ddlCompany.SelectedValue != "")
        {
            aReceiveDal.GetProductionUnit(ddlProductionUnit, ddlCompany.SelectedValue);
            aWarehouseStockInBll.LoadWhInformation(warehouseDropDownList, ddlCompany.SelectedValue);
        }
    }

    protected void ddlProductionUnit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPendingMaterialConversionList();
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        CalculateQuantityVatAndValue();
    }

    protected void viewLinkButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("MaterialConversionReceiveView.aspx");
    }
}
