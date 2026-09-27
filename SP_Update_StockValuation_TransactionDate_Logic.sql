USE [SalesDisDB_Unicorn_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- 1. Update [dbo].[sp_Report_DCStore_StockValuationLedger]
-- ============================================================================
PRINT 'Updating dbo.sp_Report_DCStore_StockValuationLedger...';
GO

IF OBJECT_ID('dbo.sp_Report_DCStore_StockValuationLedger', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Report_DCStore_StockValuationLedger;
GO

CREATE PROCEDURE dbo.sp_Report_DCStore_StockValuationLedger
    @FromDate DATETIME,
    @ToDate DATETIME,
    @ComUnitId INT = NULL,   -- Optional: DC Store/Depot (Default: NULL for All)
    @CategoryId INT = NULL,  -- Optional: Category ID
    @ProductId INT = NULL    -- Optional: Product ID
AS
BEGIN
    SET NOCOUNT ON;

    -- Adjust ToDate to end of the day (23:59:59)
    SET @ToDate = DATEADD(SECOND, 86399, CAST(CAST(@ToDate AS DATE) AS DATETIME));

    -- -------------------------------------------------------------------------
    -- Main DC Stock Report Query
    -- -------------------------------------------------------------------------
    SELECT 
        CU.ComUnitCode                                             AS [DC Code],
        CU.ComUnitName                                             AS [Distribution Center],
        PD.ProductCode                                             AS [Product Code],
        PD.ProductName                                             AS [Product Name],
        ISNULL(PD.PackSize, N'N/A')                                AS [Pack Size],
        ISNULL(cat.CategoryName, N'N/A')                           AS [Category Name],
        
        -- 1. OPENING BALANCE (Before FromDate)
        CAST(ISNULL(OP.OpeningQty, 0.00) AS DECIMAL(18,2))         AS [Opening Qty],
        CAST(ISNULL(OP.OpeningRate, ISNULL(UP.CostPrice, 0.00)) AS DECIMAL(18,2)) AS [Opening Rate],
        CAST(ISNULL(OP.OpeningValue, (ISNULL(OP.OpeningQty, 0.00) * ISNULL(UP.CostPrice, 0.00))) AS DECIMAL(18,2)) AS [Opening Total Value],
        
        -- 2. MOVEMENTS (Between FromDate and ToDate)
        CAST(ISNULL(MOV.RcvFromWHQty, 0.00) AS DECIMAL(18,2))      AS [Receive From WH],
        CAST(ISNULL(MOV.B2BRcvQty, 0.00) AS DECIMAL(18,2))         AS [B2B Receive],
        CAST(ISNULL(MOV.ReturnRcvQty, 0.00) AS DECIMAL(18,2))      AS [Return Rcv],
        CAST(ISNULL(MOV.SalesQty, 0.00) AS DECIMAL(18,2))          AS [Sales],
        CAST(ISNULL(MOV.B2BIssueQty, 0.00) AS DECIMAL(18,2))       AS [B2B Issue],
        
        -- 3. CLOSING BALANCE (Opening + WH Rcv + B2B Rcv + Return Rcv - Sales - B2B Issue)
        CAST(
            (ISNULL(OP.OpeningQty, 0) + ISNULL(MOV.RcvFromWHQty, 0) + ISNULL(MOV.B2BRcvQty, 0) + ISNULL(MOV.ReturnRcvQty, 0) - ISNULL(MOV.SalesQty, 0) - ISNULL(MOV.B2BIssueQty, 0)) 
            AS DECIMAL(18,2)
        )                                                          AS [Closing Qty],
        
        CAST(ISNULL(LAST_COST.CostPerUnit, ISNULL(UP.CostPrice, 0.00)) AS DECIMAL(18,2)) AS [Closing Rate],
        
        CAST(
            ((ISNULL(OP.OpeningQty, 0) + ISNULL(MOV.RcvFromWHQty, 0) + ISNULL(MOV.B2BRcvQty, 0) + ISNULL(MOV.ReturnRcvQty, 0) - ISNULL(MOV.SalesQty, 0) - ISNULL(MOV.B2BIssueQty, 0)) * ISNULL(LAST_COST.CostPerUnit, ISNULL(UP.CostPrice, 0.00))) 
            AS DECIMAL(18,2)
        )                                                          AS [Closing Total Value]

    FROM dbo.tblDCStore DC WITH (NOLOCK)

    -- Company Unit (DC Center Name & Code) Join
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) 
        ON DC.ComUnitId = CU.ComUnitId

    -- Product Info Join
    INNER JOIN dbo.tblProduct PD WITH (NOLOCK) 
        ON LTRIM(RTRIM(UPPER(DC.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))

    -- Unit Price Join for fallback costing
    LEFT JOIN dbo.tblUnitPrice UP WITH (NOLOCK)
        ON PD.ProductId = UP.ProductId AND UP.IsActive = 1

    -- Opening Stock Calculation (Before FromDate): Receive by StockRcvDate, Issue by ReferenceTransactionDate
    LEFT JOIN (
        SELECT 
            ComUnitId, ProductCode,
            SUM(StockQty) AS OpeningQty,
            CASE WHEN SUM(StockQty) > 0 THEN SUM(TotalCostAmount) / SUM(StockQty) ELSE AVG(CostPerUnit) END AS OpeningRate,
            SUM(TotalCostAmount) AS OpeningValue
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) < @FromDate
        GROUP BY ComUnitId, ProductCode
    ) OP ON DC.ComUnitId = OP.ComUnitId AND DC.ProductCode = OP.ProductCode

    -- Movements Calculation (Between FromDate and ToDate)
    LEFT JOIN (
        SELECT 
            ComUnitId, ProductCode,
            
            -- Receive From WH: Inbound Receive from Central Warehouse (Receive Date wise)
            SUM(CASE 
                    WHEN (Trans_Type = 'Receive' OR Trans_Type IS NULL OR Trans_Type = 'Available' OR Trans_Type = '') 
                         AND ISNULL(ReturnFromInvoiceDetailId, 0) = 0 
                         AND ISNULL(ReturnInvoiceDetailId, 0) = 0
                         AND ISNULL(StockCondition, '') NOT IN ('B2BReceive', 'TransferReceive', 'SalesReturn', 'Return')
                         AND ISNULL(StockCondition, '') NOT LIKE '%Transcational Sales Return%'
                         AND (ChalanNo NOT LIKE 'IDT%' OR ChalanNo IS NULL)
                         AND (ChalanNo NOT LIKE 'RIN%' AND ChalanNo NOT LIKE 'RINV%')
                         AND StockQty > 0
                    THEN StockQty ELSE 0 
                END) AS RcvFromWHQty,

            -- B2B Receive: Inward Transfer from another DC (Receive Date wise)
            -- Return rows can carry the source IDT chalan no, so exclude anything
            -- ReturnRcvQty already counts (else it double-counts in B2B Receive).
            SUM(CASE
                    WHEN (Trans_Type = 'Receive' OR Trans_Type = 'Available')
                         AND (
                             StockCondition IN ('B2BReceive', 'TransferReceive')
                             OR ChalanNo LIKE 'IDT%'
                         )
                         AND ISNULL(ReturnFromInvoiceDetailId, 0) = 0
                         AND ISNULL(ReturnInvoiceDetailId, 0) = 0
                         AND ISNULL(StockCondition, '') NOT IN ('DeliveryReturn', 'SalesReturn', 'Return')
                         AND StockQty > 0
                    THEN StockQty ELSE 0 
                END) AS B2BRcvQty,
            
            -- Return Rcv: Customer Return / Sales Return (Receive Date wise)
            SUM(CASE 
                    WHEN StockQty > 0
                         AND (ISNULL(ReturnFromInvoiceDetailId, 0) <> 0
                              OR ISNULL(ReturnInvoiceDetailId, 0) <> 0
                              OR ISNULL(StockCondition, '') IN ('SalesReturn', 'Return')
                              OR ISNULL(StockCondition, '') LIKE '%Transcational Sales Return%'
                              OR ChalanNo LIKE 'RIN%'
                              OR ChalanNo LIKE 'RINV%')
                    THEN StockQty ELSE 0
                END) AS ReturnRcvQty,
            
            -- Sales Issue: Customer Deliveries / Sales (Transaction Date wise)
            SUM(CASE 
                    WHEN StockQty < 0 AND ISNULL(StockCondition, '') NOT IN ('B2BIssue', 'TransferIssue')
                    THEN ABS(StockQty) 
                    WHEN (Trans_Type = 'Issue' OR Trans_Type = 'Sales') AND ISNULL(StockCondition, '') NOT IN ('B2BIssue', 'TransferIssue')
                    THEN StockQty 
                    ELSE 0 
                END) AS SalesQty,

            -- B2B Issue: Outward Transfer to another DC (Transaction Date wise)
            SUM(CASE 
                    WHEN StockCondition IN ('B2BIssue', 'TransferIssue')
                    THEN ABS(StockQty) 
                    ELSE 0 
                END) AS B2BIssueQty

        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) >= @FromDate 
          AND (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) <= @ToDate
        GROUP BY ComUnitId, ProductCode
    ) MOV ON DC.ComUnitId = MOV.ComUnitId AND DC.ProductCode = MOV.ProductCode

    -- Latest Cost Per Unit Calculation
    LEFT JOIN (
        SELECT 
            ComUnitId, ProductCode, CostPerUnit,
            ROW_NUMBER() OVER (PARTITION BY ComUnitId, ProductCode ORDER BY StockRcvDate DESC, DCStoreId DESC) AS RowNum
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) <= @ToDate AND CostPerUnit > 0
    ) LAST_COST ON DC.ComUnitId = LAST_COST.ComUnitId AND DC.ProductCode = LAST_COST.ProductCode AND LAST_COST.RowNum = 1

    LEFT JOIN dbo.tblProCategory cat WITH (NOLOCK) ON PD.CategoryId = cat.CategoryId

    -- Dynamic Filter Condition
    WHERE 
        (@ComUnitId IS NULL OR DC.ComUnitId = @ComUnitId)
        AND (@CategoryId IS NULL OR PD.CategoryId = @CategoryId)
        AND (@ProductId IS NULL OR PD.ProductId = @ProductId)

    GROUP BY 
        CU.ComUnitCode, CU.ComUnitName, PD.ProductCode, PD.ProductName, 
        PD.PackSize, cat.CategoryName, OP.OpeningQty, OP.OpeningRate, 
        OP.OpeningValue, MOV.RcvFromWHQty, MOV.B2BRcvQty, MOV.ReturnRcvQty, MOV.SalesQty, MOV.B2BIssueQty, 
        LAST_COST.CostPerUnit, UP.CostPrice

    HAVING 
        (ISNULL(OP.OpeningQty, 0) <> 0 OR ISNULL(MOV.RcvFromWHQty, 0) <> 0 OR ISNULL(MOV.B2BRcvQty, 0) <> 0 OR ISNULL(MOV.ReturnRcvQty, 0) <> 0 OR ISNULL(MOV.SalesQty, 0) <> 0 OR ISNULL(MOV.B2BIssueQty, 0) <> 0)

    ORDER BY CU.ComUnitName, cat.CategoryName, PD.ProductName;
