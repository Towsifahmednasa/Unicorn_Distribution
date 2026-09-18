USE [SalesDisDB_Unicorn_DB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- 1. Sync Invoices with Discrepancy between tblInvoice and tblInvoiceDetail
-- ============================================================================
PRINT 'Syncing invoice header totals with actual saved details...';

UPDATE inv
SET 
    inv.TpTotal = d.TotPrice,
    inv.TpDiscount = d.TotDiscount,
    inv.TpVat = d.TotVat,
    inv.TpGrandTotal = d.TotNet,
    inv.ReceivableAmount = (d.TotNet - ISNULL(inv.AdjustAmount, 0))
FROM tblInvoice inv
INNER JOIN (
    SELECT 
        InvoiceId,
        ISNULL(SUM(TotalPrice), 0) AS TotPrice,
        ISNULL(SUM(DiscountAmount), 0) AS TotDiscount,
        ISNULL(SUM(TotalPriceVatAmount), 0) AS TotVat,
        ISNULL(SUM(NetAmount), 0) AS TotNet
    FROM tblInvoiceDetail
    GROUP BY InvoiceId
) d ON inv.InvoiceId = d.InvoiceId
WHERE inv.InvoiceDate > '2022-11-16'
  AND ABS(inv.TpGrandTotal - d.TotNet) > 10;

PRINT 'Invoice headers synced successfully!';
GO

-- ============================================================================
-- 2. Update sp_Check_anomalyInvoiceDetails to handle AdjustAmount & ABS tolerance
-- ============================================================================
PRINT 'Updating sp_Check_anomalyInvoiceDetails...';
GO

ALTER PROCEDURE [dbo].[sp_Check_anomalyInvoiceDetails]
	@InvoiceId  INT,
	@orderId    INT
AS
BEGIN 
    SET NOCOUNT ON;

    SELECT  
        TpGrandTotal,
        NetAmount,
        InvoiceNo,
        invoicedate 
    FROM (
        SELECT 
            tblInvoice.OrderId, 
            tblInvoice.InvoiceId, 
            tblInvoice.TpGrandTotal,
            ISNULL(tblInvoice.AdjustAmount, 0) AS AdjustAmount,
            SUM(tblInvoiceDetail.NetAmount) AS NetAmount,
            tblInvoice.InvoiceNo,
            tblInvoice.InvoiceDate AS invoicedate
        FROM tblInvoice WITH(NOLOCK)
        INNER JOIN tblInvoiceDetail WITH(NOLOCK) ON tblInvoice.InvoiceId = tblInvoiceDetail.InvoiceId 
        WHERE tblInvoice.InvoiceId = @InvoiceId 
          AND tblInvoice.OrderId = @orderId
          AND tblInvoice.InvoiceDate > '2022-11-16'
        GROUP BY 
            tblInvoice.OrderId,
            tblInvoice.InvoiceId,
            tblInvoice.TpGrandTotal,
            tblInvoice.AdjustAmount,
            tblInvoice.InvoiceNo,
            tblInvoice.InvoiceDate
    ) tblx
    WHERE ABS((TpGrandTotal - AdjustAmount) - NetAmount) > 10;
END
GO

PRINT 'sp_Check_anomalyInvoiceDetails updated successfully!';
GO
