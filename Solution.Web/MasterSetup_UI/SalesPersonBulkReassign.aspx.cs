using Library.DAL.MasterSetup_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetup_UI_SalesPersonBulkReassign : System.Web.UI.Page
{
    private static CustomerInfoDAL _dal = new CustomerInfoDAL();

    private bool IsDoctor { get { return rbType.SelectedValue == "1"; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        using (DataTable dt = _dal.GetSalesPersonsForReassign())
        {
            lbFrom.DataSource = dt;
            lbFrom.DataValueField = "EmpInfoId";
            lbFrom.DataTextField = "EmpName";
            lbFrom.DataBind();

            ddlTo.DataSource = dt;
            ddlTo.DataValueField = "EmpInfoId";
            ddlTo.DataTextField = "EmpName";
            ddlTo.DataBind();
            ddlTo.Items.Insert(0, new ListItem("-- Select Sales Person --", ""));
        }
    }

    private void Msg(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + message.Replace("'", "") + "','Faild');", true);
    }

    private List<int> FromIds()
    {
        return lbFrom.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => int.Parse(i.Value)).ToList();
    }

    private void ClearGrid()
    {
        gvList.DataSource = null;
        gvList.DataBind();
    }

    protected void gvList_PreRender(object sender, EventArgs e)
    {
        if (gvList.Rows.Count > 0) gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var fromIds = FromIds();
        if (fromIds.Count == 0) { ClearGrid(); Msg("Please select at least one From Sales Person!"); return; }

        gvList.DataSource = _dal.GetBulkReassignList(IsDoctor, fromIds);
        gvList.DataBind();
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        bool all = ((CheckBox)gvList.HeaderRow.FindControl("chkAll")).Checked;
        foreach (GridViewRow row in gvList.Rows)
            ((CheckBox)row.FindControl("chkSelect")).Checked = all;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var fromIds = FromIds();
        var ids = gvList.Rows.Cast<GridViewRow>()
            .Where(r => ((CheckBox)r.FindControl("chkSelect")).Checked)
            .Select(r => Convert.ToInt32(gvList.DataKeys[r.RowIndex][0])).ToList();

        if (ids.Count == 0) { Msg("Please select at least one row!"); return; }
        if (ddlTo.SelectedValue == "") { Msg("Please select the To Sales Person!"); return; }

        try
        {
            _dal.BulkReassignSalesPerson(IsDoctor, ids, fromIds, int.Parse(ddlTo.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Operation successful!','Success','SalesPersonBulkReassign.aspx');", true);
        }
        catch (Exception ex)
        {
            Msg("Operation Faild! " + ex.Message);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalesPersonBulkReassign.aspx");
    }
}

