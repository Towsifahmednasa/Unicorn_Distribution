# WF-M05-02 — Customer approval status and pending queue

**Scope:** how `tblCustMaster.ActionStatus` represents Pending/Verified/Approved/Rejected customers and how the pending queue is presented. This process is only *partially traced*: mobile-app creation and the SPs that change status live outside this module (approval-workflow module, PAGE-695) or outside the repo. Only facts read in code are stated; the rest is `UNKNOWN`.

## Trigger & actors
- Trigger: a customer row exists with `ActionStatus` in (`'0'`,`'1'`) — e.g. created by field staff through the mobile API (INFERRED; the web page auto-approves, BR-M05-005).
- Actors: approvers by hierarchy (MIO / ASM / RSM / NSM employee ids are exposed by `sp_Get_CustomerApp`) working in `Approval_UI/CustomerApproveList.aspx`.

## Preconditions
Customer rows with status 0/1; `tblCustomerApprovalLog` rows per step (writer = `sp_webapi_SaveCustomerAppLog`, body UNKNOWN).

## Steps
1. `CustomerApproveList.aspx.cs:526` → `GetCustomer_ApplogDAL(param, role, appStatus, from, to, empId)` → `sp_Get_CustomerApp` (body READ, `LIVE_DB_UPDATE…sql:222-393`): returns customers whose `ActionStatus` is not 2/3 joined to their latest approval-log step; shows `ExistenceStatus` ("Already Exists" when the same mobile occurs on another non-rejected customer — BR-M05-048), `WaitingForRole`, salesperson names and geography. (CONFIRMED)
2. The approver opens the record in `MasterSetup_UI/CustomerEntry.aspx?MID=…` (`CustomerApproveList.aspx.cs:114`); edits follow PAGE-168 rules and do not alter status. (CONFIRMED)
3. Approve / Reject buttons (`:121-225`) build `CustomerSaveAppLog_DAO` (Status `"Accepted"` or `"Rejected"`, `Step = current + 1`, `Type = "Customer"`, `MenuId = 302`, from/to employee, geography ids, dates = now, `EntryByS/EntryByApp = Session UserId`) → `SaveCustomer_ApplogDAL` → `sp_webapi_SaveCustomerAppLog` (via the `DeleteAction` helper). The effect on `tblCustMaster.ActionStatus` (0 → 1 → 2, or 3) is **UNKNOWN**.
4. Approved customers (`'2'`) leave the queue and are counted by the customer-code generator (only `ActionStatus='2'` rows feed `MAX(code)`, BR-M05-004).

## Data created / changed
`tblCustomerApprovalLog` (one row per step; columns beyond the DAO UNKNOWN); `tblCustMaster.ActionStatus` (who/when for 0/1/3 UNKNOWN).

## Branches / exceptions
- Web-created customers skip the queue (auto `'2'`).
- Duplicate mobile is flagged, never blocked (BR-M05-048).
- The approve/reject failure branch in the page shows a *success* alert (`CustomerApproveList.aspx.cs:167-176`, approval-workflow module) — INFERRED defect.

## Postconditions
Approved customers are usable downstream (how orders use `ActionStatus` is UNKNOWN in this module).

## Related BRs
BR-M05-004, 005, 047, 048.

## Related pages
PAGE-695 CustomerApproveList (other module), PAGE-168, PAGE-169, PAGE-173.

## Tests
1. Insert a customer with `ActionStatus='0'` and a duplicate mobile ⇒ queue shows "Already Exists".
2. Approve via UI ⇒ verify log row and `ActionStatus` (expected `'2'` — UNKNOWN).
3. Reject ⇒ `ActionStatus='3'` and excluded from the queue.
