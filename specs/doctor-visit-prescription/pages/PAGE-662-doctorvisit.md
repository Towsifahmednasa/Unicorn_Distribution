---
page_id: PAGE-662
module: M16
page: DoctorVisit
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-662 — DoctorVisit

> **Evidence level:** Tier A — `DoctorVisit.aspx` (page title "Doctor Call Plan List", ~640 lines, JS-driven), `DoctorVisit.aspx.cs` (one WebMethod), `DoctorVisitDAL.GetDoctorVisitList`. `sp_Get_DoctorTourPlanMasteList` body absent. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-662 |
| Module | M16 |
| Page | DoctorVisit — "Doctor Call Plan List" (monthly plan headers, DCP) |
| Folder | DoctorVisit_UI |
| Kind | Read-only list / search with Excel export |

## 2. Purpose
Search and view monthly doctor call plans (one row per employee/month: month, year, remarks at final submit, approval status) and drill into a plan's detail (`DoctorPlanDetailsView.aspx?id=<DocTPMaster>`). CONFIRMED.

## 3. Business Objective
Give managers visibility of who submitted a doctor call plan for the month and its approval state. INFERRED.

## 4. Navigation
View button → `../DoctorVisit_UI/DoctorPlanDetailsView.aspx?id=<DocTPMaster>` (`editClick`, `:628`). Inbound: `DoctorPlanDetailsView` back link; menu `UNKNOWN`.

## 5. Route
`~/DoctorVisit_UI/DoctorVisit.aspx`.

## 6. Source Files
`Solution.Web/DoctorVisit_UI/DoctorVisit.aspx`, `DoctorVisit.aspx.cs` (`GetDoctorVisitList`), `Library.DAL/DoctorVisit_DAL/DoctorVisitDAL.cs:114-132`, `Solution.Web/DoctorModule_UI/Setup.aspx(.cs)` (`Get_UserRoleInfo`, `Get_CapturedBy_For_ddl`), `CustomScript/_QuickDataAccess.js` (`_getApprovalList_Active`, `_getEmployeeList_Active`), XLSX library from cdnjs (`xlsx.full.min.js 0.17.3`).

## 7. UI Control Inventory
| Control | Label | Default | Data source |
|---|---|---|---|
| FiscalYearSelect | Year | current year (set on ready) | static options |
| MonthSelect | Month | current month | static options |
| EmployeeIdSelect | Employee | none | `_getEmployeeList_Active` |
| UserRoleSelect | User Role | none | `Setup.aspx/Get_UserRoleInfo` → `sp_Get_UserRoleInfo` |
| FromDate / ToDate | From/To Date | empty | datepicker |
| ApprovalStatusSelect | Approval Status | none | `_getApprovalList_Active` (`SoftwareUseId`/`WebShow`) |
| GroupSelect/ZoneSelect/AreaSelect/TeritorySelect/MarketSelect | hierarchy | none | client cascade (`_QuickDataAccess.js`) |
| btnSave (labelled search; `onclick=GetDegree()`) | Search | – | – |
| exportBtn | Export | – | client-side `XLSX.utils.table_to_sheet` → file `Doctor Call Plan List.xlsx` |
| Table `#dtTb` | columns: (view) SL, Employee ID, Employee Name, Designation, User Role, Month, Year, Remarks, Approval Status, Actions | – | JSON `EmpMasterCode, EmpName, DesigName, RoleName, MonthName1, YearValue, FinalSubmitRemarks, ApprovalStatus, DocTPMaster` |

## 8. Field Mapping
Filters → client-built SQL fragment `param` → WebMethod → `@param` of `sp_Get_DoctorTourPlanMasteList` (see §12). No writes.

## 9. Dropdown Mapping
See §7. `ResetClick()` uses Razor `@Url.Action("DoctorVisit","DoctorVisit")` (dead; Reset does not work).

## 10. Button Action Mapping
Search → `GetDegree()` (name is a leftover; it loads the plan list); View → `editClick(DocTPMaster)`; Export → XLSX of the rendered table.

