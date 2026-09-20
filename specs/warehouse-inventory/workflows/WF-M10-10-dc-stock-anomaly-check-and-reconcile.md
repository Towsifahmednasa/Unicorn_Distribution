# WF-M10-10 DC store anomaly check and reconcile (operational SP scripts)

**Module:** M10 data-integrity tooling | **Confidence:** CONFIRMED for SP bodies; **no application page calls these SPs** (grep of `*.cs` finds only `CentralStoreDAL.cs` referencing `sp_Report_CentralStore_StockValuationLedger`, not these two) - they are DBA scripts.

## Trigger and actors
DBA/support after invoice rollback/delete problems (see `sp_Delete_ProformaInvoice_Update.sql` comments) suspects inflated or orphaned `tblDCStore` rows.

## Steps
| # | SP | Logic |
|---|---|---|
| 1 | `sp_Check_DCStore_StockAnomalies(@ProductCode=NULL)` (`sp_Check_DCStore_StockAnomalies.sql`) | (a) summary counts: total Issue rows; **orphan Issue rows** = `Trans_Type='Issue'` rows with no `tblInvoiceDetail` where `DCStoreId = Issue.ReferenceDCStoreId AND TotalQuantity = ABS(StockQty)`; valid Issue rows; **corrupted Receive rows** = `Trans_Type='Receive' AND StockQty > TotalQuantity`; (b) list of orphan Issue rows with possible deleted invoice id from `tblInvoiceDetail_DeleterRecord`; (c) list of inflated Receive rows with excess = `StockQty - TotalQuantity` |
| 2 | `sp_Adjust_DCStore_Stock_Only(@InvoiceCode, @ProductCode, @FixCorruptedReceiveQty=1)` (`SP_Adjust_DCStore_Stock_Only.sql`), one transaction with TRY/CATCH | if `@InvoiceCode` given: gather `(DCStoreId, TotalQuantity, ProductCode, BatchNo)` from the invoice details (or `tblInvoiceDetail_DeleterRecord` via `tblInvoiceDeleteLog` when deleted); cursor: `DELETE TOP(1)` from `tblDCStore` where `ReferenceDCStoreId = detail.DCStoreId AND Trans_Type='Issue' AND StockQty = -TotalQty`; optionally `UPDATE ... SET StockQty = TotalQuantity` for that Receive row when `StockQty > TotalQuantity`. Then (if flag=1) **for all rows** `Trans_Type='Receive' AND StockQty <> TotalQuantity` (optionally filtered by product): `SET StockQty = TotalQuantity`. Returns status row |

## Data changed
`tblDCStore` (DELETE Issue rows, UPDATE Receive `StockQty`).

## Branches / risks
- The generic step resets **every** Receive row whose `StockQty <> TotalQuantity`, not only those `>`. In this module StockConditionFreeze, SampleTypeConvertion, OtherStockAction (freeze return raises), StockBatchUpdate and B2B `sp_SAP_B2BChallanDetailInsert` reduce Receive `StockQty` in place; running the SP would restore those quantities to `TotalQuantity` (undoing freezes/samples/transfers) (GAP-M10-005).
- Orphan detection matches on quantity, so two invoices with equal quantity on the same lot can hide an orphan.
- Not idempotent-safe for partial reruns; no backup/audit table written.

## Postconditions
Consistent quantity model only if all stock movements use ledger rows.

## Related
BR-M10-070..073; GAP-M10-005, 017.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF10-T1 | Delete an invoice manually leaving its Issue row | check SP lists it as orphan |
| WF10-T2 | Run adjust with invoice code | Issue row deleted, receive StockQty aligned |
| WF10-T3 | Run adjust general fix after a StockConditionFreeze | freeze deduction undone (defect) |
