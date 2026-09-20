# WF-M18-01 — Generic role-chain approval-log engine (shared by all M18 pages)

**Scope.** The reusable mechanism behind every `Approval_UI` list page except the transfer page: (1) administrators configure a role chain, (2) a list SP returns each subject's latest approval-log row, (3) the page decides who may act, (4) an action appends the next step through a per-domain "save app-log" SP. Confidence per step is tagged. SP bodies are absent except `sp_Get_CustomerApp` (read side only); the rest is `UNKNOWN`.

> Two separate "approval engines" exist in the repository. This document is the one used by M18. The other (`tblAction*` per-user action permission, `ClsApprovalAction`) is documented in **WF-M18-05**; it is NOT used by any M18 page.

## 1. Trigger & actors
| Actor | Role |
|---|---|
| Submitter | field employee (mobile/web) creating a plan, leave, expense, order, customer, doctor, etc. (submission code lives in other modules; `IsFromApp` in the doctor SP suggests mobile app; INFERRED) |
| Approver | user whose `Session["RoleTypeId"]` equals the row's `ToRoleTypeId`, or a gate-exempt role (BR-M18-003), or hard-coded employee 496 on some pages (BR-M18-004) |
| Administrator | configures the chain on `UserPermission/ApprovalStepMap.aspx` (PAGE-026, module M02) |

## 2. Preconditions
- The approver's menu entry exists for the page (`sp_GET_MainPermissionByUserRoleandPageUrl` non-empty unless `UserRoleID == 2`) — BR-M18-011.
- A chain (from-role → ordered to-roles) has been saved for the page's menu (config table names UNKNOWN; DAO `ApprovalMapMaster{ApprovalMapMasterId,MenuId,MenuName,FromRoleId}` + `ApprovalMapDetail{ApprovalMapDetailId,ApprovalMapMasterId,ToRoleId,Order}` — `Library.DAO/UserRoleDAO/ApprovalMapMaster.cs:9-26`).

## 3. Steps
| # | Step | Where (page → handler → BLL → DAL → SP) | Conf. |
|---|---|---|---|
| 1 | Admin picks menu + from-role, adds ordered to-roles, saves | `ApprovalStepMap.aspx` JS `FinalSave()` → `[WebMethod] SaveMenu` (`ApprovalStepMap.aspx.cs:21`) → `ApprovalMapDAL.SaveApprovalMapMaster` → `sp_Save_ApprovalMapMaster(@MenuId,@FromRoleId,@MenuName)` (returns PK via `SaveDataReturnPrimaryKey`); then for each detail `SaveApprovalMapDetail` → `sp_Save_ApprovalMapDetail(@ApprovalMapMasterId,@Order,@ToRoleId)` (`ApprovalMapDAL.cs:99-156`). No BLL. | CONFIRMED |
| 2 | Admin can reload an existing map | `LoadApprovalMap(roleId,typeId)` → `sp_GET_ApprovalMapLoad(@FromRoleId,@MenuId)` | CONFIRMED (SP body UNKNOWN) |
| 3 | Subject submitted → first log row created with first `ToRoleTypeId` | outside M18 (SP/API) | UNKNOWN |
| 4 | Approver opens list page | `Page_Load` → `UserPersmissionValidation` → `LoadData()` (per page) | CONFIRMED |
| 5 | Page loads acting user's market row | `CommonDataLoad.GetEmpMarketStructure_Active(EmpInfoId)` → `sp_Get_EmployeeFieldForceInfo_EmpId(@id)`; row 0 → hidden Group/Region/Area/Territory ids (Tour, DCP, Expense, Order only) | CONFIRMED |
| 6 | Page scopes by role | `RoleTypeName` AM/DZSM/NSM(/DIC on Order) → `@Role` + `@param` SQL fragment (BR-M18-007, BR-M18-008) | CONFIRMED |
| 7 | List SP returns latest log per subject | `Get*_Applog*` DAL → `sp_Get_*App*(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId)`; verified body only for `sp_Get_CustomerApp`: `LogMax = MAX(Step) GROUP BY TableId`, join log on `TableId & Step=MaxStep`; returns `Step, MaxStep, RoleTypeId, ToRoleTypeId, FromEmpId, ToEmpId, Status, WaitingForRole, ApprovalStatusWeb, <domain id>ApprovalId` | CONFIRMED (customer) / INFERRED (others from grid `DataKeyNames`) |
| 8 | Page hides actions for non-owners | gate loop after bind: if `hfToRoleTypeId != Session RoleTypeId` and role not exempt → `lbApprove/lbReject/lbEdit.Visible=false`, `lbMsg="Waiting for Another Approver"` (BR-M18-002/003) | CONFIRMED |
| 9 | Approver clicks approve/reject (row or bulk) | `RowCommand`/`btnBulk*_Click` builds DTO: `Step = hfStep + 1`, `Status`, `Type`, `MenuId`, hierarchy ids, `FromEmpId=EmpInfoId`, `ToEmpId=hfToEmpId`, audit dates/user (BR-M18-001/009) | CONFIRMED |
| 10 | DAL writes the step | `Save*_Applog*DAL` → `DataAccessManager.SqlConnectionOpen(SalesDB)` → `SaveData` / `SaveDataReturnPrimaryKey` (implicit `BeginTransaction` :414/:436) → `SqlConnectionClose()` commit; exception → `SqlConnectionClose(true)` rollback + rethrow (`CustomerInfoDAL.cs:1054-1066`) | CONFIRMED |
| 11 | SP appends log row / advances state | `sp_webapi_Save*AppLog`, `sp_web_Save*AppLog`, `sp_SaveLeaveAppLog` — computes next `ToRoleTypeId` from the chain, updates subject header status | UNKNOWN |
| 12 | Page reports result and reloads | `ShowSuccesalert`/`faildalert` JS + `LoadData()` | CONFIRMED |

