using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Library.DAL.DataManager;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class SalesOperationRollbackDAL
    {
        private string GetConnectionString()
        {
            return "Data Source=" + SqlUserAccess.DataSource +
                   ";Initial Catalog=" + DataBase.SalesDB +
                   ";Integrated Security=false;User ID=" + SqlUserAccess.UserName +
                   ";Password=" + SqlUserAccess.PassWord +
                   ";Encrypt=True;TrustServerCertificate=True;";
        }

        public string RollbackProforma(string proformaCode, string loginUser, string remarks)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Get Invoice ID, Order ID, Delivery Invoice No, and ComUnitId
                    string queryInv = "SELECT InvoiceId, OrderId, DelivaryInvoiceNo, ComUnitId FROM tblInvoice WHERE InvoiceNo = @InvoiceNo";
                    SqlCommand cmdInv = new SqlCommand(queryInv, conn, trans);
                    cmdInv.Parameters.AddWithValue("@InvoiceNo", proformaCode);
                    int invoiceId = 0;
                    int orderId = 0;
                    int comUnitId = 0;
                    string deliveryInvoiceNo = "";
                    using (SqlDataReader reader = cmdInv.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            invoiceId = Convert.ToInt32(reader["InvoiceId"]);
                            orderId = reader["OrderId"] != DBNull.Value ? Convert.ToInt32(reader["OrderId"]) : 0;
                            deliveryInvoiceNo = reader["DelivaryInvoiceNo"] != DBNull.Value ? reader["DelivaryInvoiceNo"].ToString() : "";
                            comUnitId = reader["ComUnitId"] != DBNull.Value ? Convert.ToInt32(reader["ComUnitId"]) : 0;
                        }
                    }

                    if (invoiceId == 0)
                    {
                        throw new Exception("Proforma Invoice not found!");
                    }

                    // 2. Validate that no Delivery Invoice exists
                    if (!string.IsNullOrEmpty(deliveryInvoiceNo))
                    {
                        throw new Exception("Delivery Invoice already created! Please delete the Delivery Invoice first.");
                    }

                    // 3. Write Log before deleting
                    string queryLogMaster = @"
                        INSERT INTO tblInvoiceDeleteLog (
                            InvoiceId, InvoiceNo, DelInvoiceNo, DeleteDateTime, DeleteBy
                        )
                        SELECT 
                            InvoiceId, InvoiceNo, 'PROFORMA', GETDATE(), @User
                        FROM tblInvoice WHERE InvoiceId = @InvoiceId
                    ";
                    SqlCommand cmdLogMaster = new SqlCommand(queryLogMaster, conn, trans);
                    cmdLogMaster.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdLogMaster.Parameters.AddWithValue("@User", loginUser);
                    cmdLogMaster.ExecuteNonQuery();

                    // 4. Update tblOrder: Set IsInvoice = 0 and reset tblOrderDetail Status
                    if (orderId > 0)
                    {
                        string queryOrder = "UPDATE tblOrder SET IsInvoice = 0 WHERE OrderId = @OrderId";
                        SqlCommand cmdOrder = new SqlCommand(queryOrder, conn, trans);
                        cmdOrder.Parameters.AddWithValue("@OrderId", orderId);
                        cmdOrder.ExecuteNonQuery();

                        string queryOrderDtl = "UPDATE tblOrderDetail SET Status = NULL WHERE OrderId = @OrderId";
                        SqlCommand cmdOrderDtl = new SqlCommand(queryOrderDtl, conn, trans);
                        cmdOrderDtl.Parameters.AddWithValue("@OrderId", orderId);
                        cmdOrderDtl.ExecuteNonQuery();
                    }

                    // 5. Return stock: Delete the negative Issue rows created by the invoice in tblDCStore
                    string queryItems = "SELECT InvoiceDetailId, ProductCode, BatchNo, ExpDate, Quantity, DCStoreId FROM tblInvoiceDetail WHERE InvoiceId = @InvoiceId";
                    SqlCommand cmdItems = new SqlCommand(queryItems, conn, trans);
                    cmdItems.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    DataTable dtItems = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdItems))
                    {
                        adapter.Fill(dtItems);
                    }

                    foreach (DataRow row in dtItems.Rows)
                    {
                        int invoiceDetailId = Convert.ToInt32(row["InvoiceDetailId"]);
                        string productCode = row["ProductCode"].ToString();
                        string batchNo = row["BatchNo"].ToString();
                        DateTime expDate = Convert.ToDateTime(row["ExpDate"]);
                        decimal qty = Convert.ToDecimal(row["Quantity"]);
                        object dcStoreIdVal = row["DCStoreId"];
                        int dcStoreId = dcStoreIdVal != DBNull.Value ? Convert.ToInt32(dcStoreIdVal) : 0;

                        if (qty > 0)
                        {
                            bool updated = false;

                            // Rule 1: Delete the matching negative Issue row linked via ReferenceDCStoreId
                            if (dcStoreId > 0)
                            {
                                string queryDelIssue = @"
                                    DELETE TOP (1) FROM dbo.tblDCStore 
                                    WHERE ReferenceDCStoreId = @DCStoreId 
                                      AND Trans_Type = 'Issue'
                                      AND StockQty = (@Qty * -1);";
                                SqlCommand cmdDelIssue = new SqlCommand(queryDelIssue, conn, trans);
                                cmdDelIssue.Parameters.AddWithValue("@DCStoreId", dcStoreId);
                                cmdDelIssue.Parameters.AddWithValue("@Qty", qty);
                                int rowsDel = cmdDelIssue.ExecuteNonQuery();

                                if (rowsDel > 0)
                                {
                                    updated = true;

                                    // Safety cap: ensure Receive row doesn't exceed TotalQuantity
                                    string queryCap = @"
                                        UPDATE dbo.tblDCStore
                                        SET StockQty = TotalQuantity
                                        WHERE DCStoreId = @DCStoreId
                                          AND Trans_Type = 'Receive'
                                          AND StockQty > TotalQuantity;";
                                    SqlCommand cmdCap = new SqlCommand(queryCap, conn, trans);
                                    cmdCap.Parameters.AddWithValue("@DCStoreId", dcStoreId);
                                    cmdCap.ExecuteNonQuery();
                                }
                                else
                                {
                                    // Check if the dcStoreId itself was an Issue row
                                    string queryTrans = "SELECT Trans_Type FROM tblDCStore WHERE DCStoreId = @DCStoreId";
                                    SqlCommand cmdTrans = new SqlCommand(queryTrans, conn, trans);
                                    cmdTrans.Parameters.AddWithValue("@DCStoreId", dcStoreId);
                                    object transTypeVal = cmdTrans.ExecuteScalar();
                                    if (transTypeVal != null && transTypeVal.ToString() == "Issue")
                                    {
                                        string queryDelSelf = "DELETE FROM tblDCStore WHERE DCStoreId = @DCStoreId";
                                        SqlCommand cmdDelSelf = new SqlCommand(queryDelSelf, conn, trans);
                                        cmdDelSelf.Parameters.AddWithValue("@DCStoreId", dcStoreId);
                                        cmdDelSelf.ExecuteNonQuery();
                                        updated = true;
                                    }
                                }
                            }

                            // Rule 2: Fallback to delete matching Issue row by ProductCode, Batch, ExpDate, ComUnitId
                            if (!updated)
                            {
                                string queryDelFallback = @"
                                    DELETE TOP (1) FROM dbo.tblDCStore 
                                    WHERE ProductCode = @ProductCode 
                                      AND BatchNo = @BatchNo 
                                      AND ExpDate = @ExpDate 
                                      AND ComUnitId = @ComUnitId 
                                      AND Trans_Type = 'Issue'
                                      AND StockQty = (@Qty * -1);";
                                SqlCommand cmdDelFb = new SqlCommand(queryDelFallback, conn, trans);
                                cmdDelFb.Parameters.AddWithValue("@ProductCode", productCode);
                                cmdDelFb.Parameters.AddWithValue("@BatchNo", batchNo);
                                cmdDelFb.Parameters.AddWithValue("@ExpDate", expDate);
                                cmdDelFb.Parameters.AddWithValue("@ComUnitId", comUnitId);
                                cmdDelFb.Parameters.AddWithValue("@Qty", qty);
                                int fbDel = cmdDelFb.ExecuteNonQuery();
                                if (fbDel > 0)
                                {
                                    updated = true;
                                }
                            }

                            // Rule 3: Last-resort fallback for legacy data where no Issue row was ever created
                            if (!updated && dcStoreId > 0)
                            {
                                string queryLegacy = @"
                                    UPDATE dbo.tblDCStore 
                                    SET StockQty = CASE WHEN StockQty + @Qty > TotalQuantity THEN TotalQuantity ELSE StockQty + @Qty END
                                    WHERE DCStoreId = @DCStoreId 
                                      AND Trans_Type = 'Receive';";
                                SqlCommand cmdLegacy = new SqlCommand(queryLegacy, conn, trans);
                                cmdLegacy.Parameters.AddWithValue("@DCStoreId", dcStoreId);
                                cmdLegacy.Parameters.AddWithValue("@Qty", qty);
                                cmdLegacy.ExecuteNonQuery();
                            }

                            // Delete any freeze records for this item
                            string queryDelFreeze = "DELETE FROM tblDCStoreFreeze WHERE InvoiceDetailId = @InvoiceDetailId";
                            SqlCommand cmdDelFreeze = new SqlCommand(queryDelFreeze, conn, trans);
                            cmdDelFreeze.Parameters.AddWithValue("@InvoiceDetailId", invoiceDetailId);
                            cmdDelFreeze.ExecuteNonQuery();
                        }
                    }

                    // 6. Delete tblReturnAmount if any
                    string queryDelReturnAmt = "DELETE FROM tblReturnAmount WHERE InvoiceId = @InvoiceId";
                    SqlCommand cmdDelReturnAmt = new SqlCommand(queryDelReturnAmt, conn, trans);
                    cmdDelReturnAmt.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdDelReturnAmt.ExecuteNonQuery();

                    // 7. Delete tblInvoice and tblInvoiceDetail
                    string queryDelDetail = "DELETE FROM tblInvoiceDetail WHERE InvoiceId = @InvoiceId";
                    SqlCommand cmdDelDetail = new SqlCommand(queryDelDetail, conn, trans);
                    cmdDelDetail.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdDelDetail.ExecuteNonQuery();

                    string queryDelInvoice = "DELETE FROM tblInvoice WHERE InvoiceId = @InvoiceId";
                    SqlCommand cmdDelInvoice = new SqlCommand(queryDelInvoice, conn, trans);
                    cmdDelInvoice.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdDelInvoice.ExecuteNonQuery();

                    trans.Commit();
                    return "SUCCESS";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "Error: " + ex.Message;
                }
            }
        }

        public string RollbackDeliveryInvoice(string deliveryInvoiceNo, string loginUser, string remarks)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Get Invoice details
                    string queryInv = "SELECT InvoiceId, OrderNo, CustomerMasterId FROM tblInvoice WHERE DelivaryInvoiceNo = @DelivaryInvoiceNo";
                    SqlCommand cmdInv = new SqlCommand(queryInv, conn, trans);
                    cmdInv.Parameters.AddWithValue("@DelivaryInvoiceNo", deliveryInvoiceNo);
                    int invoiceId = 0;
                    string orderNo = "";
                    int customerMasterId = 0;
                    using (SqlDataReader reader = cmdInv.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            invoiceId = Convert.ToInt32(reader["InvoiceId"]);
                            orderNo = reader["OrderNo"].ToString();
                            customerMasterId = Convert.ToInt32(reader["CustomerMasterId"]);
                        }
                    }

                    if (invoiceId == 0)
                    {
                        throw new Exception("Delivery Invoice not found!");
                    }

                    // 2. Validate Payment
                    string queryPay = "SELECT COUNT(*) FROM tblCustPayDetail WHERE InvoiceId = @InvoiceId";
                    SqlCommand cmdPay = new SqlCommand(queryPay, conn, trans);
                    cmdPay.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    int paymentCount = Convert.ToInt32(cmdPay.ExecuteScalar());
                    if (paymentCount > 0)
                    {
                        throw new Exception("Payment has been settled for this Delivery Invoice. Please rollback the payment first!");
                    }

                    // 3. Load Invoice Items for Stock Adjustment and Availability Validation
                    string queryItems = @"SELECT ProductCode, BatchNo, ExpDate, Quantity, DeliveryQuantity, DCStoreId, InvoiceDetailId
                                          FROM tblInvoiceDetail WHERE InvoiceId = @InvoiceId";
                    SqlCommand cmdItems = new SqlCommand(queryItems, conn, trans);
                    cmdItems.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    DataTable dtItems = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdItems))
                    {
                        adapter.Fill(dtItems);
                    }

                    // 4. Validate Stock Availability Batch/Exp Date wise
                    foreach (DataRow row in dtItems.Rows)
                    {
                        string productCode = row["ProductCode"].ToString();
                        string batchNo = row["BatchNo"].ToString();
                        DateTime expDate = Convert.ToDateTime(row["ExpDate"]);
                        decimal qty = Convert.ToDecimal(row["Quantity"]);
                        decimal delQty = Convert.ToDecimal(row["DeliveryQuantity"]);
                        decimal returnQty = qty - delQty;

                        if (returnQty > 0)
                        {
                            // Check stock in tblDCStore
                            string queryStock = @"SELECT SUM(StockQty) FROM tblDCStore 
                                                  WHERE ProductCode = @ProductCode AND BatchNo = @BatchNo AND ExpDate = @ExpDate";
                            SqlCommand cmdStock = new SqlCommand(queryStock, conn, trans);
                            cmdStock.Parameters.AddWithValue("@ProductCode", productCode);
                            cmdStock.Parameters.AddWithValue("@BatchNo", batchNo);
                            cmdStock.Parameters.AddWithValue("@ExpDate", expDate);
                            
                            object stockVal = cmdStock.ExecuteScalar();
                            decimal availableStock = stockVal != DBNull.Value ? Convert.ToDecimal(stockVal) : 0;

                            if (availableStock < returnQty)
                            {
                                throw new Exception(string.Format("Sufficient stock not available for Product {0}, Batch {1}, Exp Date {2:dd-MMM-yyyy}. Required: {3}, Available: {4}", 
                                    productCode, batchNo, expDate, returnQty, availableStock));
                            }
                        }
                    }

                    // 5. Deduct Return Qty from Stock (Stock Adjustment) using System's sp_DeleteDeliveryInvoice exact logic
                    foreach (DataRow row in dtItems.Rows)
                    {
                        string productCode = row["ProductCode"].ToString();
                        string batchNo = row["BatchNo"].ToString();
                        DateTime expDate = Convert.ToDateTime(row["ExpDate"]);
                        decimal qty = Convert.ToDecimal(row["Quantity"]);
                        decimal delQty = Convert.ToDecimal(row["DeliveryQuantity"]);
                        decimal returnQty = qty - delQty;
                        object dcStoreIdVal = row["DCStoreId"];
                        int dcStoreId = dcStoreIdVal != DBNull.Value ? Convert.ToInt32(dcStoreIdVal) : 0;
                        int invoiceDetailId = Convert.ToInt32(row["InvoiceDetailId"]);

                        if (returnQty > 0 && dcStoreId > 0)
                        {
                            // Subtract the return quantity from the linked DCStoreId (Original Receive row or Issue Row)
                            // Following sp_DeleteDeliveryInvoice style:
                            string queryDeduct = @"
                                INSERT INTO dbo.tblDCStore 
                                (
                                    DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
                                    TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
                                    ComUnitId, StockQty, DamageQty, StockRcvDate, ReqId, ReqChildId,
                                    StockCondition, StockInTransfarId,
                                    Trans_Type, CostPerUnit, TotalCostAmount,
                                    ReferenceDCStoreId, ReferenceTransactionDate
                                )
                                SELECT
                                    (SELECT ISNULL(MAX(DCStoreId), 0) + 1 FROM tblDCStore),
                                    StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
                                    0,
                                    ExpDate, MfgDate, GETDATE(), ChalanNo, ChalanDate,
                                    ComUnitId, -@ReturnQty,
                                    0, GETDATE(), ReqId, ReqChildId,
                                    'Issue', StockInTransfarId,
                                    'Issue',
                                    CostPerUnit,
                                    -@ReturnQty * CostPerUnit,
                                    CASE WHEN Trans_Type = 'Receive' THEN DCStoreId ELSE ReferenceDCStoreId END,
                                    GETDATE()
                                FROM tblDCStore
                                WHERE DCStoreId = @DCStoreId;

                                -- Delete freeze rows if any
                                DELETE FROM tblDCStoreFreeze WHERE InvoiceDetailId = @InvoiceDetailId;
                            ";
                            SqlCommand cmdDeduct = new SqlCommand(queryDeduct, conn, trans);
                            cmdDeduct.Parameters.AddWithValue("@ReturnQty", returnQty);
                            cmdDeduct.Parameters.AddWithValue("@DCStoreId", dcStoreId);
                            cmdDeduct.Parameters.AddWithValue("@InvoiceDetailId", invoiceDetailId);
                            cmdDeduct.ExecuteNonQuery();
                        }
                    }

                    // 6. Reverse Journal Posting for the Delivery Invoice (TableID for Invoice = 2014)
                    string queryVouchers = "SELECT VoucherMasterId, VoucherCode FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster WHERE CommonPK_ID = @InvoiceId AND TableID = 2014";
                    SqlCommand cmdVouchers = new SqlCommand(queryVouchers, conn, trans);
                    cmdVouchers.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    DataTable dtVouchers = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdVouchers))
                    {
                        adapter.Fill(dtVouchers);
                    }

                    foreach (DataRow vRow in dtVouchers.Rows)
                    {
                        int origMasterId = Convert.ToInt32(vRow["VoucherMasterId"]);
                        string origVoucherCode = vRow["VoucherCode"].ToString();

                        // Insert Master Reversal using Standard Voucher Sequence Generator
                        string queryInsMaster = @"
                            DECLARE @OrigMasterId INT = @OrigId;
                            DECLARE @ApprovalDate DATETIME = GETDATE();
                            DECLARE @yearText NVARCHAR(MAX) = SUBSTRING(CONVERT(NVARCHAR(MAX), YEAR(@ApprovalDate)), 3, 3);
                            DECLARE @monthText NVARCHAR(MAX) = (CASE WHEN LEN(MONTH(@ApprovalDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX), MONTH(@ApprovalDate)) END);
                            DECLARE @dateText NVARCHAR(MAX) = (CASE WHEN LEN(DAY(@ApprovalDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX), DAY(@ApprovalDate)) END);

                            DECLARE @OrigVoucherCode NVARCHAR(MAX);
                            SELECT @OrigVoucherCode = VoucherCode FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster WHERE VoucherMasterId = @OrigMasterId;

                            DECLARE @Prefix NVARCHAR(MAX) = SUBSTRING(@OrigVoucherCode, 1, CHARINDEX('-', @OrigVoucherCode));

                            DECLARE @NewVoucherCode NVARCHAR(MAX);
                            SELECT @NewVoucherCode = @Prefix + (@yearText + @monthText + @dateText) + CONVERT(NVARCHAR(MAX), CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 6)), 10000)) + 1)  
                            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster 
                            WHERE CONVERT(DATE, PostingDate) = CONVERT(DATE, @ApprovalDate) AND VoucherCode LIKE @Prefix + '%';

                            INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster (
                                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherNo, VoucherCategoryID,
                                CurrencyId, CurrencyCovRate, Narration, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter,
                                ReceiveFromIsBuyer, ByerId, PaytoAndReceiveFromIsOther, Other, OtherAdress, PaytoIsSupplier,
                                SupplierId, IsInvoice, TotalAmount, TotalAmountBDT, Remarks, EntryBy, EntryDate, VoucherType,
                                BankCheckNo, SalesReturnId, CustPayDetailId, WHStockInMasterId, PurchaseId, SampleInvoiceId,
                                WholeSaleId, BankAccountId, ChequeNo, ChequeDate, CashAccountId, RefNo, RefDate,
                                IsReconcile, ToCompanyInfoId, ToFinancialYearId, IsAdjusted, Note, IsFormERP,
                                PurchaseOrderLocalReceiveId, InvoiceId, PaymentMasterId, CommonPK_ID, TableID,
                                SpotPurchaseId, LandedCostActualDifference, CommercialInvoiceActualDifference, IsInventoryValuation,
                                PreviousCompanyInfoId
                            )
                            SELECT 
                                @NewVoucherCode, CompanyInfoId, FinancialYearId, @ApprovalDate, VoucherNo, VoucherCategoryID,
                                CurrencyId, CurrencyCovRate, 'Reversal of ' + VoucherCode + ' : ' + ISNULL(Narration, ''), IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter,
                                ReceiveFromIsBuyer, ByerId, PaytoAndReceiveFromIsOther, Other, OtherAdress, PaytoIsSupplier,
                                SupplierId, IsInvoice, TotalAmount, TotalAmountBDT, @Remarks, @User, GETDATE(), VoucherType,
                                BankCheckNo, SalesReturnId, CustPayDetailId, WHStockInMasterId, PurchaseId, SampleInvoiceId,
                                WholeSaleId, BankAccountId, ChequeNo, ChequeDate, CashAccountId, RefNo, RefDate,
                                IsReconcile, ToCompanyInfoId, ToFinancialYearId, IsAdjusted, Note, IsFormERP,
                                PurchaseOrderLocalReceiveId, InvoiceId, PaymentMasterId, CommonPK_ID, TableID,
                                SpotPurchaseId, LandedCostActualDifference, CommercialInvoiceActualDifference, IsInventoryValuation,
                                PreviousCompanyInfoId
                            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster
                            WHERE VoucherMasterId = @OrigMasterId;

                            SELECT SCOPE_IDENTITY();
                        ";
                        SqlCommand cmdInsMaster = new SqlCommand(queryInsMaster, conn, trans);
                        cmdInsMaster.Parameters.AddWithValue("@OrigId", origMasterId);
                        cmdInsMaster.Parameters.AddWithValue("@Remarks", "Reversal of " + origVoucherCode + " : " + remarks);
                        cmdInsMaster.Parameters.AddWithValue("@User", loginUser);
                        int newMasterId = Convert.ToInt32(cmdInsMaster.ExecuteScalar());

                        // Insert Detail Reversal
                        string queryInsDetail = @"
                            INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail (
                                VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
                                VoucherCategory, CostCenterId, RefInfoInd, SubsidoryLedgerId, Remarks
                            )
                            SELECT 
                                @NewMasterId, AccountId, 
                                CASE WHEN DebitorCredit = 'Debit' THEN 'Credit' ELSE 'Debit' END,
                                CreditAmount, DebitAmount, CreditAmountBDT, DebitAmountBDT,
                                VoucherCategory, CostCenterId, RefInfoInd, SubsidoryLedgerId, 'Reversal of detail'
                            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail
                            WHERE VoucherMasterId = @OrigMasterId;
                        ";
                        SqlCommand cmdInsDetail = new SqlCommand(queryInsDetail, conn, trans);
                        cmdInsDetail.Parameters.AddWithValue("@NewMasterId", newMasterId);
                        cmdInsDetail.Parameters.AddWithValue("@OrigMasterId", origMasterId);
                        cmdInsDetail.ExecuteNonQuery();
                    }

                    // 7. Write Log to tblInvoiceDeleteLog and tblInvoiceDetail_DeleterRecord
                    string queryLogMaster = @"
                        INSERT INTO tblInvoiceDeleteLog (
                            InvoiceId, InvoiceNo, DelInvoiceNo, DeleteDateTime, DeleteBy, DeliveryDeleteBy, DeliveryDeleteDateTime
                        )
                        SELECT 
                            InvoiceId, InvoiceNo, DelivaryInvoiceNo, GETDATE(), @User, @User, CONVERT(varchar(20), GETDATE(), 120)
                        FROM tblInvoice WHERE InvoiceId = @InvoiceId
                    ";
                    SqlCommand cmdLogMaster = new SqlCommand(queryLogMaster, conn, trans);
                    cmdLogMaster.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdLogMaster.Parameters.AddWithValue("@User", loginUser);
                    cmdLogMaster.ExecuteNonQuery();

                    string queryLogDetail = @"
                        INSERT INTO tblInvoiceDetail_DeleterRecord (
                            InvoiceDetailId, InvoiceId, ProductCode, BatchNo, Quantity, DeliveryQuantity, ExpDate, ReceiveDate
                        )
                        SELECT 
                            InvoiceDetailId, InvoiceId, ProductCode, BatchNo, Quantity, DeliveryQuantity, ExpDate, GETDATE()
                        FROM tblInvoiceDetail WHERE InvoiceId = @InvoiceId
                    ";
                    SqlCommand cmdLogDetail = new SqlCommand(queryLogDetail, conn, trans);
                    cmdLogDetail.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdLogDetail.ExecuteNonQuery();

                    // 8. Delete / Reset Delivery Invoice Records
                    string queryResetInvoice = @"
                        UPDATE tblInvoice 
                        SET DelivaryInvoiceNo = NULL, 
                            DeliveryInvoiceStatus = NULL, 
                            DeliveryDate = NULL,
                            DeliveryPersonName = NULL,
                            DeliveryPersonPhNo = NULL,
                            DeliveryInvoiceCreateDate = NULL
                        WHERE InvoiceId = @InvoiceId
                    ";
                    SqlCommand cmdResetInvoice = new SqlCommand(queryResetInvoice, conn, trans);
                    cmdResetInvoice.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdResetInvoice.ExecuteNonQuery();

                    string queryResetDetail = @"
                        UPDATE tblInvoiceDetail 
                        SET DeliveryQuantity = 0,
                            DeliveryBonusQuantity = 0,
                            DeliveryTotalQuantity = 0,
                            DeliveryTotalPrice = 0,
                            DeliveryTotalPriceVatAmount = 0,
                            DeliveryDiscountPercentage = 0,
                            DeliveryDiscountAmount = 0,
                            DeliveryNetAmount = 0,
                            DeliveryStatus = NULL,
                            ReturnReason = NULL
                        WHERE InvoiceId = @InvoiceId
                    ";
                    SqlCommand cmdResetDetail = new SqlCommand(queryResetDetail, conn, trans);
                    cmdResetDetail.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmdResetDetail.ExecuteNonQuery();

                    trans.Commit();
                    return "SUCCESS";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "Error: " + ex.Message;
                }
            }
        }

        public string RollbackCustomerPayment(string moneyReceiptNo, string loginUser, string remarks)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Get Payment Master details
                    string queryPay = "SELECT CustPayId, CollectionNo FROM tblCustomerPay WHERE CollectionNo = @CollectionNo";
                    SqlCommand cmdPay = new SqlCommand(queryPay, conn, trans);
                    cmdPay.Parameters.AddWithValue("@CollectionNo", moneyReceiptNo);
                    int custPayId = 0;
                    using (SqlDataReader reader = cmdPay.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            custPayId = Convert.ToInt32(reader["CustPayId"]);
                        }
                    }

                    if (custPayId == 0)
                    {
                        throw new Exception("Customer Payment Record not found!");
                    }

                    // 2. Load Payment Details to reset invoice payment statuses
                    string queryDetails = "SELECT CustPayDetailId, InvoiceId, PaymentAmount FROM tblCustPayDetail WHERE CustPayId = @CustPayId";
                    SqlCommand cmdDetails = new SqlCommand(queryDetails, conn, trans);
                    cmdDetails.Parameters.AddWithValue("@CustPayId", custPayId);
                    DataTable dtDetails = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdDetails))
                    {
                        adapter.Fill(dtDetails);
                    }

                    // 3. Reverse Journal Posting for the payment (TableID for Customer Payment = 2017)
                    string queryVouchers = "SELECT VoucherMasterId, VoucherCode FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster WHERE CommonPK_ID = @CustPayId AND TableID = 2017";
                    SqlCommand cmdVouchers = new SqlCommand(queryVouchers, conn, trans);
                    cmdVouchers.Parameters.AddWithValue("@CustPayId", custPayId);
                    DataTable dtVouchers = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdVouchers))
                    {
                        adapter.Fill(dtVouchers);
                    }

                    foreach (DataRow vRow in dtVouchers.Rows)
                    {
                        int origMasterId = Convert.ToInt32(vRow["VoucherMasterId"]);
                        string origVoucherCode = vRow["VoucherCode"].ToString();

                        // Insert Master Reversal using Standard Voucher Sequence Generator
                        string queryInsMaster = @"
                            DECLARE @OrigMasterId INT = @OrigId;
                            DECLARE @ApprovalDate DATETIME = GETDATE();
                            DECLARE @yearText NVARCHAR(MAX) = SUBSTRING(CONVERT(NVARCHAR(MAX), YEAR(@ApprovalDate)), 3, 3);
                            DECLARE @monthText NVARCHAR(MAX) = (CASE WHEN LEN(MONTH(@ApprovalDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX), MONTH(@ApprovalDate)) END);
                            DECLARE @dateText NVARCHAR(MAX) = (CASE WHEN LEN(DAY(@ApprovalDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX), DAY(@ApprovalDate)) END);

                            DECLARE @OrigVoucherCode NVARCHAR(MAX);
                            SELECT @OrigVoucherCode = VoucherCode FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster WHERE VoucherMasterId = @OrigMasterId;

                            DECLARE @Prefix NVARCHAR(MAX) = SUBSTRING(@OrigVoucherCode, 1, CHARINDEX('-', @OrigVoucherCode));

                            DECLARE @NewVoucherCode NVARCHAR(MAX);
                            SELECT @NewVoucherCode = @Prefix + (@yearText + @monthText + @dateText) + CONVERT(NVARCHAR(MAX), CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 6)), 10000)) + 1)  
                            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster 
                            WHERE CONVERT(DATE, PostingDate) = CONVERT(DATE, @ApprovalDate) AND VoucherCode LIKE @Prefix + '%';

                            INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster (
                                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherNo, VoucherCategoryID,
                                CurrencyId, CurrencyCovRate, Narration, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter,
                                ReceiveFromIsBuyer, ByerId, PaytoAndReceiveFromIsOther, Other, OtherAdress, PaytoIsSupplier,
                                SupplierId, IsInvoice, TotalAmount, TotalAmountBDT, Remarks, EntryBy, EntryDate, VoucherType,
                                BankCheckNo, SalesReturnId, CustPayDetailId, WHStockInMasterId, PurchaseId, SampleInvoiceId,
                                WholeSaleId, BankAccountId, ChequeNo, ChequeDate, CashAccountId, RefNo, RefDate,
                                IsReconcile, ToCompanyInfoId, ToFinancialYearId, IsAdjusted, Note, IsFormERP,
                                PurchaseOrderLocalReceiveId, InvoiceId, PaymentMasterId, CommonPK_ID, TableID,
                                SpotPurchaseId, LandedCostActualDifference, CommercialInvoiceActualDifference, IsInventoryValuation,
                                PreviousCompanyInfoId
                            )
                            SELECT 
                                @NewVoucherCode, CompanyInfoId, FinancialYearId, @ApprovalDate, VoucherNo, VoucherCategoryID,
                                CurrencyId, CurrencyCovRate, 'Reversal of ' + VoucherCode + ' : ' + ISNULL(Narration, ''), IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter,
                                ReceiveFromIsBuyer, ByerId, PaytoAndReceiveFromIsOther, Other, OtherAdress, PaytoIsSupplier,
                                SupplierId, IsInvoice, TotalAmount, TotalAmountBDT, @Remarks, @User, GETDATE(), VoucherType,
                                BankCheckNo, SalesReturnId, CustPayDetailId, WHStockInMasterId, PurchaseId, SampleInvoiceId,
                                WholeSaleId, BankAccountId, ChequeNo, ChequeDate, CashAccountId, RefNo, RefDate,
                                IsReconcile, ToCompanyInfoId, ToFinancialYearId, IsAdjusted, Note, IsFormERP,
                                PurchaseOrderLocalReceiveId, InvoiceId, PaymentMasterId, CommonPK_ID, TableID,
                                SpotPurchaseId, LandedCostActualDifference, CommercialInvoiceActualDifference, IsInventoryValuation,
                                PreviousCompanyInfoId
                            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster
                            WHERE VoucherMasterId = @OrigMasterId;

                            SELECT SCOPE_IDENTITY();
                        ";
                        SqlCommand cmdInsMaster = new SqlCommand(queryInsMaster, conn, trans);
                        cmdInsMaster.Parameters.AddWithValue("@OrigId", origMasterId);
                        cmdInsMaster.Parameters.AddWithValue("@Remarks", "Reversal of " + origVoucherCode + " : " + remarks);
                        cmdInsMaster.Parameters.AddWithValue("@User", loginUser);
                        int newMasterId = Convert.ToInt32(cmdInsMaster.ExecuteScalar());

                        // Insert Detail Reversal
                        string queryInsDetail = @"
                            INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail (
                                VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
                                VoucherCategory, CostCenterId, RefInfoInd, SubsidoryLedgerId, Remarks
                            )
                            SELECT 
                                @NewMasterId, AccountId, 
                                CASE WHEN DebitorCredit = 'Debit' THEN 'Credit' ELSE 'Debit' END,
                                CreditAmount, DebitAmount, CreditAmountBDT, DebitAmountBDT,
                                VoucherCategory, CostCenterId, RefInfoInd, SubsidoryLedgerId, 'Reversal of detail'
                            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail
                            WHERE VoucherMasterId = @OrigMasterId;
                        ";
                        SqlCommand cmdInsDetail = new SqlCommand(queryInsDetail, conn, trans);
                        cmdInsDetail.Parameters.AddWithValue("@NewMasterId", newMasterId);
                        cmdInsDetail.Parameters.AddWithValue("@OrigMasterId", origMasterId);
                        cmdInsDetail.ExecuteNonQuery();
                    }

                    // 4. For each payment detail row, reset invoice payment status and log
                    foreach (DataRow row in dtDetails.Rows)
                    {
                        int custPayDetailId = Convert.ToInt32(row["CustPayDetailId"]);
                        int invoiceId = Convert.ToInt32(row["InvoiceId"]);

                        // Reset invoice payment status in tblInvoice
                        string queryResetInvoice = "UPDATE tblInvoice SET PaymentStatus = NULL, PaymentAmount = NULL WHERE InvoiceId = @InvoiceId";
                        SqlCommand cmdResetInvoice = new SqlCommand(queryResetInvoice, conn, trans);
                        cmdResetInvoice.Parameters.AddWithValue("@InvoiceId", invoiceId);
                        cmdResetInvoice.ExecuteNonQuery();

                        // Log the deletion to tblCustPayDetail_DeleteLog
                        SqlCommand cmdLog = new SqlCommand("sp_InsertCustPayDetail_DeleteLog", conn, trans);
                        cmdLog.CommandType = CommandType.StoredProcedure;
                        cmdLog.Parameters.AddWithValue("@Remarks", remarks);
                        cmdLog.Parameters.AddWithValue("@CustPayDetailId", custPayDetailId);
                        cmdLog.Parameters.AddWithValue("@LoginName", loginUser);
                        cmdLog.ExecuteNonQuery();
                    }

                    // 5. Delete details and master from tables
                    string queryDelDetail = "DELETE FROM tblCustPayDetail WHERE CustPayId = @CustPayId";
                    SqlCommand cmdDelDetail = new SqlCommand(queryDelDetail, conn, trans);
                    cmdDelDetail.Parameters.AddWithValue("@CustPayId", custPayId);
                    cmdDelDetail.ExecuteNonQuery();

                    string queryDelMaster = "DELETE FROM tblCustomerPay WHERE CustPayId = @CustPayId";
                    SqlCommand cmdDelMaster = new SqlCommand(queryDelMaster, conn, trans);
                    cmdDelMaster.Parameters.AddWithValue("@CustPayId", custPayId);
                    cmdDelMaster.ExecuteNonQuery();

                    trans.Commit();
                    return "SUCCESS";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "Error: " + ex.Message;
                }
            }
        }
    }
}
