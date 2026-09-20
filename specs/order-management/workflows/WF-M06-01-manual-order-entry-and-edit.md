# WF-M06-01 — Manual (web) order entry and edit

**Pages:** PAGE-226 `OrderListView` (list/preview/entry point), PAGE-221 `ManualOrderCreation` (create/edit). **BR:** BR-M06-001…013, 019.

## Trigger & actors
Back-office order-desk user (any menu-permitted user; no role check) receives an order by phone/walk-in/transfer and keys it in. Edit happens from the OrderListView preview/edit link while the order is not invoiced.

## Preconditions
Customer exists in `View_CustomerMaster`; a sales person exists in `tblEmpGeneralInfo`; DC exists (`tblCompanyUnit`); products exist with a price row (`tblUnitPrice`, lookup assumed).

## Steps
1. `OrderListView.AreaLoad()` → `OrderInfoBLL.LoadOrderWithDetail()` (no-arg) → `OrderInfoDAL:592` = `SELECT * FROM tblOrder ⟕ tblCustMaster ⟕ tblCompanyUnit WHERE IsInvoice='0'` (all un-invoiced orders, no paging/filter). Row **Preview** → `LoadOrderWithDetail(orderId)` modal (`OrderListView.aspx.cs:50-116`); **Edit / New** → `ManualOrderCreation.aspx[?OrderId=]`.
2. `ManualOrderCreation.Page_Load`: dropdowns, date, and either `LoadOrderForEdit` (locks the submit button if `IsInvoice`) or blank grid + proposed number (`OrderListBLL.OrdNo()` → `OrderListDAL.OrderManualId`).
3. User selects customer (autocomplete) → `CustomerInfo` fills panel and pre-selects sales person/DC (BR-007).
4. User adds product rows (code/name → price & VAT lookup), quantities, may overwrite TP.
5. Submit → client `validateOrderForm()` → server `Validation()` → `SaveAllData()`.
6. **New:** `OrderListDAL.SaveOrderMaster` → `sp_Save_ManualOrderMaster` (generates `ODR-yyyymmdd<id>` only when code is NULL/''/`ORD-000000%`; snapshots hierarchy; inserts `tblOrder` with `ActionStatus='2'`, `IsInvoice=0`, `IsPrepareforInvoice=1`, `IsManual=1`, `OrderSenderType='Web Manual'`) → `@OrderId` out.
   **Edit:** `sp_Update_ManualOrderMaster` (update header, reset status flags, delete all details).
7. For each valid line: `sp_Save_ManualOrderDetail` (insert `tblOrderDetail`; discount fields 0; `IsSpDis=0`).
8. New ⇒ form cleared + "Order <code> Created Successfully!"; Edit ⇒ redirect to OrderListView.

## Data created / changed
`tblOrder` (1 row insert / update), `tblOrderDetail` (n inserts; on edit all previous rows deleted first).

## Branches / exceptions
- Edit of invoiced order: blocked at page (BR-001) — SP would still allow it.
- Any exception ⇒ alert "Error: <message>"; **no transaction** — a failure after the header call leaves a header with missing/partial lines (edit: header updated and details wiped).
- Row with unresolved product id or qty ≤ 0 silently dropped.
- Duplicate submits (double click) create two orders (no idempotency token).

## Postconditions
Order appears on the proforma list (PAGE-217) for its DC/route/territory if `ActionStatus` ∈ ('2','Accepted','Approved') — WF-M06-02.

## Related data script
`Create_Order_ODR-20260901134.sql` recreates/repairs one such order: if the code exists it resets `IsInvoice=0, IsPrepareforInvoice=1, ActionStatus='2'`, sets GrossValue/TotalNetPayable to 322550.00 and `tblOrderDetail.Status=NULL`; otherwise it inserts the header (DC id 12 / `BD02`, MIO `EMP000055`, customer `C0000134`, `OrderSenderType='Web Manual'`, `IsManual=1`) and 14 detail lines inside one transaction. It shows: (a) production order codes follow `ODR-<yyyyMMdd><OrderId>`; (b) the state triple that makes an order invoiceable; (c) `tblOrderDetail.Status` is the per-line "invoiced/undelivered" marker that must be cleared to re-invoice; (d) data fixes are applied by hand-written scripts (no admin UI).

## Tests
BR-001: edit invoiced order → locked. BR-005: qty 0. BR-008: create when the highest manual code is an `ODR-` code (verify the create page loads). BR-011: edit removes a row. See PAGE-221 §27.
