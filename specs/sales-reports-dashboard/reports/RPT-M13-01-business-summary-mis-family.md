# RPT-M13-01 Business Summary / MIS on-screen report family

_Module M13. Documentation only, generated from source 2026-09-19. Confidence: CONFIRMED unless tagged. SP bodies are not in the repository._

## 1. Scope
On-screen (GridView) MIS reports - **no Crystal file**; export is `btnExportToExcel_Click` (grid rendered as .xls through `HtmlTextWriter`, header/sub-title rows added) or the legacy viewer path (RPT-M13-02). Pages: TotalSummary (531), BusinessSummary (494), BranchTotalSummary (493), DZSMTotalSummary (506), TerritoryWiseSalesReport (530), TotalSummaryNew (532), TotalSummaryNew2 (533), TotalSummaryNew_PT (534), TotalSummaryNew_T (535), Copy of TotalSummaryNew (498), TotalSummaryProductWise (536), MIOWiseTotalSummaryNew (518), MIOWiseTotalSummaryNew_T (519), MIOWiseBusinessSummary (517), CustomerWiseSale (499), MonitoringReport (520), ProductWiseSalesReport (523), FinalsalesReport (509), FinalsalesReport_T (510), FinalSales (508).

## 2. Lineage
| List page (ID) | Output | BLL -> DAL | SP / SQL | Params | Filters on page | Tables (known) |
|---|---|---|---|---|---|---|
| TotalSummary 531 / BusinessSummary 494 | grid `loadGridView`; Excel via BusinessSummaryViewer (531) | TotalSummaryBLL.LoadSummaryBLL -> TotalSummaryDAL.LoadSummaryDAL | **inline SQL** (KPIs below) | @FromDate,@ToDate | from/to date | tblCompanyUnit, tblInvoice, tblInvoiceDetail, tblSubInvoiceMaster, tblSubInvoiceDetail, tblOrder, tblCustMaster |
| BranchTotalSummary 493 | grid + 4 Excel buttons | BranchwiseLoadSummaryBLL | inline SQL, `WHERE C.ComUnitId=@Branch` | @FromDate,@ToDate,@Branch | dates, sales-centre | same |
| TotalSummaryNew 532 | grid `loadGridView` (product rows) | LoadSummaryProductcodewise -> LoadSummaryProductcodewiseNew | sp_ProductWiseBusinessSummaryMISReportByParam | @fromdate,@todate,@Type,@Area,@Terr,@ZonId | dates, level radio (SC/Zone/Area/Territory), market-structure selectors | UNKNOWN |
| TotalSummaryNew2 533 | grids gv_DistributionCenter/gv_Zone/gv_Area/gv_Territory | LoadSummaryProductcodewiseGyash (6 args) | sp_BusinessSummaryMISReport_All_New_New | @fromdate,@todate,@Type,@Area,@Terr,@ZoneId | + Transactional/Non-transactional radio | UNKNOWN |
| TotalSummaryNew_T 535 | same 4 grids | LoadSummaryProductcodewiseGyash__New | sp_BusinessSummaryMISReport_TT | same | same | UNKNOWN |
| MIOWiseBusinessSummary 517 | same grids | LoadMIOWiseBusinessSummaryDAL | sp_RPT_MIOWiseBusinessSummary | @fromdate,@todate,@Depid | sales centre | UNKNOWN |
| MIOWiseTotalSummaryNew 518 / _T 519 | loadGridView | MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash -> MIOTotalSummaryDAL | sp_MiowiseMISReport | @fromDate,@toDate,@unitid | sales centre | UNKNOWN |
| TotalSummaryNew_PT 534 | loadGridView | LoadBusinessSummaryProductwise | sp_Rpt_BusinessSummaryProductwise | @fromdate,@todate | dates | UNKNOWN |
| Copy of TotalSummaryNew 498 / TotalSummaryProductWise 536 | grid | LoadSummary2BLL -> LoadSummaryProductcodewise | sp_ProductWiseBusinessSummaryMISReport | @fromdate,@todate | dates | UNKNOWN |
| DZSMTotalSummary 506 | loadGridView (27 cols) | DZSMwiseLoadSummaryparm | sp_GET_DZSMwiseReportParam_ByProcess | @fromdate,@todate,@parm (=ZoneSelect value) | dates, group/zone/area | UNKNOWN |
| TerritoryWiseSalesReport 530 | grid | DZSMwiseLoadSummaryBLL / DZSMwiseLoadSummaryparm_vvvv | sp_GET_DZSMwiseReport, sp_GET_DZSMwiseNAtionalReport, sp_GET_DZSMwiseReportParam_ByProcess_vv / _Area / _Terri, sp_GET_DZSMProcessDate | dates + zone/area/territory | | UNKNOWN |
| MonitoringReport 520 | 4 grids | rptMonitoringReport | sp_RPTMonitoringReport | @fromdate,@todate,@Type | level, Tran/NONTran | UNKNOWN |
| ProductWiseSalesReport 523 | 4 grids | ProductWiseSalesReportDAL | sp_RPT_MIS_ProductWiseSalesReport | @fromdate,@todate,@Type,@Area,@Terr,@ZonId | level, Tran/NONTran | UNKNOWN |
| CustomerWiseSale 499 | pivot grid | TotalSummaryBLL.CustomerWiseSale (C# loop) | DB function GetmonthlycustomerSaleDate(@FromDate,@ToDate) | month range | dates | UNKNOWN (function) |
| FinalsalesReport 509 / _T 510 / FinalSales 508 | grid + count boxes | FinalSales, FinalSales2 | sp_GetFinalSales, sp_NumberofInvoiceandCust | @fromdate,@todate | dates | UNKNOWN |

`@Type` values: "SC","Zone","AreaTran","AreaNONTran","TerritoryTran","TerritoryNONTran" (+"AreaTranForDSM"/"AreaNONTranForDSM" in New2) - BR-M13-028, BR-M13-029.

## 3. KPIs / calculations exactly as coded (inline SQL of `TotalSummaryDAL.LoadSummaryDAL`, `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:30-88`)
| Column | Formula |
|---|---|
| NumberofProformaInvoice | COUNT(DISTINCT InvoiceId), TpGrandTotal>0, InvoiceDate in range (tblInvoice + tblSubInvoiceMaster added) |
| SumofNetProformaAmount | SUM(NetAmount) - SUM(TotalPriceVatAmount) |
| ProTpVat | SUM(TotalPriceVatAmount) |
| NumberofInvoiceSold / SumofNetSalesAmount / DelTpVat | delivered invoices (Partial/Full, UpdateDate in range): COUNT DISTINCT; SUM(DeliveryNetAmount-DeliveryTotalPriceVatAmount); SUM(DeliveryTotalPriceVatAmount) |
| NumberofReturnInvoice / SumofNetReturnAmount / DelReTpVat | lines Reject/Partial: COUNT DISTINCT DelivaryInvoiceNo; (SUM(NetAmount)-SUM(VAT)) - SUM(DeliveryNetAmount-DeliveryVAT); SUM(VAT)-SUM(DeliveryVAT) |
| NetInvoiceAmt | SumofNetProformaAmount + ProTpVat |
| NetReturnAmt | SumofNetReturnAmount + DelReTpVat |
| NetSalesAmt | SumofNetSalesAmount + DelTpVat (labelled "Gross Collection" in MonitoringReport) |
| salesTP | proforma net - return net |
| SalesVat | ProTpVat - DelReTpVat |
| SalesTotal | salesTP + SalesVat |
| Outstanding1 / 2 / 3 | (proforma net - return net - sold net) / (ProVat - ReturnVat - DelTpVat) / sum of both |
| NumberofUndelInvoice, SumofNetUnAmount, UnTpVat | DeliveryInvoiceStatus IS NULL, InvoiceDate in range |
| NumberofOrder / NumberOfOrderValue | COUNT(DISTINCT OrderCode) / SUM(GrossValue) by SubmissionDate |
| CustomerCoverPer | customers invoiced * 100 / all customers of the ComUnit, integer division |
No GP, COGS, growth-% or achievement-% is computed in this family. SP-fed pages show the same column names with SP-internal arithmetic (UNKNOWN). Business rules: BR-M13-021, BR-M13-022, BR-M13-023, BR-M13-024, BR-M13-025, BR-M13-026, BR-M13-027.

## 4. Filters, security, output
Role narrowing and scope mapping: BR-M13-028..030. Footer totals computed in C# (BR-M13-030; rounding inconsistencies). Excel: `btnExportToExcel_Click` -> `application/ms-excel` HTML table (file names e.g. "Product wise BS_<level>_<timestamp>.xls" in TotalSummaryNew). Session: Session["UserId"] used by `OrderInfoBLL.LoadSC/LoadTerritory/LoadZone` (legacy dropdowns); role from RoleTypeName/EmpInfoId/RoleTypeId.

## 5. Variant comparison
See README section 8 (which variant is newest, menu-referenced, uses which SP). Summary: TotalSummaryNew (2025-10-05, sp_..MISReportByParam) is newest; TotalSummaryNew2/_T/MIOWiseBusinessSummary are three variants of one page (SP swapped, receivable columns changed); MIOWiseTotalSummaryNew_T and FinalsalesReport_T are byte-identical copies.

