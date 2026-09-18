-- =========================================================================================
-- LIVE DATABASE DEPLOYMENT SCRIPT
-- Modules: 
--   1. Customer Payment (Discount, AIT & Delivery Charge Settlement)
--   2. Customer Aging & Outstanding Report
--   3. Delivery Sales Return (Delivery Charge Deduction & Account Posting)
-- Date: 06-Sep-2026
-- =========================================================================================

-- =========================================================================================
-- STEP 1: SCHEMA CHANGES (SalesDisDB_Unicorn_DB)
-- =========================================================================================
USE [SalesDisDB_Unicorn_DB]
GO

-- 1.1 Add DeductionCharge & NetReturnAmount to tblCustomerReturnMaster if not exists
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblCustomerReturnMaster]') 
      AND name = 'DeductionCharge'
)
BEGIN
    ALTER TABLE [dbo].[tblCustomerReturnMaster]
    ADD [DeductionCharge] DECIMAL(18, 2) NULL CONSTRAINT DF_tblCustomerReturnMaster_DeductionCharge DEFAULT 0.00;
    PRINT 'Added column DeductionCharge to tblCustomerReturnMaster.';
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblCustomerReturnMaster]') 
      AND name = 'NetReturnAmount'
)
BEGIN
    ALTER TABLE [dbo].[tblCustomerReturnMaster]
    ADD [NetReturnAmount] DECIMAL(18, 2) NULL CONSTRAINT DF_tblCustomerReturnMaster_NetReturnAmount DEFAULT 0.00;
    PRINT 'Added column NetReturnAmount to tblCustomerReturnMaster.';
END
GO

-- 1.2 Add DeductionCharge & NetReturnAmount to tblReturnInvoice (Market Return) if not exists
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblReturnInvoice]') 
      AND name = 'DeductionCharge'
)
BEGIN
    ALTER TABLE [dbo].[tblReturnInvoice]
    ADD [DeductionCharge] DECIMAL(18, 2) NULL CONSTRAINT DF_tblReturnInvoice_DeductionCharge DEFAULT 0.00;
    PRINT 'Added column DeductionCharge to tblReturnInvoice.';
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblReturnInvoice]') 
      AND name = 'NetReturnAmount'
)
BEGIN
    ALTER TABLE [dbo].[tblReturnInvoice]
    ADD [NetReturnAmount] DECIMAL(18, 2) NULL CONSTRAINT DF_tblReturnInvoice_NetReturnAmount DEFAULT 0.00;
    PRINT 'Added column NetReturnAmount to tblReturnInvoice.';
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblReturnInvoice]') 
      AND name = 'IsPosting'
)
BEGIN
    ALTER TABLE [dbo].[tblReturnInvoice]
    ADD [IsPosting] BIT NULL CONSTRAINT DF_tblReturnInvoice_IsPosting DEFAULT 0;
    PRINT 'Added column IsPosting to tblReturnInvoice.';
END
GO

-- 1.3 Add ReturnInvoiceDetailId to tblDCStore (Market Return Detail Link) if not exists
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblDCStore]') 
      AND name = 'ReturnInvoiceDetailId'
)
BEGIN
    ALTER TABLE [dbo].[tblDCStore]
    ADD [ReturnInvoiceDetailId] INT NULL CONSTRAINT DF_tblDCStore_ReturnInvoiceDetailId DEFAULT 0;
    PRINT 'Added column ReturnInvoiceDetailId to tblDCStore.';
END
GO


-- =========================================================================================
-- STEP 2: ACCOUNTS CONNECTIVITY INTEGRATION (CSTL_ERPDB_Unicorn)
-- =========================================================================================
USE [CSTL_ERPDB_Unicorn]
GO

-- 2.1 Add Return Delivery Charge Income (FlagId = 1032, GLId = 5968) to Table_Integration_AccountsConnectivity
IF NOT EXISTS (
    SELECT 1 FROM dbo.Table_Integration_AccountsConnectivity 
    WHERE PageId = 23 AND FlagId = 1032
)
BEGIN
    INSERT INTO dbo.Table_Integration_AccountsConnectivity (CompanyId, PageId, FlagId, GLId, SubGL, DRCR)
    VALUES (1, 23, 1032, 5968, 0, 'CR');
    PRINT 'Inserted Return Delivery Charge Income mapping (PageId=23, FlagId=1032, GLId=5968).';
END
ELSE
BEGIN
    UPDATE dbo.Table_Integration_AccountsConnectivity 
    SET GLId = 5968, DRCR = 'CR'
    WHERE PageId = 23 AND FlagId = 1032;
    PRINT 'Updated Return Delivery Charge Income mapping.';
END
GO


