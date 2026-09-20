# WF-M10-07 Sample stock conversion (DC) and sample stock for warehouse

**Module:** M10 | **Confidence:** CONFIRMED (DC, PAGE-379); warehouse variant PAGE-376 only mechanically checked (INFERRED).

## Trigger and actors
Sales-center/warehouse staff convert part of a batch to/from sample stock: `SampleTypeConvertion.aspx` (DC, `tblDCStore`) or `SampleStockforWarehouse.aspx` (central, `tblCentralStore.StockInQty`).

## Steps (DC)
| # | Page | Logic |
|---|---|---|
| 1 | `SampleTypeConvertion.aspx` | choose SC, action (`Sample to Sound` / `Sound to Sample`), date, product; `Button1_Click` -> `SampleTypeConventionBLL.GetProductDcStore` (`tblDCStore` by DC + ProductId, `StockQty>0`) |
| 2 | same | Convention qty per batch row (<= stock, BR-M10-040); Add -> staged `ProductGridView` |
| 3 | same | `submitButton_Click1` -> `Validation` (BR-M10-038) -> `SaveDataForSubDcStockOutMasterBll` (INSERT `tblSampleStockForDcMaster`, code `SSD########`, Status `Posted`) -> `SaveDataForSubStockOutDetailBll` (INSERT `tblSampleStockForDcDetails`) -> `StockOut` (`tblDCStore.StockQty -= qty`) (BR-M10-041) |
| 4 | `SampleTypeConventionView.aspx` | list `tblSampleStockForDcMaster`; delete -> `StockIn` (`+= qty`) then delete details + master (BR-M10-042) |

## Steps (warehouse variant, INFERRED from DAL)
`SampleStockforWarehouse.aspx` -> master `tblSampleStockForWareHouseMaster` + details; `SampleStockForWHDal.StockOutCentral`: `UPDATE tblCentralStore SET StockInQty = StockInQty - @SampleStock WHERE ReceiveId`; delete on `SampleStockForWareHouseView` re-adds (`StockInCentral`). Note: it changes `StockInQty`, not `Quantity` (BR-M10-059).

## Data changed
Sample masters/details; `tblDCStore.StockQty` / `tblCentralStore.StockInQty`.

## Branches / exceptions
Both actions decrease stock; no counterpart increase; no server-side stock check; delete has no status guard; failures in detail insert are silent (BLL returns true).

## Postconditions
Stock reduced by the sample quantity; document listed.

## Related
BR-M10-038..042, 059; PAGE-379, 378, 376, 377; GAP-M10-010.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF07-T1 | Convert 5 units either direction | StockQty -5 |
| WF07-T2 | Delete document | +5 restored |
| WF07-T3 | Qty > stock | textbox cleared |
