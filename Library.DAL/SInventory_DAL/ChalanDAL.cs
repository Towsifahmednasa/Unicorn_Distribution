using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ChalanDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForChalanInfo(ChalanInfo aChalanInfo)
        {
            string insertQuery = @"insert into tblChalanInfo (ChalanId,ChalanDate,ChalanNo,TrackNo,DriverName,FromComUnitCode,FromComUnitName,FromComUnitAddress,ToComUnitCode,ToComUnitName,ToComUnitAddress,TotalValue,TotalVat,GrandTotal,ManufacId,IsDeliver,FromComUnitId) 
            values (@ChalanId,@ChalanDate,@ChalanNo,@TrackNo,@DriverName,@FromComUnitCode,@FromComUnitName,@FromComUnitAddress,@ToComUnitCode,@ToComUnitName,@ToComUnitAddress,@TotalValue,@TotalVat,@GrandTotal,@ManufacId,@IsDeliver,@FromComUnitId)";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", aChalanInfo.ChalanId),
                new SqlParameter("@ChalanDate", aChalanInfo.ChalanDate),
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(aChalanInfo.ChalanNo)),
                new SqlParameter("@TrackNo", SInventorySql.DbValue(aChalanInfo.TrackNo)),
                new SqlParameter("@DriverName", SInventorySql.DbValue(aChalanInfo.DriverName)),
                new SqlParameter("@FromComUnitCode", SInventorySql.DbValue(aChalanInfo.FromComUnitCode)),
                new SqlParameter("@FromComUnitName", SInventorySql.DbValue(aChalanInfo.FromComUnitName)),
                new SqlParameter("@FromComUnitAddress", SInventorySql.DbValue(aChalanInfo.FromComUnitAddress)),
                new SqlParameter("@ToComUnitCode", SInventorySql.DbValue(aChalanInfo.ToComUnitCode)),
                new SqlParameter("@ToComUnitName", SInventorySql.DbValue(aChalanInfo.ToComUnitName)),
                new SqlParameter("@ToComUnitAddress", SInventorySql.DbValue(aChalanInfo.ToComUnitAddress)),
                new SqlParameter("@TotalValue", aChalanInfo.TotalValue),
                new SqlParameter("@TotalVat", aChalanInfo.TotalVat),
                new SqlParameter("@GrandTotal", aChalanInfo.GrandTotal),
                new SqlParameter("@ManufacId", aChalanInfo.ManufacId),
                new SqlParameter("@IsDeliver", false),
                new SqlParameter("@FromComUnitId", aChalanInfo.fromunitid)
            });
        }

        public bool SaveDataForChalanDetail(ChalanDetail aChalanDetail)
        {
            string insertQuery = @"insert into tblChalanDetail (ChalanDetailsId,ProductCode,ProductName,Quantity,BatchNo,UnitPrice,Value,Vat,ValueWVat,ChalanId,DCStoreId) 
            values (@ChalanDetailsId,@ProductCode,@ProductName,@Quantity,@BatchNo,@UnitPrice,@Value,@Vat,@ValueWVat,@ChalanId,@DCStoreId)";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                new SqlParameter("@ChalanDetailsId", aChalanDetail.ChalanDetailId),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aChalanDetail.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aChalanDetail.ProductName)),
                new SqlParameter("@Quantity", aChalanDetail.Quantity),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aChalanDetail.BatchNo)),
                new SqlParameter("@UnitPrice", aChalanDetail.UnitPrice),
                new SqlParameter("@Value", aChalanDetail.Value),
                new SqlParameter("@Vat", aChalanDetail.Vat),
                new SqlParameter("@ValueWVat", aChalanDetail.ValueWVat),
                new SqlParameter("@ChalanId", aChalanDetail.ChalanId),
                new SqlParameter("@DCStoreId", aChalanDetail.DCStoreId)
            });
        }
        public bool ChalanUpdate(string chalanId)
        {
            string query = @"UPDATE dbo.tblChalanInfo SET IsDeliver='True' WHERE ChalanId=@ChalanId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", SInventorySql.DbValue(chalanId))
            });
        }
        public bool SubChalanUpdate(string chalanId)
        {
            string query = @"UPDATE dbo.tblSubDepotChalanReturnInfo SET IsDeliver='True' WHERE SChalanId=@SChalanId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@SChalanId", SInventorySql.DbValue(chalanId))
            });
        }
        public bool IsChalanAlreadyReceived(string chalanId)
        {
            // Race-safe duplicate gate for chalan receive. UPDLOCK is held until the
            // ambient transaction ends, so a concurrent receive of the same chalan blocks
            // here and then sees the first transaction's committed IsDeliver marker.
            // Uses the EntLib connection (same pool as Execute) so the TransactionScope
            // stays on one connection string and is not promoted to MSDTC.
            string query = @"SELECT ISNULL(IsDeliver,'') AS IsDeliver FROM dbo.tblChalanInfo WITH (UPDLOCK, ROWLOCK) WHERE ChalanId=@ChalanId";
            DataTable dt = aCommonInternalDal.DataContainerDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", SInventorySql.DbValue(chalanId))
            }, "SSIDB");
            if (dt.Rows.Count == 0)
            {
                return true; // unknown chalan: nothing valid to receive
            }
            string v = dt.Rows[0][0].ToString().Trim();
            return v.Equals("True", StringComparison.OrdinalIgnoreCase) || v == "1";
        }
        public bool SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)
        {
            string insertQuery = @" 

insert into tblDCStoreFreeze (DCStoreFreezeId,StorageLocation,TotalQuantity,ProductCode,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,ChalanNo,ChalanDate,StockQty,DamageQty,StockRcvDate,StockCondition,ComUnitId,DCStoreId,ChalanDetailsId) 
            values (@DCStoreFreezeId,@StorageLocation,@TotalQuantity,@ProductCode,@ProductName,@PackSize,@BatchNo,@ExpDate,@ReceiveDate,@ChalanNo,@ChalanDate,@StockQty,@DamageQty,@StockRcvDate,@StockCondition,@ComUnitId,@DCStoreId,@ChalanDetailsId)";

            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                new SqlParameter("@DCStoreFreezeId", aDcStoreFreezeDao.DCStoreFreezeId),
                new SqlParameter("@StorageLocation", SInventorySql.DbValue(aDcStoreFreezeDao.StorageLocation)),
                new SqlParameter("@TotalQuantity", aDcStoreFreezeDao.TotalQuantity),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aDcStoreFreezeDao.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aDcStoreFreezeDao.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aDcStoreFreezeDao.PackSize)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aDcStoreFreezeDao.BatchNo)),
                new SqlParameter("@ExpDate", aDcStoreFreezeDao.ExpDate),
                new SqlParameter("@ReceiveDate", aDcStoreFreezeDao.ReceiveDate),
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(aDcStoreFreezeDao.ChalanNo)),
                new SqlParameter("@ChalanDate", aDcStoreFreezeDao.ChalanDate),
                new SqlParameter("@StockQty", aDcStoreFreezeDao.StockQty),
                new SqlParameter("@DamageQty", aDcStoreFreezeDao.DamageQty),
                new SqlParameter("@StockRcvDate", aDcStoreFreezeDao.StockRcvDate),
                new SqlParameter("@StockCondition", SInventorySql.DbValue(aDcStoreFreezeDao.StockCondition)),
                new SqlParameter("@ComUnitId", aDcStoreFreezeDao.ComUnitId),
                new SqlParameter("@DCStoreId", aDcStoreFreezeDao.DCStoreId),
                new SqlParameter("@ChalanDetailsId", SInventorySql.DbValue(aDcStoreFreezeDao.ChalanDetailsId))
            });
        }
        public bool DCStockInDALMainSub(DCStockNew aDcStockNew)
        {
            string query = @"INSERT INTO dbo.tblDCStore
  (DCStoreId,StorageLocation,ProductCode,ProductName,PackSize,BatchNo,TotalQuantity,ExpDate,MfgDate,ReceiveDate,ChalanNo,ChalanDate,ComUnitId,StockQty,DamageQty,StockRcvDate,SChalanDetailsId,StockCondition)
  VALUES (@DCStoreId,@StorageLocation,@ProductCode,@ProductName,@PackSize,@BatchNo,@TotalQuantity,@ExpDate,@MfgDate,@ReceiveDate,@ChalanNo,@ChalanDate,@ComUnitId,@StockQty,@DamageQty,@StockRcvDate,@SChalanDetailsId,'Available')";
            return SInventorySql.Execute(query, DcStockParameters(aDcStockNew, true));
        }