-- =========================================================================================
-- STEP 3: STORED PROCEDURES (SalesDisDB_Unicorn_DB)
-- =========================================================================================
USE [SalesDisDB_Unicorn_DB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 3.1 sp_Report_CustomerInvoiceAgingOutstanding
CREATE OR ALTER PROCEDURE [dbo].[sp_Report_CustomerInvoiceAgingOutstanding]
    @AsOnDate           DATE,
    @FromDate           DATE = NULL,
    @ToDate             DATE = NULL,
    @ComUnitId          INT = NULL,
    @CustomerMasterId   INT = NULL,
    @SalesPersonId      INT = NULL,
    @OnlyDueInvoices    BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @AsOnDate IS NULL
        SET @AsOnDate = CAST(GETDATE() AS DATE);

    ;WITH CTE_InvoiceSummary AS (
        SELECT 
            invD.InvoiceId,
            SUM(ISNULL(invD.DeliveryNetAmount, 0.00)) AS DetailNetAmount
        FROM dbo.tblInvoiceDetail invD WITH (NOLOCK)
        GROUP BY invD.InvoiceId
    ),
    CTE_PaymentSummary AS (
        SELECT 
            cpd.InvoiceId,
            SUM(ISNULL(cpd.PaymentAmount, 0.00) + ISNULL(cpd.Discount, 0.00) + ISNULL(cpd.AIT, 0.00)) AS PaidAmount,
            SUM(ISNULL(cpd.Discount, 0.00)) AS DiscountAmount,
            SUM(ISNULL(cpd.AIT, 0.00)) AS AITAmount
        FROM dbo.tblCustPayDetail cpd WITH (NOLOCK)
        LEFT JOIN dbo.tblCustomerPay cp WITH (NOLOCK) 
            ON cpd.CustPayId = cp.CustPayId
        WHERE (@AsOnDate IS NULL OR CAST(ISNULL(cpd.custPaymentDate, cp.PaymentDate) AS DATE) <= @AsOnDate)
        GROUP BY cpd.InvoiceId
    ),
    CTE_BaseData AS (
        SELECT 
            im.InvoiceId,
            cm.CustomerName AS [Customer Name],
            im.InvoiceNo AS [Invoice No],
            im.InvoiceDate AS [InvoiceDate_Raw],
            CONVERT(VARCHAR(11), im.InvoiceDate, 106) AS [Invoice Date],
            (ISNULL(invS.DetailNetAmount, 0.00) + ISNULL(im.DeliveryCharge, 0.00)) AS [Invoice Amount],
            ISNULL(payS.PaidAmount, 0.00) AS [Paid Amount],
            DATEDIFF(DAY, CAST(im.InvoiceDate AS DATE), @AsOnDate) AS [Credit Days],
            ((ISNULL(invS.DetailNetAmount, 0.00) + ISNULL(im.DeliveryCharge, 0.00)) - ISNULL(payS.PaidAmount, 0.00)) AS [Outstanding],
            ISNULL(emp.EmpName, 'N/A') AS [Responsible Person]
        FROM dbo.tblInvoice im WITH (NOLOCK)
        INNER JOIN dbo.tblCustMaster cm WITH (NOLOCK) 
            ON im.CustomerMasterId = cm.CustomerMasterId
        LEFT JOIN CTE_InvoiceSummary invS 
            ON im.InvoiceId = invS.InvoiceId
        LEFT JOIN CTE_PaymentSummary payS 
            ON im.InvoiceId = payS.InvoiceId
        LEFT JOIN dbo.tblEmpGeneralInfo emp WITH (NOLOCK) 
            ON cm.SalesPersonId = emp.EmpInfoId
        WHERE 
            CAST(im.InvoiceDate AS DATE) <= @AsOnDate
            AND (@FromDate IS NULL OR CAST(im.InvoiceDate AS DATE) >= @FromDate)
            AND (@ToDate IS NULL OR CAST(im.InvoiceDate AS DATE) <= @ToDate)
            AND (@ComUnitId IS NULL OR @ComUnitId = 0 OR im.ComUnitId = @ComUnitId)
            AND (@CustomerMasterId IS NULL OR @CustomerMasterId = 0 OR im.CustomerMasterId = @CustomerMasterId)
            AND (@SalesPersonId IS NULL OR @SalesPersonId = 0 OR cm.SalesPersonId = @SalesPersonId)
    )
    SELECT 
        ROW_NUMBER() OVER (ORDER BY [InvoiceDate_Raw] ASC, [Invoice No] ASC) AS [SL],
        [Customer Name],
        [Invoice No],
        [Invoice Date],
        [Invoice Amount],
        [Paid Amount],
        [Credit Days],
        [Outstanding],
        [Responsible Person]
    FROM CTE_BaseData
    WHERE [Outstanding] > 0.00
    ORDER BY [InvoiceDate_Raw] ASC, [Invoice No] ASC;
END
GO
PRINT 'Updated sp_Report_CustomerInvoiceAgingOutstanding.';
GO


-- 3.2 sp_GET_PaymentInvSP
CREATE OR ALTER PROCEDURE [dbo].[sp_GET_PaymentInvSP]
	@param NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SET @param = ISNULL(@param, '');

    DECLARE @Q NVARCHAR(MAX) = N'
    SELECT 
        ord.TerritoryName_Ord,   
        ((ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) - ISNULL(tblinvDetls.PaymentTotalPriceVatAmount, 0)) - ISNULL(TPAmount, 0) AS TP_Pay,
        -ISNULL(VATAmount, 0) AS Vat_Pay, 
        ord.OrderCode AS OrderNo,
        CONVERT(VARCHAR, ord.SubmissionDate, 103) AS OrderDate, 
        ord.CustomerCode,
        ord.CustomerName,  
        ord.MarketId,
        ord.DistributionRouteId,  
        ord.CustomerMasterId,  
        INV.InvoiceId,
        INV.InvoiceNo,
        INV.InvoiceDate,
        INV.PaymentInvoiceNo AS RtnInvoiceNo,
        INV.PaymentDate AS RtnInvoiceDate,   
        (ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) AS TotalDelivery,
        ISNULL(PP, 0) AS PaymentAmount,  
        ((ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) - ISNULL(PP, 0)) AS Due,
        ISNULL(ReturnTotal, 0) AS AdjustableAmount, 
        ISNULL(tblCompanyUnit.CompanyId, 16) AS CompanyId, 
        ISNULL(tblPaymentType.PaymentTypeName, ''Credit'') AS PaymentTypeName 
    FROM tblInvoice AS INV WITH(NOLOCK)
    LEFT JOIN tblOrder real_ord WITH(NOLOCK) ON INV.OrderId = real_ord.OrderId
    LEFT JOIN tblCustMaster C WITH(NOLOCK) ON C.CustomerMasterId = INV.CustomerMasterId
    CROSS APPLY (
        SELECT 
            ISNULL(real_ord.CustomerMasterId, INV.CustomerMasterId) AS CustomerMasterId,
            ISNULL(real_ord.ComUnitId, INV.ComUnitId) AS ComUnitId,
            ISNULL(real_ord.CustomerCode, C.CustomerCode) AS CustomerCode,
            ISNULL(real_ord.CustomerName, C.CustomerName) AS CustomerName,
            ISNULL(real_ord.MarketId, C.MarketId) AS MarketId,
            ISNULL(real_ord.DistributionRouteId, C.DistributionRouteId) AS DistributionRouteId,
            ISNULL(real_ord.TerritoryId, C.TerritoryId) AS TerritoryId,
            ISNULL(real_ord.TerritoryName_Ord, ISNULL(C.MarketName, '''')) AS TerritoryName_Ord,
            ISNULL(real_ord.OrderCode, ISNULL(INV.OrderNo, ''Opening'')) AS OrderCode,
            ISNULL(real_ord.SubmissionDate, INV.InvoiceDate) AS SubmissionDate
    ) AS ord
    LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = INV.ComUnitId
    LEFT JOIN dbo.tblPaymentType ON INV.PaymentTypeId = tblPaymentType.PaymentTypeId
    LEFT JOIN (
        SELECT InvoiceId, 
               ISNULL(SUM(PaymentAmount) + SUM(ISNULL(Discount, 0)) + SUM(ISNULL(AIT, 0)), 0) AS PP, 
               SUM(TPAmount) AS TPAmount, 
               SUM(VATAmount) AS VATAmount 
        FROM tblCustPayDetail 
        GROUP BY InvoiceId
    ) AS P ON INV.InvoiceId = P.InvoiceId 
    INNER JOIN (
        SELECT InvoiceId, SUM(PaymentNetAmount) AS TotalDelivery 
        FROM tblInvoiceDetail WITH(NOLOCK) 
        GROUP BY InvoiceId
    ) AS TD ON INV.InvoiceId = TD.InvoiceId 
    LEFT JOIN (
        SELECT InvoiceId, SUM(TPGrandTotal) AS ReturnTotal 
        FROM tblReturnInvoice  
        GROUP BY InvoiceId
    ) AS RTN ON INV.InvoiceId = RTN.InvoiceId
    INNER JOIN (
        SELECT InvoiceId, SUM(PaymentTotalPriceVatAmount) AS PaymentTotalPriceVatAmount, SUM(PaymentTotalPrice) AS PaymentTotalPrice 
        FROM tblInvoiceDetail WITH(NOLOCK)  
        GROUP BY InvoiceId
    ) tblinvDetls ON tblinvDetls.InvoiceId = INV.InvoiceId
    WHERE (((ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) - ISNULL(PP, 0))) > 0 
      AND INV.PaymentInvoiceNo IS NOT NULL  
      AND INV.SndReturnInvoiceNo IS NULL ' + @param + ' ORDER BY InvoiceDate ASC';

    EXEC sp_executesql @Q;
END;
GO
PRINT 'Updated sp_GET_PaymentInvSP.';
GO


-- 3.3 sp_GET_PaymentInvSPTPVATAmt
CREATE OR ALTER PROCEDURE [dbo].[sp_GET_PaymentInvSPTPVATAmt]
	@InvoiceId int,
	@PayAmount decimal(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ((ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) - ISNULL(tblinvDetls.PaymentTotalPriceVatAmount, 0)) - ISNULL(TPAmount, 0) AS TP_Pay,
        ISNULL(tblinvDetls.PaymentTotalPriceVatAmount, 0) - ISNULL(VATAmount, 0) AS Vat_Pay  
    FROM tblInvoice AS INV WITH(NOLOCK)
    INNER JOIN tblOrder ord WITH(NOLOCK) ON INV.OrderId = ord.OrderId
    INNER JOIN tblCustMaster C ON C.CustomerMasterId = ord.CustomerMasterId
    LEFT JOIN (
        SELECT InvoiceId, 
               ISNULL(SUM(PaymentAmount) + SUM(ISNULL(Discount, 0)) + SUM(ISNULL(AIT, 0)), 0) AS PP, 
               SUM(TPAmount) AS TPAmount, 
               SUM(VATAmount) AS VATAmount 
        FROM tblCustPayDetail 
        GROUP BY InvoiceId
    ) AS P ON INV.InvoiceId = P.InvoiceId 
    INNER JOIN (
        SELECT InvoiceId, SUM(PaymentNetAmount) AS TotalDelivery 
        FROM tblInvoiceDetail AS IVD WITH(NOLOCK) 
        GROUP BY InvoiceId
    ) AS TD ON INV.InvoiceId = TD.InvoiceId 
    LEFT JOIN (
        SELECT InvoiceId, SUM(TPGrandTotal) AS ReturnTotal 
        FROM tblReturnInvoice  
        GROUP BY InvoiceId
    ) AS RTN ON INV.InvoiceId = RTN.InvoiceId
    INNER JOIN (
        SELECT InvoiceId, SUM(PaymentTotalPriceVatAmount) AS PaymentTotalPriceVatAmount, SUM(PaymentTotalPrice) AS PaymentTotalPrice 
        FROM tblInvoiceDetail 
        GROUP BY InvoiceId
    ) tblinvDetls ON tblinvDetls.InvoiceId = INV.InvoiceId
    WHERE PaymentInvoiceNo IS NOT NULL AND INV.InvoiceId = @InvoiceId;
END;
GO
PRINT 'Updated sp_GET_PaymentInvSPTPVATAmt.';
GO


-- 3.4 sp_GET_PaymentInvSPPaymentAmount
CREATE OR ALTER PROCEDURE [dbo].[sp_GET_PaymentInvSPPaymentAmount]
	@param nvarchar(max),
	@PaymentAmount nvarchar(max),
	@CollectionBy nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;
    SET @param = ISNULL(@param, '');
   
    DECLARE @Q NVARCHAR(MAX) = '
    SELECT 
        ''' + CAST(@CollectionBy AS NVARCHAR(MAX)) + ''' AS CollectionBy, 
        0 AS TP_Pay, 
        0 AS Vat_Pay, 
        ord.ComUnitId, 
        ' + @PaymentAmount + ' AS PayNewAmount, 
        ord.OrderCode AS OrderNo,
        CONVERT(VARCHAR, ord.SubmissionDate, 103) AS OrderDate, 
        C.CustomerCode,
        C.CustomerName,  
        ord.MarketId,
        ord.DistributionRouteId,  
        ord.CustomerMasterId,  
        INV.InvoiceId,
        INV.InvoiceNo,
        INV.InvoiceDate,
        INV.PaymentInvoiceNo AS RtnInvoiceNo,
        INV.PaymentDate AS RtnInvoiceDate,
        (ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) AS TotalDelivery,
        ISNULL(PP, 0) AS PaymentAmount,
        ((ISNULL(TotalDelivery, 0) + ISNULL(INV.DeliveryCharge, 0)) - ISNULL(PP, 0)) AS Due,
        ISNULL(ReturnTotal, 0) AS AdjustableAmount 
    FROM tblInvoice AS INV WITH(NOLOCK)
    INNER JOIN tblOrder ord WITH(NOLOCK) ON INV.OrderId = ord.OrderId
    INNER JOIN tblCustMaster C ON C.CustomerMasterId = ord.CustomerMasterId
    LEFT JOIN (
        SELECT InvoiceId, 
               ISNULL(SUM(PaymentAmount) + SUM(ISNULL(Discount, 0)) + SUM(ISNULL(AIT, 0)), 0) AS PP 
        FROM tblCustPayDetail 
        GROUP BY InvoiceId
    ) AS P ON INV.InvoiceId = P.InvoiceId 
    INNER JOIN (
        SELECT InvoiceId, SUM(PaymentNetAmount) AS TotalDelivery 
        FROM tblInvoiceDetail AS IVD WITH(NOLOCK) 
        GROUP BY InvoiceId
    ) AS TD ON INV.InvoiceId = TD.InvoiceId 
    LEFT JOIN (
        SELECT InvoiceId, SUM(TPGrandTotal) AS ReturnTotal 
        FROM tblReturnInvoice  
        GROUP BY InvoiceId
    ) AS RTN ON INV.InvoiceId = RTN.InvoiceId
    WHERE PaymentInvoiceNo IS NOT NULL 
      AND (PaymentStatus IS NULL OR PaymentStatus = ''Partial'') ' + @param + ' 
    ORDER BY InvoiceDate ASC';

    EXEC sp_executesql @Q;
END;
GO
PRINT 'Updated sp_GET_PaymentInvSPPaymentAmount.';
GO


-- 3.5 sp_Sales_CustomerPaymentPosting
CREATE OR ALTER PROCEDURE [dbo].[sp_Sales_CustomerPaymentPosting]
	@CustPayId INT,
	@InvoiceId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @MyCursor2 CURSOR;
	DECLARE @BiusinessUnitId INT, @FinancialYearId INT, @DeliveryNetAmount DECIMAL(18, 4),
			@Discount DECIMAL(18, 4), @AIT DECIMAL(18, 4), @TableId INT, @InvoiceId2 INT, @VoucherMasterId INT,
			@BankId INT, @CashId INT, @SubsidiaryInfoId BIGINT, @ReferenceInfoId BIGINT;
	DECLARE @ApprovalDate DATETIME, @InvoiceNo NVARCHAR(MAX), @CustomerName NVARCHAR(MAX);
	DECLARE @VoucherCode NVARCHAR(100), @VoucherType NVARCHAR(100) = 'Journal';
	DECLARE @CurrencyId INT = 1, @Conversionrate DECIMAL(18, 4) = 1.0000;

	SET @MyCursor2 = CURSOR FOR
		SELECT DISTINCT CI.CompanyId, FY.FinancialYearId, FORMAT(CPY.PaymentDate, 'dd-MMM-yyyy') AS UpdateDate,
			CPD.PaymentAmount, ISNULL(CPD.Discount, 0) AS Discount, ISNULL(CPD.AIT, 0) AS AIT, TBL.IntegrationTableId, CPY.CustPayId,
			CMS.CustomerName + ' (' + CMS.CustomerCode + ')' AS CustomerName, InvoiceNo, CPD.BankAccId, CPD.CashAccId,
			ISNULL(SB.SubsidiaryInfoId, 0) AS SubsidiaryInfoId, ISNULL(RF.ReferenceInfoId, 0) AS ReferenceInfoId  
		FROM tblCustPayDetail AS CPD 
        LEFT JOIN dbo.tblCustomerPay AS CPY ON CPD.CustPayId = CPY.CustPayId
        LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = CPD.InvoiceId
        INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = tblInvoice.ComUnitId
		INNER JOIN CSTL_ERPDB_Unicorn..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		LEFT JOIN tblCustMaster AS CMS ON tblInvoice.CustomerMasterId = CMS.CustomerMasterId
		LEFT JOIN (SELECT S_Info_Id, SubsidiaryInfoId FROM CSTL_ACCDB_Unicorn..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON tblInvoice.CustomerMasterId = SB.S_Info_Id
		LEFT JOIN CSTL_ACCDB_Unicorn..tblReferenceInfo AS RF ON RF.ReferenceTypeId = 1016 AND R_Info_Id = @InvoiceId 
		INNER JOIN (SELECT CompanyId, FinancialYearId, StartDate, EndDate FROM CSTL_ACCDB_Unicorn..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND CPY.PaymentDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB_Unicorn..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblCustomerPay'))) 	
		LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS V ON TableID = 2017 AND CPY.CustPayId = V.CommonPK_ID
		WHERE (CPD.PaymentAmount + ISNULL(CPD.Discount, 0) + ISNULL(CPD.AIT, 0)) > 0 
		  AND CPD.InvoiceId = @InvoiceId 
		  AND CPD.CustPayId = @CustPayId;

	OPEN @MyCursor2;
	FETCH NEXT FROM @MyCursor2                               
	INTO @BiusinessUnitId, @FinancialYearId, @ApprovalDate, @DeliveryNetAmount, @Discount, @AIT, @TableId, @InvoiceId2,
		 @CustomerName, @InvoiceNo, @BankId, @CashId, @SubsidiaryInfoId, @ReferenceInfoId;
		
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @yearText NVARCHAR(MAX) = SUBSTRING(CONVERT(NVARCHAR(MAX), YEAR(@ApprovalDate)), 3, 3);   
		DECLARE @monthText NVARCHAR(MAX) = (CASE WHEN LEN(MONTH(@ApprovalDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX), MONTH(@ApprovalDate)) END);
		DECLARE @dateText NVARCHAR(MAX) = (CASE WHEN LEN(DAY(@ApprovalDate)) = 1 THEN '0' + CONVERT(NVARCHAR(MAX), DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX), DAY(@ApprovalDate)) END);
		
		SELECT @VoucherCode = (@yearText + @monthText + @dateText) + CONVERT(NVARCHAR(MAX), CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 6)), 10000)) + 1)  
		FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster 
		WHERE CONVERT(DATE, PostingDate) = CONVERT(DATE, @ApprovalDate);

		SET @VoucherType = CASE 
            WHEN @BankId IS NOT NULL AND @BankId != 0 THEN 'CreditVoucherBank'
            WHEN @CashId != 0 AND @CashId IS NOT NULL THEN 'CreditVoucherCash'
        END;
	
		IF (@VoucherType = 'CreditVoucherBank') SET @VoucherCode = 'CVB-' + @VoucherCode;	
		IF (@VoucherType = 'CreditVoucherCash') SET @VoucherCode = 'CVC-' + @VoucherCode;
		IF (@VoucherType = 'Journal') SET @VoucherCode = 'JVP-' + @VoucherCode;

		INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster
		( 
			VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
			CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
			TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
			ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy,
			ApprovedDate, CommonPK_ID, TableID
		)
		VALUES  
		( 
			@VoucherCode, @BiusinessUnitId, @FinancialYearId, @ApprovalDate, 2,
			@CurrencyId, @Conversionrate,
			'An amount of BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + 
			CASE WHEN @Discount > 0 THEN ', including a discount of BDT ' + CONVERT(NVARCHAR(50), @Discount) ELSE '' END + 
			CASE WHEN @AIT > 0 THEN ', and an AIT of BDT ' + CONVERT(NVARCHAR(50), @AIT) ELSE '' END + 
			' has been received from customer "' + @CustomerName + '" against Invoice No. "' + @InvoiceNo + '" on ' + CONVERT(NVARCHAR(50), @ApprovalDate, 105) + '.',
			0, 0, @DeliveryNetAmount + @Discount + @AIT, @DeliveryNetAmount + @Discount + @AIT,
			'Auto Posting', 'admin', @ApprovalDate, @VoucherType, 'Approved',
			0, 0, 0, 'Auto Process', @ApprovalDate, @InvoiceId2, @TableId
		);
		
		SET @VoucherMasterId = SCOPE_IDENTITY();

		-- Bank / Cash GL
		DECLARE @DrGl INT = 0;
        IF (@BankId IS NOT NULL AND @BankId != 0)
            SELECT @DrGl = AccountId FROM CSTL_ACCDB_Unicorn..tblChartOfAccounts WHERE BankAccountId = @BankId;
        IF (@CashId IS NOT NULL AND @CashId != 0)
            SELECT @DrGl = AccountId FROM CSTL_ACCDB_Unicorn..tblChartOfAccounts WHERE CashAccountId = @CashId;

		-- Debit Side 1: Cash / Bank
		IF (ISNULL(@DeliveryNetAmount, 0) > 0)
		BEGIN
			INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd) 
			VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Customer Payment', @ReferenceInfoId);
		END

		-- Debit Side 2: Discount
		IF (@Discount > 0)
		BEGIN
			DECLARE @DrGl1 INT = 0;
			SELECT @DrGl1 = GLId FROM CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1023;
			INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd) 
			VALUES (@VoucherMasterId, @DrGl1, 'Debit', 0.00, 0.00, @Discount, 0.00, 'Customer Payment', @ReferenceInfoId);
		END

		-- Debit Side 3: AIT
		IF (@AIT > 0)
		BEGIN
			DECLARE @DrGl2 INT = 0;
			SELECT @DrGl2 = GLId FROM CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1024;
			INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd) 
			VALUES (@VoucherMasterId, @DrGl2, 'Debit', 0.00, 0.00, @AIT, 0.00, 'Customer Payment', @ReferenceInfoId);
		END

		-- Credit Side: Accounts Receivable
		DECLARE @CrGl INT = 0;
		SELECT @CrGl = GLId FROM CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2;
		
		INSERT INTO CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks, SubsidoryLedgerId, RefInfoInd)
		VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, (@DeliveryNetAmount + @AIT + @Discount), 0, 0, 'Customer Payment', @SubsidiaryInfoId, 0);

		FETCH NEXT FROM @MyCursor2
		INTO @BiusinessUnitId, @FinancialYearId, @ApprovalDate, @DeliveryNetAmount, @Discount, @AIT, @TableId, @InvoiceId2,
			 @CustomerName, @InvoiceNo, @BankId, @CashId, @SubsidiaryInfoId, @ReferenceInfoId;
	END;

	CLOSE @MyCursor2;
	DEALLOCATE @MyCursor2;

	-- COGS POSTING: If Full Payment (DueAmount <= 0)
	DECLARE @DueAmount DECIMAL(18, 2);
 
	SELECT 
	    @DueAmount = ((ISNULL(INV.DeliveryTpGrandTotal, 0) + ISNULL(INV.DeliveryCharge, 0)) - ISNULL(CPD.PaidAmount, 0))
	FROM tblInvoice AS INV
	LEFT JOIN (
	    SELECT InvoiceId, SUM(ISNULL(PaymentAmount, 0) + ISNULL(Discount, 0) + ISNULL(AIT, 0)) AS PaidAmount
	    FROM tblCustPayDetail
	    WHERE InvoiceId = @InvoiceId
	    GROUP BY InvoiceId
	) AS CPD ON INV.InvoiceId = CPD.InvoiceId
	WHERE INV.InvoiceId = @InvoiceId;
 
	IF ISNULL(@DueAmount, 0) <= 0
	BEGIN
	    EXEC sp_Sales_DeliveryCOGSPosting @InvoiceId = @InvoiceId;
	END

END
GO
PRINT 'Updated sp_Sales_CustomerPaymentPosting.';
GO


-- 3.6 sp_DeliverySalesReturnPosting
CREATE OR ALTER PROCEDURE [dbo].[sp_DeliverySalesReturnPosting]
    @ReturnId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT = 1;
    DECLARE @FinancialYearId INT;
    DECLARE @PostingDate DATETIME = CAST(GETDATE() AS DATE);
    DECLARE @VoucherCode NVARCHAR(100);
    DECLARE @VoucherMasterId INT;
    DECLARE @InvoiceId INT;
    DECLARE @InvoiceNo NVARCHAR(100);
    DECLARE @CustomerName NVARCHAR(500);
    DECLARE @CustomerMasterId INT;
    DECLARE @ComUnitId INT;
    DECLARE @SubsidiaryInfoId BIGINT = 0;
    DECLARE @ReferenceInfoId BIGINT = 0;
    DECLARE @TotalReturnAmount DECIMAL(18, 4) = 0.00;
    DECLARE @DeductionCharge DECIMAL(18, 4) = 0.00;
    DECLARE @NetReturnAmount DECIMAL(18, 4) = 0.00;
    DECLARE @StockValue DECIMAL(18, 4) = 0.00;
    DECLARE @TableId INT = 2018;
    DECLARE @TableIdInv INT = 2023;

    -- 1. Fetch Return Master Data
    SELECT 
        @InvoiceId          = CRM.InvoiceId,
        @InvoiceNo          = INV.InvoiceNo,
        @CustomerMasterId   = INV.CustomerMasterId,
        @ComUnitId          = INV.ComUnitId,
        @CustomerName       = CMS.CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' ELSE '' END,
        @TotalReturnAmount  = ISNULL(CRM.TotalAmount, 0.00),
        @DeductionCharge    = ISNULL(CRM.DeductionCharge, 0.00),
        @NetReturnAmount    = ISNULL(CRM.NetReturnAmount, (ISNULL(CRM.TotalAmount, 0.00) - ISNULL(CRM.DeductionCharge, 0.00)))
    FROM dbo.tblCustomerReturnMaster AS CRM WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice AS INV WITH (NOLOCK) ON CRM.InvoiceId = INV.InvoiceId
    LEFT JOIN dbo.tblCustMaster AS CMS WITH (NOLOCK) ON INV.CustomerMasterId = CMS.CustomerMasterId
    WHERE CRM.CustReturnId = @ReturnId;

    IF (@InvoiceId IS NULL OR @TotalReturnAmount <= 0)
        RETURN;

    -- Ensure NetReturnAmount is consistent
    IF (@NetReturnAmount <= 0 AND @TotalReturnAmount > 0)
        SET @NetReturnAmount = @TotalReturnAmount - @DeductionCharge;

    -- Get Company ID
    SELECT TOP 1 @CompanyId = CI.CompanyId
    FROM tblCompanyUnit UNT
    INNER JOIN CSTL_ERPDB_Unicorn..tblCompanyInfo CI ON UNT.CompanyId = CI.CompanyId
    WHERE UNT.ComUnitId = @ComUnitId;

    IF (@CompanyId IS NULL) SET @CompanyId = 1;

    -- 2. Get Active Financial Year
    SELECT TOP 1 @FinancialYearId = FinancialYearId
    FROM CSTL_ACCDB_Unicorn.dbo.tblFinancialYear
    WHERE (Status = 'Active' OR @PostingDate BETWEEN StartDate AND EndDate)
      AND CompanyId = @CompanyId
    ORDER BY CASE WHEN Status = 'Active' THEN 1 ELSE 2 END, FinancialYearId DESC;

    IF (@FinancialYearId IS NULL) SET @FinancialYearId = 2;

    -- 3. Get Subsidiary & Reference Info
    SELECT TOP 1 @SubsidiaryInfoId = SubsidiaryInfoId
    FROM CSTL_ACCDB_Unicorn.dbo.tblSubsidiaryInfo
    WHERE SubsidiaryTypeId = 1 AND S_Info_Id = @CustomerMasterId;

    SELECT TOP 1 @ReferenceInfoId = ReferenceInfoId
    FROM CSTL_ACCDB_Unicorn.dbo.tblReferenceInfo
    WHERE ReferenceTypeId = 1016 AND R_Info_Id = @InvoiceId;

    -- =========================================================================
    -- VOUCHER 1: SALES RETURN JOURNAL VOUCHER (Revenue & Receivable Reversal)
    -- =========================================================================
    DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@PostingDate)), 3, 2);
    DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CONVERT(NVARCHAR(10), MONTH(@PostingDate)), 2);
    DECLARE @dateText NVARCHAR(10) = RIGHT('0' + CONVERT(NVARCHAR(10), DAY(@PostingDate)), 2);

    SELECT @VoucherCode = 'JVP-' + (@yearText + @monthText + @dateText) + 
           CONVERT(NVARCHAR(10), CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 6)), 10000)) + 1)
    FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
    WHERE CONVERT(DATE, PostingDate) = @PostingDate;

    INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
    (
        VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
        CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
        TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
        ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy,
        ApprovedDate, CommonPK_ID, TableID, SalesReturnId
    )
    VALUES
    (
        @VoucherCode, @CompanyId, @FinancialYearId, @PostingDate, 2,
        1, 1.0000,
        'Sales Return from customer "' + @CustomerName + '" against Invoice No. "' + @InvoiceNo + '" for BDT ' + 
        CONVERT(NVARCHAR(50), @TotalReturnAmount) + 
        CASE WHEN @DeductionCharge > 0 THEN ', with Return Delivery Charge deduction of BDT ' + CONVERT(NVARCHAR(50), @DeductionCharge) ELSE '' END + '.',
        0, 0,
        @TotalReturnAmount, @TotalReturnAmount,
        'Auto Posting', 'admin', @PostingDate, 'Journal',
        'Approved', 0, 0, 0, 'Auto Process',
        @PostingDate, @ReturnId, @TableId, @ReturnId
    );

    SET @VoucherMasterId = SCOPE_IDENTITY();

    -- Debit Side: Sales Return GL (PageId = 23, FlagId = 1020)
    DECLARE @DrGlReturn INT = 0;
    SELECT TOP 1 @DrGlReturn = GLId 
    FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
    WHERE PageId = 23 AND FlagId = 1020;

    IF (@DrGlReturn IS NULL OR @DrGlReturn = 0) SET @DrGlReturn = 4170;

    INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail
    (
        VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount,
        DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd
    )
    VALUES
    (
        @VoucherMasterId, @DrGlReturn, 'Debit', 0.00, 0.00,
        @TotalReturnAmount, 0.00, 'Sales Return', @ReferenceInfoId
    );

    -- Credit Side 1: Delivery Income (if deduction exists) (PageId = 23, FlagId = 1032)
    IF (@DeductionCharge > 0)
    BEGIN
        DECLARE @CrGlDeliveryIncome INT = 0;
        SELECT TOP 1 @CrGlDeliveryIncome = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 23 AND FlagId = 1032;

        IF (@CrGlDeliveryIncome IS NULL OR @CrGlDeliveryIncome = 0) SET @CrGlDeliveryIncome = 5968;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail
        (
            VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount,
            DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd
        )
        VALUES
        (
            @VoucherMasterId, @CrGlDeliveryIncome, 'Credit', 0.00, 0.00,
            0.00, @DeductionCharge, 'Return Delivery Charge Income', @ReferenceInfoId
        );
    END

    -- Credit Side 2: Accounts Receivable / Customer Subsidiary (PageId = 23, FlagId = 2)
    DECLARE @CrGlAR INT = 0;
    SELECT TOP 1 @CrGlAR = GLId 
    FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
    WHERE PageId = 23 AND FlagId = 2;

    IF (@CrGlAR IS NULL OR @CrGlAR = 0) SET @CrGlAR = 2112;

    INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail
    (
        VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount,
        DebitAmountBDT, CreditAmountBDT, Remarks, SubsidoryLedgerId, RefInfoInd
    )
    VALUES
    (
        @VoucherMasterId, @CrGlAR, 'Credit', 0.00, 0.00,
        0.00, @NetReturnAmount, 'Sales Return Receivable Adjustment', @SubsidiaryInfoId, @ReferenceInfoId
    );

    -- =========================================================================
    -- VOUCHER 2: INVENTORY VALUATION / COGS REVERSAL VOUCHER
    -- =========================================================================
    SELECT @StockValue = SUM(ISNULL((CTRS1.UnitPrice * RTND.ReturnQty) + (CTRS1.VATPerUnit * RTND.ReturnQty), 0.00))
    FROM dbo.tblCustomerReturnMaster AS RTNM WITH (NOLOCK)
    INNER JOIN dbo.tblCustomerReturnDetail AS RTND WITH (NOLOCK) ON RTND.CustReturnId = RTNM.CustReturnId
    INNER JOIN dbo.tblInvoiceDetail AS INVD WITH (NOLOCK) ON INVD.InvoiceDetailId = RTND.InvoiceDetailId
    INNER JOIN dbo.tblDCStore AS DCS WITH (NOLOCK) ON DCS.DCStoreId = INVD.DCStoreId
    INNER JOIN dbo.tblCentralStore AS CTRS1 WITH (NOLOCK) ON DCS.TempReceiveId = CTRS1.ReceiveId
    WHERE RTNM.CustReturnId = @ReturnId;

    IF (ISNULL(@StockValue, 0.00) > 0.00)
    BEGIN
        DECLARE @VoucherCodeInv NVARCHAR(100);
        SELECT @VoucherCodeInv = 'JVP-' + (@yearText + @monthText + @dateText) + 
               CONVERT(NVARCHAR(10), CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 6)), 10000)) + 1)
        FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
        WHERE CONVERT(DATE, PostingDate) = @PostingDate;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
        (
            VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
            CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
            TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
            ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy,
            ApprovedDate, CommonPK_ID, TableID, SalesReturnId
        )
        VALUES
        (
            @VoucherCodeInv, @CompanyId, @FinancialYearId, @PostingDate, 2,
            1, 1.0000,
            'Delivery Sales Return, Inventory Valuation against Invoice No. "' + @InvoiceNo + '" (Stock Value: BDT ' + CONVERT(NVARCHAR(50), @StockValue) + ').',
            0, 0,
            @StockValue, @StockValue,
            'Auto Posting', 'admin', @PostingDate, 'Journal',
            'Approved', 0, 0, 0, 'Auto Process',
            @PostingDate, @ReturnId, @TableIdInv, @ReturnId
        );

        DECLARE @VoucherMasterIdInv INT = SCOPE_IDENTITY();

        -- Debit Side: Finished Goods Inventory (PageId = 23, FlagId = 1016)
        DECLARE @DrGlFG INT = 0;
        SELECT TOP 1 @DrGlFG = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 23 AND FlagId = 1016;

        IF (@DrGlFG IS NULL OR @DrGlFG = 0) SET @DrGlFG = 4156;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail
        (
            VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount,
            DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd
        )
        VALUES
        (
            @VoucherMasterIdInv, @DrGlFG, 'Debit', 0.00, 0.00,
            @StockValue, 0.00, 'Sales Return Stock In', @ReferenceInfoId
        );

        -- Credit Side: Cost of Goods Sold (PageId = 23, FlagId = 1021)
        DECLARE @CrGlCOGS INT = 0;
        SELECT TOP 1 @CrGlCOGS = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 23 AND FlagId = 1021;

        IF (@CrGlCOGS IS NULL OR @CrGlCOGS = 0) SET @CrGlCOGS = 4162;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail
        (
            VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount,
            DebitAmountBDT, CreditAmountBDT, Remarks, RefInfoInd
        )
        VALUES
        (
            @VoucherMasterIdInv, @CrGlCOGS, 'Credit', 0.00, 0.00,
            0.00, @StockValue, 'Sales Return COGS Reversal', @ReferenceInfoId
        );
    END

    -- Update Return Posting Flag
    UPDATE dbo.tblCustomerReturnMaster 
    SET IsPosting = 1 
    WHERE CustReturnId = @ReturnId;

END
GO
PRINT 'Updated sp_DeliverySalesReturnPosting.';
GO

-- ============================================================================
-- SECTION 4: MARKET SALES RETURN (SalesReturn.aspx) UPDATES
-- ============================================================================

-- 4.1 Schema Alteration: Add DeductionCharge and NetReturnAmount to tblReturnInvoice
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblReturnInvoice') AND name = 'DeductionCharge')
BEGIN
    ALTER TABLE dbo.tblReturnInvoice ADD DeductionCharge DECIMAL(18, 2) NULL DEFAULT 0.00;
    PRINT 'Added DeductionCharge column to tblReturnInvoice.';
END
ELSE
BEGIN
    PRINT 'DeductionCharge already exists in tblReturnInvoice.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblReturnInvoice') AND name = 'NetReturnAmount')
BEGIN
    ALTER TABLE dbo.tblReturnInvoice ADD NetReturnAmount DECIMAL(18, 2) NULL DEFAULT 0.00;
    PRINT 'Added NetReturnAmount column to tblReturnInvoice.';
END
ELSE
BEGIN
    PRINT 'NetReturnAmount already exists in tblReturnInvoice.';
END
GO

-- 4.2 Schema Alteration: Add ReturnInvoiceDetailId to tblDCStore
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblDCStore') AND name = 'ReturnInvoiceDetailId')
BEGIN
    ALTER TABLE dbo.tblDCStore ADD ReturnInvoiceDetailId INT NULL DEFAULT 0;
    PRINT 'Added ReturnInvoiceDetailId column to tblDCStore.';
END
ELSE
BEGIN
    PRINT 'ReturnInvoiceDetailId already exists in tblDCStore.';
END
GO

-- 4.2 Updated Stored Procedure spInsertReturnInvoice_new
CREATE OR ALTER PROCEDURE [dbo].[spInsertReturnInvoice_new]
(
    @ReturnInvoiceId INT,
    @InvoiceDate datetime=null,
    @OrderId int=null,
    @OrderNo nvarchar(50)=null,
    @OrderDate datetime=null,
    @CustomerMasterId int=null,
    @ComUnitId int=null,
    @MiaId int=null,
    @PaymentTypeId int=null,
    @TpTotal decimal(18,2)=null,
    @TpDiscount decimal(18,2)=null,
    @TpVat decimal(18,2)=null,
    @TpGrandTotal decimal(18,2)=null,
    @DeductionCharge decimal(18,2)=0,
    @NetReturnAmount decimal(18,2)=0,
  
    @DpNAme nvarchar(50)=null, 
    @createBy nvarchar(50)=null,
    @Createdate datetime=null,
    @TotalSpecialAmount decimal(18,2)=null,
    @ProductOffer bit=null,
    @OldTradePolicy bit=null,
    @Remarks nvarchar(max)=null,
    @FixedCustomer bit=null,
    @MIACode nvarchar(50)=null,
    @MIAName nvarchar(100)=null,
    @MarketCode nvarchar(50)=null,
    @MarketName nvarchar(100)=null,
    @AreaCode nvarchar(50)=null,
    @DisCode nvarchar(50)=null,
    @FEName nvarchar(100)=null,
    @RegionCode nvarchar(50)=null,
    @DZSMName nvarchar(100)=null,  
    @invoiceid int=null,
    @DpMob nvarchar(100)=null,
    @Type bit=null,
    @MIOId_new  int=null,
    @Terri_Id_new   int=null,
    @MioEmpId_new   int=null,
    @Mio_SapCode_New  nvarchar(100)=null
)
AS
BEGIN
    DECLARE @ReturnInvoiceNo nvarchar(50);

    DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@InvoiceDate)), 3, 3);   
    DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN LEN(MONTH(@InvoiceDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@InvoiceDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@InvoiceDate)) END);
    DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN LEN(DAY(@InvoiceDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@InvoiceDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@InvoiceDate)) END);

    SELECT @ReturnInvoiceNo='RIN--'+(@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(ReturnInvoiceId),10000))+1001) 
    FROM tblReturnInvoice 
    WHERE CONVERT(date,ReturnInvoiceDate)=CONVERT(date, @InvoiceDate);

    DECLARE @RegionId_Rtn INT=0, @AreaId_Rtn INT=0;
    SELECT @AreaId_Rtn=ar.AreaId, @RegionId_Rtn=rg.RegionId 
    FROM dbo.tblTerritory terry WITH (NOLOCK)
    INNER JOIN dbo.tblArea ar WITH (NOLOCK) ON ar.AreaId = terry.AreaId
    INNER JOIN dbo.tblRegion rg WITH (NOLOCK) ON rg.RegionId = ar.RegionId 
    WHERE terry.TerritoryId=@Terri_Id_new;

    INSERT INTO [dbo].[tblReturnInvoice]
    (
        ReturnInvoiceNo, ReturnInvoiceDate, CreateBy, CreateDate,
        OrderNo, OrderDate, CustomerMasterId, ComUnitId, MiaId, PaymentTypeId,
        TpTotal, TpDiscount, TpVat, TpGrandTotal, DeductionCharge, NetReturnAmount,
        OrderId, TotalSpecialAmount, OldTradePolicy, ProductOffer, Remarks,
        MIACode, MIAName, MarketCode, MarketName, AreaCode, DisCode, FEName,
        RegionCode, DZSMName, FixedCustomer, InvoiceId, IsSalesReturnWithoutOrder,
        SubInvoiceId, Types, [MIOId_new], [Terri_Id_new], [MioEmpId_new],
        [Mio_SapCode_New], RegionId_Rtn, AreaId_Rtn
    )
    VALUES
    (
        @ReturnInvoiceNo, @InvoiceDate, @CreateBy, @CreateDate,
        @OrderNo, @OrderDate, @CustomerMasterId, @ComUnitId, @MiaId, @PaymentTypeId,
        @TpTotal, @TpDiscount, @TpVat, ISNULL(@TpGrandTotal, @TpTotal), ISNULL(@DeductionCharge, 0), ISNULL(@NetReturnAmount, ISNULL(@TpGrandTotal, @TpTotal) - ISNULL(@DeductionCharge, 0)),
        @OrderId, @TotalSpecialAmount, @OldTradePolicy, @ProductOffer, @Remarks,
        @MIACode, @MIAName, @MarketCode, @MarketName, @AreaCode, @DisCode, @FEName,
        @RegionCode, @DZSMName, @FixedCustomer, @InvoiceId, 'No order',
        0, @Type, @MIOId_new, @Terri_Id_new, @MioEmpId_new,
        @Mio_SapCode_New, @RegionId_Rtn, @AreaId_Rtn
    );
       
    SELECT SCOPE_IDENTITY();
END;
GO
PRINT 'Updated spInsertReturnInvoice_new.';
GO

-- 4.3 Updated Stored Procedure sp_SalesReturnPosting
CREATE OR ALTER PROCEDURE dbo.sp_SalesReturnPosting
    @ReturnInvoiceId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT = 1;
    DECLARE @FinancialYearId INT;
    DECLARE @PostingDate DATETIME;
    DECLARE @VoucherCode NVARCHAR(100);
    DECLARE @VoucherMasterId INT;
    DECLARE @ReturnInvoiceNo NVARCHAR(100);
    DECLARE @ReturnNetAmount DECIMAL(18,4);
    DECLARE @DeductionCharge DECIMAL(18,4) = 0;
    DECLARE @NetReturnAmount DECIMAL(18,4) = 0;
    DECLARE @ReturnCostAmount DECIMAL(18,4);
    DECLARE @CustomerName NVARCHAR(500);
    DECLARE @CustomerMasterId INT;
    DECLARE @ComUnitId INT;
    DECLARE @SubsidiaryInfoId BIGINT = 0;
    DECLARE @ReferenceInfoId BIGINT = 0;
    DECLARE @TableIdSales INT = 2018; -- tblCustomerReturnMaster / tblReturnInvoice AR Table ID
    DECLARE @TableIdCOGS INT = 2023;  -- tblCustomerReturnMaster / tblReturnInvoice COGS Table ID

    -- 1. Fetch Return Invoice Header and Amounts
    SELECT 
        @ReturnInvoiceNo    = RINV.ReturnInvoiceNo,
        @PostingDate        = ISNULL(RINV.ReturnInvoiceDate, CAST(GETDATE() AS DATE)),
        @CustomerMasterId   = RINV.CustomerMasterId,
        @ComUnitId          = RINV.ComUnitId,
        @ReturnNetAmount    = ISNULL(RINV.TpGrandTotal, ISNULL(RINV.TpTotal, 0)),
        @DeductionCharge    = ISNULL(RINV.DeductionCharge, 0),
        @NetReturnAmount    = ISNULL(RINV.NetReturnAmount, 0),
        @CustomerName       = CMS.CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' ELSE '' END
    FROM tblReturnInvoice AS RINV WITH (NOLOCK)
    LEFT JOIN tblCustMaster AS CMS ON RINV.CustomerMasterId = CMS.CustomerMasterId
    WHERE RINV.ReturnInvoiceId = @ReturnInvoiceId;

    IF (@ReturnInvoiceNo IS NULL)
        RETURN;

    -- If ReturnNetAmount is 0, calculate from detail NetAmount
    IF (@ReturnNetAmount IS NULL OR @ReturnNetAmount <= 0)
    BEGIN
        SELECT @ReturnNetAmount = SUM(ISNULL(NetAmount, TotalPrice))
        FROM tblReturnInvoiceDetail
        WHERE ReturnInvoiceId = @ReturnInvoiceId;
    END

    -- If NetReturnAmount is not explicitly set, calculate it
    IF (@NetReturnAmount IS NULL OR @NetReturnAmount <= 0)
    BEGIN
        SET @NetReturnAmount = @ReturnNetAmount - @DeductionCharge;
        IF (@NetReturnAmount < 0) SET @NetReturnAmount = 0;
    END

    -- Calculate Cost Amount from detail with fallback to DCStore CostPerUnit using OUTER APPLY
    SELECT @ReturnCostAmount = SUM(
        ISNULL(RIND.TotalQuantity, RIND.Quantity) * 
        ISNULL(NULLIF(RIND.CostPrice, 0), ISNULL(DCS.CostPerUnit, 0))
    )
    FROM tblReturnInvoiceDetail AS RIND
    OUTER APPLY (
        SELECT TOP 1 CostPerUnit 
        FROM tblDCStore 
        WHERE ProductCode = RIND.ProductCode AND CostPerUnit > 0 
        ORDER BY DCStoreId DESC
    ) AS DCS
    WHERE RIND.ReturnInvoiceId = @ReturnInvoiceId;

    IF (@ReturnCostAmount IS NULL) SET @ReturnCostAmount = 0;

    -- 2. Get Active Financial Year
    SELECT TOP 1 @FinancialYearId = FinancialYearId
    FROM CSTL_ACCDB_Unicorn.dbo.tblFinancialYear
    WHERE (Status = 'Active' OR @PostingDate BETWEEN StartDate AND EndDate)
      AND CompanyId = @CompanyId
    ORDER BY CASE WHEN Status = 'Active' THEN 1 ELSE 2 END, FinancialYearId DESC;

    IF (@FinancialYearId IS NULL)
        SET @FinancialYearId = 2;

    -- 3. Customer Subsidiary Ledger
    SELECT TOP 1 @SubsidiaryInfoId = SubsidiaryInfoId
    FROM CSTL_ACCDB_Unicorn.dbo.tblSubsidiaryInfo
    WHERE SubsidiaryTypeId = 1 AND S_Info_Id = @CustomerMasterId;

    IF (@SubsidiaryInfoId IS NULL OR @SubsidiaryInfoId = 0) AND (@CustomerMasterId IS NOT NULL AND @CustomerMasterId > 0)
    BEGIN
        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblSubsidiaryInfo (SubsidiaryTypeId, S_Info_Id, IsSelect, Description, EntryBy, EntryDate)
        VALUES (1, @CustomerMasterId, 1, ISNULL(@CustomerName, 'Customer ' + CAST(@CustomerMasterId AS VARCHAR)), 1, GETDATE());
        SET @SubsidiaryInfoId = SCOPE_IDENTITY();
    END

    -- 4. Reference Info
    SELECT TOP 1 @ReferenceInfoId = ReferenceInfoId
    FROM CSTL_ACCDB_Unicorn.dbo.tblReferenceInfo
    WHERE ReferenceTypeId = 1018 AND R_Info_Id = @ReturnInvoiceId;

    IF (@ReferenceInfoId IS NULL OR @ReferenceInfoId = 0)
    BEGIN
        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblReferenceInfo (ReferenceTypeId, R_Info_Id, Description, EntryBy, EntryDate)
        VALUES (1018, @ReturnInvoiceId, 'Return Invoice ' + @ReturnInvoiceNo, 1, GETDATE());
        SET @ReferenceInfoId = SCOPE_IDENTITY();
    END

    -- Helper for Voucher Code formatting
    DECLARE @yearText NVARCHAR(10)  = SUBSTRING(CAST(YEAR(@PostingDate) AS NVARCHAR(10)), 3, 2);
    DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CAST(MONTH(@PostingDate) AS NVARCHAR(10)), 2);
    DECLARE @dateText NVARCHAR(10)  = RIGHT('0' + CAST(DAY(@PostingDate) AS NVARCHAR(10)), 2);
    DECLARE @MaxVoucherNum INT;

    -- =========================================================================
    -- VOUCHER 1: SALES RETURN / AR REVERSAL VOUCHER (TableID = 2018, IsValuation = 0)
    -- =========================================================================
    IF NOT EXISTS (
        SELECT 1 FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster 
        WHERE TableID = @TableIdSales AND CommonPK_ID = @ReturnInvoiceId AND (IsInventoryValuation = 0 OR IsInventoryValuation IS NULL)
    ) AND (@ReturnNetAmount > 0)
    BEGIN
        SELECT @MaxVoucherNum = ISNULL(MAX(CAST(RIGHT(VoucherCode, 5) AS INT)), 10000)
        FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
        WHERE CAST(PostingDate AS DATE) = @PostingDate AND VoucherCode LIKE 'JVP-%';

        SET @VoucherCode = 'JVP-' + @yearText + @monthText + @dateText + CAST(@MaxVoucherNum + 1 AS NVARCHAR(20));

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
        (
            VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
            CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
            TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
            ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy,
            ApprovedDate, CommonPK_ID, TableID, IsInventoryValuation, SalesReturnId
        )
        VALUES
        (
            @VoucherCode, @CompanyId, @FinancialYearId, @PostingDate, 2,
            1, 1.0000,
            'Market Sales Return from ' + ISNULL(@CustomerName, 'Customer') + ' under Return Invoice ' + @ReturnInvoiceNo + ' for BDT ' + CAST(@ReturnNetAmount AS NVARCHAR(50)) + 
            CASE WHEN @DeductionCharge > 0 THEN ', with Return Delivery Charge deduction of BDT ' + CAST(@DeductionCharge AS NVARCHAR(50)) ELSE '' END + 
            ', recorded as Accounts Receivable Reversal.',
            0, 0,
            @ReturnNetAmount, @ReturnNetAmount, 'Market Sales Return Auto Posting', 1, GETDATE(), 'Journal',
            'Approved', 0, 0, 0, 'Auto Process',
            @PostingDate, @ReturnInvoiceId, @TableIdSales, 0, @ReturnInvoiceId
        );

        SET @VoucherMasterId = SCOPE_IDENTITY();

        -- Debit: Sales Return GL (PageId = 23, FlagId = 1020, fallback 4170)
        DECLARE @DrSalesRtnGl INT = 0;
        SELECT TOP 1 @DrSalesRtnGl = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 23 AND FlagId = 1020;
        IF (@DrSalesRtnGl IS NULL OR @DrSalesRtnGl = 0) SET @DrSalesRtnGl = 4170;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail 
        (
            VoucherMasterId, AccountId, DebitorCredit,
            DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
            Remarks, SubsidoryLedgerId, RefInfoInd
        ) 
        VALUES 
        (
            @VoucherMasterId, @DrSalesRtnGl, 'Debit',
            0.00, 0.00, @ReturnNetAmount, 0.00,
            'Sales Return', 0, ISNULL(@ReferenceInfoId, 0)
        );

        -- Credit: Delivery / Deduction Income GL (PageId = 23, FlagId = 1032, fallback 5968)
        IF (@DeductionCharge > 0)
        BEGIN
            DECLARE @CrDeliveryIncomeGl INT = 0;
            SELECT TOP 1 @CrDeliveryIncomeGl = GLId 
            FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
            WHERE PageId = 23 AND FlagId = 1032;
            IF (@CrDeliveryIncomeGl IS NULL OR @CrDeliveryIncomeGl = 0) SET @CrDeliveryIncomeGl = 5968;

            INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail 
            (
                VoucherMasterId, AccountId, DebitorCredit,
                DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
                Remarks, SubsidoryLedgerId, RefInfoInd
            ) 
            VALUES 
            (
                @VoucherMasterId, @CrDeliveryIncomeGl, 'Credit',
                0.00, 0.00, 0.00, @DeductionCharge,
                'Return Delivery / Deduction Income', 0, ISNULL(@ReferenceInfoId, 0)
            );
        END

        -- Credit: Accounts Receivable GL (PageId = 163, FlagId = 2, fallback 2112)
        DECLARE @CrARGl INT = 0;
        SELECT TOP 1 @CrARGl = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 163 AND FlagId = 2;
        IF (@CrARGl IS NULL OR @CrARGl = 0) SET @CrARGl = 2112;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail 
        (
            VoucherMasterId, AccountId, DebitorCredit,
            DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
            Remarks, SubsidoryLedgerId, RefInfoInd
        ) 
        VALUES 
        (
            @VoucherMasterId, @CrARGl, 'Credit',
            0.00, 0.00, 0.00, @NetReturnAmount,
            'Receivable Reversal - Net Credit to Customer', ISNULL(@SubsidiaryInfoId, 0), 0
        );
    END

    -- =========================================================================
    -- VOUCHER 2: INVENTORY VALUATION & COGS REVERSAL (TableID = 2023, IsValuation = 1)
    -- =========================================================================
    IF NOT EXISTS (
        SELECT 1 FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster 
        WHERE TableID = @TableIdCOGS AND CommonPK_ID = @ReturnInvoiceId AND IsInventoryValuation = 1
    ) AND (@ReturnCostAmount > 0)
    BEGIN
        SELECT @MaxVoucherNum = ISNULL(MAX(CAST(RIGHT(VoucherCode, 5) AS INT)), 10000)
        FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
        WHERE CAST(PostingDate AS DATE) = @PostingDate AND VoucherCode LIKE 'JVP-%';

        SET @VoucherCode = 'JVP-' + @yearText + @monthText + @dateText + CAST(@MaxVoucherNum + 1 AS NVARCHAR(20));

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster
        (
            VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
            CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
            TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
            ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy,
            ApprovedDate, CommonPK_ID, TableID, IsInventoryValuation, SalesReturnId
        )
        VALUES
        (
            @VoucherCode, @CompanyId, @FinancialYearId, @PostingDate, 2,
            1, 1.0000,
            'Cost reversal of BDT ' + CAST(@ReturnCostAmount AS NVARCHAR(50)) + ' has been recorded for the Cost of Goods Sold & Inventory adjustment to ' + ISNULL(@CustomerName, 'Customer') + ' under Return Invoice ' + @ReturnInvoiceNo + '.',
            0, 0,
            @ReturnCostAmount, @ReturnCostAmount, 'Market Sales Return COGS Auto Posting', 1, GETDATE(), 'Journal',
            'Approved', 0, 0, 0, 'Auto Process',
            @PostingDate, @ReturnInvoiceId, @TableIdCOGS, 1, @ReturnInvoiceId
        );

        SET @VoucherMasterId = SCOPE_IDENTITY();

        -- Debit: Finished Goods Inventory (PageId = 23, FlagId = 1018, fallback 4156)
        DECLARE @DrInvGl INT = 0;
        SELECT TOP 1 @DrInvGl = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 23 AND FlagId = 1018;
        IF (@DrInvGl IS NULL OR @DrInvGl = 0) SET @DrInvGl = 4156;

        -- Credit: Cost of Goods Sold (PageId = 23, FlagId = 1019, fallback 4162)
        DECLARE @CrCOGSGl INT = 0;
        SELECT TOP 1 @CrCOGSGl = GLId 
        FROM CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity 
        WHERE PageId = 23 AND FlagId = 1019;
        IF (@CrCOGSGl IS NULL OR @CrCOGSGl = 0) SET @CrCOGSGl = 4162;

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail 
        (
            VoucherMasterId, AccountId, DebitorCredit,
            DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
            Remarks, SubsidoryLedgerId, RefInfoInd
        ) 
        VALUES 
        (
            @VoucherMasterId, @DrInvGl, 'Debit',
            0.00, 0.00, @ReturnCostAmount, 0.00,
            'Finished Goods Inventory Addition', 0, 0
        );

        INSERT INTO CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail 
        (
            VoucherMasterId, AccountId, DebitorCredit,
            DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT,
            Remarks, SubsidoryLedgerId, RefInfoInd
        ) 
        VALUES 
        (
            @VoucherMasterId, @CrCOGSGl, 'Credit',
            0.00, 0.00, 0.00, @ReturnCostAmount,
            'Cost of Goods Sold Reversal', 0, 0
        );
    END

    -- 5. Mark IsPosting in tblReturnInvoice
    UPDATE tblReturnInvoice SET IsPosting = 1 WHERE ReturnInvoiceId = @ReturnInvoiceId;
END;
GO
PRINT 'Updated sp_SalesReturnPosting.';
GO

PRINT '=======================================================';
PRINT 'ALL LIVE DATABASE UPDATES COMPLETED SUCCESSFULLY!';
PRINT '=======================================================';
GO
