-- ======================================================================================
-- SCRIPT: SP_NewStockTransferDcToDc_Related.sql
-- PAGE: SInventory_UI/NewStockTransferDcToDc.aspx & B2B Transfer (Issue, Receive, Delete)
-- DATE: 2026-09-01
-- ======================================================================================

USE [SalesDisDB_Unicorn_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ======================================================================================
-- 1. PROCEDURE: sp_Del_B2BDelete
-- DESCRIPTION: Deletes a B2B DC-to-DC Challan and restores issued stock back to tblDCStore
-- ======================================================================================
IF OBJECT_ID('dbo.sp_Del_B2BDelete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Del_B2BDelete;
GO

CREATE PROCEDURE [dbo].[sp_Del_B2BDelete]
    @chalanId NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DCstoreID NVARCHAR(500);
    DECLARE @Qty NVARCHAR(500);

    DECLARE @MyCursor CURSOR;
    SET @MyCursor = CURSOR FAST_FORWARD FOR
        SELECT DCStoreId, Quantity 
        FROM dbo.tblChalanDetail 
        WHERE ChalanId = @chalanId;

    OPEN @MyCursor;
    FETCH NEXT FROM @MyCursor INTO @DCstoreID, @Qty;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @Qty_Now DECIMAL(18,2) = 0;

        SELECT @Qty_Now = StockQty 
        FROM dbo.tblDCStore 
        WHERE DCStoreId = @DCstoreID;

        UPDATE dbo.tblDCStore 
        SET StockQty = @Qty_Now + @Qty 
        WHERE DCStoreId = @DCstoreID;

        FETCH NEXT FROM @MyCursor INTO @DCstoreID, @Qty;
    END;

    CLOSE @MyCursor;
    DEALLOCATE @MyCursor;

    DELETE FROM dbo.tblChalanDetail WHERE ChalanId = @chalanId;
    DELETE FROM dbo.tblChalanInfo WHERE ChalanId = @chalanId;
END;
GO

-- ======================================================================================
-- 2. PROCEDURE: sp_DeleteDwpotToWhChalan
-- DESCRIPTION: Deletes Depot To WH Challan Master and Detail
-- ======================================================================================
IF OBJECT_ID('dbo.sp_DeleteDwpotToWhChalan', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DeleteDwpotToWhChalan;
GO

CREATE PROCEDURE [dbo].[sp_DeleteDwpotToWhChalan]
    @masterId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.tblDepotToWHChalanInfo WHERE SChalanId = @masterId;
    DELETE FROM dbo.tblDepotToWHChalanDetail WHERE SChalanId = @masterId;
END;
GO

-- ======================================================================================
-- 3. PROCEDURE: sp_SAP_B2BStockReceiveByDc
-- DESCRIPTION: Receives B2B Challan Stock at destination DC and inserts into tblDCStore
-- ======================================================================================
IF OBJECT_ID('dbo.sp_SAP_B2BStockReceiveByDc', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SAP_B2BStockReceiveByDc;
GO

CREATE PROCEDURE [dbo].[sp_SAP_B2BStockReceiveByDc]
    @MasterId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ReceiveIdMAX INT = 0;

    DECLARE @ProductCode NVARCHAR(MAX);
    DECLARE @ProductName NVARCHAR(MAX);
    DECLARE @PackSize NVARCHAR(MAX);
    DECLARE @BatchNo NVARCHAR(MAX);
    DECLARE @Quantity INT;
    DECLARE @ExpDate DATETIME;
    DECLARE @ReceiveDate DATETIME;
    DECLARE @ChalanNo NVARCHAR(MAX);
    DECLARE @ChalanDate DATETIME;
    DECLARE @ComUnitId INT;
    DECLARE @ReqId INT;
    DECLARE @ReqChildId INT;
    DECLARE @StockInTransfarId INT;
    DECLARE @MfgDate DATETIME;
    DECLARE @ChalanDetailsId INT;

    DECLARE @MyCursor CURSOR;
    SET @MyCursor = CURSOR FAST_FORWARD FOR
        SELECT 
            PD.ProductCode,
            PD.ProductName,
            PD.PackSize,
            CD.BatchNo,
            Quantity,
            DC.ExpDate,
            DC.ReceiveDate,
            CI.ChalanNo,
            CI.ChalanDate, 
            C.ComUnitId,
            DC.MfgDate,
            CD.ChalanDetailsId
        FROM tblChalanDetail AS CD
        LEFT JOIN tblChalanInfo AS CI ON CD.ChalanId = CI.ChalanId
        LEFT JOIN tblDCStore AS DC ON CD.DCStoreId = DC.DCStoreId
        LEFT JOIN tblProduct AS PD ON CD.ProductCode = PD.ProductCode
        LEFT JOIN tblCompanyUnit AS C ON CI.ToComUnitCode = C.ComUnitCode
        WHERE CD.ChalanId = @MasterId;

    OPEN @MyCursor;
    FETCH NEXT FROM @MyCursor INTO 
        @ProductCode, @ProductName, @PackSize, @BatchNo, @Quantity,
        @ExpDate, @ReceiveDate, @ChalanNo, @ChalanDate, @ComUnitId,
        @MfgDate, @ChalanDetailsId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @ReceiveIdMAX = (ISNULL(MAX(DCStoreId), 0) + 1) FROM tblDCStore;

        INSERT INTO [dbo].[tblDCStore]
        (
            DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
            TotalQuantity, ExpDate, ReceiveDate, ChalanNo, ChalanDate,
            ComUnitId, StockQty, DamageQty, StockRcvDate,
            StockCondition, MfgDate, ChalanDetailsId, Trans_Type
        )
        VALUES
        (
            @ReceiveIdMAX, '', @ProductCode, @ProductName, @PackSize, @BatchNo,
            @Quantity, @ExpDate, @ReceiveDate, @ChalanNo, @ChalanDate,
            @ComUnitId, @Quantity, 0, GETDATE(),
            'Available', @MfgDate, @ChalanDetailsId, 'Receive'
        );

        FETCH NEXT FROM @MyCursor INTO 
            @ProductCode, @ProductName, @PackSize, @BatchNo, @Quantity,
            @ExpDate, @ReceiveDate, @ChalanNo, @ChalanDate, @ComUnitId,
            @MfgDate, @ChalanDetailsId;
    END;

    CLOSE @MyCursor;
    DEALLOCATE @MyCursor;

    RETURN @ReceiveIdMAX;
END;
GO

-- ======================================================================================
-- 4. PROCEDURE: sp_SAP_B2BChallanMasterInsert
-- DESCRIPTION: Inserts B2B Challan Master from SAP Stock Movement
-- ======================================================================================
IF OBJECT_ID('dbo.sp_SAP_B2BChallanMasterInsert', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SAP_B2BChallanMasterInsert;
GO

CREATE PROCEDURE [dbo].[sp_SAP_B2BChallanMasterInsert] 
    @ChallanNo NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ReceiveIdMAX INT = 0;

    DECLARE @ChallanDate DATETIME;
    DECLARE @FromUnitCode NVARCHAR(500);
    DECLARE @FromUnitName NVARCHAR(500);
    DECLARE @FromUnitAddress NVARCHAR(500);
    DECLARE @ToUnitCode NVARCHAR(500);
    DECLARE @ToUnitName NVARCHAR(500);
    DECLARE @ToUnitAddress NVARCHAR(500);
    DECLARE @TotalValue DECIMAL(18,2);
    DECLARE @TotalVat DECIMAL(18,2);
    DECLARE @GrandTotal DECIMAL(18,2);
    DECLARE @FromComUnitId INT;

    DECLARE @MyCursor CURSOR;
    SET @MyCursor = CURSOR FAST_FORWARD FOR
        SELECT 
            challan_date,
            FUNT.ComUnitCode AS FromUnitCode,
            FUNT.ComUnitName AS FromUnitName,
            FUNT.Address AS FromUnitAddress,
            TUNT.ComUnitCode AS ToUnitCode,
            TUNT.ComUnitName AS ToUnitName,
            TUNT.Address AS ToUnitAddress,
            ISNULL(SUM(quantity * ISNULL(UnitPrice, 1)), 0) AS TotalValue,
            ISNULL(SUM(quantity * ISNULL(VATAmountPerUnit, 1)), 0) AS TotalVat,
            ISNULL(SUM(quantity * ISNULL(UnitPrice, 1)) + SUM(quantity * ISNULL(VATAmountPerUnit, 1)), 0) AS GrandTotal,
            FUNT.ComUnitId AS FromComUnitId
        FROM SAP_API_Data..tblSAP_StockMovementMaster AS M
        LEFT JOIN tblCompanyUnit AS FUNT ON M.from_plant_code = FUNT.SAP_Code
        LEFT JOIN tblCompanyUnit AS TUNT ON M.to_plant_code = TUNT.SAP_Code
        LEFT JOIN SAP_API_Data..tblSAP_StockMovementDetail AS D ON M.StockMovementMasterId = D.StockMovementMasterId
        LEFT JOIN tblProduct AS PD ON SUBSTRING(D.product_code, 13, LEN(D.product_code)) = PD.SAP_Code
        LEFT JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
        WHERE UP.IsActive = 1 
          AND challan_code NOT IN (SELECT ChallanNo FROM tblWHStockInMaster WHERE ChallanNo IS NOT NULL) 
          AND UPPER(RTRIM(LTRIM(challan_code))) = UPPER(RTRIM(LTRIM(@ChallanNo)))
        GROUP BY 
            challan_date, FUNT.ComUnitCode, FUNT.ComUnitName, FUNT.Address, 
            TUNT.ComUnitCode, TUNT.ComUnitName, TUNT.Address, FUNT.ComUnitId 
        HAVING ISNULL(SUM(quantity), 0) > 0;

    OPEN @MyCursor;
    FETCH NEXT FROM @MyCursor INTO 
        @ChallanDate, @FromUnitCode, @FromUnitName, @FromUnitAddress, 
        @ToUnitCode, @ToUnitName, @ToUnitAddress, @TotalValue, @TotalVat, @GrandTotal, @FromComUnitId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @ReceiveIdMAX = (ISNULL(MAX(ChalanId), 0) + 1) FROM tblChalanInfo;

        INSERT INTO [dbo].[tblChalanInfo]
        (
            ChalanId, ChalanDate, ChalanNo, TrackNo, DriverName,
            FromComUnitCode, FromComUnitName, FromComUnitAddress,
            ToComUnitCode, ToComUnitName, ToComUnitAddress,
            TotalValue, TotalVat, GrandTotal,
            ManufacId, IsDeliver, FromComUnitId, Note
        )
        VALUES
        (
            @ReceiveIdMAX, GETDATE(), @ChallanNo, '', '',
            @FromUnitCode, @FromUnitName, @FromUnitAddress,
            @ToUnitCode, @ToUnitName, @ToUnitAddress,
            @TotalValue, @TotalVat, @GrandTotal,
            1, 'False', @FromComUnitId, 'From SAP'
        );

        FETCH NEXT FROM @MyCursor INTO 
            @ChallanDate, @FromUnitCode, @FromUnitName, @FromUnitAddress, 
            @ToUnitCode, @ToUnitName, @ToUnitAddress, @TotalValue, @TotalVat, @GrandTotal, @FromComUnitId;
    END;

    CLOSE @MyCursor;
    DEALLOCATE @MyCursor;

    RETURN @ReceiveIdMAX;
END;
GO

-- ======================================================================================
-- 5. PROCEDURE: sp_SAP_B2BChallanDetailInsert
-- DESCRIPTION: Inserts B2B Challan Details from SAP Stock Movement & deducts DC Store stock
-- ======================================================================================
IF OBJECT_ID('dbo.sp_SAP_B2BChallanDetailInsert', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SAP_B2BChallanDetailInsert;
GO

CREATE PROCEDURE [dbo].[sp_SAP_B2BChallanDetailInsert] 
    @MasterId INT,
    @ChallanNo NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ReceiveIdMAX INT;

    DECLARE @ProductId INT;
    DECLARE @ProductCode NVARCHAR(500);
    DECLARE @ProductName NVARCHAR(500);
    DECLARE @Batch NVARCHAR(500);
    DECLARE @ExpDate DATETIME;
    DECLARE @MfgDate DATETIME;
    DECLARE @Qty INT;
    DECLARE @Price DECIMAL(18,2);
    DECLARE @VAT DECIMAL(18,2);
    DECLARE @TotalAmount DECIMAL(18,2);
    DECLARE @ComUnitId INT;

    DECLARE @MyCursor2 CURSOR;
    SET @MyCursor2 = CURSOR FAST_FORWARD FOR
        SELECT 
            PD.ProductId, PD.ProductCode, PD.ProductName, batch_no, expiry_date, manufacturer_date,
            quantity, ISNULL(UnitPrice, 1) AS UnitPrice, ISNULL(VATAmountPerUnit, 1) AS VAT,
            (quantity * ISNULL(UnitPrice, 1)) + (quantity * ISNULL(VATAmountPerUnit, 1)) AS TotalAmount,
            FUNT.ComUnitId
        FROM SAP_API_Data..tblSAP_StockMovementMaster AS M
        LEFT JOIN tblCompanyUnit AS FUNT ON M.from_plant_code = FUNT.SAP_Code
        LEFT JOIN SAP_API_Data..tblSAP_StockMovementDetail AS D ON M.StockMovementMasterId = D.StockMovementMasterId
        LEFT JOIN tblProduct AS PD ON SUBSTRING(D.product_code, 13, LEN(D.product_code)) = PD.SAP_Code
        LEFT JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
        WHERE ISNULL(quantity, 0) > 0 AND UP.IsActive = 1 
          AND UPPER(RTRIM(LTRIM(challan_code))) = UPPER(RTRIM(LTRIM(@ChallanNo)));

    OPEN @MyCursor2;
    FETCH NEXT FROM @MyCursor2 INTO 
        @ProductId, @ProductCode, @ProductName, @Batch, @ExpDate, @MfgDate,
        @Qty, @Price, @VAT, @TotalAmount, @ComUnitId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @StockQuantity INT = 0;
        DECLARE @AdjustmentQuantity INT = @Qty;

        WHILE (@AdjustmentQuantity > 0)
        BEGIN
            DECLARE @DCStoreId INT = 0;

            SELECT TOP 1 @StockQuantity = StockQty, @DCStoreId = DCStoreId 
            FROM tblDCStore 
            WHERE StockQty > 0 AND ComUnitId = @ComUnitId AND ProductCode = @ProductCode
            ORDER BY ExpDate;

            IF (@StockQuantity > 0)
            BEGIN
                IF (@StockQuantity > @AdjustmentQuantity) 
                BEGIN
                    UPDATE tblDCStore SET StockQty = @StockQuantity - @AdjustmentQuantity WHERE DCStoreId = @DCStoreId;
                    SET @AdjustmentQuantity = 0;
                END
                ELSE IF (@StockQuantity <= @AdjustmentQuantity)
                BEGIN
                    UPDATE tblDCStore SET StockQty = 0 WHERE DCStoreId = @DCStoreId;
                    SET @AdjustmentQuantity = @AdjustmentQuantity - @StockQuantity;
                END

                SELECT @ReceiveIdMAX = (ISNULL(MAX(ChalanDetailsId), 0) + 1) FROM tblChalanDetail;

                INSERT INTO [dbo].[tblChalanDetail]
                (
                    ChalanDetailsId, ProductCode, ProductName, Quantity, BatchNo,
                    UnitPrice, Value, Vat, ValueWVat, ChalanId, DCStoreId
                )
                VALUES
                (
                    @ReceiveIdMAX, @ProductCode, @ProductName, @Qty, @Batch,
                    @Price, @Qty * @Price, @VAT, @TotalAmount, @MasterId, @DCStoreId
                );
            END
            ELSE
            BEGIN
                BREAK;
            END
        END;

        FETCH NEXT FROM @MyCursor2 INTO 
            @ProductId, @ProductCode, @ProductName, @Batch, @ExpDate, @MfgDate,
            @Qty, @Price, @VAT, @TotalAmount, @ComUnitId;
    END;

    CLOSE @MyCursor2;
    DEALLOCATE @MyCursor2;
END;
GO

-- ======================================================================================
-- 6. PROCEDURE: sp_SAP_ChallanSendDetailByChalanId
-- DESCRIPTION: Retrieves Challan Detail for sending to SAP
-- ======================================================================================
IF OBJECT_ID('dbo.sp_SAP_ChallanSendDetailByChalanId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SAP_ChallanSendDetailByChalanId;
GO

CREATE PROCEDURE [dbo].[sp_SAP_ChallanSendDetailByChalanId]
    @ChalanId INT
AS
BEGIN 
    SET NOCOUNT ON;

    SELECT 
        c.ChalanId, 
        pro.SAP_Code AS product_code,  
        c.BatchNo AS batch_no,
        c.Quantity AS quantity,
        proU.UOMSAPCode AS UoM, 
        '' AS remarks 
    FROM tblChalanDetail c WITH (NOLOCK)
    LEFT JOIN tblProduct pro ON c.ProductCode = pro.ProductCode
    LEFT JOIN tblStockUOM proU ON proU.StockUOMId = pro.StockUOMId
    WHERE c.ChalanId = @ChalanId;
END;
GO

-- ======================================================================================
-- 7. PROCEDURE: sp_SAP_Up_SAP_ChallanSendByChalanId
-- DESCRIPTION: Flags Challan as sent to SAP
-- ======================================================================================
IF OBJECT_ID('dbo.sp_SAP_Up_SAP_ChallanSendByChalanId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SAP_Up_SAP_ChallanSendByChalanId;
GO

CREATE PROCEDURE [dbo].[sp_SAP_Up_SAP_ChallanSendByChalanId]
    @ChalanId INT
AS
BEGIN 
    SET NOCOUNT ON;
    UPDATE tblChalanInfo 
    SET SAP_ChallanSend = 1 
    WHERE ChalanId = @ChalanId;
END;
GO

-- ======================================================================================
-- 8. PROCEDURE: sp_SAP_Up_SAP_ChallanConfirmByChalanId
-- DESCRIPTION: Flags Challan confirmation as sent to SAP
-- ======================================================================================
IF OBJECT_ID('dbo.sp_SAP_Up_SAP_ChallanConfirmByChalanId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SAP_Up_SAP_ChallanConfirmByChalanId;
GO

CREATE PROCEDURE [dbo].[sp_SAP_Up_SAP_ChallanConfirmByChalanId]
    @ChalanId INT
AS
BEGIN 
    SET NOCOUNT ON;
    UPDATE tblChalanInfo 
    SET SAP_Challan_ConfirmationSend = 1 
    WHERE ChalanId = @ChalanId;
END;
GO
