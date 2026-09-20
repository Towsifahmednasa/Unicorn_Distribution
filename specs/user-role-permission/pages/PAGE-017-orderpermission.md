---
page_id: PAGE-017
module: M02
page: OrderPermission
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-017 — OrderPermission ("Order Place Permission")

> **Evidence level:** Tier A for code-behind, DAL and markup; the inline JS at the end of the aspx (lines ≈360-525, a copy of report-page helpers) was skimmed only (frontend PARTIAL). SPs `sp_Get_ZoneListOrdPer`, `sp_Get_AreaListOrdPer`, `sp_GET_EmployeeInfoOrdPermission`, `sp_Get_AMDZSMListByTerritoryId`, `sp_Save_OrderPermission` are **not in the repository**. No code in the repository *reads* the saved permission (grep for `OrderPermission`/`PermittedEmpId`: only this page and `Setup2DAL.Save_OrderPermission`).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-017 |
| Module | M02 |
| Kind | Entry grid (search → edit rows → save) |
| Folder | `Solution.Web/Reports_UI` (name of folder is misleading; the page is not a report) |
| Master | `~/MasterPages/NewMasterPage.master` |
| Title | "Order Place Permission" |
| Class | `Reports_UI_OrderPermission` |
| Size | aspx 525 lines, code-behind 478 |

## 2. Purpose
For territories in a chosen zone/area, list the territory's current employee and let an administrator nominate another **permitted employee** (AM/DZSM-level candidates for that territory) who may place orders for the territory during a From–To date-time window.

## 3. Business Objective
Delegation of order placement: allow a substitute (e.g. an area manager) to place orders on behalf of a territory's regular MIO for a limited time (leave/vacancy cover). INFERRED from title, column names ("New Permitted User", From/To Date) and DAL names.

## 4. Navigation
No inbound link in code; DB menu (UNKNOWN). Row-command `EditData` (never rendered — no such button in the grid) would redirect to `../DoctorModule_UI/MileageClaim.aspx?id=`; the hidden Print/Excel buttons open `../SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx?...` (copy-paste remnants from an expense report page).

## 5. Route
`~/Reports_UI/OrderPermission.aspx` (also a static WebMethod `GetMileageClaimList`, unused).

## 6. Source Files
`OrderPermission.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/Setup2DAL.cs` (`Save_OrderPermission`, `GetZoneListOrdPer`, `GetAreaListOrdPer`), `Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs` (`GetEmployeeInfoOrdPermission`, `GetAMDZSMListByTerritoryId`).

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| `ddlZone` (autopostback) | Zone | no | `sp_Get_ZoneListOrdPer` (`RegionId`/`RegionName`); change loads `ddlArea` via `sp_Get_AreaListOrdPer(@RegionId)` |
| `ddlArea` | Area | no | – |
| Hidden row (`display:none`): `ddlmonth`, `ddlYear`, `EmployeeIdSelect`, `UserRoleSelect`, `ddlEmployeeStatus` | Month/Employee/Year/Role/Status | – | never populated (code commented out) and not used in the query |
| `btnSearch` | Search | – | `btnSearch_Click` → `LoadData()` |
| `resetBtn` | Reset | – | reload page |
| `btnPrint`, `btnExportToExcel` | (hidden via `style="display:none"`) | – | remnants |
| `loadGridView` | grid | – | columns: SL (with hidden `hfEmpInfoId`, `hfTerritoryId`, `hfPermittedEmpId`), header/row checkbox (`chkSelectAll`/`chkSelect`), Area, Territory, Employee ID (`EmpMasterCode`), Employee Name (`EmpName`), **New Permitted User** (`ddlPermittedEmpId`), From Date (`txtFrmDate`, `datetime-local`), To Date (`txtToDate`, `datetime-local`) |
| `btnSave` | Submit | – | `OnClientClick=sweetAlertConfirm_Submit` → `btnSave_Click` |

