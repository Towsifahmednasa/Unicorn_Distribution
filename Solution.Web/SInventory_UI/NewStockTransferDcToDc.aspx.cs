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
using SalesSolution.Web.DataLayer;

public partial class SInventory_UI_NewStockTransferDcToDc : System.Web.UI.Page
{
    private CommonDataLoad _CmnLoad = new CommonDataLoad();

    ChalanBLL aChalanBll =new ChalanBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            ChalanId();
            Todate();
            aChalanBll.LoadManufac(manufacDropDownList);
            PermissionForShowBtn();
        }
        
    }
    public void PermissionForShowBtn()
    {
        try
        {
            string filepath = Path.GetDirectoryName(Request.Path);
            filepath = filepath.TrimStart('\\');
            string text = Path.GetExtension(Request.Path);
            filepath = "../" + filepath + "/" + Path.GetFileName(Request.Path);
            DataTable dtuserpermission = _CmnLoad.GetPermissionForShowBtn(filepath);
            if (dtuserpermission.Rows.Count > 0)
            {

                btnSave.Visible = false;



            }

        }
        catch (Exception ex)
        {

        }

    }
    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewStockTransferDcToDc.aspx");
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
    protected void backLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("B2BTransferView.aspx");


    }

    public void Todate()
    {
        chalanDateTextBox.Text = DateTime.Today.ToString("dd-MMM-yyyy");
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)productGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)productGridView.Rows[i].Cells[0].FindControl("chkSelect");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    public void Clear()
    {
        prodctCodeTextBox.Text = string.Empty;
        chalanDateTextBox.Text = string.Empty;
        chalanNoTextBox.Text = string.Empty;
        fromComUnitAddressTextBox.Text = string.Empty;
        fromComUnitCodeTextBox.Text = string.Empty;
        fromComUnitNameTextBox.Text = string.Empty;
        toComUnitNameTextBox.Text = string.Empty;
        toComUnitCodeTextBox.Text = string.Empty;
        toComUnitAddressTextBox.Text = string.Empty;
        driverNameTextBox.Text = string.Empty;
        truckNoTextBox.Text = string.Empty;
        chalanGridView.DataSource = null;
        chalanGridView.DataBind();
        productGridView.DataSource = null;
        productGridView.DataBind();
        fromCunithiddenValue.Value = string.Empty;
    }

    public bool Validation()
    {
        if (chalanNoTextBox.Text == "")
        {
            showMessageBox("Please Insert chalanNo!!");
            return false;
        }
        if (chalanDateTextBox.Text.Trim() == "")
        {
            showMessageBox("Please Insert chalanDate!!");
            return false;
        }
        if (manufacDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Manufacturer!!");
            return false;
        }
        if (fromComUnitCodeTextBox.Text.Trim()  == "")
        {
            showMessageBox("Please Select fromComUnit!!");
            return false;
        }
        if (toComUnitCodeTextBox.Text.Trim()  == "")
        {
            showMessageBox("Please Select toComUnit!!");
            return false;
        }
        //if (chalanGridView.Rows.Count<0)
        //{
        //    showMessageBox("Please Add Product!!");
        //    return false;
        //}
        if (chalanGridView.Rows.Count < 1)
        {
            showMessageBox("Please Add Product!!");
            return false;
        }
        return true;
    }
    protected void submitButton_Click1(object sender, EventArgs e)
    {
        try
        {
            if (Validation())
            {
                ChalanInfo aChalanInfo = new ChalanInfo()
                {
                    ChalanDate = Convert.ToDateTime(chalanDateTextBox.Text.Trim()),
                    ChalanNo = chalanNoTextBox.Text.Trim(),
                    TrackNo = truckNoTextBox.Text.Trim(),
                    DriverName = driverNameTextBox.Text.Trim(),
                    FromComUnitCode = fromComUnitCodeTextBox.Text.Trim(),
                    FromComUnitName = fromComUnitNameTextBox.Text.Trim(),
                    FromComUnitAddress = fromComUnitAddressTextBox.Text.Trim(),
                    fromunitid = Convert.ToInt32(fromCunithiddenValue.Value),
                    ToComUnitCode = toComUnitCodeTextBox.Text.Trim(),
                    ToComUnitName = toComUnitNameTextBox.Text.Trim(),
                    ToComUnitAddress = toComUnitAddressTextBox.Text.Trim(),
                    ManufacId = Convert.ToInt32(manufacDropDownList.SelectedValue),
                };
                decimal totalvalue = 0;
                decimal totalvat = 0;
                decimal grandtotal = 0;
                for (int i = 0; i < chalanGridView.Rows.Count; i++)
                {
                    decimal rowQty = 0;
                    decimal rowVatUnit = 0;
                    decimal rowUnitPrice = 0;

                    decimal.TryParse(HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[2].Text).Trim(), out rowQty);
                    if (chalanGridView.DataKeys[i] != null && chalanGridView.DataKeys[i][1] != null)
                        decimal.TryParse(chalanGridView.DataKeys[i][1].ToString(), out rowVatUnit);
                    if (chalanGridView.DataKeys[i] != null && chalanGridView.DataKeys[i][2] != null)
                        decimal.TryParse(chalanGridView.DataKeys[i][2].ToString(), out rowUnitPrice);

                    totalvat += (rowQty * rowVatUnit);
                    totalvalue += (rowQty * rowUnitPrice);
                    grandtotal += (rowQty * rowUnitPrice) + (rowQty * rowVatUnit);
                }
                aChalanInfo.TotalValue = totalvalue;
                aChalanInfo.TotalVat = totalvat;
                aChalanInfo.GrandTotal = grandtotal;

                int chalanId;
                bool status = aChalanBll.SaveDataForChalanInfo(aChalanInfo, out chalanId);
                DataTable dt = aChalanBll.LoadChalanById(chalanId.ToString());
                if (dt.Rows.Count > 0)
                {
                    printChalanNoTextBox.Text = dt.Rows[0]["ChalanNo"].ToString();
                }
                List<ChalanDetail> aChalanDetailList = new List<ChalanDetail>();

                if (status == true)
                {
                    for (int i = 0; i < chalanGridView.Rows.Count; i++)
                    {
                        int dcStoreId = 0;
                        decimal rowQty = 0;
                        decimal rowVatUnit = 0;
                        decimal rowUnitPrice = 0;

                        if (chalanGridView.DataKeys[i] != null && chalanGridView.DataKeys[i][0] != null)
                            int.TryParse(chalanGridView.DataKeys[i][0].ToString(), out dcStoreId);
                        if (chalanGridView.DataKeys[i] != null && chalanGridView.DataKeys[i][1] != null)
                            decimal.TryParse(chalanGridView.DataKeys[i][1].ToString(), out rowVatUnit);
                        if (chalanGridView.DataKeys[i] != null && chalanGridView.DataKeys[i][2] != null)
                            decimal.TryParse(chalanGridView.DataKeys[i][2].ToString(), out rowUnitPrice);

                        decimal.TryParse(HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[2].Text).Trim(), out rowQty);

                        ChalanDetail aChalanDetail = new ChalanDetail()
                        {
                            ChalanId = chalanId,
                            DCStoreId = dcStoreId,
                            ProductCode = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[0].Text).Trim(),
                            ProductName = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[1].Text).Trim(),
                            Quantity = rowQty,
                            BatchNo = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[3].Text).Trim(),
                            UnitPrice = rowUnitPrice,
                            Value = rowQty * rowUnitPrice,
                            Vat = rowQty * rowVatUnit,
                            ValueWVat = (rowQty * rowUnitPrice) + (rowQty * rowVatUnit)
                        };
                        aChalanDetailList.Add(aChalanDetail);
                    }
                    if (aChalanBll.SaveDataForChalanDetail(aChalanDetailList))
                    {
                        showMessageBox("Data saved successfully!");
                        Clear();
                        Todate();
                        ChalanId();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            showMessageBox("Error saving data: " + ex.Message.Replace("'", ""));
        }
    }
    public void ChalanId()
    {
        ChalanBLL aChalanBll = new ChalanBLL();
        chalanNoTextBox.Text = aChalanBll.ChalanNoForShow();
    }
    
    protected void fromComUnitCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string unitCode = fromComUnitCodeTextBox.Text.Trim();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(unitCode))
        {
            aDataTable = aChalanBll.ComUnit(unitCode);
            if (aDataTable.Rows.Count > 0)
            {
                fromComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                fromComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                fromCunithiddenValue.Value = aDataTable.Rows[0]["ComUnitId"].ToString();
            }
            else
            {
                showMessageBox("Company Unit Information Not Found!!");
                fromComUnitNameTextBox.Text = string.Empty;
                fromComUnitAddressTextBox.Text =  string.Empty;
                fromComUnitCodeTextBox.Text = string.Empty;
            }
        }
    }
    protected void toComUnitCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string unitCode = toComUnitCodeTextBox.Text.Trim();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(unitCode))
        {
            aDataTable = aChalanBll.ComUnit(unitCode);
            if (aDataTable.Rows.Count > 0)
            {
                toComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                toComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
            }
            else
            {
                showMessageBox("Company Unit Information Not Found!!");
                toComUnitNameTextBox.Text = string.Empty;
                toComUnitAddressTextBox.Text = string.Empty;
                toComUnitCodeTextBox.Text = string.Empty;
            }
        }
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dtdata = aChalanBll.GetProductDcStore(prodctCodeTextBox.Text, fromComUnitCodeTextBox.Text);
        if (dtdata.Rows.Count>0 )
        {
            productGridView.DataSource = dtdata;
            productGridView.DataBind();
        }
        else
        {
            showMessageBox("No Product Found!!");
        }
    }

    public bool HasDCStoreId(int dcstoreId)
    {
        for (int i = 0; i < chalanGridView.Rows.Count; i++)
        {
            if (Convert.ToInt32(chalanGridView.DataKeys[i][0].ToString())==dcstoreId)
            {
                return false;
                break;
                
            }
        }
        return true;
    }
    protected void addButton_Click(object sender, EventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("DCStoreId");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("TransferQty");
        aDataTable.Columns.Add("BatchNo");
        aDataTable.Columns.Add("ExpDate");
        aDataTable.Columns.Add("VATAmountPerUnit");
        aDataTable.Columns.Add("UnitPrice");

        DataRow dataRow = null;
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)productGridView.Rows[i].FindControl("chkSelect");
            TextBox txtTransferQty = (TextBox)productGridView.Rows[i].FindControl("transferQtyTextBox");

            if (ChkBoxRows != null && ChkBoxRows.Checked && txtTransferQty != null && !string.IsNullOrEmpty(txtTransferQty.Text.Trim()))
            {
                int dcStoreId = 0;
                if (productGridView.DataKeys[i] != null && productGridView.DataKeys[i][0] != null)
                {
                    int.TryParse(productGridView.DataKeys[i][0].ToString(), out dcStoreId);
                }

                if (dcStoreId > 0 && HasDCStoreId(dcStoreId))
                {
                    dataRow = aDataTable.NewRow();
                    dataRow["ProductCode"] = HttpUtility.HtmlDecode(productGridView.Rows[i].Cells[1].Text.Trim());
                    dataRow["ProductName"] = HttpUtility.HtmlDecode(productGridView.Rows[i].Cells[2].Text.Trim());
                    dataRow["TransferQty"] = txtTransferQty.Text.Trim();
                    dataRow["BatchNo"] = HttpUtility.HtmlDecode(productGridView.Rows[i].Cells[4].Text.Trim());
                    dataRow["ExpDate"] = HttpUtility.HtmlDecode(productGridView.Rows[i].Cells[5].Text.Trim());
                    dataRow["VATAmountPerUnit"] = productGridView.DataKeys[i][1] != null ? productGridView.DataKeys[i][1].ToString() : "0";
                    dataRow["DCStoreId"] = dcStoreId.ToString();
                    dataRow["UnitPrice"] = productGridView.DataKeys[i][2] != null ? productGridView.DataKeys[i][2].ToString() : "0";

                    aDataTable.Rows.Add(dataRow);
                }
            }
        }
        for (int i = 0; i < chalanGridView.Rows.Count; i++)
        {
            dataRow = aDataTable.NewRow();
            dataRow["ProductCode"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[0].Text.Trim());
            dataRow["ProductName"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[1].Text.Trim());
            dataRow["TransferQty"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[2].Text.Trim());
            dataRow["BatchNo"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[3].Text.Trim());
            dataRow["ExpDate"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[4].Text.Trim());
            dataRow["VATAmountPerUnit"] = chalanGridView.DataKeys[i][1].ToString();
            dataRow["DCStoreId"] = chalanGridView.DataKeys[i][0].ToString();
            dataRow["UnitPrice"] = chalanGridView.DataKeys[i][2].ToString();

            aDataTable.Rows.Add(dataRow);
        }

        chalanGridView.DataSource = aDataTable;
        chalanGridView.DataBind();

        decimal total = 0;
        foreach (DataRow r in aDataTable.Rows)
        {
            decimal qty = 0;
            decimal price = 0;
            decimal.TryParse(r["TransferQty"].ToString(), out qty);
            decimal.TryParse(r["UnitPrice"].ToString(), out price);
            total += (qty * price);
        }
        grandTotalWordLabel.Text = total.ToString("N2");
    }
    protected void DeleteImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton productCodeTextBox = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)productCodeTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("DCStoreId");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("TransferQty");
        aDataTable.Columns.Add("BatchNo");
        aDataTable.Columns.Add("ExpDate");
        aDataTable.Columns.Add("VATAmountPerUnit");
        aDataTable.Columns.Add("UnitPrice");

        DataRow dataRow = null;
        for (int i = 0; i < chalanGridView.Rows.Count; i++)
        {
            if (i != rowindex)
            {
                dataRow = aDataTable.NewRow();
                dataRow["ProductCode"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[0].Text.Trim());
                dataRow["ProductName"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[1].Text.Trim());
                dataRow["TransferQty"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[2].Text.Trim());

                dataRow["BatchNo"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[3].Text.Trim());
                dataRow["ExpDate"] = HttpUtility.HtmlDecode(chalanGridView.Rows[i].Cells[4].Text.Trim());
                dataRow["VATAmountPerUnit"] = chalanGridView.DataKeys[i][1].ToString();
                dataRow["DCStoreId"] = chalanGridView.DataKeys[i][0].ToString();
                dataRow["UnitPrice"] = chalanGridView.DataKeys[i][2].ToString();

                aDataTable.Rows.Add(dataRow);
            }
        }

        chalanGridView.DataSource = aDataTable;
        chalanGridView.DataBind();

        decimal total = 0;
        foreach (DataRow r in aDataTable.Rows)
        {
            decimal qty = 0;
            decimal price = 0;
            decimal.TryParse(r["TransferQty"].ToString(), out qty);
            decimal.TryParse(r["UnitPrice"].ToString(), out price);
            total += (qty * price);
        }
        grandTotalWordLabel.Text = total.ToString("N2");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string url = "../SInventory_RPTVIEW/DcToDcChalanReportViewer.aspx?chalanno=" + printChalanNoTextBox.Text;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void dQtyTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox txtChanged = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtChanged.Parent.Parent;

        decimal transferQty = 0;
        decimal stockQty = 0;

        decimal.TryParse(txtChanged.Text.Trim(), out transferQty);
        decimal.TryParse(row.Cells[3].Text.Trim(), out stockQty);

        if (transferQty > stockQty)
        {
            showMessageBox("Transfer Qty. can't be more than Stock Quantity (" + stockQty + ")");
            txtChanged.Text = string.Empty;
            txtChanged.Focus();
        }
    }

}