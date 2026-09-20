---
page_id: PAGE-674
module: M16
page: DoctorInfoReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-674 — DoctorInfoReport

> **Evidence level:** Tier A — `DoctorInfoReport.aspx` (1070 lines: 6 GridViews + JS), `DoctorInfoReport.aspx.cs` (2495 lines, read: page load, all `Get_Doctor_Information_*` methods, footers, export, radio handlers; row-created header builders skimmed), `DoctorInfoReportDal.cs` (full). All `sp_Rpt_*` bodies absent. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-674 |
| Module | M16 |
| Page | DoctorInfoReport — "SMC Family Doctor Report" (Summary by Zone/Area/MIO/Doctor, and Details per day) |
| Folder | Reports_UI |
| Output | ASP.NET GridViews (HTML) + Excel (`.xls` HTML table) — **no Crystal Report** |
| Size note | the code-behind is ~2500 lines, most of it repetitive footer/header builders and commented-out code |

## 2. Purpose
Management report of doctor coverage by the field force for a date range: how many doctors are assigned, visited (DCR/coverage), planned (DCP), repeat-visited, prescribers and prescriptions (RX), broken down by weeks 1-4 and by Zone, Area, MIO (territory) or Doctor; plus a "Details" per-doctor per-day matrix (DCP/DCR/RX for day 1-31). CONFIRMED (column headers).

## 3. Business Objective
Monitor call-plan compliance and prescription generation of the "SMC Family Doctor" programme (the report title used in export file names, `DoctorInfoReport.aspx.cs:1406,…`). INFERRED: "SMC" = the SMC type / "Pharma Platform" concept used across the module.

## 4. Navigation
Menu row DB-driven (`UNKNOWN`). Reset → `Response.Redirect("DoctorInfoReport.aspx")`.

## 5. Route
`~/Reports_UI/DoctorInfoReport.aspx`.

## 6. Source Files
`Solution.Web/Reports_UI/DoctorInfoReport.aspx`, `.aspx.cs`, `Library.DAL/DoctorInfo_DAL/DoctorInfoReportDal.cs` (SP wrappers), `IVMarketStructure.ascx(.cs)` (visible here), `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:865` (`GetDoctorWiseDayList`, only used by dead `LoadData()`).

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| rbReportTypeName (RadioButtonList, AutoPostBack) | (report type) `1` Summary / `2` Details | **Yes** at Search (`showMessageBox("Please Report Type!")`) | none | postback clears grids, toggles panels (`:1814-1859`) |
| rdoAnotherReport (RadioButtonList, AutoPostBack) | Summary view: `Zone` Zone Wise, `Area` Area Wise, `MIO` MIO Wise, `Doctor` Doctor Wise (`Provider` option commented out) | required for Summary but **not validated**: none selected ⇒ nothing happens | none | shows Zone/Area/Territory dropdowns as needed (`:1861-1933`) |
| txtFromDate / txtTodate | From Date / To Date | No | first / last day of current month ("dd MMMM, yyyy") | passed as text to SPs (`@frmDate/@toDate`) |
| IVMarketStructure (Zone/Area/Territory dropdowns; `MarketSt` panel) | Market structure | Details export requires Zone | none | `divZone/divArea/divTerritory` panels toggled per view |
| chkDownload | "Is only download" | – | unchecked | Details only: run and download without rendering |
| lblInfo | "Last Process Date" | – | from `sp_GET_SMCFamilyDoctorLastProcessDate` | try/catch, blank on failure |
| btnSearch / btnExportToExcel / resetBtn | – | – | – | – |
| loadGridView (Provider), GridView1 (MIO), gv_Zone, gv_Area, GridView2 (Doctor), GridView3 (Details) | result grids | – | – | show/hide via panels `PPOVIDERWISE, MIOWISE, Details, DocWise` |

