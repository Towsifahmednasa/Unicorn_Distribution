---
page_id: PAGE-420
module: M11
page: StockTransferOrder
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-420 — StockTransferOrder (STO request: warehouse → DC)

> **Evidence level:** Tier A — `StockTransferOrder.aspx.cs` read in full (914 lines), control inventory of `.aspx`, `RequisitionBLL` (save, numbering, issue/pick logic) and `RequisitionDAL` (insert/update/delete of requisition + `StockInTransfarInsertDAL`, `ProcessWeightedAvg`), `ChalanDAL.GetProductWhStore`, and the two SP scripts `SP_Fix_CentralStoreStoIssue.sql` / `SP_Fix_CentralStoreStoIssue_Corrected.sql` (both read fully; they define `tbl_INV_CentralStoreStoIssue`, called later in the STO chain). UI PARTIAL: the markup contains two overlapping product-grid designs (edit-by-code vs batch pick) and dead upload controls; only IDs/handlers were inventoried.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-420 |
| Module | M11 (entry point of the STO chain; downstream pages belong to M06/M07) |
| Name / class | StockTransferOrder / `SInventory_UI_StockTransferOrder` (`.aspx.cs:17`) |
| Kind | Entry / transaction — **no stock movement** at this step |
| Size | aspx 676 lines, code-behind 914 lines |

## 2. Purpose
CONFIRMED (title/prompts "Are you sure you want to Create St…", requisition numbers `REQ-`): the central warehouse or logistic user raises a **Stock Transport Order (STO) / requisition** asking a warehouse to send stated products and batches to a Distribution Center.

## 3. Business Objective
INFERRED: formalise replenishment of a DC from the central warehouse: the request (`tblRequisition` + `tblRequsitionChild`) is later picked (PAGE-259), issued (PAGE-219), shipped and received by the DC (PAGE-413), where central-store stock, weighted-average cost and DC stock change.

## 4. Navigation
Inbound: `OrderRequisitionView.aspx` (`?MasID=` edit link). Outbound: `OrderRequisitionView.aspx` (list button, `ListImageButton_Click`), `OrderRequisitionCreation.aspx` (cancel).

## 5. Route
`~/SInventory_UI/StockTransferOrder.aspx[?MasID=<ReqId>]`.

## 6. Source Files
| Type | Path |
|---|---|
| View/code-behind | Solution.Web/SInventory_UI/StockTransferOrder.aspx(.cs) |
| BLL | RequisitionBLL.cs (`SaveRequsition` :~19-26, `SaveRequsitionChild` :32-42, `ReqNo`/`ReqnNoGenerator` :72-110, `UpdateCentralStockAndStockInTransfar` :281-388), ChalanBLL.GetProductWhStore, DCStoreBLL (`ProductInfo`, `GetProductStock`) |
| DAL | RequisitionDAL.cs (`SaveReuqisitionDAL` :85-, `SaveReuqisitionChildDAL` :123-, `UpdateManufacturerInfo` :67-, `DeleteRequisitionDtls` :114-, `StockInTransfarInsertDAL` :425-450, `UpdateCentralStockStockOut` :396-404, `ProcessWeightedAvg` :406-414); ChalanDAL.GetProductWhStore :619-629; DCStoreDAL.GetProductStock :90- |
| SQL | SP_Fix_CentralStoreStoIssue.sql; SP_Fix_CentralStoreStoIssue_Corrected.sql |
| Web service | SInventoryWebService.asmx `GetProductWithCode` (autocomplete, edit grid) |

