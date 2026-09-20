---
page_id: PAGE-702
module: M18
page: LeaveApproveList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-702 — LeaveApproveList

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs` (627 LOC), `CustomerInfoDAL.GetLeave_AppLogDAL` (:752) and `SaveLeave_ApplogDAL` (:1467) read. SP bodies `sp_Get_Leave_AppLog`, `sp_SaveLeaveAppLog` are NOT in the repository → effects `UNKNOWN`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-702 / M18 |
| Page name | LeaveApproveList (heading "Leave List") |
| Kind | Approval list with bulk actions |
| Folder / master | Approval_UI / NewMasterPage.master |
| Class | Approval_UI_LeaveApproveList |
| Related | Leave application entry/view pages in hr-field-force module (PAGE-608..610, PAGE-577..580) — not documented here |

## 2. Purpose
Managers review employee leave applications (type, days, from-to, return date, attachment) and verify/accept or reject them. CONFIRMED from columns and DAL name; business meaning INFERRED.

## 3. Business Objective
Move a leave application through the configured approval chain; a final-level approver (role 5 or 9, or the special employee 496) marks it "Accepted", earlier approvers mark it "Verified". INFERRED from `.cs:195-206`.

## 4. Navigation
DB menu. Outbound: eye button would open `../MasterSetup_UI/CustomerEntry.aspx?MID={TableId}` (`.cs:186` — wrong target copy-pasted from Customer page) but `lbEdit` has `Visible="false"` in markup (aspx:273) and is only re-hidden in code, so it never shows. `DoctorEntry.aspx` hidden button (:454). Reset → `LeaveApproveList.aspx` (:625).

## 5. Route
`~/Approval_UI/LeaveApproveList.aspx`.

## 6. Source Files
View/code-behind; CustomerInfoDAL; CommonDataLoad; SeedDataDAL (`GetLeaveTypeList` :888, `GetApprovalStatusList`, `GetDistributionCenterDataTableList`); Setup2DAL; DAO `LeaveApplo_Save`, `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Visibility/notes |
|---|---|---|---|---|
| FromDate / ToDate | date pickers | no | FromDate = today-7, ToDate = today, format `dd MMMM, yyyy` (:39-40) | — |
| ddlDistributionCenter | Distribution Center | no | blank / DIC preselect | inside hidden row (aspx:67) |
| EmployeeIdSelect | Employee | no | blank | `sp_CS_GetEmpGeneralInfo_All` |
| UserRoleSelect | User Role | no | blank | `sp_Get_UserRoleInfo` |
| ApprovalStatusSelect | Approval Status | no | blank | `sp_GET_ActionStatusList` |
| ddlLeaveType | Leave Type | no | blank | `GetLeaveTypeList` (value `LeaveBalanceId`, text `LeaveTypeName`) — NOTE the filter later compares this to `B.LeaveTypeId` (:547): a balance id is used as a type id (GAP-M18-016) |
| btnSearch / resetBtn | Search / Reset | — | — | — |
| btnBulkApprove / btnBulkReject | Approve Selected / Reject Selected | — | — | confirm(`... selected leave applicati...`) |
| loadGridView | Leave List | — | — | keys `TableId,LeaveApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep`; columns Employee ID(EmpMasterCode), Employee Name, User Role, Days, Leave From-To Date (LeaveFromDate), Leave Type, Date Of Returns To Duty, attachment thumbnail (`ImageString`), Approval Status (ApprovalStatusWeb), Actions |
| lbApprove | check | — | — | `Enabled = ApprovalStatusWeb != "Approved"` (aspx:276) |
| lbReject | x | — | — | `Enabled = ApprovalStatusWeb != "Rejected"` (aspx:280) |
| lbEdit | eye | — | — | `Visible=false` |
No "Waiting For" column on this page (unlike PAGE-697/701/706).

## 8. Field Mapping
Object `LeaveApplo_Save` → `SaveLeave_ApplogDAL` (:1467). DAL lines 1477-1509 add `@LeaveApprovalId,@Date,@FromEmpId,@ToEmpId,@TableId,@Status,@Comments,@Type,@Step,@GroupId,@RegionId,@AreaId,@TerritoryId,@ToGroupId..@ToTerritoryId,@EntryByS,@EntryDateS,@EntryTimeS,@ApproveByS,@ApproveDateS,@ApproveTimeS,@EntryByApp,@EntryDateApp,@EntryTimeApp,@ApproveByApp,@ApproveDateApp,@ApproveTimeApp,@MenuId` (30 parameters). Then `SaveDataReturnPrimaryKey("sp_SaveLeaveAppLog")` (:1512) and `isSuccess = (pk > 0)`.
| UI | DTO | Value | SP param |
|---|---|---|---|
| hfCustomerApprovalId (LeaveApprovalId) | LeaveApprovalId | :216 | `@LeaveApprovalId` |
| hfCustomerMasterId (TableId) | TableId | :218 | `@TableId` |
| hidden hierarchy fields | GroupId/RegionId/AreaId/TerritoryId | always 0 (fields never filled: market lookup is commented out :463-505) | `@GroupId..@TerritoryId` |
| Session EmpInfoId / hfToEmpId | FromEmpId / ToEmpId | :225-226 | `@FromEmpId,@ToEmpId` |
| hfStep+1 | Step | :227-228 | `@Step` |
| computed | Status "Accepted"/"Verified"/"Rejected" | :195-206,:255 | `@Status` |
| literal | Type "Leave" (approve :229, bulk approve :344) / "Order" (reject :277, bulk reject :408) | — | `@Type` |
| literal | MenuId 381 | :236,:284,:351,:415 | `@MenuId` |
| now / Session UserId | Date, EntryDateS, ApproveDateS / EntryByS, EntryByApp | :231-235 | as named |
Table.column: UNKNOWN.

