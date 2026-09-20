---
page_id: PAGE-336
module: M09
page: RptMIOWiseReceiveableReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "UNKNOWN", "rules": "PARTIAL", "permission": "COMPLETE", "status": "UNKNOWN", "tests": "PARTIAL"}
---
# PAGE-336 — RptMIOWiseReceiveableReport (Receivable / Sales-Collection summary by DC, Zone, Area, Territory)

> **Evidence level:** Tier A (report page) — code-behind read for role handling, parameter building, `LoadInfo`, footer totals and export skeleton (lines 1-95, 565-812, 1174-1260; the four grids' repetitive footer code was sampled); `TotalSummaryDAL.RptMIOWiseReceiveableReport` read. The feeding SP `sp_RPT_MIS_RptMIOWiseReceiveableReport` is **not in the repo**, so all figures' formulas (receivable, sales, rejection, return, collection) are UNKNOWN; only column names and page aggregation are verified. The user control `IVMarketSTForZone.ascx` supplies Zone/Area/Territory/Market pickers (not read in detail).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-336 |
| Kind | Report / Dashboard — GridView based (no Crystal .rpt; `Reports/CrystalReports/MIOWIseReceivable.rpt` exists but is loaded only by `SInventory_RPTVIEW/MIOwiseInTransitReportViewer.aspx.cs:52`, not by this page) |
| Page class | SInventory_UI_RptMIOWiseReceiveableReport; LOC 1492 |
| Master | NewMasterPage.master |

## 2. Purpose
Shows, for a date range, how much was invoiced, rejected, returned, sold and collected, and what remains receivable, aggregated at Distribution Center (depot), Zone, Area or Territory level (Area/Territory in "Transactional" or "Non-transactional" mode). Exports the visible grid to Excel. CONFIRMED (`LoadInfo` :647-1170).

## 3. Business Objective
Give sales management a receivable position by market-structure level, with TP (trade price) and gross (with VAT) amounts. INFERRED from column names (…TP, …GrossAmt, …Vat).

## 4. Navigation
Menu path UNKNOWN. `excelButton1_Click` opens `BusinessSummaryViewer.aspx` (copy-paste; unrelated viewer).

## 5. Route
`~/SInventory_UI/RptMIOWiseReceiveableReport.aspx`.

## 6. Source Files
View + code-behind; `TotalSummaryBLL.RptMIOWiseReceiveableReport` (:218) → `TotalSummaryDAL` (:498-511); `OrderInfoBLL.LoadSC/LoadTerritory/LoadZone`; user control `IVMarketSTForZone.ascx`.

## 7. UI Control Inventory
| Control | Label | Default / rules |
|---|---|---|
| rbReportTypeName | Report type | Distribution Center (1, selected) / Zone (2) / Area (3) / Territory (4); DZSM users start on Area with items 1-2 disabled and Zone picker disabled (:49-67); AM users start on Area, items 1-2 disabled and Zone/Area/Group pickers disabled (:68-89) |
| fromDateTextBox / toDateTextBox | From / To Date | today `dd MMMM, yyyy`; both required ("Please Select Date Range!!") |
| IVMarketStructure (GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect) | Market structure | Territory picker enabled only for type 4 |
| rbAmount | Amount basis | Transactional (1, selected) / Non-transactional (2) — only used for Area and Territory |
| viewRptButton | View | `LoadInfo` |
| excelButton1 / btnExportToExcel | Excel | exports the grid for the selected type |
| gv_DistributionCenter / gv_Zone / gv_Area / gv_Territory | Result grids | columns NumberofInvoice, InvoieAmountTP, InvoiceGrossAmt, InvoiceVatTp, RejectAmtTP, RejectGrossAmt, RejectionTpVat, SalesAmtTP, SalesGrossAmt, SalesVat, ReturnAmountTP, ReturnGrossAmt, ReturnAmountVat, JustSalesAmtTP, JustSalesGrossAmt, JustSalesVat, CollectionAmtTP, CollectionGrossAmt, CollectionVat, ReceivableTP, ReceivableGrossAmount, plus ComUnitShortName / RegionName / AreaName / TerritoryName as level key |
| salesCenterDropDownList / territoryDropDownList / zoneDropDownList / rptTypeDropDownList | – | populated (`LoadSC/LoadTerritory/LoadZone` by `Session["UserId"]`) but not used in the query |

## 8. Field Mapping
Page → `RptMIOWiseReceiveableReport(fromdate, todate, Type, ZonId, Area, Terr)` → SP params `@fromdate,@todate,@Type,@Area,@Terr,@ZonId` (`TotalSummaryDAL.cs:498-510`). `Type` values: `SC`, `Zone`, `AreaTran`, `AreaNONTran`, `TerritoryTran`, `TerritoryNONTran`. For Area: ZonId = zone picker, Area = area picker; for Territory additionally Terr = territory picker. For SC / Zone, ZonId/Area/Terr = "" (the Zone branch has ZonId assignment commented out).

## 9. Dropdown Mapping
Market-structure pickers come from the user control (cascading, DB-driven; source not analysed).

## 10. Button Action Mapping
View → `LoadInfo`; radio change → clears all four grids, hides market panel (`divMrk`) unless Area/Territory, enables Territory picker for Territory; Excel → `btnExportToExcel_Click` re-runs `LoadInfo` and streams `.xls` (HTML table) named `<Type>BusinessSummary_List_<timestamp>.xls`.

## 11. Frontend Flow
Select type → (Area/Territory: pick structure + basis) → dates → View → one grid shown. Footer of each grid is filled server-side in `LoadInfo` with column totals: `Math.Round(sum)` formatted `#,##0` (NumberofInvoice rounded to integer). Header rows are custom-built in `*_OnRowCreated` (multi-level headers).

## 12. Backend Flow
`LoadInfo` (:647-1170): clear grids → if both dates present: build Type per selection → SP call → if rows: bind the grid matching Type and compute footer sums with LINQ (`row.Field<decimal?>` null→0); ReceivableTP sum wrapped in try/catch (:798-806). If no dates: "Please Select Date Range!!"; if no rows: "No Data Found!!" (:1152). SP errors propagate (no try/catch around the call).

## 13. API Mapping
None.

## 14. Service Mapping
`TotalSummaryBLL.RptMIOWiseReceiveableReport`; `OrderInfoBLL` loaders.

## 15. Repository Mapping
 `TotalSummaryDAL.RptMIOWiseReceiveableReport` → `GetDataTableAction("sp_RPT_MIS_RptMIOWiseReceiveableReport", params, "SSIDB")` (command timeout 99,000,000 s in `ClsCommonInternalDAL.GetDataTableAction`).

## 16. Stored Procedure Mapping
`sp_RPT_MIS_RptMIOWiseReceiveableReport(@fromdate,@todate,@Type,@Area,@Terr,@ZonId)` — body NOT in repo (UNKNOWN). Also feeds PAGE-337 (`sp_RPT_MIS_BusinessSummary_Acc`, different SP, same UI pattern).

## 17. Database Mapping
UNKNOWN (SP-only). Presumed sources tblInvoice, tblInvoiceDetail, tblCustPayDetail, tblReturnInvoice (INFERRED from column names; not verified).

## 18. Validation Matrix
| Rule | Message | BR |
|---|---|---|
| From and To date both non-empty | "Please Select Date Range!!" (in `LoadInfo` gate :660; Excel button :1195) | BR-M09-039 |
| Empty result | "No Data Found!!" | – |
No check that From ≤ To; `Convert.ToDateTime` on unparseable text throws.

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M09-039 | Report level/mode maps to SP `@Type`: SC, Zone, AreaTran/AreaNONTran, TerritoryTran/TerritoryNONTran (Transactional radio = "Tran"). DZSM and AM roles are steered to the Area view with Distribution-Center/Zone views disabled; AM additionally has Zone/Area/Group pickers locked. Enforcement is client-visible UI state only (no server re-check on View). | RptMIOWiseReceiveableReport.aspx.cs:29-89, :666-709 | CONFIRMED |
| BR-M09-040 | Grid footers are page-side sums of the SP rows rounded to whole taka (`Math.Round`, `#,##0`); receivable = column `ReceivableTP` / `ReceivableGrossAmount` supplied by the SP (formula UNKNOWN). | :726-810 | CONFIRMED (aggregation) / UNKNOWN (receivable formula) |

## 20. Permission Matrix
Session read: RoleTypeName ("DZSM", "AM" special cases), EmpInfoId, RoleTypeId (read but unused for filtering), UserId (dropdown loaders). Data scoping by user territory happens (if at all) inside the SP or the user control — UNKNOWN. Otherwise menu-based.

## 21. Status Lifecycle
n/a (read-only).

## 22. Error Handling
`Page_Load` session reads wrapped in empty catch. `LoadInfo` has no try/catch around SP (errors surface to the error page).

## 23. Audit
None (read-only).

## 24. Integration
None.

## 25. Reports
Grid + Excel (HTML .xls). No Crystal report.

## 26. Dependencies
`IVMarketSTForZone.ascx`, AjaxControlToolkit, TotalSummaryBLL/DAL.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-336-01 | Login as DZSM | Area type preselected; DC/Zone radio disabled | BR-M09-039 |
| TC-336-02 | View without dates | "Please Select Date Range!!" | BR-M09-039 |
| TC-336-03 | Territory + Non-transactional | SP called with Type `TerritoryNONTran`, Terr set | BR-M09-039 |
| TC-336-04 | Compare footer to column sums | rounded to whole numbers | BR-M09-040 |
| TC-336-05 | Excel export of Area view | .xls with the Area grid | – |
| TC-336-06 | Reconcile ReceivableTP with PAGE-306 outstanding for the same period | expected difference documented (different formulas) | GAP-M09-002 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/RptMIOWiseReceiveableReport.aspx(.cs)`, `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:498-511`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Receivable definition | none | SP not in repo | UNKNOWN |

## 30. Known Gaps
SP body missing; role restrictions only cosmetic; `excelButton1_Click` targets an unrelated viewer; unused dropdowns; `Convert.ToDateTime` without TryParse; command timeout effectively unlimited; potential mismatch with PAGE-306 outstanding.

## 31. Change Impact
Same SP family as PAGE-337 (`sp_RPT_MIS_BusinessSummary_Acc`); same page template copied in Aging, DepositSlipReport, BankDepositSAP, DACollection etc. (identical export/Excel code).
