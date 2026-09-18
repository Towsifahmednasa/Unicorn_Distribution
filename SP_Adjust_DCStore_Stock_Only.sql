-- =========================================================================================
-- Stored Procedure: [dbo].[sp_Adjust_DCStore_Stock_Only]
-- Description: শুধুমাত্র DC Store-এর স্টক অ্যাডজাস্ট (Rollback / Reconcile) করার জন্য ডেডিকেটেড SP।
--              ১. ইনভয়েস রোলব্যাকের পর অতিরিক্ত স্টক বেড়ে যাওয়া রোধ করে।
--              ২. tblDCStore-এর ফ্যান্টম/ভুল নেগেটিভ ইস্যু রো মুছে ফেলে।
--              ৩. Receive রো-এর StockQty কে আসল চালানের TotalQuantity-র সাথে রি-এলাইন করে।
-- =========================================================================================

IF OBJECT_ID('dbo.sp_Adjust_DCStore_Stock_Only', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Adjust_DCStore_Stock_Only;
GO

CREATE PROCEDURE [dbo].[sp_Adjust_DCStore_Stock_Only]
    @InvoiceCode NVARCHAR(500) = NULL,      -- যে ইনভয়েসের স্টক রিভার্স করতে হবে (যেমন: INV-...)
    @ProductCode NVARCHAR(100) = NULL,      -- নির্দিষ্ট প্রোডাক্টের স্টক রিকনসাইল করতে চাইলে
    @FixCorruptedReceiveQty BIT = 1         -- 1 হলে Receive রো-এর StockQty > TotalQuantity হয়ে যাওয়া ডাটা ঠিক করবে
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @AffectedRows INT = 0;
        DECLARE @DeletedIssues INT = 0;
        DECLARE @FixedReceives INT = 0;

        -- =========================================================================
        -- কেস ১: ইনভয়েস কোড দেওয়া হলে -> ওই ইনভয়েসের ইস্যু রো মুছে ফেলা ও স্টক অ্যাডজাস্ট
        -- =========================================================================
        IF (@InvoiceCode IS NOT NULL AND LTRIM(RTRIM(@InvoiceCode)) <> '')
        BEGIN
            -- ইনভয়েসের ডিটেইল টেবিল (অথবা ব্যাকআপ টেবিল) থেকে ডিসিস্টোর রেফারেন্স নেওয়া
            DECLARE @Details TABLE
            (
                DCStoreId INT,
                TotalQty DECIMAL(18, 2),
                ProductCode NVARCHAR(100),
                BatchNo NVARCHAR(100)
            );

            INSERT INTO @Details (DCStoreId, TotalQty, ProductCode, BatchNo)
            SELECT ISNULL(ID.DCStoreId, 0), ISNULL(ID.TotalQuantity, 0), ID.ProductCode, ID.BatchNo
            FROM dbo.tblInvoice I
            INNER JOIN dbo.tblInvoiceDetail ID ON I.InvoiceId = ID.InvoiceId
            WHERE I.InvoiceNo = @InvoiceCode;

            -- যদি ইতিমধ্যে tblInvoiceDetail থেকে মুছে গিয়ে থাকে, তবে ডিলিট রেকর্ড থেকে নেওয়া
            IF NOT EXISTS (SELECT 1 FROM @Details)
            BEGIN
                INSERT INTO @Details (DCStoreId, TotalQty, ProductCode, BatchNo)
                SELECT ISNULL(DCStoreId, 0), ISNULL(TotalQuantity, 0), ProductCode, BatchNo
                FROM dbo.tblInvoiceDetail_DeleterRecord
                WHERE InvoiceId = (SELECT TOP 1 InvoiceId FROM dbo.tblInvoiceDeleteLog WHERE InvoiceNo = @InvoiceCode ORDER BY DeleteDateTime DESC);
            END

            -- ১. tblDCStore থেকে ওই ইনভয়েসের তৈরি হওয়া নেগেটিভ ইস্যু (Trans_Type = 'Issue') মুছে ফেলা
            DECLARE @DCStoreId INT, @TotalQty DECIMAL(18, 2);

            DECLARE curDetails CURSOR LOCAL FAST_FORWARD FOR
                SELECT DCStoreId, TotalQty FROM @Details WHERE DCStoreId > 0;

            OPEN curDetails;
            FETCH NEXT FROM curDetails INTO @DCStoreId, @TotalQty;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                -- নেগেটিভ রো চিহ্নিত করে ডিলিট
                DELETE TOP (1) FROM dbo.tblDCStore
                WHERE ReferenceDCStoreId = @DCStoreId
                  AND Trans_Type = 'Issue'
                  AND StockQty = (@TotalQty * -1);

                SET @DeletedIssues = @DeletedIssues + @@ROWCOUNT;

                -- ২. রিসিভ রো যদি আগে ভুলবশত বাড়িয়ে দেওয়া হয়ে থাকে, তা TotalQuantity-তে রিসেট করা
                IF (@FixCorruptedReceiveQty = 1)
                BEGIN
                    UPDATE dbo.tblDCStore
                    SET StockQty = TotalQuantity
                    WHERE DCStoreId = @DCStoreId
                      AND Trans_Type = 'Receive'
                      AND StockQty > TotalQuantity;

                    SET @FixedReceives = @FixedReceives + @@ROWCOUNT;
                END

                FETCH NEXT FROM curDetails INTO @DCStoreId, @TotalQty;
            END

            CLOSE curDetails;
            DEALLOCATE curDetails;

            PRINT 'Invoice ' + @InvoiceCode + ' Stock Reverted Successfully. Deleted Issue Rows: ' 
                  + CAST(@DeletedIssues AS NVARCHAR(10)) + ', Aligned Receive Rows: ' + CAST(@FixedReceives AS NVARCHAR(10));
        END

        -- =========================================================================
        -- কেস ২: প্রোডাক্ট কোড দেওয়া হলে (অথবা জেনারেল ফিক্স) -> Receive রো স্টক রিসেট
        -- =========================================================================
        IF (@FixCorruptedReceiveQty = 1)
        BEGIN
            -- Receive রো-এর StockQty কখনোই TotalQuantity-র বেশি হতে পারে না
            -- পূর্বে sp_Delete_ProformaInvoice এর যোগ করার কারণে যাদের StockQty বেড়ে গিয়েছিল, তাদের ফিক্স করা
            UPDATE dbo.tblDCStore
            SET StockQty = TotalQuantity
            WHERE Trans_Type = 'Receive'
              AND StockQty <> TotalQuantity
              AND (@ProductCode IS NULL OR ProductCode = @ProductCode);

            SET @FixedReceives = @FixedReceives + @@ROWCOUNT;

            PRINT 'General Stock Alignment Completed. Total Receive Rows Corrected: ' + CAST(@FixedReceives AS NVARCHAR(10));
        END

        COMMIT TRANSACTION;

        -- চূড়ান্ত ফলাফল আউটপুট
        SELECT 
            'SUCCESS' AS Status,
            @InvoiceCode AS ProcessedInvoice,
            @ProductCode AS ProcessedProduct,
            @DeletedIssues AS IssueRowsRemoved,
            @FixedReceives AS ReceiveRowsAligned,
            'DC Store Stock has been successfully adjusted!' AS Message;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
