using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_MaterialConversionReceiveView : System.Web.UI.Page
{
    private readonly WarehouseStockInBll aWarehouseStockInBll = new WarehouseStockInBll();
    private readonly MaterialConversionReceiveDal aReceiveDal = new MaterialConversionReceiveDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGridView();
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

    private void LoadGridView()
    {
        DataTable aDataTable = aReceiveDal.GetMaterialConversionReceiveData();
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    protected void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void printButton_Click(object sender, EventArgs e)
    {
        int iRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
        string reqId = loadGridView.DataKeys[iRowIndex][0].ToString();

        Session["ReportType"] = "";
        Session["ReportType"] = "STD";

        string url = "../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=" + reqId;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top='+Mtop+', left='+Mleft+'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string stockInId = loadGridView.DataKeys[rowindex][0].ToString();
            Session["IDInformation"] = stockInId;
            Response.Redirect("MaterialConversionReceive.aspx");
        }

        if (e.CommandName == "DeleteData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string stockInId = loadGridView.DataKeys[rowindex][0].ToString();
            
            aWarehouseStockInBll.DeleteWhStockInInfoById(stockInId);
            ShowMessageBox("Welldone! Material Conversion Receive Information Deleted Successfully!!!");
        }

        LoadGridView();
    }

    protected void addNewLinkButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("MaterialConversionReceive.aspx");
    }
}
