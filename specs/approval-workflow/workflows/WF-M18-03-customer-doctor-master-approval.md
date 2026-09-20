# WF-M18-03 — Customer and Doctor master-data approval

**Pages:** CustomerApproveList (PAGE-695), DoctorApprovalList (PAGE-699). Uses the role-chain approval-log engine (WF-M18-01) but with master-data specifics: no hierarchy ids sent, no scope filter, duplicate-mobile detection (customer only), and different final-status behaviour.

## 1. Trigger & actors
Trigger: a customer/doctor record exists whose latest approval-log row is "waiting" (customer: `tblCustMaster.ActionStatus` not in ('2','3'), verified in `sp_Get_CustomerApp`; doctor: rows returned by `sp_Get_DoctorClaimApp`, criteria UNKNOWN). Actors: approver by RoleTypeId (gate-exempt 5,4,14,9 on both pages), submitter (mobile/web; source UNKNOWN).

## 2. Preconditions
- Record created through a path that inserts a pending status and a log row. **The repository's web-entry SPs do not do this**: `sp_Save_CustomerMaster` inserts `ActionStatus='2'` (approved) and code `'C'+(MAX(code among approved)+1)` (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:403-540`); `sp_Save_DoctorMaster` inserts `ApprovalStatus='2'`, `IsFromApp=0`, code `'D'+...` (:672-735). So records reaching these queues are created elsewhere (mobile app, INFERRED via `IsFromApp`, `sp_webapi_*` naming; customer path UNKNOWN).
- Approver has menu permission for the page (BR-M18-011).

## 3. Steps
| # | Step | Where | Conf. |
|---|---|---|---|
| 1 | Open list | Customer: `LoadData` → `CustomerInfoDAL.GetCustomer_ApplogDAL` → `sp_Get_CustomerApp` (dynamic SQL, `@param` appended; excludes ActionStatus 2/3; latest log per customer via `MAX(Step)`; `ExistenceStatus`, `WaitingForRole`, `ApprovalStatusWeb` derived) — `CustomerApproveList.aspx.cs:409-563`, `LIVE_DB...sql:221-395`. Doctor: `LoadData` → `DoctorDAL.GetDoctor_ApplogDAL("", "", …)` → `sp_Get_DoctorClaimApp` — `DoctorApprovalList.aspx.cs:338-422` | CONFIRMED (customer SP), UNKNOWN (doctor SP) |
| 2 | Waiting gate | `hfToRoleTypeId` vs session role; "Waiting for Another Approver" (customer) / "Waiting for Another Employee" (doctor) | CONFIRMED |
| 3 | Approver inspects (optional) | Edit → new window `MasterSetup_UI/CustomerEntry.aspx?MID=` or `DoctorEntry.aspx?MID=` | CONFIRMED |
| 4 | Approve | Customer: Status "Accepted" (always), Type "Customer", MenuId 302, `SaveCustomer_ApplogDAL` → `ClsCommonInternalDAL.DeleteAction("sp_webapi_SaveCustomerAppLog")` (EntLib, exceptions swallowed). Doctor: Status "Verified" (always), Type "Doctor", MenuId 303, `DoctorDAL.SaveDoctor_ApplogDAL` → `SaveData("sp_webapi_SaveDoctorAppLog")` | CONFIRMED |
| 5 | Reject | Status "Rejected" (both), same SPs | CONFIRMED |
| 6 | Bulk | tick rows + Approve/Reject Selected; customer counts all attempts as success | CONFIRMED |
| 7 | SP updates log and master status | `tblCustMaster.ActionStatus` 0→1→2 / 3 (INFERRED from list SP mapping and `ApprovalStatusWeb` text); doctor `ApprovalStatus` (UNKNOWN); code assignment at approval (UNKNOWN — codes are assigned at insert in the repo SPs) | UNKNOWN |

## 4. Data created / changed
Approval-log rows (`tblCustomerApprovalLog` verified by name for customers; doctor log table name UNKNOWN); master status columns via SP (UNKNOWN).

## 5. Branches / exceptions
- Duplicate mobile (`CellNo` shared with another customer whose `ActionStatus <> '3'`) → `ExistenceStatus='Already Exists'`; intended red row but `loadGridView_RowDataBound` searches a column header "ExistenceStatus" that does not exist (BR-M18-017, GAP-M18-017).
- Customer save exceptions are swallowed and reported as success (BR-M18-019).
- Doctor never reaches "Accepted" from this page (BR-M18-021).
- No comment/reason capture on rejection (Comments never set).

## 6. Postconditions
Customer approved: no longer listed (ActionStatus 2). Rejected: no longer listed (3). Verified/intermediate: still listed, now waiting on next role.

## 7. Related
BR-M18-015..023, 001..014; GAP-M18-012, 015, 020, 022, 026; PAGE-168 CustomerEntry, PAGE-669 DoctorEntry (customer-master / doctor-visit modules).

## 8. Tests
| ID | Scenario | Expected |
|---|---|---|
| WF03-T1 | Customer with duplicate CellNo listed | "Already Exists" and (expected) red row — currently no highlight |
| WF03-T2 | Approve customer | Status "Accepted", Step+1, MenuId 302 |
| WF03-T3 | DB down during customer approve | UI shows "Operation successful!" though nothing saved |
| WF03-T4 | Doctor approve by role 5 | Status "Verified" |
| WF03-T5 | Customer created by `sp_Save_CustomerMaster` | never appears in queue (ActionStatus 2) |
