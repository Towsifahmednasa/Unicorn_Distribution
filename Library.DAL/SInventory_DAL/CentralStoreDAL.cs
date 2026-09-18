using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class CentralStoreDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForStockReceive(CentralStore aReceive)
        {
            string insertQuery = @"insert into tblCentralStore (ReceiveId,ProductCode,ProductName,PackSize,BatchNo,Quantity,ExpDate,ReceiveDate,InternalNoteNo,StockInQty,UnitPrice,TotalAmount) 
            values (@ReceiveId,@ProductCode,@ProductName,@PackSize,@BatchNo,@Quantity,@ExpDate,@ReceiveDate,@InternalNoteNo,@StockInQty,@UnitPrice,@TotalAmount)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ReceiveId", aReceive.ReceiveId),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aReceive.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aReceive.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aReceive.PackSize)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aReceive.BatchNo)),
                new SqlParameter("@Quantity", aReceive.Quantity),
                new SqlParameter("@ExpDate", aReceive.ExpDate),
                new SqlParameter("@ReceiveDate", aReceive.ReceiveDate),
                new SqlParameter("@InternalNoteNo", SInventorySql.DbValue(aReceive.InternalNoteNo)),
                new SqlParameter("@StockInQty", aReceive.StockInQty),
                new SqlParameter("@UnitPrice", aReceive.UnitPrice),
                new SqlParameter("@TotalAmount", aReceive.TotalAmount)
            };
            return SInventorySql.Execute(insertQuery, parameters);
        }
        
        public bool SaveDataForCurrentStock(CurrentStock aReceive)
        {
            string insertQuery = @"insert into tblCurrentStock (StockId,ProductCode,ProductName,PackSize,Quantity,ComUnitId,ComUnitCode) 
            values (@StockId,@ProductCode,@ProductName,@PackSize,@Quantity,@ComUnitId,@ComUnitCode)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StockId", aReceive.StockId),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aReceive.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aReceive.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aReceive.PackSize)),
                new SqlParameter("@Quantity", aReceive.Quantity),
                new SqlParameter("@ComUnitId", aReceive.ComUnitId),
                new SqlParameter("@ComUnitCode", SInventorySql.DbValue(aReceive.StorageLocation))
            };
            return SInventorySql.Execute(insertQuery, parameters);
        }

        public bool HasProductName(CurrentStock aReceive)
        {
            string query = "select top 1 StockId from tblCurrentStock where ProductCode = @ProductCode";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aReceive.ProductCode))
            };
            return SInventorySql.Exists(query, parameters);
        }

        public DataTable CurrentStockQty(CurrentStock aReceive)
        {
            string query = "select * from tblCurrentStock where ProductCode = @ProductCode";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aReceive.ProductCode))
            };
            return SInventorySql.GetDataTable(query, parameters);
        }

        public DataTable CurrentStockReport()
        {
            string query = "select ProductCode, ProductName, PackSize, Quantity from tblCurrentStock";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadStockReceiveView()
        {
            string query = @"SELECT * FROM tblCentralStore ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CentralStore StockReceiveEditLoad(string ReceiveId)
        {
            string query = "select * from tblCentralStore where ReceiveId = @ReceiveId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ReceiveId", SInventorySql.DbValue(ReceiveId))
            };
            DataTable receiveTable = SInventorySql.GetDataTable(query, parameters);
            CentralStore aReceive = new CentralStore();
            if (receiveTable.Rows.Count > 0)
            {
                DataRow row = receiveTable.Rows[0];
                aReceive.ReceiveId = Int32.Parse(row["ReceiveId"].ToString());
                aReceive.InternalNoteNo = row["InternalNoteNo"].ToString();
                aReceive.ProductCode = row["ProductCode"].ToString();
                aReceive.ProductName = row["ProductName"].ToString();
                aReceive.PackSize = row["PackSize"].ToString();
                aReceive.BatchNo = row["BatchNo"].ToString();
                aReceive.Quantity = Convert.ToDecimal(row["Quantity"].ToString());
                aReceive.ExpDate = Convert.ToDateTime(row["ExpDate"].ToString());
                aReceive.ReceiveDate = Convert.ToDateTime(row["ReceiveDate"].ToString());
            }
            return aReceive;
        }
       
        public bool UpdateStockReceive(CentralStore aReceive)
        {
            string query = @"UPDATE tblCentralStore SET ProductName=@ProductName,ProductCode=@ProductCode,PackSize=@PackSize,BatchNo=@BatchNo,Quantity=@Quantity,ExpDate=@ExpDate,ReceiveDate=@ReceiveDate WHERE ReceiveId=@ReceiveId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProductName", SInventorySql.DbValue(aReceive.ProductName)),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aReceive.ProductCode)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aReceive.PackSize)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aReceive.BatchNo)),
                new SqlParameter("@Quantity", aReceive.Quantity),
                new SqlParameter("@ExpDate", aReceive.ExpDate),
                new SqlParameter("@ReceiveDate", aReceive.ReceiveDate),
                new SqlParameter("@ReceiveId", aReceive.ReceiveId)
            };
            return SInventorySql.Execute(query, parameters);
        }
       
        public DataTable StockCheck(string stockid)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * from tblCurrentStock where StockId = @StockId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StockId", SInventorySql.DbValue(stockid == null ? null : stockid.Trim()))
            };
            aDataTable = SInventorySql.GetDataTable(query, parameters);

            return aDataTable;
        }
        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblUnitPrice where IsActive=1 AND ProductCode=@ProductCode";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(productId == null ? null : productId.Trim()))
            };
            aDataTableEmpInfo= SInventorySql.GetDataTable(query, parameters);
            return aDataTableEmpInfo;
        }
        public void UpdateCurrentStockQuantity(string stockId,string Quantity)
        {
            string updateQuery = @"UPDATE tblCurrentStock SET Quantity=@Quantity WHERE ProductCode=@ProductCode";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Quantity", SInventorySql.DbValue(Quantity)),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(stockId == null ? null : stockId.Trim()))
            };
            SInventorySql.Execute(updateQuery, parameters);
        }
        //public bool UpdateCurrentStockQuantity(CurrentStock aReceive)
        //{
        //    string query = @"UPDATE tblCurrentStock SET Quantity='" + aReceive.Quantity + "' WHERE StockId=" + aReceive.StockId + "";
        //    return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        //}

        public void LoadCategoryDropdown(DropDownList ddl)
        {
            string queryStr = "select CategoryId, CategoryName from tblProCategory order by CategoryName";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CategoryName", "CategoryId", queryStr);
        }

        public void LoadProductDropdown(DropDownList ddl)
        {
            string queryStr = "select ProductId, (ProductCode + ' - ' + ProductName) as ProductInfo from tblProduct where IsActive=1 order by ProductName";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ProductInfo", "ProductId", queryStr);
        }

        public DataTable GetStockValuationLedger(string fromDate, string toDate, int? categoryId, int? productId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@FromDate", fromDate));
            sqlParameters.Add(new SqlParameter("@ToDate", toDate));
            sqlParameters.Add(new SqlParameter("@CategoryId", (object)categoryId ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("@ProductId", (object)productId ?? DBNull.Value));

            return aCommonInternalDal.GetDataTableAction("sp_Report_CentralStore_StockValuationLedger", sqlParameters, "SSIDB");
        }

        public DataTable GetNationalStockValuation(string fromDate, string toDate, int? categoryId, int? productId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@FromDate", fromDate));
            sqlParameters.Add(new SqlParameter("@ToDate", toDate));
            sqlParameters.Add(new SqlParameter("@CategoryId", (object)categoryId ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("@ProductId", (object)productId ?? DBNull.Value));

            return aCommonInternalDal.GetDataTableAction("sp_Report_NationalStockValuation_DateRange", sqlParameters, "SSIDB");
        }

        public DataTable GetNationalStockValuationBatchDetails(string productCode, string locationName, string fromDate, string toDate)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ProductCode", productCode));
            sqlParameters.Add(new SqlParameter("@LocationName", locationName));
            sqlParameters.Add(new SqlParameter("@FromDate", fromDate));
            sqlParameters.Add(new SqlParameter("@ToDate", toDate));

            return aCommonInternalDal.GetDataTableAction("sp_Report_NationalStockValuation_BatchDetails", sqlParameters, "SSIDB");
        }
    }
}
