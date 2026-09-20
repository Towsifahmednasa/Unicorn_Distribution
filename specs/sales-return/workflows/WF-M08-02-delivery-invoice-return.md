# WF-M08-02 — Delivery invoice return (return against a delivered invoice)

**Confidence:** CONFIRMED (code + `sp_DeliverySalesReturnPosting` body).

## Trigger & actors
Depot user has a Delivery Invoice No whose goods (all or part) came back. No approval role in the flow.

## Preconditions
Invoice exists with `DelivaryInvoiceNo`, order is non-direct (`IsDirect` 0/NULL), invoice not present in `tblCustomerReturnMaster` (BR-M08-031); lines have `DeliveryTotalQuantity`.

## Steps
| # | Step | Page → handler → BLL → DAL → SQL |
|---|---|---|
| 1 | Enter/search delivery invoice no | PAGE-287 `Button2_Click` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` (concatenated SQL) |
| 2 | Header labels filled | `LoadInvoice` → `OrderInfoBLL.LoadInvoice`, `GetCompanyUnitById`, `GetDANameById`, `GetOrderSenderNameByOrderId` |
| 3 | Enter return qty per line → amounts/totals; optional deduction | `retQtyTextBox_OnTextChanged`, `CalculateReturnTotals` |
| 4 | Save Return (confirm) | `saveButton_Click` → `Validation` |
| 5 | Master | `SaveCustomerReturn` → `tblCustomerReturnMaster` (`ActionStatus='Posted'`) |
| 6 | Per line with qty>0 | `SaveCustomerReturnDetail` → `tblCustomerReturnDetail`; `InsertDCStoreStockReturn` → `tblDCStore` Receive row |
| 7 | Post | `PostDeliverySalesReturn` → `sp_DeliverySalesReturnPosting` |

## Data created / changed
`tblCustomerReturnMaster` (+1, IsPosting→1), `tblCustomerReturnDetail` (+n), `tblDCStore` (+n, StockCondition 'SalesReturn', TotalQuantity 0, StockQty=qty, ReferenceDCStoreId=original, SalesReturnDetailId, ReturnFromInvoiceDetailId), `CSTL_ACCDB_Unicorn.tblDebitCreditVoucherMaster/Detail` (+2 vouchers).

## Stock effects
New Receive ledger row per returned line at the invoice's original batch (copy of original row's StorageLocation, batch, expiry, cost). **Only if** the original row (via `tblInvoiceDetail.DCStoreId`) is `Trans_Type='Receive'`; otherwise nothing is inserted though the return is recorded (BR-M08-036). Original row untouched.

## Valuation impact
Ledger row carries the original `CostPerUnit`; the GL inventory voucher is valued differently — at `tblCentralStore.UnitPrice + VATPerUnit` (transfer price incl. VAT) via `tblDCStore.TempReceiveId` (BR-M08-038) — so ledger cost and GL valuation can differ.

## Accounting posting implications
Voucher 1 (TableID 2018): Dr Sales Return (23/1020→4170) = total return incl. VAT; Cr Delivery Charge Income (23/1032→5968) = deduction; Cr AR (23/2→2112, customer subsidiary) = net. Voucher 2 (TableID 2023) posted only if central-store valuation > 0: Dr FG Inventory (23/1016→4156) / Cr COGS (23/1021→4162). `EntryBy='admin'`. No idempotency: re-running the SP duplicates vouchers (BR-M08-039). Silent no-posting if invoice not found or total ≤ 0.

## Branches / exceptions
- Invoice already returned / not found → "Already Returned or Payment Exist or Invalid Delivery Invoice No. !!!".
- Deduction > total → blocked.
- SP failure after stock insert → return + stock exist without vouchers (`IsPosting` NULL); no retry UI.
- Approval methods exist (`ApprovalUpdateDAL`, `LoadCustomerReturnApproval`) but no page uses them.

## Postconditions
Invoice can never be returned again through this page; stock increased; two approved journals.

## Related BR ids
BR-M08-031…039.

## Related pages
PAGE-287; reports RPT-M08-02 (delivery return report family); PAGE-288 (delete invoice) is the opposite operation for undelivered invoices.

## Tests
PAGE-287 §27 T1-T10. Add: return an invoice whose `DCStoreId` row is an 'Issue' row → confirm no stock row; return with `DeliveryNetAmount` including VAT → compare voucher total with invoice net.
