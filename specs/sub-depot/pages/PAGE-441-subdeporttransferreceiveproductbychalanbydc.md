---
page_id: PAGE-441
module: M12
page: SubdeportTransferReceiveProductByChalanByDC
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-441 — SubdeportTransferReceiveProductByChalanByDC

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-441 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubdeportTransferReceiveProductByChalanByDC |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SubdeportTransferReceiveProductByChalanByDC |
| aspx size | 12217 bytes |
| Code-behind LOC | 179 |
| Inline script size | 984 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**SubdeportTransferReceiveProductByChalanByDC** (caption `Receive Product from Chalan By DC`, `.aspx:25`, folder `SInventory_UI`) is the DC's receipt screen for a Sub-depot->DC return chalan. It is opened from `SubDeportTransferStockReceiveByDC.aspx` (`?SChalanId=`), shows the return chalan lines with `Rcv Qty` / `Un Rcv Qty` and on Submit creates new DC stock rows in `tblDCStore`, flags the return chalan delivered and (for un-received qty) creates `Restricted` rows in `tblDCStoreFreeze`. CONFIRMED (`SubdeportTransferReceiveProductByChalanByDC.aspx.cs:58-118`).

## 3. Business Objective
Bring returned sub-depot goods back into the parent DC's stock and quarantine damaged quantities, closing the return chalan (WF-M12-02). INFERRED from labels/flow. Note it books the stock into a **hard-coded DC (ComUnitId 6)**, not into the DC the chalan was addressed to (BR-M12-018).

## 4. Navigation
Reached from `SubDeportTransferStockReceiveByDC.aspx` (`DataNavigateUrlFormatString="SubdeportTransferReceiveProductByChalanByDC.aspx?SChalanId={0}"`, `SubDeportTransferStockReceiveByDC.aspx:123-125`). `backLinkButton_Click` redirects to `TransferStockReceiveByDC.aspx` (main DC page, not the sub-depot list - `.cs:173-176`). The `Popup` to the receive report is commented out (`.cs:116`). Master `NewMasterPage.master`.

