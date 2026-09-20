# WF-M11-03 — DC → Warehouse return challan: create, transfer, warehouse receive

**Confidence:** CONFIRMED (PAGE-408, PAGE-409 handler code, PAGE-414, DALs).

## Trigger & actors
DC user creates the return challan (PAGE-408); a DC/ops user presses *Transfer* in `DepoToWhTransferView` (PAGE-409); warehouse user receives (`StockRcvByWH` → PAGE-414).

## Preconditions
DC has stock (`tblDCStore` rows `StockQty>0`) or freeze stock (`tblDCStoreFreeze`); no other untransferred challan exists for the DC (BR-M11-022).

## Steps
| # | Step | Page → DAL → SQL |
|---|---|---|
| 1 | Create challan (lines from stock or freeze stock, purpose per line) | PAGE-408 → `SCtoWHTransferDal.SaveDataForChalanInfo/Detail` → `tblDepotToWHChalanInfo` (IsDeliver 'False', IsTransfer NULL), `tblDepotToWHChalanDetail` |
| 2 | Optional delete of pending challan | PAGE-409 `DeleteData` → `sp_DeleteDwpotToWhChalan` |
| 3 | Transfer stock | PAGE-409 `TransferData` → `UpdateDCStockQuantity`/`UpdateFreezeQuantity` (`StockQty := StockQty − Quantity`) → `UpdateChalanMasterStatus` (`IsTransfer='OK'`) |
| 4 | Warehouse lists pending challans | `StockRcvByWH.aspx` → `SCtoWHTransferDal.GetWarehouseRcvStock` (`IsDeliver != 'OK'`) |
| 5 | Warehouse receives | PAGE-414 `Save` → `StockRcvByWhDal.CentralStorStockIn` (`tblCentralStore` row) or `WhStockConditionFreezeBll` (`tblWhStockConditionFreeze`, `tblWhStoreFreeze`); `updateChallanStatus` (`IsDeliver='OK'`) |
| 6 | Print challan | `DcToWHChalanReportViewer.aspx?chalanno=` → `SCtoWHTransferDal.ChalanReport` → `crpDCToWhChalan.rpt` |

## Data created / changed
tblDepotToWHChalanInfo/Detail; tblDCStore or tblDCStoreFreeze (StockQty decrement); tblCentralStore (+row) or tblWhStoreFreeze/tblWhStockConditionFreeze (+rows).

## Stock effects
DC: in-place decrement of the chosen row at step 3 (not at save). Warehouse: new central-store lot for received qty (normal stock) at step 5; damage/short and freeze-sourced lines to WH freeze stock. Between step 3 and 5 goods are in transit (no ledger, only `IsTransfer='OK'`, `IsDeliver` not 'OK').

## Valuation impact
Warehouse lot values = original STO price/VAT of the DC row × **issued** quantity (BR-M11-039); central weighted-average ledger not updated (INFERRED).

## Accounting posting implications
None in repo code.

## SAP hand-off states
None for this flow (no SAP calls or flags).

## Branches / exceptions
Transfer with insufficient stock → negative StockQty (no check); partial failure at transfer → status set from last row only; WH receive partial failure → duplicates on retry (GAP-M11-010); challan print key wrong (GAP-M11-008).

## Postconditions
Challan `IsTransfer='OK'` and `IsDeliver='OK'`; central/freeze stock increased.

## Related BR ids
BR-M11-022…028, 036…040.

## Related pages
PAGE-408, 409 DepoToWhTransferView (B), 414, 415 StockRcvByWH (B), 400 DcToWHChalanReportViewer.

## Tests
PAGE-408 §27 T1-T8; PAGE-414 §27 T1-T7.
