# WF-M10-09 Central store issue for STO picking (weighted-average ledger)

**Module:** M10 data model (workflow owned by the requisition/picking module; documented here because it is the stock-valuation writer for the central store) | **Confidence:** CONFIRMED for the SP body (repo copies) and the DAL call; the picking pages themselves are outside M10.

## Trigger and actors
When central-store stock is picked for a distribution-center requisition (`RequisitionBLL` picking loop), each allocated lot creates a `tblStockInTransfar` row and the SP `tbl_INV_CentralStoreStoIssue` records the issue.

## Steps
| # | Code | Logic |
|---|---|---|
| 1 | `RequisitionBLL` (`Library.BLL/SInventory_BLL/RequisitionBLL.cs:335-381`) | lot allocation from `RequisitionDAL.GetCurrentStockofCentralStock(ProductCode)` (`RequisitionDAL.cs:300-350`): inbound lots = `tbl_INV_Central_BatchWiseStock` rows with `Trans_Qty>0` joined to `tbl_INV_CentralStore_WeightedAvg` by `Weighted_Avg_Id`; less outbound rows (`Trans_Qty<0`) summed by `Issue_Weighted_Avg_Id` + batch; only lots with remaining qty > 0; ordered `ExpDate, ReceiveDate` (FEFO) |
| 2 | same | `StockInTransfarInsertDAL` (INSERT `tblStockInTransfar`), `UpdateCentralStockStockOut(0, receiveId)` (`tblCentralStore.Quantity = 0` for that receive id), `ProcessWeightedAvg(StockInTransfarId)` -> SP `tbl_INV_CentralStoreStoIssue` (`RequisitionDAL.cs:406-414`) |
| 3 | SP `tbl_INV_CentralStoreStoIssue` (`SP_Fix_CentralStoreStoIssue_Corrected.sql`; older `SP_Fix_CentralStoreStoIssue.sql`) | per `tblStockInTransfar` row: opening = latest `tbl_INV_CentralStore_WeightedAvg` row for (Company_Id, Item_Id) ordered `Trans_Date DESC, Weighted_Avg_Id DESC`; opening rate = `Close_Amt / Close_Qty` if `Close_Qty>0` else 0; issue qty `Curr_Qty = -PickingQty`, `Curr_Rate = opening rate`, `Curr_Amt = Curr_Qty x rate`; closing qty/amount = opening + current; new average rate = `Close_Amt / Close_Qty` if `Close_Qty>0` else 0. Idempotence: reuse an existing ledger row with the same `Mst_Trans_Id`, `Ref_No` (requisition no), company, item; then insert one negative `tbl_INV_Central_BatchWiseStock` row (`Remarks='STO Picking'`, `Issue_Weighted_Avg_Id` = inbound lot's ledger id, guarded against duplicates) |
| 4 | Rollback | `RollBackDAL.DeleteStockInTransfer` (`RollBackDAL.cs:60-70`) deletes batch and weighted-average rows by `StockInTransfarId`/`Mst_Trans_Id` and the picking rows |

## Data changed
`tblStockInTransfar`, `tblCentralStore.Quantity`, `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock`.

## Branches / exceptions
- No insufficient-stock test in the SP: closing qty may go negative; the lot-availability logic is in the caller.
- SP references `CSTL_ERPDB..Table_Inventory_Types` (cross-database) and `Table_Inventory_TranscationTypes` (`RespectiveTable='tblStockInTransfar'`).
- Two repo versions of the SP differ (first: inserts batch row against `@Ref_Weighted_Avg_Id`, no duplicate guard; corrected: reuses existing WA row, writes batch row against the new WA id with `Issue_Weighted_Avg_Id` = inbound id) - which is live is UNKNOWN (GAP-M10-016).

## Postconditions
Central weighted-average balance and rate reduced; issue valued at the pre-issue average rate.

## Related
BR-M10-066, 069, 077; GAP-M10-006, 016. Pages of M10 that bypass this ledger: PAGE-389, 392, 381, 394, 390, 376.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF09-T1 | Issue 10 from balance 100 @ 5.00 | new row Curr_Qty -10, Curr_Rate 5.00, Close_Qty 90, Avg_Rate 5.00 |
| WF09-T2 | Re-run for same StockInTransfarId | no second ledger row |
| WF09-T3 | Issue more than balance | Close_Qty negative, avg rate 0 |
