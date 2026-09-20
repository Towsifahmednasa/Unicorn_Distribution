---
page_id: PAGE-675
module: M16
page: RXDoctoriseMonthlypt
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-675 — RXDoctoriseMonthlypt

> **Evidence level:** Tier A — `RXDoctoriseMonthlypt.aspx` (JS export), `.aspx.cs` (800 lines, read fully), `CmnCrystaltoView.cs` DAL methods (parameter lists read). `DynamicPivot*` / `DynamicDateByDateRange` / `GetDynamicVisitStatusReportRX` bodies absent. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-675 |
| Module | M16 |
| Page | RXDoctoriseMonthlypt — "RX (prescription) report, doctor-wise monthly / date-range pivot" |
| Folder | Reports_UI |
| Output | dynamically built HTML `<table>` injected into `<div id="data" runat="server">` (`data.InnerHtml`); client-side CSV export; no Crystal Report |
| Sibling pages | DcrDoctoriseMonthlypt (PAGE-673), CVRDoctoriseMonthlypt (PAGE-671), DcpDoctoriseMonthlypt (PAGE-672) — near-identical copies (see `reports/RPT-M16-01-...`) |

## 2. Purpose
Show, for a date range, a pivot with one column per calendar day of the range and one row per doctor / brand / product / user / user+product, giving the prescription counts; or a "Dynamic RX Status Report". CONFIRMED (view names, `@ListToPivot=dates`).

## 3. Business Objective
Track prescription generation (RX) by doctor, product, brand and MIO for a period, filtered by provider type, pharma platform, doctor type, approval status and market structure. INFERRED.

## 4. Navigation
Menu `UNKNOWN`. Reset → `RXDoctoriseMonthlypt.aspx`.

## 5. Route
`~/Reports_UI/RXDoctoriseMonthlypt.aspx`.

## 6. Source Files
`Solution.Web/Reports_UI/RXDoctoriseMonthlypt.aspx(.cs)`, `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs` (lines 838-850, 980-1013, 1346-1381, 1383-1421, 1462-1499, 1501-1600, 1636-1670), `Library.BLL/SInventory_BLL/ProductBLL.LoadProductSQ`, `IVMarketStructure.ascx(.cs)` (visible), `CommonDataLoad`, `SeedDataDAL`.

## 7. UI Control Inventory
| Control | Label | Default | Notes / source |
|---|---|---|---|
| rbType (DCR / RX) | (type) | RX selected | **not used** by `LoadData` for anything but a local `Type` variable (BR-M16-060) |
| rbReportTypeName (AutoPostBack) | report type: 1 Doctor Wise, 2 Product Brand Wise, 3 Product Wise, 4 User Wise, 5 User & Product Wise, 6 Dynamic RX Status Report | 1 | index-based reads `Items[0..5]` |
| fromDateTextBox / toDateTextBox | From Date / TO Date | first / last day of current month "dd MMMM, yyyy" | – |
| ddlBrand | Brand Filter (visible only for report 6 via `divBrand`) | none | `ProductBLL.LoadProductSQ` |
| ddlProgramType | Provider Type | none | `GetProgramTypeListParam(" and GRP.IsDoctor=1")` |
| DoctorTypeSelect | Doctor Type | none | `sp_Get_DoctorType_Active` |
| ddlPharmaPlatform | Pharma Platform | none | `GetSMCTypeListParam(" and GRP.forDotor=1")` |
| ddlApprovalStatus | Approval Status | none | `sp_GET_ActionStatusList` |
| ddlProduct | Product Name (visible for report 5 via `divPro`) | none | `sp_Get_Product_List` |
| ddlMIO / EmployeeIdSelect | User (`divUser`, report 6) | none | `sp_CS_GetEmpGeneralInfo_All` via `GetEmployeeList_All` |
| IVMarketStructure | Zone / Area / Territory | none | visible on this page |
| btnSearch / resetBtn | – | – | – |
| btnExportToExcel | (server) | `Visible=false`, handler body fully commented out | dead |
| "Export to Excel" button | `exporttocsv()` | – | jQuery `tableToCSV` on `#export`, file name `"<view name> RX Report"` |

