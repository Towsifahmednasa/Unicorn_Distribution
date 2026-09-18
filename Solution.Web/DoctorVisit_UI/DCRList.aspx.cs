using Library.DAL.DoctorModule_DAL;
using Library.DAL.DoctorVisit_DAL;
using Newtonsoft.Json;
using SalesSolution.Web.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Library.DAL.MasterSetup_DAL;
using Library.DAO.MasterSetup_DAO;
using SalesSolution.Web.Models;
using System.Web.UI.WebControls;

public partial class DoctorVisit_UI_DCRList : System.Web.UI.Page
{
    public static DoctorVisitDAL _DoctorVisit_DAL = new DoctorVisitDAL();
    static CommonDataLoad _dataLoad = new CommonDataLoad();
    public static SetupDAL _setupDAL = new SetupDAL();
    static SeedDataDAL _seedRepo = new SeedDataDAL();
    string RoleTypeName = "";
    string EmpInfoId = "";
    string ToRoleTypeId = "";
    string ApprovalStatus = "";

    private DropDownList F_GroupSelect, F_ZoneSelect, F_AreaSelect, F_TeritorySelect;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            F_GroupSelect = (DropDownList)IVMarketStructure.FindControl("GroupSelect") as DropDownList;
            F_ZoneSelect = (DropDownList)IVMarketStructure.FindControl("ZoneSelect") as DropDownList;
            F_AreaSelect = (DropDownList)IVMarketStructure.FindControl("AreaSelect") as DropDownList;
            F_TeritorySelect = (DropDownList)IVMarketStructure.FindControl("TeritorySelect") as DropDownList;

            IVMarketStructure.Visible = false;