END;
GO

PRINT 'dbo.sp_Report_DCStore_StockValuationLedger updated successfully!';
GO

-- ============================================================================
-- 2. Update [dbo].[sp_Report_NationalStockValuation_DateRange]
-- ============================================================================
PRINT 'Updating dbo.sp_Report_NationalStockValuation_DateRange...';
GO

IF OBJECT_ID('dbo.sp_Report_NationalStockValuation_DateRange', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Report_NationalStockValuation_DateRange;
GO

CREATE PROCEDURE dbo.sp_Report_NationalStockValuation_DateRange
    @FromDate DATETIME,
    @ToDate DATETIME,
    @CategoryId INT = NULL,     -- Optional
    @ProductId INT = NULL       -- Optional
AS
BEGIN
    SET NOCOUNT ON;

    -- Adjust ToDate to end of the day (23:59:59)
    SET @ToDate = DATEADD(SECOND, 86399, CAST(CAST(@ToDate AS DATE) AS DATETIME));

    DECLARE @DynamicPivotColumns NVARCHAR(MAX) = '';
    DECLARE @SelectPivotColumns NVARCHAR(MAX) = '';
    DECLARE @SqlQuery NVARCHAR(MAX) = '';

    -- -------------------------------------------------------------------------
    -- 1. Dynamic Depot (ComUnit) Columns
    -- -------------------------------------------------------------------------
    SELECT 
        @DynamicPivotColumns = STRING_AGG(QUOTENAME(ComUnitName), ', '),
        @SelectPivotColumns  = STRING_AGG('ISNULL(' + QUOTENAME(ComUnitName) + ', 0) AS [' + ComUnitName + ']', ', ')
    FROM dbo.tblCompanyUnit WITH (NOLOCK);

    -- -------------------------------------------------------------------------
    -- 2. Dynamic SQL Execution Script
    -- -------------------------------------------------------------------------
    SET @SqlQuery = '
    WITH DC_Stock AS (
        SELECT 
            DC.ProductCode,
            CU.ComUnitName,
            SUM(DC.StockQty) AS DepotStockQty
        FROM dbo.tblDCStore DC WITH (NOLOCK)
        INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON DC.ComUnitId = CU.ComUnitId
        WHERE (CASE WHEN DC.StockQty < 0 OR DC.Trans_Type = ''Issue'' THEN ISNULL(DC.ReferenceTransactionDate, DC.StockRcvDate) ELSE DC.StockRcvDate END) <= ''' + CONVERT(VARCHAR(23), @ToDate, 121) + '''
        GROUP BY DC.ProductCode, CU.ComUnitName
    ),
    Pivoted_DC AS (
        SELECT ProductCode, ' + @DynamicPivotColumns + '
        FROM DC_Stock
        PIVOT (
            SUM(DepotStockQty)
            FOR ComUnitName IN (' + @DynamicPivotColumns + ')
        ) AS PivotTable
    )
    SELECT 
        PD.ProductCode                                             AS [Product Code],
        PD.ProductName                                             AS [Product Name],
        ISNULL(PD.PackSize, N''N/A'')                                AS [Pack Size],
        ISNULL(cat.CategoryName, N''N/A'')                           AS [Category],
        
        -- Avg Rate (Central Rate)
        CAST(ISNULL(CL_CS.WH_Rate, 0.0000) AS DECIMAL(18,2))       AS [Avg Rate],

        -- 1. OPENING NATIONAL STOCK (FromDate Central + All Depots)
        CAST(
            (ISNULL(OP_CS.WH_OP_Qty, 0) + ISNULL(OP_DC.DC_OP_Qty, 0)) 
            AS DECIMAL(18,2)
        )                                                          AS [Opening National Qty],

        -- 2. CENTRAL STORE CLOSING QTY
        CAST(ISNULL(CL_CS.WH_StockQty, 0.00) AS DECIMAL(18,2))     AS [Central Store Qty],
        
        -- 3. DYNAMIC DEPOT CLOSING STOCKS (Chattogram, Dhaka etc.)
        ' + @SelectPivotColumns + ',
        
        -- 4. TOTAL DEPOTS CLOSING QTY
        CAST(ISNULL(ALL_DC.Total_DC_Stock, 0.00) AS DECIMAL(18,2)) AS [Total Depots Qty],
        
        -- 5. TOTAL CLOSING NATIONAL QTY (ToDate Central + Depots)
        CAST(
            (ISNULL(CL_CS.WH_StockQty, 0) + ISNULL(ALL_DC.Total_DC_Stock, 0)) 
            AS DECIMAL(18,2)
        )                                                          AS [National Stock Qty],
        
        -- 6. VALUATION BREAKDOWN
        CAST(ISNULL(CL_CS.WH_TotalValue, 0.00) AS DECIMAL(18,2))   AS [Central Store Value],
        CAST(ISNULL(ALL_DC.Total_DC_Value, 0.00) AS DECIMAL(18,2)) AS [Depots Total Value],
        
        -- GRAND TOTAL NATIONAL VALUATION
        CAST(
            (ISNULL(CL_CS.WH_TotalValue, 0) + ISNULL(ALL_DC.Total_DC_Value, 0)) 
            AS DECIMAL(18,2)
        )                                                          AS [National Total Value]

    FROM dbo.tblProduct PD WITH (NOLOCK)

    -- Opening Central Store Stock
    LEFT JOIN (
        SELECT 
            Item_Id, Close_Qty AS WH_OP_Qty,
            ROW_NUMBER() OVER (PARTITION BY Item_Id ORDER BY Weighted_Avg_Id DESC) AS RowNum
        FROM dbo.tbl_INV_CentralStore_WeightedAvg WITH (NOLOCK)
        WHERE Trans_Date < ''' + CONVERT(VARCHAR(23), @FromDate, 121) + '''
    ) OP_CS ON PD.ProductId = OP_CS.Item_Id AND OP_CS.RowNum = 1

    -- Opening All DC Stock: Receive by StockRcvDate, Issue by ReferenceTransactionDate
    LEFT JOIN (
        SELECT ProductCode, SUM(StockQty) AS DC_OP_Qty
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE (CASE WHEN StockQty < 0 OR Trans_Type = ''Issue'' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) < ''' + CONVERT(VARCHAR(23), @FromDate, 121) + '''
        GROUP BY ProductCode
    ) OP_DC ON LTRIM(RTRIM(UPPER(PD.ProductCode))) = LTRIM(RTRIM(UPPER(OP_DC.ProductCode)))

    -- Closing Central Store Stock
    LEFT JOIN (
        SELECT 
            Item_Id, Close_Qty AS WH_StockQty, Avg_Rate AS WH_Rate, Close_Amt AS WH_TotalValue,
            ROW_NUMBER() OVER (PARTITION BY Item_Id ORDER BY Weighted_Avg_Id DESC) AS RowNum
        FROM dbo.tbl_INV_CentralStore_WeightedAvg WITH (NOLOCK)
        WHERE Trans_Date <= ''' + CONVERT(VARCHAR(23), @ToDate, 121) + '''
    ) CL_CS ON PD.ProductId = CL_CS.Item_Id AND CL_CS.RowNum = 1

    -- Dynamic Pivoted Depot Closing Stock Join
    LEFT JOIN Pivoted_DC PDC ON LTRIM(RTRIM(UPPER(PD.ProductCode))) = LTRIM(RTRIM(UPPER(PDC.ProductCode)))

    -- All Depots Aggregated Closing Stock Join
    LEFT JOIN (
        SELECT 
            ProductCode,
            SUM(StockQty) AS Total_DC_Stock,
            SUM(TotalCostAmount) AS Total_DC_Value
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE (CASE WHEN StockQty < 0 OR Trans_Type = ''Issue'' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) <= ''' + CONVERT(VARCHAR(23), @ToDate, 121) + '''
        GROUP BY ProductCode
    ) ALL_DC ON LTRIM(RTRIM(UPPER(PD.ProductCode))) = LTRIM(RTRIM(UPPER(ALL_DC.ProductCode)))

    LEFT JOIN dbo.tblProCategory cat WITH (NOLOCK) ON PD.CategoryId = cat.CategoryId

    WHERE 
        (' + ISNULL(CAST(@CategoryId AS VARCHAR), 'NULL') + ' IS NULL OR PD.CategoryId = ' + ISNULL(CAST(@CategoryId AS VARCHAR), 'NULL') + ')
        AND (' + ISNULL(CAST(@ProductId AS VARCHAR), 'NULL') + ' IS NULL OR PD.ProductId = ' + ISNULL(CAST(@ProductId AS VARCHAR), 'NULL') + ')
        AND (
            ISNULL(OP_CS.WH_OP_Qty, 0) > 0 OR ISNULL(OP_DC.DC_OP_Qty, 0) > 0 
            OR ISNULL(CL_CS.WH_StockQty, 0) > 0 OR ISNULL(ALL_DC.Total_DC_Stock, 0) > 0
        )

    ORDER BY cat.CategoryName, PD.ProductName;
    ';

    -- Execute Query
    EXEC sp_executesql @SqlQuery;
END;
GO

PRINT 'dbo.sp_Report_NationalStockValuation_DateRange updated successfully!';
GO

-- ============================================================================
-- 3. Update [dbo].[sp_Report_NationalStockValuation_BatchDetails]
-- ============================================================================
PRINT 'Updating dbo.sp_Report_NationalStockValuation_BatchDetails...';
GO

IF OBJECT_ID('dbo.sp_Report_NationalStockValuation_BatchDetails', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Report_NationalStockValuation_BatchDetails;
GO

CREATE PROCEDURE [dbo].[sp_Report_NationalStockValuation_BatchDetails]
    @ProductCode NVARCHAR(100),
    @LocationName NVARCHAR(150),
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Adjust ToDate to end of day
    SET @ToDate = DATEADD(SECOND, 86399, CAST(CAST(@ToDate AS DATE) AS DATETIME));

    IF @LocationName = 'Central Store Qty'
    BEGIN
        SELECT 
            B.Batch_No AS [Batch No],
            CONVERT(VARCHAR(10), MIN(B.Mfg_Date), 105) AS [MFG Date],
            CONVERT(VARCHAR(10), MIN(B.Exp_Date), 105) AS [Exp Date],
            CAST(SUM(B.Trans_Qty) AS DECIMAL(18,2)) AS [Qty]
        FROM dbo.tbl_INV_Central_BatchWiseStock B WITH (NOLOCK)
        INNER JOIN dbo.tbl_INV_CentralStore_WeightedAvg WA WITH (NOLOCK) ON B.Weighted_Avg_Id = WA.Weighted_Avg_Id
        WHERE WA.Item_Code = @ProductCode 
          AND WA.Trans_Date <= @ToDate
        GROUP BY B.Batch_No
        HAVING SUM(B.Trans_Qty) > 0
        ORDER BY B.Batch_No;
    END
    ELSE IF @LocationName = 'Opening National Qty'
    BEGIN
        SELECT 
            [Batch No],
            CONVERT(VARCHAR(10), MIN([MfgDate]), 105) AS [MFG Date],
            CONVERT(VARCHAR(10), MIN([ExpDate]), 105) AS [Exp Date],
            CAST(SUM([Quantity]) AS DECIMAL(18,2)) AS [Qty]
        FROM (
            SELECT 
                B.Batch_No AS [Batch No],
                MIN(B.Mfg_Date) AS [MfgDate],
                MIN(B.Exp_Date) AS [ExpDate],
                SUM(B.Trans_Qty) AS [Quantity]
            FROM dbo.tbl_INV_Central_BatchWiseStock B WITH (NOLOCK)
            INNER JOIN dbo.tbl_INV_CentralStore_WeightedAvg WA WITH (NOLOCK) ON B.Weighted_Avg_Id = WA.Weighted_Avg_Id
            WHERE WA.Item_Code = @ProductCode 
              AND WA.Trans_Date < @FromDate
            GROUP BY B.Batch_No
            
            UNION ALL
            
            SELECT 
                BatchNo AS [Batch No],
                MIN(MfgDate) AS [MfgDate],
                MIN(ExpDate) AS [ExpDate],
                SUM(StockQty) AS [Quantity]
            FROM dbo.tblDCStore WITH (NOLOCK)
            WHERE ProductCode = @ProductCode 
              AND (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) < @FromDate
            GROUP BY BatchNo
        ) Combined
        GROUP BY [Batch No]
        HAVING SUM([Quantity]) > 0
        ORDER BY [Batch No];
    END
    ELSE IF @LocationName = 'Total Depots Qty'
    BEGIN
        SELECT 
            BatchNo AS [Batch No],
            CONVERT(VARCHAR(10), MIN(MfgDate), 105) AS [MFG Date],
            CONVERT(VARCHAR(10), MIN(ExpDate), 105) AS [Exp Date],
            CAST(SUM(StockQty) AS DECIMAL(18,2)) AS [Qty]
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE ProductCode = @ProductCode 
          AND (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) <= @ToDate
        GROUP BY BatchNo
        HAVING SUM(StockQty) > 0
        ORDER BY BatchNo;
    END
    ELSE IF @LocationName = 'National Stock Qty'
    BEGIN
        SELECT 
            [Batch No],
            CONVERT(VARCHAR(10), MIN([MfgDate]), 105) AS [MFG Date],
            CONVERT(VARCHAR(10), MIN([ExpDate]), 105) AS [Exp Date],
            CAST(SUM([Quantity]) AS DECIMAL(18,2)) AS [Qty]
        FROM (
            SELECT 
                B.Batch_No AS [Batch No],
                MIN(B.Mfg_Date) AS [MfgDate],
                MIN(B.Exp_Date) AS [ExpDate],
                SUM(B.Trans_Qty) AS [Quantity]
            FROM dbo.tbl_INV_Central_BatchWiseStock B WITH (NOLOCK)
            INNER JOIN dbo.tbl_INV_CentralStore_WeightedAvg WA WITH (NOLOCK) ON B.Weighted_Avg_Id = WA.Weighted_Avg_Id
            WHERE WA.Item_Code = @ProductCode 
              AND WA.Trans_Date <= @ToDate
            GROUP BY B.Batch_No
            
            UNION ALL
            
            SELECT 
                BatchNo AS [Batch No],
                MIN(MfgDate) AS [MfgDate],
                MIN(ExpDate) AS [ExpDate],
                SUM(StockQty) AS [Quantity]
            FROM dbo.tblDCStore WITH (NOLOCK)
            WHERE ProductCode = @ProductCode 
              AND (CASE WHEN StockQty < 0 OR Trans_Type = 'Issue' THEN ISNULL(ReferenceTransactionDate, StockRcvDate) ELSE StockRcvDate END) <= @ToDate
            GROUP BY BatchNo
        ) Combined
        GROUP BY [Batch No]
        HAVING SUM([Quantity]) > 0
        ORDER BY [Batch No];
    END
    ELSE
    BEGIN
        -- Specific Depot Closing Stock
        SELECT 
            DC.BatchNo AS [Batch No],
            CONVERT(VARCHAR(10), MIN(DC.MfgDate), 105) AS [MFG Date],
            CONVERT(VARCHAR(10), MIN(DC.ExpDate), 105) AS [Exp Date],
            CAST(SUM(DC.StockQty) AS DECIMAL(18,2)) AS [Qty]
        FROM dbo.tblDCStore DC WITH (NOLOCK)
        INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON DC.ComUnitId = CU.ComUnitId
        WHERE DC.ProductCode = @ProductCode 
          AND CU.ComUnitName = @LocationName
          AND (CASE WHEN DC.StockQty < 0 OR DC.Trans_Type = 'Issue' THEN ISNULL(DC.ReferenceTransactionDate, DC.StockRcvDate) ELSE DC.StockRcvDate END) <= @ToDate
        GROUP BY DC.BatchNo
        HAVING SUM(DC.StockQty) > 0
        ORDER BY DC.BatchNo;
    END
END;
GO

PRINT 'dbo.sp_Report_NationalStockValuation_BatchDetails updated successfully!';
GO