### Save-SP contract (same for every domain SP; DAL `CustomerInfoDAL.cs:881-1511`, `DoctorDAL.cs:321`)
`@<Domain>ApprovalId, @Date, @FromEmpId, @ToEmpId, @TableId, @Status, @Comments, @Type, @Step, @GroupId, @RegionId, @AreaId, @TerritoryId, @ToGroupId, @ToRegionId, @ToAreaId, @ToTerritoryId, @EntryByS, @EntryDateS, @EntryTimeS, @ApproveByS, @ApproveDateS, @ApproveTimeS, @EntryByApp, @EntryDateApp, @EntryTimeApp, @ApproveByApp, @ApproveDateApp, @ApproveTimeApp, @MenuId` (30 params). Pages set only Id, Date, FromEmpId, ToEmpId, TableId, Status, Type, Step, Group..Territory, EntryByS, EntryDateS, ApproveDateS, EntryByApp, MenuId; the rest go as NULL. `<Domain>ApprovalId` names: `OrderApprovalId`, `TourPlanApprovalId`, `VisitPlanApprovalId`, `TADAApprovalId`, `ExpanseApprovalId`, `MileageApprovalId`, `DCRApprovalId`, `PrescriptionApprovalId`, `LeaveApprovalId`, `DoctorApprovalId`, `CustomerApprovalId`.