            RoleTypeName = Session["RoleTypeName"].ToString();
            EmpInfoId = Session["EmpInfoId"].ToString();
            ToRoleTypeId = Session["RoleTypeId"].ToString();
            if (!IsPostBack)
            {
                FromDate.Text = DateTime.Now.ToString("dd MMMM, yyyy");
                ToDate.Text = DateTime.Now.ToString("dd MMMM, yyyy");
                LoadInitialInfo();

                LoadData();
            }
        }
        catch (Exception ex)
        {
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
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string unitPriceId = loadGridView.DataKeys[rowindex][0].ToString();

            Response.Redirect("DCRReport.aspx?id=" + unitPriceId);
        }

        if (e.CommandName == "ApproveData" || e.CommandName == "RejectData")
        {
            string status = e.CommandName == "ApproveData" ? "Accepted" : "Rejected";
            int rowindex = Convert.ToInt32(e.CommandArgument);
            int dcrId = Convert.ToInt32(loadGridView.DataKeys[rowindex][0].ToString());

            // Fetch approval details
            DataTable dtApp = GetLatestDCRApprovalLog(dcrId);
            if (dtApp.Rows.Count > 0)
            {
                DataRow dr = dtApp.Rows[0];
                int dcrApprovalId = Convert.ToInt32(dr["DCRApprovalId"]);
                int toEmpId = Convert.ToInt32(dr["ToEmpId"]);
                int step = Convert.ToInt32(dr["Step"]);
                int groupId = Convert.ToInt32(dr["GroupId"]);
                int regionId = Convert.ToInt32(dr["RegionId"]);
                int areaId = Convert.ToInt32(dr["AreaId"]);
                int territoryId = Convert.ToInt32(dr["TerritoryId"]);

                CustomerInfoDAL customerInfoDAL = new CustomerInfoDAL();
                OrderSaveApprovalLogDAO aMaster = new OrderSaveApprovalLogDAO();
                aMaster.OrderApprovalId = dcrApprovalId;
                aMaster.TableId = dcrId;
                aMaster.GroupId = groupId;
                aMaster.RegionId = regionId;
                aMaster.AreaId = areaId;
                aMaster.TerritoryId = territoryId;

                aMaster.FromEmpId = EmpInfoId == "" ? 0 : Convert.ToInt32(EmpInfoId);
                aMaster.ToEmpId = toEmpId;
                aMaster.Step = step + 1;
                aMaster.Type = "DCR";
                aMaster.Status = status;
                aMaster.Date = DateTime.Now;
                aMaster.EntryDateS = DateTime.Now;
                aMaster.ApproveDateS = DateTime.Now;
                aMaster.EntryByS = Convert.ToInt32(Session["UserId"].ToString());
                aMaster.EntryByApp = Convert.ToInt32(Session["UserId"].ToString());
                aMaster.MenuId = 382;

                ResultInfo Res = customerInfoDAL.SaveDCR_Applog_DAL(aMaster);
                if (Res.isSuccess)
                {
                    string msg = status == "Accepted" ? "Approved" : "Rejected";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "ShowSuccesalert('" + "Operation " + msg + " successful!" + "','Success');", true);
                    LoadData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Operation failed!" + "','Faild');", true);
                }
            }
        }
    }

    private DataTable GetLatestDCRApprovalLog(int dcrId)
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SalesDB"].ConnectionString;
        using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
        {
            string query = @"
                SELECT TOP 1 
                    ISNULL(L.DCRApprovalId, 0) AS DCRApprovalId,
                    DCR.DcrId AS TableId,
                    ISNULL(L.ToEmpId, 0) AS ToEmpId,
                    ISNULL(L.Step, 0) AS Step,
                    ISNULL(L.GroupId, DCR.GroupId) AS GroupId,
                    ISNULL(L.RegionId, DCR.RegionId) AS RegionId,
                    ISNULL(L.AreaId, DCR.AreaId) AS AreaId,
                    ISNULL(L.TerritoryId, DCR.TerritoryId) AS TerritoryId
                FROM dbo.tbl_DCRInfo DCR
                LEFT JOIN dbo.tblDCRApprovalLog L ON L.TableId = DCR.DcrId
                WHERE DCR.DcrId = @DcrId
                ORDER BY L.DCRApprovalId DESC";
            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DcrId", dcrId);
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();
    }
    private void LoadData()
    {
        DataTable aDataTable = _DoctorVisit_DAL.GetDCRList(param());
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();

       }
    private string param()
    {


        var param = "  ";

        if (FromDate.Text != "" && ToDate.Text != "")
        {
            param = param + " AND CONVERT(date,DCR.EntryDate)  BETWEEN '" + FromDate.Text + "' AND '" + ToDate.Text + "' ";
        }
        if (FromDate.Text != "" && ToDate.Text == "")
        {
            param = param + " AND CONVERT(date,DCR.EntryDate)  BETWEEN '" + FromDate.Text + "' AND '" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ";
        }


        if (ApprovalStatusSelect.SelectedValue != "")
        {

            param = param + " AND DCR.ApprovalStatus='" + ApprovalStatusSelect.SelectedValue + "'";


        }

        if (UserRoleSelect.SelectedValue != "")
        {

            param = param + " AND us.UserRoleID='" + UserRoleSelect.SelectedValue + "'";

        }

        if (EmployeeIdSelect.SelectedValue != "")
        {

            param = param + " AND dtl.EmpInfoId='" + EmployeeIdSelect.SelectedValue + "'";

        }

        if (ddlPharmaPlatform.SelectedValue != "")
        {

            param = param + " AND DCR.SmcTypeId_Dcr='" + ddlPharmaPlatform.SelectedValue + "'";

        }

        if (F_ZoneSelect.SelectedValue != "")
        {

            param = param + " AND DCR.RegionId='" + F_ZoneSelect.SelectedValue + "'";

        }

        if (F_AreaSelect.SelectedValue != "")
        {

            param = param + " AND DCR.AreaId='" + F_AreaSelect.SelectedValue + "'";

        }

        if (F_TeritorySelect.SelectedValue != "")
        {

            param = param + " AND DCR.TerritoryId='" + F_TeritorySelect.SelectedValue + "'";

        }


        string Role = "";
        DataTable dtMarket = _dataLoad.GetEmpMarketStructure_Active(EmpInfoId);

        string FFID = "";
        switch (RoleTypeName)
        {



            case "MIO":
                FFID = dtMarket.Rows[0]["MIOEmpId"].ToString();
                param = param + " AND View_Webapi_EmployeeFieldForceInfo.MIOEmpId=" + FFID;
                Role = "AM";

                break;

            case "AM":
                FFID = dtMarket.Rows[0]["ASMEMPId"].ToString();
                param = param + " AND View_Webapi_EmployeeFieldForceInfo.ASMEMPId=" + FFID;
                Role = "AM";

                break;
            case "DZSM":
                FFID = dtMarket.Rows[0]["RSMEMPId"].ToString();
                param = param + " AND  View_Webapi_EmployeeFieldForceInfo.RSMEMPId=" + FFID;
                Role = "DZSM";
                break;
            case "NSM":
                FFID = dtMarket.Rows[0]["NSMEMPId"].ToString();
                param = param + " AND  View_Webapi_EmployeeFieldForceInfo.NSMEMPId=" + FFID;
                Role = "NSM";
                break;


            default:

                Role = "";
                break;
        }


        return param;
    }

  
    protected void resetBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("DCRList.aspx");
    }
    private void LoadInitialInfo()
    {
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

        try
        {
            using (DataTable dt = _dataLoad.GetEmployeeList_Active())
            {
                EmployeeIdSelect.DataSource = dt;
                EmployeeIdSelect.DataValueField = "EmpInfoId";
                EmployeeIdSelect.DataTextField = "EmpName";
                EmployeeIdSelect.DataBind();
                EmployeeIdSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                EmployeeIdSelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }

        try
        {
            using (DataTable dt = _setupDAL.Get_UserRoleInfo())
            {
                UserRoleSelect.DataSource = dt;
                UserRoleSelect.DataValueField = "UserRoleID";
                UserRoleSelect.DataTextField = "RoleName";
                UserRoleSelect.DataBind();
                UserRoleSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                UserRoleSelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


        try
        {
            using (DataTable dt = _seedRepo.GetApprovalStatusList())
            {
                ApprovalStatusSelect.DataSource = dt;
                ApprovalStatusSelect.DataValueField = "SoftwareUseId";
                ApprovalStatusSelect.DataTextField = "WebShow";
                ApprovalStatusSelect.DataBind();
                ApprovalStatusSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ApprovalStatusSelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


    }
    [WebMethod]

    public static string GetDCRList(string param)
    {
        string RoleTypeName = "";
        string EmpInfoId = "";
        string ToRoleTypeId = "";
        string ApprovalStatus = "";
        RoleTypeName =HttpContext.Current.Session["RoleTypeName"].ToString();
        EmpInfoId = HttpContext.Current.Session["EmpInfoId"].ToString();
        ToRoleTypeId = HttpContext.Current.Session["RoleTypeId"].ToString();
        string Role = "";
        DataTable dtMarket = _dataLoad.GetEmpMarketStructure_Active(EmpInfoId);

        string FFID = "";
        switch (RoleTypeName)
        {



            case "MIO":
                FFID = dtMarket.Rows[0]["MIOEmpId"].ToString();
                param = param + " AND View_Webapi_EmployeeFieldForceInfo.MIOEmpId=" + FFID;
                Role = "AM";

                break;

            case "AM":
                FFID = dtMarket.Rows[0]["ASMEMPId"].ToString();
                param = param + " AND View_Webapi_EmployeeFieldForceInfo.ASMEMPId=" + FFID;
                Role = "AM";

                break;
            case "DZSM":
                FFID = dtMarket.Rows[0]["RSMEMPId"].ToString();
                param = param + " AND  View_Webapi_EmployeeFieldForceInfo.RSMEMPId=" + FFID;
                Role = "DZSM";
                break;
            case "NSM":
                FFID = dtMarket.Rows[0]["NSMEMPId"].ToString();
                param = param + " AND  View_Webapi_EmployeeFieldForceInfo.NSMEMPId=" + FFID;
                Role = "NSM";
                break;


            default:

                Role = "";
                break;
        }
        DataTable dt = _DoctorVisit_DAL.GetDCRList(param);
        string JSONresult;
        JSONresult = JsonConvert.SerializeObject(dt);
        return (JSONresult);
    }
}