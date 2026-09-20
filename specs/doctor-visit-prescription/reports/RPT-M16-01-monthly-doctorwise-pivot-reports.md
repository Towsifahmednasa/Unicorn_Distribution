# RPT-M16-01 — Doctor-wise date-range pivot reports (RX / DCR / CVR / DCP)

## Report family
Four near-identical pages in `Solution.Web/Reports_UI/` (folder-level "Doctorise Monthly" reports) that render a **dynamic pivot** (one column per calendar day of the chosen range) produced by `DynamicPivot*` stored procedures. **None uses Crystal Reports**: there is no `.rpt` for M16 (`find` over `Library.CrystalReports` and `Solution.Web/Reports/CrystalReports` shows only picking-list reports, `rptDCPicking*`, unrelated to doctors). Output is an HTML `<table id='tblTable'>` built in code-behind and injected into `<div id="data" runat="server">`; export is client-side CSV via jQuery `tableToCSV` (`exporttocsv()` on `#export`); the server "Export to Excel" buttons are `Visible=false` with commented-out handlers.

| Page | ID | Report types offered in UI | Code |
|---|---|---|---|
| `RXDoctoriseMonthlypt.aspx` | PAGE-675 (Tier A) | 1 Doctor Wise, 2 Product Brand Wise, 3 Product Wise, 4 User Wise, 5 User & Product Wise, 6 Dynamic RX Status Report | `RXDoctoriseMonthlypt.aspx.cs` |
| `DcrDoctoriseMonthlypt.aspx` | PAGE-673 (Tier A) | 1 Doctor Wise, 2 Product Brand Wise, 4 User Wise (3, 5, 6 exist only in code) | `DcrDoctoriseMonthlypt.aspx.cs` |
| `CVRDoctoriseMonthlypt.aspx` | PAGE-671 (Tier B, read for this document) | 1 "Customer Wise" only (2-6 commented out of markup; code branches remain) | `CVRDoctoriseMonthlypt.aspx.cs` |
| `DcpDoctoriseMonthlypt.aspx` | PAGE-672 (Tier B, read for this document) | 1 Doctor Wise, 2 User Wise, 3 Customer Wise | `DcpDoctoriseMonthlypt.aspx.cs` |
DCR = doctor call report (visit done); DCP = doctor call plan; CVR = customer visit report (customers rather than doctors — "Customer Wise" title; INFERRED from the `...CVR_New_ForSearch` SP name and label); RX = prescription.

## Request parameters (no QueryString; all form fields)
| Page field | Meaning | Sent as |
|---|---|---|
| `fromDateTextBox`/`toDateTextBox` ("dd MMMM, yyyy"; default first/last day of current month) | period | `@Month` = from, `@Year` = to (parameter names are misleading) and `@ListToPivot` = `mainDate` from `DynamicDateByDateRange(@frmDate,@toDate)` row 0 |
| `ddlApprovalStatus` (from `sp_GET_ActionStatusList`) | approval status filter | `@ApprovalStatus` (`"Select"` when empty) |
| `ddlProgramType` (Provider Type), `ddlPharmaPlatform`, `DoctorTypeSelect` | provider filters | text or id depending on report type (BR-M16-059) |
| `IVMarketStructure(Market)` Zone / Area / Territory | hierarchy | `@ZoneSelect,@AreaSelect,@TeritorySelect` (territory read only if Area chosen, BR-M16-058) |
| `ddlProduct`, `ddlMIO`, `ddlBrand` (only some reports) | product / user / brand | `@ProID`, `@EmpID`, `@Brand` |
Session: `RoleTypeName` (AM ⇒ Area required, DZSM ⇒ Zone required), `EmpInfoId`, `RoleTypeId` (used by the hierarchy control prefill on Reports_UI/IVMarketStructureMarket.ascx).

## SPs feeding the reports (bodies absent from repo — column-level output UNKNOWN)
`@ColumnToPivot` is always the constant `'DcrDate'` and `@ListToPivot` the date list. Common parameter block "P10" = `@Month,@Year,@ListToPivot,@ColumnToPivot,@ApprovalStatus,@ProviderType,@PharmaPlatform,@ZoneSelect,@AreaSelect,@TeritorySelect`; "P11" = P10 + `@DoctorTypeSelect` (inserted after `@PharmaPlatform`).