## 9. Dropdown Mapping
See §7. `ddlLeaveType` value/filter mismatch noted.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB |
|---|---|---|---|---|
| Search | `btnSearch_Click` :619 | `GetLeave_AppLogDAL` | `sp_Get_Leave_AppLog(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId)` | read |
| Approve | `RowCommand ApproveData` :191 | `SaveLeave_ApplogDAL` | `sp_SaveLeaveAppLog` | approval-log step (UNKNOWN) |
| Reject | `RowCommand RejectData` :253 | same | same | same |
| Approve/Reject Selected | :301 / :377 | same | same | per-row call |

## 11. Frontend Flow
`SelectAllRows` (aspx:304) toggles DataTable rows; DataTables init + UpdatePanel `endRequest` re-init (aspx:310-348). Confirm dialogs on bulk buttons.

## 12. Backend Flow
`Page_Load` :28-70: Session read; first load → `LoadInitialInfo` (employee, role, status, leave-type dropdowns; each in silent `try/catch`), default dates, `UserPersmissionValidation`, DC dropdown, `LoadData`. Any exception → redirect Dashboard (:68).
`LoadData` :457-609: market lookup and role switch fully commented out → `Role=""`, no area/region/group scoping (contrast PAGE-697/701/706) — every approver sees all leave rows the SP returns (SP may scope by `@Role`; UNKNOWN). Date filter: both dates present → `AND CONVERT(date,Employee_LeaveApplications.EntryDate) BETWEEN '<From>' AND '<To>'`; only From → BETWEEN From and today (`DateTime.Now.ToString("dd-MMM-yyyy")`); note the date text is a `dd MMMM, yyyy` string sent straight to SQL (culture dependent). Status → `Employee_LeaveApplications.ApprovalStatus='<v>'`; role → `us.UserRoleID`; employee → `Employee_LeaveApplications.EmployeeId`; leave type → `B.LeaveTypeId`. Gate loop: roles {5,4,14,3,9} see everything; else if `EmpInfoId == "496"` only `lbEdit` hidden (approve/reject visible on all rows); else the standard "Waiting for Another Approver" gate (:558-606).
Approve: status decision `ToRoleTypeId is 5 or 9` → "Accepted"; else `EmpInfoId == "496"` → "Accepted"; else "Verified". Reject: "Rejected". Save path `SaveDataReturnPrimaryKey`: tx begun implicitly, `ExecuteScalar` first column returned as pk; `pk>0` = success; commit on `SqlConnectionClose`, rollback+rethrow on exception (no page catch).

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`CustomerInfoDAL.GetLeave_AppLogDAL` :752 → `sp_Get_Leave_AppLog` (commented alternative `sp_Webapi_Get_Leave_AppLog`); `SaveLeave_ApplogDAL` :1467 → `sp_SaveLeaveAppLog` (commented alternative `sp_webapi_SaveLeaveAppLog` at :1511); lookups as PAGE-697 plus `SeedDataDAL.GetLeaveTypeList` :888.

## 16. Stored Procedure Mapping
`sp_Get_Leave_AppLog` (read), `sp_SaveLeaveAppLog` (write, must SELECT an identity/positive scalar for success), lookups. Bodies absent (GAP-M18-001). The parameter list of the save SP is (by DAL) identical in shape to the other approval-log SPs.

