USE [SalesDisDB_Unicorn_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================
-- Procedure: tbl_INV_CentralStoreStoIssue (CORRECTED)
-- ==========================================================================
IF OBJECT_ID('dbo.tbl_INV_CentralStoreStoIssue', 'P') IS NOT NULL
    DROP PROCEDURE dbo.tbl_INV_CentralStoreStoIssue;
GO

CREATE PROCEDURE [dbo].[tbl_INV_CentralStoreStoIssue]
	@StockInTransfarId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ProductId INT;
	DECLARE @ProductCode NVARCHAR(100);
	DECLARE @ProductName NVARCHAR(200);
	DECLARE @PackSize NVARCHAR(100);
	DECLARE @FinalOutputQuantity DECIMAL(18,3);
	DECLARE @MaterialConversionDate DATETIME;
	DECLARE @ChallanNo NVARCHAR(100);
	DECLARE @ChallanDate DATETIME;
	DECLARE @CostPerUnit DECIMAL(18,4);
	DECLARE @TotalPrice DECIMAL(18,4);
	DECLARE @StockInTransferMasterId INT;
	DECLARE @WearhouseId INT;
	DECLARE @MaterialConversionCode NVARCHAR(100);
	DECLARE @CompanyId INT;
	DECLARE @ApproveDate DATETIME;
	DECLARE @TranscationTypeId INT;
	DECLARE @InventoryTypeId INT;
	DECLARE @InventoryType NVARCHAR(200);
	DECLARE @ReceiveId INT;
	DECLARE @ReqId INT;
	DECLARE @Batch NVARCHAR(200);
	DECLARE @MfgDate DATETIME;
	DECLARE @ExpDate DATETIME;

	DECLARE @MyCursor CURSOR;
	SET @MyCursor = CURSOR FAST_FORWARD FOR
		SELECT 
			PD.ProductId,
			PD.ProductCode,
			PD.ProductName,
			PD.PackSize,
			WM.PickingQty,
			WM.ReceiveDate,
			RQ.PickingNo,
			RQ.PickingDate,
			ISNULL(CRS.Avg_Rate, 0),
			WM.PickingQty * ISNULL(CRS.Avg_Rate, 0) AS TotalPrice,
			WM.StockInTransfarId,
			WH.WearhouseId,
			RQ.ReqNo,
			PD.CompanyId,
			RQ.PickingDate,
			TSC.TranscationTypeId,
			ITS.InventoryTypeId,
			ITS.InventoryType,
			WM.ReceiveId,
			WM.ReqId,
			WM.BatchNo,
			ISNULL(WM.MfgDate, GETDATE()) AS MfgDate,
			ISNULL(WM.ExpDate, GETDATE()) AS ExpDate
		FROM dbo.tblStockInTransfar AS WM
		LEFT JOIN dbo.tblRequisition AS RQ ON WM.ReqId = RQ.ReqId
		LEFT JOIN dbo.Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tblStockInTransfar')))
		LEFT JOIN CSTL_ERPDB..Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1
		INNER JOIN dbo.tblProduct AS PD ON WM.ProductCode = PD.ProductCode
		INNER JOIN dbo.tblWearhouse AS WH ON PD.CompanyId = WH.CompanyId
		LEFT JOIN (
			SELECT WG.Company_Id, WG.Item_Id, WG.Close_Qty, WG.Avg_Rate, WG.Curr_Rate, WG.Trans_Date
			FROM dbo.tbl_INV_CentralStore_WeightedAvg WG
			JOIN (
				SELECT Item_Id, Company_Id, MAX(Weighted_Avg_Id) AS MaxWeighted_Avg_Id 
				FROM dbo.tbl_INV_CentralStore_WeightedAvg 
				GROUP BY Item_Id, Company_Id
			) Latest ON WG.Item_Id = Latest.Item_Id AND WG.Company_Id = Latest.Company_Id AND WG.Weighted_Avg_Id = Latest.MaxWeighted_Avg_Id
		) AS CRS ON WH.CompanyId = CRS.Company_Id AND PD.ProductId = CRS.Item_Id
		WHERE WM.StockInTransfarId = @StockInTransfarId;

	OPEN @MyCursor;
	FETCH NEXT FROM @MyCursor INTO 
		@ProductId, @ProductCode, @ProductName, @PackSize, @FinalOutputQuantity,
		@MaterialConversionDate, @ChallanNo, @ChallanDate, @CostPerUnit, @TotalPrice,
		@StockInTransferMasterId, @WearhouseId, @MaterialConversionCode, @CompanyId,
		@ApproveDate, @TranscationTypeId, @InventoryTypeId, @InventoryType,
		@ReceiveId, @ReqId, @Batch, @MfgDate, @ExpDate;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @WH_Opening_Qty DECIMAL(18, 3) = 0;
		DECLARE @WH_Opening_Rate DECIMAL(18, 4) = 0;
		DECLARE @WH_Opening_Amount DECIMAL(18, 4) = 0;
		DECLARE @WH_Closing_Qty DECIMAL(18, 3) = 0;
		DECLARE @WH_Average_Rate DECIMAL(18, 4) = 0;
		DECLARE @WH_Closing_Amount DECIMAL(18, 4) = 0;

		SELECT TOP 1 
			@WH_Opening_Qty = ISNULL(Close_Qty, 0),
			@WH_Opening_Amount = ISNULL(Close_Amt, 0)
		FROM dbo.tbl_INV_CentralStore_WeightedAvg
		WHERE Company_Id = @CompanyId AND Item_Id = @ProductId
		ORDER BY Trans_Date DESC, Weighted_Avg_Id DESC;

		SET @WH_Opening_Rate = CASE WHEN @WH_Opening_Qty > 0 THEN @WH_Opening_Amount / @WH_Opening_Qty ELSE 0 END;

		DECLARE @Curr_Qty DECIMAL(18,3) = @FinalOutputQuantity * -1;
		DECLARE @Curr_Rate DECIMAL(18,4) = @WH_Opening_Rate;
		DECLARE @Curr_Amt DECIMAL(18,4) = @Curr_Qty * @Curr_Rate;

		SET @WH_Closing_Qty = @WH_Opening_Qty + @Curr_Qty;
		SET @WH_Closing_Amount = @WH_Opening_Amount + @Curr_Amt;
		SET @WH_Average_Rate = CASE WHEN @WH_Closing_Qty > 0 THEN @WH_Closing_Amount / @WH_Closing_Qty ELSE 0 END;

		DECLARE @Weighted_Avg_Id_New INT = 0;

		-- 1. Check if Weighted Avg Record already exists
		SELECT @Weighted_Avg_Id_New = Weighted_Avg_Id
		FROM dbo.tbl_INV_CentralStore_WeightedAvg
		WHERE Mst_Trans_Id = @StockInTransfarId
		  AND Ref_No = @MaterialConversionCode
		  AND Company_Id = @CompanyId
		  AND Item_Id = @ProductId;

		IF (ISNULL(@Weighted_Avg_Id_New, 0) = 0)
		BEGIN
			INSERT INTO dbo.tbl_INV_CentralStore_WeightedAvg
			(
				Mst_Trans_Id, Trans_Id, Item_Code, Ref_No, Trans_Date, Actual_Date, Entry_Date,
				Trans_Type_Id, Inv_Type_Id, Inv_Type, Company_Id, Warehouse_Id, Item_Id,
				OP_Qty, OP_Rate, OP_Amt,
				Curr_Qty, Curr_Rate, Curr_Amt,
				Close_Qty, Avg_Rate, Close_Amt,
				Weighted_Avg_Id_Issue
			)
			VALUES
			(
				@StockInTransfarId, @ReceiveId, @ProductCode, @MaterialConversionCode, GETDATE(), @MaterialConversionDate, @ApproveDate,
				@TranscationTypeId, @InventoryTypeId, @InventoryType, @CompanyId, @WearhouseId, @ProductId,
				@WH_Opening_Qty, @WH_Opening_Rate, @WH_Opening_Amount,
				@Curr_Qty, @Curr_Rate, @Curr_Amt,
				@WH_Closing_Qty, @WH_Average_Rate, @WH_Closing_Amount,
				NULL
			);

			SET @Weighted_Avg_Id_New = SCOPE_IDENTITY();
		END

		-- 2. Insert Batch Details Row for this Issue into tbl_INV_Central_BatchWiseStock
		IF (@Weighted_Avg_Id_New > 0)
		BEGIN
			DECLARE @Ref_Weighted_Avg_Id INT = 0;

			-- Find Parent Inbound Weighted_Avg_Id for reference
			SELECT @Ref_Weighted_Avg_Id = Weighted_Avg_Id 
			FROM dbo.tbl_INV_Central_BatchWiseStock 
			WHERE Batch_Stock_Id = @ReceiveId;

			IF (ISNULL(@Ref_Weighted_Avg_Id, 0) = 0)
			BEGIN
				SELECT TOP 1 @Ref_Weighted_Avg_Id = BS.Weighted_Avg_Id 
				FROM dbo.tbl_INV_Central_BatchWiseStock BS
				INNER JOIN dbo.tbl_INV_CentralStore_WeightedAvg WA ON BS.Weighted_Avg_Id = WA.Weighted_Avg_Id
				WHERE WA.Item_Code = @ProductCode 
				  AND BS.Batch_No = @Batch 
				  AND BS.Trans_Qty > 0
				ORDER BY BS.Batch_Stock_Id DESC;
			END

			-- Prevent duplicate batch entry for this transfer
			IF NOT EXISTS
			(
				SELECT 1 
				FROM dbo.tbl_INV_Central_BatchWiseStock 
				WHERE Weighted_Avg_Id = @Weighted_Avg_Id_New 
				  AND StockInTransfarId = @StockInTransfarId
				  AND Batch_No = @Batch
			)
			BEGIN
				INSERT INTO dbo.tbl_INV_Central_BatchWiseStock
				(
					Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty,
					Remarks, StockInTransfarId, Issue_Weighted_Avg_Id
				)
				VALUES
				(
					@Weighted_Avg_Id_New, @Batch, @MfgDate, @ExpDate, @FinalOutputQuantity * -1,
					N'STO Picking', @StockInTransfarId, ISNULL(@Ref_Weighted_Avg_Id, 0)
				);
			END
		END

		FETCH NEXT FROM @MyCursor INTO 
			@ProductId, @ProductCode, @ProductName, @PackSize, @FinalOutputQuantity,
			@MaterialConversionDate, @ChallanNo, @ChallanDate, @CostPerUnit, @TotalPrice,
			@StockInTransferMasterId, @WearhouseId, @MaterialConversionCode, @CompanyId,
			@ApproveDate, @TranscationTypeId, @InventoryTypeId, @InventoryType,
			@ReceiveId, @ReqId, @Batch, @MfgDate, @ExpDate;
	END

	CLOSE @MyCursor;
	DEALLOCATE @MyCursor;
END;
GO

PRINT 'Procedure tbl_INV_CentralStoreStoIssue created successfully!';
GO
