using System;
using System.Data;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class StockTransferReportBLL
    {
        private StockTransferReportDAL _dal = new StockTransferReportDAL();

        public DataTable LoadStockTransferReport(string fromDate, string toDate, string fromLocationId, string toLocationId)
        {
            return _dal.LoadStockTransferReport(fromDate, toDate, fromLocationId, toLocationId);
        }

        public void LoadCompanyUnits(DropDownList ddl)
        {
            DataTable dt = _dal.LoadCompanyUnits();
            ddl.DataSource = dt;
            ddl.DataTextField = "ComUnitName";
            ddl.DataValueField = "ComUnitId";
            ddl.DataBind();
        }
    }
}
