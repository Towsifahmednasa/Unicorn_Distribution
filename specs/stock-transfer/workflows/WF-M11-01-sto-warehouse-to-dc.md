# WF-M11-01 — Stock Transport Order: warehouse → DC (request, pick/issue, DC receive)

**Confidence:** CONFIRMED for PAGE-420 and PAGE-413 code; pick/issue steps confirmed in BLL/DAL but their pages (PAGE-259 CreatePickingDetailForWareHouse, PAGE-219 IssueRequisitionProducts) belong to M07/M06 and were not reviewed at page level. SAP notification body UNKNOWN.

## Trigger & actors
Logistics/warehouse user raises an STO for a DC; warehouse picks and issues; DC user receives (list: `StockReceiveByDC.aspx`, Admin sees all DCs, others their mapped DCs).

## Preconditions
Central stock in `tblCentralStore` (`Quantity>0`) and weighted-average rows exist; DC, warehouse, manufacturer master data; the user is allowed the menu pages.

## Steps
| # | Step | Page → handler → BLL → DAL → SQL |
|---|---|---|
| 1 | Create STO: search product batches, choose qty, save | PAGE-420 `Button1_Click` → `ChalanBLL.GetProductWhStore` → `tblCentralStore`; `submitButton_Click` → `SaveAllData` → `RequisitionBLL.SaveRequsition/SaveRequsitionChild` → `tblRequisition`, `tblRequsitionChild` |
| 2 | Pick (other module) | PAGE-259 → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `UpdateCentralStockAndStockInTransfar` → `tblStockInTransfar` insert; `tblCentralStore.Quantity` := remaining; `ProcessWeightedAvg` → `tbl_INV_CentralStoreStoIssue(@StockInTransfarId)` |
| 3 | Issue (other module) | PAGE-219 → `UpdateStockTransfarInfoUpdate` → `tblStockInTransfar` amounts, `tblRequsitionChild.IsIssue/IssueQty/PriceAmount…`; requisition `Submit='OK'`, issue challan no/date/driver/truck |
| 4 | DC opens receive list | `StockReceiveByDC.aspx` → `RequisitionBLL.GetAllStockRcvByDc` (`tblRequisition WHERE Submit='OK' AND (ReceiveIssue IS NULL OR ='') AND ComUnitId=@ComUnitId`) |
| 5 | DC receives | PAGE-413 `Save` → `RequisitionBLL.DCStockIn2` → `tblDCStoreTransaction`, `tblDCStore` Receive row, `tblStockInTransfar.IsTransfared='OK'`, `tblRequisition.ReceiveIssue='OK'`; damage → `tblDCStoreFreeze` (Restricted) |
| 6 | SAP notification | `SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan(challanNo)` → SP `MakeRESTRequest(@StoNo,@RcvBy)` (errors swallowed) |
| 7 | Print receipt | `StockReceiveReportViewer.aspx?reqId=` |

## Data created / changed
tblRequisition (+, then Submit/ReceiveIssue flags), tblRequsitionChild (+, IssueQty), tblStockInTransfar (+, IsTransfared), tblCentralStore (Quantity reduced), tbl_INV_CentralStore_WeightedAvg (+ issue row) and tbl_INV_Central_BatchWiseStock (+ issue row, semantics per deployed SP), tblDCStore (+ Receive row), tblDCStoreTransaction (+), tblDCStoreFreeze (+ for damage).

## Stock effects (which store rows change)
- Warehouse: `tblCentralStore.Quantity` set to remaining per lot at **pick**; weighted-average ledger gets a negative issue row (rate = latest closing average).
- DC: **one `tblDCStore` Receive row per STO lot line at receive**: `TotalQuantity`=issued, `StockQty`=received, `DamageQty`; damage quantity is not available stock (freeze row).
- In transit: between issue and receive the goods are visible in neither store (no in-transit ledger; tracked only by `tblRequisition.ReceiveIssue` NULL and `tblStockInTransfar.IsTransfared` NULL).

## Valuation impact
Warehouse side at weighted-average rate (`tbl_INV_CentralStoreStoIssue`); DC side at STO unit price (`CostPerUnit`) with `TotalCostAmount` from the STO total incl. VAT (inconsistent) — GAP-M11-011. No accounting voucher is posted anywhere in this flow (none found in the code paths).

## Accounting posting implications
None in repository code for STO issue/receive. Any inter-unit accounting or SAP MIGO/STO posting occurs in SAP (via `MakeRESTRequest`) — UNKNOWN.

## SAP hand-off states
STO number/challan no is sent on DC receive; the ERP records no success flag for this path (contrast the challan flags of WF-M11-04). Failure is invisible.

## Branches / exceptions
Insufficient central stock at pick spills to the next batch (`restQty` logic); if stock is still short the remainder is silently not issued (no error). Duplicate receive blocked by `IF NOT EXISTS (StockInTransfarId)`. Partial failure mid-loop leaves some lines received (re-openable). Two `tbl_INV_CentralStoreStoIssue` definitions (GAP-M11-002).

## Postconditions
STO closed for the DC (`ReceiveIssue='OK'`), DC stock increased by received quantities, damage quarantined.

## Related BR ids
BR-M11-015…021, 029…035.

## Related pages
PAGE-420, 413, 417 StockReceiveByDC (B), 259/219 (other modules), PAGE-403/404 viewers.

## Tests
PAGE-420 §27 T1-T8; PAGE-413 §27 T1-T8.