## 8. Field Mapping
| UI (per ticked row) | Variable | DAL | SP param | Column (UNKNOWN) |
|---|---|---|---|---|
| `hfTerritoryId` | `TerritoryId` | `Setup2DAL.Save_OrderPermission` | `@TerritoryId` | order-permission table |
| `ddlPermittedEmpId.SelectedValue` | `PermittedEmpId` | same | `@PermittedEmpId` | – |
| `txtFrmDate` | `DateTime.Parse` → `FrmDate` | same | `@FrmDate` | – |
| `txtToDate` | `ToDate` | same | `@ToDate` | – |
| `Session["LoginName"]` | `sessionUser` | same | `@EntryBy` | – |

## 9. Dropdown Mapping
Zone/Area as above; per-row `ddlPermittedEmpId` ← `EmployeeInformationDaL.GetAMDZSMListByTerritoryId(hfTerritoryId)` → `sp_Get_AMDZSMListByTerritoryId(@TerritoryId)` (`EmpInfoId`/`EmpName`), first item "Please Select From List".

## 10. Button Action Mapping
Search → `LoadData()`; header checkbox → `chkSelectAll_CheckedChanged` (tick all); Submit → `btnSave_Click`.

## 11. Frontend Flow
UpdatePanel with progress overlay; select2 styling (`pageLoad`); SweetAlert confirm on Submit; datetime-local pickers.

## 12. Backend Flow
`LoadData()`: `param()` builds a **SQL fragment string** `" AND ar.RegionId='<zone>'"` and `" AND ar.AreaId='<area>'"` (when selected) → `GetEmployeeInfoOrdPermission(param)` → `sp_GET_EmployeeInfoOrdPermission(@Parm)` (the SP presumably appends the fragment to dynamic SQL — UNKNOWN). For each grid row it loads the candidate list, and when `hfPermittedEmpId` is non-empty/non-"0" it ticks the row and selects that employee (failures swallowed). Search is **not** run on first load (`LoadData()` commented out in `Page_Load`).
`btnSave_Click`: `Validation2()`; then for every ticked row: parse dates, `Save_OrderPermission(territory, permittedEmp, from, to, LoginName)`; `Res` keeps only the **last row's** result; if `Res.isSuccess` → `ShowSuccesalert('Operation successful!','Success')` and reload the grid; else `faildalert('Already Exist!','Faild')`.
`Save_OrderPermission` (`Setup2DAL.cs:340-389`): `pk = SaveDataReturnPrimaryKey("sp_Save_OrderPermission", ...)`; `pk>0` → success, else `isValiCheck=true` (shown as "Already Exist!"); exception → rethrown after closing.

## 13. API Mapping
`[WebMethod] GetMileageClaimList(string param)` — unused leftover; it passes an arbitrary client string to `_setupDAL.GetMileageClaimList` with no session check.

## 14. Service Mapping
_Sections 14–15 are documented together in this block._
No BLL. DAL as listed in §6.



## 15. Repository Mapping
See §14 block above (combined §14–15).

## 16. Stored Procedure Mapping
`sp_Get_ZoneListOrdPer`, `sp_Get_AreaListOrdPer(@RegionId)`, `sp_GET_EmployeeInfoOrdPermission(@Parm)`, `sp_Get_AMDZSMListByTerritoryId(@TerritoryId)`, `sp_Save_OrderPermission(@TerritoryId,@PermittedEmpId,@FrmDate,@ToDate,@EntryBy)` — all bodies UNKNOWN.

## 17. Database Mapping
UNKNOWN (tables behind the SPs; territory/area/region/employee masters).

