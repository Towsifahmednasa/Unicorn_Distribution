---
page_id: PAGE-381
module: M10
page: StockConditionFreeze
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-381 — StockConditionFreeze

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-381 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | StockConditionFreeze |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_StockConditionFreeze |
| aspx size | 28059 bytes |
| Code-behind LOC | 516 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Moves quantities of **Available** stock into a restricted condition (e.g. Restricted / Blocked / Damage - values come from `tblStockCondition`) so the goods can no longer be issued/sold. Works either for a Distribution Center's stock (`tblDCStore`) or - if the user's session has `CentralWareHouse=True` - for Central Warehouse stock (`tblCentralStore`). Per row the user chooses a condition, a quantity and remarks, then submits (row button or bulk). Source: `Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs`.

## 3. Business Objective
Quarantine damaged/expired/disputed stock: quantity leaves the sellable pool, is recorded in a freeze table with the reason (condition) and the operator, and can later be returned through `OtherStockAction` (DC) or `WhFreezeStockRelease` (WH). Permission to use each condition is granted per user on `StockConditionPermission`.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/StockConditionFreeze.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockConditionFreeze.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | StockConditionFreezeBLL (Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | StockConditionFreezeDAL (Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs) |
| DAO/entity class | DCStoreFreezeDAO (Library.DAO/SInventory_Entities/DCStoreFreezeDAO.cs) |
| DAO/entity class | StockConditionFreezeDAO (Library.DAO/SInventory_Entities/StockConditionFreezeDAO.cs) |

## 7. UI Control Inventory
No validators. Verified controls:

| Control | Label | Required | Default / state | Source |
|---|---|---|---|---|
| centalWHCheckBox | Central Warehouse | - | disabled unless `Session["CentralWareHouse"]=="True"` (`:19-22`); toggles `DIVCH`/`DIVDC` panels | session |
| whDropDownList | (warehouse) | needed when Central WH checked | loaded by `RequisitionBLL.WareHouseLoad` on check | `RequisitionBLL` |
| dcDropDownList1 | Distribution Center | needed when unchecked | list restricted to user's units (`OtherStockActionBLL.DCLoad(userId)` -> `tblCompanyUnit LEFT JOIN tblUserCompanyUnit WHERE UserId`) | DAL `OtherStockActionDAL.DCLoad :93-105` |
| manufacturerDropDownList | Manufacturer | needed for search | `SelectedIndex=1` | `tblManufacturer` |
| Search (LinkButton1 -> `submitButton_Click`) | Search Information | - | loads DC rows or WH rows | - |
| Submit (LinkButton2 -> `submitButton0_OnClick`) | Submit Information | - | processes all checked rows | - |
| loadGridView | ProductCode, Product Name, Batch, ExpDate, Receive Date, StockInQty (`TotalQuantity`), C.StockQty (`StockQty`), Amount, Stock (`StockCondition`), Stock Condition (dropdown), Qty, Remarks, select | DataKeyNames `nomanslandID` (= DCStoreId or central ReceiveId) | see section 9 |
| StockConditionDropDownList | Stock Condition | selected index must be != 0 | conditions the user may use | `tblStockCondition` + `tblStockConditionPermission` |
| returnQtyTextBox | Qty | must be non-empty and not `0` | digits only (`FilteredTextBoxExtender ValidChars="0123456789"`) | - |
| remarksTextBox | Remarks | No | empty | - |

## 8. Field Mapping
DC branch (`loadGridView_RowCommand`/`submitButton0_OnClick`, DataKey = `DCStoreId`):

| Step | Source value | Target |
|---|---|---|
| 1 | `manufacturerDropDownList`, `dcDropDownList1.SelectedValue` (a **ComUnitId**, stored in the `DCStoreId` field), `returnQtyTextBox` (FreezeQty), LoginName, Now | `tblStockConditionFreeze` (StockConditionFreezeID=`MAX+1`, DCStoreId, ManufacId, FreezeQty, EntryBy, EntryDate) - `StockConditionFreezeBLL.SaveforDC` / `StockConditionFreezeDAL.SaveforDC :74-79` |
| 2 | source `tblDCStore` row (`LoadStockStockQtyDCData`) + condition text + qty + remarks | `tblDCStoreFreeze` (DCStoreFreezeId=`MAX+1`, StorageLocation, TotalQuantity=qty, ProductCode, ProductName, PackSize, BatchNo, ExpDate, ReceiveDate, ChalanNo, ChalanDate, StockQty=qty, DamageQty=0, StockRcvDate=Now, StockCondition, Remarks, ComUnitId, DCStoreId, StockConditionFreezeID) - `SaveDCStoreFreeze2 :205-210` |
| 3 | `dtdcstock - qty` | `tblDCStore.StockQty` (`UpdateDCStore :90-98`) |

Central WH branch (DataKey = central `ReceiveId`): step 1 `tblStockConditionFreeze.ReceiveId = whDropDownList.SelectedValue` (**warehouse id, not receive id**), step 2 `tblDCStoreFreeze` (same columns but `ReceiveId` instead of ComUnitId/DCStoreId; `SaveDCStoreFreeze :197-203`), step 3 `tblCentralStore.Quantity = Quantity - qty` (`UpdateCentralStore :81-88`).

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Text / Value |
|---|---|---|---|
| StockConditionDropDownList (per row) | `StockConditionFreezeBLL.LoadStockConditionBll` -> `StockConditionFreezeDAL.LoadStockConditionBll :38-45` | `select * from tblStockCondition WHERE StockCondition<>'Available' AND StockConId IN (SELECT StockConId FROM tblStockConditionPermission where Permission=1 AND UserId=@UserId)` | `StockCondition` / `StockConId` (first item = placeholder, index 0) |
| dcDropDownList1 | `OtherStockActionBLL.DCLoad` | `SELECT * FROM tblCompanyUnit LEFT JOIN tblUserCompanyUnit ... WHERE UserId=@UserId` | `ComUnitName` / `ComUnitId` |
| whDropDownList | `RequisitionBLL.WareHouseLoad` | UNKNOWN (not traced) | warehouse |
| manufacturerDropDownList | `OtherStockActionBLL.LoadmanufacturerName` | `select * from tblManufacturer` | `ManufacName` / `ManufacId` |
Grid queries: DC = `tblDCStore INNER JOIN tblUnitPrice ON ProductCode WHERE StockQty>0 AND StockCondition='Available' AND ComUnitId=@ComUnitId` (`LoadStockDCData :16-25`); WH = `tblCentralStore INNER JOIN tblUnitPrice ON ProductCode WHERE StockCondition='Available'` (`LoadWHData :47-53`, no `Quantity>0` filter). Manufacturer selection is not used to filter either query.

## 10. Button Action Mapping
| Button | Handler | Effect | DB change |
|---|---|---|---|
| LinkButton1 (Search) | `submitButton_Click :100` | loads DC or WH rows (needs manufacturer + DC/WH selected) | none |
| row button `ViewData` | `loadGridView_RowCommand :145-297` | freeze a single row | as section 8 |
| LinkButton2 (Submit) | `submitButton0_OnClick :315-515` | loops checked rows, same logic, then reloads DC grid and alerts `Stock Freeze Successfull` | as section 8 per row |
| LinkButton4 (Reset) | `cancelButton_Click :38` | empty | none |

## 11. Frontend Flow
Init: `LoadDropDown` (manufacturer, DC list by user); enable Central-WH checkbox if session flag; toggling the checkbox shows the WH or DC panel and loads warehouse list. Search binds the grid; every row gets its own condition dropdown populated per user. JS: `pageLoad` (cosmetic select2). All logic is server-side postback.

## 12. Backend Flow
`submitButton0_OnClick` (bulk) / `loadGridView_RowCommand` (single):
1. `bigStore = Convert.ToInt32(row.Cells[7].Text)` (stock shown, integer-truncated).
2. Requires qty text non-empty, != `"0"`, and condition index != 0; else alert `Insert  Quantity and Select Stock Condition!!`.
3. If `bigStore < qty` -> alert ` Quantity must be Less then Stock Qty` and no action; if `bigStore >= qty` proceed.
4. Central-WH or DC branch as in section 8: each step executes only if the previous returned id/true (`StockConditionFreezeID>0`, `FreezeID>0`), each is a separate statement (no transaction).
5. Bulk path ends with `LoadDCStoreData()` and `Stock Freeze Successfull` regardless of per-row outcomes; the DC branch inside the single-row handler reloads the grid only on success.
6. Exceptions (non-numeric qty cannot occur due to client filter, but `Convert` on tampered input can; DAO conversions) are unhandled.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:20 |
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| RequisitionBLL.WareHouseLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:134 |
| StockConditionFreezeBLL.LoadStockDCData | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:17 |
| StockConditionFreezeBLL.LoadStockConditionBll | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:25 |
| StockConditionFreezeBLL.LoadWHData | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:30 |
| StockConditionFreezeBLL.SaveforWH | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:38 |
| StockConditionFreezeBLL.SaveDCStoreFreeze | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:116 |
| StockConditionFreezeBLL.UpdateCentralStore | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:86 |
| StockConditionFreezeBLL.SaveforDC | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:62 |
| StockConditionFreezeBLL.LoadStockStockQtyDCData | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:21 |
| StockConditionFreezeBLL.SaveDCStoreFreeze2 | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:134 |
| StockConditionFreezeBLL.UpdateDCStore | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:90 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:33 | inline SQL: tblManufacturer |  |
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| RequisitionDAL.WareHouseLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:25 | inline SQL: tblWearhouse |  |
| StockConditionFreezeDAL.LoadStockDCData | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:17 | inline SQL: tblDCStore, tblUnitPrice | @ComUnitId |
| StockConditionFreezeDAL.LoadStockConditionBll | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:39 | inline SQL: tblStockCondition, tblStockConditionPermission | @UserId |
| StockConditionFreezeDAL.LoadWHData | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:48 | inline SQL: tblCentralStore, tblUnitPrice |  |
| StockConditionFreezeDAL.SaveforWH | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:68 | inline SQL: tblStockConditionFreeze |  |
| StockConditionFreezeDAL.StockConditionFreezeParameters | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:237 | delegates / no SQL literal | @DCStoreId, @EntryBy, @EntryDate, @FreezeQty, @ManufacId, @ReceiveId, @StockConditionFreezeID |
| StockConditionFreezeDAL.SaveDCStoreFreeze | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:198 | inline SQL: tblDCStoreFreeze |  |
| StockConditionFreezeDAL.DcStoreFreezeParameters | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:262 | delegates / no SQL literal | @BatchNo, @ChalanDate, @ChalanNo, @ComUnitId, @DCStoreFreezeId, @DCStoreId, @DamageQty, @ExpDate, @P |
| StockConditionFreezeDAL.UpdateCentralStore | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:82 | inline SQL: tblCentralStore | @ReceiveId, @StockQty |
| StockConditionFreezeDAL.SaveforDC | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:75 | inline SQL: tblStockConditionFreeze |  |
| StockConditionFreezeDAL.LoadStockStockQtyDCData | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:28 | inline SQL: tblDCStore, tblUnitPrice | @DCStoreId |
| StockConditionFreezeDAL.SaveDCStoreFreeze2 | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:206 | inline SQL: tblDCStoreFreeze |  |
| StockConditionFreezeDAL.UpdateDCStore | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:91 | inline SQL: tblDCStore | @DCStoreId, @StockQty |

## 16. Stored Procedure Mapping
None (all inline SQL via `SInventorySql`/`ClsCommonInternalDAL`, parameterised for these DAL methods except the `PrimaryKeyMax` lookups).

## 17. Database Mapping
| Table | R/W | Notes |
|---|---|---|
| tblDCStore | R + W | `StockQty` decremented in place on the source (Receive) row |
| tblCentralStore | R + W | `Quantity` decremented in place |
| tblUnitPrice | R | INNER JOIN on ProductCode (no `IsActive` filter -> duplicate rows if several price rows) |
| tblStockCondition, tblStockConditionPermission | R | condition list per user |
| tblStockConditionFreeze | W | freeze header (StockConditionFreezeID, DCStoreId or ReceiveId, ManufacId, FreezeQty, EntryBy, EntryDate) |
| tblDCStoreFreeze | W | freeze detail (both DC and WH cases) |
| tblCompanyUnit, tblUserCompanyUnit, tblManufacturer | R | dropdowns |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Qty | non-empty, not "0" | `loadGridView_RowCommand :154` | Insert  Quantity and Select Stock Condition!! | BR-M10-030 |
| Condition | index != 0 | same | same | BR-M10-030 |
| Qty <= stock | integer-truncated compare | `:156-160` | ` Quantity must be Less then Stock Qty` | BR-M10-030 |
| Qty digits | client filter only (0-9) | aspx | - | BR-M10-030 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-027 | Only stock in condition `Available` can be frozen: DC rows need `StockQty>0` and `ComUnitId`=selected DC; central rows need `StockCondition='Available'` (no quantity>0 filter). | `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:16-25,47-53` | `LoadStockDCData`, `LoadWHData` | CONFIRMED |
| BR-M10-028 | A user may only choose conditions that are not `Available` and for which `tblStockConditionPermission` has `Permission=1` for that user. | `StockConditionFreezeDAL.cs:38-45` | `LoadStockConditionBll` | CONFIRMED |
| BR-M10-029 | The Central Warehouse option is enabled only when `Session["CentralWareHouse"]=="True"`; DC choices are limited to the user's assigned units (`tblUserCompanyUnit`). | `StockConditionFreeze.aspx.cs:19-22,59-65`; `OtherStockActionDAL.cs:93-105` | `Page_Load`, `LoadDropDown` | CONFIRMED |
| BR-M10-030 | A row is frozen only when qty is filled and not 0, a condition is chosen and qty does not exceed the displayed stock (compared as truncated integer); otherwise a message and no action. No lower bound (>0) beyond "not 0". | `StockConditionFreeze.aspx.cs:145-160,315-340` | `loadGridView_RowCommand`, `submitButton0_OnClick` | CONFIRMED |
| BR-M10-031 | **DC freeze effect:** insert `tblStockConditionFreeze`, insert `tblDCStoreFreeze` (qty, condition text, remarks, source DCStoreId, ComUnitId), then set `tblDCStore.StockQty = StockQty - qty` on the source row (in-place update, not a ledger row). The frozen qty is therefore excluded from `StockCondition='Available'` stock lists. | `StockConditionFreeze.aspx.cs:226-286`; `StockConditionFreezeDAL.cs:74-98,205-210` | freeze DC branch | CONFIRMED |
| BR-M10-032 | **Central WH freeze effect:** insert `tblStockConditionFreeze` (ReceiveId field receives the **warehouse dropdown value**), insert `tblDCStoreFreeze` with `ReceiveId` (ComUnitId/DCStoreId not set), then `tblCentralStore.Quantity = Quantity - qty`. These `tblDCStoreFreeze` rows have no ComUnitId so `OtherStockAction` (filters `F.ComUnitId = @ComUnitId`) never lists them, and `WhFreezeStockRelease` reads `tblWhStoreFreeze`, not `tblDCStoreFreeze`: they cannot be released through either page. | `StockConditionFreeze.aspx.cs:166-223`; `StockConditionFreezeDAL.cs:81-88,197-203`; `OtherStockActionDAL.cs:17-30`; `WhFreezeStockReleaseDal.cs:17-24` | freeze WH branch | CONFIRMED (code) / operational impact INFERRED |
| BR-M10-033 | Bulk submit reports success (`Stock Freeze Successfull`) after looping all checked rows even when some rows were skipped by validation messages; each row's three writes are independent (no rollback). | `StockConditionFreeze.aspx.cs:315-515` | `submitButton0_OnClick` | CONFIRMED |

## 20. Permission Matrix
Session keys: `UserId` (condition permission, DC list), `LoginName` (EntryBy), `CentralWareHouse` (enables WH mode). No role check. Commented-out code (`myListDropDown_Change`) once special-cased `admin1`/`Restricted`.

## 21. Status Lifecycle
Stock condition of quantity (not a document status): `Available` (tblDCStore/tblCentralStore rows) -> frozen quantity recorded in `tblDCStoreFreeze.StockCondition` = chosen condition text (e.g. `Restricted`, `Blocked`; the report SQL in `DCStockReportDAL.cs:132,143,363,373` shows these two values; the full list lives in `tblStockCondition` - content UNKNOWN). Return path: `OtherStockAction` (DC) decrements freeze qty and re-credits `tblDCStore.StockQty`. The freeze header `tblStockConditionFreeze` has no status.

## 22. Error Handling
No try/catch anywhere; `Convert.ToInt32(cell.Text)` on non-integer stock text or missing row (`dt.Rows[0]`) throws. User feedback via `alert()`.

## 23. Audit
`tblStockConditionFreeze.EntryBy/EntryDate`; freeze detail has StockRcvDate=Now, Remarks, StockConditionFreezeID link. No before/after stock log.

## 24. Integration
None.

## 25. Reports
Freeze quantities appear in `DCStockReportDAL` restricted/blocked sections (see RPT-M10-02).

## 26. Dependencies
`StockConditionFreezeBLL/DAL`, `OtherStockActionBLL/DAL`, `RequisitionBLL`, `SInventorySql`, `ClsPrimaryKeyFind`, DAO `StockConditionFreezeDAO`, `DCStoreFreezeDAO`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-381-01 | Non-central user | Central WH checkbox disabled | BR-M10-029 |
| TC-381-02 | Search DC with stock | only `Available` rows with StockQty>0 | BR-M10-027 |
| TC-381-03 | User without permission for any condition | dropdown only placeholder; freeze blocked by message | BR-M10-028/030 |
| TC-381-04 | Freeze 5 of 20 at DC | tblDCStoreFreeze +5, tblDCStore.StockQty 20->15, tblStockConditionFreeze row | BR-M10-031 |
| TC-381-05 | Qty 25 > stock 20 | message ` Quantity must be Less then Stock Qty`, no writes | BR-M10-030 |
| TC-381-06 | Qty 0 or blank | `Insert  Quantity and Select Stock Condition!!` | BR-M10-030 |
| TC-381-07 | Central WH freeze | tblCentralStore.Quantity reduced; freeze row not visible in OtherStockAction | BR-M10-032 |
| TC-381-08 | Bulk with 3 rows, 1 invalid | message for the invalid row, then `Stock Freeze Successfull` | BR-M10-033 |
| TC-381-09 | Stock 20.6 displayed | truncated to 20 for compare | BR-M10-030 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockConditionFreeze.aspx`, `Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Negative-stock/freeze enforcement | None existed | UI/server compare to displayed stock only; in-place quantity update | PARTIAL | BR-M10-030/031 |

## 30. Known Gaps
- GAP-M10-009: central-WH freeze via this page writes to `tblDCStoreFreeze` with wrong field semantics (`ReceiveId` = warehouse id in header) and produces rows that no release page lists (BR-M10-032).
- Parallel model in `WhStockConditionFreeze` / `WhFreezeStockRelease` (`tblWhStoreFreeze`) duplicates WH freezing (see PAGE-394/390).
- Freeze uses in-place `StockQty` update on Receive rows, inconsistent with the ledger-style Issue rows used by sales (see README data model) and with `sp_Adjust_DCStore_Stock_Only`, which resets Receive `StockQty` to `TotalQuantity` (GAP-M10-005).
- INNER JOIN `tblUnitPrice` without `IsActive` can duplicate rows and amounts; stock compare by truncated integer; race conditions (read-modify-write of `StockQty`); no transaction.

## 31. Change Impact
`tblDCStore.StockQty` and `tblCentralStore.Quantity` are also written by PAGE-375/379/389/392/394/390 and sales/return/transfer DALs (`InvoiceDAL`, `ReturnInvoiceDAL`, `SalesReturnDAL`, `CustomerReturnDAL`, `SalesOperationRollbackDAL`, `SCtoWHTransferDal`); `StockConditionFreezeDAL.LoadInvoice*` methods are unrelated picking helpers living in this DAL. `StockBatchUpdate.aspx` reuses the class name `SInventory_UI_StockConditionFreeze`.

