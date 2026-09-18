using Library.DAL.DoctorVisit_DAL;
using SalesSolution.Web.DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DoctorVisit_UI_SalesmanCustomerVisitReport : System.Web.UI.Page
{
    public static DoctorVisitDAL _DoctorVisit_DAL = new DoctorVisitDAL();
    static CommonDataLoad _dataLoad = new CommonDataLoad();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd MMMM, yyyy");
            ToDate.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            LoadInitialInfo();
        }
    }

    private void LoadInitialInfo()
    {
        try
        {
            using (DataTable dt = _dataLoad.GetEmployeeList_Active())
            {
                EmployeeIdSelect.DataSource = dt;
                EmployeeIdSelect.DataValueField = "EmpInfoId";
                EmployeeIdSelect.DataTextField = "EmpName";
                EmployeeIdSelect.DataBind();
                EmployeeIdSelect.Items.Insert(0, new ListItem("--- All Employees ---", "0"));
                EmployeeIdSelect.SelectedIndex = 0;
            }
        }
        catch (Exception ex) 
        { 
        }
    }

    private static DateTime ParseDateRobust(string dateStr)
    {
        DateTime result;
        string[] formats = { 
            "dd MMMM, yyyy", 
            "d MMMM, yyyy", 
            "dd-MMMM-yyyy", 
            "d-MMMM-yyyy",
            "dd-MMM-yyyy",
            "d-MMM-yyyy",
            "yyyy-MM-dd",
            "dd/MM/yyyy",
            "MM/dd/yyyy",
            "d/M/yyyy"
        };
        
        if (DateTime.TryParseExact(dateStr.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result))
        {
            return result;
        }
        
        return DateTime.Parse(dateStr, System.Globalization.CultureInfo.InvariantCulture);
    }

    [WebMethod]
    public static string GetSalesmanCustomerVisitReportData(string startDate, string endDate, int empInfoId, string filterStatus)
    {
        try
        {
            DateTime start = ParseDateRobust(startDate);
            DateTime end = ParseDateRobust(endDate);

            DataTable dt = _DoctorVisit_DAL.GetSalesmanCustomerVisitReport(start, end, empInfoId, filterStatus);
            string JSONresult = JsonConvert.SerializeObject(dt);
            return JSONresult;
        }
        catch (Exception ex)
        {
            return "[]";
        }
    }
}
