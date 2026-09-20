# WF-M10-05 Central warehouse stock adjustment (and its reversal)

**Module:** M10 | **Confidence:** CONFIRMED.

## Trigger and actors
Warehouse controller writes down central stock after a count/damage: `WHStockAdjustmentEntry.aspx`; `WHStockAdjustmentView.aspx` lists/prints/deletes.

## Preconditions
`tblCentralStore` rows with `Quantity>0` for the product; adjustment types in `tblAdjustmentType` (maintained by `AdjustmentEntry.aspx`, PAGE-356).

## Steps
| # | Page | Handler -> DAL -> SQL |
|---|---|---|
| 1 | `WHStockAdjustmentEntry.aspx` | `PermissionForShowBtn` (SP `sp_GET_btnShow`) on every request; product search -> `WHStockAdjDAL.LoadProductFromWH` (`tblCentralStore WHERE ProductId AND Quantity>0`) |
| 2 | same | qty entry -> `reqQtyTextBox_OnTextChanged` blocks qty > row stock (BR-M10-050) |
| 3 | same | `Button1_Click` -> `Validation` (BR-M10-049) -> `GetTransNo` (BR-M10-051) -> `SaveWHStockAdjMaster` (INSERT `tblWHStockAdjustment`, `ActionStatus='Approved'`) -> per ticked row `SaveWHStockAdjDetail` (INSERT `tblWHAdjustmentDetail`) + `UpdateCentralStoreStock(...,"-")` (`Quantity = Quantity - qty`) (BR-M10-052) |
| 4 | `WHStockAdjustmentView.aspx` | list `LoadWHStockAdjustment(param)`; `reportData` -> `WHStockAdjustmentListReportViewer.aspx?WHStockAdjId=`; `EditData` (delete) -> `UpdateCentralStoreStock(...,"+")` per detail then `DeleteStockWHAdjustment` (BR-M10-053) |

## Data changed
`tblWHStockAdjustment`, `tblWHAdjustmentDetail` (INSERT/DELETE), `tblCentralStore.Quantity` (- on save, + on delete). Ledger tables not changed (GAP-M10-006).

## Branches / exceptions
Adjustment type and To Store do not alter the effect (always decrease). No approval step. No server-side stock check.

## Postconditions
Central stock reduced; adjustment listed with auto-generated `WHA-yymm1nnnn` number.

## Related
BR-M10-049..053; PAGE-392, 393, 356, 357, 353; GAP-M10-006, 012.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF05-T1 | Adjust 3 of 10 | stock 7, doc Approved |
| WF05-T2 | Choose "increase" type | still decreases |
| WF05-T3 | Delete adjustment | stock 10 restored |
| WF05-T4 | Save without remarks | `Give Remarks` |
