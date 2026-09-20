# WF-M13-02 Logistics requisition rollback (STO / picking / challan)

_Module M13, reverse-engineered 2026-09-19 (documentation only). Confidence tags as in the page specs._

## Trigger and actors
Logistics user opens `LogisticeRollBackOperation.aspx`, chooses SG / PG / CG.

## Steps
1. `DropDownList1_SelectedIndexChanged`: SG -> `RequisitionBLL.GetRequisitionView()` (CreatePicking IS NULL); PG -> `GetAllNonSubmitReq()` (Submit empty and CreatePicking set); CG -> `RollBackBLL.GetAllStockRcvByDcDAL()` (Submit='OK' and ReceiveIssue empty). (BR-M13-011..013)
2. Delete icon (confirm) -> SG `ImageButton2_Click` -> `RequisitionDAL.DeleteRequisition` (delete header + children).
3. PG `editImageButton_OnClick`: `GetStockInTransfer(reqId)` -> `UpdatePickingInformationOnRequisitionDAL` -> per row `UpdateIssueInformationOnRequisitionChildDAL(ReqChildId)` + `UpdateCentralStockStockOut(Quantity, ReceiveId)` (tblCentralStore.Quantity += Quantity) -> `DeleteStockInTransfer` (tbl_INV_Central_BatchWiseStock, tbl_INV_CentralStore_WeightedAvg, tblStockInTransfar).
4. CG `edit2ImageButton_OnClick`: as PG then `UpdateIssueInformationOnRequisition` (Submit, IssueChalanNo = NULL) and `UpdateStockTransfarInfoUpdate` (tblRequsitionChild.IsIssue = NULL).
5. Alert "RollBack Successfull!!" / "Error!!"; grid reloaded.

## Data changed
tblRequisition, tblRequsitionChild, tblStockInTransfar, tblCentralStore, batch-wise and weighted-average tables (PAGE-514 section 17). No transaction (BR-M13-014); no audit.

## Branches / exceptions
`UPsts && Delsts` false -> "Error!!" (partial changes may persist).

## Related
BR-M13-011..014; PAGE-514; GAP-M13-011, GAP-M13-004.

