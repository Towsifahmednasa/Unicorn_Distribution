---
page_id: PAGE-220
module: M06
page: ManualOrderApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-220 — ManualOrderApproval

> **Evidence level:** Tier A — `Solution.Web/SInventory_UI/ManualOrderApproval.aspx(.cs)`, `OrderListBLL/DAL` (approval methods), `ClsApprovalAction.cs`, `OrderInfoDAL.GetCustomerDueHistory`. Database is PARTIAL: contents of `tblAction*` tables (the action vocabulary) are not in the repo. This is the M06 consumer of the generic approval engine documented in M18 (`specs/approval-workflow/README.md`).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-220 |
| Module | M06 — Order Management (uses M18 approval engine) |
| Kind | Approval |
| Folder | SInventory_UI; master `NewMasterPage.master` |
| Class | SInventory_UI_ManualOrderApproval |
| Code-behind LOC | 177 |

## 2. Purpose
Shows orders flagged for "special approval" (over-credit/over-due exceptions) that are waiting at the approval step the logged-in user is responsible for, lets the user see the customer's due history, and approve/cancel a batch of them. INFERRED from the SQL filter `IsSpecialApproval = 1` and the "Total Due"/"Due History" UI (CONFIRMED literal labels).

## 3. Business Objective
Give a designated approver control over exception orders before they can be turned into proforma invoices. After approval, `ActionStatus` carries the action text (e.g. "Approved") which the invoice list accepts (`'2'`, `'Accepted'`, `'Approved'` — `LIVE_DB_UPDATE…sql:1362`). INFERRED (the actual action texts live in DB table `tblAction`).

## 4. Navigation
Menu-driven; `tblMainMenu.URL LIKE '%ManualOrderApproval.aspx%'` is joined in `ClsApprovalAction` to find this user's approval step, so the page name is a **key into the approval configuration** (renaming the file would break approval). No inbound code links (grep).

## 5. Route
`~/SInventory_UI/ManualOrderApproval.aspx`.

## 6. Source Files
View/code-behind above; `Library.BLL/SInventory_BLL/OrderListBLL.cs:33-52`; `Library.DAL/SInventory_DAL/OrderListDAL.cs:29-88, 282-354`; `Library.DAL/InternalCls/ClsApprovalAction.cs`; `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory:712`.

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| statusRadioButtonList | operation (Approve / Cancel / …) | Yes (server `:103`) | items = `tblAction WHERE IsShow=1`; enabled state computed per user (BR-014) |
| loadGridView | pending orders | — | DataKeys `OrderId,CustomerMasterId`; columns Order Code, Company Name, Unit Name, Order Date (dd-MMM-yyyy), Customer, MIO, Market, Territory, Region, Area, QTY, Gross Value, Total Due, Preview (LinkButton `PreviewDueHistory`), row checkbox `chkSelect`, header `chkSelectAll` (AutoPostBack) |
| submitButton / cancelButton | Submit / Cancel | — | Cancel simply reloads the list |
| ModalPopupExtender10 → dueGridView | Due History popup | — | columns SL, Company, Unit, Invoice No, Invoice Date, Due Amount; close button `Button41` |

## 8. Field Mapping
| UI | DTO | BLL → DAL | SQL | Column |
|---|---|---|---|---|
| statusRadioButtonList.SelectedItem.Text | OrderInfoMaster.ActionStatus | `ApprovalUpdateDAL` → `OrderListDAL.ApprovalUpdateDAL:339` | `UPDATE tblOrder SET ActionStatus=@ActionStatus, ApprovedBy=@ApprovedBy, ApprovedDate=@ApprovedDate WHERE OrderId=@OrderId` | tblOrder.ActionStatus |
| Session["LoginName"] | ApprovedBy | ″ | ″ | tblOrder.ApprovedBy |
| DateTime.Today | ApprovedDate | ″ | ″ | tblOrder.ApprovedDate (date only) |
| row DataKeys[0] | OrderId | ″ | WHERE | tblOrder.OrderId |

## 9. Dropdown Mapping
Radio list only: `OrderListDAL.LoadApprovalControlDAL` (RadioButtonList overload → `ClsApprovalAction.LoadActionControlByUser`): item text `tblAction.ActionText`, value `ActionId`.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| submitButton | btnSubmit0_Click `:84` | validate → UPDATE tblOrder per checked row → reload list |
| cancelButton | cancelButton_Click `:22` | reload |
| Preview | loadGridView_RowCommand `:154` | loads `GetCustomerDueHistory(customerMasterId)` into modal |
| chkSelectAll | chkSelectAll_CheckedChanged `:70` | toggles all row checkboxes |