//        public bool DCStockInDALMain(DCStockNew aDcStockNew)
//        {
//            string query = @"INSERT INTO dbo.tblDCStore
//  (DCStoreId,StorageLocation,ProductCode,ProductName,PackSize,BatchNo,TotalQuantity,ExpDate,MfgDate,ReceiveDate,ChalanNo,ChalanDate,ComUnitId,StockQty,DamageQty,StockRcvDate,ChalanDetailsId,StockCondition)
//  VALUES (@DCStoreId,@StorageLocation,@ProductCode,@ProductName,@PackSize,@BatchNo,@TotalQuantity,@ExpDate,@MfgDate,@ReceiveDate,@ChalanNo,@ChalanDate,@ComUnitId,@StockQty,@DamageQty,@StockRcvDate,@ChalanDetailsId,'Available')";
//            return SInventorySql.Execute(query, DcStockParameters(aDcStockNew, false));
//        }

        public bool DCStockInDALMain(DCStockNew aDcStockNew)
        {
            string query = @"
        -- Inward Receive for Destination Depot
        INSERT INTO dbo.tblDCStore 
        (
            DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
            TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
            ComUnitId, StockQty, DamageQty, StockRcvDate, ChalanDetailsId,
            StockCondition, Trans_Type, CostPerUnit, TotalCostAmount,
            ReferenceDCStoreId, ReferenceTransactionDate
        )
        SELECT
            (SELECT ISNULL(MAX(DCStoreId), 0) + 1 FROM tblDCStore),
            ISNULL(DS.StorageLocation, 'Central Depot'), 
            CD.ProductCode, 
            CD.ProductName, 
            ISNULL(DS.PackSize, ISNULL((SELECT TOP 1 PackSize FROM tblProduct WHERE ProductCode = CD.ProductCode), '')), 
            CD.BatchNo,
            @StockQty, 
            ISNULL(DS.ExpDate, GETDATE()), 
            ISNULL(DS.MfgDate, GETDATE()), 
            GETDATE(), 
            @ChalanNo, 
            @ChalanDate,
            @ComUnitId,
            @StockQty,
            0, 
            GETDATE(),
            @ChalanDetailsId,
            'B2BReceive', 
            'Receive',
            ISNULL(DS.CostPerUnit, ISNULL((SELECT TOP 1 CostPrice FROM tblUnitPrice WHERE ProductCode = CD.ProductCode AND IsActive = 1), 0)),
            @StockQty * ISNULL(DS.CostPerUnit, ISNULL((SELECT TOP 1 CostPrice FROM tblUnitPrice WHERE ProductCode = CD.ProductCode AND IsActive = 1), 0)),
            ISNULL(DS.DCStoreId, 0),
            GETDATE()
        FROM dbo.tblChalanDetail AS CD
        LEFT JOIN dbo.tblDCStore AS DS ON DS.DCStoreId = CD.DCStoreId
        WHERE CD.ChalanDetailsId = @ChalanDetailsId;";

            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@ComUnitId",       SInventorySql.DbValue(aDcStockNew.ComUnitId)),
                new SqlParameter("@ChalanDetailsId", SInventorySql.DbValue(aDcStockNew.ChalanDetailsId)),
                new SqlParameter("@StockQty",        SInventorySql.DbValue(aDcStockNew.StockQty)),
                new SqlParameter("@ChalanNo",        SInventorySql.DbValue(aDcStockNew.ChalanNo)),
                new SqlParameter("@ChalanDate",      SInventorySql.DbValue(aDcStockNew.ChalanDate))
            });
        }
        public DataTable DCStoreReport(string reqId, string TYpe)
        {
            string query = "";

            if (TYpe == "Report")
            {
                query = @"SELECT tblChalanDetail.UnitPrice,tblChalanDetail.ValueWVat VATAmountPerUnit,DS.MfgDate,DS.ExpDate,tblProduct.ProductName, tblChalanDetail.Quantity StockQty,* FROM dbo.tblChalanInfo
                            LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId
                            LEFT JOIN dbo.tblDCStore DS ON dbo.tblChalanDetail.DCStoreId = DS.DCStoreId
                            LEFT JOIN dbo.tblStockInTransfar ON DS.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
                            LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
                            LEFT JOIN dbo.tblMIGODetail ON dbo.tblCentralStore.MigoDetailID = dbo.tblMIGODetail.MigoDetailID
                            LEFT JOIN dbo.tblProduct ON dbo.tblChalanDetail.ProductCode = dbo.tblProduct.ProductCode
                            LEFT JOIN dbo.tblCompanyUnit ON DS.ComUnitId = dbo.tblCompanyUnit.ComUnitId 
                            LEFT JOIN dbo.tblUnitPrice ON DS.ProductCode = dbo.tblUnitPrice.ProductCode 
                            WHERE tblChalanInfo.ChalanNo=@ChalanNo";
            }

            if (TYpe == "Entry")
            {
                query = @"SELECT tblUnitPrice.UnitPrice,tblUnitPrice.VATAmountPerUnit,DS.MfgDate,DS.ExpDate,tblProduct.ProductName,* FROM dbo.tblDCStore
                            LEFT JOIN dbo.tblChalanDetail ON dbo.tblDCStore.ChalanDetailsId = dbo.tblChalanDetail.ChalanDetailsId
                            LEFT JOIN dbo.tblDCStore DS ON dbo.tblChalanDetail.DCStoreId = DS.DCStoreId
                            LEFT JOIN dbo.tblStockInTransfar ON DS.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
                            LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
                            LEFT JOIN dbo.tblMIGODetail ON dbo.tblCentralStore.MigoDetailID = dbo.tblMIGODetail.MigoDetailID
                            LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode
                            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblDCStore.ComUnitId = dbo.tblCompanyUnit.ComUnitId 
                            LEFT JOIN dbo.tblUnitPrice ON dbo.tblDCStore.ProductCode = dbo.tblUnitPrice.ProductCode 
                            WHERE tblUnitPrice.IsActive=1 AND tblDCStore.ChalanNo=@ChalanNo";
            }
            
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(reqId))
            });
        }
        public bool DCStockInDAL(DCStockNew aDcStockNew)
        {
            string query = @"INSERT INTO dbo.tblDCStoreFreeze
  (DCStoreId,DCStoreFreezeId,ProductCode,ProductName,PackSize,BatchNo,TotalQuantity,ExpDate,ChalanNo,ChalanDate,ComUnitId,StockQty,DamageQty,StockCondition)
  VALUES (@DCStoreId,@DCStoreFreezeId,@ProductCode,@ProductName,@PackSize,@BatchNo,@TotalQuantity,@ExpDate,@ChalanNo,@ChalanDate,@ComUnitId,@StockQty,@DamageQty,'StockInTransfer')";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@DCStoreId", aDcStockNew.DCStoreId),
                new SqlParameter("@DCStoreFreezeId", aDcStockNew.DCStoreFreezeId),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aDcStockNew.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aDcStockNew.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aDcStockNew.PackSize)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aDcStockNew.BatchNo)),
                new SqlParameter("@TotalQuantity", aDcStockNew.TotalQuantity),
                new SqlParameter("@ExpDate", aDcStockNew.ExpDate),
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(aDcStockNew.ChalanNo)),
                new SqlParameter("@ChalanDate", aDcStockNew.ChalanDate),
                new SqlParameter("@ComUnitId", aDcStockNew.ComUnitId),
                new SqlParameter("@StockQty", aDcStockNew.StockQty),
                new SqlParameter("@DamageQty", aDcStockNew.DamageQty)
            });
        }
        public DataTable DCInfoWithDCId(string dcstoreId)
        {
            string query = "SELECT * FROM dbo.tblDCStore WHERE DCStoreId=@DCStoreId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@DCStoreId", SInventorySql.DbValue(dcstoreId))
            });
        }
        public void LoadManufac(DropDownList aDropDownList)
        {
            string query = @"SELECT * FROM dbo.tblManufacturer ";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "ManufacName", "ManufacId", query, "SSIDB");
        }
        public bool HasProductcode(ChalanDetail aChalanDetail)
        {
            string query = "select * from tblDCStock where ProductCode = @ProductCode and BatchNo=@BatchNo";
            DataTable dataTable = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aChalanDetail.ProductCode)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aChalanDetail.BatchNo))
            });
            return dataTable.Rows.Count > 0;
        }

        public DataTable LoadChalanView()
        {
            string query = @"SELECT *  FROM tblChalanInfo ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ChalanLoadInReceive(string comunitId)
        {
            string query = @"SELECT * FROM dbo.tblChalanInfo
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblCompanyUnit.ComUnitCode=dbo.tblChalanInfo.ToComUnitCode
            WHERE tblCompanyUnit.ComUnitId=@ComUnitId AND (IsDeliver IS NULL OR IsDeliver IN ('False', 'false', '0'))";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ComUnitId", SInventorySql.DbValue(comunitId))
            });
        }
        public DataTable SubdeportChalanLoadInReceive(string comunitId)
        {
            string query = @"SELECT * FROM dbo.tblSubDepotChalanReturnInfo
            WHERE (IsDeliver IS NULL OR IsDeliver IN ('False', 'false', '0'))";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetChalanReceieve(string id)
        {
            string query = @"SELECT tblProduct.ProductName,*,tblChalanInfo.ChalanDate AS ReceiveDate FROM dbo.tblChalanInfo
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblCompanyUnit.ComUnitCode=dbo.tblChalanInfo.ToComUnitCode
            LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId
            LEFT JOIN dbo.tblProduct ON dbo.tblChalanDetail.ProductCode = dbo.tblProduct.ProductCode
            LEFT JOIN dbo.tblDCStore ON dbo.tblChalanDetail.DCStoreId=dbo.tblDCStore.DCStoreId
            WHERE (tblChalanInfo.IsDeliver IS NULL OR tblChalanInfo.IsDeliver IN ('False', 'false', '0')) and tblChalanInfo.ChalanId=@ChalanId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", SInventorySql.DbValue(id))
            });
        }
        public DataTable SubChalanReceieve(string id)
        {
            string query = @"SELECT tblProduct.ProductName,*,tblSubDepotChalanReturnInfo.ChalanDate AS ReceiveDate FROM dbo.tblSubDepotChalanReturnInfo
            LEFT JOIN dbo.tblSubDepotChalanRetuenDetail ON dbo.tblSubDepotChalanReturnInfo.SChalanId = tblSubDepotChalanRetuenDetail.SChalanId
            LEFT JOIN dbo.tblProduct ON dbo.tblSubDepotChalanRetuenDetail.ProductCode = dbo.tblProduct.ProductCode
            LEFT JOIN dbo.tblDCStore ON dbo.tblSubDepotChalanRetuenDetail.DCStoreId=dbo.tblDCStore.DCStoreId
            WHERE tblSubDepotChalanReturnInfo.IsDeliver='False' and tblSubDepotChalanReturnInfo.SChalanId=@SChalanId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@SChalanId", SInventorySql.DbValue(id))
            });
        }
        public DataTable LoadChalanById(string id)
        {
            string query = @"SELECT * FROM dbo.tblChalanInfo WHERE ChalanId=@ChalanId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", SInventorySql.DbValue(id))
            });
        }
