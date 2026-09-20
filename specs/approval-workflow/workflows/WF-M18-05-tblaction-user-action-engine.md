# WF-M18-05 — Per-user action-permission engine (`tblAction*`, `ClsApprovalAction`)

**Status in M18:** this is the *other* generic approval mechanism in the code base. It is documented here because the assignment for module M18 names it, but **no page in `Solution.Web/Approval_UI` uses it** (grep for `ClsApprovalAction`, `LoadApprovalControl*`, `LoadForApproval*` in `Solution.Web` finds only `SInventory_UI/ManualOrderApproval.aspx.cs:53,55`). The only live consumer found is M06 `ManualOrderApproval` (`specs/order-management/pages/PAGE-220-manualorderapproval.md`). `CreditLimitDAL.LoadApprovalControlDAL/LoadForApprovalConditionDAL` (`CreditLimitDAL.cs:69-77`) wrap the engine but no caller exists: `SInventory_UI/CustomerCreditLimitApproval.aspx.cs` (PAGE-189) calls only `LoadCustomerCreditLimitApproval` and `ApprovalUpdateDAL` (:38,:129,:153) — dead wrappers.

## 1. Trigger & actors
A user opens an approval page whose access is governed by a per-user, per-menu action assignment. Actor: any logged-in user with rows in `tblActionUserWiseApproval` for their `LoginName`.

## 2. Preconditions / data model (names from SQL text only; no DDL in repo)
| Table | Role (from the join in `ClsApprovalAction.cs:15-19`) |
|---|---|
| `tblAction` | master list of action types; columns `ActionId`, `ActionText`, `IsShow` (`ClsApprovalAction.cs:25`, `OrderListDAL.cs:48`) |
| `tblActionSteps` (`ASId`) | named step set |
| `tblActionStepDetail` | `ASId`, `ActionId`, `ActionCondition` — which action is allowed and the status condition it applies to |
| `tblActionPageWiseStep` | `ManuSL` (menu id) ↔ `ASId` — which step set a page uses |
| `tblActionUserWiseApproval` | `LoginName`, `ManuSL`, `ActionId` — which user may perform which action on which menu |
| `tblMainMenu` | `SL`, `URL` — page match by `URL LIKE '%<pageName>%'` |
Population of these tables (admin UI) — not found in Approval_UI; configuration screen UNKNOWN (GAP-M18-002).

## 3. Steps
| # | Step | Code | Conf. |
|---|---|---|---|
| 1 | Approval page reads current file name and Session LoginName | `ManualOrderApproval.aspx.cs:51-52` | CONFIRMED |
| 2 | Determine the status condition the user works on | `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition(pageName,userName)`: runs the 5-table join with `UA.LoginName='<user>' AND MM.URL LIKE '%<page>%'` (string-concatenated, executed via `ExecuteAllSqlQueryByStoreProcedure`, connection key `SolutionConnectionStringSSIDB`), returns `Rows[0]["ActionCondition"]` or `""` (`ClsApprovalAction.cs:65-86`) | CONFIRMED |
| 3 | Load the list of orders in that condition | `orderListBll.GetSpecialApproveOrder(actionStatus)` (M06) | CONFIRMED |
| 4 | Build the action radio list | `OrderListBLL.LoadApprovalControlBLL(RadioButtonList…)` → `ClsApprovalAction.LoadActionControlByUser` (:13-64): read `tblAction WHERE IsShow=1` into the list (text `ActionText`, value `ActionId`); with the user's matching row: enable the item whose `ActionId` equals the row's; disable all other items except the one whose text is "Cancel"; also disable the item whose text equals the row's `ActionCondition`; no matching row → disable all items | CONFIRMED |
| 5 | (DropDownList variant) | `OrderListDAL.LoadApprovalControlDAL(DropDownList…)` (:29-78): same rules but parameterised SQL (`@LoginName`, `@PageName` via `SInventorySql`), case-insensitive compare, "Cancel" always enabled | CONFIRMED |
| 6 | User selects an action and submits | `btnSubmit0_Click` → `Validation()` → `SubmitApproval()` → `orderListBll.ApprovalUpdateDAL(orderInfoMaster{OrderId, ActionStatus=SelectedItem.Text, ApprovedBy=LoginName, ApprovedDate=Today})` (`ManualOrderApproval.aspx.cs:84-152`) | CONFIRMED |

## 4. Data created / changed
Only the target document (`tblOrder.ActionStatus` etc., M06). The engine tables are read-only from code (no writes found in DAL).

## 5. Branches / exceptions
- More than one row for a user/page: only `Rows[0]` is used (undefined ordering, no `ORDER BY`) (BR-M18-057).
- "Cancel" is always selectable (if the item text equals "Cancel") (BR-M18-058).
- The item whose text equals `ActionCondition` is disabled (prevents re-applying the same status) (BR-M18-059).
- No assignment → everything disabled (BR-M18-060).
- The step-based tables (`tblActionSteps`, `tblActionStepDetail`, `tblActionPageWiseStep`) only feed the `JOIN`; step **ordering/sequencing is not evaluated in code** (no next-step logic) — multi-level sequencing, if any, is data-driven via `ActionCondition` matching.
- The legacy `ClsApprovalAction` builds SQL by string concatenation with `userName` and `pageName` (GAP-M18-018); the `OrderListDAL` copy is parameterised.

## 6. Postconditions
UI shows only permitted actions; the document status updates through M06.

## 7. Related
BR-M18-056..060; GAP-M18-002, 018; M06 PAGE-220; `WarehouseStockInApprovalDal.cs:16` has the engine call commented out (`//ClsApprovalAction approvalAction`), `DB_Manager.cs:201` has a commented duplicate.

## 8. Tests
| ID | Scenario | Expected |
|---|---|---|
| WF05-T1 | User with one `tblActionUserWiseApproval` row for the page | only that action + "Cancel" enabled |
| WF05-T2 | User with no row | all actions disabled |
| WF05-T3 | User whose row `ActionCondition` equals an action's text | that action disabled |
| WF05-T4 | User with two rows for the same page | first returned row wins; document nondeterminism |
| WF05-T5 | `LoginName` containing `'` | legacy RadioButtonList path breaks / injectable |