## 5. Route
`~/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx |
| Code-behind | Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ChalanBLL (Library.BLL/SInventory_BLL/ChalanBLL.cs) |
| DAL class | ChalanDAL (Library.DAL/SInventory_DAL/ChalanDAL.cs) |
| DAO/entity class | DCStockNew (Library.DAO/SInventory_Entities/DCStockNew.cs) |
| DAO/entity class | DCStoreFreezeDAO (Library.DAO/SInventory_Entities/DCStoreFreezeDAO.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Handler |
|---|---|---|---|---|---|---|
| clnNoTextBox | Chalan No | - | ChalanNo | - | `ChalanBLL.SubGetChalanReceieve(SChalanId)` (`.cs:40-50`) | - |
| clnDateTextBox | Chalan Date | - | dd-MMM-yyyy | - | same | - |
| rcvDateTextBox | Receive Date | parseable date | today | - | system | - |
| truckTextBox / driverNameTextBox | Truck No / Driver Name | - | from chalan | - | same | - |
| hdComUnitId | (hidden) | - | `ComUnitId` of the joined `tblDCStore` row | hidden | same | - |
| hdReqId | (hidden) | - | ChalanNo | hidden | same | - |
| rcvGridView | Product Code, Name, Pack Size, Batch No, Quantity, ExpDate, Receive Date, Rcv Qty, Un Rcv Qty, Mfg date | - | - | DataKeys `SChalanId,SChalanDetailsId,DCStoreId` | `ChalanDAL.SubChalanReceieve` | `gv_DocumentUpload_PreRender` |
| rcvQtyTextBox | Rcv Qty | numeric | `Eval("Quantity")` | digits (FilteredTextBoxExtender) | - | `rcvQtyTextBox_TextChanged` (:162) |
| damageTextBox | Un Rcv Qty | numeric | 0 | digits | - | `damageTextBox_TextChanged` (:151) |
| submitButton | Submit | - | - | JS `sweetAlertConfirm_Submit` | - | `submitButton_Click` (:127) |
| backLinkButton | Back | - | - | - | - | :173 |

## 8. Field Mapping
| UI control / source | DTO (`DCStockNew`) | Handler | BLL | DAL | Table.column |
|---|---|---|---|---|---|
| cells 0-4 | ProductCode, ProductName, PackSize, BatchNo, TotalQuantity | `Save()` :67-71 | `ChalanBLL.DCStockInSub` (:36) | `ChalanDAL.DCStockInDALMainSub` :107 (params `DcStockParameters(...,true)` :631) | `tblDCStore.ProductCode/ProductName/PackSize/BatchNo/TotalQuantity` |
| rcvQtyTextBox | StockQty | :72 | same | same | `tblDCStore.StockQty` |
| damageTextBox | DamageQty | :73 | same | same | `tblDCStore.DamageQty` |
| cell5 / cell9 / cell6 | ExpDate / mfgdate / ReceiveDate | :74-76 | same | same | `ExpDate`, `MfgDate`, `ReceiveDate` |
| rcvDateTextBox | StockRcvDate | :77 | same | same | `StockRcvDate` |
| clnNo / clnDate | ChalanNo, ChalanDate | :78-79 | same | same | `ChalanNo`, `ChalanDate` |
| **constant 6** | ComUnitId | :80 | same | same | `tblDCStore.ComUnitId` |
| DataKeys[1] | ChalanDetailsId | :81 | same | same | `tblDCStore.SChalanDetailsId` |
| generated | DCStoreId = MAX(DCStoreId)+1 | BLL:41 | `PrimaryKeyMax` | - | `tblDCStore.DCStoreId` |
| constant | StockCondition | - | - | DAL:111 | `'Available'` |
| DataKeys[0] | ChalanId | :82 | `ChalanBLL.DCStockInSub` -> `SubChalanUpdate` | `ChalanDAL.SubChalanUpdate` :70 | `tblSubDepotChalanReturnInfo.IsDeliver='True'` |
| damage>0 -> DCStoreFreezeDAO | DCStoreId (=new tblDCStore id), qty=damage, `Restricted`, ComUnitId=hdComUnitId | :86-111 | `ChalanBLL.SaveDCStoreFreeze2` (:47) | `ChalanDAL.SaveDCStoreFreeze2` :78 | `tblDCStoreFreeze` (columns per DAL: DCStoreFreezeId, ..., StockQty, StockCondition, ComUnitId, DCStoreId, ChalanDetailsId) |
The `ChalanDetailsId` is written to `tblDCStore.SChalanDetailsId` (sub-depot chalan line id), unlike main receive which writes `ChalanDetailsId`.

## 9. Dropdown Mapping
No dropdowns. Lines come from `ChalanDAL.SubChalanReceieve` (`tblSubDepotChalanReturnInfo` LEFT JOIN `tblSubDepotChalanRetuenDetail`, `tblProduct`, `tblDCStore ON RetuenDetail.DCStoreId = tblDCStore.DCStoreId`; `WHERE IsDeliver='False' AND SChalanId=@`; `ChalanDAL.cs:288-298`) - the last join is semantically wrong (BR-M12-049).

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL | DB change |
|---|---|---|---|---|
| Submit (JS confirm) | `submitButton_Click` :127 -> `Save()` :58 | per line `ChalanBLL.DCStockInSub`: `PrimaryKeyMax("DCStoreId","tblDCStore")`, `DCStockInDALMainSub` (parameterised INSERT), `SubChalanUpdate` (UPDATE); damage>0: `ChalanBLL.SaveDCStoreFreeze2` | INSERT tblDCStore; UPDATE tblSubDepotChalanReturnInfo SET IsDeliver='True'; INSERT tblDCStoreFreeze | 3 tables |
| Back | :173 | none | none | none |
Alert `Data Save Successfully!!`, then `Clear()`.

## 11. Frontend Flow
As PAGE-446: preload from query string, rcv/damage post-back arithmetic (BR-M12-015), `sweetAlertConfirm_Submit` (swal) confirmation, no popup after save. JS: `pageLoad()` select2 init only (`.aspx:49`).

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `Page_Load` | :18-26 | `Request.QueryString["SChalanId"]` -> `LoadInfo`, `LoadGrid` (throws if no pending rows) | no | no |
| `Save` | :58-118 | loop lines: DCStockNew(ComUnitId **6**, DCStoreId generated) -> `DCStockInSub` (INSERT + flag chalan delivered per line) -> damage>0 -> `SaveDCStoreFreeze2` (`ComUnitId = Convert.ToInt32(hdComUnitId.Value)`) | no | no |
INSERTs here use `SInventorySql` (parameterised, exceptions propagate) - contrast the sub-depot side.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ChalanBLL.SubGetChalanReceieve | Library.BLL/SInventory_BLL/ChalanBLL.cs:80 |
| ChalanBLL.DCStockInSub | Library.BLL/SInventory_BLL/ChalanBLL.cs:37 |
| ChalanBLL.SaveDCStoreFreeze2 | Library.BLL/SInventory_BLL/ChalanBLL.cs:48 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ChalanDAL.SubChalanReceieve | Library.DAL/SInventory_DAL/ChalanDAL.cs:289 | inline SQL: tblDCStore, tblProduct, tblSubDepotChalanRetuenDetail, tblSubDepotChalanReturnInfo | @SChalanId |
| ChalanDAL.DCStockInDALMainSub | Library.DAL/SInventory_DAL/ChalanDAL.cs:108 | inline SQL: tblDCStore |  |
| ChalanDAL.DcStockParameters | Library.DAL/SInventory_DAL/ChalanDAL.cs:632 | delegates / no SQL literal | @BatchNo, @ChalanDate, @ChalanDetailsId, @ChalanNo, @ComUnitId, @DCStoreId, @DamageQty, @ExpDate, @M |
| ChalanDAL.SubChalanUpdate | Library.DAL/SInventory_DAL/ChalanDAL.cs:71 | inline SQL: tblSubDepotChalanReturnInfo | @SChalanId |
| ChalanDAL.SaveDCStoreFreeze2 | Library.DAL/SInventory_DAL/ChalanDAL.cs:79 | inline SQL: tblDCStoreFreeze | @BatchNo, @ChalanDate, @ChalanDetailsId, @ChalanNo, @ComUnitId, @DCStoreFreezeId, @DCStoreId, @Damag |

## 16. Stored Procedure Mapping
None called. All SQL inline parameterised via `SInventorySql`/`ChalanDAL` (`ChalanDAL.cs:70-77,107-113,288-298,631-658`). Body-level verification COMPLETE for the write path.

## 17. Database Mapping
| Table | Access | Columns | Source |
|---|---|---|---|
| tblSubDepotChalanReturnInfo | SELECT, UPDATE | header; IsDeliver -> 'True' | `ChalanDAL.cs:70-77,288-298` |
| tblSubDepotChalanRetuenDetail | SELECT | lines | :288-298 |
| tblDCStore | SELECT (wrong join), INSERT | DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo, TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate, ComUnitId(=6), StockQty, DamageQty, StockRcvDate, SChalanDetailsId, StockCondition('Available') | `ChalanDAL.cs:107-113,631-658` |
| tblDCStoreFreeze | INSERT | Restricted freeze row | `ChalanDAL.cs:78-105` |
| tblProduct | SELECT | name | - |
No `Trans_Type`, `CostPerUnit`, `TotalCostAmount`, `ReferenceDCStoreId` are written (contrast `ChalanDAL.DCStockInDALMain` :122-160 used by main B2B receive).

## 18. Validation Matrix
Same as PAGE-446 §18 (numeric parsing only; no reconciliation; no still-pending check at save). Confirmation via swal dialog. No page-specific messages beyond `Data Save Successfully!!`.

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-015 | Received qty and damaged/un-received qty textboxes drive each other on post-back (rcv = sent - damage; damage = sent - rcv). There is no server-side check that rcv + damage = sent, that values are numeric, or that neither is negative/greater than sent. | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs:139-160,51-61; Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs:151-172 | damageTextBox_TextChanged / rcvQtyTextBox_TextChanged | CONFIRMED |
| BR-M12-017 | A chalan is flagged delivered (IsDeliver='True') as a side-effect of saving the FIRST received line (ChalanUpdate is called inside DCStockIn2 for every line). There is no partial-receipt state, no cancel/reject path and no sent-vs-received reconciliation. | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:26-36; Library.BLL/SInventory_BLL/ChalanBLL.cs:36-46; Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:30-34 | DCStockIn2 / DCStockInSub | CONFIRMED |
| BR-M12-018 | DC receipt of a Sub-depot->DC return inserts a new tblDCStore row per line (plain INSERT with SChalanDetailsId, StockCondition 'Available', no Trans_Type/CostPerUnit/TotalCostAmount) and sets tblSubDepotChalanReturnInfo.IsDeliver='True'. The receiving ComUnitId is hard-coded to 6 for the stock row. | Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs:65-85; Library.BLL/SInventory_BLL/ChalanBLL.cs:36-46; Library.DAL/SInventory_DAL/ChalanDAL.cs:70-77,107-113,631-658 | SubdeportTransferReceiveProductByChalanByDC.Save -> ChalanBLL.DCStockInSub | CONFIRMED |
| BR-M12-019 | The DC-side list of pending Sub-depot->DC returns is not filtered by DC or sub-depot: `SubdeportChalanLoadInReceive(comunitId)` ignores its argument and returns every return chalan with IsDeliver NULL/false/0. | Library.DAL/SInventory_DAL/ChalanDAL.cs:269-273; Solution.Web/SInventory_UI/SubDeportTransferStockReceiveByDC.aspx.cs:35-51,60-71 | ChalanDAL.SubdeportChalanLoadInReceive | CONFIRMED |
| BR-M12-020 | DC-side damaged qty on a return receipt is stored in tblDCStoreFreeze as 'Restricted' (StockQty = damage), with ComUnitId taken from the hidden `hdComUnitId` (= ComUnitId of the tblDCStore row joined to the return line by DCStoreId, which actually holds a SubDCStoreId - see GAP-M12-013) rather than the hard-coded 6 used for the stock row. | Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs:40-50,86-112; Library.DAL/SInventory_DAL/ChalanDAL.cs:288-298 | SubdeportTransferReceiveProductByChalanByDC.Save | INFERRED |
| BR-M12-049 | On the Sub-depot->DC return page `ChalanDetail.DCStoreId` (persisted to tblSubDepotChalanRetuenDetail.DCStoreId) carries the SUB-DEPOT store id (DataKey `SubDCStoreId`). The DC-side receive query joins that value to `tblDCStore.DCStoreId`, so ExpDate/MfgDate/ComUnitId shown and used at DC receive come from an unrelated DC store row with the same numeric id. | Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs:170-176 and .aspx:295; Library.DAL/SInventory_DAL/ChalanDAL.cs:288-298; Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs:40-50,74-75 | SubChalanReceieve | INFERRED |

No calculations except rcv/damage arithmetic; no valuation stored (BR-M12-018).

## 20. Permission Matrix
- Session keys read: none in page code. No check that the user belongs to the receiving DC; the DC is hard-coded to 6 for the stock row. Menu-based only.

## 21. Status Lifecycle
`tblSubDepotChalanReturnInfo.IsDeliver`: False (created by PAGE-451) -> True at first saved line (`SubChalanUpdate`). New `tblDCStore` rows are `Available`; damaged quantity goes to `tblDCStoreFreeze` as `Restricted` (released later by the main-depot `OtherStockAction` page - out of M12). No reject state.

## 22. Error Handling
No try/catch; blank qty -> `FormatException`; no pending rows -> `IndexOutOfRange`; SQL exceptions propagate (parameterised `SInventorySql`). Alerts through JS.

## 23. Audit
No audit of receiving user/time on `tblDCStore` insert (only `StockRcvDate`).

## 24. Integration
None (main B2B receive triggers SAP calls elsewhere; not here).

## 25. Reports
No report is opened (popup commented out). The corresponding viewer for the DC->Sub side is RPT-M12-02; there is no viewer for return-chalan receipt.

## 26. Dependencies
BLL: `ChalanBLL`, `RequisitionBLL`(unused). DAL: `ChalanDAL`, `ClsPrimaryKeyFind`. DAO: `DCStockNew`, `DCStoreFreezeDAO`. Tables per §17.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Full receipt | open return chalan, defaults, Submit | new tblDCStore row(s) ComUnitId=6, StockQty=sent, `Available`; IsDeliver True | BR-M12-018 |
| TC-02 | Damage | Un Rcv 3 | rcv auto = sent-3; tblDCStoreFreeze `Restricted` StockQty 3 with ComUnitId from hidden field | BR-M12-020 |
| TC-03 | DC other than 6 (negative) | chalan returned to DC 3 | stock still booked to ComUnitId 6 | BR-M12-018 |
| TC-04 | List scope | two DCs with pending returns | both appear in every DC's list | BR-M12-019 |
| TC-05 | Date fields | compare ExpDate shown to source batch | taken from tblDCStore row with id = SubDCStoreId (wrong batch possible) | BR-M12-049 |
| TC-06 | Blank qty | clear Rcv Qty | FormatException | BR-M12-015 |
| TC-07 | Double receive | two tabs | duplicate DC stock | BR-M12-017 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx`, `Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SubdeportTransferReceiveProductByChalanByDC.aspx(.cs)` | n/a | MISSING | - |
| Receiving DC | None existed | hard-coded 6 | - | DEFECT | BR-M12-018 |
| Cost/ledger fields | None existed | not written | valuation SP treats NULL Trans_Type as receive (INFERRED) | GAP | GAP-M12-030 |

## 30. Known Gaps
- Hard-coded `ComUnitId = 6` (`.cs:80`); pending list not filtered; wrong join for dates (BR-M12-049); no cost columns on returned stock; `Back` goes to main-depot page; popup disabled; no reconciliation; delivered flag on first line; copy of main-depot receive page with a different SQL path.

## 31. Change Impact
- `ChalanBLL.DCStockInSub`, `ChalanDAL.DCStockInDALMainSub/SubChalanUpdate/SubChalanReceieve` are used only by this page. `ChalanBLL.SaveDCStoreFreeze2`/`ChalanDAL.SaveDCStoreFreeze2` are shared with the main-depot receive page `TransferReceiveProductByChalanByDC.aspx.cs` (grep of `SaveDCStoreFreeze2` callers; `ProductReceiveBySubDeport` uses the sub-depot copy). Rows created here appear in the DC stock reports and the DC valuation ledger (`sp_Report_DCStore_StockValuationLedger`).

