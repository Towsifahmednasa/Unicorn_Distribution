# WF-M13-01 Sales operation rollback (proforma / delivery / payment)

_Module M13, reverse-engineered 2026-09-19 (documentation only). Confidence tags as in the page specs._

## Trigger and actors
An authorised (menu-permitted, no code-level role check) user opens `SalesOperationRollback.aspx` and submits a document number and a reason.

## Preconditions
Session `LoginName` present; document exists; guards below. Order of undo: payment -> delivery -> proforma.

## Steps
1. `SalesOperationRollback.aspx.cs` `btnRollback_Click` validates code, remarks, session (BR-M13-001) -> confirm() in browser.
2. Dispatch by radio to `SalesOperationRollbackBLL.RollbackProforma | RollbackDeliveryInvoice | RollbackCustomerPayment` -> `SalesOperationRollbackDAL` (own connection, one SqlTransaction).
3. **Proforma** (BR-M13-002..004): select tblInvoice by InvoiceNo -> refuse if DelivaryInvoiceNo set -> INSERT tblInvoiceDeleteLog('PROFORMA') -> tblOrder.IsInvoice=0, tblOrderDetail.Status=NULL -> per detail line reverse tblDCStore Issue row (rules 1-3) and delete tblDCStoreFreeze -> delete tblReturnAmount, tblInvoiceDetail, tblInvoice.
4. **Delivery** (BR-M13-005..008): select by DelivaryInvoiceNo -> refuse if tblCustPayDetail exists -> for lines with Quantity-DeliveryQuantity>0 verify SUM(tblDCStore.StockQty) per product/batch/expiry -> INSERT negative Issue row -> copy vouchers (CommonPK_ID=InvoiceId, TableID=2014) as reversal with Dr/Cr swapped into CSTL_ACCDB_Unicorn -> INSERT tblInvoiceDeleteLog + tblInvoiceDetail_DeleterRecord -> UPDATE tblInvoice/tblInvoiceDetail delivery columns to NULL/0.
5. **Payment** (BR-M13-009): select tblCustomerPay by CollectionNo -> reverse vouchers (TableID=2017) -> per tblCustPayDetail: tblInvoice.PaymentStatus/PaymentAmount=NULL + `sp_InsertCustPayDetail_DeleteLog` -> delete tblCustPayDetail, tblCustomerPay.
6. Commit -> "SUCCESS" -> page alert "Rollback operation successfully completed!" and clears inputs; on exception rollback and alert "Error: ..." (BR-M13-010).

## Data changed / status transitions
See `pages/PAGE-528-salesoperationrollback.md` sections 17 and 21: order invoiced -> not invoiced; delivery Full/Partial -> NULL; payment settled -> NULL; stock Issue rows added/removed; accounting reversal vouchers created; logs written.

## Branches / exceptions
Not found; delivery already exists; payment exists; insufficient stock; any SQL error (full rollback).

## Postconditions
Consistent state one step earlier in order-to-cash; delete logs present (without the entered reason for invoice logs).

## Related BR / pages / tests
BR-M13-001..010; PAGE-528; tests TC-01..TC-12 in the page spec. Risks: GAP-M13-004, -009, -010.

