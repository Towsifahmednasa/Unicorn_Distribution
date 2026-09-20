# WF-M08-03 — Second-time (post-payment) partial return

**Confidence:** CONFIRMED for the page code; `sp_BeforesndSalesReturnList`, `sp_GET_DelivaryInvoiceNoCheckById`, `sp_PaymentConformationFull` bodies UNKNOWN.

## Trigger & actors
Depot/collection user finds an invoice with "partial dues" after the payment confirmation stage and records what the customer finally accepted.

## Preconditions
Invoice has `PaymentInvoiceNo` (payment-confirmed) and `SndReturnInvoiceNo` is NULL; route/territory known (BR-M08-046).

## Steps
| # | Step | Page → handler → DAL → SQL |
|---|---|---|
| 1 | Choose DC → routes; route → territories; optional invoice text; Search | PAGE-296 `Button1_Click`→`GridView()`→`SalesReturnNewDAL.LoadOrderWithInvoiceSP`→`sp_BeforesndSalesReturnList(@param)` |
| 2 | Click *Go to Partial* on the row | `gotoinvoiceButton_Click` → `Session["InvoiceId"]` → redirect |
| 3 | Invoice lines load; reasons load | PAGE-298 `Page_Load`→`LoadInvoice`→`OrderInfoDALSalesReturn.LoadInvoicePartialPayment` |
| 4 | Edit DQty per line; **Calculate** | `lnkCalculate_Click`→`CalculateQty`,`DqtyCalculation` |
| 5 | Generate Invoice (SweetAlert confirm) | `saveButton_Click`→ duplicate guard `sp_GET_DelivaryInvoiceNoCheckById(invoiceId,'SndReturnInvoiceNo')` |
| 6 | Header update | `SaveInvoice`→`SalesReturnDAL.PaymentUpdateInvoice` → `tblInvoice.sndReturn*`, `SndReturnInvoiceNo='SRN-'+InvoiceNo` |
| 7 | Line update | `SaveInvoiceDetail`→ (Partial/Reject) `UpdateDCStock` → `tblDCStore.StockQty += PaymentQty − DQty` on the invoice's original batch row; `sp_UpdateAndInsertInvoiceDetailSalesReturn` → `tblInvoiceDetailReturn` |
| 8 | Print (optional) | `printButton_Click` → `DelivaryInvoiceReturnViewer.aspx?InvNo=RTN-<InvoiceNo>` |

## Data created / changed
`tblInvoice` (sndReturn* header columns, SndReturnInvoiceNo, SndReturnPaymentBy/Date), `tblInvoiceDetailReturn` (+1 per line), `tblDCStore` (UPDATE `StockQty` on existing row for Partial/Reject lines).

## Stock effects
**In-place update** of the original Receive (batch) row: `StockQty = StockQty + (PaymentQuantity − DQty)`. No ledger row. This differs from all other return flows (which insert rows) and from invoice issue (which inserts negative 'Issue' rows). If the invoice's issue row remains, the Receive row can exceed `TotalQuantity` (GAP-M08-006).

## Valuation impact
None recorded here; the quantity returns at the batch's existing cost (unchanged row) — no `CostPerUnit` recomputation, no valuation voucher.

## Accounting posting implications
**None** in this flow (no call to any posting SP). Whether receivable/COGS are adjusted from `sndReturn*` figures elsewhere is UNKNOWN (GAP-M08-009).

## Branches / exceptions
- Missing Calculate → blocked. Missing reason on Reject → blocked; on Partial → not enforced (typo).
- Duplicate → "Already Exist!".
- Any exception → generic alert; header may be updated while details are not (header saved first).
- "ALL Submit"/Full path on PAGE-296 is dead.

## Postconditions
Invoice disappears from the worklist (`SndReturnInvoiceNo` set); `sndReturnInvoiceStatus` ∈ {Full, Partial, Reject}.

## Related BR ids
BR-M08-019…030, 046…049.

## Related pages
PAGE-296, PAGE-298; reports RPT-M08-04, RPT-M08-05.

## Tests
PAGE-298 §27 T1-T8; PAGE-296 §27 T1-T6.
