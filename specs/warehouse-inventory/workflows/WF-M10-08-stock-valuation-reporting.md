# WF-M10-08 Stock valuation reporting (DC ledger and national valuation)

**Module:** M10 | **Confidence:** CONFIRMED - based on the SP bodies in `SP_Update_StockValuation_TransactionDate_Logic.sql` (the repo copy of the SPs; the live database version is UNKNOWN but `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:51` also creates `sp_Report_DCStore_StockValuationLedger`).

## Trigger and actors
Finance/inventory user runs `DCStockValuationReport.aspx` (DC ledger) or `NationalStockValuationReport.aspx` (central + all DCs) for a date range.

## Steps
| # | Page | Handler -> BLL -> DAL -> SQL |
|---|---|---|
| 1 | `DCStockValuationReport.aspx` | `Page_Load` requires `Session["UserId"]` else redirect `~/Login.aspx`; defaults From/To = today; `LoadFilters` (DC, category, product) |
| 2 | same | `searchButton_Click` -> `LoadData` -> `DCStockReportBLL.GetDCStockValuationLedger(from,to,comUnitId?,categoryId?,productId?)` -> `DCStockReportDAL.GetDCStockValuationLedger` (`GetDataTableAction`) -> SP `sp_Report_DCStore_StockValuationLedger` (dates passed as `dd MMMM, yyyy` strings) |
| 3 | same | grid bound; footer totals accumulate Opening/Receive/B2B/Return/Sales/B2B Issue/Closing **quantities** only (the amount accumulators are declared but never added); `exportButton_Click` -> HTML table as `.xls` |
| 4 | `NationalStockValuationReport.aspx` | `LoadData` -> `CentralStoreBLL.GetNationalStockValuation` -> `CentralStoreDAL` -> SP `sp_Report_NationalStockValuation_DateRange`; SL No column added in code; batch popup links call `sp_Report_NationalStockValuation_BatchDetails(@ProductCode,@LocationName,@FromDate,@ToDate)` |

## Valuation logic (as coded)
See README "Valuation method" and BR-M10-060..068. In short: DC ledger = per (DC, product) opening from cost-layer sums, movements classified by transaction date, closing at **latest unit cost** x closing qty; national valuation = central store **moving weighted average** ledger + DC cost-layer sums; batch drill-down aggregates batch quantities.

## Data read
`tblDCStore`, `tblCompanyUnit`, `tblProduct`, `tblUnitPrice`, `tblProCategory`, `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock`.

## Branches / exceptions
- Empty dates -> `Please select a valid date range.`; exceptions shown as `Error retrieving report: <message>`.
- Category/product/DC optional (NULL = all).
- `STRING_AGG` requires SQL Server 2017+.
- Dynamic SQL embeds dates via `CONVERT(varchar(23), @d, 121)`; category/product are integers.

## Postconditions
Read-only; no data changed.

## Related
BR-M10-060..068; PAGE-365, 374; RPT-M10-01; GAP-M10-007.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF08-T1 | Single DC, today | opening, movement, closing consistent: Closing = Opening + Rcv + B2B + Return - Sales - B2B Issue |
| WF08-T2 | Issue row created today for a receipt dated last month | counted in this month's Sales via `ReferenceTransactionDate` |
| WF08-T3 | To-date 30-Jun | includes rows to 30-Jun 23:59:59 |
| WF08-T4 | Product with closing rate history | closing rate = latest CostPerUnit>0 |
| WF08-T5 | National report, batch popup 'Central Store Qty' | batches with positive sum only |
