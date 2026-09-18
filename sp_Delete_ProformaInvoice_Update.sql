USE [SalesDisDB_Unicorn_DB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Delete_ProformaInvoice] 
	@InvoiceCode NVARCHAR(500),
	@User NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM dbo.tblInvoice WHERE InvoiceNo = @InvoiceCode AND DelivaryInvoiceNo IS NULL)
    BEGIN
        DECLARE @OrderIdForUpdate INT;
        DECLARE @InvoiceId INT;
        DECLARE @InvoiceDetailId INT;
        DECLARE @DCStoreId INT;
        DECLARE @OrderId INT;
        DECLARE @OrderDetailsId INT;

        DECLARE @TotalQuantity DECIMAL(18,2) = 0;

        DECLARE @ProductCode NVARCHAR(MAX);
        DECLARE @ProductName NVARCHAR(MAX);
        DECLARE @PackSize NVARCHAR(MAX);
        DECLARE @BatchNo NVARCHAR(MAX);
        DECLARE @ReceiveDate DATETIME;
        DECLARE @ExpDate DATETIME;
        DECLARE @CostPrice DECIMAL(18,2);
        DECLARE @UnitPrice DECIMAL(18,2);
        DECLARE @UnitVatAmount DECIMAL(18,2);
        DECLARE @Quantity DECIMAL(18,2);
        DECLARE @BonusQuantity DECIMAL(18,2);
        DECLARE @TotalPrice DECIMAL(18,2);
        DECLARE @TotalPriceVatAmount DECIMAL(18,2);
        DECLARE @DiscountPercentage DECIMAL(18,2);
        DECLARE @DiscountAmount DECIMAL(18,2);
        DECLARE @NetAmount DECIMAL(18,2);
        DECLARE @DeliveryQuantity DECIMAL(18,2);
        DECLARE @DeliveryBonusQuantity DECIMAL(18,2);
        DECLARE @DeliveryTotalQuantity DECIMAL(18,2);
        DECLARE @DeliveryTotalPrice DECIMAL(18,2);
        DECLARE @DeliveryTotalPriceVatAmount DECIMAL(18,2);
        DECLARE @DeliveryDiscountPercentage DECIMAL(18,2);
        DECLARE @DeliveryDiscountAmount DECIMAL(18,2);
        DECLARE @DeliveryNetAmount DECIMAL(18,2);
        DECLARE @DeliveryStatus NVARCHAR(50);
        DECLARE @SpecialAmount DECIMAL(18,2);
        DECLARE @DelivarySpecialAmount DECIMAL(18,2);
        DECLARE @ReturnReason NVARCHAR(500);

        DECLARE @MyCursor CURSOR;
        SET @MyCursor = CURSOR FAST_FORWARD FOR
            SELECT 
                I.InvoiceId,
                ID.InvoiceDetailId,
                ISNULL(ID.DCStoreId, 0) AS DCStoreId,
                I.OrderId,
                ID.OrderDetailsId,
                ID.TotalQuantity,
                ID.ProductCode,
                ID.ProductName,
                ID.PackSize,
                ID.BatchNo,
                ID.ReceiveDate,
                ID.ExpDate,
                ID.CostPrice,
                ID.UnitPrice,
                ID.UnitVatAmount,
                ID.Quantity,
                ID.BonusQuantity,
                ID.TotalPrice,
                ID.TotalPriceVatAmount,
                ID.DiscountPercentage,
                ID.DiscountAmount,
                ID.NetAmount,
                ID.DeliveryQuantity,
                ID.DeliveryBonusQuantity,
                ID.DeliveryTotalQuantity,
                ID.DeliveryTotalPrice,
                ID.DeliveryTotalPriceVatAmount,
                ID.DeliveryDiscountPercentage,
                ID.DeliveryDiscountAmount,
                ID.DeliveryNetAmount,
                ID.DeliveryStatus,
                ID.SpecialAmount,
                I.DelivarySpecialAmount,
                ID.ReturnReason 
            FROM dbo.tblInvoice I 
            INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
            WHERE I.DelivaryInvoiceNo IS NULL 
              AND I.InvoiceNo = @InvoiceCode 
            ORDER BY ID.InvoiceDetailId;

        OPEN @MyCursor;
        FETCH NEXT FROM @MyCursor INTO 
            @InvoiceId, @InvoiceDetailId, @DCStoreId, @OrderId, @OrderDetailsId, @TotalQuantity,
            @ProductCode, @ProductName, @PackSize, @BatchNo, @ReceiveDate, @ExpDate,
            @CostPrice, @UnitPrice, @UnitVatAmount, @Quantity, @BonusQuantity,
            @TotalPrice, @TotalPriceVatAmount, @DiscountPercentage, @DiscountAmount,
            @NetAmount, @DeliveryQuantity, @DeliveryBonusQuantity, @DeliveryTotalQuantity,
            @DeliveryTotalPrice, @DeliveryTotalPriceVatAmount, @DeliveryDiscountPercentage,
            @DeliveryDiscountAmount, @DeliveryNetAmount, @DeliveryStatus,
            @SpecialAmount, @DelivarySpecialAmount, @ReturnReason;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- =========================================================================================
            -- [ফিক্স]: আসল Receive রো-তে StockQty আর ADD করা হবে না।
            -- ইনভয়েস তৈরির সময় tblDCStore-এ যে Issue লাইনটি (-Qty) ইনসার্ট হয়েছিল, সেটি সোজা DELETE করা হবে।
            -- =========================================================================================
            IF (@DCStoreId > 0)
            BEGIN
                -- ১. tblDCStore থেকে সংশ্লিষ্ট ইস্যু লাইনটি মুছে ফেলা
                DELETE TOP (1) FROM dbo.tblDCStore 
                WHERE ReferenceDCStoreId = @DCStoreId 
                  AND Trans_Type = 'Issue'
                  AND StockQty = (@TotalQuantity * -1);

                -- ২. যদি অতীতে ভুল প্রসিডিউরের কারণে Receive রো-র StockQty > TotalQuantity হয়ে থাকে, তবে তা ঠিক করা
                UPDATE dbo.tblDCStore
                SET StockQty = TotalQuantity
                WHERE DCStoreId = @DCStoreId
                  AND Trans_Type = 'Receive'
                  AND StockQty > TotalQuantity;
            END

            -- রিটার্ন ট্র্যাক লগ রাখা
            INSERT INTO dbo.tblProInvoiceReturnTrack 
            (
                ReturnTotalQuantity, DCStoreId, ReturnDate, ReturnExecutionDateTime, InvoiceDetailId, InvoiceId
            )
            VALUES 
            (
                @TotalQuantity, @DCStoreId, CONVERT(NVARCHAR(11), GETDATE(), 106), GETDATE(), @InvoiceDetailId, @InvoiceId
            );

            -- ডিলিট হিস্ট্রি রেকর্ড সংরক্ষণ
            INSERT INTO [dbo].[tblInvoiceDetail_DeleterRecord]
            (
                [InvoiceDetailId], [ProductCode], [ProductName], [PackSize], [BatchNo],
                [ReceiveDate], [ExpDate], [CostPrice], [UnitPrice], [UnitVatAmount],
                [Quantity], [BonusQuantity], [TotalQuantity], [TotalPrice], [TotalPriceVatAmount],
                [DiscountPercentage], [DiscountAmount], [NetAmount], [InvoiceId], [DCStoreId],
                [DeliveryQuantity], [DeliveryBonusQuantity], [DeliveryTotalQuantity], [DeliveryTotalPrice],
                [DeliveryTotalPriceVatAmount], [DeliveryDiscountPercentage], [DeliveryDiscountAmount],
                [DeliveryNetAmount], [DeliveryStatus], [OrderDetailsId], [SpecialAmount],
                [DelivarySpecialAmount], [ReturnReason]
            )
            VALUES
            (
                @InvoiceDetailId, @ProductCode, @ProductName, @PackSize, @BatchNo,
                @ReceiveDate, @ExpDate, @CostPrice, @UnitPrice, @UnitVatAmount,
                @Quantity, @BonusQuantity, @TotalQuantity, @TotalPrice, @TotalPriceVatAmount,
                @DiscountPercentage, @DiscountAmount, @NetAmount, @InvoiceId, @DCStoreId,
                @DeliveryQuantity, @DeliveryBonusQuantity, @DeliveryTotalQuantity, @DeliveryTotalPrice,
                @DeliveryTotalPriceVatAmount, @DeliveryDiscountPercentage, @DeliveryDiscountAmount,
                @DeliveryNetAmount, @DeliveryStatus, @OrderDetailsId, @SpecialAmount,
                @DelivarySpecialAmount, @ReturnReason
            );

            -- ইনভয়েস ডিটেইল ডিলিট
            DELETE FROM dbo.tblInvoiceDetail WHERE InvoiceDetailId = @InvoiceDetailId;
 
            SET @OrderIdForUpdate = @OrderId;
 
            FETCH NEXT FROM @MyCursor INTO 
                @InvoiceId, @InvoiceDetailId, @DCStoreId, @OrderId, @OrderDetailsId, @TotalQuantity,
                @ProductCode, @ProductName, @PackSize, @BatchNo, @ReceiveDate, @ExpDate,
                @CostPrice, @UnitPrice, @UnitVatAmount, @Quantity, @BonusQuantity,
                @TotalPrice, @TotalPriceVatAmount, @DiscountPercentage, @DiscountAmount,
                @NetAmount, @DeliveryQuantity, @DeliveryBonusQuantity, @DeliveryTotalQuantity,
                @DeliveryTotalPrice, @DeliveryTotalPriceVatAmount, @DeliveryDiscountPercentage,
                @DeliveryDiscountAmount, @DeliveryNetAmount, @DeliveryStatus,
                @SpecialAmount, @DelivarySpecialAmount, @ReturnReason;
        END

        CLOSE @MyCursor;
        DEALLOCATE @MyCursor;

        DECLARE @InvoiceIdForLog INT;
        DECLARE @InvoiceNoForLog NVARCHAR(500);
        SELECT @InvoiceIdForLog = InvoiceId, @InvoiceNoForLog = InvoiceNo 
        FROM dbo.tblInvoice 
        WHERE InvoiceNo = @InvoiceCode;

        -- অর্ডারের স্ট্যাটাস রিসেট
        UPDATE dbo.tblOrderDetail SET [Status] = NULL WHERE OrderId = @OrderIdForUpdate;
        UPDATE dbo.tblOrder SET IsInvoice = 0 WHERE OrderId = @OrderIdForUpdate;

        DECLARE @InvoMasId INT = 0;
        SELECT @InvoMasId = InvoiceId FROM dbo.tblInvoice WHERE InvoiceNo = @InvoiceCode;

        DELETE FROM dbo.tblReturnAmount WHERE InvoiceId = @InvoMasId;
        DELETE FROM dbo.tblInvoice WHERE InvoiceNo = @InvoiceCode;

        -- ডিলিট লগ এন্ট্রি
        INSERT INTO dbo.tblInvoiceDeleteLog (InvoiceId, InvoiceNo, DeleteDateTime, DeleteBy)
        VALUES (@InvoiceIdForLog, @InvoiceNoForLog, GETDATE(), @User);

    END
END
GO
