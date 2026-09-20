---
page_id: PAGE-257
module: M07
page: ChallanReportView
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-257 — ChallanReportView

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-257 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | ChallanReportView |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_ChallanReportView |
| aspx size | 19963 bytes |
| Code-behind LOC | 380 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |


## 2. Purpose
CONFIRMED: "View List" of issue challans (requisitions with `Submit='OK'`) between a From and To date for one Sales Center or all ("National Report" checkbox), with per-row Edit (popup PAGE-258) and Print (`ChallanReportViewer`), Summary/Details grids (buttons currently `Visible=false`) and Excel export. Also documents the edit popup `ChallanVIewEditNew.aspx` (PAGE-258), reviewed here: it edits challan date, driver and truck.

## 3. Business Objective
INFERRED: let the central warehouse/DC review, reprint and correct issue challans (e.g. add truck and driver missing at issue time, GAP-M07-029) and export challan quantities/values.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CreatePickingOnWareHouse.aspx`

Outbound references: `challanreportviewer.aspx`, `challanvieweditnew.aspx`, `createpickingonwarehouse.aspx`


## 5. Route
`~/SInventory_UI/ChallanReportView.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ChallanReportView.aspx |
| Code-behind | Solution.Web/SInventory_UI/ChallanReportView.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ChallanReportViewBLL (Library.BLL/SInventory_BLL/ChallanReportViewBLL.cs) |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | ChallanReportViewDAL (Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |


## 7. UI Control Inventory
| Control | Type | Label | Required | Default | Source / handler |
|---|---|---|---|---|---|
| CheckBox1 | CheckBox | "National Report :" | - | unchecked | ticking sets SC = "" and disables the SC list (`CheckBox1_CheckedChanged`) |
| salesCenterDropDownList | DropDownList | "Sales Center" | if not national | index 1 | `OrderInfoBLL.LoadSC(ddl, Session["UserId"])` |
| dateTextBox / TextBox1 (+calendars dd-MMM-yyyy) | TextBox | "Challan From Date" / "Challan to Date" | none server-side (`Convert.ToDateTime` -> FormatException when empty) | empty | - |
| searchButton "View List" / btnReload "Reload" | Button | - | - | - | `searchButton_Click`, `btnReload_Click` |
| detailsButton / summaryButton | Button | "Details Report" / "Summary Report" | - | `Visible=false` | `DetailsGrid`, `SummaryGrid` |
| excelButton1 | Button | "Export to Excel" | - | - | `btnExportToExcel_Click` |
| reportListGridView (`DataKeyNames=ReqId`) | GridView | D.C.Code, D.C.Name, Req.No, Req.Date, DeliveryClnNo, DeliveryClnDate, Edit, Print | - | - | Edit -> `loadGridView_RowCommand` "EditData" popup; Print -> `printButton_Click` |
| PAGE-258 controls | clnDateTextBox (calendar, required), truckNoTextBox, driverNameTextBox, hidden `salesCenterInfoIdHiddenField` (ReqId from `?ID=`), bmitButton, MessageLabel | - | date required ("Please Enter Challan Date!!"), truck/driver optional | `bmitButton_Click` |

## 8. Field Mapping
PAGE-258: clnDateTextBox -> `Requesition.IssuChalanDate` -> `@IssuChalanDate` -> tblRequisition.SubmitDate, PickingDate, IssuChalanDate (all three set to the same value); driverNameTextBox -> `@DriverName` -> DriverName; truckNoTextBox -> `@TruckNo` -> TruckNo; ID -> `@ReqId`.

## 9. Dropdown Mapping
`OrderInfoBLL.LoadSC(ddl, UserId)` only.

## 10. Button Action Mapping
| Button | Chain | DB change |
|---|---|---|
| View List | `ChallanReportViewBLL.ChallanReportDAL2(from,to[,unit])` -> `SELECT * FROM tblRequisition WHERE [ComUnitId=@ComUnitId and] Submit='OK' AND SubmitDate BETWEEN @FromDate and @ToDate order by ReqNo` (`ChallanReportViewDAL.cs:329-339`) | none |
| Summary / Details | `ChallanSummaryReportDAL2` / `ChallanDetailReportDAL2` (queries built by `ChallanSummaryReportQuery`/`ChallanDetailReportQuery`, tblRequsitionChild x tblStockInTransfar x price) | none |
| Print | popup `ChallanReportViewer?reqId=<ReqId>` | none |
| Edit (PAGE-258 Save) | `ChallanReportViewBLL.UpdateDataForSalesCenter` -> `UPDATE tblRequisition SET SubmitDate=@d, PickingDate=@d, IssuChalanDate=@d, DriverName, TruckNo WHERE ReqId=@ReqId` (`ChallanReportViewDAL.cs:126-131`) | tblRequisition |
| Excel | re-runs Details/Summary grid and streams `ChallanReport.xls` (HTML table) with hard-coded company header | none |

## 11. Frontend Flow
Popup edit: `window.open('ChallanVIewEditNew.aspx?ID=<ReqId>', ..., 600x700)`; after save the user presses Reload. Excel export renders the GridView into an HTML `.xls` response (`VerifyRenderingInServerForm` overridden).

## 12. Backend Flow
`Page_Load` loads SC (SC index 1). `searchButton_Click` clears grids and branches on SC empty (national) vs unit. PAGE-258 `ComUnitLoad` reads `LoadRequisitionIssueInfo(ReqId)`; `bmitButton_Click` validates date then updates; result label "Data Update Successfully!!! Please Reload" / "Data Not Update!!!" (red). No try/catch, no transaction.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| ChallanReportViewBLL.ChallanReportDAL2 | Library.BLL/SInventory_BLL/ChallanReportViewBLL.cs:456 |
| ChallanReportViewBLL.ChallanSummaryReportDAL2 | Library.BLL/SInventory_BLL/ChallanReportViewBLL.cs:498 |
| ChallanReportViewBLL.ChallanDetailReportDAL2 | Library.BLL/SInventory_BLL/ChallanReportViewBLL.cs:488 |


## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| ChallanReportViewDAL.ChallanReportDAL2 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:330 | inline SQL: tblRequisition |  |
| ChallanReportViewDAL.DateRangeTable | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:433 | delegates / no SQL literal | @ComUnitId, @FromDate, @ToDate |
| ChallanReportViewDAL.ChallanSummaryReportDAL2 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:397 | delegates / no SQL literal |  |
| ChallanReportViewDAL.ChallanSummaryReportQuery | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:463 | inline SQL: tblCentralStore, tblProduct, tblRequisition, tblStockInTransfar |  |
| ChallanReportViewDAL.ChallanDetailReportDAL2 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:385 | delegates / no SQL literal |  |
| ChallanReportViewDAL.ChallanDetailReportQuery | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:447 | inline SQL: tblCentralStore, tblProduct, tblRequisition, tblStockInTransfar |  |


## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).


## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCentralStore, tblCompanyUnit, tblProduct, tblRequisition, tblStockInTransfar, tblUnitPrice, tblUserCompanyUnit, tblWHStockInDetail
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).


