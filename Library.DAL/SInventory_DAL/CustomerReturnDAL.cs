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
    public class CustomerReturnDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public int SaveCustomerReturn(CustomerReturnMasterDAO aCustomerReturnMasterDao) 
        {
             List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

             aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aCustomerReturnMasterDao.InvoiceId));
             aSqlParameterlist.Add(new SqlParameter("@TotalQty", aCustomerReturnMasterDao.TotalQty));
             aSqlParameterlist.Add(new SqlParameter("@TotalAmount", aCustomerReturnMasterDao.TotalAmount));
             aSqlParameterlist.Add(new SqlParameter("@DeductionCharge", aCustomerReturnMasterDao.DeductionCharge));
             aSqlParameterlist.Add(new SqlParameter("@NetReturnAmount", aCustomerReturnMasterDao.NetReturnAmount));
             aSqlParameterlist.Add(new SqlParameter("@EntryBy", aCustomerReturnMasterDao.EntryBy));
             aSqlParameterlist.Add(new SqlParameter("@EntryDate", aCustomerReturnMasterDao.EntryDate));
             aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aCustomerReturnMasterDao.ActionStatus));
            
            string insertQuery = @"INSERT INTO dbo.tblCustomerReturnMaster
                                    (
                                        InvoiceId,
                                        TotalQty,
                                        TotalAmount,
                                        DeductionCharge,
                                        NetReturnAmount,
                                        EntryBy,
                                        EntryDate,ActionStatus                                        
                                    )
                                    VALUES
                                    (   @InvoiceId,
                                        @TotalQty,
                                        @TotalAmount,
                                        @DeductionCharge,
                                        @NetReturnAmount,
                                        @EntryBy,
                                        @EntryDate,@ActionStatus
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }
        public int SaveCustomerReturnDetail(CustomerReturnDetailDAO aCustomerReturnDetailDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvoiceDetailId", aCustomerReturnDetailDao.InvoiceDetailId));
            aSqlParameterlist.Add(new SqlParameter("@ReturnQty", aCustomerReturnDetailDao.ReturnQty));
            aSqlParameterlist.Add(new SqlParameter("@ReturnAmount", aCustomerReturnDetailDao.ReturnAmount));
            aSqlParameterlist.Add(new SqlParameter("@CustReturnId", aCustomerReturnDetailDao.CustReturnId));
            

            string insertQuery = @"INSERT INTO dbo.tblCustomerReturnDetail
                                    (
                                        InvoiceDetailId,
                                        ReturnQty,
                                        ReturnAmount,
                                        CustReturnId
                                    )
                                    VALUES
                                    (   @InvoiceDetailId,
                                        @ReturnQty,
                                        @ReturnAmount,
                                        @CustReturnId
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }

        public DataTable LoadInvoiceNew(string delinvno)
        {
            string query = @"SELECT UNT.ComUnitName,ROUND(DeliveryNetAmount/Quantity, 4) AS UnitPrice,*,(DeliveryTotalQuantity-ISNULL(tbltemp.Qty,0))AvialableQty,PackSize,NetAmount,tblInvoice.InvoiceId FROM dbo.tblInvoice
            LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
            LEFT JOIN tblCompanyUnit AS UNT ON tblInvoice.ComUnitId = UNT.ComUnitId
            LEFT JOIN tblOrder AS ODR ON tblInvoice.OrderId = ODR.OrderId
            LEFT JOIN (SELECT InvoiceDetailId,ISNULL(SUM(ReturnQty),0)Qty FROM dbo.tblCustomerReturnDetail GROUP BY InvoiceDetailId)AS  tbltemp
            ON tbltemp.InvoiceDetailId = tblInvoiceDetail.InvoiceDetailId
            LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount)Amnt FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp2 ON tbltemp2.InvoiceId = tblInvoice.InvoiceId
            WHERE DelivaryInvoiceNo='" + delinvno + "' AND ISNULL(ODR.IsDirect,0) = 0 ";

            //AND tblInvoice.InvoiceId NOT IN(SELECT DISTINCT A.InvoiceId FROM dbo.tblCustomerReturnMaster A)

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoiceNewConsumer(string delinvno)
        {
            string query = @"SELECT  *,(TotalQuantity-ISNULL(tbltemp.Qty,0))AvialableQty,PackSize,NetAmount,tblInvoice.InvoiceId FROM dbo.tblInvoice
            LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
            LEFT JOIN tblOrder AS ODR ON tblInvoice.OrderId = ODR.OrderId
            LEFT JOIN (SELECT InvoiceDetailId,ISNULL(SUM(ReturnQty),0)Qty FROM dbo.tblCustomerReturnDetail GROUP BY InvoiceDetailId)AS  tbltemp
            ON tbltemp.InvoiceDetailId = tblInvoiceDetail.InvoiceDetailId
            LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount)Amnt FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp2 ON tbltemp2.InvoiceId = tblInvoice.InvoiceId
             WHERE DelivaryInvoiceNo='" + delinvno + "' AND ODR.IsDirect=1";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool ApprovalUpdateDAL(CustomerReturnMasterDAO aCustomerReturnMasterDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CustReturnId", aCustomerReturnMasterDao.CustReturnId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aCustomerReturnMasterDao.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aCustomerReturnMasterDao.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aCustomerReturnMasterDao.ApprovedDate));

            string query = @"UPDATE tblCustomerReturnMaster SET ActionStatus=@ActionStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE CustReturnId=@CustReturnId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        } 
        public bool UpdateDCStoreQty(string dcstoreId,int qty)
        {
            string query = @"UPDATE dbo.tblDCStore SET StockQty=StockQty+" + qty + " WHERE DCStoreId='" + dcstoreId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public DataTable LoadCustomerReturnApproval(string actionstatus)
        {
            string query = @"SELECT InvoiceNo,TotalQty,TotalAmount,EntryBy,EntryDate,* FROM dbo.tblCustomerReturnMaster
LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = tblCustomerReturnMaster.InvoiceId
LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
            WHERE ActionStatus='" +actionstatus+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoiceDataApproval(string id)
        {
            string query = @"SELECT * FROM dbo.tblCustomerReturnMaster
            LEFT JOIN dbo.tblCustomerReturnDetail ON tblCustomerReturnDetail.CustReturnId = tblCustomerReturnMaster.CustReturnId
            LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceDetailId = tblCustomerReturnDetail.InvoiceDetailId
            WHERE tblCustomerReturnMaster.CustReturnId='" + id + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool PostDeliverySalesReturn(int custReturnId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@ReturnId", custReturnId));

            return aCommonInternalDal.UpdateAction("sp_DeliverySalesReturnPosting", aSqlParameterList);
        }

        public DataTable LoadCustomerInfoByCode(string custCode)
        {
            string query = @"SELECT 
                tblCustMaster.CustomerMasterId,
                tblCustMaster.CustomerName,
                tblCustMaster.Address,
                tblCustMaster.CustomerCode,
                tblArea.AreaName,
                tblEmpGeneralInfo.EmpMasterCode,
                tblEmpGeneralInfo.EmpName,
                tblCustCategory.CategoryName,
                tblMarket.MarketName,
                tblMIOInfo.MIOId
            FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId
            LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId
            WHERE tblCustMaster.CustomerCode='" + custCode + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetCompanyUnitById(string comUnitId)
        {
            string query = "SELECT ComUnitCode, ComUnitName FROM tblCompanyUnit WHERE ComUnitId='" + comUnitId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetDANameById(string daId)
        {
            string query = "SELECT Name FROM tblDAInfo WHERE DAId='" + daId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetOrderSenderNameByOrderId(string orderId)
        {
            string query = "SELECT OrderSenderName FROM tblOrder WHERE OrderId='" + orderId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        // Race-safe gate: takes a U-lock on the invoice row for the duration of the ambient
        // transaction, so two concurrent returns against the same invoice serialize here.
        public bool LockInvoiceForReturn(int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));
            string query = "SELECT InvoiceId FROM dbo.tblInvoice WITH (UPDLOCK, ROWLOCK) WHERE InvoiceId = @InvoiceId";
            return aCommonInternalDal.DataContainerDataTable(query, aSqlParameterList, "SSIDB").Rows.Count > 0;
        }

        // True if any line of this invoice now has more returned than was delivered.
        // Called inside the save transaction, after the detail inserts.
        public bool HasOverReturn(int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));
            string query = @"SELECT COUNT(*) AS Cnt
                FROM dbo.tblInvoiceDetail D
                LEFT JOIN (SELECT InvoiceDetailId, ISNULL(SUM(ReturnQty),0) Qty
                           FROM dbo.tblCustomerReturnDetail GROUP BY InvoiceDetailId) R
                       ON R.InvoiceDetailId = D.InvoiceDetailId
                WHERE D.InvoiceId = @InvoiceId AND ISNULL(R.Qty,0) > ISNULL(D.DeliveryTotalQuantity,0)";
            System.Data.DataTable dt = aCommonInternalDal.DataContainerDataTable(query, aSqlParameterList, "SSIDB");
            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["Cnt"]) > 0;
        }

        // sp_DeliverySalesReturnPosting has SET NOCOUNT ON, so PostDeliverySalesReturn's bool is
        // false even on success. This mirrors the SP's own logic instead: TotalAmount <= 0 is the
        // SP's designed early-RETURN no-op (OK); otherwise IsPosting=1 (the SP's last statement,
        // set only after all voucher inserts) proves the posting ran to completion.
        public bool IsDeliverySalesReturnPosted(int custReturnId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CustReturnId", custReturnId));
            string query = @"SELECT CASE WHEN ISNULL(TotalAmount,0) <= 0 THEN 1
                                         ELSE CAST(ISNULL(IsPosting,0) AS INT) END AS Posted
                             FROM dbo.tblCustomerReturnMaster WHERE CustReturnId = @CustReturnId";
            System.Data.DataTable dt = aCommonInternalDal.DataContainerDataTable(query, aSqlParameterList, "SSIDB");
            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["Posted"]) == 1;
        }

        public bool InsertDCStoreStockReturn(int invoiceDetailId, int returnQty, int salesReturnDetailId = 0)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceDetailId", invoiceDetailId));
            aSqlParameterList.Add(new SqlParameter("@ReturnQty", returnQty));
            aSqlParameterList.Add(new SqlParameter("@SalesReturnDetailId", salesReturnDetailId));

            string query = @"
            INSERT INTO dbo.tblDCStore 
            (
                DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
                TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
                ComUnitId, StockQty, DamageQty, StockRcvDate, ReqId, ReqChildId,
                StockCondition, StockInTransfarId,
                Trans_Type, CostPerUnit, TotalCostAmount,
                ReferenceDCStoreId, ReferenceTransactionDate,
                SalesReturnDetailId, ReturnFromInvoiceDetailId
            )
            SELECT
                (SELECT ISNULL(MAX(DCStoreId), 0) + 1 FROM tblDCStore),
                StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
                0,
                ExpDate, MfgDate, GETDATE(), ChalanNo, ChalanDate,
                ComUnitId, @ReturnQty,
                0, GETDATE(), ReqId, ReqChildId,
                'SalesReturn', StockInTransfarId,
                'Receive',
                CostPerUnit,
                @ReturnQty * CostPerUnit,
                DCStoreId,
                GETDATE(),
                @SalesReturnDetailId,
                @InvoiceDetailId
            FROM tblDCStore
            WHERE DCStoreId = (SELECT DCStoreId FROM tblInvoiceDetail WHERE InvoiceDetailId = @InvoiceDetailId)
              AND Trans_Type = 'Receive'";

            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterList, "SSIDB");
        }
    }
}
