using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerInvoiceAgingDAL
    {
        private readonly ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public DataTable GetCustomerInvoiceAging(DateTime? asOnDate, DateTime? fromDate, DateTime? toDate, int? comUnitId, int? customerMasterId, int? salesPersonId, bool onlyDueInvoices)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@AsOnDate", (object)asOnDate ?? DBNull.Value),
                new SqlParameter("@FromDate", (object)fromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object)toDate ?? DBNull.Value),
                new SqlParameter("@ComUnitId", (object)comUnitId ?? DBNull.Value),
                new SqlParameter("@CustomerMasterId", (object)customerMasterId ?? DBNull.Value),
                new SqlParameter("@SalesPersonId", (object)salesPersonId ?? DBNull.Value),
                new SqlParameter("@OnlyDueInvoices", onlyDueInvoices)
            };

            return aInternalDal.GetDataTableAction("sp_Report_CustomerInvoiceAgingOutstanding", parameters, "SSIDB");
        }

        public void LoadDepotDropdown(DropDownList ddl)
        {
            const string query = "SELECT ComUnitId, ComUnitCode + ' - ' + ComUnitName AS ComUnitName FROM tblCompanyUnit ORDER BY ComUnitName ASC";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", query);
        }

        public void LoadCustomerDropdown(DropDownList ddl, int? comUnitId = null)
        {
            string query = "SELECT CustomerMasterId, CustomerCode + ' - ' + CustomerName AS CustomerName FROM tblCustMaster WHERE IsActive = 1";
            var parameters = new List<SqlParameter>();
            if (comUnitId.HasValue && comUnitId.Value > 0)
            {
                query += " AND ComUnitId = @ComUnitId";
                parameters.Add(new SqlParameter("@ComUnitId", comUnitId.Value));
            }
            query += " ORDER BY CustomerName ASC";

            if (parameters.Count > 0)
            {
                aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", query, parameters);
            }
            else
            {
                aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", query);
            }
        }

        public void LoadSalesPersonDropdown(DropDownList ddl)
        {
            const string query = @"
                SELECT DISTINCT E.EmpInfoId, E.EmpName 
                FROM tblEmpGeneralInfo E
                INNER JOIN tblCustMaster C ON C.SalesPersonId = E.EmpInfoId
                WHERE E.EmployeeStatus = 'Active' OR E.EmployeeStatus IS NULL
                ORDER BY E.EmpName ASC";

            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "EmpName", "EmpInfoId", query);
        }
    }
}
