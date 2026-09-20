# WF-M10-04 Stock condition freeze (quarantine) and release

**Module:** M10 | **Confidence:** CONFIRMED for code paths; content of `tblStockCondition` UNKNOWN.

Four cooperating pages: `StockConditionPermission` (who may use which condition), `StockConditionFreeze` (DC and, in legacy mode, central WH), `WhStockConditionFreeze` + `WhFreezeStockRelease` (central WH, dedicated tables), `OtherStockAction` (DC release).

## Trigger and actors
Stock controller finds damaged/expired/disputed goods and moves them out of sellable stock; later a controller releases them. Administrator grants condition permissions.

## Preconditions
Conditions in `tblStockCondition` other than `Available`; the user has `tblStockConditionPermission.Permission=1` for the condition; the stock row is `StockCondition='Available'` with quantity > 0.

## Steps - permission
| # | Page | Logic |
|---|---|---|
| 0 | `StockConditionPermission.aspx` | choose user; checkboxes of `tblStockCondition`; save -> for each **checked** item delete existing (UserId, StockConId) row and INSERT with `Permission=true` (`StockConditionPermissionBll.LoadStockPermissionDataForSave`). Unchecked items are never revoked (BR-M10-054). |

## Steps - DC freeze and release
| # | Page | Logic |
|---|---|---|
| 1 | `StockConditionFreeze.aspx` (DC mode) | search `tblDCStore` rows (`StockCondition='Available'`, `StockQty>0`, `ComUnitId`) (BR-M10-027) |
| 2 | same | per row: condition (permitted list, BR-M10-028), qty, remarks; `loadGridView_RowCommand` / `submitButton0_OnClick` |
| 3 | same | checks (BR-M10-030); INSERT `tblStockConditionFreeze`; INSERT `tblDCStoreFreeze`; `UPDATE tblDCStore SET StockQty = StockQty - qty` (BR-M10-031) |
| 4 | `OtherStockAction.aspx` | search `tblDCStoreFreeze` (`StockQty>0`, DC, manufacturer); stage rows; submit -> `tblDCStoreFreeze.StockQty -= qty`, then `tblDCStore.StockQty += qty` on the source `DCStoreId` (BR-M10-034..037) |

## Steps - central WH freeze and release
| # | Page | Logic |
|---|---|---|
| 5a | `WhStockConditionFreeze.aspx` | warehouse selected; rows `tblCentralStore` (`Available`, `Quantity>0`); select rows, condition, qty; INSERT `tblWhStockConditionFreeze` (ReceiveId, FreezeQty, EntryBy/Date), INSERT `tblWhStoreFreeze` (copy + StockQty, StockCondition, Remarks, WhStockConditionFreezeID), `tblCentralStore.Quantity -= qty` (BR-M10-055) |
| 5b | `WhFreezeStockRelease.aspx` | list `tblWhStoreFreeze` (`StockQty>0`); return qty must be integer, >0 and <= displayed freeze qty; submit -> `tblWhStoreFreeze.StockQty -= qty`, `tblCentralStore.Quantity += qty` by `ReceiveId` (BR-M10-056) |
| 5c | `StockConditionFreeze.aspx` (Central WH mode, session `CentralWareHouse=True`) | legacy variant writing to `tblDCStoreFreeze`; those rows cannot be released by 4 or 5b (BR-M10-032, GAP-M10-009) |

## Data changed
Freeze headers `tblStockConditionFreeze` / `tblWhStockConditionFreeze`; freeze detail `tblDCStoreFreeze` / `tblWhStoreFreeze`; live stock `tblDCStore.StockQty` / `tblCentralStore.Quantity` (in-place). Freeze rows are never deleted; release only lowers `StockQty` (0 = released).

## Negative stock / freeze enforcement
- UI compares entered qty with the displayed integer-truncated stock; no server re-check at write time; no DB constraint evidence.
- Sales/valuation read `StockCondition='Available'` rows and `StockQty>0`, so quarantined quantity disappears from sellable lists because it is removed from the stock row (not by a flag).
- Reports show frozen stock as Restricted / Blocked from `tblDCStoreFreeze.StockCondition` (RPT-M10-02).

## Branches / exceptions
Qty > stock -> message only; qty blank/0 -> message; bulk submit still reports success (BR-M10-033); no transaction; lost updates under concurrency.

## Postconditions
Frozen qty tracked; release restores quantity to the source row.

## Related
BR-M10-027..037, 054..056; PAGE-381, 382, 394, 390, 375; GAP-M10-005, 009, 022.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF04-T1 | Freeze 5 of 20 (DC) | stock 15, freeze 5 |
| WF04-T2 | Release 5 (OtherStockAction) | freeze 0, stock 20 |
| WF04-T3 | User without permission | no condition selectable |
| WF04-T4 | Uncheck a condition on permission page and save | permission remains (BR-M10-054) |
| WF04-T5 | Central WH freeze via StockConditionFreeze | not releasable by 4/5b |
| WF04-T6 | Central WH freeze via WhStockConditionFreeze, release via WhFreezeStockRelease | stock restored |
