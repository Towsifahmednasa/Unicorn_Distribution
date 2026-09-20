---
page_id: PAGE-673
module: M16
page: DcrDoctoriseMonthlypt
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-673 — DcrDoctoriseMonthlypt

> **Evidence level:** Tier A — `DcrDoctoriseMonthlypt.aspx(.cs)` (787 lines, read: initial load, `LoadData`, `Validatea`, `RemoveHiddenColumns`), `CmnCrystaltoView.cs` methods for DCR (params read), `Reports_UI/IVMarketStructureMarket.ascx.cs` (role prefill). SP bodies absent. Reviewed 2026-09-19. Shared logic with PAGE-675 is cross-referenced rather than repeated.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-673 |
| Module | M16 |
| Page | DcrDoctoriseMonthlypt — DCR (doctor call report / visits) doctor-wise date-range pivot |
| Folder | Reports_UI |
| Output | generated HTML table in `data.InnerHtml`; client CSV export (`exporttocsv()`), server export button `Visible=false` |

## 2. Purpose
Count doctor visits (DCR) per day for a date range, per doctor, per product brand, or per user (MIO). Also has (unreachable) Product-wise, DCP-wise and "Dynamic Visit Status" branches. CONFIRMED.

## 3. Business Objective
Coverage/frequency monitoring of doctor calls by approval status, brand, doctor type and MIO. INFERRED.

