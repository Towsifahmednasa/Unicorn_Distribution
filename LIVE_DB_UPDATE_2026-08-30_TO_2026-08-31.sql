/* ==========================================================================
   LIVE DATABASE UPDATE SCRIPT (SINGLE SCRIPT)
   Generated: 31-Aug-2026
   ========================================================================== */

-- 1. TABLE SCHEMA UPDATES
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblCustMaster') AND name = 'TaggedSalesPersonIds')
BEGIN
    ALTER TABLE dbo.tblCustMaster ADD TaggedSalesPersonIds NVARCHAR(MAX) NULL;
    PRINT 'Added TaggedSalesPersonIds to tblCustMaster';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblDoctorMaster') AND name = 'TaggedSalesPersonIds')
BEGIN
    ALTER TABLE dbo.tblDoctorMaster ADD TaggedSalesPersonIds NVARCHAR(MAX) NULL;
    PRINT 'Added TaggedSalesPersonIds to tblDoctorMaster';
END
GO

-- Fix column size for sales return temp tables (supports long product names)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblSalesReturnDetail') AND name = 'ProductName' AND max_length < 1000)
BEGIN
    ALTER TABLE dbo.tblSalesReturnDetail ALTER COLUMN ProductName NVARCHAR(500);
    ALTER TABLE dbo.tblSalesReturnDetail ALTER COLUMN ProductCode NVARCHAR(150);
    PRINT 'Altered ProductName and ProductCode on tblSalesReturnDetail';
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tblTempSalesReturnOrder') AND name = 'CustomerName' AND max_length < 500)
BEGIN
    ALTER TABLE dbo.tblTempSalesReturnOrder ALTER COLUMN CustomerName NVARCHAR(250);
    ALTER TABLE dbo.tblTempSalesReturnOrder ALTER COLUMN ComUnitName NVARCHAR(250);
    ALTER TABLE dbo.tblTempSalesReturnOrder ALTER COLUMN MIOName NVARCHAR(250);
    PRINT 'Altered columns on tblTempSalesReturnOrder';
END
GO

-- 2. STORED PROCEDURES
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO


