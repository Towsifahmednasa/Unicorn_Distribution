# WF-M12-01 DC -> Sub-depot stock transfer (send and receive)
**Actors**: DC store user (PAGE-450), sub-depot user (PAGE-449 list, PAGE-446 receive). **Trigger**: sub-depot needs stock.
**Preconditions**: sub-depot registered under DC (BR-M12-001); DC batch stock exists; active unit price.
**Steps**
1. PAGE-450 `Button1_Click` -> `SubDepotChalanBLL.GetProductDcStoreSubdeport` -> SELECT tblDCStore (BR-M12-005).
2. Tick rows + qty, `addButton_Click` (BR-M12-006/007); Submit -> `Validation` (BR-M12-008).
3. `SaveDataForChalanInfo`: id=MAX+1, no `SDC`+id; INSERT `tblSubDepotChalanInfo` IsDeliver=false (BR-M12-004/009).
4. `SaveDataForChalanDetail` per line: INSERT `tblSubDepotChalanDetail`; `tblDCStore.StockQty = current - qty` (BR-M12-010).
5. Chalan visible in `StockReceiveBySubDepot.aspx` (IsDeliver False/NULL for sub-depot, BR-M12-013) -> link `ProductReceiveBySubDeport.aspx?ChalanId=`.
6. PAGE-446 `Save`: per line INSERT `tblSubDepotStore` (Available), `UPDATE tblSubDepotChalanInfo SET IsDeliver='True'`, damage>0 -> INSERT `tblSubDepotStoreFreeze` Restricted (BR-M12-014..017); report popup (RPT-M12-02).
**State machine**: none -> IsDeliver=false (in transit) -> IsDeliver=true. **Branches/exceptions**: qty>stock only checked client-side; failures mid-way leave partial data; no cancel/reject; double receive possible.
**Postconditions**: DC stock reduced; sub-depot Available stock (+Restricted freeze) increased; chalan closed. **Tests**: PAGE-450 TC-01..10, PAGE-446 TC-01..08. **Reports**: RPT-M12-01, RPT-M12-02, RPT-M12-06 (In-transit bucket).
