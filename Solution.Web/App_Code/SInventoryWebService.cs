using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for SInventoryWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SInventoryWebService : System.Web.Services.WebService {

    public SInventoryWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    private string strConnectionString = ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ToString();
    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }


    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomernew(string prefixText)
    {
        string param = "";

        if (Session["MOParam"] != null)
        {
            param = Session["MOParam"].ToString();
        }

        string sql = @"SELECT TOP 10
    (CST.CustomerCode + ' - ' + CST.CustomerName + ':' + 
     CONVERT(NVARCHAR(50), CST.CustomerMasterId)) AS Cust
FROM tblCustMaster CST
WHERE CST.CustomerCode IS NOT NULL
  AND (
        CST.CustomerCode LIKE @prefixText
        OR CST.CustomerName LIKE @prefixText
      )";

        if (!string.IsNullOrEmpty(param))
        {
            sql += " " + param;
        }

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);

        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50)
            .Value = "%" + prefixText + "%";

        DataTable DTLocal = new DataTable();

        try
        {
            da.Fill(DTLocal);
        }
        catch (Exception ex)
        {
            throw new Exception("Autocomplete Error: " + ex.Message);
        }

        string[] items = new string[DTLocal.Rows.Count];

        for (int i = 0; i < DTLocal.Rows.Count; i++)
        {
            items[i] = DTLocal.Rows[i]["Cust"].ToString();
        }

        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProductList(string prefixText)
    {
        string sql = "SELECT ProductCode+' : '+ProductName AS Product FROM tblProduct  WITH (NOLOCK) WHERE GroupId=1 AND IsActive=1 and ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer(string prefixText)
    {
        string sql = "";
        string userType = Session["UserType"] != null ? Session["UserType"].ToString() : "";
        string comUnit = Session["ComUnitId"] != null ? Session["ComUnitId"].ToString() : "";
        
        if (userType == "Admin" || string.IsNullOrEmpty(comUnit))
        {
            sql = @"SELECT TOP 20 CustomerCode+':'+CustomerName AS Customer 
                    FROM tblCustMaster WITH (NOLOCK) 
                    WHERE IsActive=1 AND (CustomerCode LIKE @prefixText OR CustomerName LIKE @prefixText) 
                    ORDER BY CustomerName ASC";
        }
        else
        {
            sql = @"SELECT TOP 20 CustomerCode+':'+CustomerName AS Customer 
                    FROM tblCustMaster WITH (NOLOCK) 
                    WHERE ComUnitId=@ComUnitId AND IsActive=1 AND (CustomerCode LIKE @prefixText OR CustomerName LIKE @prefixText) 
                    ORDER BY CustomerName ASC";
        }
        
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        if (sql.Contains("@ComUnitId"))
        {
            da.SelectCommand.Parameters.Add("@ComUnitId", System.Data.SqlDbType.VarChar, 50).Value = comUnit.Trim();
        }
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer_New(string prefixText)
    {
        string sql = @"select top 10 ISNULL(CustomerCode,'')+' : '+ISNULL(CustomerName,'')+ ' : '+ISNULL(CellNo,'') +' |' + CAST(CustomerMasterId as nvarchar(max)) as Customer from tblCustMaster with(nolock) 
where   IsActive=1 AND CustomerCode IS NOT NULL and     ( CustomerName like @prefixText  or  CustomerCode like @prefixText  or  ISNULL(CellNo,'') like @prefixText)
order by CustomerName asc";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer_ALL(string prefixText)
    {
        string sql = @"select top 10 ISNULL(CustomerCode,'')+' : '+ case when IsActive=1 then  ISNULL(CustomerName,'') else ISNULL(CustomerName,'') +' (Inactive) ' end +' : '+ISNULL(CellNo,'') +' |' + CAST(CustomerMasterId  as nvarchar(max)) as Customer from tblCustMaster with(nolock) 
where     CustomerCode IS NOT NULL and     ( CustomerName like @prefixText  or  CustomerCode like @prefixText  or  ISNULL(CellNo,'') like @prefixText)
order by CustomerName asc";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetSubDepotInvoiceNo(string prefixText)
    {
        string ComUnitId = "";

        ComUnitId = Session["ComUnitId"].ToString();

        string sql = "Select (Cast(InvoiceId as nvarchar(50))+':'+InvoiceNo) As InvoiceNo from tblSubInvoiceMaster where  InvoiceId Is NOT NULL  And cast( InvoiceDate as date) between '2020/07/01' and CURRENT_TIMESTAMP And ComUnitId='" + ComUnitId + "' AND InvoiceNo like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["InvoiceNo"].ToString(), i);
            i++;
        }
        return items;
    }

    //GetProformaInvoice NO
    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProformaInvoiceNo(string prefixText)
    {
        string ComUnitId = "";

        ComUnitId = Session["ComUnitId"].ToString();

        string sql = "Select (Cast(InvoiceId as nvarchar(50))+':'+InvoiceNo) As InvoiceNo from tblInvoice where  InvoiceId Is NOT NULL  And cast( InvoiceDate as date) between '2020/07/01' and CURRENT_TIMESTAMP And ComUnitId='" + ComUnitId + "' AND InvoiceNo like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["InvoiceNo"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProformaInvoiceNoNew(string prefixText)
    {
        string ComUnitId = "";

        ComUnitId = Session["ComUnitId"].ToString();

        string sql = @"  
Select top 1  (Cast(0 as nvarchar(50))+':'+'N/A' ) As InvoiceNo from tblInvoice with(nolock) 

 union all  Select top 20 (Cast(InvoiceId as nvarchar(50))+':'+InvoiceNo) As InvoiceNo from tblInvoice with (nolock)  where  InvoiceId Is NOT NULL  And cast( InvoiceDate as date) between '2020/07/01' and CURRENT_TIMESTAMP And ComUnitId='" + ComUnitId + "' AND InvoiceNo like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["InvoiceNo"].ToString(), i);
            i++;
        }
        return items;
    }


    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetPreBatch(string prefixText)
    {
        string productid = Session["ProductId"].ToString();
        Session["ProductId"] = null;
        string sql = "SELECT DISTINCT BatchNo FROM dbo.tblCentralStore WHERE ProductId='"+productid+"' AND BatchNo like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["BatchNo"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetAllInvoice(string prefixText)
    {
        string CompIDD = Session["CompIDD"].ToString();
        
        string sql = "SELECT top 15 InvoiceNo FROM tblInvoice with (nolock) WHERE ComUnitId='"+ CompIDD + "' and  InvoiceNo LIKE @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["InvoiceNo"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProductWithCode(string prefixText)
    {
        string sql = "SELECT ProductCode+':'+ProductName AS Product FROM tblProduct WHERE ProductCode+':'+ProductName LIKE @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%"+prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer_ALL_Active(string prefixText)
    {
        string sql = "";

        sql = @"select top 25 * from (select   ISNULL(CustomerCode+' : ','') +ISNULL( + case when IsActive=1 then  CustomerName else CustomerName +' (Inactive) ' end ,'') +ISNULL(' : '+CellNo,'') +' |' + CAST(CustomerMasterId  as nvarchar(max)) as Customer from tblCustMaster with(nolock)   where  IsActive=1
           
 ) tbl  where Customer like @prefixText";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }


    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer_ALL_new(string prefixText)
    {
        string sql = "";

        sql = @"select top 25 * from (select   ISNULL(CustomerCode+' : ','') +ISNULL( + case when IsActive=1 then  CustomerName else CustomerName +' (Inactive) ' end ,'') +ISNULL(' : '+CellNo,'') +' |' + CAST(CustomerMasterId  as nvarchar(max)) as Customer from tblCustMaster with(nolock) 
 ) tbl  where Customer like @prefixText";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }
    

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomerPaymentSearch(string prefixText)
    {
        string sql = @"SELECT TOP 10 
                          ISNULL(CustomerCode, '') + ' : ' + ISNULL(CustomerName, '') + ' | ' + CAST(CustomerMasterId AS NVARCHAR(MAX)) AS Customer 
                       FROM tblCustMaster WITH(NOLOCK) 
                       WHERE IsActive = 1
                         AND (CustomerName LIKE @prefixText OR CustomerCode LIKE @prefixText)";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer_ALL_ForDIC(string prefixText)
    {
        string sql = "";

        sql = @"select top 25 * from (select   ISNULL(CSTMR.CustomerCode+' : ','')  +ISNULL(   CustomerName+' : '  ,'') +ISNULL(   cunit.ShortName  ,'')  +' |' + CAST(CSTMR.CustomerMasterId  as nvarchar(max)) as Customer from tblCustMaster AS CSTMR   with (nolock)
            INNER JOIN dbo.tblMarket AS MKT   with (nolock) ON CSTMR.MarketId = MKT.MarketId
        INNER JOIN dbo.tblSubTerritory tr   with (nolock) ON tr.SubTerritoryId = MKT.SubTerritoryId
        INNER JOIN dbo.tblTerritory terry   with (nolock) ON terry.TerritoryId = tr.TerritoryId
        INNER JOIN dbo.tblArea ar   with (nolock) ON ar.AreaId = terry.AreaId
        INNER JOIN dbo.tblRegion rg    with (nolock)ON rg.RegionId = ar.RegionId
        INNER JOIN dbo.tbl_Group gp   with (nolock) ON gp.GroupId = rg.GroupId

        left JOIN dbo.tblRouteInformationMarketDetail rDtl   with (nolock) ON rDtl.MarketId = CSTMR.MarketId
        left JOIN dbo.tblRouteInformationMaster RMas   with (nolock) ON RMas.RouteInformationMasterId = rDtl.RouteInformationMasterId
		inner join tblCompanyUnit cunit  with (nolock) on RMas.DCId=cunit.ComUnitId
 ) tbl where Customer like @prefixText";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetDoctor_ALL(string prefixText)
    {
        string sql = "";

        sql = @"select top 10 DoctorCode+' : '+ case when IsActive=1 then  DoctorName else DoctorName +' (Inactive) ' end +' |' + CAST(DoctorId  as nvarchar(max)) as DoctorName from tblDoctorMaster with(nolock) 
where DoctorCode IS NOT NULL AND ISNULL(isCustomer, 0) = 0 AND (DoctorCode LIKE 'D%' OR ISNULL(IsDoctor, 0) = 1) and  ( DoctorName like @prefixText  or  DoctorCode like @prefixText )
order by DoctorName asc";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["DoctorName"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer_WithoutGeneral(string prefixText)
    {
        string sql = "";

        if (Session["UserType"].ToString() == "Admin")
        {
            sql = @"select top 15 CustomerCode+' : '+CustomerName+' |' + CAST(CustomerMasterId  as nvarchar(max)) as Customer from tblCustMaster with(nolock) 
where   IsActive=1 AND CustomerCode IS NOT NULL  and  CustomerName like @prefixText
order by CustomerName asc";
        }
        else
        {
            sql = "select top 15 CustomerCode+' : '+CustomerName+' |' + CAST(CustomerMasterId  as nvarchar(max)) as Customer from tblCustMaster with(nolock)  where  IsActive=1 AND CustomerCode IS NOT NULL  and ProgramTypeId <>4   and   CustomerName like  @prefixText";
        }

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProduct2(string prefixText)
    {
        string sql = "SELECT ProductName+':'+ProductCode AS Product FROM tblProduct WHERE ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProduct3(string prefixText)
    {
        string sql = "SELECT ProductCode+':'+ProductName+':'+PackSize AS Product FROM tblProduct WHERE ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetEmpInfo(string prefixText)
    {
        string sql = "";

        if (Session["UserType"].ToString() == "Admin")
        {
            sql = "select EmpMasterCode+':'+EmpName as EmpInfo from tblEmpGeneralInfo where EmpName like @prefixText";
        }
        else
        {
            string comUnit = Session["ComUnitId"].ToString();
            sql = "select EmpMasterCode+':'+EmpName as EmpInfo from tblEmpGeneralInfo where ComUnitId='" + comUnit.Trim() + "'  and  EmpName like @prefixText";
        }

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["EmpInfo"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProductByMenufracturer(string prefixText, string contextKey)
    {
        int manufac = 0;
        if (contextKey != "")
        {
            string[] St = new string[] {};
            DataTable dT = new DataTable();
            string[] mainstrnin = contextKey.Split(',');
            manufac = Convert.ToInt32(mainstrnin[0]);
        }
        string sql = "SELECT ProductCode+':'+ProductName AS Product FROM tblProduct WHERE ProductName like @prefixText and ManufacId ='" + manufac + "'";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProduct(string prefixText, string contextKey)
    {
        string sql = "SELECT ProductCode+' : '+ProductName AS Product FROM tblProduct WITH (NOLOCK) WHERE GroupId=1 AND IsActive=1 and ProductName like @prefixText ";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }
}
