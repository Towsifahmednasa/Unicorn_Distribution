using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_ProductView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    ProductBLL aProductBLL = new ProductBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDowns();
            ProductLoad();
        }
    }

    private void BindDropDowns()
    {
        aProductBLL.LoadProductCategory(categoryDropDownList);
        categoryDropDownList.Items.Insert(0, new ListItem("--All Categories--", "0"));

        aProductBLL.LoadGenericGroup(genericGroupDropDownList);
        genericGroupDropDownList.Items.Insert(0, new ListItem("--All Generic Groups--", "0"));

        aProductBLL.LoadProductLine(productLineDropDownList);
        productLineDropDownList.Items.Insert(0, new ListItem("--All Product Lines--", "0"));
    }

    private void ProductLoad()
    {
        string categoryId = categoryDropDownList.SelectedValue;
        string genericGroupId = genericGroupDropDownList.SelectedValue;
        string productLineId = productLineDropDownList.SelectedValue;

        aDataTable = aProductBLL.LoadProductSearch(categoryId, genericGroupId, productLineId);
        
        try
        {
            string logPath = HttpContext.Current.Server.MapPath("~/SInventory_UI/debug_product.txt");
            System.IO.File.WriteAllText(logPath, string.Format("categoryId: '{0}', genericGroupId: '{1}', productLineId: '{2}', rows: {3}, time: {4}", categoryId, genericGroupId, productLineId, aDataTable.Rows.Count, DateTime.Now));
        }
        catch { }

        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        ProductLoad();
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        productLineDropDownList.SelectedIndex = 0;
        categoryDropDownList.SelectedIndex = 0;
        genericGroupDropDownList.SelectedIndex = 0;
        ProductLoad();
    }

    private void PopUp(string Id)
    {
        string url = "ProductEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string productId = loadGridView.DataKeys[rowindex][0].ToString();

            string url = "ProductEntry.aspx?ID=" + productId;
            Response.Redirect(url);
        }
    }
    protected void miaTargetReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ProductLoad();
    }
    protected void miaTargetNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProductEntry.aspx");
    }

    protected void EmpCetegoryAddImageButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductEntry.aspx");
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
}