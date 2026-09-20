# M11 - Stock Transfer & Chalan Receive

> Reverse-engineered specification (documentation only). Reviewed 2026-09-19. Tier A: PAGE-408, 412, 413, 414, 420. Other pages Tier B (PAGE-407, 409, 423 were additionally read in detail where they change stock; their specs remain Tier B).

## 1. Purpose & business overview
M11 moves stock between organisational units and records the documents (challans) that go with it:
- **STO (warehouse to DC)**: request (PAGE-420) -> pick/issue (M06/M07 pages) -> DC receipt (PAGE-413) -> SAP notification.
- **DC to DC (B2B)**: challan with immediate stock issue at the sender (PAGE-412), receipt at the destination (PAGE-423), list/delete (PAGE-407).
- **DC to warehouse return**: challan (PAGE-408), stock deduction on "Transfer" (PAGE-409), warehouse receipt (PAGE-414).
- **SAP-origin B2B challans** created and flagged by SAP-related stored procedures (no C# caller).
- Reports: challan prints, STO print, and "in-transit" reports that are actually customer receivables.

## 2. Page catalogue
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-398 | AgingInTransitReportViewer - Aging receivable viewer (RPT-M11-03) | Report viewer | B | [spec](pages/PAGE-398-agingintransitreportviewer.md) |
| PAGE-399 | DcToDcChalanReportViewer - DC to DC challan print (RPT-M11-01) | Report viewer | B | [spec](pages/PAGE-399-dctodcchalanreportviewer.md) |
| PAGE-400 | DcToWHChalanReportViewer - DC to WH challan print (RPT-M11-01) | Report viewer | B | [spec](pages/PAGE-400-dctowhchalanreportviewer.md) |
| PAGE-401 | InTransitReportViewer - In-transit (receivable) viewer (RPT-M11-03) | Report viewer | B | [spec](pages/PAGE-401-intransitreportviewer.md) |
| PAGE-402 | MIOwiseInTransitReportViewer - MIO-wise receivable viewer (RPT-M11-03) | Report viewer | B | [spec](pages/PAGE-402-miowiseintransitreportviewer.md) |
| PAGE-403 | StockReceiveReportViewer - STO receipt print (RPT-M11-01) | Report viewer | B | [spec](pages/PAGE-403-stockreceivereportviewer.md) |
| PAGE-404 | StockTransfarOrderReportViewer - STO order print (RPT-M11-02) | Report viewer | B | [spec](pages/PAGE-404-stocktransfarorderreportviewer.md) |
| PAGE-405 | TransferStockReceiveReportViewer - B2B challan/receipt print (RPT-M11-01) | Report viewer | B | [spec](pages/PAGE-405-transferstockreceivereportviewer.md) |
| PAGE-406 | AgingInTransitReport - Aging receivable launcher (RPT-M11-03) | Report launcher | B | [spec](pages/PAGE-406-agingintransitreport.md) |
| PAGE-407 | B2BTransferView - B2B challan list (Pending/Received), print, delete via sp_Del_B2BDelete (read in detail: BR-M11-010/011, GAP-M11-001) | List / delete | B | [spec](pages/PAGE-407-b2btransferview.md) |
| PAGE-408 | DepoToWHTransfer - DC to warehouse return challan (WF-M11-03) | Entry / transaction | A | [spec](pages/PAGE-408-depotowhtransfer.md) |
| PAGE-409 | DepoToWhTransferView - Pending DC-WH challans: Transfer decrements DC stock, Delete via sp_DeleteDwpotToWhChalan (read in detail: BR-M11-026/027) | List / stock transfer | B | [spec](pages/PAGE-409-depotowhtransferview.md) |
| PAGE-410 | InTransitReport - In-transit (receivable) report launcher (RPT-M11-03) | Report launcher | B | [spec](pages/PAGE-410-intransitreport.md) |
| PAGE-411 | MIOwiseInTransitReport - MIO-wise receivable launcher (RPT-M11-03) | Report launcher | B | [spec](pages/PAGE-411-miowiseintransitreport.md) |
| PAGE-412 | NewStockTransferDcToDc - DC to DC (B2B) transfer challan; SAP/B2B SP set (WF-M11-02, WF-M11-04) | Entry / transaction | A | [spec](pages/PAGE-412-newstocktransferdctodc.md) |
| PAGE-413 | ReceiveProductByChalanByDC - DC receives STO shipment (WF-M11-01) | Entry / transaction | A | [spec](pages/PAGE-413-receiveproductbychalanbydc.md) |
| PAGE-414 | ReceiveProductByChalanByWh - Warehouse receives DC-WH challan (WF-M11-03) | Entry / transaction | A | [spec](pages/PAGE-414-receiveproductbychalanbywh.md) |
| PAGE-415 | StockRcvByWH - Pending DC-WH challans awaiting WH receipt (leads to PAGE-414) | List | B | [spec](pages/PAGE-415-stockrcvbywh.md) |
| PAGE-416 | StockReceive - Legacy stock receive page (not reviewed) | Entry | B | [spec](pages/PAGE-416-stockreceive.md) |
| PAGE-417 | StockReceiveByDC - Receivable STO list per DC (Admin: all DCs; others by UserId) (leads to PAGE-413) | List | B | [spec](pages/PAGE-417-stockreceivebydc.md) |
| PAGE-418 | StockReceiveView - Received stock view (not reviewed) | List | B | [spec](pages/PAGE-418-stockreceiveview.md) |
| PAGE-419 | StockTransferDcToDc - Older DC to DC transfer page (superseded by PAGE-412; not reviewed) | Entry | B | [spec](pages/PAGE-419-stocktransferdctodc.md) |
| PAGE-420 | StockTransferOrder - STO request warehouse to DC (WF-M11-01) | Entry / transaction | A | [spec](pages/PAGE-420-stocktransferorder.md) |
| PAGE-421 | StockTransferReport - Stock transfer report page | Report | B | [spec](pages/PAGE-421-stocktransferreport.md) |
| PAGE-422 | StockTransportOrderReportList - STO list opening the STO print (RPT-M11-02) | Report list | B | [spec](pages/PAGE-422-stocktransportorderreportlist.md) |
| PAGE-423 | TransferReceiveProductByChalanByDC - DC receives a B2B challan: DCStockIn2 + damage freeze + SAP notify (read in detail: BR-M11-009, WF-M11-02) | Entry / transaction | B | [spec](pages/PAGE-423-transferreceiveproductbychalanbydc.md) |
| PAGE-424 | TransferStockReceiveByDC - Pending B2B challans for receiving DC (leads to PAGE-423) | List | B | [spec](pages/PAGE-424-transferstockreceivebydc.md) |
| PAGE-425 | TransferStockReceiveReportViewer - Duplicate viewer of PAGE-405 (RPT-M11-01) | Report viewer | B | [spec](pages/PAGE-425-transferstockreceivereportviewer.md) |

## 3. Business rules index
| BR | Rule (abridged) | Page |
|---|---|---|
| BR-M11-001 | Sender and receiver DCs are identified by typed unit code resolved in tblCompanyUnit; unknown code is rejected and cleared; no rule stops from = to and no user/DC scoping applies. | PAGE-412 |
| BR-M11-002 | A challan requires number, date, manufacturer, from unit, to unit and ≥1 product line. | PAGE-412 |
| BR-M11-003 | Available stock per batch/expiry = SUM(StockQty) of tblDCStore rows (Trans_Type Receive+Issue) of the from-unit and product, shown only if > 0; unit price/VAT come from the active tblUnitPrice. | PAGE-412 |
| BR-M11-004 | Transfer quantity must not exceed displayed stock — enforced only when the qty box posts back; no server-side re-check at Save. | PAGE-412 |
| BR-M11-005 | The same source DCStoreId cannot be added twice (list de-duplication by DCStoreId). | PAGE-412 |
| BR-M11-006 | Challan number = IDT + id (MAX(ChalanId)+1), left-padded with zeros to 6 digits only when the id has 1-3 digits; ids ≥ 1000 are unpadded. | PAGE-412 |
| BR-M11-007 | Challan value = Σ qty × active unit price; VAT = Σ qty × VAT/unit; grand = value + VAT (per detail Value, Vat, ValueWVat). | PAGE-412 |
| BR-M11-008 | Stock leaves the sender at save time: each detail with qty>0 inserts a negative Trans_Type='Issue', StockCondition='B2BIssue' ledger row referencing the source row (ReferenceDCStoreId) and the challan detail; the goods are "in tra... | PAGE-412 |
| BR-M11-009 | Receiving (PAGE-423) inserts a destination Receive row (B2BReceive) for the received quantity and flips IsDeliver to 'True'; the quantity difference stays as a restricted tblDCStoreFreeze row for damage. | PAGE-412 |
| BR-M11-010 | Deleting a B2B challan from B2BTransferView is allowed only when its status text is not "Received" (status = IsDeliver='True'); the page deletes tblChalanInfo first, then calls sp_Del_B2BDelete. | PAGE-412 |
| BR-M11-011 | sp_Del_B2BDelete exists in two conflicting definitions: v1 (2026-09-01) adds Quantity back onto the source Receive row and leaves Issue rows; v2 (2026-09-02, "Fix STO Chalan Process") deletes the Issue (and B2BReceive) ledger rows... | PAGE-412 |
| BR-M11-012 | SAP-origin challans: sp_SAP_B2BChallanMasterInsert builds a challan header from SAP stock movement (plants mapped by tblCompanyUnit.SAP_Code, products by tblProduct.SAP_Code), skipping challans already in tblWHStockInMaster; missi... | PAGE-412 |
| BR-M11-013 | sp_SAP_B2BChallanDetailInsert consumes source DC stock FIFO by expiry using the decrement model (StockQty -=), not Issue rows; detail quantity = full SAP line qty per allocation step; stale-variable defect creates phantom details ... | PAGE-412 |
| BR-M11-014 | SAP hand-off states on the challan: SAP_ChallanSend (challan payload sent: sp_SAP_ChallanSendDetailByChalanId supplies SAP product code, batch, qty, UoM) and SAP_Challan_ConfirmationSend (receipt confirmation sent) are set to 1 by... | PAGE-412 |
| BR-M11-015 | An STO needs manufacturer, warehouse, DC and at least one line with code, name and requested qty; requisition number is system-generated REQ-+7 digits from MAX(ReqId)+1. | PAGE-420 |
| BR-M11-016 | Lines are created batch-specific (IsFromBatch=true, BatchNO from the picked central-store batch) so pick/issue draws from that batch first. | PAGE-420 |
| BR-M11-017 | Requested qty ≤ displayed central stock is checked only on postback; not re-checked at save or at pick (pick spills to next batch by FIFO Quantity order when short). | PAGE-420 |
| BR-M11-018 | Saving an STO does not move stock; stock moves at picking/issue (central store Quantity set to remaining; weighted-average issue at the running average rate). | PAGE-420 |
| BR-M11-019 | Editing an STO replaces all its lines; batch numbers are not carried into the re-inserted lines (BatchNO NULL). | PAGE-420 |
| BR-M11-020 | Central-store stock shown for search is Quantity>0 per receive lot for the product regardless of warehouse selected. | PAGE-420 |
| BR-M11-021 | STO issue costing: tbl_INV_CentralStoreStoIssue values the issue at the latest weighted-average closing rate of the product/company (Close_Amt/Close_Qty of last row), producing Curr_Amt = −qty × rate; new average = closing amount/... | PAGE-420 |
| BR-M11-022 | Only one open (untransferred) DC→WH challan per DC at a time: a new one is refused while any challan of that from-unit has IsTransfer IS NULL. | PAGE-408 |
| BR-M11-023 | Challan number SDC+id (MAX(SChalanId)+1; zero-padded to 6 only for 1-3 digit ids). | PAGE-408 |
| BR-M11-024 | Lines may come from normal DC stock (tblDCStore, StockQty>0) or freeze stock (tblDCStoreFreeze, DCStoreFreezeId>0); each line carries a purpose (tblPurpose, default 1). | PAGE-408 |
| BR-M11-025 | Challan value = Σ qty × unit price (active tblUnitPrice), VAT = Σ qty × VAT/unit. | PAGE-408 |
| BR-M11-026 | Saving the challan does not reduce stock; stock is decremented only when a user presses *Transfer* on PAGE-409 (IsTransfer='OK'): StockQty := StockQty − Quantity on the Receive row (or freeze row) — an in-place decrement, not an I... | PAGE-408 |
| BR-M11-027 | Transfer (PAGE-409) does not verify quantity ≤ current stock (result may be negative) and marks the challan transferred based on the last row's update result only. | PAGE-408 |
| BR-M11-028 | Warehouse and manufacturer are pre-forced to id 1 on load (defaults), still changeable. | PAGE-408 |
| BR-M11-029 | Only STO lines not yet transferred (tblStockInTransfar.IsTransfared IS NULL) of the requisition are shown; when none remain the page reports "Stock Already Received!!". | PAGE-413 |
| BR-M11-030 | Received qty defaults to issued qty; unreceived (damaged) qty is entered, bounded by issued qty, and required on every row (0 allowed). | PAGE-413 |
| BR-M11-031 | Stock effect: one tblDCStore Receive row per line, StockQty = received qty (not issued), TotalQuantity = issued qty, guarded so a StockInTransfarId can create only one row. | PAGE-413 |
| BR-M11-032 | Unreceived/damaged qty is not added to available stock: it is written to tblDCStoreFreeze as 'Restricted' (freeze stock) at the new receive row. | PAGE-413 |
| BR-M11-033 | Valuation: CostPerUnit = STO unit price (excl. VAT); TotalCostAmount = STO total incl. VAT — the DC ledger cost is the transfer price, not the warehouse weighted-average cost, and is internally inconsistent. | PAGE-413 |
| BR-M11-034 | Completion flags: tblStockInTransfar.IsTransfared='OK' per line and tblRequisition.ReceiveIssue='OK' (removes the STO from the DC's receive list). | PAGE-413 |
| BR-M11-035 | SAP hand-off: after saving, the challan/STO number is passed to MakeRESTRequest with the user id; any failure is ignored, so the ERP receipt stands even if SAP is not notified; no state flag is set by this page. | PAGE-413 |
| BR-M11-036 | Only challans with IsDeliver = false are receivable; IsDeliver becomes 'OK' once at least one line was stocked in or frozen; the challan is then closed for all lines. | PAGE-414 |
| BR-M11-037 | Stock effect (branch A): a line sourced from normal DC stock (DCStoreId>0, no freeze id) is added to central stock as a new tblCentralStore row Available/Regular with Quantity = received qty; lines sourced from freeze stock are ad... | PAGE-414 |
| BR-M11-038 | Unreceived/damaged quantity always goes to WH freeze stock (tblWhStockConditionFreeze + tblWhStoreFreeze, Restricted). | PAGE-414 |
| BR-M11-039 | Valuation: price and VAT per unit are those of the DC row's original STO line; totals are computed on the issued quantity, so TotalPrice/TotalVAT/TotalAmount overstate value when qty is short. | PAGE-414 |
| BR-M11-040 | tblCentralStore.MigoDetailID is populated with the challan detail id (SChalanDetailsId), and tblWhStockConditionFreeze.ReceiveId with the same id, not with a MIGO or central ReceiveId (the freeze store's ReceiveId is separately re... | PAGE-414 |

## 4. Status lifecycles
| Object | States | Set by |
|---|---|---|
| tblChalanInfo.IsDeliver (DC-DC) | 'False' (created, in transit) -> 'True' (received) | PAGE-412 insert / `ChalanDAL.ChalanUpdate` (PAGE-423) |
| tblChalanInfo SAP flags | SAP_ChallanSend 0->1; SAP_Challan_ConfirmationSend 0->1 | `sp_SAP_Up_SAP_ChallanSendByChalanId` / `...ConfirmByChalanId` (external caller) |
| tblDepotToWHChalanInfo | IsTransfer NULL -> 'OK' (stock deducted, PAGE-409); IsDeliver 'False' -> 'OK' (WH received, PAGE-414) | PAGE-409, PAGE-414 |
| STO chain | tblRequisition Submit 'OK' (issued) -> ReceiveIssue 'OK' (DC received); tblStockInTransfar IsIssue 'OK' -> IsTransfared 'OK' | PAGE-219/259, PAGE-413 |
| tblDCStore ledger | Issue row (B2BIssue, negative) at DC-DC send; Receive row 'B2BReceive'/'Available' at receipt | PAGE-412 / PAGE-423 / PAGE-413 |
| Freeze stock | tblDCStoreFreeze / tblWhStoreFreeze StockCondition 'Restricted' | receive pages |

## 5. Roles / permissions observed
Menu-driven. Code-level: `GetPermissionForShowBtn(path)` hides Save on PAGE-412/420 when the permission SP returns rows; `StockReceiveByDC` scopes DC list by `Session["UserType"]`/`UserId`; all other checks absent (no role checks before stock-changing actions; typed unit codes unrestricted). `Session["LoginName"]`/`["UserId"]` used for audit/SAP calls without null guards.

## 6. Data model (tables and SPs that matter)
- Documents: tblRequisition, tblRequsitionChild, tblStockInTransfar, tblChalanInfo, tblChalanDetail, tblDepotToWHChalanInfo, tblDepotToWHChalanDetail.
- Stock: tblDCStore (ledger: Trans_Type Receive/Issue, ReferenceDCStoreId, ChalanDetailsId, StockCondition B2BIssue/B2BReceive/Available), tblDCStoreFreeze, tblDCStoreTransaction, tblCentralStore, tblWhStoreFreeze, tblWhStockConditionFreeze, tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock.
- SPs (repo): sp_Del_B2BDelete (x2), sp_DeleteDwpotToWhChalan, sp_SAP_B2BChallanMasterInsert/DetailInsert, sp_SAP_B2BStockReceiveByDc, sp_SAP_ChallanSendDetailByChalanId, sp_SAP_Up_SAP_ChallanSendByChalanId, sp_SAP_Up_SAP_ChallanConfirmByChalanId, tbl_INV_CentralStoreStoIssue (x2), sp_Adjust_DCStore_Stock_Only, sp_Check_DCStore_StockAnomalies.
- **Ledger contract:** available stock = sum of `StockQty` over Receive+Issue rows; the repair/diagnostic scripts assume Receive rows keep `StockQty = TotalQuantity` (GAP-M11-003).

## 7. Integrations
SAP: staged data `SAP_API_Data..tblSAP_StockMovementMaster/Detail`; SAP product/plant codes; challan payload SP; flags; REST notification `MakeRESTRequest` on receive (PAGE-413/423); SAP dashboard. See WF-M11-04.

## 8. Reports
See `reports/README.md`: RPT-M11-01 to RPT-M11-03.

## 9. Workflows
- [WF-M11-01 STO warehouse to DC](workflows/WF-M11-01-sto-warehouse-to-dc.md)
- [WF-M11-02 DC to DC B2B transfer](workflows/WF-M11-02-dc-to-dc-b2b-transfer.md)
- [WF-M11-03 DC to warehouse return](workflows/WF-M11-03-dc-to-warehouse-return.md)
- [WF-M11-04 SAP B2B challan and hand-off](workflows/WF-M11-04-sap-b2b-challan-and-handoff.md)

## 10. Known gaps summary
See [GAPS-M11.md](GAPS-M11.md) (21 items). Headlines: two conflicting `sp_Del_B2BDelete` scripts (001) and two `tbl_INV_CentralStoreStoIssue` scripts (002); mixed stock models vs the adjust/anomaly scripts (003); SAP detail SP phantom rows (004); SQL injection in transfer DALs (005); no callers/scheduler for SAP SPs (006); no transactions (007); in-transit reports are receivables (017).

## 11. What is still unverified
- Which SP script versions are deployed (GAP-M11-001/002); SAP job/scheduler and `MakeRESTRequest`, `sp_RPT_ChallanStatusByDate`, `sp_SAP_StockReceive` bodies.
- Pick/issue pages (PAGE-259/219, other modules) at page level; Tier-B pages 415-419, 421, 422, 424 and all viewers beyond wiring.
- DDL of tblChalanInfo (SAP columns), tblDCStore, tblDepotToWHChalanInfo (IsDeliver/IsTransfer types).
- Accounting effects of transfers (none found in repo; SAP-side UNKNOWN).
