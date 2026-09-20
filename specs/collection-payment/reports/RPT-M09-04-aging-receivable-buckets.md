# RPT-M09-04 — Aging Report (day-bucketed receivable, in-transit style)

| Item | Value |
|---|---|
| Page | `Solution.Web/SInventory_UI/Aging.aspx(.cs)` (PAGE-305, Tier B; read fully for this document) |
| Data | `TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew(districtId, from, to)` → `TotalSummaryDAL.cs:255-265` → **`sp_Get_AgingReceivableReport(@fromdate,@todate,@districtId)`** — body NOT in repo |
| Filters | Depot ("DC" dropdown via `OtherStockActionBLL.DCLoad`; on exception retries with district id 0 = all), From date, To date (both parsed with `Convert.ToDateTime`; no empty check) |
| Output | Grid: Sales Center (ComUnitName), Customer Code/Name, Order No/Date, Invoice No/Date, FE Code (DistrictCode), Territory Code (AreaCode), MIO Code/Name, Market Code/Name, **IntransitDay**, Type (SpecialAmount), then bucket pairs, `TotalTP`, `TotalNet` |
| Export | `btnExportToExcel_Click` → `BusinessSummary.xls` (HTML) with header "Aging Report"; `excelButton1_Click` opens the unrelated `BusinessSummaryViewer.aspx` |

## Bucket boundaries (labels in code)
Header row built in `loadGridView_OnRowCreated` (`Aging.aspx.cs:54-94`); each bucket spans two columns (TP Amount, Net Amount; `DiscountAmount[_n]` / `NetAmount[_n]` fields):
| # | Label | Fields |
|---|---|---|
| 1 | 1 to 10 | DiscountAmount, NetAmount |
| 2 | 11 to 20 | DiscountAmount_2, NetAmount_2 |
| 3 | 21 to 30 | DiscountAmount_3, NetAmount_3 |
| 4 | 31 to 40 | DiscountAmount_4, NetAmount_4 |
| 5 | 41 to 50 | DiscountAmount_5, NetAmount_5 |
| 6 | 51+ | DiscountAmount_6, NetAmount_6 |
Totals: `TotalTP`, `TotalNet`. **The age measured (days since invoice / since dispatch — `IntransitDay` suggests in-transit days) and inclusive/exclusive edges are decided inside the SP: UNKNOWN.** Only the labels "1 to 10 … 51+" are verifiable. The column named `DiscountAmount` is labelled "TP Amount" in the header (mis-named field).

## Notes
Hard-coded header colspan 15 must equal the number of leading columns (fragile). Sibling in-transit report (`AgingInTransitReport`) is another module's page and not analysed here.
