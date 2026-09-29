using Library.DAL.MasterSetup_DAL;
using Library.DAO.MasterSetup_DAO;
using SalesSolution.Web.DataLayer;
using SalesSolution.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetup_UI_DoctorEntry : System.Web.UI.Page
{
    private static SeedDataDAL _seedRepo = new SeedDataDAL();
    private CommonDataLoad _dataLoad = new CommonDataLoad();

    private static DoctorDAL _DAL = new DoctorDAL();
    private static CommonDataLoad _CommonDataLoad = new CommonDataLoad();
    private HiddenField hfGroupId, hfZone, hfArea, hfTeritory, hfSubTeritory, hfMarket;

    private DropDownList GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect;
    protected void Page_Load(object sender, EventArgs e)
    {
        hfGroupId = (HiddenField)IVMarketStructure.FindControl("hfGroupId") as HiddenField;
        hfZone = (HiddenField)IVMarketStructure.FindControl("hfZone") as HiddenField;
        hfArea = (HiddenField)IVMarketStructure.FindControl("hfArea") as HiddenField;
        hfTeritory = (HiddenField)IVMarketStructure.FindControl("hfTeritory") as HiddenField;
        hfSubTeritory = (HiddenField)IVMarketStructure.FindControl("hfSubTeritory") as HiddenField;
        hfMarket = (HiddenField)IVMarketStructure.FindControl("hfMarket") as HiddenField;
         

        GroupSelect = (DropDownList)IVMarketStructure.FindControl("GroupSelect") as DropDownList;
        ZoneSelect = (DropDownList)IVMarketStructure.FindControl("ZoneSelect") as DropDownList;
        AreaSelect = (DropDownList)IVMarketStructure.FindControl("AreaSelect") as DropDownList;
        TeritorySelect = (DropDownList)IVMarketStructure.FindControl("TeritorySelect") as DropDownList;
        SubTeritory = (DropDownList)IVMarketStructure.FindControl("SubTeritory") as DropDownList;
        MarketSelect = (DropDownList)IVMarketStructure.FindControl("MarketSelect") as DropDownList;

        IVMarketStructure.Visible = false;

        if (!IsPostBack)
        {

            txtActiveDate.Text = DateTime.Now.ToString("dd-MMMM-yyyy");
            LoadInitialInfo();

            if (!string.IsNullOrEmpty(Request.QueryString["MID"]))
            {
                btnUpdate.Visible = true;

                id_mastetID.Value = Request.QueryString["MID"];
                GetOneRecord(id_mastetID.Value);
            }
            else
            {
                btnSave.Visible = true;
            }
        }
    }

    private void GetOneRecord(string Id)
    {
        try
        {
            using (DataTable dt = _DAL.GetDoctorSetupById(Id))
            {


                DoctorName.Text = dt.Rows[0]["DoctorName"].ToString();
                SecondaryCode.Text = dt.Rows[0]["SecondaryCode"].ToString();

                UPCode.Text = dt.Rows[0]["UPCode"].ToString();
                designationSelect.SelectedValue = dt.Rows[0]["DesignationId"].ToString();
                DoctorTypeSelect.SelectedValue = dt.Rows[0]["DoctorTypeId"].ToString();
                txtRemarks.Text = dt.Rows[0]["Reamrks"].ToString();
                txaStreetAddress.Text = dt.Rows[0]["StreetAddress"].ToString();
                GenderSelect.SelectedValue = dt.Rows[0]["Gender"].ToString();

                try
                {
                    chkIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
                }
                catch (Exception ex)
                {
                    chkIsActive.Checked = false;
                }

                ddlStationType.SelectedValue = dt.Rows[0]["StationTypeId"].ToString();
                ddlProgramType.SelectedValue = dt.Rows[0]["ProgramTypeId"].ToString();
                ddlPharmaPlatform.SelectedValue = dt.Rows[0]["SMCTypeId"].ToString();
                docCat.SelectedValue = dt.Rows[0]["DoctorCategoryId"].ToString();

                string taggedSalesPersonIds = dt.Columns.Contains("TaggedSalesPersonIds") && dt.Rows[0]["TaggedSalesPersonIds"] != DBNull.Value
                    ? dt.Rows[0]["TaggedSalesPersonIds"].ToString()
                    : (dt.Columns.Contains("SalesPersonId") && dt.Rows[0]["SalesPersonId"] != DBNull.Value ? dt.Rows[0]["SalesPersonId"].ToString() : "");

                if (!string.IsNullOrEmpty(taggedSalesPersonIds))
                {
                    string[] selectedList = taggedSalesPersonIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (ListItem item in ddlSalesPerson.Items)
                    {
                        if (selectedList.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                }

                
                ddlDivision.SelectedValue = dt.Rows[0]["DivisionId"].ToString();
                ddlDivision_SelectedIndexChanged(null, null);

                ddlDistrict.SelectedValue = dt.Rows[0]["DistrictId"].ToString();
                ddlDistrict_SelectedIndexChanged(null, null);
                ddlThana.SelectedValue = dt.Rows[0]["ThanaId"].ToString();

                txtUnion.Text = dt.Rows[0]["UnionName"].ToString();

                try
                {
                    using (DataTable dtGroup = _dataLoad.GetGroupInfo_Active())
                    {
                        GroupSelect.DataSource = dtGroup;
                        GroupSelect.DataValueField = "GroupId";
                        GroupSelect.DataTextField = "GroupName";
                        GroupSelect.DataBind();
                        GroupSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                        GroupSelect.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {

                }

                GroupSelect.SelectedValue = dt.Rows[0]["GroupId"].ToString();
                hfGroupId.Value = dt.Rows[0]["GroupId"].ToString();
                 

                hfZone.Value = dt.Rows[0]["RegionId"].ToString();

 

                hfArea.Value = dt.Rows[0]["AreaId"].ToString();
 

                hfTeritory.Value = dt.Rows[0]["TerritoryId"].ToString();
                 
                hfSubTeritory.Value = dt.Rows[0]["SubTerritoryId"].ToString();


                 
                hfMarket.Value = dt.Rows[0]["MarketId"].ToString();
                DoctorTypeSelect_SelectedIndexChanged(null, null);

                string[] degree = dt.Rows[0]["DegreeId"].ToString().Split(',');
               
                    foreach (ListItem item in degreeSelect.Items)
                {
                    for (int i = 0; i < degree.Length; i++)
                    {
                        if (item.Value == degree[i].ToString())
                        {
                            item.Selected = true;

                        }
                    }
                }



                string[] Speciality = dt.Rows[0]["DoctorSpecialityId"].ToString().Split(',');

                foreach (ListItem item in doctorSpecialitySelect.Items)
                {
                    for (int i = 0; i < Speciality.Length; i++)
                    {
                        if (item.Value == Speciality[i].ToString())
                        {
                            item.Selected = true;

                        }
                    }
                }

                string[] Brand = dt.Rows[0]["BrandId"].ToString().Split(',');

                foreach (ListItem item in BrandSelect.Items)
                {
                    for (int i = 0; i < Brand.Length; i++)
                    {
                        if (item.Value == Brand[i].ToString())
                        {
                            item.Selected = true;

                        }
                    }
                }


                // Own query: the master SP left-joins chambers, which gives one blank row for a doctor with none.
                using (DataTable dtChamber = _DAL.GetDoctorChambers(Id))
                {
                    gv_Chamber.DataSource = dtChamber;
                    gv_Chamber.DataBind();
                }


                using (DataTable dtSpecialDay = _DAL.GetContactById(Id))
                {
                    gv_Contact.DataSource = dtSpecialDay;
                    gv_Contact.DataBind();
                }
                try
                {

                    using (DataTable dtSpecialDay = _DAL.GetSpecialDayById(Id))
                    {
                        gv_Special.DataSource = dtSpecialDay;
                        gv_Special.DataBind();
                    }
                }
                catch(Exception ex)
                {

                }

             

            }







        }
        catch (Exception ex) { }

        try
        {
            if (Session["RoleTypeName"].ToString() != "Admin")
            {
                // Keep core doctor identity fields editable in edit mode.
                SecondaryCode.ReadOnly = true;
                UPCode.ReadOnly = true;
                txtRemarks.ReadOnly = true;
                txtUnion.ReadOnly = true;

                // Leave Doctor Category, Designation, Gender and Doctor Type editable.
                ddlStationType.Enabled = false;
                ddlProgramType.Enabled = false;
                ddlPharmaPlatform.Enabled = false;
                ddlDivision.Enabled = false;
                ddlDistrict.Enabled = false;
                ddlThana.Enabled = false;
                //GroupSelect.Enabled = false;

                // Set hidden fields (if needed, otherwise they are already non-editable)
            

                //// Set multi-selection listboxes as disabled (if applicable)
                //foreach (ListItem item in degreeSelect.Items)
                //{
                //    item.Enabled = false;
                //}

                //foreach (ListItem item in doctorSpecialitySelect.Items)
                //{
                //    item.Enabled = false;
                //}

                //foreach (ListItem item in BrandSelect.Items)
                //{
                //    item.Enabled = false;
                //}

                //// Disable GridViews (optional: prevents editing)
                //gv_Chamber.Enabled = false;
                //gv_Contact.Enabled = false;
                //gv_Special.Enabled = false;


            }
        }
        catch
        {

        }
            }
    private void LoadInitialInfo()
    {



        try
        {
            using (DataTable dt = _CommonDataLoad.GetDesignation_Active())
            {
                designationSelect.DataSource = dt;
                designationSelect.DataValueField = "DesignationId";
                designationSelect.DataTextField = "DesignationName";
                designationSelect.DataBind();
                designationSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                designationSelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


        // Sales Person

        try
        {
            using (DataTable dt = _CommonDataLoad.GetDoctor_SalesPerson_Active())
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
            using (DataTable dt = _CommonDataLoad.GetContactType())
            {
                ddlContactType.DataSource = dt;
                ddlContactType.DataValueField = "ContactTypeId";
                ddlContactType.DataTextField = "ContactType";
                ddlContactType.DataBind();
                ddlContactType.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlContactType.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


        try
        {
            using (DataTable dt = _CommonDataLoad.GetSpecialDay_Active())
            {
                ddlSpecialDay.DataSource = dt;
                ddlSpecialDay.DataValueField = "SpecialDayId";
                ddlSpecialDay.DataTextField = "SpecialDay";
                ddlSpecialDay.DataBind();
                ddlSpecialDay.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlSpecialDay.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


        try
        {
            using (DataTable dt = _CommonDataLoad.GetDocCat_Active())
            {
                docCat.DataSource = dt;
                docCat.DataValueField = "CategoryId";
                docCat.DataTextField = "CategoryName";
                docCat.DataBind();
                docCat.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                docCat.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


     


        try
        {
            using (DataTable dt = _CommonDataLoad.DoctorSpeciality_Active())
            {
                doctorSpecialitySelect.DataSource = dt;
                doctorSpecialitySelect.DataValueField = "SpecialityId";
                doctorSpecialitySelect.DataTextField = "SpecialityName";
                doctorSpecialitySelect.DataBind();
                doctorSpecialitySelect.Items.Insert(-1, "");
                doctorSpecialitySelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


        try
        {
            using (DataTable dt = _CommonDataLoad.GetDoctorBrand_Active())
            {
                BrandSelect.DataSource = dt;
                BrandSelect.DataValueField = "ProductBrandId";
                BrandSelect.DataTextField = "ProductSQName";
                BrandSelect.DataBind();
                BrandSelect.Items.Insert(-1, "");
                BrandSelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }

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


        try
        {
            using (DataTable dt = _CommonDataLoad.GetChamberType_Active())
            {
                ChamberTypeSelect.DataSource = dt;
                ChamberTypeSelect.DataValueField = "ChamberId";
                ChamberTypeSelect.DataTextField = "ChamberName";
                ChamberTypeSelect.DataBind();
                ChamberTypeSelect.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ChamberTypeSelect.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }


 


        try
        {
            using (DataTable dt = _seedRepo.GetDivisionList())
            {
                ddlDivision.DataSource = dt;
                ddlDivision.DataValueField = "DivisionId";
                ddlDivision.DataTextField = "DivisionName";
                ddlDivision.DataBind();
                ddlDivision.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlDivision.SelectedIndex = 0;
            }


        }
        catch (Exception ex) { }



        try
        {
            using (DataTable dt = _seedRepo.GetStationTypeList())
            {
                ddlStationType.DataSource = dt;
                ddlStationType.DataValueField = "StationTypeId";
                ddlStationType.DataTextField = "StationTypeName";
                ddlStationType.DataBind();
                ddlStationType.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                ddlStationType.SelectedIndex = 0;
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

        try
        {
            using (DataTable dt = _CommonDataLoad.GetDoctor_SalesPerson_Active())
            {
                ddlSalesPerson.DataSource = dt;
                ddlSalesPerson.DataValueField = "EmpInfoId";
                ddlSalesPerson.DataTextField = "EmpName";
                ddlSalesPerson.DataBind();
                // The earlier bind leaves index 0 selected, which here is the first employee and
                // got saved as the doctor's sales person on every edit.
                ddlSalesPerson.ClearSelection();
            }
        }
        catch (Exception ex) { }

        Chamber_gv_Initial();
        gv_Contact_Inital();
        gv_Special_Inital();

    }

    public void gv_Contact_Inital()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("ContactTypeId");
        aDataTable.Columns.Add("ContactType");
        aDataTable.Columns.Add("Contact");
        gv_Contact.DataSource = aDataTable;
        gv_Contact.DataBind();


        
    }

    public void gv_Special_Inital()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SpecialDayId");
        aDataTable.Columns.Add("SpecialDay");
        aDataTable.Columns.Add("SpecialDate");
        gv_Special.DataSource = aDataTable;
        gv_Special.DataBind();

    }
 
    public void Chamber_gv_Initial()
    {
        BindChambers(NewChamberTable());
    }

    private DataTable NewChamberTable()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("ChemberId");
        aDataTable.Columns.Add("ChamberTypeId");
        aDataTable.Columns.Add("ChamberTypeName");
        aDataTable.Columns.Add("ChamberName");
        aDataTable.Columns.Add("ChamberAddress");
        aDataTable.Columns.Add("Phone");
        aDataTable.Columns.Add("Shift");
        aDataTable.Columns.Add("SittingTime");
        aDataTable.Columns.Add("ChamberContactName");
        return aDataTable;
    }

    // The chamber list lives only in the grid between postbacks, so it is read back from there.
    private DataTable ChamberTableFromGrid()
    {
        DataTable aDataTable = NewChamberTable();
        foreach (GridViewRow row in gv_Chamber.Rows)
        {
            DataRow dataRow = aDataTable.NewRow();
            dataRow["ChemberId"] = ((HiddenField)row.FindControl("hfChemberId")).Value;
            dataRow["ChamberTypeId"] = ((HiddenField)row.FindControl("hfChamberTypeId")).Value;
            dataRow["ChamberTypeName"] = ((Label)row.FindControl("lbl_ChamberType")).Text;
            dataRow["ChamberName"] = ((Label)row.FindControl("lbl_ChamberName")).Text;
            dataRow["ChamberAddress"] = ((Label)row.FindControl("lbl_ChamberAddress")).Text;
            dataRow["Phone"] = ((Label)row.FindControl("lbl_Phone")).Text;
            dataRow["Shift"] = ((Label)row.FindControl("lbl_Shift")).Text;
            dataRow["SittingTime"] = ((Label)row.FindControl("lbl_SittingTime")).Text;
            dataRow["ChamberContactName"] = ((Label)row.FindControl("lbl_ChamberContactName")).Text;
            aDataTable.Rows.Add(dataRow);
        }
        return aDataTable;
    }

    private void BindChambers(DataTable aDataTable)
    {
        gv_Chamber.DataSource = aDataTable;
        gv_Chamber.DataBind();
    }

    private void ClearChamberInputs()
    {
        hfChamberEditIndex.Value = string.Empty;
        addButtonChamber.Text = "<i class=\"fa fa-plus-circle\"></i>Add To List";
        ChamberTypeSelect.SelectedValue = string.Empty;
        ddlChamberShift.SelectedValue = string.Empty;
        txtChamberName.Text = string.Empty;
        txtChamberAddress.Text = string.Empty;
        txtPhone.Text = string.Empty;
        txtSittingTime.Text = string.Empty;
        txtChamberContactName.Text = string.Empty;
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    public bool Validation()
    {

        DoctorName.CssClass = "form-control form-control-sm";
        MarketSelect.CssClass = "form-select form-select-sm mb-3 mySelect2";


        if (DoctorName.Text == "")
        {
            DoctorName.ToolTip = "please fill out this field";
            DoctorName.CssClass = "form-control form-control-sm is-invalid";
            DoctorName.Focus();
            return false;
        }
        //if (MarketSelect.SelectedValue == "")
        //{
        //    MarketSelect.ToolTip = "please fill out this field";
        //    MarketSelect.CssClass = "form-select form-select-sm mb-3 mySelect2  invalid";
        //    MarketSelect.Focus();
        //    return false;
        //}


       

        return true;
    }


    public bool ChemberValidation()
    {

        txtChamberName.CssClass = "form-control form-control-sm";
        txtPhone.CssClass = "form-control form-control-sm";
        ChamberTypeSelect.CssClass = "form-select form-select-sm mb-3 mySelect2";


        if (txtChamberName.Text == "")
        {
            txtChamberName.ToolTip = "please fill out this field";
            txtChamberName.CssClass = "form-control form-control-sm is-invalid";
            txtChamberName.Focus();
            return false;
        }
        if (ChamberTypeSelect.SelectedValue == "")
        {
            ChamberTypeSelect.ToolTip = "please fill out this field";
            ChamberTypeSelect.CssClass = "form-select form-select-sm mb-3 mySelect2  invalid";
            ChamberTypeSelect.Focus();
            return false;
        }
        if (txtPhone.Text != "")
        {
            if (txtPhone.Text.Length != 11)
            {

                string text6 = "Mobile NO must be 11 digits!";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showpop6('" + text6 + "')</script>", false);
                txtPhone.Focus();
                txtPhone.CssClass = "form-control form-control-sm  is-invalid";

                return false;
            }
        }

        return true;
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (Validation())
        {


            List<DoctorChemberDetailDAO> ChamberList = new List<DoctorChemberDetailDAO>();


            foreach (DataRow chamber in ChamberTableFromGrid().Rows)
            {
                string chemberId = chamber["ChemberId"].ToString();
                string chamberTypeId = chamber["ChamberTypeId"].ToString();

                DoctorChemberDetailDAO _DAO = new DoctorChemberDetailDAO();

                _DAO.ChemberId = string.IsNullOrEmpty(chemberId) ? 0 : int.Parse(chemberId);
                _DAO.ChamberTypeId = string.IsNullOrEmpty(chamberTypeId) ? (int?)null : int.Parse(chamberTypeId);

                _DAO.Name = NullIfEmpty(chamber["ChamberName"]);
                _DAO.Address = NullIfEmpty(chamber["ChamberAddress"]);
                _DAO.Phone = NullIfEmpty(chamber["Phone"]);
                _DAO.Shift = NullIfEmpty(chamber["Shift"]);
                _DAO.SittingTime = NullIfEmpty(chamber["SittingTime"]);
                _DAO.ChamberContactName = NullIfEmpty(chamber["ChamberContactName"]);

                ChamberList.Add(_DAO);

            }



            List<DoctorSpecialDayDAO> SpecialList = new List<DoctorSpecialDayDAO>();


            for (int i = 0; i < gv_Special.Rows.Count; i++)
            {

                HiddenField hfSpecialDayId = ((HiddenField)gv_Special.Rows[i].Cells[1].FindControl("hfSpecialDayId"));


                Label lbl_SpecialDate = ((Label)gv_Special.Rows[i].Cells[1].FindControl("lbl_SpecialDate"));
               


                DoctorSpecialDayDAO _DAO = new DoctorSpecialDayDAO();

                _DAO.SpecialDayId = string.IsNullOrEmpty(hfSpecialDayId.Value) ? (int?)null : int.Parse(hfSpecialDayId.Value);

                try
                {
                    _DAO.SpecialDate =
                string.IsNullOrEmpty(lbl_SpecialDate.Text) ? (DateTime?)null : DateTime.Parse(lbl_SpecialDate.Text).Date;
                }
                catch
                {
                    _DAO.SpecialDate = null;
                }

                SpecialList.Add(_DAO);

            }

            List<DoctorContactDetailDAO> ContactList = new List<DoctorContactDetailDAO>();


            for (int i = 0; i < gv_Contact.Rows.Count; i++)
            {

                HiddenField hfContactTypeId = ((HiddenField)gv_Contact.Rows[i].Cells[1].FindControl("hfContactTypeId"));


                Label lbl_Contact = ((Label)gv_Contact.Rows[i].Cells[1].FindControl("lbl_Contact"));
                Label lbl_ContactType = ((Label)gv_Contact.Rows[i].Cells[1].FindControl("lbl_ContactType"));



                DoctorContactDetailDAO _DAO = new DoctorContactDetailDAO();

                _DAO.ContactTypeId = string.IsNullOrEmpty(hfContactTypeId.Value) ? (int?)null : int.Parse(hfContactTypeId.Value);

                _DAO.Contact =
                string.IsNullOrEmpty(lbl_Contact.Text) ? null : lbl_Contact.Text;

                _DAO.ContactType =
                string.IsNullOrEmpty(lbl_ContactType.Text) ? null : lbl_ContactType.Text;
                ContactList.Add(_DAO);

            }


            DoctorMasterDAO aMaster = new DoctorMasterDAO();

            aMaster.DoctorId = id_mastetID.Value == "" ? 0 : Convert.ToInt32(id_mastetID.Value);

            aMaster.DoctorName = string.IsNullOrEmpty(DoctorName.Text) ? null : DoctorName.Text;
            aMaster.SecondaryCode = string.IsNullOrEmpty(SecondaryCode.Text) ? null : SecondaryCode.Text;

            aMaster.UPCode = string.IsNullOrEmpty(UPCode.Text) ? null : UPCode.Text;



            aMaster.DesignationId = designationSelect.SelectedIndex > 0 ? int.Parse(designationSelect.SelectedValue) : (int?)null;
            aMaster.Gender= GenderSelect.SelectedIndex > 0 ? GenderSelect.SelectedItem.Text : null;

            aMaster.DoctorTypeId = DoctorTypeSelect.SelectedIndex > 0 ? int.Parse(DoctorTypeSelect.SelectedValue) : (int?)null;
            
            List<string> selectedSpIds = new List<string>();
            List<string> selectedSpCodes = new List<string>();
            foreach (ListItem item in ddlSalesPerson.Items)
            {
                if (item.Selected && !string.IsNullOrEmpty(item.Value))
                {
                    selectedSpIds.Add(item.Value);
                    string[] parts = item.Text.Split(':');
                    if (parts.Length > 0) selectedSpCodes.Add(parts[0].Trim());
                }
            }
            aMaster.TaggedSalesPersonIds = string.Join(",", selectedSpIds);
            aMaster.TaggedSalesPersonCodes = string.Join(",", selectedSpCodes);
            aMaster.SalesPersonId = selectedSpIds.Count > 0 ? (int?)Convert.ToInt32(selectedSpIds[0]) : (int?)null;


            aMaster.Reamrks = string.IsNullOrEmpty(txtRemarks.Text) ? null : txtRemarks.Text;
            aMaster.StreetAddress = string.IsNullOrEmpty(txaStreetAddress.Text) ? null : txaStreetAddress.Text;
            aMaster.IsActive = chkIsActive.Checked;

            
 
            aMaster.MarketId = MarketSelect.SelectedIndex > 0 ? int.Parse(MarketSelect.SelectedValue) : (int?)null;



            aMaster.StationTypeId = ddlStationType.SelectedIndex > 0 ? int.Parse(ddlStationType.SelectedValue) : (int?)null;
            aMaster.ProgramTypeId = ddlProgramType.SelectedIndex > 0 ? int.Parse(ddlProgramType.SelectedValue) : (int?)null;


            aMaster.SMCTypeId = ddlPharmaPlatform.SelectedIndex > 0 ? int.Parse(ddlPharmaPlatform.SelectedValue) : (int?)null;




            aMaster.DivisionId = ddlDivision.SelectedIndex > 0 ? int.Parse(ddlDivision.SelectedValue) : (int?)null;
            aMaster.DoctorCategoryId = docCat.SelectedIndex > 0 ? int.Parse(docCat.SelectedValue) : (int?)null;

            

            aMaster.DistrictId = ddlDistrict.SelectedIndex > 0 ? int.Parse(ddlDistrict.SelectedValue) : (int?)null;

            


            aMaster.ThanaId = ddlThana.SelectedIndex > 0 ? int.Parse(ddlThana.SelectedValue) : (int?)null;



            aMaster.UnionName = string.IsNullOrEmpty(txtUnion.Text) ? null : txtUnion.Text;

            if (chkIsActive.Checked)
            {
                aMaster.Activedate = string.IsNullOrEmpty(txtActiveDate.Text) ? (DateTime?)null : DateTime.Parse(txtActiveDate.Text).Date;

            }
            else
            {
                aMaster.InactiveDate = string.IsNullOrEmpty(txtActiveDate.Text) ? (DateTime?)null : DateTime.Parse(txtActiveDate.Text).Date;

            }

            string DegreeArray = "";

            foreach (ListItem item in degreeSelect.Items)
            {
                if (item.Selected)
                {
                    
                    DegreeArray = DegreeArray + item.Value + ",";
                }
            }
           
            DegreeArray = DegreeArray.TrimEnd(',');


            string SpecialityArray = "";
            foreach (ListItem item in doctorSpecialitySelect.Items)
            {
                if (item.Selected)
                {
                    SpecialityArray = SpecialityArray + item.Value + ",";
                }

            }
            SpecialityArray = SpecialityArray.TrimEnd(',');


            string BrandSelectArray = "";
            foreach (ListItem item in BrandSelect.Items)
            {
                if (item.Selected)
                {
                    BrandSelectArray = BrandSelectArray + item.Value + ",";
                }

            }
            BrandSelectArray = BrandSelectArray.TrimEnd(',');


            ResultInfo Res = _DAL.SaveInfo(aMaster, DegreeArray, SpecialityArray, BrandSelectArray, ChamberList, SpecialList, ContactList, Session["UserId"].ToString());
            if (Res.isSuccess == true)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + "Operation successful!" + "','Success','DoctorView.aspx');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + "Already Exist!" + "','Faild');", true);

            }

        }

    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDivision.SelectedIndex > 0)
        {
            try
            {
                using (DataTable dt = _seedRepo.GetDistrictList(ddlDivision.SelectedValue))
                {
                    ddlDistrict.DataSource = dt;
                    ddlDistrict.DataValueField = "DistrictId";
                    ddlDistrict.DataTextField = "DistrictName";
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    ddlDistrict.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }
        }
        else
        {
            ddlDistrict.Items.Clear();
            ddlThana.Items.Clear();
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDistrict.SelectedIndex > 0)
        {
            try
            {
                using (DataTable dt = _seedRepo.GetThanaList(ddlDistrict.SelectedValue))
                {
                    ddlThana.DataSource = dt;
                    ddlThana.DataValueField = "ThanaId";
                    ddlThana.DataTextField = "ThanaName";
                    ddlThana.DataBind();
                    ddlThana.Items.Insert(0, new ListItem("Please Select From List", String.Empty));
                    ddlThana.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }
        }
        else
        {
            
            ddlThana.Items.Clear();
        }
    }
    // Adds the entered chamber, or writes it back over the row being edited.
    public void AddChamber()
    {
        DataTable aDataTable = ChamberTableFromGrid();

        int editIndex;
        bool isEdit = int.TryParse(hfChamberEditIndex.Value, out editIndex) && editIndex >= 0 && editIndex < aDataTable.Rows.Count;
        DataRow dataRow = isEdit ? aDataTable.Rows[editIndex] : aDataTable.NewRow();

        dataRow["ChamberTypeName"] = ChamberTypeSelect.SelectedItem.Text;
        dataRow["ChamberTypeId"] = ChamberTypeSelect.SelectedValue;
        dataRow["ChamberName"] = txtChamberName.Text.Trim();
        dataRow["ChamberAddress"] = txtChamberAddress.Text.Trim();
        dataRow["Phone"] = txtPhone.Text.Trim();
        dataRow["Shift"] = ddlChamberShift.SelectedValue;
        dataRow["SittingTime"] = txtSittingTime.Text.Trim();
        dataRow["ChamberContactName"] = txtChamberContactName.Text.Trim();

        if (!isEdit)
            aDataTable.Rows.Add(dataRow);

        BindChambers(aDataTable);
        ClearChamberInputs();
    }

    public void EditChamber(int row)
    {
        DataRow chamber = ChamberTableFromGrid().Rows[row];

        ClearChamberInputs();
        SelectIfListed(ChamberTypeSelect, chamber["ChamberTypeId"].ToString());
        SelectIfListed(ddlChamberShift, chamber["Shift"].ToString());
        txtChamberName.Text = chamber["ChamberName"].ToString();
        txtChamberAddress.Text = chamber["ChamberAddress"].ToString();
        txtPhone.Text = chamber["Phone"].ToString();
        txtSittingTime.Text = chamber["SittingTime"].ToString();
        txtChamberContactName.Text = chamber["ChamberContactName"].ToString();

        hfChamberEditIndex.Value = row.ToString();
        addButtonChamber.Text = "<i class=\"fa fa-plus-circle\"></i>Update";
        txtChamberName.Focus();
    }

    public void RemoveChamber(int row)
    {
        DataTable aDataTable = ChamberTableFromGrid();
        aDataTable.Rows.RemoveAt(row);
        BindChambers(aDataTable);
        // Row numbers shift after a removal, so drop any edit in progress.
        ClearChamberInputs();
    }

    // Old data can hold a value that is no longer in the dropdown (inactive chamber type, free-text shift).
    private static void SelectIfListed(DropDownList list, string value)
    {
        if (list.Items.FindByValue(value) != null)
            list.SelectedValue = value;
    }

    private static string NullIfEmpty(object value)
    {
        string text = Convert.ToString(value);
        return string.IsNullOrEmpty(text) ? null : text;
    }

    protected void editChamber_Click(object sender, EventArgs e)
    {
        EditChamber(((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex);
    }

    protected void addButtonChamber_Click(object sender, EventArgs e)
    {
        if (ChemberValidation()) {
            AddChamber();
        }
    }

    protected void deleteChamber_Click(object sender, EventArgs e)
    {
        LinkButton ImageButton = (LinkButton)sender;
        GridViewRow currentRow = (GridViewRow)ImageButton.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        RemoveChamber(rowindex);

    }

    protected void DoctorTypeSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        degreeSelect.Items.Clear();
        if (DoctorTypeSelect.SelectedIndex > 0)
        {

            try
            {
                // Degrees are not tagged to a doctor type in tblDoctorDegree, so load all active ones.
                using (DataTable dt = _CommonDataLoad.GetDegree_Active())
                {
                    degreeSelect.DataSource = dt;
                    degreeSelect.DataValueField = "DegreeId";
                    degreeSelect.DataTextField = "DegreeName";
                    degreeSelect.DataBind();
                    degreeSelect.Items.Insert(-1, "");
                    degreeSelect.SelectedIndex = 0;
                }


            }
            catch (Exception ex) { }
        }
        else
        {

        }

    }

    protected void btnSpecialAdd_Click(object sender, EventArgs e)
    {
        AddSpecialDay();
    }

    protected void deleteSpecial_Click(object sender, EventArgs e)
    {
        LinkButton ImageButton = (LinkButton)sender;
        GridViewRow currentRow = (GridViewRow)ImageButton.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        RemoveSpecialDay(rowindex);
    }


    public void AddSpecialDay()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SpecialDayId");
        aDataTable.Columns.Add("SpecialDay");
        aDataTable.Columns.Add("SpecialDate");


        DataRow dataRow = null;
        for (int i = 0; i < gv_Special.Rows.Count; i++)
        {
            dataRow = aDataTable.NewRow();
            HiddenField hfSpecialDayId = ((HiddenField)gv_Special.Rows[i].Cells[1].FindControl("hfSpecialDayId"));
            Label lbl_SpecialDay = ((Label)gv_Special.Rows[i].Cells[1].FindControl("lbl_SpecialDay"));
            Label lbl_SpecialDate = ((Label)gv_Special.Rows[i].Cells[1].FindControl("lbl_SpecialDate"));

            dataRow["SpecialDayId"] = hfSpecialDayId.Value.Trim();
            dataRow["SpecialDay"] = lbl_SpecialDay.Text.Trim();
            dataRow["SpecialDate"] = lbl_SpecialDate.Text.Trim();


            aDataTable.Rows.Add(dataRow);
        }
        dataRow = aDataTable.NewRow();
        dataRow["SpecialDayId"] = ddlSpecialDay.SelectedValue.Trim();
        dataRow["SpecialDay"] = ddlSpecialDay.SelectedItem.Text.Trim();
        dataRow["SpecialDate"] = txtSpecialDate.Text.Trim();


        aDataTable.Rows.Add(dataRow);
        gv_Special.DataSource = aDataTable;
        gv_Special.DataBind();
        ddlSpecialDay.SelectedValue = "";
        txtSpecialDate.Text = string.Empty;

    }
    public void RemoveSpecialDay(int row)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SpecialDayId");
        aDataTable.Columns.Add("SpecialDay");
        aDataTable.Columns.Add("SpecialDate");

        DataRow dataRow = null;
        for (int i = 0; i < gv_Special.Rows.Count; i++)
        {
            if (i != row)
            {
                dataRow = aDataTable.NewRow();
                HiddenField hfSpecialDayId = ((HiddenField)gv_Special.Rows[i].Cells[1].FindControl("hfSpecialDayId"));
                Label lbl_SpecialDay = ((Label)gv_Special.Rows[i].Cells[1].FindControl("lbl_SpecialDay"));
                Label lbl_SpecialDate = ((Label)gv_Special.Rows[i].Cells[1].FindControl("lbl_SpecialDate"));

                dataRow["SpecialDayId"] = hfSpecialDayId.Value.Trim();
                dataRow["SpecialDay"] = lbl_SpecialDay.Text.Trim();
                dataRow["SpecialDate"] = lbl_SpecialDate.Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }
        }
        gv_Special.DataSource = aDataTable;
        gv_Special.DataBind();

    }

    protected void lblContactAdd_Click(object sender, EventArgs e)
    {
        AddContact();
    }

    protected void deleteContact_Click(object sender, EventArgs e)
    {
        LinkButton ImageButton = (LinkButton)sender;
        GridViewRow currentRow = (GridViewRow)ImageButton.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        RemoveContact(rowindex);
    }

    public void AddContact()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("ContactTypeId");
        aDataTable.Columns.Add("ContactType");
        aDataTable.Columns.Add("Contact");


        DataRow dataRow = null;
        for (int i = 0; i < gv_Contact.Rows.Count; i++)
        {
            dataRow = aDataTable.NewRow();
            HiddenField hfContactTypeId = ((HiddenField)gv_Contact.Rows[i].Cells[1].FindControl("hfContactTypeId"));
            Label lbl_ContactType = ((Label)gv_Contact.Rows[i].Cells[1].FindControl("lbl_ContactType"));
            Label lbl_Contact = ((Label)gv_Contact.Rows[i].Cells[1].FindControl("lbl_Contact"));
 

            dataRow["ContactTypeId"] = hfContactTypeId.Value.Trim();
            dataRow["ContactType"] = lbl_ContactType.Text.Trim();
            dataRow["Contact"] = lbl_Contact.Text.Trim();



            aDataTable.Rows.Add(dataRow);
        }
        dataRow = aDataTable.NewRow();
        dataRow["ContactTypeId"] = ddlContactType.SelectedValue.Trim();
        dataRow["ContactType"] = ddlContactType.SelectedItem.Text.Trim();
        dataRow["Contact"] = txtContact.Text.Trim();


        aDataTable.Rows.Add(dataRow);
        gv_Contact.DataSource = aDataTable;
        gv_Contact.DataBind();
        ddlContactType.SelectedValue = "";
        txtContact.Text = string.Empty;

    }
    public void RemoveContact(int row)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("ContactTypeId");
        aDataTable.Columns.Add("ContactType");
        aDataTable.Columns.Add("Contact");

        DataRow dataRow = null;
        for (int i = 0; i < gv_Contact.Rows.Count; i++)
        {
            if (i != row)
            {
                dataRow = aDataTable.NewRow();
                HiddenField hfContactTypeId = ((HiddenField)gv_Contact.Rows[i].Cells[1].FindControl("hfContactTypeId"));
                Label lbl_ContactType = ((Label)gv_Contact.Rows[i].Cells[1].FindControl("lbl_ContactType"));
                Label lbl_Contact = ((Label)gv_Contact.Rows[i].Cells[1].FindControl("lbl_Contact"));

                dataRow["ContactTypeId"] = hfContactTypeId.Value.Trim();
                dataRow["ContactType"] = lbl_ContactType.Text.Trim();
                dataRow["Contact"] = lbl_Contact.Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }
        }
        gv_Contact.DataSource = aDataTable;
        gv_Contact.DataBind();

    }
}
