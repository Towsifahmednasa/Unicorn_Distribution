using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    // Invoice Adjustment (Bad Debts / Promotional Expense).
    // All writes go through the EntLib helper so they enlist in the page's TransactionScope.
    public class InvoiceAdjustmentDAL
    {
        private readonly ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetInvoiceHeader(string invoiceNo, int comUnitId)
        {
            string query = @"SELECT TOP 1 I.InvoiceId, I.InvoiceNo, I.DelivaryInvoiceNo, I.InvoiceDate, I.UpdateDate AS DeliveryDate,
                                    I.CustomerMasterId, I.ComUnitId, ISNULL(C.CustomerCode, '') AS CustomerCode, ISNULL(C.CustomerName, '') AS CustomerName,
                                    (ISNULL(I.DeliveryTpGrandTotal, 0) + ISNULL(I.DeliveryCharge, 0)) AS DeliveredAmount,
                                    ISNULL(PD.Paid, 0) AS PaidAmount,
                                    ISNULL(ADJ.TotalAdjustment, 0) AS PrevAdjustment
                             FROM dbo.tblInvoice I WITH (NOLOCK)
                             LEFT JOIN dbo.tblCustMaster C WITH (NOLOCK) ON C.CustomerMasterId = I.CustomerMasterId
                             OUTER APPLY (SELECT SUM(ISNULL(PaymentAmount,0) + ISNULL(Discount,0) + ISNULL(AIT,0) + ISNULL(VATAmount,0)) AS Paid
                                          FROM dbo.tblCustPayDetail WITH (NOLOCK) WHERE InvoiceId = I.InvoiceId) PD
                             OUTER APPLY (SELECT SUM(TotalAdjustment) AS TotalAdjustment
                                          FROM dbo.tblInvoiceBadDebtsMaster WITH (NOLOCK) WHERE InvoiceId = I.InvoiceId) ADJ
                             WHERE (I.InvoiceNo = @InvoiceNo OR I.DelivaryInvoiceNo = @InvoiceNo)
                               AND (@ComUnitId = 0 OR I.ComUnitId = @ComUnitId)";
            return aCommonInternalDal.DataContainerDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@InvoiceNo", invoiceNo),
                new SqlParameter("@ComUnitId", comUnitId)
            }, "SSIDB");
        }

        // One row per invoice line, plus PrevAdjustment (sum already adjusted on that line).
        public DataTable GetInvoiceDetails(int invoiceId)
        {
            string query = @"SELECT D.InvoiceDetailId, D.ProductCode, D.ProductName, D.CostPrice, D.UnitPrice, D.UnitVatAmount,
                                    D.Quantity, D.BonusQuantity, D.TotalQuantity, D.TotalPrice, D.TotalPriceVatAmount,
                                    D.DiscountPercentage, D.DiscountAmount, D.NetAmount, D.InvoiceId, D.DCStoreId,
                                    D.DeliveryQuantity, D.DeliveryBonusQuantity, D.DeliveryTotalQuantity, D.DeliveryTotalPrice,
                                    D.DeliveryTotalPriceVatAmount, D.DeliveryDiscountPercentage,
                                    ISNULL(D.DeliveryNetAmount, 0) AS DeliveryNetAmount, ISNULL(D.DeliveryStatus, '') AS DeliveryStatus,
                                    ISNULL(ADJ.PrevAdjustment, 0) AS PrevAdjustment
                             FROM dbo.tblInvoiceDetail D WITH (NOLOCK)
                             OUTER APPLY (SELECT SUM(AdjustmentAmount) AS PrevAdjustment
                                          FROM dbo.tblInvoiceBadDebtsDetails WITH (NOLOCK) WHERE InvoiceDetailId = D.InvoiceDetailId) ADJ
                             WHERE D.InvoiceId = @InvoiceId
                             ORDER BY D.InvoiceDetailId";
            return aCommonInternalDal.DataContainerDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@InvoiceId", invoiceId)
            }, "SSIDB");
        }

        // List page: one row per adjustment with the posted voucher code (if any).
        public DataTable GetAdjustmentList(System.DateTime fromDate, System.DateTime toDate)
        {
            string query = @"SELECT M.BadDebtsId, M.AdjustmentDate, M.AdjustmentType, M.InvoiceNo, I.DelivaryInvoiceNo,
                                    ISNULL(C.CustomerCode, '') AS CustomerCode, ISNULL(C.CustomerName, '') AS CustomerName,
                                    ISNULL(U.ComUnitName, '') AS ComUnitName, M.TotalAdjustment, M.Remarks, M.EntryBy, M.EntryDate,
                                    ISNULL(V.VoucherCode, '') AS VoucherCode
                             FROM dbo.tblInvoiceBadDebtsMaster M WITH (NOLOCK)
                             LEFT JOIN dbo.tblInvoice I WITH (NOLOCK) ON I.InvoiceId = M.InvoiceId
                             LEFT JOIN dbo.tblCustMaster C WITH (NOLOCK) ON C.CustomerMasterId = M.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit U WITH (NOLOCK) ON U.ComUnitId = M.ComUnitId
                             OUTER APPLY (SELECT TOP 1 VM.VoucherCode
                                          FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster VM WITH (NOLOCK)
                                          INNER JOIN CSTL_ERPDB_Unicorn..tbl_AccIntegrationTable T ON T.IntegrationTableId = VM.TableID
                                          WHERE LTRIM(RTRIM(UPPER(T.Table_Name))) = 'TBLINVOICEBADDEBTSMASTER' AND VM.CommonPK_ID = M.BadDebtsId) V
                             WHERE CAST(M.AdjustmentDate AS DATE) BETWEEN @FromDate AND @ToDate
                             ORDER BY M.BadDebtsId DESC";
            return aCommonInternalDal.DataContainerDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date)
            }, "SSIDB");
        }

        public DataTable GetAdjustmentDetails(int badDebtsId)
        {
            string query = @"SELECT D.ProductCode, ISNULL(ID.ProductName, '') AS ProductName, D.DeliveryNetAmount, D.AdjustmentAmount
                             FROM dbo.tblInvoiceBadDebtsDetails D WITH (NOLOCK)
                             LEFT JOIN dbo.tblInvoiceDetail ID WITH (NOLOCK) ON ID.InvoiceDetailId = D.InvoiceDetailId
                             WHERE D.BadDebtsId = @BadDebtsId
                             ORDER BY D.BadDebtsDetailId";
            return aCommonInternalDal.DataContainerDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@BadDebtsId", badDebtsId)
            }, "SSIDB");
        }

        // Returns the new BadDebtsId (0 on failure).
        public int SaveMaster(InvoiceAdjustment m)
        {
            string query = @"INSERT INTO dbo.tblInvoiceBadDebtsMaster
                                (InvoiceId, InvoiceNo, CustomerMasterId, ComUnitId, AdjustmentType, AdjustmentDate, TotalAdjustment, Remarks, EntryBy, EntryDate)
                             VALUES (@InvoiceId, @InvoiceNo, @CustomerMasterId, @ComUnitId, @AdjustmentType, @AdjustmentDate, @TotalAdjustment, @Remarks, @EntryBy, GETDATE())";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(query, new List<SqlParameter>
            {
                new SqlParameter("@InvoiceId", m.InvoiceId),
                new SqlParameter("@InvoiceNo", (object)m.InvoiceNo ?? System.DBNull.Value),
                new SqlParameter("@CustomerMasterId", m.CustomerMasterId),
                new SqlParameter("@ComUnitId", m.ComUnitId),
                new SqlParameter("@AdjustmentType", m.AdjustmentType),
                new SqlParameter("@AdjustmentDate", m.AdjustmentDate),
                new SqlParameter("@TotalAdjustment", m.TotalAdjustment),
                new SqlParameter("@Remarks", (object)m.Remarks ?? System.DBNull.Value),
                new SqlParameter("@EntryBy", (object)m.EntryBy ?? System.DBNull.Value)
            }, "SSIDB");
        }

        public bool SaveDetail(int badDebtsId, int invoiceId, InvoiceAdjustmentDetail d)
        {
            string query = @"INSERT INTO dbo.tblInvoiceBadDebtsDetails
                                (BadDebtsId, InvoiceId, InvoiceDetailId, ProductCode, DeliveryNetAmount, AdjustmentAmount)
                             VALUES (@BadDebtsId, @InvoiceId, @InvoiceDetailId, @ProductCode, @DeliveryNetAmount, @AdjustmentAmount)";
            return aCommonInternalDal.SaveDataByInsertCommand(query, new List<SqlParameter>
            {
                new SqlParameter("@BadDebtsId", badDebtsId),
                new SqlParameter("@InvoiceId", invoiceId),
                new SqlParameter("@InvoiceDetailId", d.InvoiceDetailId),
                new SqlParameter("@ProductCode", (object)d.ProductCode ?? System.DBNull.Value),
                new SqlParameter("@DeliveryNetAmount", d.DeliveryNetAmount),
                new SqlParameter("@AdjustmentAmount", d.AdjustmentAmount)
            }, "SSIDB");
        }

        // sp_Sales_InvoiceAdjustmentPosting raises on error, so the caller's TransactionScope rolls back.
        public void PostJournal(int badDebtsId)
        {
            aCommonInternalDal.ExecuteStoredProcedure("sp_Sales_InvoiceAdjustmentPosting", new List<SqlParameter>
            {
                new SqlParameter("@BadDebtsId", badDebtsId)
            });
        }

        public bool IsJournalPosted(int badDebtsId)
        {
            string query = @"SELECT COUNT(*) FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster V
                             INNER JOIN CSTL_ERPDB_Unicorn..tbl_AccIntegrationTable T ON T.IntegrationTableId = V.TableID
                             WHERE LTRIM(RTRIM(UPPER(T.Table_Name))) = 'TBLINVOICEBADDEBTSMASTER' AND V.CommonPK_ID = @BadDebtsId";
            DataTable dt = aCommonInternalDal.DataContainerDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@BadDebtsId", badDebtsId)
            }, "SSIDB");
            return dt.Rows.Count > 0 && System.Convert.ToInt32(dt.Rows[0][0]) > 0;
        }
    }
}
