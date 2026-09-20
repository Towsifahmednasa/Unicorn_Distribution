# WF-M10-03 Warehouse stock-out (issue from an approved stock-in)

**Module:** M10 | **Confidence:** CONFIRMED.

## Trigger and actors
Warehouse operator needs to remove received central stock (damage/return/correction) -> `WHStockInList.aspx` -> `WarehouseStockOut.aspx`.

## Preconditions
An `Approved` `tblWHStockInMaster`; `tblCentralStore` rows exist whose `MigoDetailID` equals the stock-in detail id (how they are created is UNKNOWN, see WF-M10-01).

## Steps
| # | Page | Handler -> BLL -> DAL -> SQL |
|---|---|---|
| 1 | `WHStockInList.aspx` | `LoadGrid` -> `WHStockListBLL.LoadWHStock` -> `WHStockListDAL.LoadWHStock` (`Status='Approved'`); link `HyperLink1_OnClick` -> `Session["WHId"]=id` -> redirect |
| 2 | `WarehouseStockOut.aspx` | `Page_Load` -> `loadData` -> `LoadWHStockwithDetail(Session["WHId"])`: stock-in master + lines LEFT JOIN `tblCentralStore` on `MigoDetailID = WHStockInDetailID` (C.Stock = `Quantity`) |
| 3 | same | operator ticks lines, enters Out Qty; `qtyTextBox_OnTextChanged` blocks qty > C.Stock (BR-M10-024) |
| 4 | same | `submitButton_Click` -> `Validation` (BR-M10-023) -> `SaveAllData` -> `WHStockListBLL.SaveStockOutMasterInfo` (INSERT `tblWHStockOutMaster`, code `WHS-nnnnnnn`) -> `SaveWarehouseStockOutDetail` per line: INSERT `tblWHStockOutDetail` then `UPDATE tblCentralStore SET Quantity = Quantity - qty WHERE MigoDetailID = ...` (BR-M10-025) |

## Data changed
`tblWHStockOutMaster`, `tblWHStockOutDetail` (INSERT), `tblCentralStore.Quantity` (decrease). Not changed: `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock` (BR-M10-026).

## Branches / exceptions
- Direct URL without `Session["WHId"]` -> NullReferenceException.
- Qty > C.Stock: reset to 0 -> then blocked by "non-zero" rule.
- Server does not re-check stock; two operators can over-issue; quantity can go negative.
- Cancel button redirects to `WhFreezeStockRelease.aspx`.

## Postconditions
Central stock reduced in the legacy table; requisition/valuation ledgers unchanged (divergence, GAP-M10-006). The stock-in stays `Approved`.

## Related
BR-M10-022..026; PAGE-389, 395; GAP-M10-006, 021.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF03-T1 | Approved stock-in listed | yes |
| WF03-T2 | Out qty within stock | stock decreases by qty |
| WF03-T3 | Out qty above stock | alert, qty 0 |
| WF03-T4 | Compare WeightedAvg `Close_Qty` before/after | unchanged |
