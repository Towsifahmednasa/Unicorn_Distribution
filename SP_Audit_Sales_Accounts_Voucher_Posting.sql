/* =========================================================================================
   PRODUCTION LIVE SCRIPT: AUDIT SALES TO ACCOUNTS VOUCHER POSTING INTEGRITY
   Target Database : [SalesDisDB_Unicorn_DB] (or your Live Sales DB)
   Cross Databases : [CSTL_ACCDB_Unicorn] (Accounts DB), [CSTL_ERPDB_Unicorn] (ERP Master DB)
   Purpose         : 1-Click Verification of Voucher Existence, Detail Posting, Debit/Credit Balance, 
                     and Value Accuracy across 5 Operations (Material Conversion Receive, 
                     Delivery Confirmation, Customer Payment, Sales Return, Non-Transactional Return).
   Execution Safe  : Read-Only Audit, WITH (NOLOCK) on all production tables.
   Generated Date  : 03-Sep-2026
   ========================================================================================= */

USE [SalesDisDB_Unicorn_DB];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.sp_Audit_Sales_Accounts_Voucher_Posting', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Audit_Sales_Accounts_Voucher_Posting;
GO

CREATE PROCEDURE dbo.sp_Audit_Sales_Accounts_Voucher_Posting
    @OperationType NVARCHAR(50) = 'ALL',  -- 'ALL', 'MATERIAL_CONVERSION', 'DELIVERY_CONFIRMATION', 'CUSTOMER_PAYMENT', 'SALES_RETURN', 'NON_TRANSACTIONAL_RETURN'
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL,
    @ShowOnlyMismatches BIT = 0           -- 1 = Only show errors/mismatches, 0 = Show all records
