---
page_id: PAGE-706
module: M18
page: TourPlanApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-706 — TourPlanApproval

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs`, `CustomerInfoDAL.GetTourPlan_ApplogDAL` (:480) and `SaveTourPlan_ApplogDAL` (:944) read. SP bodies (`sp_Get_TourPlanApp`, `sp_webapi_SaveTourPlanAppLog`) are NOT in the repository; effects `UNKNOWN`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-706 / M18 — Approval Workflow |
| Page name | TourPlanApproval |
| Kind | Approval list (single-row approve/reject only) |
| Web folder / master | Approval_UI / ~/MasterPages/NewMasterPage.master |
| Page class | Approval_UI_TourPlanApproval (`Solution.Web/Approval_UI/TourPlanApproval.aspx.cs`, 467 LOC) |
| Suspected duplicate | Functional twin of DCPCVPApproval (PAGE-697) minus bulk buttons; pair with `DoctorModule_UI/TourPlanDetailsView.aspx` and `DoctorModule_UI/TourPlannedApprovalList` (PAGE-592, other module) |

## 2. Purpose
An approver reviews each employee's monthly tour plan (visit/working plan) and approves ("Verified") or rejects it. INFERRED (name, SP `sp_Get_TourPlanApp`, `MenuId=371`, columns Year/Month/FinalSubmitRemarks).

## 3. Business Objective
Ensure a monthly tour plan is checked by the responsible manager(s) in the configured approval chain before execution; expose who the plan is currently waiting for. INFERRED.

## 4. Navigation
Menu DB-driven (`UNKNOWN`). Outbound: `../DoctorModule_UI/TourPlanDetailsView.aspx?id={TableId}` in a new window (`.cs:208`); hidden `DoctorEntry.aspx` (:323); Reset → `TourPlanApproval.aspx` (:461). Inbound: `DCPCVPApproval.aspx.cs:590` (Reset link of another page).

## 5. Route
`~/Approval_UI/TourPlanApproval.aspx`.

## 6. Source Files
View/code-behind as above. DAL: CustomerInfoDAL (`GetTourPlan_ApplogDAL`, `SaveTourPlan_ApplogDAL`), CommonDataLoad, SeedDataDAL, Setup2DAL. DAO: `OrderSaveApprovalLogDAO`. Core: `Library.DAL/DataManager/DataAccessManager.cs`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| ddlDistributionCenter | Distribution Center | no | blank / DIC preselect+disabled (:50-54) | inside `visible=false` row (aspx:59) — never rendered |
| ddlmonth, ddlYear | Month, Year | no | current month / year (`GetMonthList` :125, `GetYearList` :110) | year 2015-2050; month list = 12 months from now |
| EmployeeIdSelect, UserRoleSelect, ApprovalStatusSelect | Employee, User Role, Approval Status | no | blank | loaded from `sp_CS_GetEmpGeneralInfo_All`, `sp_Get_UserRoleInfo`, `sp_GET_ActionStatusList` (silent catch :162,:178,:195) |
| btnSearch / resetBtn | Search / Reset | — | — | `LoadData` / redirect |
| loadGridView | (no heading) | — | — | keys `TableId,TourPlanApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep`; columns Employee ID, Employee Name, Designation, User Role, Year, Month, Remarks (FinalSubmitRemarks), Approval Status (ApprovalStatusWeb), Waiting For (WaitingForRole), Actions |
| lbEdit(eye)/lbApprove/lbReject, lbMsg | row actions | — | — | hidden when `hfToRoleTypeId != Session RoleTypeId` unless role in {5,4,14} (:417-448); text "Waiting for Another Approver" |
| hf* row + page hidden fields | — | — | — | same as PAGE-697 (`hfCustomerApprovalId`=`TourPlanApprovalId`, `hfCustomerMasterId`=`TableId`) |
No bulk buttons (no `btnBulk*` in aspx or code-behind — CONFIRMED by grep).

## 8. Field Mapping
Identical to PAGE-697 §8 except: DAL `SaveTourPlan_ApplogDAL` (:944), SP `sp_webapi_SaveTourPlanAppLog`, first id parameter `@TourPlanApprovalId`, `MenuId = 371` (:245,:293), `Type = "Leave"` for both approve (:238) and reject (:286), Status "Verified" (:215) / "Rejected" (:264). Table/column: UNKNOWN.

## 9. Dropdown Mapping
As §7. No dependent/cascading dropdowns.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB change |
|---|---|---|---|---|
| Search | `btnSearch_Click` :463 → `LoadData` | `GetTourPlan_ApplogDAL` | `sp_Get_TourPlanApp(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId)` | read |
| Approve | `RowCommand ApproveData` :213 | `SaveTourPlan_ApplogDAL` | `sp_webapi_SaveTourPlanAppLog` | approval-log step (UNKNOWN) |
| Reject | `RowCommand RejectData` :262 | same | same | same |
| View | `EditData` :202 | — | — | none |
| Reset | :459 | — | — | none |

## 11. Frontend Flow
Same pageLoad plugin init (pickadate/select2); no bulk JS; postbacks only.

## 12. Backend Flow
`Page_Load` :28-77 (any exception → redirect to `../Dashboard_UI/DashboardOne.aspx`, unlike PAGE-697). `LoadData` :326-451 identical algorithm to PAGE-697: market lookup, `RoleTypeName` switch (AM uses `View_Webapi_EmployeeFieldForceInfo_Top1.EmpAreaId`; DZSM `...EmpRegionId`; NSM `...EmpGroupId`), filter concatenation (year/month/status/role/employee, :374-405), SP call, gate loop with exempt roles {5,4,14} (role 9 is NOT exempt here, differs from PAGE-697). Save path: `SaveTourPlan_ApplogDAL` → `SaveData` (tx begin, commit on close, rollback+rethrow on exception; page has no catch around it).

## 13. API Mapping
None.

## 14. Service Mapping
No BLL (direct DAL).

## 15. Repository Mapping
See PAGE-697 §15; list DAL `CustomerInfoDAL.GetTourPlan_ApplogDAL` :480 → `sp_Get_TourPlanApp`; save DAL `SaveTourPlan_ApplogDAL` :944 → `sp_webapi_SaveTourPlanAppLog`.

## 16. Stored Procedure Mapping
`sp_Get_TourPlanApp` (read, body absent), `sp_webapi_SaveTourPlanAppLog` (write, body absent), plus the common lookups (bodies absent). GAP-M18-001.

## 17. Database Mapping
Tables UNKNOWN. Filter text references `mas.YearValue`, `mas.MonthValue`, `mas.ApprovalStatus`, `mas.EmpInfoId`, `us.UserRoleID`, `View_Webapi_EmployeeFieldForceInfo[_Top1]` (`.cs:348-403`).

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Approve/Reject | none server-side beyond hidden-field parsing (`""`→0) | — |
| Failure | `!Res.isSuccess` → alert "Already Exist!" (dead branch, BR-M18-013) | "Already Exist!" |
| Success | "Operation  Approved successful!" (approve, double space verbatim) / "Operation disapproved successful!" (reject) | verbatim |
| Filters | concatenated SQL (GAP-M18-007) | — |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-001 | Step written = row Step + 1 | TourPlanApproval.aspx.cs:236-237,:284-285 | CONFIRMED |
| BR-M18-002 | Actions visible only when `ToRoleTypeId == Session RoleTypeId`, else "Waiting for Another Approver" | :417-448 | CONFIRMED |
| BR-M18-003 | Exempt roles here: 5, 4, 14 | :417 | CONFIRMED |
| BR-M18-005 | MenuId 371 hard-coded | :245,:293 | CONFIRMED |
| BR-M18-006 | Actor market hierarchy loaded and passed | :334-342 | CONFIRMED |
| BR-M18-007 | Scope filter AM/DZSM/NSM | :344-372 | CONFIRMED |
| BR-M18-009 | Audit fields (now, UserId, EmpInfoId) | :234-244 | CONFIRMED |
| BR-M18-011 | Menu-permission check unless UserRoleID==2 | :79-109 | CONFIRMED |
| BR-M18-012 | DIC preselect | :50-54 | CONFIRMED |
| BR-M18-013 | Save "success" = no exception | DataAccessManager.cs:447 | CONFIRMED |
| BR-M18-014 | Status strings "Verified"/"Rejected" (never "Accepted": final approval status for tour plans is not written by this page) | :215,:264 | CONFIRMED |
| BR-M18-044 | `Type="Leave"` on both approve and reject of a tour plan (copy-paste of Leave) | :238,:286 | CONFIRMED |
| BR-M18-045 | Default filters current year and current month | :121,:137 | CONFIRMED |
| BR-M18-046 | No bulk approval on this page | aspx/cs | CONFIRMED |

## 20. Permission Matrix
Same as PAGE-697: menu-permission row check (non-role-2) + UI gating; no server re-check. `Page_Load` redirects to Dashboard on missing Session/any exception (better than PAGE-697).

## 21. Status Lifecycle
[Pending, Step n] → approve (ToRole user) → "Verified", Step n+1 (INFERRED) ; → reject → "Rejected", Step n+1. Final "Accepted"/"Approved" state never written from this page (role 5 users approve with "Verified" too, contrast PAGE-702/701). Header status column UNKNOWN.

## 22. Error Handling
Loader catches silent; Page_Load redirect; save unhandled.

## 23. Audit
Same as BR-M18-009; no comments; times null.

## 24. Integration
`sp_webapi_*` SP naming shared with API layer (INFERRED). Detail view page `TourPlanDetailsView.aspx` in DoctorModule_UI.

## 25. Reports
None.

## 26. Dependencies
CustomerInfoDAL, CommonDataLoad, SeedDataDAL, Setup2DAL, OrderSaveApprovalLogDAO, ResultInfo.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Approver role == ToRoleTypeId approves | `sp_webapi_SaveTourPlanAppLog` with Status "Verified", Step+1, MenuId 371, Type "Leave" | 001,005,014,044 |
| TC-02 | Role 9 user, not ToRole | Actions hidden (role 9 not exempt here) | 003 |
| TC-03 | Role 5 user | actions on all rows | 003 |
| TC-04 | Reject | Status "Rejected"; message "Operation disapproved successful!" | 014 |
| TC-05 | Default load | filters = current month/year | 045 |
| TC-06 | Session expired | redirect to Dashboard/Login path | 011 |
| TC-07 | Check for bulk buttons | absent | 046 |

## 28. Reverse Engineering References
`Solution.Web/Approval_UI/TourPlanApproval.aspx(.cs)`, `CustomerInfoDAL.cs:480,944`, WF-M18-02.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | see §12 | MISSING |
| Type discriminator | None existed | "Leave" | CODE SMELL (GAP-M18-004) |
| Final approval state | None existed | never "Accepted" from this page | UNKNOWN (GAP-M18-026) |

## 30. Known Gaps
GAP-M18-001, 003, 004, 005, 006, 007, 008 (Rows[0] in role switch), 010 (hard-coded role/menu ids), 013, 026.

## 31. Change Impact
`SaveTourPlan_ApplogDAL` / `GetTourPlan_ApplogDAL` used only by this page (grep verified). Twin page PAGE-697 has its own DAL pair. `MenuId 371` presumably ties to the approval map row for Tour Plan (INFERRED).