## 7. UI Control Inventory
| Control | Type / label | Required | Default / state | Source / notes |
|---|---|---|---|---|
| manufacturerDropDownList | DropDownList | Yes | index 1 | `RequisitionBLL.LoadmanufacturerName` (`tblManufacturer`); changing it calls `InitialGrid()` |
| reqNoTextBox | TextBox | — | ReadOnly, `ReqNo()` = `REQ-` + 7-digit(MAX(ReqId)+1) | display only — regenerated at save |
| reqDateTextBox | TextBox | — | today; ReadOnly in the edit-grid variant | |
| wareHouseDropDownList | DropDownList | Yes | index 1 | `tblWearhouse` |
| dcDropDownList | DropDownList | Yes | — | `tblCompanyUnit` as "code:name" (all DCs, no user scope) |
| txtRemarks | TextBox | — | — | **not read by code-behind — never saved** |
| id_fu / btnUpload / lbl_up_status / IsFileUploaded | Excel upload | — | — | `ExcelToGrid` (:705-762) saves the file under `~/ExcelFiles/<name>` and reads first sheet via Jet/ACE OLEDB into `productGridView` (unvalidated file name/extension) |
| prodctCodeTextBox + Button1 | product search | required to search | — | `ChalanBLL.GetProductWhStore(code)` → central store batches with `Quantity>0` |
| productGridView `DataKeyNames=ReceiveId` | GridView | — | — | cols: select, code, name, pack, stock qty (`Cells[4]`), batch, `transferQtyTextBox` (digits, AutoPostBack → `dQtyTextBox_TextChanged`) |
| addButton | "Add" | — | — | `addButton_Click` :842-900 |
| chalanGridView `DataKeyNames=ReceiveId` | GridView of request lines | ≥1 row (new mode) | — | productCodeTextBox (RO), productNameTextBox, packSizeTextBox (RO), txtBatchNo (RO), reqQtyTextBox (RO, digits+'.') |
| submitButton | LinkButton/Button, `confirm('Are you sure you want to Create St…')` | — | text "Update" when `MasID` present; hidden if `GetPermissionForShowBtn` returns rows | `submitButton_Click` |
| hiddenField | HiddenField | — | ReqId in edit mode | |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | SQL param | Table.column |
|---|---|---|---|---|---|---|
| (generated) | `Requesition.ReqId = MAX(ReqId)+1`, `ReqNo = ReqNo()` | `SaveAllData` :356-371 | `SaveRequsition` :19-26 | `SaveReuqisitionDAL` | `@ReqId`,`@ReqNo` | tblRequisition.ReqId, ReqNo |
| reqDateTextBox | `ReqDate` | :360 | | | `@ReqDate` | ReqDate |
| wareHouseDropDownList | `WarehouseId`, `WearhouseName` | :361-362 | | | | WarehouseId, WearhouseName |
| dcDropDownList | `ComUnitId`, `ComUnitCode`, `ComUnitName` (split of "code:name") | :349,363-365 | | | | ComUnitId, ComUnitCode, ComUnitName |
| Session LoginName | `EntryBy`, `EntryDate=Now` | :366-367 | | | | EntryBy, EntryDate |
| manufacturerDropDownList | `ManufacId` | :368 | | | | ManufacId |
| const true | `IsFromBatch` | :369 | | | | IsFromBatch |
| chalanGridView row | `RequsitionChild.ProductCode` (upper), ProductName, PackSize, `BatchNo`, `ReqQty`, `ReqId` | :375-393 | `SaveRequsitionChild` (`ReqChildId = MAX+1`) | `SaveReuqisitionChildDAL` :123- | `@ProductCode,@ProductName,@PackSize,@BatchNO,@ReqQty,@ReqId,@ReqChildId` | tblRequsitionChild.* |
| edit mode header | `UpdateBy`, `UpdateDate` | :400-413 | `UpdateManufacturerInfo` | :67- | | tblRequisition (ReqNo, ReqDate, WarehouseId, ComUnit*, UpdateBy/Date, ManufacId…) |
| edit mode lines | delete then re-insert | :416-436 | `DeleteRequisitionDtls`, `SaveRequsitionChild` | :114-,123- | | tblRequsitionChild (**BatchNo not re-supplied → NULL**) |

## 9. Dropdown Mapping
Manufacturer ← `tblManufacturer`; Warehouse ← `tblWearhouse` (all rows); DC ← `tblCompanyUnit` (all rows, `ComUnitCode:ComUnitName`). No filtering by user.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Show (Button1) | `Button1_Click` :798-822 | shows central-store batches of the product (Quantity>0) or "No Product Found!!" / "Please select product !!" |
| Add | `addButton_Click` | **rebinds `chalanGridView` from ticked rows with qty** (it first clears the grid, so previously added rows are lost — the subsequent "append existing" loop never runs) |
| Save / Update | `submitButton_Click` :628-643 | `Validation()` → `SaveAllData()`; failure → "Select Manufacturer Name!!" |
| Upload | `btnUpload_Click` | Excel import into `productGridView` |
| List / Cancel | redirects | |
| edit-grid buttons ImageButton1/2 | `AddRowInGrid`, `ImageButton2_Click` | add/remove blank product rows (edit mode) |

## 11. Frontend Flow
New: choose manufacturer/warehouse/DC → type product code → Show → tick batch rows, enter transfer qty (postback checks vs stock) → Add → Save. Edit (`?MasID`): `GetOneRecord` fills header and `productGridView` with the existing lines; save deletes and recreates lines. Autocomplete `GetProductWithCode` for code/name typing (edit variant).

