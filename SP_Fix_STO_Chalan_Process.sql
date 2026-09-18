-- ======================================================================================
-- SCRIPT: SP_Fix_STO_Chalan_Process.sql
-- PURPOSE: Fix STO and Challan Process issues (B2B Transfer Issue/Receive & Delete Rollback)
-- DATABASE: [SalesDisDB_Unicorn_DB]
-- DATE: 2026-09-02
-- ======================================================================================

USE [SalesDisDB_Unicorn_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ======================================================================================
-- 1. PROCEDURE: [dbo].[sp_Del_B2BDelete]
-- DESCRIPTION: Cleanly deletes a B2B Transfer Challan, removing any Issue and Receive 
--              ledger rows created in tblDCStore, and deleting the Challan records.
-- ======================================================================================
IF OBJECT_ID('dbo.sp_Del_B2BDelete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Del_B2BDelete;
GO

CREATE PROCEDURE [dbo].[sp_Del_B2BDelete]
    @chalanId NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Delete B2B Issue lines in tblDCStore associated with this Challan
        DELETE FROM dbo.tblDCStore 
        WHERE Trans_Type = 'Issue' 
          AND (
              ChalanDetailsId IN (SELECT ChalanDetailsId FROM dbo.tblChalanDetail WHERE ChalanId = @chalanId)
              OR ChalanNo IN (SELECT ChalanNo FROM dbo.tblChalanInfo WHERE ChalanId = @chalanId)
          );

        -- 2. Delete B2B Receive lines in tblDCStore if the Challan was already received at destination
        DELETE FROM dbo.tblDCStore 
        WHERE Trans_Type = 'Receive' 
          AND StockCondition = 'B2BReceive'
          AND (
              ChalanDetailsId IN (SELECT ChalanDetailsId FROM dbo.tblChalanDetail WHERE ChalanId = @chalanId)
              OR ChalanNo IN (SELECT ChalanNo FROM dbo.tblChalanInfo WHERE ChalanId = @chalanId)
          );

        -- 3. Delete Challan Details and Master
        DELETE FROM dbo.tblChalanDetail WHERE ChalanId = @chalanId;
        DELETE FROM dbo.tblChalanInfo WHERE ChalanId = @chalanId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;
GO

PRINT 'sp_Del_B2BDelete updated successfully!';
GO