-- ==========================================================================
-- Procedure: sp_Report_DCStore_StockValuationLedger
-- ==========================================================================
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

    -- Opening Stock Calculation (Before FromDate)
    LEFT JOIN (
        SELECT 
            ComUnitId, ProductCode,
            SUM(StockQty) AS OpeningQty,
            CASE WHEN SUM(StockQty) > 0 THEN SUM(TotalCostAmount) / SUM(StockQty) ELSE AVG(CostPerUnit) END AS OpeningRate,
            SUM(TotalCostAmount) AS OpeningValue
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE StockRcvDate < @FromDate
        GROUP BY ComUnitId, ProductCode
    ) OP ON DC.ComUnitId = OP.ComUnitId AND DC.ProductCode = OP.ProductCode

    -- Movements Calculation (Between FromDate and ToDate)
    LEFT JOIN (
        SELECT 
            ComUnitId, ProductCode,
            
            -- Receive From WH: Inbound Receive from Central Warehouse (excluding B2B DC transfer and customer returns)
            SUM(CASE 
                    WHEN (Trans_Type = 'Receive' OR Trans_Type IS NULL OR Trans_Type = 'Available' OR Trans_Type = '') 
                         AND ISNULL(ReturnFromInvoiceDetailId, 0) = 0 
                         AND ISNULL(StockCondition, '') NOT IN ('B2BReceive', 'TransferReceive', 'SalesReturn', 'Return')
                         AND (ChalanNo NOT LIKE 'IDT%' OR ChalanNo IS NULL)
                         AND (ChalanNo NOT LIKE 'RIN%' AND ChalanNo NOT LIKE 'RINV%')
                         AND StockQty > 0
                    THEN StockQty ELSE 0 
                END) AS RcvFromWHQty,

            -- B2B Receive: Inward Transfer from another DC (TransferStockReceiveByDC.aspx / OtherStockAction.aspx)
            SUM(CASE 
                    WHEN (Trans_Type = 'Receive' OR Trans_Type = 'Available')
                         AND (
                             StockCondition IN ('B2BReceive', 'TransferReceive') 
                             OR ChalanNo LIKE 'IDT%'
                         )
                         AND StockQty > 0
                    THEN StockQty ELSE 0 
                END) AS B2BRcvQty,
            
            -- Return Rcv: Customer Return / Sales Return
            SUM(CASE 
                    WHEN ISNULL(ReturnFromInvoiceDetailId, 0) <> 0 
                         OR ISNULL(StockCondition, '') IN ('SalesReturn', 'Return')
                         OR ChalanNo LIKE 'RIN%'
                         OR ChalanNo LIKE 'RINV%'
                    THEN StockQty ELSE 0 
                END) AS ReturnRcvQty,
            
            -- Sales Issue: Customer Deliveries / Sales
            SUM(CASE 
                    WHEN StockQty < 0 AND ISNULL(StockCondition, '') NOT IN ('B2BIssue', 'TransferIssue')
                    THEN ABS(StockQty) 
                    WHEN (Trans_Type = 'Issue' OR Trans_Type = 'Sales') AND ISNULL(StockCondition, '') NOT IN ('B2BIssue', 'TransferIssue')
                    THEN StockQty 
                    ELSE 0 
                END) AS SalesQty,

            -- B2B Issue: Outward Transfer to another DC (NewStockTransferDcToDc.aspx)
            SUM(CASE 
                    WHEN StockCondition IN ('B2BIssue', 'TransferIssue')
                    THEN ABS(StockQty) 
                    ELSE 0 
                END) AS B2BIssueQty

        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE StockRcvDate >= @FromDate AND StockRcvDate <= @ToDate
        GROUP BY ComUnitId, ProductCode
    ) MOV ON DC.ComUnitId = MOV.ComUnitId AND DC.ProductCode = MOV.ProductCode

    -- Latest Cost Per Unit Calculation
    LEFT JOIN (
        SELECT 
            ComUnitId, ProductCode, CostPerUnit,
            ROW_NUMBER() OVER (PARTITION BY ComUnitId, ProductCode ORDER BY StockRcvDate DESC, DCStoreId DESC) AS RowNum
        FROM dbo.tblDCStore WITH (NOLOCK)
        WHERE StockRcvDate <= @ToDate AND CostPerUnit > 0
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


-- ==========================================================================
-- Procedure: sp_Get_CustomerApp
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Get_CustomerApp', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Get_CustomerApp;
GO

CREATE PROCEDURE [dbo].[sp_Get_CustomerApp]
    @param NVARCHAR(MAX) = NULL,
    @Role NVARCHAR(MAX) = NULL,
    @AppStatus NVARCHAR(MAX) = NULL,
    @FromDt DATETIME = NULL,
    @ToDt DATETIME = NULL,
    @EmpId INT = NULL,
    @TerritoryId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @where NVARCHAR(MAX) = N' WHERE 1=1 ';
    SET @where += N' AND ISNULL(mas.ActionStatus, ''0'') NOT IN (''2'', ''3'') ';

    IF (@AppStatus IS NOT NULL AND @AppStatus <> '')
    BEGIN
        SET @where += N' AND (tblCustomerApprovalLog.Status IN (''' + @AppStatus + ''') OR (tblCustomerApprovalLog.CustomerApprovalId IS NULL AND mas.ActionStatus = ''' + @AppStatus + ''')) ';
        
        IF (@FromDt IS NOT NULL AND @ToDt IS NULL)
        BEGIN
            SET @where += N' AND CONVERT(DATE, ISNULL(tblCustomerApprovalLog.EntryDateApp, mas.CreateDate)) = ''' + CONVERT(NVARCHAR(10), @FromDt, 120) + ''' ';
        END

        IF (@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
        BEGIN
            SET @where += N' AND CONVERT(DATE, ISNULL(tblCustomerApprovalLog.EntryDateApp, mas.CreateDate)) BETWEEN ''' + CONVERT(NVARCHAR(10), @FromDt, 120) + ''' AND ''' + CONVERT(NVARCHAR(10), @ToDt, 120) + ''' ';
        END
    END
    ELSE
    BEGIN
        IF (@FromDt IS NOT NULL AND @ToDt IS NULL)
        BEGIN
            SET @where += N' AND CONVERT(DATE, mas.CreateDate) = ''' + CONVERT(NVARCHAR(10), @FromDt, 120) + ''' ';
        END

        IF (@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
        BEGIN
            SET @where += N' AND CONVERT(DATE, mas.CreateDate) BETWEEN ''' + CONVERT(NVARCHAR(10), @FromDt, 120) + ''' AND ''' + CONVERT(NVARCHAR(10), @ToDt, 120) + ''' ';
        END
    END

    IF (@TerritoryId IS NOT NULL)
    BEGIN
        SET @where += N' AND View_Webapi_EmployeeFieldForceInfo.TerritoryId = ' + CAST(@TerritoryId AS NVARCHAR(20)) + ' ';
    END

    DECLARE @sql NVARCHAR(MAX);

    SET @sql = N'
    ;WITH DupCell AS (
        SELECT CellNo 
        FROM dbo.tblCustMaster WITH (NOLOCK) 
        WHERE CellNo IS NOT NULL AND CellNo <> '''' AND ActionStatus <> ''3''
        GROUP BY CellNo 
        HAVING COUNT(*) > 1
    )
    SELECT 
        ISNULL(EGI.EmpName, N''N/A'') AS SalesPersonName,
        CASE WHEN DupCell.CellNo IS NOT NULL THEN ''Already Exists'' ELSE ''New'' END AS ExistenceStatus,
        sst.SMCType,
        ISNULL(RT.RoleType, N''Pending Approval'') AS WaitingForRole,
        ISNULL(mas.ActionStatus, ''0'') AS ApprovalStatus,
        mas.CellNo,
        mas.Address,

        CASE 
            WHEN ISNULL(mas.ActionStatus, ''0'') = ''0'' THEN ''Pending''
            WHEN mas.ActionStatus = ''1'' THEN ''Verified''
            WHEN mas.ActionStatus = ''2'' THEN ''Approved''
            WHEN mas.ActionStatus = ''3'' THEN ''Rejected''
            ELSE mas.ActionStatus 
        END AS ApprovalStatusWeb,

        pt.ProgramTypeName,
        mr.MarketCode,
        mr.MarketName,
        Chmist.CustomerType,

        DR.DistributionRouteName,

        tblEmpGeneralInfo.EmpInfoId,
        mas.CustomerMasterId,
        mas.CustomerName,
        mas.CustomerCode,

        FORMAT(mas.CreateDate, ''dd MMM yyyy hh:mm tt'') AS EntryDate,

        ISNULL(tblCustomerApprovalLog.CustomerApprovalId, 0) AS CustomerApprovalId,
        tblCustomerApprovalLog.Date,
        ISNULL(tblCustomerApprovalLog.FromEmpId, 0) AS FromEmpId,
        ISNULL(tblCustomerApprovalLog.ToEmpId, 0) AS ToEmpId,
        mas.CustomerMasterId AS TableId,
        ISNULL(tblCustomerApprovalLog.Status, ''Pending'') AS Status,
        tblCustomerApprovalLog.Comments,
        ISNULL(tblCustomerApprovalLog.Type, ''Customer'') AS Type,
        ISNULL(tblCustomerApprovalLog.Step, 0) AS Step,

        tblCustomerApprovalLog.GroupId,
        tblCustomerApprovalLog.RegionId,
        tblCustomerApprovalLog.AreaId,
        tblCustomerApprovalLog.TerritoryId,
        tblCustomerApprovalLog.RoleTypeId,
        ISNULL(tblCustomerApprovalLog.ToRoleTypeId, 0) AS ToRoleTypeId,

        ISNULL(tblEmpGeneralInfo.EmpMasterCode + '' : '' + tblEmpGeneralInfo.EmpName, N''N/A'') AS EmpMasterCode,
        tblEmpGeneralInfo.EmpName,

        View_Webapi_EmployeeFieldForceInfo.TerritoryId,
        View_Webapi_EmployeeFieldForceInfo.AreaId,
        View_Webapi_EmployeeFieldForceInfo.RegionId,
        View_Webapi_EmployeeFieldForceInfo.GroupId,
        View_Webapi_EmployeeFieldForceInfo.TerritoryName,
        View_Webapi_EmployeeFieldForceInfo.TerritoryCode,
        View_Webapi_EmployeeFieldForceInfo.AreaCode,
        View_Webapi_EmployeeFieldForceInfo.AreaName,
        View_Webapi_EmployeeFieldForceInfo.RegionCode,
        View_Webapi_EmployeeFieldForceInfo.RegionName,
        View_Webapi_EmployeeFieldForceInfo.GroupName,

        MIOEmpId,
        ASMEMPId,
        RSMEMPId,
        NSMEMPId,

        ISNULL(LogMax.MaxStep, 0) AS MaxStep,
        ISNULL(RT.RoleType, N''Admin/Approval'') AS WaitingRole,
        '''' AS WatingEmployee

    FROM dbo.tblCustMaster mas WITH (NOLOCK)
    LEFT JOIN DupCell ON DupCell.CellNo = mas.CellNo
    LEFT JOIN tblEmpGeneralInfo EGI ON mas.SalesPersonId = EGI.EmpInfoId
    LEFT JOIN dbo.tblCustomerType Chmist ON Chmist.CustomerTypeId = mas.CustomerTypeId
    LEFT JOIN dbo.tblProgramType pt ON pt.ProgramTypeId = mas.ProgramTypeId
    LEFT JOIN dbo.tblDistributionRoute DR ON DR.DistributionRouteId = mas.DistributionRouteId
    LEFT JOIN tblMarket mr ON mas.MarketId = mr.MarketId
    LEFT JOIN tblSubTerritory tr ON tr.SubTerritoryId = mr.SubTerritoryId
    LEFT JOIN tblTerritory terry ON terry.TerritoryId = tr.TerritoryId
    LEFT JOIN tblSMCType sst ON sst.SMCTypeId = mas.SMCTypeId
    LEFT JOIN tblRouteInformationMarketDetail DCdtl ON mas.MarketId = DCdtl.MarketId
    LEFT JOIN tblRouteInformationMaster dcMas ON dcMas.RouteInformationMasterId = DCdtl.RouteInformationMasterId

    LEFT JOIN (
        SELECT TableId, MAX(Step) AS MaxStep
        FROM dbo.tblCustomerApprovalLog WITH (NOLOCK)
        GROUP BY TableId
    ) LogMax ON LogMax.TableId = mas.CustomerMasterId

    LEFT JOIN tblCustomerApprovalLog 
        ON tblCustomerApprovalLog.TableId = mas.CustomerMasterId AND tblCustomerApprovalLog.Step = LogMax.MaxStep

    LEFT JOIN tblUser ON tblUser.UserId = mas.CreateBy
    LEFT JOIN tblEmpGeneralInfo ON tblEmpGeneralInfo.EmpInfoId = tblUser.EmpInfoId
    LEFT JOIN View_Webapi_EmployeeFieldForceInfo 
        ON View_Webapi_EmployeeFieldForceInfo.EmpInfoId = tblEmpGeneralInfo.EmpInfoId

    LEFT JOIN tblRoleType ON tblRoleType.RoleTypeId = tblCustomerApprovalLog.RoleTypeId
    LEFT JOIN tblRoleType RT ON RT.RoleTypeId = tblCustomerApprovalLog.ToRoleTypeId

    ' + @where + '

    ' + CASE WHEN @Role IS NOT NULL AND @Role <> '' THEN ' AND (tblCustomerApprovalLog.CustomerApprovalId IS NULL OR ISNULL(tblRoleType.RoleType,'''') <> ''' + @Role + ''') ' ELSE '' END + '

    ' + ISNULL(@param,'') + '

    ORDER BY mas.CustomerMasterId DESC
    ';

    EXEC sp_executesql @sql;

END
GO


-- ==========================================================================
-- Procedure: sp_Save_CustomerMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Save_CustomerMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Save_CustomerMaster;
GO

CREATE PROCEDURE [dbo].[sp_Save_CustomerMaster]
	 @CustomerMasterId int,
     @CustomerName  nvarchar(max)=null
     ,@Address  nvarchar(max)=null
     ,@CellNo  nvarchar(max)=null
     ,@MarketId  int=NULL
     ,@EmpInfoId INT = null
     ,@MarketName   nvarchar(max)=null
     ,@IsActive   bit=null
     ,@CustomerStation   nvarchar(max)=null
     ,@Division   nvarchar(max)=null
     ,@District   nvarchar(max)=null
     ,@Thana  nvarchar(max)=null
     ,@CustomerType  nvarchar(max)=null
     ,@CustomerTypeId  int=NULL
     ,@DistrictId  int=NULL
     ,@DivisionId  int=NULL
     ,@ThanaId  int=NULL
     ,@StationTypeId   int=NULL
     ,@DistributionRouteId   int=NULL
     ,@OwnerName nvarchar(max)=null
     ,@VoterID nvarchar(max)=null
     ,@TradeLicense nvarchar(max)=null
     ,@DrugLicense nvarchar(max)=null
     ,@PharmacyCouncilCertificate nvarchar(max)=null
     ,@BCDS nvarchar(max)=null
     ,@ActionStatus nvarchar(max)=null
     ,@Email nvarchar(max)=null
     ,@Reamrks nvarchar(max)=null
	 ,@EntryBy int=NULL
	 ,@ProgramTypeId int=NULL
	 ,@ComUnitId int=NULL
	 ,@NSMStationTypeId int=NULL
	 ,@DZSMStationTypeId int=NULL
     ,@TaggedSalesPersonIds nvarchar(500)=NULL
     ,@TaggedSalesPersonCodes nvarchar(500)=NULL
AS
BEGIN
	DECLARE @GroupId INT,@RegionId INT,@AreaId INT,@TerritoryId INT,@SubTerritoryId INT, @Unit Nvarchar(max), @ComUnitName Nvarchar(max), @ComUnitCode Nvarchar(max);

	SELECT @DivisionId=div.DivisionId,@DistrictId=dis.DistrictId, @ThanaId=mr.ThanaId,@SubTerritoryId=sr.SubTerritoryId,@TerritoryId=tr.TerritoryId,@AreaId=ar.AreaId,@RegionId=rg.RegionId,@GroupId=rg.GroupId 
    FROM tblmarket mr WITH (NOLOCK)
	INNER JOIN tblSubTerritory sr WITH (NOLOCK) ON sr.SubTerritoryId=mr.SubTerritoryId
	INNER JOIN tblTerritory tr WITH (NOLOCK) ON sr.TerritoryId=tr.TerritoryId
	INNER JOIN tblArea ar WITH (NOLOCK) ON ar.AreaId=tr.AreaId
	INNER JOIN tblRegion rg WITH (NOLOCK) ON ar.RegionId=rg.RegionId
	LEFT JOIN tbl_Thana tha WITH (NOLOCK) ON mr.ThanaId=tha.ThanaId
	LEFT JOIN tbl_District dis WITH (NOLOCK) ON dis.DistrictId=tha.district_id
	LEFT JOIN tbl_Division div WITH (NOLOCK) ON dis.DivisionId=div.DivisionId
	WHERE MarketId=@marketId;

	SELECT @StationTypeId = ISNULL(StationTypeId,null) FROM tblMarketStationDetail WHERE MarketId=@marketId AND UserRoleID=1;
	SELECT @NSMStationTypeId = ISNULL(StationTypeId,null) FROM tblMarketStationDetail WHERE MarketId=@marketId AND UserRoleID=2;
	SELECT @DZSMStationTypeId = ISNULL(StationTypeId,null) FROM tblMarketStationDetail WHERE MarketId=@marketId AND UserRoleID=3;

    SELECT @ComUnitName=cunit.ComUnitName,@ComUnitCode=cunit.ComUnitCode FROM tblCompanyUnit cunit WHERE cunit.ComUnitId=@ComUnitId;

	DECLARE @CustCode NVARCHAR(MAX);
	DECLARE @CustCodeint INT;
    SELECT @CustCodeint=MAX(CONVERT(INT,SUBSTRING(CustomerCode,2,LEN(CustomerCode)+1)))+1 FROM dbo.tblCustMaster WHERE ActionStatus='2';
    SET @CustCode='C'+CONVERT(NVARCHAR(MAX),@CustCodeint);

    IF @TaggedSalesPersonIds IS NULL AND @EmpInfoId IS NOT NULL
    BEGIN
        SET @TaggedSalesPersonIds = CAST(@EmpInfoId AS NVARCHAR(50));
    END

    INSERT INTO [dbo].[tblCustMaster]
    (
        [CustomerName],[Address],[CellNo],[MarketId],[MarketName],[IsActive],[CustomerStation],[Division],[District],[Thana],
        [CustomerType],[CustomerTypeId],[DistrictId],[DivisionId],[ThanaId],[StationTypeId],[CreateBy],[CreateDate],
        [DistributionRouteId],[OwnerName],[VoterID],[TradeLicense],[DrugLicense],[PharmacyCouncilCertificate],[BCDS],
        [ActionStatus],[Email],[Reamrks],ProgramTypeId, CustomerCode,GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId, 
        NSMStationTypeId, DZSMStationTypeId, ComUnitId,ComUnitName, ComUnitCode,SalesPersonId, TaggedSalesPersonIds, TaggedSalesPersonCodes
    )
    VALUES
    (
        @CustomerName, @Address, @CellNo, @MarketId, @MarketName, @IsActive, @CustomerStation, @Division, @District, @Thana,
        @CustomerType, @CustomerTypeId, @DistrictId, @DivisionId, @ThanaId, @StationTypeId, @EntryBy, GETDATE(),
        @DistributionRouteId, @OwnerName, @VoterID, @TradeLicense, @DrugLicense, @PharmacyCouncilCertificate, @BCDS,
        '2', @Email, @Reamrks, @ProgramTypeId, @CustCode, @GroupId, @RegionId, @AreaId, @TerritoryId, @SubTerritoryId,
        @NSMStationTypeId, @DZSMStationTypeId, @ComUnitId, @ComUnitName, @ComUnitCode, @EmpInfoId, @TaggedSalesPersonIds, @TaggedSalesPersonCodes
    );

    SELECT SCOPE_IDENTITY();
END
GO


-- ==========================================================================
-- Procedure: sp_UD_CustomerMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_UD_CustomerMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_UD_CustomerMaster;
GO

CREATE PROCEDURE [dbo].[sp_UD_CustomerMaster] 
 @CustomerMasterId INT,
 @CustomerCode NVARCHAR(MAX) = NULL,
 @CategoryId INT = NULL,
 @CustomerName NVARCHAR(MAX) = NULL,
 @Address NVARCHAR(MAX) = NULL,
 @CellNo NVARCHAR(MAX) = NULL,
 @Addrees2 NVARCHAR(MAX) = NULL,
 @City NVARCHAR(MAX) = NULL,
 @ShippingCond NVARCHAR(MAX) = NULL,
 @MarketCode NVARCHAR(MAX) = NULL,
 @MarketName NVARCHAR(MAX) = NULL,
 @MIACode NVARCHAR(MAX) = NULL,
 @MiaName NVARCHAR(MAX) = NULL,
 @AreaCode NVARCHAR(MAX) = NULL,
 @DisCode NVARCHAR(MAX) = NULL,
 @FEName NVARCHAR(MAX) = NULL,
 @ComUnitCode NVARCHAR(MAX) = NULL,
 @ComUnitName NVARCHAR(MAX) = NULL,
 @RegionCode NVARCHAR(MAX) = NULL,
 @DZSMName NVARCHAR(MAX) = NULL,
 @TermOfPayment NVARCHAR(MAX) = NULL,
 @ConPerson NVARCHAR(MAX) = NULL,
 @FixedCustomer bit,
 @LoginName NVARCHAR(MAX) = NULL,
 @IsActive bit,
 @InActiveDate datetime,
 @TaggedSalesPersonIds nvarchar(500) = NULL,
 @TaggedSalesPersonCodes nvarchar(500) = NULL
AS
BEGIN 
 UPDATE dbo.tblCustMaster 
 SET ConPerson=@ConPerson,FixedCustomer=@FixedCustomer,IsActive=@IsActive,
 CategoryId = @CategoryId, CustomerName = @CustomerName, Address = @Address,
 CellNo = @CellNo, Addrees2 = @Addrees2, City = @City, ShippingCond = @ShippingCond,
 MarketCode = @MarketCode,MarketName = @MarketName, MIACode = @MIACode,AreaCode = @AreaCode, DisCode = @DisCode, 
 FEName = @FEName, ComUnitCode = @ComUnitCode, ComUnitName = @ComUnitName, RegionCode = @RegionCode, DZSMName = @DZSMName,TermOfPayment = @TermOfPayment,
 TaggedSalesPersonIds = ISNULL(@TaggedSalesPersonIds, TaggedSalesPersonIds),
 TaggedSalesPersonCodes = ISNULL(@TaggedSalesPersonCodes, TaggedSalesPersonCodes)
 WHERE CustomerMasterId = @CustomerMasterId
END
GO


-- ==========================================================================
-- Procedure: sp_Update_CustomerMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Update_CustomerMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Update_CustomerMaster;
GO

CREATE PROCEDURE [dbo].[sp_Update_CustomerMaster]
	 @CustomerMasterId int,
     @CustomerName  nvarchar(max)=null
     ,@Address  nvarchar(max)=null
     ,@CellNo  nvarchar(max)=null
     ,@MarketId  int=NULL
     ,@EmpInfoId INT = null
     ,@MarketName   nvarchar(max)=null
     ,@IsActive   bit=null
     ,@CustomerStation   nvarchar(max)=null
     ,@Division   nvarchar(max)=null
     ,@District   nvarchar(max)=null
     ,@Thana  nvarchar(max)=null
     ,@CustomerType  nvarchar(max)=null
     ,@CustomerTypeId  int=NULL
     ,@DistrictId  int=NULL
     ,@DivisionId  int=NULL
     ,@ThanaId  int=NULL
     ,@StationTypeId   int=NULL
     ,@DistributionRouteId   int=NULL
     ,@OwnerName nvarchar(max)=null
     ,@VoterID nvarchar(max)=null
     ,@TradeLicense nvarchar(max)=null
     ,@DrugLicense nvarchar(max)=null
     ,@PharmacyCouncilCertificate nvarchar(max)=null
     ,@BCDS nvarchar(max)=null
     ,@ActionStatus nvarchar(max)=null
     ,@Email nvarchar(max)=null
     ,@Reamrks nvarchar(max)=null
	 ,@UpdateBy int=NULL
	 ,@ProgramTypeId int=NULL
	 ,@ComUnitId int=NULL
	 ,@NSMStationTypeId int=NULL
	 ,@DZSMStationTypeId int=NULL
     ,@TaggedSalesPersonIds nvarchar(500)=NULL
     ,@TaggedSalesPersonCodes nvarchar(500)=NULL
AS
BEGIN
	DECLARE @GroupId INT,@RegionId INT,@AreaId INT,@TerritoryId INT,@SubTerritoryId INT, @Unit Nvarchar(max), @ComUnitName Nvarchar(max), @ComUnitCode Nvarchar(max);

	SELECT @DivisionId=div.DivisionId,@DistrictId=dis.DistrictId, @ThanaId=mr.ThanaId,@SubTerritoryId=sr.SubTerritoryId,@TerritoryId=tr.TerritoryId,@AreaId=ar.AreaId,@RegionId=rg.RegionId,@GroupId=rg.GroupId 
    FROM tblmarket mr WITH (NOLOCK)
	INNER JOIN tblSubTerritory sr WITH (NOLOCK) ON sr.SubTerritoryId=mr.SubTerritoryId
	INNER JOIN tblTerritory tr WITH (NOLOCK) ON sr.TerritoryId=tr.TerritoryId
	INNER JOIN tblArea ar WITH (NOLOCK) ON ar.AreaId=tr.AreaId
	INNER JOIN tblRegion rg WITH (NOLOCK) ON ar.RegionId=rg.RegionId
	LEFT JOIN tbl_Thana tha WITH (NOLOCK) ON mr.ThanaId=tha.ThanaId
	LEFT JOIN tbl_District dis WITH (NOLOCK) ON dis.DistrictId=tha.district_id
	LEFT JOIN tbl_Division div WITH (NOLOCK) ON dis.DivisionId=div.DivisionId
	WHERE MarketId=@marketId;

    IF @TaggedSalesPersonIds IS NULL AND @EmpInfoId IS NOT NULL
    BEGIN
        SET @TaggedSalesPersonIds = CAST(@EmpInfoId AS NVARCHAR(50));
    END

    UPDATE [dbo].[tblCustMaster]
    SET 
        [CustomerName]=@CustomerName,
        [Address]=@Address,
        [CellNo]=@CellNo,
		GroupId=@GroupId,RegionId=@RegionId,AreaId=@AreaId,TerritoryId=@TerritoryId,SubTerritoryId=@SubTerritoryId,
        [MarketId]=@MarketId,
        [MarketName]=@MarketName,
        [IsActive]=@IsActive,
        [CustomerStation]=@CustomerStation,
        [Division]=@Division,
        [District]=@District,
        [Thana]=@Thana,
        [CustomerType]=@CustomerType,
        [CustomerTypeId]=@CustomerTypeId,
        [DistrictId]=@DistrictId,
        [DivisionId]=@DivisionId,
        [ThanaId]=@ThanaId,
        [StationTypeId]=@StationTypeId,
        UpdateBy=@UpdateBy,
        UpdateDate=GETDATE(),
		SalesPersonId = @EmpInfoId,
        TaggedSalesPersonIds = @TaggedSalesPersonIds,
        TaggedSalesPersonCodes = @TaggedSalesPersonCodes,
        [DistributionRouteId]=@DistributionRouteId,
        [OwnerName]=@OwnerName,
        [VoterID]=@VoterID,
        [TradeLicense]=@TradeLicense,
        [DrugLicense]=@DrugLicense,
        [PharmacyCouncilCertificate]=@PharmacyCouncilCertificate,
        [BCDS]=@BCDS,
        [Email]=@Email,
        [Reamrks]=@Reamrks,
        ProgramTypeId=@ProgramTypeId, 
        NSMStationTypeId=@NSMStationTypeId, 
        DZSMStationTypeId=@DZSMStationTypeId
	WHERE CustomerMasterId=@CustomerMasterId;

	DELETE FROM tblCustProductLine WHERE CustomerMasterId=@CustomerMasterId;

    SELECT @StationTypeId = ISNULL(StationTypeId,null) FROM tblMarketStationDetail WHERE MarketId=@MarketId AND UserRoleID=1;
	SELECT @NSMStationTypeId = ISNULL(StationTypeId,null) FROM tblMarketStationDetail WHERE MarketId=@MarketId AND UserRoleID=2;
	SELECT @DZSMStationTypeId = ISNULL(StationTypeId,null) FROM tblMarketStationDetail WHERE MarketId=@MarketId AND UserRoleID=3;

    DECLARE @divId INT, @disId INT, @thana_Id INT;
    SELECT @divId=div.DivisionId, @disId=dis.DistrictId, @thana_Id=mr.ThanaId 
    FROM tblmarket mr WITH (NOLOCK)
	LEFT JOIN tbl_Thana tha WITH (NOLOCK) ON mr.ThanaId=tha.ThanaId
	LEFT JOIN tbl_District dis WITH (NOLOCK) ON dis.DistrictId=tha.district_id
	LEFT JOIN tbl_Division div WITH (NOLOCK) ON dis.DivisionId=div.DivisionId 
	WHERE MarketId=@MarketId;

    UPDATE dbo.tblCustMaster 
    SET UpdateBy=@UpdateBy, UpdateDate=GETDATE(), ThanaId=@thana_Id, DistrictId=@disId, DivisionId=@divId, StationTypeId=@StationTypeId, NSMStationTypeId=@NSMStationTypeId, DZSMStationTypeId=@DZSMStationTypeId
    WHERE CustomerMasterId=@CustomerMasterId;
END
GO


-- ==========================================================================
-- Procedure: sp_Save_DoctorMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Save_DoctorMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Save_DoctorMaster;
GO

CREATE PROCEDURE [dbo].[sp_Save_DoctorMaster]
	  @DoctorId int 
      ,@SecondaryCode  nvarchar(max)=null
      ,@DesignationId int=null
	  ,@Gender  nvarchar(max)=null
      ,@DoctorName nvarchar(max)=null
	  ,@IsActive  bit=null
      ,@Activedate   datetime=null
      ,@InactiveDate   datetime=null
      ,@EntryBy  int=null
	  ,@DivisionId  int=null
      ,@DistrictId  int=null
      ,@ThanaId  int=null
	  ,@UPCode nvarchar(max)=null
      ,@DoctorTypeId  int=null
      ,@MarketId  int=null
      ,@UnionName  nvarchar(max)=null
      ,@Reamrks  nvarchar(max)=null
	  ,@StreetAddress  nvarchar(max)=null
      ,@StationTypeId   int=null
	  ,@ProgramTypeId   int=null
	  ,@DoctorCategoryId   int=null
	  ,@SMCTypeId   int=null
	  ,@SalesPersonId INT = NULL
      ,@TaggedSalesPersonIds nvarchar(500) = NULL
      ,@TaggedSalesPersonCodes nvarchar(500) = NULL
AS
BEGIN
	DECLARE @CustCode NVARCHAR(MAX);
	DECLARE @CustCodeint INT;
    SELECT @CustCodeint=Max(CONVERT(INT,SUBSTRING(DoctorCode,2,LEN(DoctorCode)+1)))+1 FROM dbo.tblDoctorMaster WHERE ApprovalStatus='2';
    SET @CustCode='D'+CONVERT(NVARCHAR(MAX),@CustCodeint);

    IF @TaggedSalesPersonIds IS NULL AND @SalesPersonId IS NOT NULL
    BEGIN
        SET @TaggedSalesPersonIds = CAST(@SalesPersonId AS NVARCHAR(50));
    END

	INSERT INTO tblDoctorMaster
    (
        DoctorName, [DoctorCode], [SecondaryCode], [DesignationId], [Gender], [IsActive], [Activedate], [EntryBy], [EntryDate],
        [DivisionId], [DistrictId], [ThanaId], [IsFromApp], [ApprovalStatus], [UPCode], [DoctorTypeId],
        [MarketId], [UnionName], [Reamrks], [StationTypeId], ProgramTypeId, DoctorCategoryId, InactiveDate, SMCTypeId, SalesPersonId, TaggedSalesPersonIds, TaggedSalesPersonCodes, StreetAddress
    )
    VALUES
    (
        @DoctorName, @CustCode, @SecondaryCode, @DesignationId, @Gender, @IsActive, @Activedate, @EntryBy, GETDATE(),
        @DivisionId, @DistrictId, @ThanaId, 0, '2', @UPCode, @DoctorTypeId,
        @MarketId, @UnionName, @Reamrks, @StationTypeId, @ProgramTypeId, @DoctorCategoryId, @InactiveDate, @SMCTypeId, @SalesPersonId, @TaggedSalesPersonIds, @TaggedSalesPersonCodes, @StreetAddress
    );

	SELECT SCOPE_IDENTITY();
END
GO


-- ==========================================================================
-- Procedure: sp_Update_DoctorMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Update_DoctorMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Update_DoctorMaster;
GO

CREATE PROCEDURE [dbo].[sp_Update_DoctorMaster]
	   @DoctorId int 
      ,@SecondaryCode  nvarchar(max)=null
      ,@DesignationId int=null
	  ,@Gender  nvarchar(max)=null
      ,@DoctorName nvarchar(max)=null
	  ,@IsActive  bit=null
      ,@Activedate   datetime=null
      ,@InactiveDate   datetime=null
	  ,@DivisionId  int=null
      ,@DistrictId  int=null
      ,@ThanaId  int=null
	  ,@UPCode nvarchar(max)=null
      ,@DoctorTypeId  int=null
      ,@MarketId  int=null
      ,@UnionName  nvarchar(max)=null
      ,@Reamrks  nvarchar(max)=null
	  ,@StreetAddress  nvarchar(max)=null
      ,@StationTypeId   int=null
	  ,@ProgramTypeId   int=null
	  ,@DoctorCategoryId   int=null
	  ,@UpdateBy int
	  ,@SMCTypeId   int=null
	  ,@SalesPersonId INT = NULL
      ,@TaggedSalesPersonIds nvarchar(500) = NULL
      ,@TaggedSalesPersonCodes nvarchar(500) = NULL
AS
BEGIN
    IF @TaggedSalesPersonIds IS NULL AND @SalesPersonId IS NOT NULL
    BEGIN
        SET @TaggedSalesPersonIds = CAST(@SalesPersonId AS NVARCHAR(50));
    END

    UPDATE dbo.tblDoctorMaster
    SET DoctorName=@DoctorName,
        [SecondaryCode]=@SecondaryCode,
        [DesignationId]=@DesignationId,
        [Gender]=@Gender,
        [IsActive]=@IsActive,
        [Activedate]=@Activedate,
        UpdateBy=@UpdateBy,
        UpdateDate=GETDATE(),
	    StreetAddress = @StreetAddress,
        [DivisionId]=@DivisionId,
        [DistrictId]=@DistrictId,
        [ThanaId]=@ThanaId,
        [UPCode]=@UPCode,
        [DoctorTypeId]=@DoctorTypeId,
        [MarketId]=@MarketId,
        [UnionName]=@UnionName,
        [Reamrks]=@Reamrks,
        [StationTypeId]=@StationTypeId,
        ProgramTypeId=@ProgramTypeId,
        DoctorCategoryId=@DoctorCategoryId,
        InactiveDate=@InactiveDate,
        SMCTypeId=@SMCTypeId,
	    SalesPersonId = @SalesPersonId,
        TaggedSalesPersonIds = @TaggedSalesPersonIds,
        TaggedSalesPersonCodes = @TaggedSalesPersonCodes
    WHERE DoctorId = @DoctorId;

    DELETE FROM dbo.tblDoctorBrandDetail WHERE DoctorId = @DoctorId;
	DELETE FROM dbo.tblDoctorDegreeDetail WHERE DoctorId = @DoctorId;
	DELETE FROM dbo.tblDoctorSpecialityDetail WHERE DoctorId = @DoctorId;
	DELETE FROM dbo.tblDoctorSpecialDayDetail WHERE DoctorId = @DoctorId;
	DELETE FROM dbo.tblDoctorContactDetail WHERE DoctorId = @DoctorId;

	DELETE FROM dbo.tblDoctorChemberDetail 
    WHERE ChemberId NOT IN (
        SELECT ChemberId FROM (
		    SELECT ChemberId FROM tbl_PrescriptionMaster WHERE DoctorId=@DoctorId
		    UNION ALL
		    SELECT ChemberId FROM tbl_DCRInfo WHERE DoctorId=@DoctorId
        ) tbl
    ) AND DoctorId=@DoctorId;
END
GO


-- ==========================================================================
-- Procedure: sp_Get_CustMasterList_Approve
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Get_CustMasterList_Approve', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Get_CustMasterList_Approve;
GO

CREATE PROCEDURE [dbo].[sp_Get_CustMasterList_Approve]
	@Parm nvarchar(max)=null
AS
BEGIN
SET NOCOUNT ON;
   DECLARE @Q NVARCHAR(MAX)='
 SELECT mas.CustomerBsPCode, tblcom.Dept, tblMIO.StationTypeName , div.DivisionName,dis.DistrictName, tha.ThanaName,
 St.SMCType, dbo.Remove_SpecialCharacters(Address) Address, 
 ISNULL(mas.CustomerCode+'' : '','''') +ISNULL( + case when mas.IsActive=1 then mas.CustomerName else mas.CustomerName +'' (Inactive) '' end ,'''') +ISNULL('' : ''+mas.CellNo,'''') as CustomerSearch, 
 tblEmpGeneralInfo.EmpMasterCode +'' : ''+ tblEmpGeneralInfo.EmpName EmpMasterCode,
 ISNULL(
     STUFF((
         SELECT '', '' + e.EmpMasterCode + '' : '' + e.EmpName
         FROM dbo.tblEmpGeneralInfo e WITH (NOLOCK)
         WHERE ('','' + ISNULL(mas.TaggedSalesPersonIds, '''') + '','') LIKE ''%,'' + CAST(e.EmpInfoId AS NVARCHAR(50)) + '',%''
         FOR XML PATH('''')), 1, 2, ''''),
     EGI.EmpMasterCode + '' : '' + EGI.EmpName
 ) AS SalaesPersonName,
 rg.RegionCode, Ar.AreaCode, Tr.TerritoryCode,case when mas.ActionStatus=''0'' then ''Pending''  
 when mas.ActionStatus=''1'' then ''Verified'' when mas.ActionStatus=''2'' then ''Approved'' when mas.ActionStatus=''3'' then ''Rejected'' else mas.ActionStatus end ApprovalStatus, 
 pt.ProgramTypeName, mr.MarketCode,mr.MarketName MarketName, Chmist.CustomerType,dcMas.RouteName DistributionRouteName,  
 FORMAT(mas.CreateDate,''dd MMM yyyy hh:mm tt'') EntryDate, * from dbo.tblCustMaster mas WITH (NOLOCK) 
 LEFT JOIN dbo.tblCustomerType Chmist WITH (NOLOCK) ON Chmist.CustomerTypeId = mas.CustomerTypeId
 LEFT JOIN dbo.tblProgramType pt WITH (NOLOCK) ON pt.ProgramTypeId = mas.ProgramTypeId
 left join tblUser with (nolock) on tblUser.UserId=mas.CreateBy
 LEFT JOIN dbo.tblEmpGeneralInfo with (nolock) ON tblEmpGeneralInfo.EmpInfoId = tblUser.EmpInfoId
 left join tblMarket mr WITH (NOLOCK) on mas.MarketId=mr.MarketId
 LEFT JOIN (select MarketId,st.StationTypeName from tblMarketStationDetail dtl
		inner join tblStationType st on st.StationTypeId=dtl.StationTypeId 
		 where dtl.UserRoleID=1) tblMIO ON tblMIO.MarketId = mr.MarketId

 LEFT JOIN (select dtl.MarketId,com.ComUnitCode+'' : ''+ComUnitName Dept from tblRouteInformationMarketDetail dtl WITH (NOLOCK) 
		inner join tblRouteInformationMaster mas WITH (NOLOCK) on mas.RouteInformationMasterId=dtl.RouteInformationMasterId 
		inner join tblCompanyUnit com WITH (NOLOCK) on mas.DCId=com.ComUnitId 
 ) tblcom ON tblcom.MarketId = mr.MarketId

 LEFT JOIN tblEmpGeneralInfo AS EGI ON mas.SalesPersonId = EGI.EmpInfoId
 LEFT JOIN tbl_Thana tha with (nolock) ON tha.ThanaId = mr.ThanaId
 LEFT JOIN tbl_District dis with (nolock) ON tha.district_id = dis.DistrictId
 LEFT JOIN tbl_Division div with (nolock) ON div.DivisionId = dis.DivisionId
 left join tblSubTerritory subTr WITH (NOLOCK) on subTr.SubTerritoryId=mr.SubTerritoryId
 left join tblTerritory Tr WITH (NOLOCK) on subTr.TerritoryId=Tr.TerritoryId
 left join tblArea Ar WITH (NOLOCK) on Ar.AreaId=Tr.AreaId
 left join tblRegion rg WITH (NOLOCK) on Ar.RegionId=rg.RegionId
 left join tbl_Group gr WITH (NOLOCK) on gr.GroupId=rg.GroupId
 left JOIN tblRouteInformationMarketDetail DCdtl with (nolock) on mas.MarketId=DCdtl.MarketId
 left join tblRouteInformationMaster dcMas with (nolock) on dcMas.RouteInformationMasterId=DCdtl.RouteInformationMasterId
 left join tblSMCType St WITH (NOLOCK) on mas.SMCTypeId=St.SMCTypeId
 where mas.CustomerMasterId is not null
 '+@Parm +' order by CustomerCode desc ';

EXEC sp_executesql @Q;
END
GO


-- ==========================================================================
-- Procedure: sp_Get_DoctorList
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Get_DoctorList', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Get_DoctorList;
GO

CREATE PROCEDURE [dbo].[sp_Get_DoctorList]
	@Parm nvarchar(max) = ''
AS
BEGIN
   SET NOCOUNT ON;
   SET @Parm = ISNULL(@Parm, '');
   DECLARE @Q NVARCHAR(MAX) = N'
   SELECT 
   ISNULL(
       STUFF((
           SELECT '', '' + e.EmpMasterCode + '' : '' + e.EmpName
           FROM dbo.tblEmpGeneralInfo e WITH (NOLOCK)
           WHERE ('','' + ISNULL(DM.TaggedSalesPersonIds, '''') + '','') LIKE ''%,'' + CAST(e.EmpInfoId AS NVARCHAR(50)) + '',%''
           FOR XML PATH('''')), 1, 2, ''''),
       EGI.EmpMasterCode + '' : '' + EGI.EmpName
   ) AS SalesPersonName,
   STUFF((SELECT CASE WHEN dChemT.chamberName IS NOT NULL THEN CHAR(10) + '' Chamber Type: '' + dChemT.chamberName ELSE '''' END + CASE WHEN dChem.Name IS NOT NULL THEN '', Chamber Name: '' + dChem.Name
                    ELSE '''' END + CASE WHEN dChem.Phone IS NOT NULL THEN '', Chamber Phone: '' + dChem.Phone
                    ELSE '''' END + CASE WHEN dChem.Address IS NOT NULL THEN '', Chamber Address: '' + dChem.Address ELSE '''' END FROM tblDoctorChemberDetail dChem WITH (NOLOCK) LEFT JOIN tblDoctorChamber dChemT WITH (NOLOCK) ON dChemT.ChamberId = dChem.ChamberTypeId WHERE dChem.DoctorId=DM.DoctorId
           FOR XML PATH('''')), 1, 2, '''') AS ChamberInfo, tblcom.Dept, tblMIO.StationTypeName , St.SMCType,STUFF( (SELECT CONCAT('','', mm.Contact , '''') FROM tblDoctorContactDetail mm (NOLOCK) WHERE mm.DoctorId=DM.DoctorId ORDER BY mm.ContactId FOR XML PATH ('''') ),1,1,'''') AS ContactName, dt.DoctorTypeName, pt.ProgramTypeName,mr.MarketCode, rg.RegionCode, Ar.AreaCode, Tr.TerritoryCode, case when DM.ApprovalStatus=''0'' then ''Pending'' when DM.ApprovalStatus=''1'' then ''Verified'' when DM.ApprovalStatus=''2'' then ''Approved'' when DM.ApprovalStatus=''3'' then ''Rejected'' else DM.ApprovalStatus end ApprovalStatusWeb, DM.DoctorId, mr.MarketName, CONVERT(NVARCHAR(50),DM.EntryDate,106)AS EntryDate, dgs.DesignationName DesigName, DM.DoctorName, Us.UserName as UserEntryBy, Up.UserName UserUpdateBy, STUFF( (SELECT CONCAT('','', mm.DegreeName , '''') FROM tblDoctorDegree mm (NOLOCK) INNER JOIN dbo.tblDoctorDegreeDetail mgd ON mgd.DegId=mm.DegreeId WHERE mgd.DoctorId=DM.DoctorId ORDER BY mgd.DoctorDegId FOR XML PATH ('''') ),1,1,'''') AS DegreeName, STUFF( (SELECT CONCAT('','', mm.SpecialityName , '''') FROM tblDoctorSpeciality mm (NOLOCK) INNER JOIN dbo.tblDoctorSpecialityDetail mgd ON mgd.SpecialityId=mm.SpecialityId WHERE mgd.DoctorId=DM.DoctorId ORDER BY mgd.DoctorSpId FOR XML PATH ('''') ),1,1,'''') as DoctorSpeciality , STUFF( (SELECT CONCAT('','', mm.ProgramType , '''') FROM tblDoctorProgramType mm with (NOLOCK) INNER JOIN dbo.tblDoctorProgramTypeDetail mgd ON mgd.ProgramTypeId=mm.ProgramTypeId WHERE mgd.DoctorId=DM.DoctorId ORDER BY mgd. DoctorTypeDetailId FOR XML PATH ('''') ),1,1,'''') as ProgramType, * from tblDoctorMaster DM
   Left Join dbo.tblDoctorDesignation dgs with (NOLOCK) ON dgs.DesignationId= DM.DesignationId
   LEFT JOIN tblEmpGeneralInfo AS EGI with (NOLOCK) ON DM.SalesPersonId = EGI.EmpInfoId
   Left Join dbo.tblProgramType pt with (NOLOCK) ON pt.ProgramTypeId= DM.ProgramTypeId
   Left Join dbo.tblDoctorType dt with (NOLOCK) ON dt.DoctorTypeId= DM.DoctorTypeId
   left join tblMarket mr WITH (NOLOCK) on DM.MarketId=mr.MarketId
   LEFT JOIN (select MarketId,st.StationTypeName from tblMarketStationDetail dtl WITH (NOLOCK) 
		inner join tblStationType st WITH (NOLOCK) on st.StationTypeId=dtl.StationTypeId 
		 where dtl.UserRoleID=1) tblMIO ON tblMIO.MarketId = mr.MarketId 
   LEFT JOIN (select dtl.MarketId,com.ComUnitCode+'' : ''+ComUnitName Dept from tblRouteInformationMarketDetail dtl
		inner join tblRouteInformationMaster mas on mas.RouteInformationMasterId=dtl.RouteInformationMasterId 
		inner join tblCompanyUnit com on mas.DCId=com.ComUnitId 
   ) tblcom ON tblcom.MarketId = mr.MarketId
   left join tblSubTerritory subTr WITH (NOLOCK) on subTr.SubTerritoryId=mr.SubTerritoryId
   left join tblTerritory Tr WITH (NOLOCK) on subTr.TerritoryId=Tr.TerritoryId
   left join tblArea Ar WITH (NOLOCK) on Ar.AreaId=Tr.AreaId
   left join tblRegion rg WITH (NOLOCK) on Ar.RegionId=rg.RegionId
   left join tbl_Group gr WITH (NOLOCK) on gr.GroupId=rg.GroupId
   left join tblSMCType St WITH (NOLOCK) on DM.SMCTypeId=St.SMCTypeId
   Left Join tblUser Us with (NOLOCK) ON DM.EntryBy= Us.UserId
   Left Join tblUser Up with (NOLOCK) ON DM.UpdateBy= Up.UserId 
   where DM.DoctorId is not null and isnull(DM.IsDoctor,0)=1
   ' + @Parm + ' 
   ORDER BY DM.DoctorCode DESC';

   EXEC sp_executesql @Q;
END
GO


-- ==========================================================================
-- Procedure: sp_Save_ManualOrderMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Save_ManualOrderMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Save_ManualOrderMaster;
GO

CREATE PROCEDURE [dbo].[sp_Save_ManualOrderMaster]
    @OrderCode NVARCHAR(50) OUTPUT,
    @TerritoryCode NVARCHAR(50) = NULL,
    @ComUnitId INT = NULL,
    @ComUnitCode NVARCHAR(50) = NULL,
    @ComUnitName NVARCHAR(200) = NULL,
    @MIOCode NVARCHAR(50) = NULL,
    @MIOName NVARCHAR(200) = NULL,
    @MIOId INT = NULL,
    @ManufacId INT = 1,
    @CustomerCode NVARCHAR(50) = NULL,
    @CustomerName NVARCHAR(200) = NULL,
    @GrossValue DECIMAL(18,4) = 0,
    @TotalVat DECIMAL(18,4) = 0,
    @TotalDiscount DECIMAL(18,4) = 0,
    @TotalNetPayable DECIMAL(18,4) = 0,
    @SubmissionDate DATETIME = NULL,
    @FixedCustomer BIT = 0,
    @Remarks NVARCHAR(MAX) = NULL,
    @OrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CustMasterId INT = NULL, @MarketId INT = NULL, @CustTypeId INT = NULL, @ProgramTypeId INT = NULL, @DistributionRouteId INT = NULL;
    DECLARE @GroupId INT = NULL, @RegionId INT = NULL, @AreaId INT = NULL, @TerritoryId INT = NULL, @SubTerritoryId INT = NULL;
    DECLARE @GroupName_Ord NVARCHAR(200) = NULL, @RegionName_Ord NVARCHAR(200) = NULL, @AreaName_Ord NVARCHAR(200) = NULL;
    DECLARE @TerritoryName_Ord NVARCHAR(200) = NULL, @SubTerritoryName_Ord NVARCHAR(200) = NULL, @MarketName_Ord NVARCHAR(200) = NULL;
    DECLARE @SmcTypeId_Ord INT = NULL, @SMCType_Ord NVARCHAR(200) = NULL;

    SELECT TOP 1 
        @CustMasterId = mas.CustomerMasterId,
        @MarketId = mas.MarketId,
        @CustTypeId = mas.CustomerTypeId,
        @ProgramTypeId = mas.ProgramTypeId,
        @DistributionRouteId = mas.DistributionRouteId,
        @SmcTypeId_Ord = mas.SMCTypeId,
        @SMCType_Ord = st.SMCType,
        @SubTerritoryId = subTr.SubTerritoryId,
        @TerritoryId = Tr.TerritoryId,
        @AreaId = Ar.AreaId,
        @RegionId = rg.RegionId,
        @GroupId = gr.GroupId,
        @MarketName_Ord = mr.MarketName,
        @SubTerritoryName_Ord = subTr.SubTerritoryName,
        @TerritoryName_Ord = Tr.TerritoryName,
        @AreaName_Ord = Ar.AreaName,
        @RegionName_Ord = rg.RegionName,
        @GroupName_Ord = gr.GroupName
    FROM dbo.tblCustMaster mas WITH (NOLOCK)
    LEFT JOIN dbo.tblSMCType st WITH (NOLOCK) ON mas.SMCTypeId = st.SMCTypeId
    LEFT JOIN dbo.tblMarket mr WITH (NOLOCK) ON mas.MarketId = mr.MarketId
    LEFT JOIN dbo.tblSubTerritory subTr WITH (NOLOCK) ON mr.SubTerritoryId = subTr.SubTerritoryId
    LEFT JOIN dbo.tblTerritory Tr WITH (NOLOCK) ON subTr.TerritoryId = Tr.TerritoryId
    LEFT JOIN dbo.tblArea Ar WITH (NOLOCK) ON Tr.AreaId = Ar.AreaId
    LEFT JOIN dbo.tblRegion rg WITH (NOLOCK) ON Ar.RegionId = rg.RegionId
    LEFT JOIN dbo.tbl_Group gr WITH (NOLOCK) ON rg.GroupId = gr.GroupId
    WHERE mas.CustomerCode = @CustomerCode;

    IF @OrderCode IS NULL OR @OrderCode = '' OR @OrderCode LIKE 'ORD-000000%'
    BEGIN
        DECLARE @nextId INT = 1;
        SELECT @nextId = (ISNULL(MAX(OrderId), 0) + 1) FROM dbo.tblOrder WITH (NOLOCK);
        SET @OrderCode = 'ODR-' + CONVERT(NVARCHAR(8), GETDATE(), 112) + CAST(@nextId AS NVARCHAR(20));
    END

    INSERT INTO dbo.tblOrder
    (
        OrderCode,
        TerritoryCode,
        ComUnitId,
        ComUnitCode,
        ComUnitName,
        MIOCode,
        MIOName,
        ManufacId,
        CustomerCode,
        CustomerName,
        GrossValue,
        TotalVat,
        TotalDiscount,
        TotalNetPayable,
        SubmissionDate,
        FixedCustomer,
        Remarks,
        OrderType,
        IsManual,
        IsFromApp,
        IsInvoice,
        IsPrepareforInvoice,
        IsSubDepo,
        ActionStatus,
        CustomerMasterId,
        MarketId,
        TerritoryId,
        AreaId,
        RegionId,
        GroupId,
        SubTerritoryId,
        CustTypeId,
        ProgramTypeId,
        DistributionRouteId,
        GroupName_Ord,
        RegionName_Ord,
        AreaName_Ord,
        TerritoryName_Ord,
        SubTerritoryName_Ord,
        MarketName_Ord,
        SmcTypeId_Ord,
        SMCType_Ord,
        OrderSenderType,
        EntryDate,
        ServerDateTime
    )
    VALUES
    (
        @OrderCode,
        @TerritoryCode,
        @ComUnitId,
        @ComUnitCode,
        @ComUnitName,
        @MIOCode,
        @MIOName,
        @ManufacId,
        @CustomerCode,
        @CustomerName,
        @GrossValue,
        @TotalVat,
        @TotalDiscount,
        @TotalNetPayable,
        @SubmissionDate,
        @FixedCustomer,
        @Remarks,
        'Regular',
        1,
        0,
        0,
        1,
        0,
        '2',
        @CustMasterId,
        @MarketId,
        @TerritoryId,
        @AreaId,
        @RegionId,
        @GroupId,
        @SubTerritoryId,
        @CustTypeId,
        @ProgramTypeId,
        @DistributionRouteId,
        @GroupName_Ord,
        @RegionName_Ord,
        @AreaName_Ord,
        @TerritoryName_Ord,
        @SubTerritoryName_Ord,
        @MarketName_Ord,
        @SmcTypeId_Ord,
        @SMCType_Ord,
        'Web Manual',
        GETDATE(),
        GETDATE()
    );

    SET @OrderId = SCOPE_IDENTITY();
END
GO


-- ==========================================================================
-- Procedure: sp_Update_ManualOrderMaster
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Update_ManualOrderMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Update_ManualOrderMaster;
GO

CREATE PROCEDURE [dbo].[sp_Update_ManualOrderMaster]
    @OrderId INT,
    @TerritoryCode NVARCHAR(50) = NULL,
    @ComUnitId INT = NULL,
    @ComUnitCode NVARCHAR(50) = NULL,
    @ComUnitName NVARCHAR(200) = NULL,
    @MIOCode NVARCHAR(50) = NULL,
    @MIOName NVARCHAR(200) = NULL,
    @MIOId INT = NULL,
    @CustomerCode NVARCHAR(50) = NULL,
    @CustomerName NVARCHAR(200) = NULL,
    @GrossValue DECIMAL(18,4) = 0,
    @TotalVat DECIMAL(18,4) = 0,
    @TotalDiscount DECIMAL(18,4) = 0,
    @TotalNetPayable DECIMAL(18,4) = 0,
    @SubmissionDate DATETIME = NULL,
    @FixedCustomer BIT = 0,
    @Remarks NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CustMasterId INT = NULL, @MarketId INT = NULL, @CustTypeId INT = NULL, @ProgramTypeId INT = NULL, @DistributionRouteId INT = NULL;
    DECLARE @GroupId INT = NULL, @RegionId INT = NULL, @AreaId INT = NULL, @TerritoryId INT = NULL, @SubTerritoryId INT = NULL;
    DECLARE @GroupName_Ord NVARCHAR(200) = NULL, @RegionName_Ord NVARCHAR(200) = NULL, @AreaName_Ord NVARCHAR(200) = NULL;
    DECLARE @TerritoryName_Ord NVARCHAR(200) = NULL, @SubTerritoryName_Ord NVARCHAR(200) = NULL, @MarketName_Ord NVARCHAR(200) = NULL;
    DECLARE @SmcTypeId_Ord INT = NULL, @SMCType_Ord NVARCHAR(200) = NULL;

    SELECT TOP 1 
        @CustMasterId = mas.CustomerMasterId,
        @MarketId = mas.MarketId,
        @CustTypeId = mas.CustomerTypeId,
        @ProgramTypeId = mas.ProgramTypeId,
        @DistributionRouteId = mas.DistributionRouteId,
        @SmcTypeId_Ord = mas.SMCTypeId,
        @SMCType_Ord = st.SMCType,
        @SubTerritoryId = subTr.SubTerritoryId,
        @TerritoryId = Tr.TerritoryId,
        @AreaId = Ar.AreaId,
        @RegionId = rg.RegionId,
        @GroupId = gr.GroupId,
        @MarketName_Ord = mr.MarketName,
        @SubTerritoryName_Ord = subTr.SubTerritoryName,
        @TerritoryName_Ord = Tr.TerritoryName,
        @AreaName_Ord = Ar.AreaName,
        @RegionName_Ord = rg.RegionName,
        @GroupName_Ord = gr.GroupName
    FROM dbo.tblCustMaster mas WITH (NOLOCK)
    LEFT JOIN dbo.tblSMCType st WITH (NOLOCK) ON mas.SMCTypeId = st.SMCTypeId
    LEFT JOIN dbo.tblMarket mr WITH (NOLOCK) ON mas.MarketId = mr.MarketId
    LEFT JOIN dbo.tblSubTerritory subTr WITH (NOLOCK) ON mr.SubTerritoryId = subTr.SubTerritoryId
    LEFT JOIN dbo.tblTerritory Tr WITH (NOLOCK) ON subTr.TerritoryId = Tr.TerritoryId
    LEFT JOIN dbo.tblArea Ar WITH (NOLOCK) ON Tr.AreaId = Ar.AreaId
    LEFT JOIN dbo.tblRegion rg WITH (NOLOCK) ON Ar.RegionId = rg.RegionId
    LEFT JOIN dbo.tbl_Group gr WITH (NOLOCK) ON rg.GroupId = gr.GroupId
    WHERE mas.CustomerCode = @CustomerCode;

    UPDATE dbo.tblOrder
    SET 
        TerritoryCode = @TerritoryCode,
        ComUnitId = @ComUnitId,
        ComUnitCode = @ComUnitCode,
        ComUnitName = @ComUnitName,
        MIOCode = @MIOCode,
        MIOName = @MIOName,
        CustomerCode = @CustomerCode,
        CustomerName = @CustomerName,
        GrossValue = @GrossValue,
        TotalVat = @TotalVat,
        TotalDiscount = @TotalDiscount,
        TotalNetPayable = @TotalNetPayable,
        SubmissionDate = @SubmissionDate,
        FixedCustomer = @FixedCustomer,
        Remarks = @Remarks,
        OrderType = 'Regular',
        ActionStatus = '2',
        IsSubDepo = 0,
        IsPrepareforInvoice = 1,
        CustomerMasterId = ISNULL(@CustMasterId, CustomerMasterId),
        MarketId = ISNULL(@MarketId, MarketId),
        TerritoryId = ISNULL(@TerritoryId, TerritoryId),
        AreaId = ISNULL(@AreaId, AreaId),
        RegionId = ISNULL(@RegionId, RegionId),
        GroupId = ISNULL(@GroupId, GroupId),
        SubTerritoryId = ISNULL(@SubTerritoryId, SubTerritoryId),
        CustTypeId = ISNULL(@CustTypeId, CustTypeId),
        ProgramTypeId = ISNULL(@ProgramTypeId, ProgramTypeId),
        DistributionRouteId = ISNULL(@DistributionRouteId, DistributionRouteId),
        GroupName_Ord = ISNULL(@GroupName_Ord, GroupName_Ord),
        RegionName_Ord = ISNULL(@RegionName_Ord, RegionName_Ord),
        AreaName_Ord = ISNULL(@AreaName_Ord, AreaName_Ord),
        TerritoryName_Ord = ISNULL(@TerritoryName_Ord, TerritoryName_Ord),
        SubTerritoryName_Ord = ISNULL(@SubTerritoryName_Ord, SubTerritoryName_Ord),
        MarketName_Ord = ISNULL(@MarketName_Ord, MarketName_Ord),
        SmcTypeId_Ord = ISNULL(@SmcTypeId_Ord, SmcTypeId_Ord),
        SMCType_Ord = ISNULL(@SMCType_Ord, SMCType_Ord)
    WHERE OrderId = @OrderId;

    DELETE FROM dbo.tblOrderDetail WHERE OrderId = @OrderId;
END
GO


-- ==========================================================================
-- Procedure: sp_LoadOrderListForOrderCreationbyTerri
-- ==========================================================================
IF OBJECT_ID('dbo.sp_LoadOrderListForOrderCreationbyTerri', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_LoadOrderListForOrderCreationbyTerri;
GO

CREATE PROCEDURE [dbo].[sp_LoadOrderListForOrderCreationbyTerri] 
    @manufacId int = NULL,
    @comunitId int,
    @TerritoryId int = NULL
AS
BEGIN
    SELECT 
        tblOrder.PaymentType, 
        ISNULL(InvoiceInfo.DaysSinceUpdate, 0) DueAlert,
        tblOrder.CustomerMasterId, 
        tblOrder.Remarks, 
        tblOrder.ComunitId, 
        tblOrder.ManufacId, 
        tblOrder.OrderId, 
        tblOrder.OrderCode, 
        tblOrder.OrderSenderCode, 
        tblOrder.OrderSenderName, 
        tblOrder.SubmissionDate, 
        ISNULL(tblDue.ReceivableTotalAmnt, 0) DueAmount,
        tblCustomerType.CustomerType,
        tblMarket.MarketName,
        tblOrder.TerritoryName_Ord TerritoryName, 
        tblOrder.CustomerCode, 
        tblOrder.CustomerName,
        tblOrder.GrossValue, 
        tblOrder.CustomerType,
        tblOrder.DeliveryDate,
        -- Credit Limit Validation
        ISNULL(TS.Due, 0) AS Due,
        ISNULL(tblOrder.GrossValue, 0) + ISNULL(TS.Due, 0) AS ReceiveAble,
        ISNULL(LM.LimitAmount, 0) AS LimitAmount,
        ISNULL(LM.DayLimit, 0) AS DayLimit,
        ISNULL(LD.InvoiceNo, 'N/A') AS InvoiceNo,
        ISNULL(LD.InvoiceDate, GETDATE()) AS InvoiceDate,
        ISNULL(DATEDIFF(DAY, LD.InvoiceDate, GETDATE()), 0) AS DS,
        CASE 
            WHEN ISNULL(LM.LimitAmount, 0) = 0 THEN 1
            WHEN (ISNULL(tblOrder.GrossValue, 0) + ISNULL(TS.Due, 0) <= ISNULL(LM.LimitAmount, 0)) 
             AND (ISNULL(DATEDIFF(DAY, LD.InvoiceDate, GETDATE()), 0) <= ISNULL(LM.DayLimit, 0)) THEN 1
            ELSE 0
        END AS Permission
    FROM dbo.tblOrder WITH (NOLOCK) 
    LEFT JOIN dbo.tblCompanyUnit AS UNT WITH (NOLOCK) ON tblOrder.ComUnitId = UNT.ComUnitId
    LEFT JOIN dbo.tblMarket WITH (NOLOCK) ON tblOrder.MarketId = dbo.tblMarket.MarketId
    LEFT JOIN tblCustomerType WITH (NOLOCK) ON tblOrder.CustTypeId = dbo.tblCustomerType.CustomerTypeId 

    -- Due Amount
    LEFT JOIN (
        SELECT I.CustomerMasterId, 
               SUM(ISNULL(ISNULL(TD.TotalDelivery, 0) - ISNULL(P.PP, 0), 0)) ReceivableTotalAmnt 
        FROM dbo.tblInvoice I WITH(NOLOCK) 
        INNER JOIN (
            SELECT InvoiceId, SUM(PaymentNetAmount) AS TotalDelivery 
            FROM tblInvoiceDetail AS IVD WITH(NOLOCK) 
            GROUP BY InvoiceId
        ) AS TD ON I.InvoiceId = TD.InvoiceId 
        LEFT JOIN (
            SELECT InvoiceId, SUM(ISNULL(TPAmount, 0) + ISNULL(VATAmount, 0)) AS PP 
            FROM tblCustPayDetail 
            GROUP BY InvoiceId
        ) AS P ON I.InvoiceId = P.InvoiceId 
        WHERE ISNULL(ISNULL(TD.TotalDelivery, 0) - ISNULL(P.PP, 0), 0) > 10 
          AND ISNULL(TD.TotalDelivery, 0) <> ISNULL(P.PP, 0)
        GROUP BY I.CustomerMasterId
    ) tblDue ON tblDue.CustomerMasterId = tblOrder.CustomerMasterId

    -- Total Due for Permission check
    LEFT JOIN (
        SELECT INV.CustomerMasterId, INV.ComUnitId,
               SUM(ISNULL(DeliveryTpGrandTotal, 0)) - (
                   SUM(ISNULL(PaymentAmount, 0)) + 
                   SUM(ISNULL(AIT, 0)) + 
                   SUM(ISNULL(DiscountOnPayment, 0))
               ) - SUM(ISNULL(RTN.ReturnAmount, 0)) AS Due
        FROM tblInvoice AS INV
        LEFT JOIN (
            SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount 
            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS M
            LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
            WHERE ISNULL(M.IsInventoryValuation, 0) = 0 AND M.TableID = 2018 
            GROUP BY M.CommonPK_ID
        ) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
        GROUP BY INV.CustomerMasterId, INV.ComUnitId
        HAVING SUM(ISNULL(DeliveryTpGrandTotal, 0)) > 0
    ) AS TS ON tblOrder.CustomerMasterId = TS.CustomerMasterId AND UNT.ComUnitId = TS.ComUnitId

    -- Credit Limit
    LEFT JOIN (
        SELECT CompanyId, CustomerMasterId, 
               SUM(ISNULL(LimitAmount, 0)) LimitAmount, 
               SUM(ISNULL(DayLimit, 0)) DayLimit 
        FROM tblCustomerCreditLimit 
        WHERE IsActive = 1 AND LTRIM(RTRIM(UPPER(ActionStatus))) = LTRIM(RTRIM(UPPER('Approved'))) 
        GROUP BY CompanyId, CustomerMasterId
    ) AS LM ON tblOrder.CustomerMasterId = LM.CustomerMasterId AND UNT.CompanyId = LM.CompanyId

    -- Oldest Unpaid Invoice
    LEFT JOIN (
        SELECT INV.CustomerMasterId, CI.CompanyId,
               MIN(InvoiceNo) AS InvoiceNo, 
               MIN(InvoiceDate) AS InvoiceDate 
        FROM tblInvoice AS INV
        LEFT JOIN tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
        LEFT JOIN (
            SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount 
            FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS M
            LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
            LEFT JOIN tblInvoice AS INV2 ON M.CommonPK_ID = INV2.InvoiceId
            WHERE ISNULL(M.IsInventoryValuation, 0) = 0 AND M.TableID = 2018 
            GROUP BY M.CommonPK_ID
        ) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
        WHERE ISNULL(DeliveryTpGrandTotal, 0) > 0 
          AND (ISNULL(DeliveryTpGrandTotal, 0) - (
                ISNULL(PaymentAmount, 0) + ISNULL(AIT, 0) + ISNULL(DiscountOnPayment, 0)
              )) - ISNULL(RTN.ReturnAmount, 0) > 1
        GROUP BY INV.CustomerMasterId, CI.CompanyId
    ) AS LD ON tblOrder.CustomerMasterId = LD.CustomerMasterId AND UNT.CompanyId = LD.CompanyId

    OUTER APPLY (
        SELECT TOP 1 
            DATEDIFF(DAY, tblInvoice.UpdateDate, GETDATE()) AS DaysSinceUpdate
        FROM tblInvoice WITH (NOLOCK)
        WHERE tblInvoice.CustomerMasterId = tblOrder.CustomerMasterId
          AND tblInvoice.DelivaryInvoiceNo IS NOT NULL
          AND (tblInvoice.PaymentStatus IS NULL OR tblInvoice.PaymentStatus = 'Partial')
          AND DATEDIFF(DAY, tblInvoice.UpdateDate, GETDATE()) >= 30
          AND tblInvoice.DeliveryTpGrandTotal > 0
          AND tblInvoice.PaymentInvoiceStatus <> 'Reject'
        ORDER BY DATEDIFF(DAY, tblInvoice.UpdateDate, GETDATE()) DESC
    ) InvoiceInfo

    WHERE tblOrder.IsInvoice = 0 
      AND (tblOrder.OrderType = 'Regular' OR tblOrder.OrderType IS NULL OR tblOrder.IsManual = 1)
      AND tblOrder.ComUnitId = @comunitId
      AND (ISNULL(@manufacId, 0) = 0 OR tblOrder.DistributionRouteId = @manufacId OR tblOrder.DistributionRouteId IS NULL)
      AND (ISNULL(@TerritoryId, 0) = 0 OR tblOrder.TerritoryId = @TerritoryId OR tblOrder.TerritoryId IS NULL)
      AND (tblOrder.IsPrepareforInvoice = 1 OR tblOrder.IsPrepareforInvoice IS NULL)
      AND (tblOrder.ActionStatus = '2' OR tblOrder.ActionStatus = 'Accepted' OR tblOrder.ActionStatus = 'Approved')
      AND (tblOrder.IsSubDepo = 0 OR tblOrder.IsSubDepo IS NULL)
    ORDER BY tblOrder.SubmissionDate DESC
END
GO


-- ==========================================================================
-- Procedure: sp_Save_ManualOrderDetail
-- ==========================================================================
IF OBJECT_ID('dbo.sp_Save_ManualOrderDetail', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Save_ManualOrderDetail;
GO

CREATE PROCEDURE [dbo].[sp_Save_ManualOrderDetail]
    @ProductId INT,
    @ProductCode NVARCHAR(50),
    @ProductName NVARCHAR(200),
    @Quantity DECIMAL(18,4),
    @TradePrice DECIMAL(18,4),
    @TotalTradePrice DECIMAL(18,4),
    @ISGiftProduct BIT = 0,
    @IsCampaignProduct BIT = 0,
    @OrderId INT,
    @UnitVatAmount DECIMAL(18,4) = 0,
    @TotalVatAmount DECIMAL(18,4) = 0,
    @NetAmount DECIMAL(18,4) = 0
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.tblOrderDetail
    (
        ProductId,
        ProductCode,
        ProductName,
        Quantity,
        TradePrice,
        TotalTradePrice,
        ISGiftProduct,
        IsCampaignProduct,
        IsSpDis,
        OrderId,
        DiscountPercent,
        DiscountAmount,
        UnitVatAmount,
        TotalVatAmount,
        NetAmount
    )
    VALUES
    (
        @ProductId,
        @ProductCode,
        @ProductName,
        @Quantity,
        @TradePrice,
        @TotalTradePrice,
        @ISGiftProduct,
        @IsCampaignProduct,
        0,
        @OrderId,
        0,
        0,
        @UnitVatAmount,
        @TotalVatAmount,
        @NetAmount
    );
END
GO