## 12. Backend Flow
`SaveAllData` (:347-446): new mode: `SaveRequsition(out maxReqId)` (id and number regenerated in BLL) then `SaveRequsitionChild(list)` — **the header save result is not checked**; edit mode: `UpdateManufacturerInfo` → if true `DeleteRequisitionDtls` → re-insert lines. Then `Clear()` and alert message "Save Data Successfully!!". No try/catch, no transaction. Email helpers `email()`/`email2()` (hard-coded SMTP host, sender, recipients and a **hard-coded SMTP password** `<redacted>` at `.aspx.cs:589,615`) are never called (calls commented out at :634-635).

Downstream chain (other pages, verified in BLL): pick/issue (`RequisitionBLL.UpdateCentralStockAndStockInTransfar`, called from PAGE-259): for each requisition child, walk `tblCentralStore` batches (by batch if `IsFromBatch`): insert `tblStockInTransfar` (StockInTransfarId, ReqId, ReqChildId, product, batch, Quantity=PickingQty, UnitPrice, PriceAmount, VATAmount, TotalPriceAmount, Exp/Mfg/ReceiveDate, ReceiveId, IsIssue='OK') guarded by `IF NOT EXISTS (ReqChildId, BatchNo, ReceiveId)`; set `tblCentralStore.Quantity` = remaining (0 when exhausted); call `tbl_INV_CentralStoreStoIssue(@StockInTransfarId)` (weighted-average and batch ledger, see §16).

## 13. API Mapping
ASMX `GetProductWithCode`.

## 14. Service Mapping
`RequisitionBLL`: `ReqNo`, `SaveRequsition`, `SaveRequsitionChild`, `UpdateManufacturerInfo`, `DeleteRequisitionDtls`, `GetDataInfoByIdBll`, `GetDataInfoByIdBllDtls`, `LoadmanufacturerName`, `WareHouseLoad`, `DCLoad`; `ChalanBLL.GetProductWhStore`; `DCStoreBLL.ProductInfo/GetProductStock`.

## 15. Repository Mapping
`GetProductWhStore` (:619-629): `SELECT CH.ReceiveId, ProductCode PCode, ProductName PName, Quantity StockQty, BatchNo, ExpDate, ReceiveDate, PackSize FROM tblCentralStore CH ⟕ tblUnitPrice UP ON ProductId WHERE Quantity>0 AND ProductCode=@ProductCode ORDER BY BatchNo` (join to `tblUnitPrice` duplicates rows when several price rows exist; not filtered by warehouse). `SaveReuqisitionDAL`, `SaveReuqisitionChildDAL` parameterised INSERTs. `GetProductStock` (DCStoreDAL :90-): `TOP 1 Close_Qty FROM tbl_INV_CentralStore_WeightedAvg WHERE Item_Id=@ProductId ...`.

## 16. Stored Procedure Mapping
This page calls no SP. The STO chain calls **`tbl_INV_CentralStoreStoIssue(@StockInTransfarId)`** (a stored procedure with a table-like name) via `RequisitionDAL.ProcessWeightedAvg` (:406-414). Two definitions exist:
| Aspect | `SP_Fix_CentralStoreStoIssue.sql` (original "FIXED") | `SP_Fix_CentralStoreStoIssue_Corrected.sql` |
|---|---|---|
| Cursor source | `tblStockInTransfar WM ⟕ tblRequisition RQ ⟕ Table_Inventory_TranscationTypes ⟕ CSTL_ERPDB..Table_Inventory_Types ⨝ tblProduct ⨝ tblWearhouse(WH.CompanyId=PD.CompanyId) ⟕ latest tbl_INV_CentralStore_WeightedAvg` | identical |
| Weighted-avg row | inserts `tbl_INV_CentralStore_WeightedAvg` (Mst_Trans_Id=StockInTransfarId, Trans_Id=ReceiveId, Ref_No=ReqNo, Curr_Qty=−PickingQty, Curr_Rate=opening rate, Close_Qty/Avg_Rate/Close_Amt recomputed) **only if none exists** for (Mst_Trans_Id, Ref_No, Company, Item); if one exists `@Weighted_Avg_Id_New` stays 0 ⇒ **no batch row is written** | selects the existing row's id if present, inserts otherwise |
| Batch ledger row (`tbl_INV_Central_BatchWiseStock`, Trans_Qty=−PickingQty, Remarks 'STO Picking', StockInTransfarId) | `Weighted_Avg_Id = parent inbound Weighted_Avg_Id` (looked up by `Batch_Stock_Id=@ReceiveId`, fallback `TOP 1` by `Batch_No` with Trans_Qty>0, unordered); `Issue_Weighted_Avg_Id = new issue WA id` | `Weighted_Avg_Id = NEW issue WA id`; `Issue_Weighted_Avg_Id = parent inbound id (or 0)`; fallback lookup by `Item_Code` + `Batch_No` ordered `Batch_Stock_Id DESC`; **duplicate guard** (`Weighted_Avg_Id, StockInTransfarId, Batch_No`) |
| Net difference | parent/issue semantics: issue row points *to parent* via Weighted_Avg_Id | semantics **swapped**: issue row owns the id, parent moves to `Issue_Weighted_Avg_Id`; re-running is idempotent |
This is a **spec conflict** (GAP-M11-002): consumers of `tbl_INV_Central_BatchWiseStock` (`GetCurrentStockofCentralStockByBatch`, `RequisitionDAL` inbound/outbound sums join on `Weighted_Avg_Id`/`Issue_Weighted_Avg_Id`) assume one convention. Which script is live is UNKNOWN.

