using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Library.DAL.DoctorModule_DAL;
using Library.DAL.MasterSetup_DAL;
using Newtonsoft.Json;
using SalesSolution.Web.DataLayer;
public partial class MasterSetup_UI_DoctorView : System.Web.UI.Page
{

    private static SeedDataDAL _seedRepo = new SeedDataDAL();
    private static CommonDataLoad _CommonDataLoad = new CommonDataLoad();
    private CommonDataLoad _CmnLoad = new CommonDataLoad();

    private static DoctorDAL _DAL = new DoctorDAL();
    private DropDownList GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect;
    string RoleTypeName = "";
    string EmpInfoId = "";
    string ToRoleTypeId = "";
    protected void Page_Load(object sender, EventArgs e)
    {


        try
        {
            RoleTypeName = Session["RoleTypeName"].ToString();
            EmpInfoId = Session["EmpInfoId"].ToString();
            ToRoleTypeId = Session["RoleTypeId"].ToString();

        }
        catch { }
        GroupSelect = (DropDownList)IVMarketStructure.FindControl("GroupSelect") as DropDownList;
        ZoneSelect = (DropDownList)IVMarketStructure.FindControl("ZoneSelect") as DropDownList;
        AreaSelect = (DropDownList)IVMarketStructure.FindControl("AreaSelect") as DropDownList;
        TeritorySelect = (DropDownList)IVMarketStructure.FindControl("TeritorySelect") as DropDownList;
        SubTeritory = (DropDownList)IVMarketStructure.FindControl("SubTeritory") as DropDownList;
        MarketSelect = (DropDownList)IVMarketStructure.FindControl("MarketSelect") as DropDownList;

        // ✅ তারপর hide করুন
        IVMarketStructure.Visible = false;

        if (!IsPostBack)
        {

            UserPersmissionValidation();
            frmDate.Text = DateTime.Now.AddDays(-30).ToString("dd MMMM, yyyy");
            toDate.Text = DateTime.Now.ToString("dd MMMM, yyyy");

            try
            {
                using (DataTable dt = _CommonDataLoad.GetDoctorType_Active())
                {
                    DoctorTypeSelect.DataSource = dt;
                    DoctorTypeSelect.DataValueField = "DoctorTypeId";
                    DoctorTypeSelect.DataTextField = "DoctorTypeName";
                    DoctorTypeSelect.DataBind();
                    DoctorTypeSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    DoctorTypeSelect.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }

            // Sales Person

            try
            {
                using (DataTable dt = _CommonDataLoad.GetDoctorSalesPerson_forSearch())
                {
                    ddlSalesPerson.DataSource = dt;
                    ddlSalesPerson.DataValueField = "EmpInfoId";
                    ddlSalesPerson.DataTextField = "EmpName";
                    ddlSalesPerson.DataBind();
                    ddlSalesPerson.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    ddlSalesPerson.SelectedIndex = 0;
                }

            }
            catch (Exception ex) { }


            try
            {
                using (DataTable dt = _CommonDataLoad.GetDoctorDegree_All())
                {
                    degreeSelect.DataSource = dt;
                    degreeSelect.DataValueField = "DegreeId";
                    degreeSelect.DataTextField = "DegreeName";
                    degreeSelect.DataBind();
                    degreeSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    degreeSelect.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }
            

            try
            {
                using (DataTable dt = _seedRepo.GetApprovalStatusList())
                {
                    ddlApprovalStatus.DataSource = dt;
                    ddlApprovalStatus.DataValueField = "SoftwareUseId";
                    ddlApprovalStatus.DataTextField = "WebShow";
                    ddlApprovalStatus.DataBind();
                    ddlApprovalStatus.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    ddlApprovalStatus.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }


            try
            {
                using (DataTable dt = _seedRepo.GetProgramTypeListParam(" and GRP.IsDoctor=1"))
                {
                    ddlProgramType.DataSource = dt;
                    ddlProgramType.DataValueField = "ProgramTypeId";
                    ddlProgramType.DataTextField = "ProgramTypeName";
                    ddlProgramType.DataBind();
                    ddlProgramType.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    ddlProgramType.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }


            try
            {
                using (DataTable dt = _seedRepo.GetSMCTypeListParam(" and GRP.forDotor=1"))
                {
                    ddlPharmaPlatform.DataSource = dt;
                    ddlPharmaPlatform.DataValueField = "SMCTypeId";
                    ddlPharmaPlatform.DataTextField = "SMCType";
                    ddlPharmaPlatform.DataBind();
                    ddlPharmaPlatform.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    ddlPharmaPlatform.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }


           // btnSearch_Click(null, null);
        }

    }


    public void UserPersmissionValidation()
    {
        // Session expired / app restarted: go to login instead of crashing.
        if (Session["UserRoleID"] == null)
        {
            Response.Redirect("../Login.aspx");
            return;
        }
        if (Session["UserRoleID"].ToString() != "2")
        {
            try
            {
                string filepath = Path.GetDirectoryName(Request.Path);
                filepath = filepath.TrimStart('\\');
                string text = Path.GetExtension(Request.Path);
                filepath = "../" + filepath + "/" + Path.GetFileName(Request.Path);
                DataTable dtuserpermission = _CmnLoad.GetPermissionForUserRole(filepath);
                if (dtuserpermission.Rows.Count > 0)
                {
                    if (Session["UserRoleID"].ToString() != "2")
                    {
                        EmpCetegoryAddImageButton.Visible = Convert.ToBoolean(dtuserpermission.Rows[0]["RAdd"].ToString());
                        loadGridView.Columns[loadGridView.Columns.Count - 1].Visible =
                            loadGridView.Columns[1].Visible = Convert.ToBoolean(dtuserpermission.Rows[0]["REdit"].ToString());


                    }
                }
                else
                {
                    Response.Redirect("../Dashboard_UI/DashboardOne.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("../Dashboard_UI/DashboardOne.aspx");
            }
        }
    }
    protected void custNameTextBox_TextChanged(object sender, EventArgs e)
    {


        string empName = custNameTextBox.Text.Trim();
        if (empName.Contains(':'))
        {
            string[] emp = empName.Split('|');

            hfCustomerId.Value = emp[1].Trim();
            custNameTextBox.Text = emp[0].Trim();



        }
        else
        {

            custNameTextBox.Text = "";
            hfCustomerId.Value = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Input Correct Data !" + "','Faild');", true);


        }


    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {

   
              if (loadGridView.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Doctor_List_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".csv");
            Response.Charset = "";
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.Default;
            //To Export all pages.
            loadGridView.AllowPaging = false;
            this.LoadData(Parm());

             StringBuilder sb = new StringBuilder();
             List<int> visibleColIndices = new List<int>();
             for (int colIndex = 0; colIndex < loadGridView.Columns.Count; colIndex++)
             {
                 if (loadGridView.Columns[colIndex].Visible && loadGridView.Columns[colIndex].HeaderText != "Edit" && loadGridView.Columns[colIndex].HeaderText != "Action")
                 {
                     visibleColIndices.Add(colIndex);
                     sb.Append(HttpUtility.HtmlDecode(loadGridView.HeaderRow.Cells[colIndex].Text) + ',');
                 }
             }
             sb.Append("\r\n");

             foreach (GridViewRow row in loadGridView.Rows)
             {
                 foreach (int colIndex in visibleColIndices)
                 {
                     TableCell cell = row.Cells[colIndex];
                     string cellText = HttpUtility.HtmlDecode(cell.Text).Trim();
                     if (cellText.Contains(","))
                     {
                         sb.Append(String.Format("\"{0}\",", cellText));
                     }
                     else
                     {
                         sb.Append(cellText + ',');
                     }
                 }
                 sb.Append("\r\n");
             }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "No Data Found!" + "','Faild');", true);

        }
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void loadGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        loadGridView.PageIndex = e.NewPageIndex;
        this.LoadData(Parm());
    }
    // Action button is column 1 (after SL).
    protected void loadGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[1].Controls.Count > 0)
            ((WebControl)e.Row.Cells[1].Controls[0]).Attributes["onclick"] = "if(!confirm('Change Active/Inactive status of this record?')) return false;";
    }

    // Inactive rows (button says "Activate") get a red button.
    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[1].Controls.Count > 0)
        {
            var btn = (Button)e.Row.Cells[1].Controls[0];
            btn.CssClass = btn.Text == "Activate" ? "btn btn-sm btn-danger" : "btn btn-sm btn-outline-secondary";
        }
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ToggleActive")
        {
            int idx = Convert.ToInt32(e.CommandArgument);
            string code = Convert.ToString(loadGridView.DataKeys[idx]["DoctorCode"]);
            bool activate = ((Button)loadGridView.Rows[idx].Cells[1].Controls[0]).Text == "Activate";
            if (code != "")
            {
                new CustomerInfoDAL().SetActiveByCode(code, activate, Convert.ToString(Session["UserId"]));
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('" + code + (activate ? " activated." : " deactivated.") + "');", true);
            }
            LoadData(Parm());
        }
        else if (e.CommandName == "EditData")
        {

            var datKey = loadGridView.DataKeys[0];
            if (datKey != null)
            {


                string MId = e.CommandArgument.ToString();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('DoctorEntry.aspx?mid=" + MId + "' ,'_blank');", true);


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
    protected void EmpCetegoryAddImageButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("DoctorEntry.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData(Parm());
    }
    private void LoadData(string parm)
    {
        if (PerVali())
        {
            DataTable aDataTable = _DAL.GetDoctorList(parm);
            aDataTable.Columns.Add("IsActiveText", typeof(string), "IIF(ISNULL(IsActive, false), 'Yes', 'No')");
            aDataTable.Columns.Add("ToggleText", typeof(string), "IIF(ISNULL(IsActive, false), 'Deactivate', 'Activate')");
            aDataTable.Columns.Add("EntryByText", typeof(string), "IIF(ISNULL(UserEntryBy, '') IN ('', 'N/A'), 'System Admin', UserEntryBy)");
            // Never-updated rows (no UpdateDate) stay blank instead of showing System Admin.
            aDataTable.Columns.Add("UpdateByText", typeof(string), "IIF(UpdateDate IS NULL, '', IIF(ISNULL(UserUpdateBy, '') IN ('', 'N/A'), 'System Admin', UserUpdateBy))");
            aDataTable.Columns.Add("RowSL", typeof(int));
            for (int i = 0; i < aDataTable.Rows.Count; i++)
                aDataTable.Rows[i]["RowSL"] = i + 1;
            AddDetailColumns(aDataTable);
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
        lblCount.Text = "Total : " + aDataTable.Rows.Count.ToString();
        }
        else
        {
            lblCount.Text = "Total : 0";
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }
    }


    // Adds the chamber/contact/special day/speciality/degree/brand columns. How many of each
    // ("Chamber Name 1..n") depends on the doctors in the current list, so the grid columns are rebuilt per search.
    private void AddDetailColumns(DataTable list)
    {
        const string marker = "dtl";
        for (int i = loadGridView.Columns.Count - 1; i >= 0; i--)
        {
            if (loadGridView.Columns[i].AccessibleHeaderText == marker)
                loadGridView.Columns.RemoveAt(i);
        }

        int insertAt = 0;
        while (insertAt < loadGridView.Columns.Count && loadGridView.Columns[insertAt].HeaderText != "Is Active")
            insertAt++;

        ILookup<string, DataRow> rowsByDoctor = list.Rows.Cast<DataRow>().ToLookup(r => r["DoctorId"].ToString());

        foreach (DataRow detail in _DAL.GetDoctorDetailRows().Rows)
        {
            string column = detail["ColName"].ToString();
            foreach (DataRow doctor in rowsByDoctor[detail["DoctorId"].ToString()])
            {
                if (!list.Columns.Contains(column))
                {
                    list.Columns.Add(column, typeof(string));
                    loadGridView.Columns.Insert(insertAt++, new BoundField
                    {
                        DataField = column,
                        HeaderText = detail["Header"].ToString(),
                        AccessibleHeaderText = marker
                    });
                }
                doctor[column] = detail["Val"].ToString();
            }
        }
    }

    private bool PerVali()
    {
        //ZoneSelect.CssClass = "form-select form-select-sm mb-3 mySelect2";
        //AreaSelect.CssClass = "form-select form-select-sm mb-3 mySelect2";
        //TeritorySelect.CssClass = "form-select form-select-sm mb-3 mySelect2";


        //if (RoleTypeName == "DZSM")
        //{
        //    if (ZoneSelect.SelectedValue == "")
        //    {
        //        ZoneSelect.ToolTip = "please fill out this field";
        //        ZoneSelect.CssClass = "form-select form-select-sm mb-3 mySelect2 is-invalid";
        //        ZoneSelect.Focus();
        //        string text6 = "Please Select Zone!";
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showpop6('" + text6 + "')</script>", false);

        //        return false;
        //    }



        //}

        //if (RoleTypeName == "AM")
        //{
        //    if (AreaSelect.SelectedValue == "")
        //    {
        //        AreaSelect.ToolTip = "please fill out this field";
        //        AreaSelect.CssClass = "form-select form-select-sm mb-3 mySelect2 is-invalid";
        //        AreaSelect.Focus();
        //        string text6 = "Please Select Area!";
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showpop6('" + text6 + "')</script>", false);

        //        return false;
        //    }



        //}

        //if (RoleTypeName == "MIO")
        //{
        //    if (TeritorySelect.SelectedValue == "")
        //    {
        //        TeritorySelect.ToolTip = "please fill out this field";
        //        TeritorySelect.CssClass = "form-select form-select-sm mb-3 mySelect2 is-invalid";
        //        TeritorySelect.Focus();
        //        string text6 = "Please Select Territory!";
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showpop6('" + text6 + "')</script>", false);

        //        return false;
        //    }



        //}

        return true;
    }

    private string Parm()
    {
        string param = "";

        //if (GroupSelect.SelectedValue != "")
        //{
        //    param = param + " AND gr.GroupId='" + GroupSelect.SelectedValue + "' ";
        //}

        //if (ZoneSelect.SelectedValue != "")
        //{
        //    param = param + " AND rg.RegionId='" + ZoneSelect.SelectedValue + "' ";
        //}

        if (AreaSelect.SelectedValue != "")
        {
            param = param + " AND Ar.AreaId='" + AreaSelect.SelectedValue + "' ";
        }

        if (TeritorySelect.SelectedValue != "")
        {
            param = param + " AND Tr.TerritoryId='" + TeritorySelect.SelectedValue + "' ";
        }

        if (SubTeritory.SelectedValue != "")
        {
            param = param + " AND subTr.SubTerritoryId='" + SubTeritory.SelectedValue + "' ";
        }

        if (MarketSelect.SelectedValue != "")
        {
            param = param + " AND DM.MarketId='" + MarketSelect.SelectedValue + "' ";
        }

        if (ddlProgramType.SelectedValue != "")
        {
            param = param + " AND DM.ProgramTypeId='" + ddlProgramType.SelectedValue + "' ";
        }

        if (ddlPharmaPlatform.SelectedValue != "")
        {
            param = param + " AND DM.SMCTypeId='" + ddlPharmaPlatform.SelectedValue + "' ";
        }

        if (ddlSalesPerson.SelectedValue != "")
        {
            string spId = ddlSalesPerson.SelectedValue;
            param = param + " AND (DM.SalesPersonId='" + spId + "' OR (',' + ISNULL(DM.TaggedSalesPersonIds, '') + ',') LIKE '%," + spId + ",%') ";
        }

        if (DoctorTypeSelect.SelectedValue != "")
        {
            param = param + " AND DM.DoctorTypeId='" + DoctorTypeSelect.SelectedValue + "' ";
        }

        if (degreeSelect.SelectedValue != "")
        {
            param = param + " AND   DM.DoctorId in (select DoctorId from tblDoctorDegreeDetail where DegId = '" + degreeSelect.SelectedValue + "' )";
        }

        if (ddlStatus.SelectedValue != "")
        {
            param = param + " AND DM.IsActive=" + ddlStatus.SelectedValue + " ";
        }
        if (ddlApprovalStatus.SelectedValue != "")
        {
            param = param + " AND DM.ApprovalStatus='" + ddlApprovalStatus.SelectedValue + "' ";
        }
        if (hfCustomerId.Value != "")
        {
            param = param + " AND DM.DoctorId='" + hfCustomerId.Value + "' ";
        }
        if (frmDate.Text != "" && toDate.Text != "")
        {
            param = param + " AND CONVERT(date,DM.EntryDate)  BETWEEN '" + frmDate.Text + "' AND '" + toDate.Text + "' ";
        }
        if (frmDate.Text != "" && toDate.Text == "")
        {
            param = param + " AND CONVERT(date,DM.EntryDate)  BETWEEN '" + frmDate.Text + "' AND '" + DateTime.Now + "' ";
        }

        if (frmDate.Text != "" && toDate.Text == "")
        {
            param = param + " AND CONVERT(date,DM.EntryDate)  BETWEEN '" + frmDate.Text + "' AND '" + DateTime.Now + "' ";
        }

        return param;
    }

    protected void resetBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("DoctorView.aspx");
    }

}