//        public DataTable ChalanReport(string id)
//        {
//            string query = @"SELECT * FROM dbo.tblChalanInfo
//            LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId WHERE ChalanNo='"+id+"'";
//            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
//        }
        public DataTable LoadComunit(string comunitCode)
        {
            string query = @"SELECT * FROM dbo.tblCompanyUnit WHERE ComUnitCode=@ComUnitCode";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ComUnitCode", SInventorySql.DbValue(comunitCode))
            });
        }
        public ChalanInfo ChalanEditLoad(string ChalanId)
        {
            string query = "select * from tblChalanInfo where ChalanId = @ChalanId";
            DataTable dataTable = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", SInventorySql.DbValue(ChalanId))
            });
            ChalanInfo aChalanInfo = new ChalanInfo();
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                aChalanInfo.ChalanId = Int32.Parse(row["ChalanId"].ToString());
                aChalanInfo.ChalanDate = Convert.ToDateTime(row["ChalanDate"].ToString());
                aChalanInfo.ChalanNo = row["ChalanNo"].ToString();
                aChalanInfo.TrackNo = row["TrackNo"].ToString();
                aChalanInfo.DriverName = row["DriverName"].ToString();
                aChalanInfo.ToComUnitCode = row["ToComUnitCode"].ToString();
                aChalanInfo.ToComUnitName = row["ToComUnitName"].ToString();
                aChalanInfo.ToComUnitAddress = row["ToComUnitAddress"].ToString();
                aChalanInfo.FromComUnitCode = row["FromComUnitCode"].ToString();
                aChalanInfo.FromComUnitName = row["FromComUnitName"].ToString();
                aChalanInfo.FromComUnitAddress = row["FromComUnitAddress"].ToString();
                aChalanInfo.TotalValue = Convert.ToDecimal(row["TotalValue"].ToString());
                aChalanInfo.TotalVat = Convert.ToDecimal(row["TotalVat"].ToString());
                aChalanInfo.GrandTotal = Convert.ToDecimal(row["GrandTotal"].ToString());
            }
            return aChalanInfo;
        }
         public ChalanDetail ChalanDetailEditLoad(string ChalanDetailId)
        {
            string query = "select * from tblChalanDetail where ChalanDetailId = @ChalanDetailId";
            DataTable dataTable = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanDetailId", SInventorySql.DbValue(ChalanDetailId))
            });
            ChalanDetail aChalanDetail = new ChalanDetail();
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                aChalanDetail.ChalanDetailId = Int32.Parse(row["ChalanDetailId"].ToString());
                aChalanDetail.ProductCode = row["ProductCode"].ToString();
                aChalanDetail.ProductName = row["ProductName"].ToString();
                aChalanDetail.Quantity = Convert.ToDecimal(row["Quantity"].ToString());
                aChalanDetail.BatchNo = row["BatchNo"].ToString();
                aChalanDetail.UnitPrice = Convert.ToDecimal(row["UnitPrice"].ToString());
                aChalanDetail.Value = Convert.ToDecimal(row["Value"].ToString());
                aChalanDetail.Vat = Convert.ToDecimal(row["Vat"].ToString());
                aChalanDetail.ValueWVat = Convert.ToDecimal(row["ValueWVat"].ToString());
            }
            return aChalanDetail;
        }
        
        public bool UpdateaChalanInfo(ChalanInfo aChalanInfo)
        {
            string query = @"UPDATE tblChalanInfo SET ChalanDate=@ChalanDate,ChalanNo=@ChalanNo,FromComUnitCode=@FromComUnitCode,FromComUnitName=@FromComUnitName, FromComUnitAddress=@FromComUnitAddress,ToComUnitCode=@ToComUnitCode,ToComUnitName=@ToComUnitName, ToComUnitAddress=@ToComUnitAddress, TotalValue=@TotalValue,TotalVat=@TotalVat,GrandTotal=@GrandTotal WHERE ChalanId=@ChalanId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanDate", aChalanInfo.ChalanDate),
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(aChalanInfo.ChalanNo)),
                new SqlParameter("@FromComUnitCode", SInventorySql.DbValue(aChalanInfo.FromComUnitCode)),
                new SqlParameter("@FromComUnitName", SInventorySql.DbValue(aChalanInfo.FromComUnitName)),
                new SqlParameter("@FromComUnitAddress", SInventorySql.DbValue(aChalanInfo.FromComUnitAddress)),
                new SqlParameter("@ToComUnitCode", SInventorySql.DbValue(aChalanInfo.ToComUnitCode)),
                new SqlParameter("@ToComUnitName", SInventorySql.DbValue(aChalanInfo.ToComUnitName)),
                new SqlParameter("@ToComUnitAddress", SInventorySql.DbValue(aChalanInfo.ToComUnitAddress)),
                new SqlParameter("@TotalValue", aChalanInfo.TotalValue),
                new SqlParameter("@TotalVat", aChalanInfo.TotalVat),
                new SqlParameter("@GrandTotal", aChalanInfo.GrandTotal),
                new SqlParameter("@ChalanId", aChalanInfo.ChalanId)
            });
        }
        public bool UpdateaChalanDetail(ChalanDetail aChalanDetail)
        {
            string query = @"UPDATE tblChalanDetail SET ProductCode=@ProductCode,ProductName=@ProductName,Quantity=@Quantity,BatchNo=@BatchNo,UnitPrice=@UnitPrice,Value=@Value,Vat=@Vat,ValueWVat=@ValueWVat WHERE ChalanDetailsId=@ChalanDetailId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aChalanDetail.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aChalanDetail.ProductName)),
                new SqlParameter("@Quantity", aChalanDetail.Quantity),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aChalanDetail.BatchNo)),
                new SqlParameter("@UnitPrice", aChalanDetail.UnitPrice),
                new SqlParameter("@Value", aChalanDetail.Value),
                new SqlParameter("@Vat", aChalanDetail.Vat),
                new SqlParameter("@ValueWVat", aChalanDetail.ValueWVat),
                new SqlParameter("@ChalanDetailId", aChalanDetail.ChalanDetailId)
            });
        }

        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblProduct where ProductCode=@ProductCode ";
            aDataTableEmpInfo = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(productId == null ? null : productId.Trim()))
            });
            return aDataTableEmpInfo;
        }