## 17. Database Mapping
- **tblRequisition** (W): ReqId, ReqNo, ReqDate, WarehouseId, WearhouseName, ComUnitId, ComUnitCode, ComUnitName, ManufacId, EntryBy, EntryDate, IsFromBatch; later columns (Submit, SubmitDate, ReceiveIssue, ReceiveIssueDate, IssueChalanNo, IssuChalanDate, PickingNo…) set by other pages. Edit: UpdateBy, UpdateDate.
- **tblRequsitionChild** (W): ReqChildId, ProductCode, ProductName, PackSize, ReqQty, ReqId, BatchNO.
- R: tblCentralStore (ReceiveId, ProductCode, Quantity, BatchNo, ExpDate, ReceiveDate, PackSize), tblUnitPrice, tbl_INV_CentralStore_WeightedAvg.
- **Stock effects of this page: none.** Downstream (documented, other pages): `tblCentralStore.Quantity` reduced; `tblStockInTransfar` rows created; weighted-average and batch ledger rows written; later DC `tblDCStore` Receive row (PAGE-413).

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| manufacturer selected | `Validation` :450-456 | Please Select Manufacturer !!! |
| warehouse selected | :457-463 | Please Select WareHouse !!! |
| DC selected | :464-470 | Please Select Distribution Center  !!! |
| every chalanGrid row has Req.Qty | :472-483 | Please fill out Req.Qty!! |
| product code / name non-empty | :484-507 | Please fill out productCode!! / productName!! |
| any failure | `submitButton_Click` :638-641 | Select Manufacturer Name!! |
| STO qty ≤ stock | `dQtyTextBox_TextChanged` :764-796 | STO Qty. cantbe more then Stock Quantity |
| search without code | `Button1_Click` | Please select product !! |
| Not validated | | qty >0 and numeric (Convert.ToDecimal on text), duplicate lines, uploaded file type/size, edit-mode grid validation (validation loops `chalanGridView`, which is empty in edit mode) |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M11-015 | An STO needs manufacturer, warehouse, DC and at least one line with code, name and requested qty; requisition number is system-generated `REQ-`+7 digits from `MAX(ReqId)+1`. | StockTransferOrder.aspx.cs:450-507; RequisitionBLL.cs:66-110 | code-behind + BLL | CONFIRMED |
| BR-M11-016 | Lines are created **batch-specific** (`IsFromBatch=true`, `BatchNO` from the picked central-store batch) so pick/issue draws from that batch first. | .aspx.cs:369,383-389; RequisitionBLL.cs:287-296 | code-behind + BLL | CONFIRMED |
| BR-M11-017 | Requested qty ≤ displayed central stock is checked only on postback; not re-checked at save or at pick (pick spills to next batch by FIFO `Quantity` order when short). | .aspx.cs:764-796; RequisitionBLL.cs:300-386 | code-behind + BLL | CONFIRMED |
| BR-M11-018 | Saving an STO does not move stock; stock moves at picking/issue (central store `Quantity` set to remaining; weighted-average issue at the running average rate). | RequisitionBLL.cs:281-388; SP_Fix_CentralStoreStoIssue_Corrected.sql | BLL + SP | CONFIRMED |
| BR-M11-019 | Editing an STO replaces all its lines; batch numbers are not carried into the re-inserted lines (BatchNO NULL). | .aspx.cs:414-439 | code-behind | CONFIRMED |
| BR-M11-020 | Central-store stock shown for search is `Quantity>0` per receive lot for the product regardless of warehouse selected. | ChalanDAL.cs:619-629 | DAL | CONFIRMED |
| BR-M11-021 | STO issue costing: `tbl_INV_CentralStoreStoIssue` values the issue at the **latest weighted-average closing rate** of the product/company (`Close_Amt/Close_Qty` of last row), producing `Curr_Amt = −qty × rate`; new average = closing amount/closing qty (0 when qty ≤ 0). | SP_Fix_CentralStoreStoIssue_Corrected.sql:99-121 | SP | CONFIRMED |