## 18. Validation Matrix
| Rule | FE | BE | Message | BR |
|---|---|---|---|---|
| At least one row ticked | no | yes (`Validation2`) | `Please Select at least one employee!` | BR-M02-019 |
| For ticked rows: permitted employee, From, To present | no | **only colours the row `#f0a8a8`; `Validation2` still returns `true`** | none | BR-M02-020 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-017 | Search filters territories by optional zone (`RegionId`) and area (`AreaId`); no other filter is active. | `OrderPermission.aspx.cs:296-349` | CONFIRMED |
| BR-M02-018 | Candidate substitutes for a territory come from `sp_Get_AMDZSMListByTerritoryId` (AM/DZSM-type employees of that territory, per name); pre-existing permission (non-zero `PermittedEmpId`) is pre-selected and the row ticked. | `:124-158,521-…` | CONFIRMED (flow) / UNKNOWN (SP rule) |
| BR-M02-019 | Saving requires at least one ticked row; every ticked row is saved individually. | `:191-216,436-478` | CONFIRMED |
| BR-M02-020 | Missing permitted employee / dates on ticked rows are only highlighted, not blocked; an empty date would be sent as a null `SqlParameter` (omitted) and an empty employee as `""`. Effect depends on the SP (UNKNOWN). | `:218-251,455-459` | CONFIRMED (code) |
| BR-M02-021 | `@EntryBy` is the session **LoginName** (text), unlike other M02 pages that pass UserId. | `:459` | CONFIRMED |
| BR-M02-022 | The overall success message reflects only the last saved row; earlier failures are hidden. | `:441-474` | CONFIRMED |
| BR-M02-023 | The stored permission is not read by any code in the repository (order entry pages do not check it), so its enforcement (if any) lives in stored procedures/mobile services outside this repo. | grep | CONFIRMED (absence in repo) / UNKNOWN (enforcement) |

## 20. Permission Matrix
Master-page first-GET login check; `Session["LoginName"]` read on save (NRE → unhandled exception if the session is lost). No role check, no `UserPersmissionValidation`; any logged-in user can delegate order placement for any territory. Static `GetMileageClaimList` WebMethod has no auth.

## 21. Status Lifecycle
Time window `FrmDate`–`ToDate` (datetime) defines validity; no status column known.

## 22. Error Handling
Loaders `try{}catch{}` swallow; DAL rethrows; nothing logged.

## 23. Audit
`@EntryBy` = LoginName; other audit columns UNKNOWN.

## 24. Integration
None visible. Possibly consumed by mobile/order services (UNKNOWN).

## 25. Reports
The page name lives in `Reports_UI` and contains copy-pasted print/Excel plumbing (`PopUp` → `EmployeeExpenseReportViewer.aspx?rptType=<empIds>&Month=&Year=&fType=`), but the triggering buttons are hidden. No report is produced by this page in practice.

## 26. Dependencies
`SeedDataDAL`, `Setup2DAL`, `SetupDAL`, `CmnCrystaltoView`, `EmployeeInformationDaL`, `CommonDataLoad` (instantiated, unused), Newtonsoft.Json.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search zone only | rows for zone territories | 017 |
| TC-02 | Row with existing permission | pre-ticked with employee selected | 018 |
| TC-03 | Submit with no ticked rows | popup `Please Select at least one employee!` | 019 |
| TC-04 | Tick row, leave employee blank | row red, save still submitted | 020 |
| TC-05 | 3 rows ticked, 2nd fails | overall message from 3rd row | 022 |
| TC-06 | Save | `EntryBy` = login name string | 021 |
| TC-07 | Check order-entry with permitted employee | not verifiable in this repo | 023 |

## 28. Reverse Engineering References
`Solution.Web/Reports_UI/OrderPermission.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/Setup2DAL.cs:340-460`, `Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:411,521`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Row validation | – | highlight only | GAP-M02-018 |
| Enforcement of permission | – | not found in repo | GAP-M02-019 |

## 30. Known Gaps
GAP-M02-001, 005, 018 (non-blocking validation, last-row result), 019 (no consumer in repo), 020 (SQL fragment passed to SP), 021 (dead controls/handlers).

## 31. Change Impact
`Setup2DAL` and `EmployeeInformationDaL` are large shared classes; `sp_Save_OrderPermission` used only here (grep). Territory/area/zone list SPs `sp_Get_*OrdPer*` used only by this page and `GetAreaListOrdPerALL`/`GetTerritoryListOrdPerALL` (no page callers found).