## 4. Navigation
Menu `UNKNOWN`. Reset → `DcrDoctoriseMonthlypt.aspx`. (Note: `CVRDoctoriseMonthlypt`'s Reset also points here, `CVRDoctoriseMonthlypt.aspx.cs:633`.)

## 5. Route
`~/Reports_UI/DcrDoctoriseMonthlypt.aspx`.

## 6. Source Files
`Solution.Web/Reports_UI/DcrDoctoriseMonthlypt.aspx(.cs)`, `Reports_UI/IVMarketStructureMarket.ascx(.cs)`, `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs` (`GetDCRDoctorWiseDayList` :1015, `GetDoctorBrandWiseList` :1601, `GetDoctorProductWiseList` :1240, `GetDCRUserWiseList` :1274, `GetDynamicVisitStatusReport` :1423 (12-param overload) and :1090 (11-param), `GetDynamicDateByDateRange` :1697).

## 7. UI Control Inventory
| Control | Label | Visible? | Default / source |
|---|---|---|---|
| fromDateTextBox / toDateTextBox | From Date / TO Date | yes | first / last day of current month |
| ddlApprovalStatus | Approval Status | yes | `sp_GET_ActionStatusList` |
| ddlBrand | Brand Filter | yes | `ProductBLL.LoadProductSQ` |
| DoctorTypeSelect | Doctor Type | yes | `sp_Get_DoctorType_Active` |
| ddlMIO | User | yes | `GetEmployeeList_Active` |
| rbReportTypeName | Report Type: `1` Doctor Wise (default), `2` Product Brand Wise, `4` User Wise | yes | UI offers only 1/2/4 |
| rbType (DCR/RX), ddlProgramType, ddlPharmaPlatform, IVMarketStructure (Zone/Area/Territory), EmployeeIdSelect | – | **in `<div style="display:none">`** (aspx:105-115; comment "Hidden elements to preserve control state/postback functionality"); `EmployeeIdSelect` `Visible=false` | rendered but not user-changeable; hierarchy control pre-selects values from the user's role (see BR-M16-062) |
| btnSearch / resetBtn | – | yes | – |

## 8. Field Mapping
Same parameter contract as PAGE-675 §8, for these SPs (DCR params: `@Month=from`, `@Year=to`, `@ListToPivot=mainDate`, `@ColumnToPivot='DcrDate'`, `@ApprovalStatus`, `@ProviderType`, `@PharmaPlatform`, `@ZoneSelect`, `@AreaSelect`, `@TeritorySelect`; DCR user/brand/product methods pass `ProviderType ?? DBNull`, `PharmaPlatform ?? DBNull`). Doctor Wise passes provider/platform as **text** (`SelectedItem.Text`), Brand/User views as **id** (`SelectedValue`) — BR-M16-059. The Visit-status branch sends `@EmpID`, `@DCType`(=doctor type **id**, `DoctorTypeSelect.SelectedValue`) and `@Brand`.

## 9. Dropdown Mapping
See §7.

## 10. Button Action Mapping
Search → `Validatea()` then `LoadData()` (try/catch swallow) — same as PAGE-675.

## 11. Frontend Flow
`rbReportTypeName` change clears output. `exporttocsv()` client CSV of `#export`. No other JS.

## 12. Backend Flow
`LoadData()` (`:206-540`): builds `aPP` ("Select" default), `Zone/Area/Territory` (Territory read only if Area chosen — BR-M16-058), `ProviderType` text, `PharmaPlatform` text, `EmpID`, `DCType`, `Brand`; `dtDate = DynamicDateByDateRange(from,to)`; branches by `rbReportTypeName.SelectedValue`:
- `"1"` → `GetDCRDoctorWiseDayList` → `DynamicPivotDoctorWiseDCR_New_ForSearch`; result passed through `RemoveHiddenColumns` (drops columns named "Provider Type", "Pharma Platform", "Region", "Territory" if present) and rendered as HTML table.
- `"2"` → provider/platform re-read as **ids**; `GetDoctorBrandWiseList` → `DynamicPivotBrandWiseDCR`.
- `"3"` → `GetDoctorProductWiseList` → `DynamicPivotProductdWiseDCR` (**no such option in the UI** ⇒ unreachable).
- `"4"` → ids; `GetDCRUserWiseList` → `DynamicPivotUserWiseDCR`.
- `"5"` → `new DataTable()` (the DCP call is commented out) ⇒ empty table (unreachable).
- `"6"` → `GetDynamicVisitStatusReport` (12-param overload) → `DynamicVisitStatusReport` (unreachable from UI).
`Validatea()` identical to PAGE-675 (AM ⇒ Area; DZSM ⇒ Zone). Rendering identical (no HTML encoding).

## 13. API Mapping
None specific (leftover `GetMileageClaimList`-style WebMethod not present here; grep shows none).

## 14. Service Mapping
`ProductBLL.LoadProductSQ`; otherwise DAL directly.

## 15. Repository Mapping
`CmnCrystaltoView` (static shared instance).

## 16. Stored Procedure Mapping
| SP | DAL method | Params | Body |
|---|---|---|---|
| `DynamicDateByDateRange` | `GetDynamicDateByDateRange` | `@frmDate,@toDate` | absent |
| `DynamicPivotDoctorWiseDCR_New_ForSearch` | `GetDCRDoctorWiseDayList` | `@Month,@Year,@ListToPivot,@ColumnToPivot,@ApprovalStatus,@ProviderType,@PharmaPlatform,@ZoneSelect,@AreaSelect,@TeritorySelect` | absent |
| `DynamicPivotBrandWiseDCR` | `GetDoctorBrandWiseList` | same 10 (Provider/Platform nullable) | absent |
| `DynamicPivotProductdWiseDCR` | `GetDoctorProductWiseList` | same 10 | absent |
| `DynamicPivotUserWiseDCR` | `GetDCRUserWiseList` | same 10 | absent |
| `DynamicVisitStatusReport` | `GetDynamicVisitStatusReport` | 10 + `@EmpID,@DCType` (+`@DoctorTypeSelect`,`@Brand` in the 12-param overload) | absent |
Dead DAL: `GetDCRDoctorWiseDayListNew` (`CmnCrystaltoView.cs:941`) despite its name calls `sp_GetRXDoctorWiseRpt` (copy-paste), and has no callers.

## 17. Database Mapping
UNKNOWN (dynamic pivot SPs). By naming and elsewhere in the module: `tbl_DCRInfo` (`DcrDate` pivot column, `ApprovalStatus`, `SmcTypeId_Dcr`, doctor, chamber).

## 18. Validation Matrix
As PAGE-675 §18 (AM Area / DZSM Zone). Because the hierarchy control is hidden and pre-filled by role (`IVMarketStructureMarket.ascx.cs:100-145`: MIO ⇒ group/zone/area/territory prefilled and group/zone/area disabled; AM ⇒ group/zone prefilled, area selectable; DZSM ⇒ group/zone prefilled; NSM ⇒ group prefilled) the AM/DZSM "please select" checks depend on that prefill working.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-062 | The UI offers only Doctor Wise / Product Brand Wise / User Wise; branches for report types 3, 5 and 6 exist in code but cannot be selected; Provider Type, Pharma Platform and the market hierarchy are hidden (`display:none`) so users cannot change them — the hierarchy value comes from the role-based prefill of `IVMarketStructureMarket.ascx` | `Solution.Web/Reports_UI/DcrDoctoriseMonthlypt.aspx:105-125`; `DcrDoctoriseMonthlypt.aspx.cs:292-520`; `IVMarketStructureMarket.ascx.cs:100-145` | markup + code | CONFIRMED (prefill effect INFERRED) |
| BR-M16-055 / 056 / 057 / 058 / 059 / 060 / 061 | apply here as in PAGE-675 (date-range pivot; 'Select' approval default; AM/DZSM mandatory area/zone; territory read only if Area chosen; text-vs-id inconsistency; DCR/RX radio inert; unencoded HTML) | `DcrDoctoriseMonthlypt.aspx.cs:206-300,329-384,588-611` | – | CONFIRMED |
| BR-M16-063 | The DCR pivot removes any result columns titled "Provider Type", "Pharma Platform", "Region", "Territory" before display | `DcrDoctoriseMonthlypt.aspx.cs:545-558` | `RemoveHiddenColumns` | CONFIRMED |
| BR-M16-064 | `GetDCRDoctorWiseDayListNew` calls the RX SP `sp_GetRXDoctorWiseRpt` (copy-paste error) but is unused | `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:941-975` | DAL | CONFIRMED |

## 20. Permission Matrix
Same as PAGE-675: only the AM/DZSM mandatory-selection rule plus role-based prefill of the hidden hierarchy control. No permission-table check.

## 21. Status Lifecycle
Not applicable; approval status is a filter (`@ApprovalStatus`).

## 22. Error Handling
`btnSearch_Click` swallows exceptions.

## 23. Audit
None.

## 24. Integration
None (client CSV only).

## 25. Reports
See `reports/RPT-M16-01-monthly-doctorwise-pivot-reports.md`.

## 26. Dependencies
`IVMarketStructureMarket.ascx`, `ProductBLL`, `tableToCSV`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Doctor Wise, default month | table with day columns; hidden columns removed | 055,063 |
| T2 | Product Brand Wise with brand chosen | brand pivot; provider/platform sent as ids | 059 |
| T3 | User Wise | user pivot | 055 |
| T4 | Force POST with `rbReportTypeName=6` | dynamic visit status branch runs (not selectable in UI) | 062 |
| T5 | AM user opens page | hierarchy prefilled; Area still required | 057 |
| T6 | Territory chosen w/o area | ignored | 058 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| DCR pivot semantics | None existed | generic renderer | SP absent | UNKNOWN | GAP-M16-037/038 |

## 30. Known Gaps
GAP-M16-037, -038, -039, -040, -041, -042 (see PAGE-675), GAP-M16-050 (unreachable report branches/dead code and copy-paste DAL method), GAP-M16-051 (CVR Reset redirects to the DCR page).

## 31. Change Impact
Shares `CmnCrystaltoView`, `DynamicDateByDateRange`, `DynamicPivotBrandWiseDCR`, `DynamicPivotProductdWiseDCR`, `DynamicPivotUserWiseDCR`, `DynamicVisitStatusReport` with PAGE-671 (CVR). `IVMarketStructureMarket.ascx` shared with PAGE-671/672/675.