### Domain map (page → DAL → save SP → list SP → MenuId → Type sent)
| Page | Save DAL | Save SP | List SP | MenuId | Type (approve / reject) |
|---|---|---|---|---|---|
| CustomerApproveList | SaveCustomer_ApplogDAL (via `DeleteAction`) | sp_webapi_SaveCustomerAppLog | sp_Get_CustomerApp | 302 | Customer / Customer |
| DoctorApprovalList | DoctorDAL.SaveDoctor_ApplogDAL | sp_webapi_SaveDoctorAppLog | sp_Get_DoctorClaimApp | 303 | Doctor / Doctor |
| ExpenseApprovalList | SaveExpenseClaim_ApplogDAL (pk) | sp_web_SaveExpanseAppLog | sp_Get_ExpanseClaimApp | 356 | ExpanseClaim / Expense |
| TourPlanApproval | SaveTourPlan_ApplogDAL | sp_webapi_SaveTourPlanAppLog | sp_Get_TourPlanApp | 371 | Leave / Leave |
| MillageApprovalList (Tier B) | SaveMileageClaim_Applog_DAL | sp_web_SaveMileageAppLog | sp_Get_MileageAppData | 372 | Millege / Millege |
| DAApprovalList (Tier B) | SaveTADAClaim_ApplogDAL | sp_web_SaveTADAAppLog | sp_Get_TADAAppData | 376 | DA Claim / DA Claim |
| DCPCVPApproval | SaveVisitTourPlan_ApplogDAL | sp_webapi_SaveVisitPlanAppLog | sp_Get_DCPCVPTourPlanApp | 377 | DCP / Leave |
| RXApprovalList (Tier B) | SaveRX_Applog_DAL | sp_webapi_SavePrescriptionAppLog | sp_Get_PrescriptionApp | 379 | Prescription / Prescription |
| LeaveApproveList | SaveLeave_ApplogDAL (pk) | sp_SaveLeaveAppLog | sp_Get_Leave_AppLog | 381 | Leave / Order |
| OrderApproveList | SaveOrder_ApplogDAL | sp_webapi_SaveOrderAppLog | sp_Webapi_Get_OrderApp | 381 | Order / Order |
| DCRApprovalList (Tier B) | SaveDCR_Applog_DAL (also used by `DoctorVisit_UI/DCRList.aspx.cs:118`) | sp_webapi_SaveDCRAppLog | sp_Get_DCRApp | 382 | DCR / DCR |
| (Attendance, not in M18) | SaveAttandaceAppLog | sp_webapi_SaveAppLog | sp_Get_AttendanceInformation | — | — |
(all rows CONFIRMED from page/DAL source; Tier-B rows from grep excerpts of the page code-behind, less thoroughly read.)

## 4. Data created / changed
- One approval-log row per action (per-domain log table; verified name only `tblCustomerApprovalLog`, columns per `sp_Get_CustomerApp`); subject header status (`tblCustMaster.ActionStatus`: 0 Pending, 1 Verified, 2 Approved, 3 Rejected — verified for customers only). Everything else UNKNOWN.

## 5. Branches / exceptions
- Row's `ToRoleTypeId` ≠ user role → no actions (UI only).
- Exempt roles 5/4/14 (+9, +3 per page) act on any row (BR-M18-003).
- Hard-coded employee 496 (Leave, Expense, DA, Millage) approves as "Accepted" (BR-M18-004).
- Save exception → rollback, rethrown (page unhandled) except Customer page (swallowed, false success, BR-M18-019).
- `SaveData` never returns false → "Already Exist!" branch unreachable except pk-based pages (Expense, Leave) (BR-M18-013).

## 6. Postconditions
Log holds a new step; list reload shows the new status/step and "Waiting For" role (list-SP dependent).

## 7. Related BR / pages / gaps
BR-M18-001..014, 056..061; PAGE-695, 697, 699, 701, 702, 704, 706; GAP-M18-001, 002, 003, 004, 005, 007, 009, 010, 019.

## 8. Tests
| ID | Scenario | Expected |
|---|---|---|
| WF01-T1 | Save chain Area-Manager → Regional → Head for menu X | master + 2 details saved (three SP calls; no shared transaction) |
| WF01-T2 | Step 2 approver (not ToRole) opens list | row shows "Waiting for Another Approver" |
| WF01-T3 | Correct approver approves | SP called with `Step+1`, `MenuId` of the page, Status per BR |
| WF01-T4 | Crafted POST with another row index | executes with that row's hidden values (no re-check) — negative security test |
| WF01-T5 | Two approvers approve same row concurrently | both SP calls succeed with same `Step+1` (no concurrency token in page) — verify SP guard |
