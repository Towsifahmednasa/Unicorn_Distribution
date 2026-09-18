USE [SalesDisDB_Unicorn_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- Object: StoredProcedure [dbo].[sp_Get_MoneyReceiptReportAfterPaymentList]
-- Description: Generates Money Receipt report and grid list after customer payment
-- ============================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Get_MoneyReceiptReportAfterPaymentList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Get_MoneyReceiptReportAfterPaymentList]
GO

CREATE PROCEDURE [dbo].[sp_Get_MoneyReceiptReportAfterPaymentList]
	@Parm NVARCHAR(MAX) = ''
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Q NVARCHAR(MAX) = '
        SELECT DISTINCT
            custPay.CustPayDetailId,
            custPay.CustPayId,
            INV.InvoiceId,
            ISNULL(INV.OrderNo, '''') AS OrderNo,
            ISNULL(CSTMR.CustomerCode, '''') AS CustomerCode,
            ISNULL(CSTMR.CustomerName, '''') AS CustomerName,
            INV.InvoiceNo,
            CONVERT(VARCHAR, INV.InvoiceDate, 103) AS InvoiceDate,
            ISNULL(INV.DelivaryInvoiceNo, '''') AS DelivaryInvoiceNo,
            CONVERT(VARCHAR, ISNULL(INV.UpdateDate, INV.InvoiceDate), 103) AS DelivaryInvoiceDate,
            CASE WHEN ISNULL(masPay.RefNo, '''') <> '''' THEN masPay.RefNo ELSE CONVERT(VARCHAR, custPay.CustPayId) END AS PaymentNo,
            CONVERT(VARCHAR, custPay.custPaymentDate, 103) AS PaymentDate,
            custPay.PaymentAmount,
            ISNULL(custPay.TPAmount, 0) AS TPAmount,
            ISNULL(custPay.VATAmount, 0) AS VatAmount,
            CASE WHEN ISNULL(masPay.RefNo, '''') <> '''' THEN masPay.RefNo ELSE ''MR-'' + RIGHT(''00000000'' + CONVERT(VARCHAR, custPay.CustPayId), 8) END AS ReceiptNo,
            ISNULL(com.ComUnitCode + '' : '' + com.ComUnitName, '''') AS ComUnitName,
            ISNULL(com.Address, '''') AS Address,
            ISNULL(NULLIF(ord.OrderSenderCode + '' : '' + ord.OrderSenderName, '' : ''), ISNULL(INV.OrderSenderCode + '' : '' + INV.OrderSenderName, '''')) AS OrderBy
        FROM tblCustPayDetail AS custPay WITH(NOLOCK)
        INNER JOIN tblCustomerPay AS masPay WITH(NOLOCK) ON custPay.CustPayId = masPay.CustPayId
        INNER JOIN tblInvoice AS INV WITH(NOLOCK) ON custPay.InvoiceId = INV.InvoiceId
        LEFT JOIN tblCustMaster AS CSTMR WITH(NOLOCK) ON INV.CustomerMasterId = CSTMR.CustomerMasterId
        LEFT JOIN tblOrder AS ord WITH(NOLOCK) ON INV.OrderId = ord.OrderId
        LEFT JOIN tblCompanyUnit AS com WITH(NOLOCK) ON INV.ComUnitId = com.ComUnitId
        WHERE 1=1 ' + ISNULL(@Parm, '') + '
        ORDER BY custPay.CustPayDetailId DESC';

    EXEC sp_executesql @Q;
END
GO
