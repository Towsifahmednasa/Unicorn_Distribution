# WF-M05-03 — Customer credit limit: entry → approval → enforcement

**Scope:** define a per-company credit limit for a customer, approve it, and use it to block invoice generation. Evidence: `CustomerCreditLimitEntry.aspx.cs`, `CustomerCreditLimitApproval.aspx.cs`, `CustomerCreditView.aspx.cs`, `CreditLimitDAL.cs`, `sp_LoadOrderListForOrderCreationbyTerri` (body read, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1225-1365`), `InvoiceCreationByOrder.aspx.cs`, `InvoiceDAL.cs`.

## Trigger & actors
- Maker: any user with `CustomerCreditLimitEntry.aspx` in the menu. Checker: any user with `CustomerCreditLimitApproval.aspx` (no role or separation-of-duty check — BR-M05-023). Consumer: invoice clerks on `InvoiceCreationByOrder.aspx` (order-management module).

## Preconditions
Customer exists in `tblCustMaster`; company in `tblCompanyInfo`; for enforcement, orders exist in `tblOrder` (`IsInvoice=0`, approved) and invoices/payments feed the dues calculation.

## Steps
1. **Enter** — `CustomerCreditLimitEntry` (PAGE-190): company + customer (autocomplete `GetCustomernew`, hidden id) + amount + days. `ValidateForm` (BR-M05-019) → `IsDuplicate` (an *active* limit for the same company+customer blocks — BR-M05-020) → `CreditLimitDAL.SaveCreditLimit`: inline `INSERT INTO tblCustomerCreditLimit(CompanyId,CustomerMasterId,LimitAmount,DayLimit,IsActive,EntryBy,EntryDate,ActionStatus)` with `ActionStatus='Posted'`.
2. **Change (optional)** — list `CustomerCreditView` (PAGE-191, `GetCustomerCreditInfo("")` — filter controls are not applied) → Edit opens `CustomerCreditLimitEntry.aspx?ID=n` → `UpdateCreditLimit` sets the new values and `ActionStatus='Posted'` again (BR-M05-021). Delete = `UPDATE … SET IsActive=0` (BR-M05-029).
3. **Approve / Reject** — `CustomerCreditLimitApproval` (PAGE-189) lists `ActionStatus='Posted'` (BR-M05-022); the checker picks Approve or Reject and ticks rows; `ApprovalUpdateDAL` per row: `UPDATE … SET ActionStatus='Approved'|'Reject', ApprovedBy=<LoginName>, ApprovedDate=<today>` (BR-M05-023).
4. **Enforce (order list)** — `InvoiceCreationByOrder` → `OrderInfoBLL/DAL` → `sp_LoadOrderListForOrderCreationbyTerri(@comunitId,@manufacId,@TerritoryId)`: for each eligible order computes `Due` (delivered total − payments − AIT − discount on payment − returns from `CSTL_ACCDB_Unicorn` vouchers), the oldest unpaid invoice date and the limit `LM` = SUM of `IsActive=1` and `Approved` limits for the same company/customer; `Permission = 1` if `LimitAmount = 0` or (`GrossValue + Due <= LimitAmount` and `days since oldest unpaid <= DayLimit`) else 0 (BR-M05-024/025/026).
5. **Block** — `orderGridView` binding sets the button per row: `Permission == "1"` ⇒ "Generate Invoice" (confirm); else red "Blocked" with client alert "Credit Limit Exceeded! Invoice creation is blocked for this customer." (BR-M05-027).
6. **Print** — invoice print SQL (`InvoiceDAL.InvoiceMainDatainvNo` etc.) returns `TotalDue` and `RemainingCreditDays = MAX(0, ΣDayLimit − days since oldest unpaid)` (BR-M05-028).

## Data created / changed
| Table | Change |
|---|---|
| `tblCustomerCreditLimit` | INSERT (Posted) → UPDATE (Approved/Reject, ApprovedBy/Date) → UPDATE on edit (Posted) → UPDATE IsActive=0 on delete |
Status values: `Posted`, `Approved`, `Reject` (literal); `IsActive` bit separate.

## Branches / exceptions
- Duplicate active limit ⇒ "Data already exists."
- Rejected, Posted (incl. edited approved limits), inactive or absent limits ⇒ customer is treated as *unlimited* for enforcement.
- Multiple approved active rows for one company+customer are summed.
- No server-side check at invoice generation beyond the UI button (INFERRED).
- DAL results ignored on approval ⇒ success message shown even if update failed.

## Postconditions
Approved active limit governs the "Generate Invoice" button and invoice print until edited/deleted/replaced.

## Related BRs
BR-M05-019 … 029.

## Related pages
PAGE-190 CustomerCreditLimitEntry, PAGE-191 CustomerCreditView, PAGE-189 CustomerCreditLimitApproval; consumer `SInventory_UI/InvoiceCreationByOrder.aspx` (order-management module).

## Tests
1. Create limit 50 000 / 30 days ⇒ row `Posted`; order list still shows "Generate Invoice" (not yet enforced).
2. Approve ⇒ order with gross 60 000 and dues 0 becomes "Blocked".
3. Order 20 000, dues 10 000 ⇒ allowed; oldest unpaid invoice 40 days old ⇒ blocked.
4. Edit approved limit ⇒ status `Posted`; order list allows any amount until re-approved.
5. Reject ⇒ customer unlimited.
6. Soft delete ⇒ `IsActive=0`, enforcement stops; a new active limit can be created.