## 8. Field Mapping
| UI | DAL method | SP | Params |
|---|---|---|---|
| Summary + Provider (dead option) | `Get_Dactor_Information_Report(from,to,"")` | `sp_RPT_DoctorInfoReport` | `@frmDate,@toDate,@Parameter` |
| Summary + MIO | `Get_Dactor_Information_MIO_Wise` | `sp_Rpt_MIOwiseDoctorWeek` | `+ @Zone,@Area,@Teritory` (DBNull when empty) |
| Summary + Zone | `Get_Dactor_Information_Zone_Wise` | `sp_Rpt_ZonewiseDoctorWeek` | `@frmDate,@toDate,@Parameter,@Zone` |
| Summary + Area | `Get_Dactor_Information_Area_Wise` | `sp_Rpt_AreawiseDoctorWeek` | `+ @Zone,@Area` |
| Summary + Doctor | `Get_Dactor_Information_Doc_Wise` | `sp_Rpt_DoctorwiseDoctorWeek` | `+ @Zone,@Area,@Teritory` |
| Details | `Get_Dactor_Information_Details` | `sp_Rpt_SMCFamilyDoctorReport_ProcessData` | `+ @Zone,@Area,@Teritory` |
| Last process date | `GetLastProessDate("")` | `sp_GET_SMCFamilyDoctorLastProcessDate(@id)` | – |
(`@Parameter` is always `""` in this page.) Result columns bound in the aspx: MIO grid — `RegionName, AreaName, MIOCode(=Territory), MIOName, AssignDocCount(Assign), DCRDocCount(Coverage), DCPTotalCount(Total DCP), RepatCount(Repeat), NORXCount(No. of Prescriber), RXCount(Prescription)` and per week `{n}WeekV`(Coverage), `{n}WeekDCP`, `{n}WeekR`, `{n}WeekNP`, `{n}WeekP` for n=1..4; Doctor grid — `RegionName, AreaName, MIOCode, MIOName, DoctorName, ProgramTypeName(Provider), DoctorTypeName, DegreeName, DoctorSpeciality, DCPTotalCount, DCRDocCount, RXCount, {n}WeekDCP/V/P`; Details grid — same doctor attributes + `{d}_DCP, {d}_DCR, {d}_RX` for d=1..31 and a `to_*` total set.

## 9. Dropdown Mapping
Zone/Area/Territory: `IVMarketStructure` (active market-structure loaders, `_dataLoad.GetZone_byGroupId_*` etc.). `LoadInitialInfo()` fetches the employee list but assigns nothing (commented out).

## 10. Button Action Mapping
| Button | Handler | Behaviour |
|---|---|---|
| Search | `btnSearch_Click` (`:342-386`) | if `rbReportTypeName.SelectedValue==""` ⇒ alert "Please Report Type!"; `"2"` ⇒ `Get_Doctor_Information_Details()`; else per `rdoAnotherReport`: Provider/MIO/Zone/Area/Doctor ⇒ matching method; **none selected ⇒ nothing, no message** |
| Export to Excel | `btnExportToExcel_Click` (`:1393-1742`) | see BR-M16-054 |
| Reset | `resetBtn_Click` | reload |
| JS `exporttocsv()`/`exportToExcel()` | unused/legacy client exporters (`:749-800`); `exporttocsv` compares radio values to `"1"/"2"/"3"` while radios carry `Zone/Area/MIO/Doctor` ⇒ file name prefix never set |

## 11. Frontend Flow
First load: dates = current month; "Last Process Date" banner. Toggling report type / view postbacks (UpdatePanel; `PostBackTrigger` on export button) and resets grids and dropdowns (`F_ZoneSelect.SelectedValue = null` etc.). Grid header bands are added in `OnRowCreated` handlers (e.g. "Assign / Visited / Repeat / Prescription" bands, `loadGridView_OnRowCreated:394-440`, and analogues for GridView1/2/3, gv_Zone, gv_Area).

## 12. Backend Flow
Each `Get_Doctor_Information_*` method: read Zone/Area/Territory `SelectedValue` from the market-structure dropdowns (empty ⇒ null), call the DAL SP, toggle the four panels (`MIOWISE, PPOVIDERWISE, Details, DocWise`), bind the grid, and (MIO, Doctor, Details, Zone, Area) compute **grand totals** by summing each numeric column across the DataTable with `row.Field<int?>(col) == null ? 0 : row.Field<int>(col)` and writing them into specific footer cell indexes (e.g. MIO grid footer cells 5-30, Doctor grid footer 9-24) with `catch {}` swallowing any mismatch (an SP column/type change silently removes totals). Export: `btnExportToExcel_Click` — for Details (`rbReportTypeName.Items[1]`): requires Zone (else `showMessageBox("Please Select Zone!!")`); with `chkDownload` it re-runs `Get_Doctor_Information_Details()` and renders GridView3; without it renders the already-bound GridView3; for Summary: renders `gv_Zone`, `gv_Area`, `GridView1` or `GridView2` depending on `rdoAnotherReport`; output = `application/ms-excel` HTML with header "… (Date Range : from- to) Print Date: …", file `"<Type> <dd_MMM_yyyy_hh_mm_tt>.xls"`; any exception ⇒ `showMessageBox("No Data Found!!")`.

## 13. API Mapping
`[WebMethod] GetMileageClaimList(param)` (`:1769-1777`) — unrelated leftover calling `Setup2DAL.GetMileageClaimList` (an HR/mileage SP) with an arbitrary `param`; no caller in this page's markup (dead, but publicly callable).

## 14. Service Mapping
None (page → `DoctorInfoReportDal`, `CmnCrystaltoView`, `SetupDAL`, `Setup2DAL`).

## 15. Repository Mapping
`DoctorInfoReportDal` methods (see §8).

