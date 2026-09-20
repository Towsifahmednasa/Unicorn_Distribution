---
page_id: PAGE-608
module: M15
page: LeaveApplicationCode
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-608 — LeaveApplicationCode

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-608 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | LeaveApplicationCode |
| Kind | Page (INFERRED from name) |
| Web folder | LeaveProcess_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | LeaveProcess_UI_LeaveApplicationCode |
| aspx size | 387 bytes |
| Code-behind LOC | 69 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: `LeaveApplicationCode.aspx` has no UI (page body is empty, 7-line aspx); it hosts the **5 page-methods behind the leave application form** `LeaveApplicationEntry.aspx` (PAGE-609) and the list `LeaveApplications.aspx` (PAGE-610): `SaveLeaveApplication`, `UpdateLeaveApplication` (both call the same DAL save), `LeaveApplicationApprove`, `GetEmployeeLeaveBalance`, `GetApplicationInfoById`.

## 3. Business Objective
CONFIRMED by code/JS: let HR or an employee look up an employee's leave balance by employee code, submit a leave application for a leave type and period with a reason, and approve/reject it.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/LeaveProcess_UI/LeaveApplicationCode.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx |
| Code-behind | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | LeaveApplicationDal (Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
No server/HTML controls with an ID were extracted (viewer-only or code-generated page).

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow (page 609 JS, `LeaveApplicationEntry.aspx:220-420`)
`SearchByEmployeeCode()` on change of the code box -> `GetEmployeeLeaveBalance` -> fills employee name, `empInfoId`, leave-type list (value `LeaveBalanceId`, text `LeaveTypeName`) and a balance table (`YearlyLeaveBalance` and `LeaveBalance`). `SaveLeaveApplication()` -> `FormValidation()` -> POST `LeaveApplicationCode.aspx/SaveLeaveApplication` with `{LeaveBalanceId, EmpId, Days, LeaveFromDate, StartDate, EndDate, Reason}` where **`StartDate` and `EndDate` are both taken from `#leaveToDate`**; success -> `successalert(...,'LeaveApplications.aspx')`.

## 12. Backend Flow
| WebMethod | DAL -> SP | Notes |
|---|---|---|
| `SaveLeaveApplication` / `UpdateLeaveApplication` | `LeaveApplicationDal.SaveLeaveApplication` -> if `LeaveApplicationId>0` `IsEditable(id)` (`sp_Webapi_Get_LeaveApproveById(@id)` returns rows -> not editable -> `ErrorMessage="The leave has already been approved. Can't edit right now"`, `isSuccess=false`); else `sp_Webapi_Save_LeaveInfo(@leaveAppId,@typeId,@startDate,@endDate,@reason,@empId)` | `Days` and `LeaveFromDate` are not passed; exceptions swallowed in `IsEditable` (treated as editable) |
| `LeaveApplicationApprove(id, ApprovalStatus)` | `ApproveLeaveApplication` -> `sp_Webapi_Update_LeaveData(@id,@approvalType,@empId = Session["UserId"])` | no role check; parameter named empId receives the **UserId** |
| `GetEmployeeLeaveBalance(employeeCode)` | `GetEmployeeLeaveBalanceByCode` -> `sp_GET_EmployeeLeaveBalance(@Parameter=" AND EGI.EmpMasterCode = '<code>'")` | string-concatenated filter |
| `GetApplicationInfoById(id)` | `sp_GET_LeaveApplicationInfoById(@Parameter=" AND LVAP.LeaveApplicationId = '<id>'")` | |
Each DAL call has its own connection/transaction; static DAL instance `_leaveDal`.

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| LeaveProcess_UI_LeaveApplicationCode.SaveLeaveApplication | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs:25 | LeaveApplication leaveApplication | ~/LeaveProcess_UI/LeaveApplicationCode.aspx/SaveLeaveApplication | via BLL |
| LeaveProcess_UI_LeaveApplicationCode.UpdateLeaveApplication | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs:30 | LeaveApplication leaveApplication | ~/LeaveProcess_UI/LeaveApplicationCode.aspx/UpdateLeaveApplication | via BLL |
| LeaveProcess_UI_LeaveApplicationCode.LeaveApplicationApprove | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs:36 | int leaveApplicationId, string ApprovalStatus | ~/LeaveProcess_UI/LeaveApplicationCode.aspx/LeaveApplicationApprove | via BLL |
| LeaveProcess_UI_LeaveApplicationCode.GetEmployeeLeaveBalance | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs:42 | string employeeCode | ~/LeaveProcess_UI/LeaveApplicationCode.aspx/GetEmployeeLeaveBalance | via BLL |
| LeaveProcess_UI_LeaveApplicationCode.GetApplicationInfoById | Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs:52 | int leaveApplicationId | ~/LeaveProcess_UI/LeaveApplicationCode.aspx/GetApplicationInfoById | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| LeaveApplicationDal.SaveLeaveApplication | Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs:106 | sp_Webapi_Save_LeaveInfo | @empId, @endDate, @leaveAppId, @reason, @startDate, @typeId |
| LeaveApplicationDal.IsEditable | Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs:74 | sp_Webapi_Get_LeaveApproveById | @id |
| LeaveApplicationDal.ApproveLeaveApplication | Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs:166 | sp_Webapi_Update_LeaveData | @approvalType, @empId, @id |
| LeaveApplicationDal.GetEmployeeLeaveBalanceByCode | Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs:17 | sp_GET_EmployeeLeaveBalance | @Parameter |
| LeaveApplicationDal.GetApplicationInfoById | Library.DAL/MasterSetup_DAL/LeaveApplicationDal.cs:47 | sp_GET_LeaveApplicationInfoById | @Parameter |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_EmployeeLeaveBalance | LeaveApplicationDal.GetEmployeeLeaveBalanceByCode | body NOT in repository | read-like name (INFERRED) |
| sp_GET_LeaveApplicationInfoById | LeaveApplicationDal.GetApplicationInfoById | body NOT in repository | read-like name (INFERRED) |
| sp_Webapi_Get_LeaveApproveById | LeaveApplicationDal.IsEditable | body NOT in repository | write-like name |
| sp_Webapi_Save_LeaveInfo | LeaveApplicationDal.SaveLeaveApplication | body NOT in repository | write-like name |
| sp_Webapi_Update_LeaveData | LeaveApplicationDal.ApproveLeaveApplication | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| ddlLeaveType, employeeCode, dayQuantity, leaveFromDate, leaveToDate, leaveRemarks | non-empty | client `FormValidation()` only | `Please fill out of this field!` tooltips | BR-M15-058 |
| leave type value `0` ("Select from list") | not blocked by JS (checks `""` only) | client | - | BR-M15-058 |
| server | **no validation** (dates, days, balance, overlap) | DAL | - | BR-M15-060 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-058 | Application requires leave type, employee, day quantity, from date, to date and reason - enforced in the browser only. | `LeaveApplicationEntry.aspx:230-300` | page JS | CONFIRMED |
| BR-M15-059 | An application that already has an approval record (`sp_Webapi_Get_LeaveApproveById` returns rows) cannot be edited. | `LeaveApplicationDal.cs` `IsEditable`/`SaveLeaveApplication` | DAL | CONFIRMED |
| BR-M15-060 | The saved period is `StartDate = EndDate = To date` (JS maps `StartDate` from `leaveToDate`); the From date and typed `Days` are not sent to `sp_Webapi_Save_LeaveInfo`. Any derived day count is therefore SP-side (UNKNOWN). | `LeaveApplicationEntry.aspx:306-309`; `LeaveApplicationDal.cs` | JS + DAL | CONFIRMED |
| BR-M15-061 | Balance is shown per leave type as yearly entitlement (`YearlyLeaveBalance`) and remaining (`LeaveBalance`) from `sp_GET_EmployeeLeaveBalance`; the drop-down value is `LeaveBalanceId` (per-employee balance row), not the leave-type id. No check prevents applying beyond balance. | `LeaveApplicationEntry.aspx:365-405` | JS + SP | CONFIRMED (display), UNKNOWN (deduction) |
| BR-M15-062 | `LeaveApplicationApprove` performs approval/rejection for any caller with a session (status string free text passed to `@approvalType`); no role/step check, no approval-log row. | `LeaveApplicationCode.aspx.cs`; `LeaveApplicationDal.cs` | web-method + DAL | CONFIRMED |
| BR-M15-076 | Leave can also be approved through `Approval_UI/LeaveApproveList.aspx` (approval-log path). The two paths are independent. | see WF-M15-02 | cross-module | CONFIRMED |

## 20. Permission Matrix
Session: `UserId` used only in `ApproveLeaveApplication` (`HttpContext.Current.Session["UserId"]`). No role check on any method; reads work without session.

## 21. Status Lifecycle
Application `ApprovalStatus` labels in UI code: `Pending`, `Approved`, else Rejected (legacy JS); the filter drop-down uses `sp_GET_ActionStatusList` (values UNKNOWN). Transitions: create -> Pending (SP, INFERRED) -> Approved/Rejected via `sp_Webapi_Update_LeaveData` or the approval-log path; edit allowed only before any approval row.

## 22. Error Handling
DAL: rollback + rethrow (page-method returns HTTP 500 -> JS `Operation Faild!`); `IsEditable` swallows errors; JS success handler in approve ignores result.

## 23. Audit
Approver id (`@empId` = UserId) to `sp_Webapi_Update_LeaveData`; application entry audit UNKNOWN.

## 24. Integration
sp_Webapi_Get_LeaveApproveById, sp_Webapi_Save_LeaveInfo, sp_Webapi_Update_LeaveData

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: LeaveApplicationDal; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-608-01 | Enter valid code | Balance table and type list appear | BR-M15-061 |
| TC-608-02 | Leave a required field blank | Tooltips, no POST | BR-M15-058 |
| TC-608-03 | Submit 3-day leave 10-12 Oct | Stored period per SP; expect From=10 (actual: To date used as start) | BR-M15-060 |
| TC-608-04 | Edit approved application | `The leave has already been approved. Can't edit right now` | BR-M15-059 |
| TC-608-05 | POST `LeaveApplicationApprove` as any logged-in user | Accepted (no role check) | BR-M15-062 |
| TC-608-06 | Apply for more days than balance (edge) | Not blocked in web code | BR-M15-061 |

## 28. Reverse Engineering References
- `Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx`, `Solution.Web/LeaveProcess_UI/LeaveApplicationCode.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | LeaveApplicationCode.aspx(.cs) | n/a | MISSING | - |
| From date | None existed | not sent | UNKNOWN | ACTUAL-DEFECT | GAP-M15-015 |
| Approval authorisation | None existed | none | UNKNOWN | ACTUAL-GAP | GAP-M15-009 |

## 30. Known Gaps
- **GAP-M15-015**, **GAP-M15-009**, **GAP-M15-012** (string-concatenated `@Parameter` filters), GAP-M15-003 (static DAL), commented-out `GetLeaveApplications`.

## 31. Change Impact
`sp_Webapi_*` leave SPs are shared with the mobile app leave feature; `LeaveApplicationDal` used by pages 608/610.