## 17. Database Mapping
Tables UNKNOWN. Names in filter text: `Employee_LeaveApplications` (EntryDate, ApprovalStatus, EmployeeId), alias `B` (LeaveTypeId), `us` (UserRoleID). CONFIRMED as text.

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Bulk | ticked + visible required | "No records were approved. Please select records and ensure you have permission to approve them." / "...rejected...reject them." |
| Save fail | `pk<=0` → "Already Exist!" (reachable: SP returning 0/NULL scalar, e.g. duplicate step) | "Already Exist!" |
| Success (single) | "Operation successful!" | — |
| Success (bulk) | "{n} record(s) approved/rejected successfully!" + " {m} failed." | — |
| Dates | none (free text into SQL) | — |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-001 | Step = row Step + 1 | LeaveApproveList.aspx.cs:227-228 | CONFIRMED |
| BR-M18-002 | Standard waiting-for-approver gate | :581-606 | CONFIRMED |
| BR-M18-003 | Gate-exempt role ids: 5,4,14,3,9 | :558 | CONFIRMED |
| BR-M18-004 | EmpInfoId `496` is a hard-coded special approver: approves as "Accepted" and sees approve/reject on all rows (edit hidden) | :199,:307,:563-580 | CONFIRMED |
| BR-M18-005 | MenuId 381 hard-coded | :236 | CONFIRMED |
| BR-M18-009 | Audit fields | :231-235 | CONFIRMED |
| BR-M18-010 | Bulk semantics with success/failure counts | :319-374 | CONFIRMED |
| BR-M18-011 | Menu-permission check unless UserRoleID==2 | :146-176 | CONFIRMED |
| BR-M18-013 | Success = returned scalar > 0 (this page) | CustomerInfoDAL.cs:1512-1520 | CONFIRMED |
| BR-M18-014 | Vocabulary Accepted/Verified/Rejected | :195-206,:255 | CONFIRMED |
| BR-M18-030 | Approve status "Accepted" iff RoleTypeId ∈ {5,9} or EmpInfoId=496, else "Verified"; bulk approve identical | :195-206,:303-314 | CONFIRMED |
| BR-M18-031 | Reject writes `Type="Order"` (copy-paste); approve writes "Leave" | :277,:408 vs :229,:344 | CONFIRMED |
| BR-M18-032 | Date filter on leave EntryDate: default last 7 days .. today | :39-40,:512-519 | CONFIRMED |
| BR-M18-033 | Approve link disabled when status text is "Approved"; reject disabled when "Rejected" | aspx:276,:280 | CONFIRMED |
| BR-M18-034 | Role 3 is gate-exempt here only | :558 | CONFIRMED |
| BR-M18-035 | Edit button never visible (`Visible=false`) | aspx:273 | CONFIRMED |
| BR-M18-036 | Log hierarchy ids sent as 0 (market lookup disabled) | :463-505,:220-223 | CONFIRMED |

## 20. Permission Matrix
Menu-permission (non-role-2); UI gating by RoleTypeId; special user 496. No server-side re-check of role/step in handlers. Session missing → redirect to Dashboard.

## 21. Status Lifecycle
Written to log: Verified (intermediate), Accepted (final), Rejected. Display statuses from list SP: "Approved"/"Rejected" (aspx enable rules use `ApprovalStatusWeb`) — the mapping Accepted→Approved is INFERRED (customer SP shows 2→"Approved"; see GAP-M18-026). Step counter +1 per action. Application header status/leave balance effects: UNKNOWN (SP).

## 22. Error Handling
Loader silent catches; page-level catch → Dashboard redirect only for Page_Load; save exceptions unhandled.

## 23. Audit
As BR-M18-009; no comments.

## 24. Integration
Leave data originates in hr-field-force pages; SP naming `sp_SaveLeaveAppLog` (no `webapi` prefix) differs from siblings (older alternative name commented out).

## 25. Reports
None (see hr-field-force module for leave reports).

## 26. Dependencies
CustomerInfoDAL, CommonDataLoad, SeedDataDAL, Setup2DAL, `LeaveApplo_Save`, `ResultInfo`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | RoleTypeId 5 approves leave | Status "Accepted" | 030 |
| TC-02 | RoleTypeId 9 approves | "Accepted" | 030 |
| TC-03 | AM (role not 5/9) approves | "Verified", Step+1 | 030,001 |
| TC-04 | EmpInfoId 496 (any role) approves | "Accepted" | 004,030 |
| TC-05 | Reject | Type "Order" sent, Status "Rejected" | 031 |
| TC-06 | Row already "Approved" | approve link disabled | 033 |
| TC-07 | Page default load | From = today-7, To = today | 032 |
| TC-08 | Non-exempt role, row waits for other role | buttons hidden, message | 002 |
| TC-09 | Leave Type filter selected | compare `B.LeaveTypeId` with a LeaveBalanceId value — verify mismatch | GAP-016 |
| TC-10 | SP returns no scalar | "Already Exist!" | 013 |

## 28. Reverse Engineering References
`Solution.Web/Approval_UI/LeaveApproveList.aspx(.cs)`, `CustomerInfoDAL.cs:752,1467`, WF-M18-02.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | §12 | MISSING |
| Scope by hierarchy | None existed | disabled (commented) | UNKNOWN/RISK |
| Special user | None existed | hard-coded 496 | RISK |

## 30. Known Gaps
GAP-M18-001, 003, 004, 007, 009, 010, 011 (edit link target), 015 (zero hierarchy), 016 (leave type id mismatch), 026.

## 31. Change Impact
`SaveLeave_ApplogDAL` also exists as duplicate in `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:1026` (not called by this page). `MenuId 381` shared with OrderApproveList (BR-M18-029). Changing `sp_SaveLeaveAppLog` affects only this page (grep verified).