## 18. Validation Matrix
| Rule | Message |
|---|---|
| No rows | alert "No Data Found!" |
| Excel with no data | alert "No Data Found!!" |
| PAGE-258 date empty | alert "Please Enter Challan Date!!" |
| Dates on list | none (FormatException) |

## 19. Business Rules
| BR ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M07-054 | Challan list = requisitions with `Submit='OK'` whose `SubmitDate` is within the inclusive date range, for the chosen Sales Center or all when "National Report" is ticked; ordered by ReqNo. Sales Center list is per user, but the query filters by ComUnitId of the requisition only. | `ChallanReportViewDAL.cs:329-339`; `ChallanReportView.aspx.cs:30-77,197-207` | CONFIRMED |
| BR-M07-055 | Editing a challan sets SubmitDate, PickingDate and IssuChalanDate to the one entered date and overwrites DriverName/TruckNo; there is no status, ownership, date-range or stock check, and challan number/quantities cannot be changed. | `ChallanReportViewDAL.cs:126-131`; `ChallanVIewEditNew.aspx.cs:29-57` | CONFIRMED |
| BR-M07-056 | Excel export writes `ChallanReport.xls` from the Details grid (or Summary grid if only that is loaded) with From/To date and print date headers; the Summary export carries a hard-coded company name, address, BIN and "Issueing Office: CWH/Factory". | `ChallanReportView.aspx.cs:193-347` | CONFIRMED |
| BR-M07-057 | Print of a challan uses `crpChallan.rpt` with STO main/detail data for the ReqId (no re-validation). | `ChallanReportView.aspx.cs:161-176`; `ChallanReportViewer.aspx.cs:18-36` | CONFIRMED |

## 20. Permission Matrix
Session `UserId` only (SC list). No role check; `ChallanVIewEditNew?ID=` accepts any ReqId (no ownership check).

## 21. Status Lifecycle
No status change; challan dates editable after issue. `Submit='OK'` is the challan-issued flag set by PAGE-259.

## 22. Error Handling
No try/catch; FormatException on empty dates; `MessageLabel` for edit result.

## 23. Audit
No audit (no UpdateBy/UpdateDate in the edit UPDATE).

## 24. Integration
No integration-related classes or SPs detected from this page.


## 25. Reports
No report file referenced from this page.


## 26. Dependencies
BLL: ChallanReportViewBLL, OrderInfoBLL; DAL: ChallanReportViewDAL, OrderInfoDAL; DAO: —; Tables (inline): tblCentralStore, tblCompanyUnit, tblProduct, tblRequisition, tblStockInTransfar, tblUnitPrice, tblUserCompanyUnit, tblWHStockInDetail


## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-257-01 | National report | SC disabled, all units listed | 054 |
| TC-257-02 | Range boundaries | challan on To date included | 054 |
| TC-257-03 | Empty date | FormatException | - |
| TC-257-04 | Edit date/truck/driver | 3 date columns equal; driver/truck stored | 055 |
| TC-257-05 | Edit with empty date | "Please Enter Challan Date!!" | 055 |
| TC-257-06 | Excel details | ChallanReport.xls with header | 056 |
| TC-257-07 | Print | crpChallan opens for ReqId | 057 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ChallanReportView.aspx`, `Solution.Web/SInventory_UI/ChallanReportView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ChallanReportView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 56 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ChallanReportView.aspx |
| Data access | This document §15–17 | 0 SP names, 8 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |


## 30. Known Gaps
GAP-M07-030 (unrestricted edit), GAP-M07-031 (hard-coded company text), GAP-M07-032 (duplicated challan BLL), reload needed after edit, `detailsGridView` empty-branch binds `reportListGridView`.

## 31. Change Impact
Shares `ChallanReportViewBLL/DAL` with PAGE-256 (`ChallanReportList`) and requisition pages; edit popup PAGE-258; print viewer PAGE-238. Writes to `tblRequisition` shared with PAGE-259.
