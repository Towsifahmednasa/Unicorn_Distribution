# WF-M09-02 — Payment-stage confirmation (partial delivery / customer return) making an invoice collectable

Confidence: CONFIRMED for `PaymentPartial.aspx.cs` and `InvoiceDAL`; UNKNOWN for `sp_GET_DelivaryInvoiceNoCheckById` and the upstream pages' internals (only their links to this page were verified).

## Trigger & actors
Depot user after goods have been delivered to the customer: opens the invoice from `DelivaryInvoiceCreationAfterSalesConfirm.aspx` or `SalesReturnList_new.aspx` (they place `Session["InvoiceId"]`), then enters what the customer finally accepted.

## Preconditions
Delivery invoice exists (with `DeliveryQuantity` per line, DC store batches). Payment-stage fields (`PaymentInvoiceNo`) not yet set (BR-M09-021).

## Steps
1. `PaymentPartial.aspx` `Page_Load` → `LoadInvoice(Session["InvoiceId"])` → `OrderInfoBLL.LoadInvoicePartialPayment` → grid; reasons from `tblReturnReason`; `DqtyCalculation()`.
2. User edits per-line DQty; each change → `DqtyCalculation`: status Full/Partial/Reject (BR-M09-023), TP = UnitPrice×DQty, VAT = UnitVAT×DQty, discount % by trade term/campaign (BR-M09-030), NP.
3. **Calculate** button sets the flag (BR-M09-022).
4. **Save** → `Validation` → gate `sp_GET_DelivaryInvoiceNoCheckById(invoiceId,'PaymentInvoiceNo')` (0 rows required) → `SaveInvoice` (`UPDATE tblInvoice` Payment* header, `PaymentInvoiceNo='RTN-'+DelivaryInvoiceNo`) → `SaveInvoiceDetail` per line: `UPDATE tblInvoiceDetail` Payment* columns; for Partial/Reject lines `UPDATE tblDCStore SET StockQty = StockQty + (DelTotalQty − DQty)`.
5. Success alert; optional Print `DelivaryInvoiceReturnViewer.aspx?InvNo=RTN-…`.

## Data created / changed
tblInvoice (PaymentTpTotal, PaymentTpDiscount, PaymentTpVat, PaymentTpGrandTotal, PaymentInvoiceStatus, PaymentInvoiceNo, PaymentBy, PaymentDate); tblInvoiceDetail (PaymentQuantity, PaymentBonusQuantity, PaymentTotalQuantity, PaymentTotalPrice, PaymentTotalPriceVatAmount, PaymentDiscountPercentage, PaymentDiscountAmount, PaymentNetAmount, PaymentStatus, PaymentReturnReason); tblDCStore.StockQty.
Status: invoice payment stage none → Full/Partial/Reject.

## Branches / exceptions
Already confirmed → "Already Exist!" + form cleared. Exception → generic network-error alert (no rollback: header may be updated while some lines/stock are not). Rejected invoices (all lines 0): header status "Reject" and `PaymentInvoiceNo` is still stamped; payable = Σ PaymentNetAmount (0) + `DeliveryCharge` (BR-M09-002), so an invoice with a delivery charge would still appear with Due > 0 in the collection list (INFERRED from the SP formula; not tested). PAGE-335 excludes `DeliveryInvoiceStatus = 'Reject'` invoices.

## Postconditions
Invoice appears in collection list (WF-M09-01) with payable = Σ PaymentNetAmount + DeliveryCharge; returned stock restored to depot.

## Related BR
BR-M09-021…030, BR-M09-001/002.

## Related pages
PAGE-334, upstream DelivaryInvoiceCreationAfterSalesConfirm / SalesReturnList_new (other modules), PAGE-311.

## Tests
TC-334-01…09; plus end-to-end: partial acceptance then verify PAGE-311 Due equals Σ PaymentNetAmount + DeliveryCharge.
