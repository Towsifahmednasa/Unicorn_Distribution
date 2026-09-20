# WF-M18-02 — Field-force plan / claim / leave / order approval (multi-level Verified → Accepted)

**Family.** Pages that use the role-chain engine (WF-M18-01) with a two-tier status vocabulary: intermediate approvers write `Verified`, the final approver writes `Accepted` (page-specific), anyone can write `Rejected`. Pages: LeaveApproveList (PAGE-702), ExpenseApprovalList (PAGE-701), TourPlanApproval (PAGE-706), DCPCVPApproval (PAGE-697), OrderApproveList (PAGE-704); Tier-B siblings traced only from grep excerpts: DAApprovalList (PAGE-696), MillageApprovalList (PAGE-703), RXApprovalList (PAGE-705), DCRApprovalList (PAGE-698).

## 1. Trigger & actors
Trigger: an approver opens the list page (or a submitted item appears whose `ToRoleTypeId` is the approver's role) and presses the check or x button (or Approve/Reject Selected). Actors: AM / DZSM / NSM (role names from `Session["RoleTypeName"]`), DIC (orders), RoleTypeId 5/4/14/9/3 exempt users, employee 496 (BR-M18-004).

## 2. Preconditions
Item exists with a log row (list SP returns `Step`, `ToRoleTypeId`, `ToEmpId`, `TableId`, domain-approval id); approver passes menu permission; approver's role equals `ToRoleTypeId` or is exempt.

## 3. Steps (common)
1. `LoadData()` builds `@param` from filters + role scope → list SP → grid (see WF-M18-01 steps 4-8).
2. Approver clicks; page reads row hidden fields (`hfCustomerApprovalId`, `hfToEmpId`, `hfStep`, `hfCustomerMasterId`=TableId) — `Solution.Web/Approval_UI/*.aspx.cs` RowCommand.
3. Status decision per page (table below).
4. DTO built (`Step = hfStep+1`, `Type`, `MenuId`, audit fields) → DAL → save SP (transaction commit on close).
5. Success message and `LoadData()`.

## 4. Per-page transition matrix
| Page | Approve → Status | Final-approver rule | Reject → Status | Type approve/reject | MenuId | Gate-exempt roles | Bulk | Scope filter | Source |
|---|---|---|---|---|---|---|---|---|---|
| Leave (702) | Verified / Accepted | RoleTypeId ∈ {5,9} or EmpInfoId = 496 → Accepted | Rejected | Leave / Order | 381 | 5,4,14,3,9 (+496 partial) | yes | none (commented out) | LeaveApproveList.aspx.cs:195-206,229,277,558 |
| Expense (701) | Verified / Accepted | RoleTypeId = 5 or 496 | Rejected | ExpanseClaim / Expense | 356 | 5,4,14 (+496 partial) | no | AM/DZSM/NSM, ≤ 7 days | ExpenseApprovalList.aspx.cs:191-202,226,274,331-359,445 |
| Tour plan (706) | Verified only | none (never Accepted) | Rejected | Leave / Leave | 371 | 5,4,14 | no | AM/DZSM/NSM | TourPlanApproval.aspx.cs:215,238,264,286,344-372,417 |
| DCP/CVP (697) | Verified only | none | Rejected | DCP / Leave | 377 | 5,4,14,9 | yes | AM/DZSM/NSM | DCPCVPApproval.aspx.cs:215,238,264,286,473-501,546 |
| Order (704) | Verified / Accepted | RoleTypeId = 5 | Rejected | Order / Order | 381 | 5,4,14 | no | AM/DZSM/NSM/DIC, ≤ 7 days, ActionStatus ∉ {2,3} | OrderApproveList.aspx.cs:111-118,141,248-278,290 |
| DA claim (696, Tier B) | Verified / Accepted | RoleTypeId = 5 or 496 | Rejected | DA Claim | 376 | 5,4,14 (+496) | no (no `btnBulk*` in code-behind) | AM/DZSM/NSM switch present | DAApprovalList.aspx.cs:140-150,174,222,390 |
| Mileage (703, Tier B) | Verified / Accepted | RoleTypeId = 5 or 496 | Rejected | Millege (sic) | 372 | 5,4,14 (+496) | no | AM/DZSM/NSM switch present | MillageApprovalList.aspx.cs:175-185,209,257,393-398 |
| Prescription/RX (705, Tier B) | Verified / Accepted | RoleTypeId = 5 | Rejected | Prescription | 379 | 5,4,14 | no | AM/DZSM/NSM switch present (7-day cap on prescription EntryDate for AM) | RXApprovalList.aspx.cs:169-175,199,411 |
| DCR (698, Tier B) | Accepted only | always Accepted | Rejected | DCR | 382 | 5,4,14,9 | yes | none (commented out) | DCRApprovalList.aspx.cs:134,157,231,468 |
(DCR approval is also executable from `DoctorVisit_UI/DCRList.aspx.cs:85-125`, which re-implements the same log write with `Status = status` argument, MenuId 382.)

## 5. Data created / changed
One approval-log row per action in a domain-specific table (names UNKNOWN except customer). Subject header status and effects (leave balance deduction, expense payout, order status, plan lock) are inside the save SPs: UNKNOWN.

## 6. Branches / exceptions
- Waiting gate hides buttons (UI only).
- Bulk (Leave, DCP, DCR, Doctor, Customer only): only ticked rows with a visible button are processed; success/failure counted per row, except Customer which counts blindly (BR-M18-019).
- Save exception: unhandled (rollback+rethrow), except Customer.
- pk-based success (Expense, Leave): SP returning 0/NULL yields "Already Exist!".
- Copy-paste defects: reject `Type` differs from approve on Leave ("Order"), DCP ("Leave"), Expense ("Expense"); Tour uses "Leave" for both (BR-M18-031/039/044/047).

## 7. Postconditions
New log step (Step+1) with actor, time and status; grid shows updated status and next waiting role (SP-defined).

## 8. Related
BR-M18-001..014, 024-047; GAP-M18-003, 004, 009, 010, 025, 026; pages above; cross-link to M06 `specs/order-management/pages/PAGE-220-manualorderapproval.md` (different engine, WF-M18-05).

## 9. Tests
| ID | Scenario | Expected |
|---|---|---|
| WF02-T1 | Leave: RoleTypeId 5 approves | log Status "Accepted" |
| WF02-T2 | Leave: RoleTypeId 3 (exempt) approves | "Verified" (role 3 exempt from gate but not final) |
| WF02-T3 | Expense: role 9 approves | "Verified" |
| WF02-T4 | Tour plan: role 5 approves | "Verified" (never Accepted) |
| WF02-T5 | Order: AM approves order created 8 days ago | order not listed |
| WF02-T6 | Order approved by role 5 | Status "Accepted", Type "Order", MenuId 381 |
| WF02-T7 | DCP: reject | Type "Leave" |
| WF02-T8 | Employee 496 on Leave list | approve/reject visible on all rows, edit hidden |