## 8. Field Mapping
| UI | DAL param | SP param |
|---|---|---|
| fromDateTextBox / toDateTextBox | `Month` / `Year` (**misused names**) | `@Month` / `@Year` (= from / to text) |
| `DynamicDateByDateRange(from,to)` result row 0 col `mainDate` | `mainDate` | `@ListToPivot` (comma list of dates; `@ColumnToPivot='DcrDate'` constant) |
| ddlApprovalStatus (empty ⇒ literal `"Select"`) | `ApprovalStatus` | `@ApprovalStatus` |
| ddlProgramType `.SelectedItem.Text` | `ProviderType` | `@ProviderType` (text, not id) |
| ddlPharmaPlatform `.SelectedItem.Text` | `PharmaPlatform` | `@PharmaPlatform` |
| DoctorTypeSelect `.SelectedItem.Text` | `DoctorTypeSelect` | `@DoctorTypeSelect` |
| Zone / Area | `ZoneSelect`, `AreaSelect` | `@ZoneSelect`, `@AreaSelect` (ids) |
| Territory (only read when **Area** is selected — BR-M16-058) | `TeritorySelect` | `@TeritorySelect` |
| ddlProduct | `ProID` | `@ProID` |
| ddlMIO | `EmpID` | `@EmpID` |
| ddlBrand | `Brand` | `@Brand` |
Empty strings are sent as `""` (not DBNull) in the RX methods; SP semantics of `""` UNKNOWN.

## 9. Dropdown Mapping
See §7 (all loaders in try/catch).

## 10. Button Action Mapping
Search → `btnSearch_Click` → `Validatea()` → `LoadData()`; the whole call is wrapped in `try{}catch(Exception){}` (errors invisible). `rbReportTypeName_SelectedIndexChanged` clears output, shows/hides `divPro` (report 5) and `divBrand`+`divUser` (report 6).

## 11. Frontend Flow
Client export: `exporttocsv()` → `tableToCSV` of the rendered `#export` table; `exportToExcel()` / `exportTableToExcel()` helper functions exist (legacy; `exportToExcel` references undefined `e`).

## 12. Backend Flow
`LoadData()` (`:238-567`): clears `data.InnerHtml`; `aPP = "Select"` if no approval status chosen; collects Zone/Area/Territory, ProviderType/PharmaPlatform/DoctorType **texts**, product id, MIO id, brand; `dtDate = _DAL.GetDynamicDateByDateRange(from,to)` and uses `dtDate.Rows[0]["mainDate"]`; then per selected `rbReportTypeName` item:
1 → `GetRXDoctorWiseDayList` → `DynamicPivotDoctorWiseRX_New`; 2 → `GetRXDoctorBrandWiseList` → `DynamicPivotBrandWiseRX_new`; 3 → `GetRXDoctorProductWiseList` → `DynamicPivotProductdWiseRX_New`; 4 → `GetRXUserWiseList` → `DynamicPivotUserWiseRX_New`; 5 → `GetRXUserProductWiseList` → `DynamicPivotUserandProductdWiseRX_New` (extra `@ProID`); 6 → `GetDynamicRXStatusReport` → `GetDynamicVisitStatusReportRX` (extra `@ProID,@EmpID,@Brand`).
The result DataTable is rendered generically: header = column names, body = `cell.ToString()` **without HTML encoding** (BR-M16-061), bootstrap table classes. No totals added by the page (except what the SP returns). `Validatea()`: `RoleTypeName=="AM"` ⇒ Area required (`faildalert(" Please Select Area!")`); `"DZSM"` ⇒ Zone required (" Please Select Zone!"); other roles unrestricted.

## 13. API Mapping
`[WebMethod] GetMileageClaimList(param)` (leftover; calls HR mileage SP; no caller here).

## 14. Service Mapping
`ProductBLL.LoadProductSQ(ddlBrand)` only; rest page → DAL directly.

## 15. Repository Mapping
`CmnCrystaltoView` (static instance shared by all requests), `SeedDataDAL`, `CommonDataLoad`, `Setup2DAL`.

## 16. Stored Procedure Mapping (report SPs)
| SP | Called via | Parameters | Bodies |
|---|---|---|---|
| `DynamicDateByDateRange` | `GetDynamicDateByDateRange` | `@frmDate,@toDate` | absent — returns row 0 `mainDate` (date list string) |
| `DynamicPivotDoctorWiseRX_New` | `GetRXDoctorWiseDayList` | `@Month,@Year,@ListToPivot,@ColumnToPivot,@ApprovalStatus,@ProviderType,@PharmaPlatform,@DoctorTypeSelect,@ZoneSelect,@AreaSelect,@TeritorySelect` | absent |
| `DynamicPivotBrandWiseRX_new` | `GetRXDoctorBrandWiseList` | same 11 | absent |
| `DynamicPivotProductdWiseRX_New` | `GetRXDoctorProductWiseList` | same 11 (DoctorType/Zone/Area/Territory) | absent |
| `DynamicPivotUserWiseRX_New` | `GetRXUserWiseList` | same 11 | absent |
| `DynamicPivotUserandProductdWiseRX_New` | `GetRXUserProductWiseList` | same + `@ProID` | absent |
| `GetDynamicVisitStatusReportRX` | `GetDynamicRXStatusReport` | same + `@ProID,@EmpID,@Brand` | absent |

## 17. Database Mapping
UNKNOWN (dynamic-SQL pivot SPs). Inline SQL in `tblProductSQ` via `ProductBLL`.