//        public DataTable GetProductDcStore(string productCode,string comunitId)
//        {
//            DataTable aDataTableEmpInfo = new DataTable();
//            string query = @"SELECT ISNULL(VATAmountPerUnit,0)VATAmountPerUnit,ISNULL(VATPercentage,0)VATPercentage,UnitPrice,*,
//            tblProduct.ProductCode AS PCode , tblProduct.ProductName AS PName 
//            FROM dbo.tblDCStore
//            LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode 
//            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblDCStore.ComUnitId = dbo.tblCompanyUnit.ComUnitId 
//            LEFT JOIN dbo.tblUnitPrice ON dbo.tblDCStore.ProductCode=dbo.tblUnitPrice.ProductCode
//            WHERE  ComUnitCode=@ComUnitCode AND tblDCStore.ProductCode=@ProductCode AND tblUnitPrice.IsActive='True' AND StockQty>0 ";
//            //tblDCStore.StockInTransfarId is not null AND
//            aDataTableEmpInfo = SInventorySql.GetDataTable(query, new List<SqlParameter>
//            {
//                new SqlParameter("@ComUnitCode", SInventorySql.DbValue(comunitId)),
//                new SqlParameter("@ProductCode", SInventorySql.DbValue(productCode))
//            });
//            return aDataTableEmpInfo;
//        }


        public DataTable GetProductDcStore(string productCode, string comunitId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"
        SELECT 
            MIN(DS.DCStoreId)              AS DCStoreId,
            P.ProductCode                  AS PCode,
            P.ProductName                  AS PName,
            DS.BatchNo,
            DS.ExpDate,
            MIN(DS.ReceiveDate)            AS ReceiveDate,
            ISNULL(UP.UnitPrice, 0)        AS UnitPrice,
            ISNULL(UP.VATAmountPerUnit, 0) AS VATAmountPerUnit,
            ISNULL(UP.VATPercentage, 0)    AS VATPercentage,
            SUM(DS.StockQty)               AS StockQty

        FROM dbo.tblDCStore AS DS

        LEFT JOIN dbo.tblProduct AS P 
            ON DS.ProductCode = P.ProductCode

        LEFT JOIN dbo.tblCompanyUnit AS CU 
            ON DS.ComUnitId = CU.ComUnitId

        LEFT JOIN dbo.tblUnitPrice AS UP 
            ON DS.ProductCode = UP.ProductCode
            AND UP.IsActive = 1

        WHERE CU.ComUnitCode = @ComUnitCode
          AND DS.ProductCode  = @ProductCode
          AND DS.Trans_Type  IN ('Receive', 'Issue')

        GROUP BY 
            P.ProductCode,
            P.ProductName,
            DS.BatchNo,
            DS.ExpDate,
            UP.UnitPrice,
            UP.VATAmountPerUnit,
            UP.VATPercentage

        HAVING SUM(DS.StockQty) > 0";

            aDataTableEmpInfo = SInventorySql.GetDataTable(query, new List<SqlParameter>
    {
        new SqlParameter("@ComUnitCode", SInventorySql.DbValue(comunitId)),
        new SqlParameter("@ProductCode", SInventorySql.DbValue(productCode))
    });

            return aDataTableEmpInfo;
        }

        public DataTable LoadComUnit(string ComUnitId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM tblCompanyUnit where ComUnitCode=@ComUnitCode ";
            aDataTable = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ComUnitCode", SInventorySql.DbValue(ComUnitId == null ? null : ComUnitId.Trim()))
            });
            return aDataTable;
        }

        public DataTable CentralStoreQuantity(CentralStore aCentralStore)
        {
            string query = "select * from tblCentralStore where ProductCode = @ProductCode and BatchNo=@BatchNo";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aCentralStore.ProductCode)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aCentralStore.BatchNo))
            });
        }

        public void UpdateDCStockQuantity(string stockId, string quantity, int chalanDetailsId = 0)
        {
            string updateQuery = @"
        INSERT INTO dbo.tblDCStore 
        (
            DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
            TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
            ComUnitId, StockQty, DamageQty, StockRcvDate, ReqId, ReqChildId,
            StockCondition, StockInTransfarId,
            Trans_Type, CostPerUnit, TotalCostAmount,
            ReferenceDCStoreId, ReferenceTransactionDate,
            ChalanDetailsId
        )
        SELECT TOP 1
            (SELECT ISNULL(MAX(DCStoreId), 0) + 1 FROM tblDCStore),
            DS.StorageLocation, DS.ProductCode, DS.ProductName, DS.PackSize, DS.BatchNo,
            0,
            DS.ExpDate, DS.MfgDate, GETDATE(),
            ISNULL(CI.ChalanNo, DS.ChalanNo),
            ISNULL(CI.ChalanDate, GETDATE()),
            DS.ComUnitId, @IssueQty * -1,
            0, GETDATE(), DS.ReqId, DS.ReqChildId,
            'B2BIssue', DS.StockInTransfarId,
            'Issue',
            ISNULL(DS.CostPerUnit, 0),
            @IssueQty * -1 * ISNULL(DS.CostPerUnit, 0),
            @DCStoreId,
            GETDATE(),
            CASE WHEN @ChalanDetailsId > 0 THEN @ChalanDetailsId ELSE DS.ChalanDetailsId END
        FROM tblDCStore DS
        LEFT JOIN tblChalanDetail CD ON CD.ChalanDetailsId = @ChalanDetailsId
        LEFT JOIN tblChalanInfo CI ON CD.ChalanId = CI.ChalanId
        WHERE DS.DCStoreId = @DCStoreId;";

            SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                new SqlParameter("@IssueQty", SInventorySql.DbValue(quantity)),
                new SqlParameter("@DCStoreId", SInventorySql.DbValue(stockId == null ? null : stockId.Trim())),
                new SqlParameter("@ChalanDetailsId", chalanDetailsId)
            });
        }

        public void UpdateDCStockQuantity(string stockId, string quantity)
        {
            UpdateDCStockQuantity(stockId, quantity, 0);
        }
      
            public DataTable ChalanReport(string id)
                    {
                        string query = @"SELECT tblDCStore.MfgDate,dbo.tblProduct.ProductName,* 
            FROM dbo.tblChalanInfo
            LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId
            LEFT JOIN dbo.tblDCStore ON dbo.tblChalanDetail.DCStoreId = dbo.tblDCStore.DCStoreId
            LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode 

            LEFT JOIN dbo.tblStockInTransfar ON dbo.tblDCStore.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
            LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
            LEFT JOIN dbo.tblMIGODetail ON dbo.tblCentralStore.MigoDetailID = dbo.tblMIGODetail.MigoDetailID
            WHERE tblChalanInfo.ChalanNo=@ChalanNo";
                        return SInventorySql.GetDataTable(query, new List<SqlParameter>
                        {
                            new SqlParameter("@ChalanNo", SInventorySql.DbValue(id))
                        });
                    }


            public DataTable SubDepotChalanReport(string id)
            {
                string query = @"SELECT tblDCStore.MfgDate,dbo.tblProduct.ProductName,SubDepotName AS ToComUnitName, SubDepotAddress AS ToComUnitAddress,SubDepotCode AS ToComUnitCode,* 
FROM dbo.tblSubDepotChalanInfo
LEFT JOIN dbo.tblSubDepotChalanDetail ON dbo.tblSubDepotChalanInfo.SChalanId = dbo.tblSubDepotChalanDetail.SChalanId
LEFT JOIN dbo.tblDCStore ON dbo.tblSubDepotChalanDetail.DCStoreId = dbo.tblDCStore.DCStoreId
LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode 

LEFT JOIN dbo.tblStockInTransfar ON dbo.tblDCStore.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
LEFT JOIN dbo.tblMIGODetail ON dbo.tblCentralStore.MigoDetailID = dbo.tblMIGODetail.MigoDetailID
WHERE tblSubDepotChalanInfo.ChalanNo=@ChalanNo";
                return SInventorySql.GetDataTable(query, new List<SqlParameter>
                {
                    new SqlParameter("@ChalanNo", SInventorySql.DbValue(id))
                });
            }
        //Sub Deport

            public DataTable SubdeportDCStoreReport(string reqId)
            {
                string query = @"SELECT SubDepotCode AS ComUnitCode, SubDepotName AS ComUnitName,tblUnitPrice.UnitPrice,tblUnitPrice.VATAmountPerUnit,DS.MfgDate,DS.ExpDate,tblProduct.ProductName,* 
FROM dbo.tblSubDepotStore
LEFT JOIN dbo.tblSubDepotChalanDetail ON dbo.tblSubDepotStore.SChalanDetailsId = dbo.tblSubDepotChalanDetail.SChalanDetailsId
LEFT JOIN dbo.tblDCStore DS ON dbo.tblSubDepotChalanDetail.DCStoreId = DS.DCStoreId
LEFT JOIN dbo.tblStockInTransfar ON DS.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
LEFT JOIN dbo.tblMIGODetail ON dbo.tblCentralStore.MigoDetailID = dbo.tblMIGODetail.MigoDetailID
LEFT JOIN dbo.tblProduct ON dbo.tblSubDepotStore.ProductCode = dbo.tblProduct.ProductCode
LEFT JOIN dbo.tblSubDepot ON dbo.tblSubDepotStore.SubDepotId = dbo.tblSubDepot.SubDepotId 
LEFT JOIN dbo.tblUnitPrice ON dbo.tblSubDepotStore.ProductCode = dbo.tblUnitPrice.ProductCode 
WHERE tblUnitPrice.IsActive=1 AND tblSubDepotStore.ChalanNo=@ChalanNo";

                           // WHERE tblUnitPrice.IsActive=1 AND tblDCStore.ChalanNo='" + reqId + "'";
                return SInventorySql.GetDataTable(query, new List<SqlParameter>
                {
                    new SqlParameter("@ChalanNo", SInventorySql.DbValue(reqId))
                });
            }

        public DataTable GetProductWhStore(string productCode)
        {
            string query = @"SELECT CH.ReceiveId,CH.ProductCode AS PCode,CH.ProductName AS PName, Quantity AS StockQty,BatchNo,CH.ExpDate,ReceiveDate,CH.PackSize FROM tblCentralStore AS CH
                             LEFT JOIN tblUnitPrice AS UP ON CH.ProductId = UP.ProductId
                             WHERE Quantity > 0 AND CH.ProductCode = @ProductCode ORDER BY BatchNo";

            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductCode", SInventorySql.DbValue(productCode))
            });
        }

        private List<SqlParameter> DcStockParameters(DCStockNew aDcStockNew, bool isSubChalan)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@DCStoreId", aDcStockNew.DCStoreId),
                new SqlParameter("@StorageLocation", SInventorySql.DbValue(aDcStockNew.StorageLocation)),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aDcStockNew.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aDcStockNew.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aDcStockNew.PackSize)),
                new SqlParameter("@BatchNo", SInventorySql.DbValue(aDcStockNew.BatchNo)),
                new SqlParameter("@TotalQuantity", aDcStockNew.TotalQuantity),
                new SqlParameter("@ExpDate", aDcStockNew.ExpDate),
                new SqlParameter("@MfgDate", SInventorySql.DbValue(aDcStockNew.mfgdate)),
                new SqlParameter("@ReceiveDate", aDcStockNew.ReceiveDate),
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(aDcStockNew.ChalanNo)),
                new SqlParameter("@ChalanDate", aDcStockNew.ChalanDate),
                new SqlParameter("@ComUnitId", aDcStockNew.ComUnitId),
                new SqlParameter("@StockQty", aDcStockNew.StockQty),
                new SqlParameter("@DamageQty", aDcStockNew.DamageQty),
                new SqlParameter("@StockRcvDate", aDcStockNew.StockRcvDate)
            };

            parameters.Add(isSubChalan
                ? new SqlParameter("@SChalanDetailsId", SInventorySql.DbValue(aDcStockNew.ChalanDetailsId))
                : new SqlParameter("@ChalanDetailsId", SInventorySql.DbValue(aDcStockNew.ChalanDetailsId)));

            return parameters;
        }
    }

    
}
