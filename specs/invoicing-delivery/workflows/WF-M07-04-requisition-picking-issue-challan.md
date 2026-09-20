# WF-M07-04 - Requisition picking, central-store issue and challan (CLN)

**Trigger:** a DC requisition (`tblRequisition`, created in the requisition module) awaiting picking. **Actors:** central warehouse user. **Precondition:** `tblRequisition.CreatePicking` NULL or ''.

## Steps
1. `CreatePickingOnWareHouse` (PAGE-261) -> `RequisitionBLL.GetAllNonPickReq` -> list where `CreatePicking is null or = ''`; link `CreatePickingDetailForWareHouse.aspx?ReqId=`.
2. `CreatePickingDetailForWareHouse` (PAGE-259) `Page_Load`: if any `tblStockInTransfar` row exists for ReqId -> alert " Picking Already Completed!!" and stop; else load lines (`GetPickingDataForIssue`: `tblRequsitionChild` + latest `tbl_INV_CentralStore_WeightedAvg` Close_Qty and Avg_Rate) and header (`GetRequisitionInfoByReqId`); challan no = `"CLN-" + ReqNo.Substring(4)`, date today.
3. User ticks lines; issue qty defaults to requested, capped to min(requested, current stock); price = qty x Avg_Rate; VAT = qty x 0 (hard-coded); total = price + VAT; case qty = round(qty/PcsPerCase x CaseQty, 2) (`PriceCalculation`).
4. Save (confirm dialog; at least one line ticked): `SaveData()`:
   a. `UpdatePickingInformationOnRequisition`: `UPDATE tblRequisition SET CreatePicking='OK', Submit='OK', IssueChalanNo, IssuChalanDate, SubmitDate, PickingNo(=challan no), PickingDate, TruckNo, DriverName (read-only boxes -> empty), TotalPrice, TotalVAT, GrandTotalPrice`.
   b. `UpdateIssueInformationOnRequisitionChild(IsFromBatch, lines)` -> per line `UpdateCentralStockAndStockInTransfar`: FEFO (or batch-specific) lots from `tbl_INV_Central_BatchWiseStock` with remaining qty (inbound - issued), `INSERT tblStockInTransfar` (idempotent by ReqChildId+BatchNo+ReceiveId, IsIssue 'OK'), `UPDATE tblCentralStore.Quantity` (0 when lot exhausted, else remaining), `tbl_INV_CentralStoreStoIssue(@StockInTransfarId)` (writes `tbl_INV_CentralStore_WeightedAvg` issue row with rate = opening avg, and negative `tbl_INV_Central_BatchWiseStock` 'STO Picking' row; body in `SP_Fix_CentralStoreStoIssue_Corrected.sql`); then `UPDATE tblRequsitionChild` (IssueQty, UnitPrice, PriceAmount, VATAmount, TotalPrice, IsPicking 'OK', CaseQty, IsIssue 'OK').
   c. Challan popup `ChallanReportViewer?reqId=` -> `crpChallan.rpt`.
5. Follow-up: `ChallanReportView` (PAGE-257) lists challans (`Submit='OK'`, `SubmitDate` between dates, optional DC), prints, exports Excel, and opens `ChallanVIewEditNew` (PAGE-258) to set challan date, driver and truck (`UPDATE tblRequisition SET SubmitDate=PickingDate=IssuChalanDate=@date, DriverName, TruckNo`).
6. Receipt at the DC (other module) moves `tblStockInTransfar` into `tblDCStore` Receive rows.

## Data changed
tblRequisition, tblRequsitionChild, tblStockInTransfar, tblCentralStore, tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock.

## Exceptions / risks
Header submitted before stock movement, no transaction; shortage beyond available lots not detected (child IssueQty keeps entered value); truck/driver not enterable at issue; challan edit unrestricted (GAP-M07-028..030). The SP script has two versions in repo; live body unverified.

**Related BR:** 047-057. **Pages:** PAGE-259, 261, 256, 257, 258, 255/238 (viewers). **Tests:** see PAGE-259 and PAGE-257 §27.
