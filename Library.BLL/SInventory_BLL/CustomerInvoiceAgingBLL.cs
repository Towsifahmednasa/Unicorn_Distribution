using System;
using System.Data;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class CustomerInvoiceAgingBLL
    {
        private readonly CustomerInvoiceAgingDAL aDal = new CustomerInvoiceAgingDAL();

        public DataTable GetCustomerInvoiceAging(DateTime? asOnDate, DateTime? fromDate, DateTime? toDate, int? comUnitId, int? customerMasterId, int? salesPersonId, bool onlyDueInvoices)
        {
            return aDal.GetCustomerInvoiceAging(asOnDate, fromDate, toDate, comUnitId, customerMasterId, salesPersonId, onlyDueInvoices);
        }

        public void LoadDepotDropdown(DropDownList ddl)
        {
            aDal.LoadDepotDropdown(ddl);
        }

        public void LoadCustomerDropdown(DropDownList ddl, int? comUnitId = null)
        {
            aDal.LoadCustomerDropdown(ddl, comUnitId);
        }

        public void LoadSalesPersonDropdown(DropDownList ddl)
        {
            aDal.LoadSalesPersonDropdown(ddl);
        }
    }
}
