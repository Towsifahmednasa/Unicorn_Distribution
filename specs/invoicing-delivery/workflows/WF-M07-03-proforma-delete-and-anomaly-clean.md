# WF-M07-03 - Proforma deletion and anomaly auto-clean

**Trigger:** (a) Search on `TopSheetGenerate` (PAGE-277); (b) M06 pages `InvoiceCreationByOrder` (`:131,:259`) and `InvoiceCreationForCustomerByOrder` (`:3140`) (print/lookup); (c) manual `ProformaorInvoiceReturn.aspx.cs:57`. **Actor:** any user with the page (no role check).
**Precondition:** invoice exists; deletion only effective when `DelivaryInvoiceNo IS NULL`.

## Steps
1. Page loads invoices (e.g. `StockConditionFreezeDAL.LoadInvoiceByDAName`).
2. Per invoice: `CommonDataLoad.Check_anomalyInvoiceDetails(InvoiceId, OrderId)` -> `sp_Check_anomalyInvoiceDetails` (`SP_Fix_Invoice_Anomaly_And_Sync.sql:42`): returns a row when date > 2022-11-16 and ABS((TpGrandTotal - AdjustAmount) - SUM(detail NetAmount)) > 10 (BR-M07-036).
3. If a row: `ProformaOrInvoiceReturnBLL.DeleteProforma(InvoiceNo)` -> `ProformaOrInvoiceReturn.DeleteProformaDal` (adds `@User = Session["LoginName"]`) -> `sp_Delete_ProformaInvoice` (`sp_Delete_ProformaInvoice_Update.sql:7`):
   - guard `DelivaryInvoiceNo IS NULL` (BR-M07-037);
   - cursor per detail: delete top-1 tblDCStore Issue row with matching ReferenceDCStoreId and StockQty = -TotalQuantity; fix Receive rows with StockQty > TotalQuantity (BR-M07-038); insert tblProInvoiceReturnTrack and tblInvoiceDetail_DeleterRecord; delete detail;
   - `tblOrderDetail.Status = NULL`, `tblOrder.IsInvoice = 0` for the last OrderId; delete tblReturnAmount, tblInvoice; insert tblInvoiceDeleteLog (BR-M07-039).
4. User sees stale grid (bound before delete) until next search; M06 pages show "Please generate Invoice again!".

## Postconditions
Order can be re-invoiced; stock restored via Issue-row deletion; log rows exist (report `DeleteInvoiceReport` reads them, see GAP-M07-016).

## Exceptions / risks
No transaction (partial deletes); exact-quantity match can orphan Issue rows; zero-detail invoice leaves order flagged; DeliveryCharge/AdjustAmount can trigger false positives (GAP-M07-015..018). Remediation scripts: `SP_Adjust_DCStore_Stock_Only.sql`, `sp_Check_DCStore_StockAnomalies.sql`.

**Related BR:** 035-039. **Pages:** PAGE-277, M06 PAGE-217/218, PAGE-240/263 (report). **Tests:** TC-277-03..08.