| Stored procedure | DAL method (`CmnCrystaltoView.cs`) | Params | Live callers (page → report type) |
|---|---|---|---|
| `DynamicDateByDateRange` | `GetDynamicDateByDateRange` :1697 | `@frmDate,@toDate` → row 0 `mainDate` | PAGE-671, 672, 673, 675 (all) |
| `DynamicPivotDoctorWiseRX_New` | `GetRXDoctorWiseDayList` :980 | P11 | PAGE-675 type 1 |
| `DynamicPivotBrandWiseRX_new` | `GetRXDoctorBrandWiseList` :1636 | P11 | PAGE-675 type 2 (also commented code in PAGE-674) |
| `DynamicPivotProductdWiseRX_New` | `GetRXDoctorProductWiseList` :1501 | P11 | PAGE-675 type 3 |
| `DynamicPivotUserWiseRX_New` | `GetRXUserWiseList` :1346 | P11 | PAGE-675 type 4 |
| `DynamicPivotUserandProductdWiseRX_New` | `GetRXUserProductWiseList` :1383 | P11 + `@ProID` | PAGE-675 type 5 |
| `GetDynamicVisitStatusReportRX` | `GetDynamicRXStatusReport` :1462 | P11 + `@ProID,@EmpID,@Brand` | PAGE-675 type 6 |
| `DynamicPivotDoctorWiseDCR_New_ForSearch` | `GetDCRDoctorWiseDayList` :1015 | P10 | PAGE-673 type 1 |
| `DynamicPivotBrandWiseDCR` | `GetDoctorBrandWiseList` :1601 | P10 | PAGE-673 type 2; PAGE-671 (hidden branch) |
| `DynamicPivotProductdWiseDCR` | `GetDoctorProductWiseList` :1240 | P10 | PAGE-673 type 3 (unreachable); PAGE-671 (hidden) |
| `DynamicPivotUserWiseDCR` | `GetDCRUserWiseList` :1274 | P10 | PAGE-673 type 4; PAGE-671 (hidden) |
| `DynamicVisitStatusReport` | `GetDynamicVisitStatusReport` :1090 (11 args, sends P10 + `@EmpID,@DCType`) and :1423 (12 args, sends `@Month,@Year,@ListToPivot,@ColumnToPivot,@ApprovalStatus,@ProviderType,@PharmaPlatform,@DoctorTypeSelect,@ZoneSelect,@AreaSelect,@TeritorySelect,@EmpID,@Brand`; the 12-arg overload is the one PAGE-673/671 call) | see left | PAGE-673 type 6 (unreachable); PAGE-671 (hidden) |
| `DynamicPivotDoctorWiseCVR_New_ForSearch` | `GetCVRDoctorWiseDayList` :1052 | P10 | PAGE-671 type 1 |
| `DynamicPivotDoctorWiseDCP` | `GetDCPDoctorWiseDayList` :1163 | P10 | PAGE-672 type 1 |
| `DynamicPivotDoctorWiseDCPCustomerWise` | `GetCusDCPDoctorWiseDayList` :1202 | P10 | PAGE-672 type 3 |
| `DynamicPivotUserWiseDCP` | `GetDCPUserWiseList` :1310 | P10 | PAGE-672 type 2 |
| `DynamicPivotDoctorWiseDoctorVisitPlan` | `DoctorVisitDAL.DynamicPivotDoctorWiseDoctorVisitPlan` (`DoctorVisitDAL.cs:134`) | `@param` (raw SQL fragment) | PAGE-664 |
| `sp_GetRXDoctorWiseRpt` (`@Month,@MonthValue,@Year,@ApprovalStatus`) | `GetRXDoctorWiseDayListNew` :901 and (copy-paste) `GetDCRDoctorWiseDayListNew` :941 | – | **no callers** (dead) |
| `DynamicPivotDWSP`, `DynamicDatebyMonthYear(ForStuff)`, `sp_Process_DWSPReport_Territory` | other methods of the same DAL | – | other modules (not M16) |

## Filters & scoping
See PAGE-675 §18-§20. Only AM and DZSM roles have a mandatory-selection rule; `MIO` and `NSM` have none in the page (the hierarchy control pre-fills the user's own structure: `Reports_UI/IVMarketStructureMarket.ascx.cs:100-145` — MIO group/zone/area/territory prefilled with group, zone and area dropdowns locked; AM group/zone prefilled and locked (area selectable); DZSM group and zone prefilled, group locked; NSM group prefilled and locked). Whether the SPs additionally enforce ownership is UNKNOWN.

## Output & export
Generic renderer: header row from `DataColumn.ColumnName`, one `<td>` per cell via `cell.ToString()` (no encoding, BR-M16-061). DCR page strips columns "Provider Type", "Pharma Platform", "Region", "Territory". No totals row from the page. Export: browser CSV of `#export` (filename `<view> RX Report` on PAGE-675). No Excel/PDF server export.

## Known issues
GAP-M16-037 (bodies absent), -038, -039 (territory filter bug), -040, -041 (HTML injection risk if doctor/product names contain markup), -042, -049 (static shared `CmnCrystaltoView`), -050 (unreachable branches, copy-paste DAL), -051 (CVR Reset → DCR page).

## Tests
See PAGE-675 §27 and PAGE-673 §27; add: compare DCR pivot total for a doctor/day with `DCRList` rows of the same day and status; verify the day columns equal the range length (`DynamicDateByDateRange`); verify `"Select"` behaves as "all statuses".
