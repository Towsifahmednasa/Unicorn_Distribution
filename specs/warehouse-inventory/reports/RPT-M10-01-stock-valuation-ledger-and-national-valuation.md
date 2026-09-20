# RPT-M10-01 DC stock valuation ledger and national stock valuation

**Pages:** `Solution.Web/SInventory_UI/DCStockValuationReport.aspx(.cs)` (PAGE-365), `Solution.Web/SInventory_UI/NationalStockValuationReport.aspx(.cs)` (PAGE-374).
**Kind:** on-screen GridView + Excel export (HTML table saved as `.xls`); **no `.rpt`/Crystal file** and no `SInventory_RPTVIEW` viewer.
**Confidence:** CONFIRMED (SP bodies read in `SP_Update_StockValuation_TransactionDate_Logic.sql`).

## Call chain
| Report | Page handler | BLL | DAL | SP |
|---|---|---|---|---|
| DC ledger | `searchButton_Click` -> `LoadData` (`DCStockValuationReport.aspx.cs:70-110`) | `DCStockReportBLL.GetDCStockValuationLedger` | `DCStockReportDAL.GetDCStockValuationLedger` (`Library.DAL/SInventory_DAL/DCStockReportDAL.cs:562-572`) via `ClsCommonInternalDAL.GetDataTableAction(..., "SSIDB")` | `sp_Report_DCStore_StockValuationLedger` (`SP_Update_StockValuation_TransactionDate_Logic.sql:19-179`; also `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:51`) |
| National | `NationalStockValuationReport.LoadData` | `CentralStoreBLL.GetNationalStockValuation` | `CentralStoreDAL.GetNationalStockValuation` (`CentralStoreDAL.cs:191-200`) | `sp_Report_NationalStockValuation_DateRange` (`...sql:195-338`) |
| National batch drill-down | batch link on a quantity cell | `CentralStoreBLL` | `CentralStoreDAL.GetNationalStockValuationBatchDetails` (`:202-211`) | `sp_Report_NationalStockValuation_BatchDetails` (`...sql:354-482`) |
| (referenced, body absent) central ledger | - | `CentralStoreBLL` | `CentralStoreDAL.GetStockValuationLedger` (`:180-189`) | `sp_Report_CentralStore_StockValuationLedger` (UNKNOWN) |

## Parameters and filters
- Page: `fromDateTextBox`, `toDateTextBox` (default today, format `dd MMMM, yyyy`; passed to the SP as **strings** - culture-dependent conversion), DC (`dcDropDownList`, optional), category (`categoryDropDownList`, optional), product (`productDropDownList`, optional). National report has no DC filter.
- Session: `UserId` required, else redirect `~/Login.aspx` (both pages, `!IsPostBack`).
- SP: `@FromDate`, `@ToDate` (extended to 23:59:59), `@ComUnitId`, `@CategoryId`, `@ProductId` (NULL = all).

## Output columns
**DC ledger:** DC Code, Distribution Center, Product Code/Name, Pack Size, Category, Opening Qty/Rate/Total Value, Receive From WH, B2B Receive, Return Rcv, Sales, B2B Issue, Closing Qty, Closing Rate, Closing Total Value. Rows with zero opening and zero movement are hidden (HAVING). Page footer totals cover the seven quantity columns only (`totalOpeningAmt`, `totalClosingAmt` declared but never accumulated).
**National:** Product Code/Name, Pack Size, Category, Avg Rate (central), Opening National Qty, Central Store Qty, one dynamic column per `tblCompanyUnit.ComUnitName` (depot closing qty), Total Depots Qty, National Stock Qty, Central Store Value, Depots Total Value, National Total Value. Only products with any positive opening/closing quantity.

## Business logic (see README "Valuation method")
BR-M10-060 .. BR-M10-068 describe transaction-date selection, opening/movement/closing formulas, valuation basis (DC: cost layers for opening and **latest cost** for closing; central: moving weighted average), and batch drill-down.

## Tables/views
`tblDCStore`, `tblCompanyUnit`, `tblProduct`, `tblUnitPrice`, `tblProCategory`, `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock`.

## Output/export
Excel: `exportButton_Click` renders the GridView (HTML) into `DC_Stock_Valuation_Report_<dd_MMM_yyyy_hh_mm_tt>.xls` / `National_Stock_Valuation_Report_...xls`; national export strips hyperlinks (regex) first. No PDF/print.

## Known issues
GAP-M10-007 (valuation basis mix; amount totals missing; in-place `StockQty` changes vs `TotalCostAmount`), GAP-M10-024 (string date parameters), `STRING_AGG` needs SQL Server 2017+, `sp_Report_CentralStore_StockValuationLedger` body absent.