## 16. Stored Procedure Mapping
`sp_RPT_DoctorInfoReport`, `sp_Rpt_ZonewiseDoctorWeek`, `sp_Rpt_AreawiseDoctorWeek`, `sp_Rpt_MIOwiseDoctorWeek`, `sp_Rpt_DoctorwiseDoctorWeek`, `sp_Rpt_SMCFamilyDoctorReport_ProcessData` (name suggests it **processes/stores** a snapshot; "ProcessData"), `sp_GET_SMCFamilyDoctorLastProcessDate`, `sp_GET_DZSMProcessDate` (DAL only), `sp_GetDCRRXDoctorWiseRptView` (dead `LoadData`), `sp_Get_MileageClaimList` (dead WebMethod). **No body is in the repository.**

## 17. Database Mapping
UNKNOWN (all via SPs). The "Last Process Date" implies a precomputed table refreshed by a job (INFERRED).

## 18. Validation Matrix
| Rule | Message |
|---|---|
| Report type chosen | "Please Report Type!" |
| Details export needs a Zone | "Please Select Zone!!" |
| Dates | none (free text; parsed by SP) |
| Summary view chosen | none (silent no-op) |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-049 | A report type (Summary / Details) must be selected before Search | `Solution.Web/Reports_UI/DoctorInfoReport.aspx.cs:344-385` | `btnSearch_Click` | CONFIRMED |
| BR-M16-050 | For Summary, one of Zone/Area/MIO/Doctor view must be chosen; otherwise Search does nothing (no message) | `DoctorInfoReport.aspx.cs:350-378` | `btnSearch_Click` | CONFIRMED |
| BR-M16-051 | Default period = first to last day of current month | `DoctorInfoReport.aspx.cs:52-56` | `Page_Load` | CONFIRMED |
| BR-M16-052 | "Last Process Date" from `sp_GET_SMCFamilyDoctorLastProcessDate` is shown so users know the data snapshot age | `DoctorInfoReport.aspx.cs:58-69` | `Page_Load` | CONFIRMED (semantics INFERRED) |
| BR-M16-053 | Grand totals in grid footers are plain sums of the returned columns (`null` ⇒ 0); errors are swallowed | `DoctorInfoReport.aspx.cs:494-599,941-1005,1047-…` | `Get_Doctor_Information_*` | CONFIRMED |
| BR-M16-054 | Excel export: Details requires a Zone; "Is only download" re-queries and exports without displaying; other views export the current grid; failure message is always "No Data Found!!" | `DoctorInfoReport.aspx.cs:1393-1742` | `btnExportToExcel_Click` | CONFIRMED |

## 20. Permission Matrix
No permission check, no role scoping (an MIO/AM can query any Zone/Area/Territory by choosing it; role-based defaults like other pages' `Validatea()` are absent). Menu gating only.

## 21. Status Lifecycle
Not applicable (read-only). Note: no `@ApprovalStatus` filter is passed — whether the SPs count only approved DCR/RX is `UNKNOWN`.

## 22. Error Handling
Broad `catch {}` around last-process-date, footer totals; export `catch { showMessageBox("No Data Found!!") }`. SP errors on Search are not caught (unhandled).

## 23. Audit
None.

## 24. Integration
Depends on an out-of-repo data-processing job that fills the "SMC Family Doctor" snapshot (INFERRED).

## 25. Reports
This page is itself the report; see `reports/RPT-M16-02-doctor-info-smc-family.md`.

## 26. Dependencies
`IVMarketStructure`, `SeedDataDAL`, `CmnCrystaltoView` (legacy), `Setup2DAL`, `SetupDAL`, jQuery `tableToCSV`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Search with nothing selected | alert "Please Report Type!" | 049 |
| T2 | Summary chosen, no view selected | nothing happens | 050 |
| T3 | Summary + Zone, current month | zone grid + footer totals | 053 |
| T4 | Details without Zone → Export | "Please Select Zone!!" | 054 |
| T5 | Details + Zone + "only download" | `.xls` downloaded, grid not shown | 054 |
| T6 | Column type changed by SP | totals silently missing | 053 |
| T7 | Call `GetMileageClaimList` WebMethod with any string | returns mileage rows (unrelated) | §13 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Metric definitions (Assign, Coverage, Repeat, No. of Prescriber, Prescription) | None existed | header text only | SP absent | UNKNOWN | GAP-M16-045 |

## 30. Known Gaps
GAP-M16-043 (metric formulas UNKNOWN), GAP-M16-044 (`ProcessData` SP may write / heavy), GAP-M16-045 (no role scoping/permission), GAP-M16-046 (swallowed exceptions, silent no-op), GAP-M16-047 (dead legacy code and unrelated public WebMethod), GAP-M16-048 (2500-line code-behind with duplicated footer logic; brittle column indexes).

## 31. Change Impact
`sp_Rpt_*DoctorWeek` are only referenced through `DoctorInfoReportDal` from this page (grep). `DoctorInfoReportDal` is also instantiated by `Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs` (another module; uses the territory process-date SP). `CmnCrystaltoView` (shared with PAGE-671/672/673/675 and other modules' dynamic sales reports). Changing SP column names breaks footer sums (caught silently).