## 11. Frontend Flow
On ready: set current month/year, load role/employee/approval-status dropdowns, call `GetDegree()`, which builds `param` and POSTs `DoctorVisit.aspx/GetDoctorVisitList` as JSON `{param}`; renders rows, initialises DataTables (`bPaginate:false`). `param` clauses (`DoctorVisit.aspx:454-535`): `AND mas.YearValue='<y>'`, `AND mas.MonthValue='<m>'`, `AND CONVERT(date,tpdtl.CreatedDate) BETWEEN '<from>' AND '<to>'` (only From ⇒ to = today `yyyy/MM/dd`), `AND mas.ApprovalStatus='<status>'`, `AND tpdtl.CreatedBy='<user>'`, `AND us.UserRoleID='<role>'`, `AND mas.EmpInfoId='<emp>'`, `AND gp.GroupId=…`, `AND zn.ZoneId=…`, `AND ar.AreaId=…`, `AND tr.TerritoryId=…`, `AND mr.MarketId=…`.

## 12. Backend Flow
`[WebMethod] GetDoctorVisitList(string param)` → `_DoctorVisit_DAL.GetDoctorVisitList(param)` → `GetDataTable("sp_Get_DoctorTourPlanMasteList", @param)` → `JsonConvert.SerializeObject(dt)`. The static `DoctorVisitDAL` instance (with its instance `DataAccessManager`) is shared by all requests. No role scoping (unlike PAGE-658); no try/catch.

## 13. API Mapping
`DoctorVisit.aspx/GetDoctorVisitList` (POST JSON). Also referenced by PAGE-663 (broken). No auth check in code.

## 14. Service Mapping
None (page → DAL).

## 15. Repository Mapping
`DoctorVisitDAL.GetDoctorVisitList`.

## 16. Stored Procedure Mapping
`sp_Get_DoctorTourPlanMasteList` (body absent; aliases seen from filters: `mas` = plan master, `tpdtl` = plan detail, `us` = user, `gp/zn/ar/tr/mr` = group/zone/area/territory/market).

## 17. Database Mapping
Reads doctor tour-plan master and detail tables (names UNKNOWN), user and market-hierarchy tables.

## 18. Validation Matrix
None (all filters optional, no format checks; values concatenated into SQL fragment in the browser).

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-033 | List defaults to the current calendar month/year of DCP headers; filters year, month, created-date range, approval status, creator, role, employee, hierarchy; the filter text is assembled in the browser and passed to the SP as raw SQL | `Solution.Web/DoctorVisit_UI/DoctorVisit.aspx:401-408,454-540`; `DoctorVisit.aspx.cs:19-25` | JS + WebMethod | CONFIRMED |

## 20. Permission Matrix
No server-side permission or role scoping; any logged-in user (or anyone able to POST the WebMethod with a session cookie) can query all plans. Session not read at all.

## 21. Status Lifecycle
Displays `ApprovalStatus` of the plan master (`mas.ApprovalStatus`; textual/numeric mapping via `sp_GET_ActionStatusList`, UNKNOWN). The approve action for plans is not functional on PAGE-663 (BR-M16-034); working approve exists on `DoctorModule_UI/VisitPlannedApprovalList` (other module).

## 22. Error Handling
AJAX has no error handler (spinner `#coverScreen` hidden in `complete`). WebMethod exceptions ⇒ HTTP 500.

## 23. Audit
None (read-only).

## 24. Integration
XLSX library via CDN (external script dependency); Google not used.

## 25. Reports
Export to Excel (client-side). Related report: `DoctorVisitReport` (PAGE-664) uses the same SP for detail and `DynamicPivotDoctorWiseDoctorVisitPlan` for the pivot.

## 26. Dependencies
DataTables, select2, `_QuickDataAccess.js`, cdnjs xlsx.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Open page | rows for current month/year | 033 |
| T2 | Filter status | `mas.ApprovalStatus=<id>` applied | 033 |
| T3 | Put `'` in a filter value via crafted POST | SQL error / injection surface | GAP-M16-034 |
| T4 | Click Reset | no effect (Razor URL) | §9 |
| T5 | Export | XLSX of current table | 033 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Page title vs class | – | "Doctor Call Plan List" (DCP) though class/file named DoctorVisit | – | naming only | §1 |

## 30. Known Gaps
GAP-M16-034 (raw SQL fragment from client), GAP-M16-035 (no role scoping/permission), GAP-M16-036 (dead Razor Reset link, misnamed `GetDegree`/`GetPrescription` functions copy-pasted).

## 31. Change Impact
`sp_Get_DoctorTourPlanMasteList` shared with DoctorVisitReport (PAGE-664). `DoctorVisitDAL` shared with 9 M16 pages. Column names (`DocTPMaster`, `MonthName1`, `YearValue`, `FinalSubmitRemarks`) are consumed by PAGE-663 too.
