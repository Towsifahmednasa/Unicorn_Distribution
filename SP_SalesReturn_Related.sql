/* ==========================================================================
   SALES RETURN RELATED STORED PROCEDURES
   Page: SInventory_UI/SalesReturn.aspx
   Target Database: SalesDisDB
   ========================================================================== */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ==========================================================================
-- 1. Procedure: spInsertReturnInvoice_new
-- Purpose: Saves Sales Return Master record (tblReturnInvoice)
-- ==========================================================================
IF OBJECT_ID('dbo.spInsertReturnInvoice_new', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spInsertReturnInvoice_new;
GO

CREATE PROCEDURE [dbo].[spInsertReturnInvoice_new]
(
    @ReturnInvoiceId INT,
    @InvoiceDate datetime = null,
    @OrderId int = null,
    @OrderNo nvarchar(50) = null,
    @OrderDate datetime = null,
    @CustomerMasterId int = null,
    @ComUnitId int = null,
    @MiaId int = null,
    @PaymentTypeId int = null,
    @TpTotal decimal(18,2) = null,
    @TpDiscount decimal(18,2) = null,
    @TpVat decimal(18,2) = null,
    @DpNAme nvarchar(50) = null, 
    @createBy nvarchar(50) = null,
    @Createdate datetime = null,
    @TotalSpecialAmount decimal(18,2) = null,
    @ProductOffer bit = null,
    @OldTradePolicy bit = null,
    @Remarks nvarchar(max) = null,
    @FixedCustomer bit = null,
    @MIACode nvarchar(50) = null,
    @MIAName nvarchar(100) = null,
    @MarketCode nvarchar(50) = null,
    @MarketName nvarchar(100) = null,
    @AreaCode nvarchar(50) = null,
    @DisCode nvarchar(50) = null,
    @FEName nvarchar(100) = null,
    @RegionCode nvarchar(50) = null,
    @DZSMName nvarchar(100) = null,  
    @invoiceid int = null,
    @DpMob nvarchar(100) = null,
    @Type bit = null,
    @MIOId_new int = null,
    @Terri_Id_new int = null,
    @MioEmpId_new int = null,
    @Mio_SapCode_New nvarchar(100) = null
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ReturnInvoiceNo nvarchar(50);
    DECLARE @yearText NVARCHAR(MAX) = SUBSTRING(CONVERT(NVARCHAR(MAX), YEAR(@InvoiceDate)), 3, 3);
    DECLARE @monthText NVARCHAR(MAX) = (CASE WHEN LEN(MONTH(@InvoiceDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), MONTH(@InvoiceDate)) ELSE CONVERT(NVARCHAR(MAX), MONTH(@InvoiceDate)) END);
    DECLARE @dateText NVARCHAR(MAX) = (CASE WHEN LEN(DAY(@InvoiceDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), DAY(@InvoiceDate)) ELSE CONVERT(NVARCHAR(MAX), DAY(@InvoiceDate)) END);

    SELECT @ReturnInvoiceNo = 'RIN--' + (@yearText + @monthText + @dateText) + CONVERT(NVARCHAR(MAX), CONVERT(INT, ISNULL(MAX(ReturnInvoiceId), 10000)) + 1001)  
    FROM tblReturnInvoice 
    WHERE CONVERT(date, ReturnInvoiceDate) = CONVERT(date, @InvoiceDate);

    DECLARE @RegionId_Rtn INT = 0, @AreaId_Rtn INT = 0;
    SELECT @AreaId_Rtn = ar.AreaId, @RegionId_Rtn = rg.RegionId 
    FROM dbo.tblTerritory terry WITH (NOLOCK)
    INNER JOIN dbo.tblArea ar WITH (NOLOCK) ON ar.AreaId = terry.AreaId
    INNER JOIN dbo.tblRegion rg WITH (NOLOCK) ON rg.RegionId = ar.RegionId  
    WHERE terry.TerritoryId = @Terri_Id_new;

    INSERT INTO [dbo].[tblReturnInvoice]
    (
        ReturnInvoiceNo,  
        ReturnInvoiceDate,  
        CreateBy,  
        CreateDate,  
        OrderNo,  
        OrderDate,  
        CustomerMasterId,  
        ComUnitId,  
        MiaId,  
        PaymentTypeId,  
        TpTotal,  
        TpDiscount,  
        TpVat,  
        OrderId,  
        TotalSpecialAmount,  
        OldTradePolicy,  
        ProductOffer,
        Remarks,
        MIACode,
        MIAName,
        MarketCode,
        MarketName,
        AreaCode,
        DisCode,
        FEName,
        RegionCode,
        DZSMName,
        FixedCustomer,
        InvoiceId,
        IsSalesReturnWithoutOrder,
        SubInvoiceId,  
        Types,
        [MIOId_new],
        [Terri_Id_new],
        [MioEmpId_new],
        [Mio_SapCode_New],
        RegionId_Rtn,
        AreaId_Rtn
    )
    VALUES
    (
        @ReturnInvoiceNo,  
        @InvoiceDate,  
        @CreateBy,  
        @CreateDate,  
        @OrderNo,  
        @OrderDate,  
        @CustomerMasterId,  
        @ComUnitId,  
        @MiaId,  
        @PaymentTypeId,  
        @TpTotal,  
        @TpDiscount,  
        @TpVat,  
        @OrderId,  
        @TotalSpecialAmount,  
        @OldTradePolicy,  
        @ProductOffer,
        @Remarks,
        @MIACode,
        @MIAName,
        @MarketCode,
        @MarketName,
        @AreaCode,
        @DisCode,
        @FEName,
        @RegionCode,
        @DZSMName,
        @FixedCustomer,  
        @InvoiceId,
        'No order',
        0,
        @Type,
        @MIOId_new,
        @Terri_Id_new,
        @MioEmpId_new,
        @Mio_SapCode_New,
        @RegionId_Rtn,
        @AreaId_Rtn
    );

    SELECT SCOPE_IDENTITY();
END
GO


-- ==========================================================================
-- 2. Procedure: sp_UpdateAndInsertInvoiceDetailSalesReturn
-- Purpose: Inserts Sales Return Detail (tblInvoiceDetailReturn)
-- ==========================================================================
IF OBJECT_ID('dbo.sp_UpdateAndInsertInvoiceDetailSalesReturn', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_UpdateAndInsertInvoiceDetailSalesReturn;
GO

CREATE PROCEDURE [dbo].[sp_UpdateAndInsertInvoiceDetailSalesReturn]
    @InvoiceDetailReturnId INT OUTPUT,
    @InvoiceId INT,
    @InvoiceDetailId INT,
    @Quantity INT,
    @PreviousQuantity INT,
    @BonusQuantity INT,
    @TotalQuantity INT,
    @TotalPrice DECIMAL(18,2),
    @TotalPriceVatAmount DECIMAL(18,2),
    @DiscountPercentage DECIMAL(5,2),
    @DiscountAmount DECIMAL(18,2),
    @NetAmount DECIMAL(18,2),
    @DeliveryStatus NVARCHAR(50),
    @ReturnReason NVARCHAR(250)
AS
BEGIN
    INSERT INTO tblInvoiceDetailReturn 
    (
        InvoiceId,
        InvoiceDetailId,
        PreviousQuantity,
        sndReturnQuantity,
        sndReturnBonusQuantity,
        sndReturnTotalQuantity,
        sndReturnTotalPrice,
        sndReturnTotalPriceVatAmount,
        sndReturnDiscountPercentage,
        sndReturnDiscountAmount,
        sndReturnNetAmount,
        sndReturnStatus,
        sndReturnReason
    )
    VALUES 
    (
        @InvoiceId,
        @InvoiceDetailId,
        @PreviousQuantity,
        @Quantity,
        @BonusQuantity,
        @TotalQuantity,
        @TotalPrice,
        @TotalPriceVatAmount,
        @DiscountPercentage,
        @DiscountAmount,
        @NetAmount,
        @DeliveryStatus,
        @ReturnReason
    );

    SET @InvoiceDetailReturnId = SCOPE_IDENTITY();

    SELECT @InvoiceDetailReturnId AS InvoiceDetailReturnId;
END
GO