AS
BEGIN
    SET NOCOUNT ON;

    -- Adjust ToDate to end of day (23:59:59)
    IF @ToDate IS NOT NULL
        SET @ToDate = DATEADD(SECOND, 86399, CAST(CAST(@ToDate AS DATE) AS DATETIME));

    -- Temp Table for Storing Discrepancy Summary
    CREATE TABLE #AuditSummary (
        OperationName NVARCHAR(100),
        TotalTransactions INT,
        VouchersFound INT,
        MissingVouchers INT,
        MissingDetails INT,
        UnbalancedDrCr INT,
        ValueMismatches INT,
        FullyMatchedOK INT
    );

    -- =====================================================================================
    -- 1. MATERIAL CONVERSION RECEIVE AUDIT (TableID = 2013, PK = tblWHStockInMaster.WHStockInMasterID)
    -- =====================================================================================
    IF UPPER(@OperationType) IN ('ALL', 'MATERIAL_CONVERSION')
    BEGIN
        SELECT 
            'Material Conversion Receive' AS [Operation],
            2013 AS [TableID],
            WM.WHStockInMasterID AS [CommonPK_ID],
            WM.WHStockInCode AS [DocNumber],
            WM.WHStockInDate AS [DocDate],
            WM.Status AS [DocStatus],
            ISNULL(WM.TotalValue, 0) AS [OperationalAmount],
            
            V.VoucherMasterId,
            V.VoucherCode,
            V.PostingDate AS [VoucherDate],
            V.TotalAmountBDT AS [VoucherMasterAmountBDT],
            ISNULL(VD.TotalDebitBDT, 0) AS [VoucherTotalDebitBDT],
            ISNULL(VD.TotalCreditBDT, 0) AS [VoucherTotalCreditBDT],

            CASE 
                WHEN V.VoucherMasterId IS NULL THEN 'MISSING_VOUCHER'
                WHEN VD.TotalDebitBDT IS NULL OR VD.TotalCreditBDT IS NULL THEN 'MISSING_VOUCHER_DETAILS'
                WHEN VD.TotalDebitBDT <> VD.TotalCreditBDT THEN 'DR_CR_UNBALANCED'
                WHEN ABS(ISNULL(WM.TotalValue, 0) - ISNULL(V.TotalAmountBDT, 0)) > 1.00 THEN 'MASTER_VALUE_MISMATCH'
                WHEN ABS(ISNULL(WM.TotalValue, 0) - ISNULL(VD.TotalDebitBDT, 0)) > 1.00 THEN 'DETAIL_VALUE_MISMATCH'
                ELSE 'OK'
            END AS [AuditStatus]
        INTO #TempMatConv
        FROM dbo.tblWHStockInMaster AS WM WITH (NOLOCK)
        LEFT JOIN CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster AS V WITH (NOLOCK)
            ON V.TableID = 2013 AND V.CommonPK_ID = WM.WHStockInMasterID
        LEFT JOIN (
            SELECT 
                VoucherMasterId, 
                SUM(ISNULL(DebitAmountBDT, 0)) AS TotalDebitBDT, 
                SUM(ISNULL(CreditAmountBDT, 0)) AS TotalCreditBDT
            FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK)
            GROUP BY VoucherMasterId
        ) AS VD ON VD.VoucherMasterId = V.VoucherMasterId
        WHERE (@FromDate IS NULL OR WM.WHStockInDate >= @FromDate)
          AND (@ToDate IS NULL OR WM.WHStockInDate <= @ToDate);

        -- Insert Summary
        INSERT INTO #AuditSummary
        SELECT 
            '1. Material Conversion Receive',
            COUNT(*),
            COUNT(VoucherMasterId),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER_DETAILS' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'DR_CR_UNBALANCED' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus IN ('MASTER_VALUE_MISMATCH', 'DETAIL_VALUE_MISMATCH') THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'OK' THEN 1 ELSE 0 END)
        FROM #TempMatConv;

        -- Result Grid
        SELECT * FROM #TempMatConv
        WHERE (@ShowOnlyMismatches = 0 OR AuditStatus <> 'OK')
        ORDER BY DocDate DESC, CommonPK_ID DESC;

        DROP TABLE #TempMatConv;
    END;

    -- =====================================================================================
    -- 2. DELIVERY CONFIRMATION AUDIT (TableID = 2014, PK = tblInvoice.InvoiceId)
    -- =====================================================================================
    IF UPPER(@OperationType) IN ('ALL', 'DELIVERY_CONFIRMATION')
    BEGIN
        SELECT 
            'Delivery Confirmation' AS [Operation],
            2014 AS [TableID],
            INV.InvoiceId AS [CommonPK_ID],
            INV.InvoiceNo AS [DocNumber],
            INV.DelivaryInvoiceNo AS [DeliveryInvoiceNo],
            INV.InvoiceDate AS [DocDate],
            INV.DeliveryInvoiceStatus AS [DocStatus],
            ISNULL(INV.DeliveryTpGrandTotal, 0) AS [SalesDeliveryAmount],

            -- Sales Revenue Voucher (IsInventoryValuation = 0 or NULL)
            VS.VoucherMasterId AS [SalesVoucherMasterId],
            VS.VoucherCode AS [SalesVoucherCode],
            VS.PostingDate AS [SalesVoucherDate],
            VS.TotalAmountBDT AS [SalesVoucherMasterAmount],
            ISNULL(VSD.TotalDebitBDT, 0) AS [SalesVoucherDebit],
            ISNULL(VSD.TotalCreditBDT, 0) AS [SalesVoucherCredit],

            -- COGS / Inventory Valuation Voucher (IsInventoryValuation = 1)
            VC.VoucherMasterId AS [COGSVoucherMasterId],
            VC.VoucherCode AS [COGSVoucherCode],
            VC.TotalAmountBDT AS [COGSVoucherMasterAmount],
            ISNULL(VCD.TotalDebitBDT, 0) AS [COGSVoucherDebit],
            ISNULL(VCD.TotalCreditBDT, 0) AS [COGSVoucherCredit],

            -- Overall Audit Status
            CASE 
                WHEN VS.VoucherMasterId IS NULL THEN 'MISSING_SALES_VOUCHER'
                WHEN VSD.TotalDebitBDT IS NULL OR VSD.TotalCreditBDT IS NULL THEN 'MISSING_SALES_DETAILS'
                WHEN VSD.TotalDebitBDT <> VSD.TotalCreditBDT THEN 'SALES_DR_CR_UNBALANCED'
                WHEN ABS(ISNULL(INV.DeliveryTpGrandTotal, 0) - ISNULL(VS.TotalAmountBDT, 0)) > 1.00 THEN 'SALES_VALUE_MISMATCH'
                WHEN VC.VoucherMasterId IS NOT NULL AND VCD.TotalDebitBDT <> VCD.TotalCreditBDT THEN 'COGS_DR_CR_UNBALANCED'
                ELSE 'OK'
            END AS [AuditStatus]
        INTO #TempDelivery
        FROM dbo.tblInvoice AS INV WITH (NOLOCK)
        LEFT JOIN CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster AS VS WITH (NOLOCK)
            ON VS.TableID = 2014 AND VS.CommonPK_ID = INV.InvoiceId AND ISNULL(VS.IsInventoryValuation, 0) = 0
        LEFT JOIN (
            SELECT 
                VoucherMasterId, 
                SUM(ISNULL(DebitAmountBDT, 0)) AS TotalDebitBDT, 
                SUM(ISNULL(CreditAmountBDT, 0)) AS TotalCreditBDT
            FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK)
            GROUP BY VoucherMasterId
        ) AS VSD ON VSD.VoucherMasterId = VS.VoucherMasterId
        LEFT JOIN CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster AS VC WITH (NOLOCK)
            ON VC.TableID = 2014 AND VC.CommonPK_ID = INV.InvoiceId AND ISNULL(VC.IsInventoryValuation, 0) = 1
        LEFT JOIN (
            SELECT 
                VoucherMasterId, 
                SUM(ISNULL(DebitAmountBDT, 0)) AS TotalDebitBDT, 
                SUM(ISNULL(CreditAmountBDT, 0)) AS TotalCreditBDT
            FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK)
            GROUP BY VoucherMasterId
        ) AS VCD ON VCD.VoucherMasterId = VC.VoucherMasterId
        WHERE (INV.DelivaryInvoiceNo IS NOT NULL OR INV.DeliveryInvoiceStatus IN ('Full', 'Partial') OR VS.VoucherMasterId IS NOT NULL)
          AND (@FromDate IS NULL OR INV.InvoiceDate >= @FromDate)
          AND (@ToDate IS NULL OR INV.InvoiceDate <= @ToDate);

        -- Insert Summary
        INSERT INTO #AuditSummary
        SELECT 
            '2. Delivery Confirmation',
            COUNT(*),
            COUNT(SalesVoucherMasterId),
            SUM(CASE WHEN AuditStatus = 'MISSING_SALES_VOUCHER' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MISSING_SALES_DETAILS' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus IN ('SALES_DR_CR_UNBALANCED', 'COGS_DR_CR_UNBALANCED') THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'SALES_VALUE_MISMATCH' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'OK' THEN 1 ELSE 0 END)
        FROM #TempDelivery;

        -- Result Grid
        SELECT * FROM #TempDelivery
        WHERE (@ShowOnlyMismatches = 0 OR AuditStatus <> 'OK')
        ORDER BY DocDate DESC, CommonPK_ID DESC;

        DROP TABLE #TempDelivery;
    END;

    -- =====================================================================================
    -- 3. CUSTOMER PAYMENT AUDIT (TableID = 2017, PK = tblCustomerPay.CustPayId)
    -- =====================================================================================
    IF UPPER(@OperationType) IN ('ALL', 'CUSTOMER_PAYMENT')
    BEGIN
        SELECT 
            'Customer Payment' AS [Operation],
            2017 AS [TableID],
            CPY.CustPayId AS [CommonPK_ID],
            ISNULL(CPY.RefNo, 'CP-' + CAST(CPY.CustPayId AS NVARCHAR(50))) AS [DocNumber],
            CPY.PaymentDate AS [DocDate],
            CPY.PayType AS [DocStatus],
            (ISNULL(CPD.TotalPaymentAmount, 0) + ISNULL(CPD.TotalDiscount, 0) + ISNULL(CPD.TotalAIT, 0)) AS [OperationalTotalAmount],
            ISNULL(CPD.TotalPaymentAmount, 0) AS [CashBankAmount],
            ISNULL(CPD.TotalDiscount, 0) AS [DiscountAmount],
            ISNULL(CPD.TotalAIT, 0) AS [AITAmount],

            V.VoucherMasterId,
            V.VoucherCode,
            V.VoucherType,
            V.PostingDate AS [VoucherDate],
            V.TotalAmountBDT AS [VoucherMasterAmountBDT],
            ISNULL(VD.TotalDebitBDT, 0) AS [VoucherTotalDebitBDT],
            ISNULL(VD.TotalCreditBDT, 0) AS [VoucherTotalCreditBDT],

            CASE 
                WHEN V.VoucherMasterId IS NULL THEN 'MISSING_VOUCHER'
                WHEN VD.TotalDebitBDT IS NULL OR VD.TotalCreditBDT IS NULL THEN 'MISSING_VOUCHER_DETAILS'
                WHEN VD.TotalDebitBDT <> VD.TotalCreditBDT THEN 'DR_CR_UNBALANCED'
                WHEN ABS((ISNULL(CPD.TotalPaymentAmount, 0) + ISNULL(CPD.TotalDiscount, 0) + ISNULL(CPD.TotalAIT, 0)) - ISNULL(V.TotalAmountBDT, 0)) > 1.00 THEN 'MASTER_VALUE_MISMATCH'
                WHEN ABS((ISNULL(CPD.TotalPaymentAmount, 0) + ISNULL(CPD.TotalDiscount, 0) + ISNULL(CPD.TotalAIT, 0)) - ISNULL(VD.TotalCreditBDT, 0)) > 1.00 THEN 'DETAIL_VALUE_MISMATCH'
                ELSE 'OK'
            END AS [AuditStatus]
        INTO #TempPayment
        FROM dbo.tblCustomerPay AS CPY WITH (NOLOCK)
        LEFT JOIN (
            SELECT 
                CustPayId, 
                SUM(ISNULL(PaymentAmount, 0)) AS TotalPaymentAmount, 
                SUM(ISNULL(Discount, 0)) AS TotalDiscount, 
                SUM(ISNULL(AIT, 0)) AS TotalAIT
            FROM dbo.tblCustPayDetail WITH (NOLOCK)
            GROUP BY CustPayId
        ) AS CPD ON CPD.CustPayId = CPY.CustPayId
        LEFT JOIN CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster AS V WITH (NOLOCK)
            ON V.TableID = 2017 AND V.CommonPK_ID = CPY.CustPayId
        LEFT JOIN (
            SELECT 
                VoucherMasterId, 
                SUM(ISNULL(DebitAmountBDT, 0)) AS TotalDebitBDT, 
                SUM(ISNULL(CreditAmountBDT, 0)) AS TotalCreditBDT
            FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK)
            GROUP BY VoucherMasterId
        ) AS VD ON VD.VoucherMasterId = V.VoucherMasterId
        WHERE (@FromDate IS NULL OR CPY.PaymentDate >= @FromDate)
          AND (@ToDate IS NULL OR CPY.PaymentDate <= @ToDate);

        -- Insert Summary
        INSERT INTO #AuditSummary
        SELECT 
            '3. Customer Payment',
            COUNT(*),
            COUNT(VoucherMasterId),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER_DETAILS' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'DR_CR_UNBALANCED' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus IN ('MASTER_VALUE_MISMATCH', 'DETAIL_VALUE_MISMATCH') THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'OK' THEN 1 ELSE 0 END)
        FROM #TempPayment;

        -- Result Grid
        SELECT * FROM #TempPayment
        WHERE (@ShowOnlyMismatches = 0 OR AuditStatus <> 'OK')
        ORDER BY DocDate DESC, CommonPK_ID DESC;

        DROP TABLE #TempPayment;
    END;

    -- =====================================================================================
    -- 4. SALES RETURN AUDIT (TableID = 2018 / 2023, PK = tblReturnInvoice.ReturnInvoiceId)
    -- =====================================================================================
    IF UPPER(@OperationType) IN ('ALL', 'SALES_RETURN')
    BEGIN
        SELECT 
            'Sales Return' AS [Operation],
            2018 AS [TableID],
            RI.ReturnInvoiceId AS [CommonPK_ID],
            RI.ReturnInvoiceNo AS [DocNumber],
            RI.ReturnInvoiceDate AS [DocDate],
            ISNULL(RI.TpTotal, 0) AS [ReturnGrossAmount],
            ISNULL(RI.TpDiscount, 0) AS [ReturnDiscountAmount],
            (ISNULL(RI.TpTotal, 0) - ISNULL(RI.TpDiscount, 0)) AS [ReturnNetAmount],

            V.VoucherMasterId,
            V.VoucherCode,
            V.PostingDate AS [VoucherDate],
            V.TotalAmountBDT AS [VoucherMasterAmountBDT],
            ISNULL(VD.TotalDebitBDT, 0) AS [VoucherTotalDebitBDT],
            ISNULL(VD.TotalCreditBDT, 0) AS [VoucherTotalCreditBDT],

            CASE 
                WHEN V.VoucherMasterId IS NULL THEN 'MISSING_VOUCHER'
                WHEN VD.TotalDebitBDT IS NULL OR VD.TotalCreditBDT IS NULL THEN 'MISSING_VOUCHER_DETAILS'
                WHEN VD.TotalDebitBDT <> VD.TotalCreditBDT THEN 'DR_CR_UNBALANCED'
                WHEN ABS(ISNULL(RI.TpTotal, 0) - ISNULL(V.TotalAmountBDT, 0)) > 1.00 THEN 'MASTER_VALUE_MISMATCH'
                ELSE 'OK'
            END AS [AuditStatus]
        INTO #TempSalesReturn
        FROM dbo.tblReturnInvoice AS RI WITH (NOLOCK)
        LEFT JOIN CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster AS V WITH (NOLOCK)
            ON (V.TableID = 2018 OR V.SalesReturnId = RI.ReturnInvoiceId) 
           AND (V.CommonPK_ID = RI.ReturnInvoiceId OR V.SalesReturnId = RI.ReturnInvoiceId)
           AND ISNULL(V.IsInventoryValuation, 0) = 0
        LEFT JOIN (
            SELECT 
                VoucherMasterId, 
                SUM(ISNULL(DebitAmountBDT, 0)) AS TotalDebitBDT, 
                SUM(ISNULL(CreditAmountBDT, 0)) AS TotalCreditBDT
            FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK)
            GROUP BY VoucherMasterId
        ) AS VD ON VD.VoucherMasterId = V.VoucherMasterId
        WHERE (@FromDate IS NULL OR RI.ReturnInvoiceDate >= @FromDate)
          AND (@ToDate IS NULL OR RI.ReturnInvoiceDate <= @ToDate);

        -- Insert Summary
        INSERT INTO #AuditSummary
        SELECT 
            '4. Sales Return',
            COUNT(*),
            COUNT(VoucherMasterId),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER_DETAILS' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'DR_CR_UNBALANCED' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MASTER_VALUE_MISMATCH' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'OK' THEN 1 ELSE 0 END)
        FROM #TempSalesReturn;

        -- Result Grid
        SELECT * FROM #TempSalesReturn
        WHERE (@ShowOnlyMismatches = 0 OR AuditStatus <> 'OK')
        ORDER BY DocDate DESC, CommonPK_ID DESC;

        DROP TABLE #TempSalesReturn;
    END;

    -- =====================================================================================
    -- 5. NON-TRANSACTIONAL RETURN / DESTROY AUDIT (TableID = 2016, PK = tblDestroyProduct.DestroyId)
    -- =====================================================================================
    IF UPPER(@OperationType) IN ('ALL', 'NON_TRANSACTIONAL_RETURN')
    BEGIN
        SELECT 
            'Non-Transactional Return / Destroy' AS [Operation],
            2016 AS [TableID],
            DSP.DestroyId AS [CommonPK_ID],
            'DST-' + CAST(DSP.DestroyId AS NVARCHAR(50)) AS [DocNumber],
            DSP.StockOutDate AS [DocDate],
            DSP.Reason AS [DocStatus],
            ISNULL(DSP.StockOutQty * CTRS.UnitPrice, 0) AS [OperationalStockValue],

            V.VoucherMasterId,
            V.VoucherCode,
            V.PostingDate AS [VoucherDate],
            V.TotalAmountBDT AS [VoucherMasterAmountBDT],
            ISNULL(VD.TotalDebitBDT, 0) AS [VoucherTotalDebitBDT],
            ISNULL(VD.TotalCreditBDT, 0) AS [VoucherTotalCreditBDT],

            CASE 
                WHEN V.VoucherMasterId IS NULL THEN 'MISSING_VOUCHER'
                WHEN VD.TotalDebitBDT IS NULL OR VD.TotalCreditBDT IS NULL THEN 'MISSING_VOUCHER_DETAILS'
                WHEN VD.TotalDebitBDT <> VD.TotalCreditBDT THEN 'DR_CR_UNBALANCED'
                WHEN ABS(ISNULL(DSP.StockOutQty * CTRS.UnitPrice, 0) - ISNULL(V.TotalAmountBDT, 0)) > 1.00 THEN 'MASTER_VALUE_MISMATCH'
                ELSE 'OK'
            END AS [AuditStatus]
        INTO #TempDestroy
        FROM dbo.tblDestroyProduct AS DSP WITH (NOLOCK)
        INNER JOIN dbo.tblDCStoreFreeze AS DFZS WITH (NOLOCK) ON DSP.DcFreezeId = DFZS.DCStoreFreezeId
        INNER JOIN dbo.tblDCStore AS DCS WITH (NOLOCK) ON DCS.DCStoreId = DFZS.DCStoreId
        INNER JOIN dbo.tblCentralStore AS CTRS WITH (NOLOCK) ON DCS.TempReceiveId = CTRS.ReceiveId
        LEFT JOIN CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster AS V WITH (NOLOCK)
            ON (V.TableID = 2016 OR V.DestroyId = DSP.DestroyId)
           AND (V.CommonPK_ID = DSP.DestroyId OR V.DestroyId = DSP.DestroyId)
        LEFT JOIN (
            SELECT 
                VoucherMasterId, 
                SUM(ISNULL(DebitAmountBDT, 0)) AS TotalDebitBDT, 
                SUM(ISNULL(CreditAmountBDT, 0)) AS TotalCreditBDT
            FROM CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail WITH (NOLOCK)
            GROUP BY VoucherMasterId
        ) AS VD ON VD.VoucherMasterId = V.VoucherMasterId
        WHERE (@FromDate IS NULL OR DSP.StockOutDate >= @FromDate)
          AND (@ToDate IS NULL OR DSP.StockOutDate <= @ToDate);

        -- Insert Summary
        INSERT INTO #AuditSummary
        SELECT 
            '5. Non-Transactional Return',
            COUNT(*),
            COUNT(VoucherMasterId),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MISSING_VOUCHER_DETAILS' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'DR_CR_UNBALANCED' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'MASTER_VALUE_MISMATCH' THEN 1 ELSE 0 END),
            SUM(CASE WHEN AuditStatus = 'OK' THEN 1 ELSE 0 END)
        FROM #TempDestroy;

        -- Result Grid
        SELECT * FROM #TempDestroy
        WHERE (@ShowOnlyMismatches = 0 OR AuditStatus <> 'OK')
        ORDER BY DocDate DESC, CommonPK_ID DESC;

        DROP TABLE #TempDestroy;
    END;

    -- =====================================================================================
    -- FINAL KPI SUMMARY DASHBOARD
    -- =====================================================================================
    SELECT 
        OperationName AS [Sales / Inventory Operation],
        TotalTransactions AS [Total Records],
        VouchersFound AS [Vouchers Posted],
        MissingVouchers AS [Missing Vouchers],
        MissingDetails AS [Missing Details],
        UnbalancedDrCr AS [Dr/Cr Unbalanced],
        ValueMismatches AS [Value Mismatch],
        FullyMatchedOK AS [100% OK Count],
        CAST(
            CASE WHEN TotalTransactions > 0 
                 THEN (CAST(FullyMatchedOK AS DECIMAL(18,2)) / CAST(TotalTransactions AS DECIMAL(18,2))) * 100.0 
                 ELSE 100.0 
            END AS DECIMAL(5,2)
        ) AS [Health (%) Score]
    FROM #AuditSummary
    ORDER BY OperationName;

    DROP TABLE #AuditSummary;

END;
GO
