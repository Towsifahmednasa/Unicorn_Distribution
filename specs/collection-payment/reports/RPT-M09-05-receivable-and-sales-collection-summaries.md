# RPT-M09-05 — Receivable / Sales-Collection summary reports (GridView + Excel)

| Page | Data call | SP (body in repo?) | Notes |
|---|---|---|---|
| `RptMIOWiseReceiveableReport.aspx` (PAGE-336, Tier A) | `TotalSummaryDAL.RptMIOWiseReceiveableReport(from,to,Type,ZonId,Area,Terr)` | `sp_RPT_MIS_RptMIOWiseReceiveableReport` — NO | levels SC / Zone / Area / Territory (Transactional vs Non-transactional) |
| `Sales_Collection_Reports_Acc.aspx` (PAGE-337) | `TotalSummaryDAL.Sales_Collection_Reports_AccDAL(from,to,Type,ZonId,Area,Terr)` (:512+) | `sp_RPT_MIS_BusinessSummary_Acc` — NO | same UI pattern/parameters as PAGE-336; Excel export |
| `SalesCollectionReport.aspx` (PAGE-338) | `CmnCrystaltoView.GetSalesCollectionReportDAL(parm)` (:1910) | `sp_Get_SalesCollectionReport(@Parm)` — NO | filters DA, customer, sales person (order sender), region, product lists (`GetDAListDAL`, `GetCustomerListDAL`, `GetOrderSenderListDAL`, `GetRegionListDAL`, `GetProductListDAL`); `parm` = concatenated SQL fragment |
| `NewReceiveableReport.aspx` (PAGE-332) / `NewReceiveableReportApps.aspx` (PAGE-333) | `InvoiceDAL.GetNewReceiveableDAlWeb / GetNewReceiveableDAl(districtId, fromDate, toDate)` (:92-135) | `sp_Get_NewReceiveableListWeb` / `sp_Get_NewReceiveableList` — NO | depot (`OtherStockActionBLL.LoadSC`), date range; Excel via session `Excel`; "Apps" variant serves the mobile app |

## PAGE-336 columns (verified from markup)
NumberofInvoice, InvoieAmountTP/InvoiceGrossAmt/InvoiceVatTp, RejectAmtTP/RejectGrossAmt/RejectionTpVat, SalesAmtTP/SalesGrossAmt/SalesVat, ReturnAmountTP/ReturnGrossAmt/ReturnAmountVat, JustSalesAmtTP/JustSalesGrossAmt/JustSalesVat, CollectionAmtTP/CollectionGrossAmt/CollectionVat, ReceivableTP/ReceivableGrossAmount; key columns ComUnitShortName / RegionName / AreaName / TerritoryName. Footer totals `Math.Round` whole taka. Formulas for Sales/JustSales/Receivable: UNKNOWN (SP not in repo).

## Common characteristics
Date range mandatory ("Please Select Date Range!!"), infinite SQL command timeout (`ClsCommonInternalDAL.GetDataTableAction` 99,000,000 s), no From ≤ To check, Excel = HTML table saved as `.xls`, `VerifyRenderingInServerForm` override, copy-pasted `excelButton1_Click` opening `BusinessSummaryViewer.aspx`. Market-structure filtering uses the `IVMarketSTForZone.ascx` user control; role-based scoping (DZSM/AM) is UI-level in PAGE-336.
