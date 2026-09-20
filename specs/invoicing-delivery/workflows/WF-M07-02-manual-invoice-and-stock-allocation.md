# WF-M07-02 - Invoice save and FEFO stock allocation (InvoiceBLL path)

**Trigger:** "Invoice Generate" on `InvoiceCreationForCustomer` (PAGE-267); the same BLL methods are used by the M06 by-order pages. **Actors:** DC operator.
**Preconditions:** order code typed (customer info from `View_OrderCustomerInfo`), products with `tblUnitPrice`, stock as `tblDCStore` Receive rows.

## Steps
1. `saveButton_Click` -> `Validation()` (order no/date/blank rows) (BR-M07-006).
2. `SaveInvoice()` -> `InvoiceBLL.SaveInvoice`: `PrimaryKeyMax` invoice id; `InvoiceNoGenerator` = `INV-<ComUnitCode><MAX(InvoiceId)+1 padded 10>` (BR-M07-011); batch id (BR-M07-012) -> `INSERT tblInvoiceBatch`; `INSERT tblInvoice` (43 cols).
3. `SaveInvoiceDetail` -> `InvoiceBLL.SaveInvoiceDetails`: for each line `UpdateDCStock`: `InvoiceDAL.BatchWiseProductQty` (FEFO: Receive rows with available>0 ordered ExpDate, BatchNo) -> per batch `InvoiceDetailInsertBLL` (`Isgift` lookup, price/VAT/discount/net formulas BR-M07-015/016) -> `INSERT tblInvoiceDetail` and `InvoiceDAL.UpdateDCStoreQuantity` -> `INSERT tblDCStore` Issue row (negative StockQty, ReferenceDCStoreId).
4. `UpdateInvoiceHeaderTotals`: header = SUM of details (+DeliveryCharge, -AdjustAmount) (BR-M07-017).
5. `Clear()`, alert; Print opens `InvoiceReportViewer`.

## Data changed
tblInvoice, tblInvoiceBatch, tblInvoiceDetail, tblDCStore(Issue). Order flags are set by M06 pages (not here).

## Exceptions
- PAGE-267 fails at step 3 (`Isgift(0)` on empty result): header and batch remain, no details, no Issue rows (BR-M07-019).
- No transaction at any step (BR-M07-018); stock shortage silently invoices only what exists (BR-M07-014).
- Concurrency: max+1 keys (BR-M07-011).

## Postconditions (intended)
Proforma invoice with batch-level detail lines and reserved stock. **Related:** BR-M07-006..019; PAGE-267; GAP-M07-004/005. **Tests:** TC-267-06/08/09/10/12.
