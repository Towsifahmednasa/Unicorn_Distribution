-- sp_Report_CustomerInvoiceAgingOutstanding
-- Used by: Solution.Web/SInventory_UI/CustomerInvoiceAgingReport.aspx (via CustomerInvoiceAgingDAL)
-- Extracted from live DB (SalesDisDB_Unicorn_DB) on 2026-09-26
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
    CTE_ReturnSummary AS (
        SELECT crm.InvoiceId, SUM(ISNULL(crd.ReturnAmount, 0.00)) AS MainReturnTotal
        FROM dbo.tblCustomerReturnMaster crm WITH (NOLOCK)
        INNER JOIN dbo.tblCustomerReturnDetail crd WITH (NOLOCK)
            ON crm.CustReturnId = crd.CustReturnId
        GROUP BY crm.InvoiceId
    ),
    CTE_NonTradeReturnSummary AS (
        SELECT ri.InvoiceId, SUM(ISNULL(rid.NetAmount, 0.00)) AS NonTraMainRTNTotalAmt
        FROM dbo.tblReturnInvoice ri WITH (NOLOCK)
        INNER JOIN dbo.tblReturnInvoiceDetail rid WITH (NOLOCK)
            ON ri.ReturnInvoiceId = rid.ReturnInvoiceId
        GROUP BY ri.InvoiceId
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
            ((ISNULL(invS.DetailNetAmount, 0.00) + ISNULL(im.DeliveryCharge, 0.00)) - ISNULL(payS.PaidAmount, 0.00) - ISNULL(rtnS.MainReturnTotal, 0.00) - ISNULL(ntRtnS.NonTraMainRTNTotalAmt, 0.00)) AS [Outstanding],
            ISNULL(emp.EmpName, 'N/A') AS [Responsible Person]
        FROM dbo.tblInvoice im WITH (NOLOCK)
        INNER JOIN dbo.tblCustMaster cm WITH (NOLOCK)
            ON im.CustomerMasterId = cm.CustomerMasterId
        LEFT JOIN CTE_InvoiceSummary invS
            ON im.InvoiceId = invS.InvoiceId
        LEFT JOIN CTE_PaymentSummary payS
            ON im.InvoiceId = payS.InvoiceId
        LEFT JOIN CTE_ReturnSummary rtnS
            ON im.InvoiceId = rtnS.InvoiceId
        LEFT JOIN CTE_NonTradeReturnSummary ntRtnS
            ON im.InvoiceId = ntRtnS.InvoiceId
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
