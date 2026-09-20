# WF-M18-04 — Customer / Doctor market-transfer approval

**Pages:** request `MasterSetup_UI/Customer_Doctor_Transfer.aspx` (module customer-master/hr, not documented here); approval `Approval_UI/DoctorCustomerTransferApproval.aspx` (PAGE-700, this module); working alternative `TransferUI/CustomerDocTransferApproval.aspx` (PAGE-202, customer-master module).

## 1. Trigger & actors
A requester submits a transfer of customers/doctors to another market/DC/route (`Customer_Doctor_Transfer.aspx.cs:223` → `CustomerInfoDAL.UpdateCustomer_Doctor_Transfer` → `sp_Update_Customer_Doctor_Transfer`, `CustomerInfoDAL.cs:150`). An approver then approves the pending request. Actor identity recorded = `Session["UserId"]`.

## 2. Preconditions
Pending transfer rows exist (`sp_Get_CustomerTranferApprovalList` / `sp_Get_DoctorTranferApprovalList`, bodies UNKNOWN). Customer selected must have no pending order (validation SP `sp_check_Vali_MarketStructure` with page name "CustomerTransfer" returns no row).

## 3. Steps (as coded in PAGE-700)
| # | Step | Where | Conf. |
|---|---|---|---|
| 1 | Page load binds customer list | `Page_Load` → `rbType_SelectedIndexChanged(null,null)` → `CustomerInfoDAL.GetCustomerTranferList("")` (:78) | CONFIRMED |
| 2 | Toggle to Doctor | `rbType_SelectedIndexChanged` → `DoctorDAL.GetDoctorTransferList("")` (:84) | CONFIRMED |
| 3 | Tick a customer → validation | `chkSelect_CheckedChanged` → `CheckCustInOrder` → `CommonDataLoad.GetValidationCheck` → `sp_check_Vali_MarketStructure`; rows>0 → untick + "Order Pending!" | CONFIRMED |
| 4 | Submit | `btnSubmit0_Click` (:215) → `if (Validation())` | CONFIRMED |
| 5 | `Validation()` checks `statusRadioButtonList` (hidden, default Approve) then counts ticked rows of `loadGridView` — an unbound stock-out grid with 0 rows → returns false | `:282-318` | CONFIRMED |
| 6 | (never reached) per ticked row → `UpdateCustomer_Doctor_TransferApprove(Type "Cust" or "Doc", MasterID/DoctorId, "", UserId, "", "")` → `UpdateData("sp_Update_Customer_Doctor_TransferApprove")` (one implicit tx per call) | `:223-264`, `CustomerInfoDAL.cs:170-203` | CONFIRMED (code path) / UNKNOWN (SP) |
| 7 | (never reached) message and redirect `Customer_Doctor_Transfer.aspx` | `:269-277` | CONFIRMED |

## 4. Data created / changed
None in practice (step 6 unreachable). Intended: transfer request status and customer/doctor market/route/DC assignment (SP, UNKNOWN).

## 5. Branches / exceptions
- No row ticked → nothing (no message).
- If Validation were fixed: only last row's result determines the message; per-row transactions (partial success possible).
- Reject radio exists (hidden) but the live handler always approves (reject exists only in dead `SubmitApproval`).
- List/validation exceptions swallowed (blank grid).

## 6. Postconditions
Unchanged data. For the maintained approval flow see `specs/customer-master/pages/PAGE-202-customerdoctransferapproval.md` (`_DAL.CustomerDocTransferApprovalDAL`, `CustomerDocTransferApproval.aspx.cs:57`).

## 7. Related
BR-M18-050..055, 013; GAP-M18-011, 014, 028; PAGE-700.

## 8. Tests
| ID | Scenario | Expected |
|---|---|---|
| WF04-T1 | Load PAGE-700 | customer list |
| WF04-T2 | Tick customer with pending order | "Order Pending!" |
| WF04-T3 | Tick and Submit | no DB call (dead) — confirm via profiler |
| WF04-T4 | Same flow on PAGE-202 | working approval (out of M18 scope) |
