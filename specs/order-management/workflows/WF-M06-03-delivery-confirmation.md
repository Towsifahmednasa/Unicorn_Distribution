# WF-M06-03 — Delivery confirmation of a proforma invoice (Full / Partial / Reject)

**Pages:** `LoadingSummary.aspx` (M07, launcher), PAGE-215 `…Auto_Loading` (live), PAGE-214 `…Auto` (legacy variant). **BR:** BR-M06-049…062. **Cross-link:** M07 (invoice/delivery), M08 accounts posting.

## Trigger & actors
Dispatcher/loading clerk after goods are delivered; enters what the customer accepted.

## Preconditions
Proforma exists (`tblInvoice.DelivaryInvoiceNo IS NULL`). `LoadingSummary` row action stores `Session["InvoiceId"]` and redirects to PAGE-215 (`LoadingSummary.aspx.cs:271-272`; the PAGE-214 redirect is commented out).

## Steps (PAGE-215)
1. `Page_Load` → `LoadInvoice(id)` (proforma + lines) → grid, totals, reason lists.
2. Clerk edits `dQty` per line → `DqtyCalculation`: Full / Partial / Reject per line (BR-049); discount/VAT recomputed for the delivered qty; totals updated.
3. **Save** → `Validation` (blank/`Reject`-without-reason; note `"Parial"` typo).
4. Duplicate guard `sp_GET_DelivaryInvoiceNoCheckById(invoiceId,'DelivaryInvoiceNo')` — rows ⇒ "Already Exist!".
5. `SaveInvoice`: header status roll-up; `InvoiceDAL.UpdateInvoice` (Delivery* columns, `DelivaryInvoiceNo='DEL-'+no`) and `PaymentUpdateInvoice` (Payment* columns, `PaymentInvoiceNo='RTN-'+no`).
6. `SaveInvoiceDetail`: per line `ReturnToDCStore(tQty−dQty, DCStoreId, InvoiceDetailId)` for Partial/Reject (insert `tblDCStore` Receive row 'DeliveryReturn'), `UpdateInvoiceDetail` + `PaymentUpdateInvoiceDetail`.
7. `PostDeliveryConfPartialJournal(invoiceId)` → `sp_Sales_DeliverySalesPosting` (accounts posting, UNKNOWN).
8. Success alert, form cleared.

## Legacy variant (PAGE-214)
Same, but: no duplicate guard, no journal posting, no `Payment*` update, delivery number double-prefixed (`DEL-DEL-`), and a **forced auto full-cash payment** (`SavePayment`).

## Data changed
`tblInvoice` (Delivery*, Payment*), `tblInvoiceDetail` (Delivery*, Payment*, ReturnReason), `tblDCStore` (Receive rows), accounts DB (posting), customer payment tables (214 only).

## Exceptions
Any exception ⇒ generic "Some Networks Error…" and possible half-updated state (header done, details/posting missing); retry blocked by duplicate guard (PAGE-215).

## Postconditions
Invoice delivered; receivable basis = `DeliveryTpGrandTotal`; used by credit-limit `Due` calc (WF-M06-02 step 2) and payments (M08).

## Tests
PAGE-215 TC-01..08; PAGE-214 TC-01..05. E2E: invoice 10 units → deliver 6 → Partial, stock +4 (Receive row), due = delivered value.
