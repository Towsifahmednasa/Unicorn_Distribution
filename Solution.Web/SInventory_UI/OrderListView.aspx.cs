using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_AreaView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    AreaBLL areaBll = new AreaBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AreaLoad();
        }
    }

    private void AreaLoad()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderWithDetail();
        //aDataTable = areaBll.LoadAreaInfo();
        loadGridView.DataSource = aTable;
        loadGridView.DataBind();
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "PreviewOrder")
        {
            string orderId = e.CommandArgument.ToString();
            ShowOrderPreview(orderId);
        }
        else if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string areaId = loadGridView.DataKeys[rowindex][0].ToString();
            PopUp(areaId);
        }
    }

    private void ShowOrderPreview(string orderId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable dt = aOrderInfoBll.LoadOrderWithDetail(orderId);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow firstRow = dt.Rows[0];
            lblModalOrderCode.Text = firstRow["OrderCode"] != DBNull.Value ? firstRow["OrderCode"].ToString() : "N/A";

            DateTime subDate;
            if (firstRow["SubmissionDate"] != DBNull.Value && DateTime.TryParse(firstRow["SubmissionDate"].ToString(), out subDate))
            {
                lblModalOrderDate.Text = subDate.ToString("dd MMMM, yyyy");
            }
            else
            {
                lblModalOrderDate.Text = "N/A";
            }

            string custCode = dt.Columns.Contains("CustomerCode") && firstRow["CustomerCode"] != DBNull.Value ? firstRow["CustomerCode"].ToString() : "";
            string custName = dt.Columns.Contains("CustomerName") && firstRow["CustomerName"] != DBNull.Value ? firstRow["CustomerName"].ToString() : "";
            lblModalCustomer.Text = (!string.IsNullOrEmpty(custCode) ? custCode + " - " : "") + custName;

            string dcCode = dt.Columns.Contains("ComUnitCode") && firstRow["ComUnitCode"] != DBNull.Value ? firstRow["ComUnitCode"].ToString() : "";
            string dcName = dt.Columns.Contains("ComUnitName") && firstRow["ComUnitName"] != DBNull.Value ? firstRow["ComUnitName"].ToString() : "";
            lblModalDC.Text = (!string.IsNullOrEmpty(dcCode) ? dcCode + " : " : "") + dcName;

            string mioCode = dt.Columns.Contains("MIOCode") && firstRow["MIOCode"] != DBNull.Value ? firstRow["MIOCode"].ToString() : "";
            string mioName = dt.Columns.Contains("MIOName") && firstRow["MIOName"] != DBNull.Value ? firstRow["MIOName"].ToString() : "";
            lblModalSalesPerson.Text = (!string.IsNullOrEmpty(mioCode) ? mioCode + " : " : "") + mioName;

            lblModalAddress.Text = dt.Columns.Contains("Address") && firstRow["Address"] != DBNull.Value ? firstRow["Address"].ToString() : "N/A";
            lblModalCellNo.Text = dt.Columns.Contains("CellNo") && firstRow["CellNo"] != DBNull.Value ? firstRow["CellNo"].ToString() : "N/A";
            
            string remarks = dt.Columns.Contains("Remarks") && firstRow["Remarks"] != DBNull.Value ? firstRow["Remarks"].ToString() : "";
            lblModalRemarks.Text = !string.IsNullOrEmpty(remarks) ? remarks : "N/A";

            decimal totalQty = 0;
            decimal totalTP = 0;
            decimal totalVAT = 0;
            decimal netPayable = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row.Table.Columns.Contains("Quantity") && row["Quantity"] != DBNull.Value)
                    totalQty += Convert.ToDecimal(row["Quantity"]);
                if (row.Table.Columns.Contains("TotalTradePrice") && row["TotalTradePrice"] != DBNull.Value)
                    totalTP += Convert.ToDecimal(row["TotalTradePrice"]);
                if (row.Table.Columns.Contains("TotalVatAmount") && row["TotalVatAmount"] != DBNull.Value)
                    totalVAT += Convert.ToDecimal(row["TotalVatAmount"]);
                if (row.Table.Columns.Contains("NetAmount") && row["NetAmount"] != DBNull.Value)
                    netPayable += Convert.ToDecimal(row["NetAmount"]);
            }

            lblModalTotalQty.Text = totalQty.ToString("N0");
            lblModalTotalTP.Text = "Tk " + totalTP.ToString("N2");
            lblModalTotalVAT.Text = "Tk " + totalVAT.ToString("N2");
            lblModalNetPayable.Text = "Tk " + (netPayable > 0 ? netPayable : (totalTP + totalVAT)).ToString("N2");

            lnkModalEdit.NavigateUrl = "ManualOrderCreation.aspx?OrderId=" + orderId;

            gvModalItems.DataSource = dt;
            gvModalItems.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenOrderPreviewModal", "var myModal = new bootstrap.Modal(document.getElementById('orderPreviewModal')); myModal.show();", true);
        }
    }

    private void PopUp(string Id)
    {
        string url = "AreaEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void areaInfoNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("AreaEntry.aspx");
    }
    protected void areaReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        AreaLoad();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.Rows.Count > 0)
        {
            gv.UseAccessibleHeader = true;
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            gv.HeaderRow.CssClass = "table-light";
            if (gv.FooterRow != null)
            {
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        if (loadGridView.Rows.Count > 0)
        {
            string attachment = "attachment; filename=PendingOrderList.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            loadGridView.AllowPaging = false;



            //loadGridView.Columns[loadGridView.Columns.Count - 1].Visible =
            //            false;
            //loadGridView.Columns[loadGridView.Columns.Count - 2].Visible =
            //   false;
            //loadGridView.Columns[loadGridView.Columns.Count - 3].Visible =
            //   false;

            this.AreaLoad();

            // Create a form to contain the grid  
            HtmlForm frm = new HtmlForm();
            loadGridView.Parent.Controls.Add(frm);
            //frm.Attributes["runat"] = "server";
            //frm.Controls.Add(loadGridView);
            //frm.RenderControl(htw);

            loadGridView.HeaderRow.Style.Add("background-color", "#E5EEF1");

            // Set background color of each cell of GridView1 header row
            foreach (TableCell tableCell in loadGridView.HeaderRow.Cells)
            {
                tableCell.Style["background-color"] = "#E5EEF1";
            }

            // Set background color of each cell of each data row of GridView1
            foreach (GridViewRow gridViewRow in loadGridView.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;

                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "#FFFFFF";

                }
            }


            loadGridView.RenderControl(htw);
            string headerTable = @"Pending Order List";

            string SubTi = @"<span   style='text-align:center'>
   <h3>Pending Order List</h3>

</span>";

            HttpContext.Current.Response.Write(headerTable);
            HttpContext.Current.Response.Write(SubTi);
            Response.Write(sw.ToString());
            Response.End();
        }
        else
        {
            //showMessageBox("No Data Found!!");
        }
    }
}