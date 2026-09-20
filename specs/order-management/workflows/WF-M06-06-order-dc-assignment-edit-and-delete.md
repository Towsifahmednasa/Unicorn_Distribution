# WF-M06-06 — Order maintenance: assign DC/route, change DC, delete

**Pages:** PAGE-207 `OrderDCChange` (MasterSetup_UI), PAGE-223 `OrderEdit` (popup), PAGE-222 `OrderDeleteorEdit`, PAGE-211/213 delete-order report. **BR:** BR-M06-063…068.

## A. Assign DC + route to DC-less orders (PAGE-207)
Trigger: orders arrive without DC (`ComUnitId NULL`). Steps: page lists them via `sp_Get_OrderTrackingList_Latest(@Parm=" AND mas.ComUnitId is null ")`; user ticks orders, picks DC (`sp_GET_DistributionCenter`) then route (`sp_GET_DistributionRouteByDCID`); **Assign** → `UpdateOrderDC` → `sp_Update_OrderDC(@DCID,@RouteNameId,@ApprovedBy=UserId,@OrdId="1,2,3")` (body UNKNOWN). Result: orders then satisfy `ComUnitId=@DC` in the proforma list (WF-M06-02) — INFERRED.

## B. Change DC of a single order (PAGE-223 via PAGE-222 grid Edit)
`OrderDeleteorEdit` grid **Edit** → popup `OrderEdit.aspx?ID=<orderId>` → user picks a DC (text `ComUnitName:ComUnitCode`) → `updateButton_Click` → `OrderListBLL.UpdateDataForCompanyInfo` → inline SQL `UPDATE tblOrder SET ComUnitId, ComUnitCode, ComUnitName WHERE OrderId=@OrderId` (`OrderListDAL.cs:270-280`). **No check that the order is un-invoiced, no route/territory update, no audit.** The grid path is reachable only if the (hidden) search box is used — the visible UI offers just the delete dropdown, so PAGE-223 is effectively dormant.

## C. Delete an un-invoiced order (PAGE-222)
Dropdown from `sp_GetOrderInvoiceIsZero` → **Delete** (confirm) → `CustomerMasterDAL.DeleteRequisition` → `sp_DeleteOrder(@OrdID,@LoginName=Session["UserId"])` (UNKNOWN) → true ⇒ success; false ⇒ "Can not delete!!Proforma Already Generated!". Deleted orders are (INFERRED) archived in `tblOrderDel` and reported by RPT-M06-02.

## Data changed
A: `tblOrder` (DC/route fields — UNKNOWN). B: `tblOrder.ComUnitId/Code/Name`. C: `tblOrder`/`tblOrderDetail` removed, `tblOrderDel` (INFERRED).

## Exceptions
A/C: SP failure ⇒ generic message; C: empty selection ⇒ unhandled FormatException; B: none.

## Tests
A: select 2 orders → assign → both leave list. B: change DC of an already-invoiced order → accepted (defect check). C: delete un-invoiced → gone + report row; delete invoiced → refusal.
