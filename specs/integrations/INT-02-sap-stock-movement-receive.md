# INT-02 - SAP stock movement / challan receive (STO, B2B) - inbound

| Item | Detail |
|---|---|
| Direction | SAP -> ERP |
| Transport | Database hand-off via a separate database `SAP_API_Data` (`tblSAP_StockMovementMaster`, `tblSAP_StockMovementDetail`) referenced with 3-part names in `SP_NewStockTransferDcToDc_Related.sql`. Population of those tables: UNKNOWN (external adapter). |
| Trigger | Operator opens PAGE-681 (DIC, Stock Receive preselected) or PAGE-680 (hidden option) -> `Preview >>` -> `SAP_StockReceive.aspx` / `SAP_StockReceiveDIC.aspx` -> Approve. |
| Payload / fields | Master: `challan_code, challan_date, from_plant_code, to_plant_code, action, StockMovementMasterId`. Detail: `StockMovementDetailId, product_code (SAP code, first 12 chars stripped to match tblProduct.SAP_Code), batch_no, expiry_date, manufacturer_date, quantity`. |
| Flow | `sp_Get_SAP_StockReceivePendingData` / `sp_Get_SAP_DICStockReceivePendingData(@Parm=DICCompanyUnitId)` -> list; `sp_Get_SAP_HideChallanByChallanNo` hides handled challans; `sp_Get_SAP_StockReceivePendingDataById`; on Approve: per line `sp_SAP_Up_StockReceiveQty(@StockMovementDetailId,@quantity)` then `sp_SAP_StockReceive(@ChallanNo,@ApproveBy)` (bodies UNKNOWN). |
| B2B SPs (bodies in repo, callers not in C#) | `sp_SAP_B2BChallanMasterInsert(@ChallanNo)`: creates `tblChalanInfo` (note 'From SAP', IsDeliver 'False', ManufacId 1) only if challan not already in `tblWHStockInMaster.ChallanNo`, price row active, total qty>0; totals = sum(qty*ISNULL(UnitPrice,1)) and VAT sum(qty*ISNULL(VATAmountPerUnit,1)) (BR-M17-008). `sp_SAP_B2BChallanDetailInsert(@MasterId,@ChallanNo)`: FEFO (`ORDER BY ExpDate`) deduction from `tblDCStore` of the from-unit, inserts `tblChalanDetail` (BR-M17-009). `sp_SAP_B2BStockReceiveByDc(@MasterId)`: inserts `tblDCStore` rows (`StockCondition='Available'`, `Trans_Type='Receive'`, StockQty=Quantity) at destination (BR-M17-010). `sp_Del_B2BDelete` reverses. Invoked probably from inside `sp_SAP_StockReceive` (INFERRED). |
| Status flags / retry | No retry. Challan disappears via hide-SP after receive. Approve is not atomic across the per-line quantity updates and the receive call. |
| Authentication | ERP: session login only; no role check. SAP side UNKNOWN. |
| Error handling | Detail page `Page_Load` swallow-all; approve exceptions bubble. Defects: (a) detail insert stores `Quantity=@Qty` (full requested qty) on every lot row instead of the allocated amount; (b) `SELECT TOP 1 @StockQuantity...` keeps the previous value when no lot is found (T-SQL), so shortage is not detected and stock can be mis-decremented; (c) `MAX(id)+1` key generation (race). |
| Pages | PAGE-680, 681, 682, 683. BR-M17-002..005, 008-010, 018. |
