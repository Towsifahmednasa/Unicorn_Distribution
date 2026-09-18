using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CreatePickingDetailForWareHouse : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    ProductBLL aProductBll = new ProductBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string reqId = Request.QueryString["ReqId"];


            DataTable aTableexists = new DataTable();
            aTableexists = aRequisitionBll.GetRequisitionInfoTransferReqId(reqId);
            if (aTableexists.Rows.Count > 0)
            {
                showMessageBox(" Picking Already Completed!!");
            }
            else
            {
                LoadGrid(reqId);
                LoadMainInfo(reqId);
            }
           
        }
       
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

    private void LoadMainInfo(string reqId)
    {
        
            DataTable aTableInfo = new DataTable();
            aTableInfo = aRequisitionBll.GetRequisitionInfoByReqId(reqId);
            hdReqId.Value = aTableInfo.Rows[0]["ReqId"].ToString();
            reqNoTextBox.Text = aTableInfo.Rows[0]["ReqNo"].ToString();
            reqDateTextBox.Text = Convert.ToDateTime(aTableInfo.Rows[0]["ReqDate"].ToString()).ToString("dd-MMM-yyyy");
            clnNoTextBox.Text = aRequisitionBll.Chalan(reqNoTextBox.Text.Trim());
            clnDateTextBox.Text = Convert.ToDateTime(System.DateTime.Today.ToString()).ToString("dd-MMM-yyyy");

        bool IsFromBat = false;

        try
        {
            IsFromBat=  Convert.ToBoolean(aTableInfo.Rows[0]["IsFromBatch"].ToString());
        }
        catch { }

        hfIsFromBatch.Value = IsFromBat.ToString();


    }

    private void LoadGrid(string reqId)
    {
        aDataTable.Columns.Add("ReqChildId");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("ReqQty");
        aDataTable.Columns.Add("CurrentStockQty");
        aDataTable.Columns.Add("ProductUnitPrice");
        aDataTable.Columns.Add("IssueQty");
        aDataTable.Columns.Add("Price");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("BonusTypeId");

        // Load Sto Info 

        aDataTable = aRequisitionBll.GetPickingDataForIssue(reqId);

        issueGridView.DataSource = aDataTable;
        issueGridView.DataBind();

        for (int i = 0; i < aDataTable.Rows.Count; i++)
        {
            decimal CS = 0;
            decimal.TryParse(aDataTable.Rows[i]["CurrentStockQty"].ToString(), out CS);
            if (CS < 1)
            {
                CheckBox cb = (CheckBox)issueGridView.Rows[i].FindControl("issueCheckBox");
                if (cb != null) cb.Enabled = false;
                issueGridView.Rows[i].BackColor = Color.FromArgb(255, 230, 230);
            }
        }

    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)issueGridView.HeaderRow.FindControl("chkSelectAll");
        bool isChecked = (ChkBoxHeader != null && ChkBoxHeader.Checked);
        for (int i = 0; i < issueGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)issueGridView.Rows[i].FindControl("issueCheckBox");
            if (ChkBoxRows != null && ChkBoxRows.Enabled)
            {
                ChkBoxRows.Checked = isChecked;
                TextBox issueQtyTextBox = (TextBox)issueGridView.Rows[i].FindControl("issueQtyTextBox");
                if (issueQtyTextBox != null)
                {
                    issueQtyTextBox.Enabled = isChecked;
                }
                if (isChecked)
                {
                    PriceCalculation(i);
                }
            }
        }
        GetCalculationValue();
    }
    protected void issueCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)(((CheckBox)sender).Parent.Parent);
        int selRowIndex = row.RowIndex;
        CheckBox cb = (CheckBox)row.FindControl("issueCheckBox");

        if (cb != null && cb.Checked)
        {
            TextBox issueQtyTextBox = (TextBox)row.FindControl("issueQtyTextBox");
            if (issueQtyTextBox != null) issueQtyTextBox.Enabled = true;
            PriceCalculation(selRowIndex);
        }
        else
        {
            TextBox issueQtyTextBox = (TextBox)row.FindControl("issueQtyTextBox");
            if (issueQtyTextBox != null) issueQtyTextBox.Enabled = false;
            GetCalculationValue();
        }
    }
    protected void issueQtyTextBox_TextChanged(object sender, EventArgs e)
    {
        int setRowIndex = ((GridViewRow)(((TextBox)sender).Parent.Parent)).RowIndex;
        PriceCalculation(setRowIndex);
    }
    private void PriceCalculation(int rowIndex)
    {
        DataTable aDataTableProductPriceCase = new DataTable();
        aDataTableProductPriceCase =
            aProductBll.ProductPriceDetailWithCaseBLL(issueGridView.Rows[rowIndex].Cells[0].Text.Trim());

        TextBox issueQtyTextBox = (TextBox)issueGridView.Rows[rowIndex].FindControl("issueQtyTextBox");

        if (issueQtyTextBox != null)
        {
            if (string.IsNullOrEmpty(issueQtyTextBox.Text.Trim()))
            {
                issueQtyTextBox.Text = issueGridView.Rows[rowIndex].Cells[5].Text.Trim();
            }
            decimal enteredQty = 0;
            decimal reqQty = 0;
            decimal currStock = 0;
            decimal.TryParse(issueQtyTextBox.Text.Trim(), out enteredQty);
            decimal.TryParse(issueGridView.Rows[rowIndex].Cells[5].Text.Trim(), out reqQty);
            decimal.TryParse(issueGridView.Rows[rowIndex].Cells[3].Text.Trim(), out currStock);

            if (enteredQty > reqQty)
            {
                enteredQty = reqQty;
                issueQtyTextBox.Text = enteredQty.ToString();
            }
            if (enteredQty > currStock)
            {
                enteredQty = currStock;
                issueQtyTextBox.Text = enteredQty.ToString();
            }

            decimal issueQty = enteredQty;
            decimal unitPrice = 0;
            decimal vatPerUnit = 0;
            decimal pcsPerCase = 1;
            decimal casePerPack = 1;

            if (aDataTableProductPriceCase.Rows.Count > 0)
            {
                decimal.TryParse(aDataTableProductPriceCase.Rows[0]["WeightedAvgPrice"].ToString(), out unitPrice);
                decimal.TryParse(aDataTableProductPriceCase.Rows[0]["VATAmountPerUnit"].ToString(), out vatPerUnit);
                decimal.TryParse(aDataTableProductPriceCase.Rows[0]["PcsPerCase"].ToString(), out pcsPerCase);
                decimal.TryParse(aDataTableProductPriceCase.Rows[0]["CaseQty"].ToString(), out casePerPack);
            }
            if (unitPrice == 0)
            {
                decimal.TryParse(issueGridView.Rows[rowIndex].Cells[4].Text.Trim(), out unitPrice);
            }

            if (pcsPerCase <= 0) pcsPerCase = 1;
            decimal caseQty = decimal.Round((issueQty / pcsPerCase) * casePerPack, 2);

            TextBox priceTextBox = (TextBox)issueGridView.Rows[rowIndex].FindControl("priceTextBox");
            if (priceTextBox != null) priceTextBox.Text = (issueQty * unitPrice).ToString("0.00");

            TextBox vatTextBox = (TextBox)issueGridView.Rows[rowIndex].FindControl("vatTextBox");
            if (vatTextBox != null) vatTextBox.Text = (vatPerUnit * issueQty).ToString("0.00");

            TextBox totalPriceTextBox = (TextBox)issueGridView.Rows[rowIndex].FindControl("totalPriceTextBox");
            if (totalPriceTextBox != null)
            {
                decimal p = 0, v = 0;
                if (priceTextBox != null) decimal.TryParse(priceTextBox.Text.Trim(), out p);
                if (vatTextBox != null) decimal.TryParse(vatTextBox.Text.Trim(), out v);
                totalPriceTextBox.Text = (p + v).ToString("0.00");
            }

            TextBox caseTextBox = (TextBox)issueGridView.Rows[rowIndex].FindControl("caseTextBox");
            if (caseTextBox != null) caseTextBox.Text = caseQty.ToString();
        }

        GetCalculationValue();
    }

    private void GetCalculationValue()
    {
        decimal totalPrice = 0;
        decimal totalVat = 0;
        decimal grandTotal = 0;
        for (int i = 0; i < issueGridView.Rows.Count; i++)
        {
            GridViewRow row = issueGridView.Rows[i];
            CheckBox cb = (CheckBox)row.FindControl("issueCheckBox");

            if (cb != null && cb.Checked)
            {
                TextBox priceTextBox1 = (TextBox)row.FindControl("priceTextBox");
                decimal p = 0;
                if (priceTextBox1 != null) decimal.TryParse(priceTextBox1.Text.Trim(), out p);
                totalPrice += p;

                TextBox vatTextBox1 = (TextBox)row.FindControl("vatTextBox");
                decimal v = 0;
                if (vatTextBox1 != null) decimal.TryParse(vatTextBox1.Text.Trim(), out v);
                totalVat += v;

                TextBox totalPriceTextBox1 = (TextBox)row.FindControl("totalPriceTextBox");
                decimal t = 0;
                if (totalPriceTextBox1 != null) decimal.TryParse(totalPriceTextBox1.Text.Trim(), out t);
                grandTotal += t;
            }
        }
        totalAllPriceTextBox.Text = totalPrice.ToString("0.00");
        vatAllPriceTextBox.Text = totalVat.ToString("0.00");
        grandTotalTextBox.Text = grandTotal.ToString("0.00");
    }


    private void SaveData()
   {
       Requesition aRequesition = new Requesition();
       aRequesition.ReqId = Convert.ToInt32(hdReqId.Value);
       aRequesition.IssueChalanNo = clnNoTextBox.Text.Trim();
       aRequesition.IssuChalanDate = Convert.ToDateTime(clnDateTextBox.Text.Trim());
       aRequesition.TruckNo = truckNoTextBox.Text.Trim();
       aRequesition.DriverName = driverNameTextBox.Text.Trim();
       aRequesition.Submit = "OK";
       aRequesition.SubmitDate = Convert.ToDateTime(clnDateTextBox.Text.Trim());
       aRequesition.TotalPrice = Convert.ToDecimal(string.IsNullOrEmpty(totalAllPriceTextBox.Text.Trim()) ? "0" : totalAllPriceTextBox.Text.Trim());
       aRequesition.TotalVAT = Convert.ToDecimal(string.IsNullOrEmpty(vatAllPriceTextBox.Text.Trim()) ? "0" : vatAllPriceTextBox.Text.Trim());
       aRequesition.GrandTotalPrice = Convert.ToDecimal(string.IsNullOrEmpty(grandTotalTextBox.Text.Trim()) ? "0" : grandTotalTextBox.Text.Trim());
       aRequesition.CreatePicking = "OK";
       aRequesition.PickingNo = clnNoTextBox.Text.Trim();
       aRequesition.PickingDate = Convert.ToDateTime(clnDateTextBox.Text.Trim());


       bool updateReq = aRequisitionBll.UpdatePickingInformationOnRequisition(aRequesition);

       List<RequsitionChild> aRequsitionChildrenList = new List<RequsitionChild>();
       for (int i = 0; i < issueGridView.Rows.Count; i++)
       {
            GridViewRow row = issueGridView.Rows[i];
            CheckBox cb = (CheckBox)row.FindControl("issueCheckBox");

            if (cb != null && cb.Checked)
            {
                RequsitionChild aRequsitionChild = new RequsitionChild();
                aRequsitionChild.ProductCode = row.Cells[0].Text.Trim();
                aRequsitionChild.ProductName = row.Cells[1].Text.Trim();
                aRequsitionChild.PackSize = row.Cells[2].Text.Trim();
                decimal uPrice = 0;
                decimal.TryParse(row.Cells[4].Text.Trim(), out uPrice);
                aRequsitionChild.UnitPrice = uPrice;
                aRequsitionChild.ReqId = Convert.ToInt32(hdReqId.Value);
                aRequsitionChild.ReqChildId = Convert.ToInt32(issueGridView.DataKeys[i][0].ToString());
                
                TextBox issueQtyTextBox = (TextBox)row.FindControl("issueQtyTextBox");
                decimal iQty = 0;
                if (issueQtyTextBox != null) decimal.TryParse(issueQtyTextBox.Text.Trim(), out iQty);
                aRequsitionChild.IssueQty = iQty;
                
                TextBox priceTextBox = (TextBox)row.FindControl("priceTextBox");
                decimal pAmt = 0;
                if (priceTextBox != null) decimal.TryParse(priceTextBox.Text.Trim(), out pAmt);
                aRequsitionChild.PriceAmount = pAmt;
                
                TextBox vatTextBox = (TextBox)row.FindControl("vatTextBox");
                decimal vAmt = 0;
                if (vatTextBox != null) decimal.TryParse(vatTextBox.Text.Trim(), out vAmt);
                aRequsitionChild.VATAmount = vAmt;
                
                TextBox totalPriceTextBox = (TextBox)row.FindControl("totalPriceTextBox");
                decimal tAmt = 0;
                if (totalPriceTextBox != null) decimal.TryParse(totalPriceTextBox.Text.Trim(), out tAmt);
                aRequsitionChild.TotalPrice = tAmt;

                aRequsitionChild.IsPicking = "OK";

                HiddenField hfBatchNO = (HiddenField)row.FindControl("hfBatchNO");
                aRequsitionChild.BatchNo = hfBatchNO != null ? hfBatchNO.Value : "";

                TextBox caseTextBox = (TextBox)row.FindControl("caseTextBox");
                decimal cQty = 0;
                if (caseTextBox != null) decimal.TryParse(caseTextBox.Text.Trim(), out cQty);
                aRequsitionChild.CaseQty = cQty;

                aRequsitionChildrenList.Add(aRequsitionChild);
            }
       }

       string msg = aRequisitionBll.UpdateIssueInformationOnRequisitionChild(Convert.ToBoolean(hfIsFromBatch.Value), aRequsitionChildrenList);

       Clear();
       showMessageBox(msg);
   }

    private void Clear()
    {
        reqNoTextBox.Text = "";
        reqDateTextBox.Text = "";
        truckNoTextBox.Text = "";
        driverNameTextBox.Text = "";
        clnDateTextBox.Text = "";
        clnNoTextBox.Text = "";
        issueGridView.DataSource = null;
        issueGridView.DataBind();
        totalAllPriceTextBox.Text = "";
        vatAllPriceTextBox.Text = "";
        grandTotalTextBox.Text = "";


    }
    public bool Validation()
    {
        int CK = 0;
        for (int j = 0; j < issueGridView.Rows.Count; j++)
        {
            CheckBox cbReject = (CheckBox)issueGridView.Rows[j].FindControl("issueCheckBox");
            if (cbReject.Checked)
            {
                CK = CK + 1;
            }
        }
        if ( CK == 0)
        {
            showMessageBox("Please Select an item from List!!");
            return false;
        }
        return true;
    }
    protected void bmitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            SaveData();
            printButton_Click();
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreatePickingOnWareHouse.aspx");
    }

    public void printButton_Click()
    {
     //   int iRowIndex = (((Button)sender).Parent.Parent as GridViewRow).RowIndex;
     //   string reqId = reportListGridView.DataKeys[iRowIndex][0].ToString();

        string url = "../SInventory_RPTVIEW/ChallanReportViewer.aspx?reqId=" + Convert.ToInt32(hdReqId.Value);
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
}