## 11. Frontend Flow
Standard UpdatePanel postbacks; ModalPopupExtender shows the Due History panel. No page-local JS beyond `GridView` header fix. Inline `<style>`/script only cosmetic (not read in detail).

## 12. Backend Flow
`Page_Load` (!PostBack) → `ManualOrderInformation()`: `fileName = Path.GetFileName(Request.Path)`, `userName = Session["LoginName"]`; `actionStatus = LoadForApprovalConditionBLL(fileName,userName)` (= `ActionCondition` of the user's step, or "" if none) → `LoadApprovalControlBLL(radio, …)` → `GetSpecialApproveOrder(actionStatus)`. Submit: `Validation()` (operation chosen; ≥1 checked row — note the loop stops at the first checked row) → `SubmitApproval()` loops checked rows → `ApprovalUpdateDAL` (inline UPDATE via `SInventorySql.Execute`) → alert "Your <text> is Successfully Done !!" → list reload. No transaction, no try/catch, no per-row result check.

## 13. API Mapping
None.

## 14. Service Mapping
OrderListBLL: LoadForApprovalConditionBLL, LoadApprovalControlBLL(RadioButtonList), GetSpecialApproveOrder, ApprovalUpdateDAL. OrderInfoBLL.LoadCustomerDueHistory.

## 15. Repository Mapping
- `ClsApprovalAction.LoadActionControlByUser` / `LoadForApprovalByUserCondition` — **string-concatenated SQL** (`WHERE UA.LoginName='" + userName + "' AND MM.URL LIKE '%" + pageName + "%'`) — injection-prone, though `userName` comes from session and `pageName` from `Request.Path`.
- `OrderListDAL.GetSpecialApproveOrder` (`:282-337`): tblOrder ⟕ tblOrderDetail(SUM qty) ⟕ tblRegion/tblArea/tblMarket/tblTerritory/tblCompanyUnit/tblCompanyInfo ⟕ (due subquery on tblInvoice: `SUM(DeliveryTpGrandTotal − (PaymentAmount+DiscountOnPayment+AIT))` where `DeliveryInvoiceStatus IS NOT NULL AND <> 'Reject'`) `WHERE (IsInvoice IS NULL OR 0) AND ActionStatus=@ActionStatus AND IsSpecialApproval=1`.
- `GetCustomerDueHistory` (`OrderInfoDAL.cs:712`): per-invoice due = ΣDeliveryTpGrandTotal − (ΣPaymentAmount+ΣAIT+ΣDiscountOnPayment) − sales-return voucher amount from `CSTL_ACCDB_Unicorn..tblDebitCreditVoucher*` (TableID=2018), excluding `IsBadDue=1`.

## 16. Stored Procedure Mapping
None (all inline SQL).

## 17. Database Mapping
Read: tblActionUserWiseApproval, tblActionPageWiseStep, tblActionSteps, tblActionStepDetail, tblMainMenu, tblAction, tblOrder, tblOrderDetail, tblInvoice, tblCompanyUnit/Info, hierarchy tables, cross-DB `CSTL_ACCDB_Unicorn` vouchers. Written: `tblOrder.ActionStatus/ApprovedBy/ApprovedDate`.

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| Operation selected | `Validation:103` | "Please select an operation!!" | BR-M06-016 |
| ≥1 row checked | `Validation:109-124`, `btnSubmit0_Click:92` | "Please select at least one row!!!" | BR-M06-016 |
| User authorised for this step | not checked at submit — only controls which radio items are *enabled* at render time | — | BR-M06-017 |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-014 | Approver options are per user+page: radio items are all disabled unless the user has a row in `tblActionUserWiseApproval` joined (via `ManuSL`) to this page's menu row; then only the user's `ActionId` and the "Cancel" item are enabled, and the item whose text equals the step's `ActionCondition` is disabled. | ClsApprovalAction.cs:13-64; OrderListDAL.cs:29-78 | DAL | CONFIRMED |
| BR-M06-015 | The queue = orders with `IsSpecialApproval=1`, not invoiced, whose `ActionStatus` equals the user's step `ActionCondition` (empty string when the user has no step ⇒ matches only orders whose ActionStatus = ''). | OrderListDAL.cs:282-337; ClsApprovalAction.cs:65-86 | inline SQL | CONFIRMED |
| BR-M06-016 | An operation and at least one order must be chosen before submit. | ManualOrderApproval.aspx.cs:98-127 | page | CONFIRMED |
| BR-M06-017 | Approval is a plain overwrite: `ActionStatus := selected action text`, `ApprovedBy := LoginName`, `ApprovedDate := today`. No re-check that the order is still in the source status or that the user is entitled at postback time; no history row. | .cs:129-152; OrderListDAL.cs:339-354 | page + SQL | CONFIRMED |
| BR-M06-018 | Approver sees each customer's total due (invoiced−paid, excluding rejected delivery invoices) and a per-invoice due history (excluding bad-debt invoices, net of sales-return vouchers) before deciding. Two different due formulas are used in the grid vs. the popup. | OrderListDAL.cs:307-314; OrderInfoDAL.cs:712-745 | inline SQL | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]` only. No role check; effective authorisation = existence of `tblActionUserWiseApproval` row for (LoginName, this page). A user without a row sees a fully disabled radio list but the grid (with `actionStatus=""`) still loads and — because submit does not re-check — a crafted postback could still write ActionStatus (radio disabled items are not posted; `SelectedValue==""` blocks). Menu access is also required. CONFIRMED (code).

## 21. Status Lifecycle
`tblOrder.ActionStatus`: [special-approval order, set by an unknown producer] —(user approves: text of chosen `tblAction`)→ e.g. 'Approved' (accepted by invoice list) / 'Cancel'. Step sequencing (multi-level) is expressed by `tblActionStepDetail.ActionCondition` (the status required *before* this user may act) — values UNKNOWN. Who sets `IsSpecialApproval=1` is UNKNOWN (no setter in repo: only `GetSpecialApproveOrder` references it; `sp_Save_ManualOrderMaster` does not).

## 22. Error Handling
None: exceptions bubble to the framework (no try/catch). `ShowMessageBox` escapes `'` correctly here (`\\'`).

## 23. Audit
`ApprovedBy`, `ApprovedDate` (date only) on tblOrder; previous value overwritten, no trail.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
M18 approval tables; `CSTL_ACCDB_Unicorn` accounts DB (due history); tblInvoice.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | User with step (ActionId=Approve, ActionCondition=Pending) opens page | only Approve and Cancel enabled; grid shows orders with ActionStatus='Pending', IsSpecialApproval=1 | 014,015 |
| TC-02 | Positive | select 2 rows + Approve → submit | both orders ActionStatus='Approved', ApprovedBy=login, ApprovedDate=today | 017 |
| TC-03 | Negative | submit with no operation | "Please select an operation!!" | 016 |
| TC-04 | Negative | operation but no row | "Please select at least one row!!!" | 016 |
| TC-05 | Edge | user with no approval row | all radios disabled, grid for ActionStatus='' | 014,015 |
| TC-06 | Edge | another approver already approved a listed order | second submit overwrites again (no guard) | 017 |
| TC-07 | Positive | Preview a customer with a returned invoice | popup due = ΣNet − paid − return voucher | 018 |

## 28. Reverse Engineering References
`ManualOrderApproval.aspx(.cs)`, `OrderListDAL.cs`, `ClsApprovalAction.cs`, `OrderInfoDAL.cs:712`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Authorization at submit | None existed | not enforced | GAP-M06-009 |
| Special-approval producer | None existed | UNKNOWN | GAP-M06-008 |

## 30. Known Gaps
Producer of `IsSpecialApproval` unknown; no re-validation at submit; validation loop stops at first checked row (fine, but header loop still counts); string-concatenated SQL in `ClsApprovalAction`; grid due vs popup due formulas differ; page name coupled to `tblMainMenu.URL LIKE`.

## 31. Change Impact
`OrderListDAL.LoadApprovalControlDAL`, `LoadForApprovalConditionDAL`, `ApprovalUpdateDAL`, `GetSpecialApproveOrder` are shared with `Solution.Web/SInventory_UI/CustomerCreditLimitApproval.aspx.cs` (grep of `Solution.Web` shows only these two pages call the `OrderListBLL` approval helpers), so a change to the helper signatures/semantics also affects credit-limit approval. `ClsApprovalAction` is shared by all M18 pages. Changing status literals here changes eligibility in `sp_LoadOrderListForOrderCreationbyTerri`.