## 20. Permission Matrix
`GetPermissionForShowBtn(path)` hides Save when the permission SP returns rows (CommonDataLoad — not read). `Session["LoginName"]` used as EntryBy. No role/DC/warehouse scoping; any user can request any warehouse→DC.

## 21. Status Lifecycle
Set here: none (row created with `Submit`, `ReceiveIssue` NULL). Chain (verified in BLL/DAL): `tblRequsitionChild.IsIssue='OK'`, `tblStockInTransfar.IsIssue='OK'` (pick), `tblRequisition.Submit='OK'` (issue), `tblStockInTransfar.IsTransfared='OK'` and `tblRequisition.ReceiveIssue='OK', ReceiveIssueDate` (DC receipt, `RequisitionDAL.cs:574-592`).

## 22. Error Handling
No try/catch on save; `Convert` errors surface as ASP.NET errors. Excel upload: no validation, writes to server disk. Save result of header not checked.

## 23. Audit
EntryBy/EntryDate; UpdateBy/UpdateDate on edit. No delete audit here.

## 24. Integration
SAP not called from this page. The DC receipt (PAGE-413) later calls the SAP REST helper with the STO/challan no.

## 25. Reports
`StockTransfarOrderReportViewer.aspx` → `rptStockTransfarOrder.rpt` (RPT-M11-02).

## 26. Dependencies
tblCentralStore, tbl_INV_CentralStore_WeightedAvg, SInventoryWebService, OLEDB Jet/ACE (Excel upload), `~/ExcelFiles` folder.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Manufacturer/WH/DC set, search product, tick batch, qty 50, Add, Save | tblRequisition + tblRequsitionChild (BatchNO set); no stock change; message | 015,016,018 |
| T2 | Negative | Save with no lines | no message: validation loops over zero rows and passes; an empty requisition is saved (defect) | 015 |
| T3 | Negative | Qty > stock | alert, qty cleared | 017 |
| T4 | Edge | Search, Add, search another product, Add | first product lost (grid cleared) | — |
| T5 | Edge | Edit existing STO, save | lines recreated with BatchNO NULL | 019 |
| T6 | Edge | Remarks entered | not persisted | — |
| T7 | Integration | Pick after STO with insufficient batch | spill to next batch; `tbl_INV_CentralStoreStoIssue` per StockInTransfar row | 018,021 |
| T8 | Integration | Run STO issue twice for same StockInTransfarId with original vs Corrected SP | original: no second batch row; corrected: guarded duplicate | GAP-M11-002 |

## 28. Reverse Engineering References
`StockTransferOrder.aspx.cs`; `RequisitionBLL.cs:19-110,281-388`; `RequisitionDAL.cs:67-135,396-450,574-592`; `SP_Fix_CentralStoreStoIssue*.sql`.

## 29. Spec vs Actual
None existed. SP header of the corrected script lists no change log; the original's header lists "Fixes 1-3" (column mapping, lookup of Ref_Weighted_Avg_Id, no duplicate join) — the corrected variant changes those semantics again (§16).

## 30. Known Gaps
Empty STO can be saved; `txtRemarks` not saved; Add clears previously added rows; edit loses batch; header save result unchecked; MAX+1 ids; hard-coded SMTP credentials and recipients in dead code (`.aspx.cs:583-589,609-615`, value `<redacted>`); Excel upload writes user-named files to `~/ExcelFiles` without validation (path traversal/overwrite risk); SP conflict on batch ledger semantics.

## 31. Change Impact
`RequisitionBLL.UpdateCentralStockAndStockInTransfar` and `tbl_INV_CentralStoreStoIssue` are shared with PAGE-259/219 (M06/M07) and warehouse stock reports; `tblRequisition` fields set here drive `StockReceiveByDC` (list of receivable STOs by `ComUnitId`, `Submit='OK' AND ReceiveIssue IS NULL/''`).
