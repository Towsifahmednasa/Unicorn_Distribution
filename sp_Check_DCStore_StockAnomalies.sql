-- =========================================================================================
-- Stored Procedure: [dbo].[sp_Check_DCStore_StockAnomalies]
-- Description: Live ডাটাবেজে যেসব Issue লাইনের Invoice ডিলিট/রিমুভ হয়ে গেছে (Orphan Issues) 
--              এবং যেসব Receive লাইনে রোলব্যাকের কারণে অতিরিক্ত স্টক ঢুকেছে তা চেক করার SP।
-- =========================================================================================

IF OBJECT_ID('dbo.sp_Check_DCStore_StockAnomalies', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Check_DCStore_StockAnomalies;
GO

CREATE PROCEDURE [dbo].[sp_Check_DCStore_StockAnomalies]
    @ProductCode NVARCHAR(100) = NULL  -- নির্দিষ্ট প্রোডাক্ট ফিল্টার করতে চাইলে (ঐচ্ছিক)
AS
BEGIN
    SET NOCOUNT ON;

    -- =========================================================================
    -- সামারি ১: মোট অসঙ্গতির পরিসংখ্যান (Summary Overview)
    -- =========================================================================
    SELECT 
        (SELECT COUNT(*) FROM dbo.tblDCStore WHERE Trans_Type = 'Issue' AND (@ProductCode IS NULL OR ProductCode = @ProductCode)) AS Total_Issue_Rows,
        
        (SELECT COUNT(*) FROM dbo.tblDCStore DCS 
         WHERE DCS.Trans_Type = 'Issue' 
           AND (@ProductCode IS NULL OR DCS.ProductCode = @ProductCode)
           AND NOT EXISTS (
               SELECT 1 FROM dbo.tblInvoiceDetail ID 
               WHERE ID.DCStoreId = DCS.ReferenceDCStoreId 
                 AND ID.TotalQuantity = ABS(DCS.StockQty)
           )) AS Orphan_Issue_Rows_Invoice_Removed,

        (SELECT COUNT(*) FROM dbo.tblDCStore DCS 
         WHERE DCS.Trans_Type = 'Issue' 
           AND (@ProductCode IS NULL OR DCS.ProductCode = @ProductCode)
           AND EXISTS (
               SELECT 1 FROM dbo.tblInvoiceDetail ID 
               WHERE ID.DCStoreId = DCS.ReferenceDCStoreId 
                 AND ID.TotalQuantity = ABS(DCS.StockQty)
           )) AS Active_Issue_Rows_With_Valid_Invoice,

        (SELECT COUNT(*) FROM dbo.tblDCStore 
         WHERE Trans_Type = 'Receive' 
           AND StockQty > TotalQuantity
           AND (@ProductCode IS NULL OR ProductCode = @ProductCode)) AS Corrupted_Receive_Rows_Inflated_Stock;

    -- =========================================================================
    -- রিপোর্ট ২: যে Issue লাইনগুলোর Invoice ডিলিট হয়ে গেছে (Orphan Issue Lines)
    -- =========================================================================
    SELECT 
        DCS.DCStoreId AS [DCStore_Issue_Id],
        DCS.ReferenceDCStoreId AS [Original_Receive_DCStoreId],
        RTRIM(DCS.ProductCode) AS [ProductCode],
        RTRIM(DCS.ProductName) AS [ProductName],
        RTRIM(DCS.BatchNo) AS [BatchNo],
        DCS.StockQty AS [Orphan_Issue_Qty],
        DCS.ReferenceTransactionDate AS [Issue_Date],
        'Invoice Removed / Not Found in Active Invoices' AS [Anomalies_Reason],
        -- যদি ডিলিট রেকর্ডে কোনো ইনভয়েস আইডি পাওয়া যায়
        (SELECT TOP 1 DR.InvoiceId 
         FROM dbo.tblInvoiceDetail_DeleterRecord DR 
         WHERE DR.DCStoreId = DCS.ReferenceDCStoreId 
           AND DR.TotalQuantity = ABS(DCS.StockQty)
         ORDER BY DR.InvoiceDetailId DESC) AS [Possible_Deleted_InvoiceId]
    FROM dbo.tblDCStore DCS
    WHERE DCS.Trans_Type = 'Issue'
      AND (@ProductCode IS NULL OR DCS.ProductCode = @ProductCode)
      AND NOT EXISTS (
          SELECT 1 FROM dbo.tblInvoiceDetail ID 
          WHERE ID.DCStoreId = DCS.ReferenceDCStoreId 
            AND ID.TotalQuantity = ABS(DCS.StockQty)
      )
    ORDER BY DCS.DCStoreId DESC;

    -- =========================================================================
    -- রিপোর্ট ৩: যে Receive রো-গুলোতে রোলব্যাকের কারণে ভুলবশত স্টক বেড়ে গেছে
    -- =========================================================================
    SELECT 
        DCStoreId AS [Receive_DCStoreId],
        RTRIM(ProductCode) AS [ProductCode],
        RTRIM(ProductName) AS [ProductName],
        RTRIM(BatchNo) AS [BatchNo],
        ChalanNo,
        TotalQuantity AS [Actual_Received_Qty],
        StockQty AS [Corrupted_StockQty],
        (StockQty - TotalQuantity) AS [Excess_Inflated_Stock],
        ReceiveDate
    FROM dbo.tblDCStore
    WHERE Trans_Type = 'Receive'
      AND StockQty > TotalQuantity
      AND (@ProductCode IS NULL OR ProductCode = @ProductCode)
    ORDER BY DCStoreId DESC;

END;
GO
