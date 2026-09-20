# WF-M12-02 Sub-depot -> DC return transfer
**Actors**: sub-depot user (PAGE-451), DC user (PAGE-442 list, PAGE-441 receive).
**Steps**
1. PAGE-451 search `SubDepotChalanReturnDAL.GetProductDcStoreSubdeport` - all sub-depots' stock (BR-M12-012).
2. Submit: INSERT `tblSubDepotChalanReturnInfo` IsDeliver=false + `tblSubDepotChalanRetuenDetail` (DCStoreId column holds SubDCStoreId, BR-M12-049); `tblSubDepotStore.StockQty = current - qty` (BR-M12-011).
3. DC list `SubDeportTransferStockReceiveByDC` -> `ChalanBLL.SubdeportChalanLoadInReceive` returns all pending returns (BR-M12-019).
4. PAGE-441 `Save`: per line `ChalanBLL.DCStockInSub` -> INSERT `tblDCStore` (ComUnitId=6, Available, no cost) + `UPDATE tblSubDepotChalanReturnInfo IsDeliver='True'`; damage -> `tblDCStoreFreeze` Restricted (BR-M12-018/020).
**State machine**: none -> IsDeliver=false (sub-depot stock already reduced) -> IsDeliver=true. **Exceptions**: delete-row error (BR-M12-050), print mismatch (BR-M12-048), wrong DC (BR-M12-018). No in-transit bucket in stock report for returns.
**Together with WF-01 the full cycle DC -> Sub-depot -> DC is: DC stock down / sub-depot up (two-step) then sub-depot down / DC up (two-step), each with an in-transit flag and no reconciliation.** Tests: PAGE-451 TC-01..08, PAGE-441 TC-01..07.
