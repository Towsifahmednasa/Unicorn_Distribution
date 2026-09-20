# WF-M11-02 — DC ↔ DC (B2B) transfer: send, in-transit, receive, delete

**Confidence:** CONFIRMED (PAGE-412 code, `ChalanBLL/ChalanDAL`, `TransferReceiveProductByChalanByDC` code, both SP versions of `sp_Del_B2BDelete`).

## Trigger & actors
Sending DC user (PAGE-412), receiving DC user (PAGE-423 `TransferReceiveProductByChalanByDC`, listed by `TransferStockReceiveByDC`, `ChalanLoadInReceive(comUnitId)`), supervisor deleting a pending challan (PAGE-407 `B2BTransferView`).

## Preconditions
Source DC has batch stock (Σ StockQty of Receive+Issue rows > 0) and an active `tblUnitPrice`; destination unit exists in `tblCompanyUnit` (matched by `ComUnitCode`).

## Steps
| # | Step | Page → BLL → DAL → SQL |
|---|---|---|
| 1 | Enter from/to unit codes, search product, choose batches and qty, Add | PAGE-412 → `ChalanBLL.ComUnit`, `GetProductDcStore` |
| 2 | Save challan | `SaveDataForChalanInfo` → `tblChalanInfo` (IsDeliver 'False', number `IDT…`); `SaveDataForChalanDetail` → `tblChalanDetail` |
| 3 | Issue stock at source | per detail `ChalanDAL.UpdateDCStockQuantity` → INSERT `tblDCStore` **Issue** row (`StockQty=−qty`, `B2BIssue`, `ReferenceDCStoreId`, `ChalanDetailsId`) |
| 4 | In transit | challan `IsDeliver='False'`, appears as "Pending" in `B2BTransferView`; destination lists it via `tblChalanInfo ⨝ tblCompanyUnit(ComUnitCode=ToComUnitCode)` |
| 5 | Receive | PAGE-423 `Save` → `ChalanBLL.DCStockIn2` → `ChalanDAL.DCStockInDALMain` INSERT `tblDCStore` **Receive** row (`B2BReceive`, `TotalQuantity=StockQty=received`, cost from source row / active price, `ReferenceDCStoreId=source`) then `ChalanUpdate` → `tblChalanInfo.IsDeliver='True'`; damage → `tblDCStoreFreeze` Restricted; `MakeRESTRequest` (swallowed) |
| 6 | Alternative: delete pending challan | PAGE-407 `editImageButton_Click` (allowed if status ≠ "Received") → `DeleteChallanMasterById` (deletes `tblChalanInfo`) then `sp_Del_B2BDelete(@chalanId)` |

## Data created / changed
tblChalanInfo, tblChalanDetail, tblDCStore (Issue row at source; Receive row at destination), tblDCStoreFreeze (damage). On delete: v2 SP deletes Issue and B2BReceive ledger rows and both challan tables; v1 SP adds quantity to source rows and deletes challan tables only.

## Stock effects
Source: −qty via Issue row at send (available immediately drops). Destination: +received qty at receive. Delete (v2) restores source by removing the Issue row; (v1) inflates the Receive row while the Issue row remains (GAP-M11-001). The SAP-path challans decrement `StockQty` in place instead (WF-M11-04).

## Valuation impact
Issue row cost = source `CostPerUnit`; destination cost = source cost, else active `tblUnitPrice.CostPrice`, else 0. Challan value uses `UnitPrice` (trade price) — separate from cost.

## Accounting posting implications
None found for DC↔DC in repo code (inter-DC transfers post no voucher); challan totals (`TotalValue/TotalVat/GrandTotal`) are for the printed challan only.

## SAP hand-off states
Challans created here have `SAP_ChallanSend`/`SAP_Challan_ConfirmationSend` unset; the flag SPs may be run by an external job (UNKNOWN). Receipt triggers `MakeRESTRequest` (no flag).

## Branches / exceptions
- Received challan cannot be deleted from the UI ("Delete operation is not possible !!"), but v2 SP can also remove B2BReceive rows if invoked otherwise.
- Master deleted before SP: a failure in the SP leaves details/issue rows orphaned.
- Receive has no duplicate guard in `DCStockInDALMain`; protection is only that the challan disappears from the pending list after `ChalanUpdate`.
- No server stock re-check at send.

## Postconditions
Challan `IsDeliver='True'`, destination stock added, damage restricted; or challan removed.

## Related BR ids
BR-M11-001…014.

## Related pages
PAGE-412, 407 (B2BTransferView), 423 (TransferReceiveProductByChalanByDC), 424 (TransferStockReceiveByDC), 419 (StockTransferDcToDc legacy), 399/405/425 viewers.

## Tests
PAGE-412 §27 T1-T10.