## 18. Validation Matrix
| Rule | Message |
|---|---|
| AM must pick Area | " Please Select Area!" |
| DZSM must pick Zone | " Please Select Zone!" |
| Date range validity | none (free text; `DynamicDateByDateRange` may return no rows ⇒ `Rows[0]` exception swallowed by btnSearch try/catch ⇒ blank page) |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-055 | A report covers a date range: `DynamicDateByDateRange(from,to)` produces the list of days used as pivot columns (`@ListToPivot`, `@ColumnToPivot='DcrDate'`), and the range text is passed in the parameters named `@Month`/`@Year` | `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:838-850,980-1013`; `RXDoctoriseMonthlypt.aspx.cs:335-346` | page + SPs | CONFIRMED (SP semantic INFERRED) |
| BR-M16-056 | With no approval status chosen the literal `'Select'` is sent as `@ApprovalStatus` (meaning "all" by SP convention — UNKNOWN) | `RXDoctoriseMonthlypt.aspx.cs:255-262` | `LoadData` | CONFIRMED |
| BR-M16-057 | AM must choose an Area and DZSM a Zone before searching; other roles (MIO, NSM, admin) unrestricted; no automatic scoping to the user's own territory | `RXDoctoriseMonthlypt.aspx.cs:597-623` | `Validatea` | CONFIRMED |
| BR-M16-058 | DEFECT: the Territory filter is read only when `F_AreaSelect.SelectedValue != ""` (condition tests Area instead of Territory) — a Territory selected without an Area is ignored; same pattern in PAGE-671/673 | `RXDoctoriseMonthlypt.aspx.cs:295-298` | `LoadData` | CONFIRMED |
| BR-M16-059 | Provider type, pharma platform and doctor type are passed as display **text**; in the DCR page some report types pass the **id** instead (inconsistent contract with the same SPs) | `RXDoctoriseMonthlypt.aspx.cs:303-317`; `DcrDoctoriseMonthlypt.aspx.cs:260-283,330-337,377-384` | `LoadData` | CONFIRMED |
| BR-M16-060 | The DCR/RX radio (`rbType`) has no effect on the query (only sets a local variable); which SPs run depends solely on the report-type radio | `RXDoctoriseMonthlypt.aspx.cs:240-248` | `LoadData` | CONFIRMED |
| BR-M16-061 | Result cells are written into HTML without encoding | `RXDoctoriseMonthlypt.aspx.cs:349-373` | string concat | CONFIRMED |

## 20. Permission Matrix
Only the AM/DZSM mandatory-selection rule (BR-M16-057), based on `Session["RoleTypeName"]` (read in try/catch). No menu-independent permission check; no scoping to own territory.

## 21. Status Lifecycle
Not applicable; consumes `ApprovalStatus` (`SoftwareUseId`) to count only chosen RX approval states (SP behaviour UNKNOWN).

## 22. Error Handling
`btnSearch_Click` swallows all exceptions ⇒ a failing SP shows an empty page with no message. Loaders swallow.

## 23. Audit
None.

## 24. Integration
None (Crystal not used). Export is client-side CSV.

## 25. Reports
See `reports/RPT-M16-01-monthly-doctorwise-pivot-reports.md`.

## 26. Dependencies
`tableToCSV` plugin, `IVMarketStructure`, `ProductBLL`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Default range, Doctor Wise | pivot table, columns = dates in month | 055 |
| T2 | AM without Area | " Please Select Area!" | 057 |
| T3 | Select Territory but not Area | territory ignored | 058 |
| T4 | Switch rbType to DCR | no change in results | 060 |
| T5 | Range where `mainDate` empty | blank result (swallowed exception) | §18 |
| T6 | Doctor name containing `<b>` | rendered as HTML (no encoding) | 061 |
| T7 | Report 5 with product chosen | `@ProID` sent | 055 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Pivot contents | None existed | rendered generically | SP absent | UNKNOWN | GAP-M16-038 |

## 30. Known Gaps
GAP-M16-037 (all pivot SP bodies absent), GAP-M16-038 (unknown metrics/approval semantics), GAP-M16-039 (territory filter bug), GAP-M16-040 (text/id inconsistency), GAP-M16-041 (unencoded HTML), GAP-M16-042 (swallowed errors; four near-duplicate report pages), GAP-M16-049 (static shared `CmnCrystaltoView`).

## 31. Change Impact
`CmnCrystaltoView` DAL methods are shared with PAGE-671/672/673 and other modules' report pages. `DynamicDateByDateRange` is used by all four monthly pages. Changing `DynamicPivot*` SP output columns changes the rendered table only (generic renderer), except DCR page's `RemoveHiddenColumns` (drops columns named "Provider Type", "Pharma Platform", "Region", "Territory").
