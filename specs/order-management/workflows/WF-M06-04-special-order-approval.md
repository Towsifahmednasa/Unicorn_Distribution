# WF-M06-04 — Special-order approval (M18 engine consumer)

**Page:** PAGE-220 `ManualOrderApproval`. **BR:** BR-M06-014…018. **Cross-link:** M18 generic approval engine (`specs/approval-workflow/README.md`; `ClsApprovalAction`, `tblAction`, `tblActionSteps`, `tblActionStepDetail`, `tblActionPageWiseStep`, `tblActionUserWiseApproval`); `CustomerCreditLimitApproval.aspx` shares the same helper methods.

## Trigger & actors
An approver (user with a row in `tblActionUserWiseApproval` for this page's `tblMainMenu` entry) reviews orders flagged `IsSpecialApproval=1`.

## Preconditions
Orders exist with `IsSpecialApproval=1`, not invoiced, `ActionStatus` equal to the approver's step `ActionCondition`. The producer of these orders (mobile app / integration SP) is UNKNOWN in the repo; the manual-order SP does not set the flag.

## Steps
1. `Page_Load` → `ManualOrderInformation()`: page name from `Request.Path`, user from `Session["LoginName"]`.
2. `LoadForApprovalConditionBLL` → `ClsApprovalAction.LoadForApprovalByUserCondition`: joins `tblActionUserWiseApproval UA ⨝ tblActionPageWiseStep PS(ManuSL) ⨝ tblActionSteps SA(ASId) ⨝ tblActionStepDetail AD(ASId, ActionId) ⨝ tblMainMenu MM(UA.ManuSL=MM.SL)` for (LoginName, `MM.URL LIKE '%page%'`) → first row's `ActionCondition`.
3. `LoadApprovalControlBLL(RadioButtonList)` → all `tblAction WHERE IsShow=1`; disable all; enable the user's `ActionId` and "Cancel"; disable the item equal to `ActionCondition`.
4. `GetSpecialApproveOrder(actionStatus)` → grid (order + customer + qty + gross + total due).
5. Optional **Preview** → due history modal (`GetCustomerDueHistory`).
6. User selects rows + operation → `btnSubmit0_Click` → `Validation` → `SubmitApproval`: `UPDATE tblOrder SET ActionStatus=<action text>, ApprovedBy=<LoginName>, ApprovedDate=<today> WHERE OrderId=…` per row.
7. Alert "Your <action> is Successfully Done !!"; list reloaded.

## Data changed
`tblOrder.ActionStatus/ApprovedBy/ApprovedDate` only.

## Transition table (as coded)
| From (`ActionStatus`) | Actor | Action (text of `tblAction`) | To |
|---|---|---|---|
| = step `ActionCondition` (value UNKNOWN) | user with matching step | user's `ActionId` text (e.g. "Approved" — UNKNOWN) | that text |
| = step `ActionCondition` | same | "Cancel" | "Cancel" |
No check that the order is still in the source status at submit; no multi-step routing beyond the one step per user/page.

## Exceptions
No try/catch; no transaction; concurrent approvers can overwrite each other.

## Postconditions
If the text equals `Approved`/`Accepted`, the order becomes eligible for `sp_LoadOrderListForOrderCreationbyTerri` (WF-M06-02); `Cancel` orders drop out of both lists.

## Tests
PAGE-220 TC-01..07.
