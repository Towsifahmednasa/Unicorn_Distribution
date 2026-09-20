---
page_id: PAGE-259
module: M07
page: CreatePickingDetailForWareHouse
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---

# PAGE-259 — CreatePickingDetailForWareHouse

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-259 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | CreatePickingDetailForWareHouse |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_CreatePickingDetailForWareHouse |
| aspx size | 27525 bytes |
| Code-behind LOC | 408 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |


## 2. Purpose
CONFIRMED (code): opened from `CreatePickingOnWareHouse` (`?ReqId=`). Shows a DC requisition (STO), proposes issue quantities per line from central-store stock, computes price/VAT/case totals, and on Save marks the requisition picked/submitted, allocates central-store lots (FEFO or batch-specific), writes `tblStockInTransfar`, decrements central stock and creates the issue challan (`CLN-...`), then opens the challan print.

## 3. Business Objective
INFERRED: central warehouse issues stock to a DC against its requisition and produces the delivery challan that travels with the goods; the DC later receives it (other module). See `../workflows/WF-M07-04-requisition-picking-issue-challan.md`.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `challanreportviewer.aspx`, `createpickingonwarehouse.aspx`


## 5. Route
`~/SInventory_UI/CreatePickingDetailForWareHouse.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx |
| Code-behind | Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ProductBLL (Library.BLL/SInventory_BLL/ProductBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | ProductDAL (Library.DAL/SInventory_DAL/ProductDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAO/entity class | Requesition (Library.DAO/SInventory_Entities/Requesition.cs) |
| DAO/entity class | RequsitionChild (Library.DAO/SInventory_Entities/RequsitionChild.cs) |
| DAO/entity class | StockInTransfar (Library.DAO/SInventory_Entities/StockInTransfar.cs) |


## 7. UI Control Inventory
| Control | Type | Label | Required | Default | RO/Enabled | Source | Handler |
|---|---|---|---|---|---|---|---|
| reqNoTextBox, reqDateTextBox | TextBox | Req No/Date | - | from `tblRequisition` | ReadOnly | `GetRequisitionInfoByReqId` | - |
| clnNoTextBox | TextBox | Challan No | - | `"CLN-" + ReqNo.Substring(4)` (`RequisitionBLL.Chalan`) | ReadOnly | code | - |
| clnDateTextBox | TextBox | Challan Date | - | today `dd-MMM-yyyy` | ReadOnly | server | - |
| truckNoTextBox, driverNameTextBox | TextBox | Truck / Driver | none | empty | **ReadOnly** (`.aspx:129,143`) -> cannot be entered here | - | saved as empty |
| issueGridView (`DataKeyNames=ReqChildId`) | GridView | Code, Product Name, Pack Size, Curr. Stock, Unit Price, Req Qty, Pick Qty, Price, VAT, Total Value, Case, Pick | at least one ticked | rows from `GetPickingDataForIssue` | rows with stock < 1: checkbox disabled and pink | latest `tbl_INV_CentralStore_WeightedAvg` Close_Qty / Avg_Rate | `issueCheckBox_CheckedChanged`, `chkSelectAll_CheckedChanged`, `issueQtyTextBox_TextChanged` |
| issueQtyTextBox | TextBox | Pick Qty | - | ReqQty | ReadOnly=True, enabled when row ticked | - | recalculated server-side |
| hfBatchNO, hfIsFromBatch, hdReqId | HiddenField | - | - | from data | hidden | `IsFromBatch` of requisition | selects batch-specific allocation |
| totalAllPriceTextBox, vatAllPriceTextBox, grandTotalTextBox | TextBox | totals | - | - | ReadOnly | `GetCalculationValue` | - |
| bmitButton | LinkButton | Save | see §18 | - | confirm "Are you sure you want Save ?" | - | `bmitButton_Click` |

## 8. Field Mapping
| UI | Entity | DAL param | Column |
|---|---|---|---|
| clnNoTextBox | Requesition.IssueChalanNo / PickingNo | @IssueChalanNo, @PickingNo | tblRequisition.IssueChalanNo / PickingNo |
| clnDateTextBox | IssuChalanDate / SubmitDate / PickingDate | same names | tblRequisition.IssuChalanDate / SubmitDate / PickingDate |
| truck/driver | TruckNo / DriverName | same | tblRequisition.TruckNo / DriverName |
| totals | TotalPrice / TotalVAT / GrandTotalPrice | same | tblRequisition.* |
| constants | Submit='OK', CreatePicking='OK' | @Submit, @CreatePicking | tblRequisition.Submit / CreatePicking |
| row Pick Qty / Price / VAT / Total / Case | IssueQty / PriceAmount / VATAmount / TotalPrice / CaseQty | same | tblRequsitionChild.* (+IsPicking='OK', IsIssue='OK', MusakVATAmount=0, MusakTotalPrice=0) |
| per lot | StockInTransfar (Quantity, PickingQty, BatchNo, ExpDate, MfgDate, ReceiveDate, ReceiveId, PriceAmount, VATAmount, TotalPriceAmount, UnitPrice) | @... | tblStockInTransfar.* (IsIssue='OK') |

## 9. Dropdown Mapping
No dropdowns.

## 10. Button Action Mapping
| Button | Chain | DB change |
|---|---|---|
| Save | `SaveData()` (`.aspx.cs:266-342`): `RequisitionBLL.UpdatePickingInformationOnRequisition` -> `RequisitionDAL.UpdatePickingInformationOnRequisitionDAL`; `UpdateIssueInformationOnRequisitionChild` -> `UpdateCentralStockAndStockInTransfar` (`GetCurrentStockofCentralStock[ByBatch]`, `StockInTransfarInsertDAL`, `UpdateCentralStockStockOut`, `ProcessWeightedAvg` = SP `tbl_INV_CentralStoreStoIssue`) -> `UpdateIssueInformationOnRequisitionChildDAL`; `Clear()`; alert "Data Save Successfully!!"; then popup `ChallanReportViewer?reqId=` | tblRequisition, tblStockInTransfar, tblCentralStore, tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock, tblRequsitionChild |

## 11. Frontend Flow
Postback-only; each tick/qty change recalculates that row and the totals (`PriceCalculation`, `GetCalculationValue`). Client confirm on Save. Print popup after save.

## 12. Backend Flow
| Handler | Source | Flow |
|---|---|---|
| Page_Load | `.aspx.cs:17-37` | `!IsPostBack`: if `GetRequisitionInfofromTransferDAL` finds `tblStockInTransfar` rows for ReqId -> alert " Picking Already Completed!!" (no grid) else `LoadGrid` + `LoadMainInfo` |
| LoadMainInfo | `:51-73` | header, challan no, date, `IsFromBatch` (try/catch swallow) |
| PriceCalculation | `:157-230` | see BR-M07-049/050 |
| SaveData | `:266-342` | header update first, then per ticked row DTO -> allocation |
| bmitButton_Click | `:378-385` | `Validation()` (>=1 ticked else "Please Select an item from List!!") then SaveData + print |
No try/catch/transaction around SaveData.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.GetRequisitionInfoTransferReqId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:201 |
| RequisitionBLL.GetPickingDataForIssue | Library.BLL/SInventory_BLL/RequisitionBLL.cs:191 |
| RequisitionBLL.GetRequisitionInfoByReqId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:196 |
| RequisitionBLL.Chalan | Library.BLL/SInventory_BLL/RequisitionBLL.cs:127 |
| ProductBLL.ProductPriceDetailWithCaseBLL | Library.BLL/SInventory_BLL/ProductBLL.cs:132 |
| RequisitionBLL.UpdatePickingInformationOnRequisition | Library.BLL/SInventory_BLL/RequisitionBLL.cs:215 |
| RequisitionBLL.UpdateIssueInformationOnRequisitionChild | Library.BLL/SInventory_BLL/RequisitionBLL.cs:259 |
| RequisitionBLL.UpdateCentralStockAndStockInTransfar | Library.BLL/SInventory_BLL/RequisitionBLL.cs:282 |
| RequisitionBLL.RequsitionChildToStockInTransfar | Library.BLL/SInventory_BLL/RequisitionBLL.cs:390 |


## 15. Repository Mapping
| DAL | Source | SQL |
|---|---|---|
| RequisitionDAL.GetPickingDataForIssueDAL | `RequisitionDAL.cs:203-224` | RC.* + latest Close_Qty / Avg_Rate from tbl_INV_CentralStore_WeightedAvg by Item_Code |
| RequisitionDAL.GetCurrentStockofCentralStock(ByBatch) | `:305-395` | CTE InboundLots (tbl_INV_Central_BatchWiseStock Trans_Qty>0) minus OutboundSum (Trans_Qty<0 by Issue_Weighted_Avg_Id, Batch) ORDER BY ExpDate, ReceiveDate |
| RequisitionDAL.StockInTransfarInsertDAL | `:425-455` | IF NOT EXISTS (ReqChildId, BatchNo, ReceiveId) INSERT tblStockInTransfar |
| RequisitionDAL.UpdateCentralStockStockOut | `:396-404` | `UPDATE tblCentralStore SET Quantity=@Quantity WHERE ReceiveId=@ReceiveId` |
| RequisitionDAL.ProcessWeightedAvg | `:406-414` | `UpdateAction("tbl_INV_CentralStoreStoIssue")` (exceptions swallowed) |
| RequisitionDAL.UpdatePickingInformationOnRequisitionDAL / UpdateIssueInformationOnRequisitionChildDAL | `:275-300` | UPDATE tblRequisition / tblRequsitionChild |
| ProductDAL.ProductPriceDetailWithCase | `ProductDAL.cs:277-300` | latest Avg_Rate, PcsPerCase, CaseQty, `0 AS VATAmountPerUnit` |

## 16. Stored Procedure Mapping
`tbl_INV_CentralStoreStoIssue(@StockInTransfarId)` - **body in repo** (`SP_Fix_CentralStoreStoIssue.sql`, `SP_Fix_CentralStoreStoIssue_Corrected.sql:15-190`; live version unverified): cursor over the transfer row; opening = latest Close_Qty/Close_Amt of `tbl_INV_CentralStore_WeightedAvg` for company+item; issue row Curr_Qty = -PickingQty at opening average rate; closing qty/amount/avg recomputed; INSERT weighted-avg row only if none exists for (Mst_Trans_Id, Ref_No, Company_Id, Item_Id); INSERT `tbl_INV_Central_BatchWiseStock` negative row 'STO Picking' unless duplicate.

## 17. Database Mapping
READ: tblRequisition, tblRequsitionChild, tblStockInTransfar, tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock, tblProduct, tblProductCase, tblCentralStore. WRITTEN: tblRequisition, tblRequsitionChild, tblStockInTransfar, tblCentralStore, tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock. `View_CentralStoreCurrentStock` referenced only in commented SQL.

## 18. Validation Matrix
| Rule | Layer | Message |
|---|---|---|
| Picking already done | server Page_Load | " Picking Already Completed!!" |
| >=1 line ticked | server `Validation` | "Please Select an item from List!!" |
| Issue qty <= requested and <= current stock | server postback `PriceCalculation` (qty box is ReadOnly) | silently clamped |
| Truck/driver/date | none (date read-only) | - |
| Postback re-check of "already picked" | none | - |

## 19. Business Rules
| BR ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M07-047 | If any `tblStockInTransfar` row exists for the ReqId the page refuses to load lines (" Picking Already Completed!!"); the guard runs only on first load, and `StockInTransfarInsertDAL` additionally skips a lot already inserted for the same ReqChildId+BatchNo+ReceiveId. | `.aspx.cs:17-37`; `RequisitionDAL.cs:425-455` | CONFIRMED |
| BR-M07-048 | Challan number = `CLN-` + ReqNo without its first 4 characters (`REQ-0000123` -> `CLN-0000123`); challan/pick/submit dates = today; PickingNo = challan number. | `RequisitionBLL.cs:112-131`; `.aspx.cs:59-60,266-283` | CONFIRMED |
| BR-M07-049 | Issue quantity per line = requested qty, clamped to min(requested, current stock) where current stock = latest `Close_Qty` of the central weighted-average ledger for the product (not lot/condition specific); lines with stock < 1 cannot be ticked. | `.aspx.cs:88-104,157-190`; `RequisitionDAL.cs:203-224` | CONFIRMED |
| BR-M07-050 | Value calculation: unit price = latest `Avg_Rate` (fallback the unit price column); Price = qty x rate; VAT = qty x VATAmountPerUnit which is **hard-coded 0**; Total = Price + VAT; Case = round(qty / PcsPerCase x CaseQty, 2) with PcsPerCase<=0 -> 1; totals = sums of ticked rows, 2 decimals. Musak VAT fields saved as 0. | `.aspx.cs:157-262`; `ProductDAL.cs:277-300`; `RequisitionBLL.cs:262-280` | CONFIRMED |
| BR-M07-051 | Save order: (1) tblRequisition set Submit='OK', CreatePicking='OK' with challan data and totals; (2) per ticked line allocate central lots; (3) update requisition child. No transaction. | `.aspx.cs:266-342`; `RequisitionBLL.cs:214-280` | CONFIRMED |
| BR-M07-052 | Lot allocation: lots ordered ExpDate, ReceiveDate (or one batch when `IsFromBatch`); remaining = inbound - outbound rows; a lot fully consumed sets `tblCentralStore.Quantity = 0`, the last lot sets `Quantity = remaining after issue`; each lot writes one `tblStockInTransfar` row (Quantity=PickingQty, IsIssue 'OK') and calls `tbl_INV_CentralStoreStoIssue`. Remainder is computed via string trimming of the negative number (`Substring(1)`). | `RequisitionBLL.cs:282-380` | CONFIRMED |
| BR-M07-053 | If total lot stock is less than the entered quantity the loop ends silently and the child line still records the entered IssueQty (over-issue undetected); ProcessWeightedAvg exceptions are swallowed. | `RequisitionBLL.cs:282-380`; `RequisitionDAL.cs:406-414` | CONFIRMED |

## 20. Permission Matrix
No Session/role check; only the master page. Any menu-authorised user can issue any requisition by URL (`ReqId` from QueryString, no ownership check; `Request.QueryString["ReqId"]` passed as parameter).

## 21. Status Lifecycle
`tblRequisition.CreatePicking` ('' / NULL -> 'OK'), `Submit` (-> 'OK'), `SubmitDate`, `PickingDate`; `tblRequsitionChild.IsPicking` -> 'OK', `IsIssue` -> 'OK'; `tblStockInTransfar.IsIssue` = 'OK' at insert. "Already picked" = presence of tblStockInTransfar rows. No rollback in M07 (`RollBackBLL.UpdatePickingInformationOnRequisitionDAL` exists, not linked from this page).

## 22. Error Handling
No try/catch on the save path; `ProcessWeightedAvg` swallowed. Alerts via `alert()`; success text "Data Save Successfully!!".

## 23. Audit
No user/date audit columns written (Entry/Update columns are not part of these UPDATEs); challan date user-fixed to today.

## 24. Integration
Central weighted-average ledger (`tbl_INV_CentralStore_WeightedAvg`); cross-DB `CSTL_ERPDB..Table_Inventory_Types` referenced by the SP (`SP_Fix_CentralStoreStoIssue_Corrected.sql`).

## 25. Reports
`ChallanReportViewer.aspx?reqId=` -> `crpChallan.rpt` (see RPT-M07-02).

## 26. Dependencies
BLL: RequisitionBLL, ProductBLL. DAL: RequisitionDAL, ProductDAL. DAO: Requesition, RequsitionChild, StockInTransfar. SP: `tbl_INV_CentralStoreStoIssue`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-259-01 | open requisition already picked | alert; no grid | 047 |
| TC-259-02 | challan number | REQ-0000123 -> CLN-0000123 | 048 |
| TC-259-03 | stock < 1 | checkbox disabled | 049 |
| TC-259-04 | requested 50, stock 30 | pick qty 30 | 049 |
| TC-259-05 | price/VAT/case | qty 10 rate 12.5 -> price 125.00, VAT 0.00, total 125.00 | 050 |
| TC-259-06 | save without ticks | "Please Select an item from List!!" | - |
| TC-259-07 | save spanning two lots | 2 tblStockInTransfar rows, first lot Quantity 0 | 052 |
| TC-259-08 | exact lot consumption | remainder 0 branch | 052 |
| TC-259-09 | second save on same req | blocked on reload; duplicate lot insert skipped | 047 |
| TC-259-10 | failure in step 2 | header already Submit='OK' | 051 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx`, `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | - | MISSING |
| VAT on challan | None existed | always 0 | GAP |
| Transactionality | None existed | none | GAP |

## 30. Known Gaps
GAP-M07-028 (order, no txn, over-issue, VAT 0), GAP-M07-029 (truck/driver read-only), GAP-M07-032 (duplicated BLL), GAP-M07-034 (SP live version). Also: `IsFromBatch` parse swallow; `Substring(1)` remainder logic; two markup copies of the form (second inside a comment).

## 31. Change Impact
`RequisitionBLL.UpdateCentralStockAndStockInTransfar`/`ProcessWeightedAvg` shared with other requisition issue pages (grep); `tbl_INV_CentralStore_WeightedAvg` feeds stock valuation reports (`SP_Update_StockValuation_TransactionDate_Logic.sql`). Downstream DC receipt reads `tblStockInTransfar`.
