---
page_id: PAGE-375
module: M10
page: OtherStockAction
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-375 — OtherStockAction

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-375 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | OtherStockAction |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_OtherStockAction |
| aspx size | 32082 bytes |
| Code-behind LOC | 516 |
| Inline script size | 1183 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Returns previously frozen Distribution-Center stock (`tblDCStoreFreeze`, quantity > 0) back to sellable DC stock (`tblDCStore`). The user selects a DC (limited to their assigned units) and manufacturer, lists frozen rows, ticks rows and enters a return quantity, adds them to a staging list ("label grid") and submits; each staged row reduces the freeze quantity and adds the same quantity to the source `tblDCStore` row. Source: `Solution.Web/SInventory_UI/OtherStockAction.aspx.cs`, `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs`.

## 3. Business Objective
Release quarantined/restricted stock after it has been re-inspected (the reverse of `StockConditionFreeze` for DCs), so it can be sold again. Text of the success message is `Stock Return Successfully !!`.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/OtherStockAction.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OtherStockAction.aspx |
| Code-behind | Solution.Web/SInventory_UI/OtherStockAction.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |

## 7. UI Control Inventory
No validators. Verified controls:

| Control | Label | Required | State | Source |
|---|---|---|---|---|
| dcDropDownList | Distribution Center | needed for search (`Convert.ToInt32` unguarded) | user's units only | `OtherStockActionDAL.DCLoad(userId)` |
| manufacturerDropDownList | Manufacturer | needed for search | list of manufacturers | `tblManufacturer` |
| submitButton (Search) | Search | - | `Load()` -> `No Data Found!!` when empty | - |
| loadGridView | ProductCode, Product Name, Batch, ExpDate, StockQty (freeze qty), Amount, StockCondition, Return Qty, Remarks, select | DataKeys `DCStoreFreezeId, DCStoreId` | rows: `tblDCStoreFreeze` with `StockQty>0` |
| returnQtyTextBox | Return Qty | needed for ticked rows to be added (`Text.Trim() != ""`) | `FilteredTextBoxExtender ValidChars="0123456789."` | - |
| chkSelect / chkSelectAll | select | - | - | - |
| addButton | Add | adds ticked rows with qty to `labelGridView` unless the `DCStoreFreezeId` is already staged | - | `HasDCStoreId` |
| labelGridView | staged items (Remove button per row) | - | - | `DeleteImageButton_Click` |
| submitButton0 | Submit | processes all staged rows | - | `submitButton0_OnClick :390` |

## 8. Field Mapping
| Step | Source | Target |
|---|---|---|
| list | `OtherStockActionDAL.LoadData :17-30`: `tblDCStoreFreeze F INNER JOIN tblUnitPrice U ON ProductCode INNER JOIN tblProduct P WHERE StockQty>0 AND F.ComUnitId=@ComUnitId AND P.ManufacId=@ManufacId ORDER BY F.ExpDate` | grid |
| release 1 | `LoadStockData(DCStoreFreezeId)` -> current freeze `StockQty`; new = `StockQty - returnQty` | `tblDCStoreFreeze.StockQty` (`UpdateDCStoreFreezeStock :116-123`) |
| release 2 (only if release 1 returned true) | `LoadStockDCStockData(DCStoreId)` -> `StockQty`; new = `StockQty + returnQty` | `tblDCStore.StockQty` (`UpdateDCStock :125-132`) |
`DCStoreId` = `DataKeys[1]` = `F.DCStoreId` (the original stock row recorded by StockConditionFreeze).

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacturerDropDownList | LoadDropDown → OtherStockActionBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |
| dcDropDownList | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacturerDropDownList | LoadDropDown → OtherStockActionBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Handler | Effect | DB |
|---|---|---|---|
| submitButton | `submitButton_Click :260` -> `Load()` | bind freeze list | none |
| addButton | `addButton_Click :90` | stage ticked rows | none |
| DeleteImageButton | `DeleteImageButton_Click :156` | unstage a row, untick source row | none |
| submitButton0 | `submitButton0_OnClick :390` | for each staged row: checks then two UPDATEs; ends `Load2()` | `tblDCStoreFreeze`, `tblDCStore` |
| row command `ViewData` | `loadGridView_RowCommand :275` | single-row variant of the same release (same two UPDATEs) | same |

## 11. Frontend Flow
Init: `LoadDropDown` (manufacturer; DC by `Session["UserId"]`). Client JS: `DisableButton`, `pageLoad` (cosmetic). Staging is held only in the view state of `labelGridView`; navigating away loses it.

## 12. Backend Flow
`submitButton0_OnClick`: for each staged row: `bigStore = Convert.ToInt32(Cells[5].Text)` (integer-truncated freeze qty); if qty text non-empty and != `"0"`: if `bigStore < qty` alert `Return Quantity must be Less then Stock Qty` and (because the following `if` is skipped) nothing happens; else re-read freeze `StockQty` (`LoadStockData`), `newFreeze = StockQty - qty`, `UpdateDCStoreFreezeStock`; if true re-read `tblDCStore.StockQty`, `+ qty`, `UpdateDCStock`; success alert. If `StockQty==0 or >0` guard is always true for non-negative data. On failure of the first UPDATE: `Transaction Error!!`. After the loop `Load2()`. No transaction: if the second UPDATE fails the freeze qty has already been reduced (stock lost).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:20 |
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| OtherStockActionBLL.LoadData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:16 |
| OtherStockActionBLL.LoadStockData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:61 |
| OtherStockActionBLL.UpdateDCStoreFreezeStock | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:57 |
| OtherStockActionBLL.LoadStockDCStockData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:65 |
| OtherStockActionBLL.UpdateDCStock | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:70 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:33 | inline SQL: tblManufacturer |  |
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| OtherStockActionDAL.LoadData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:18 | inline SQL: tblDCStoreFreeze, tblProduct, tblUnitPrice | @ComUnitId, @ManufacId |
| OtherStockActionDAL.LoadStockData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:47 | inline SQL: tblDCStoreFreeze | @DCStoreFreezeId |
| OtherStockActionDAL.UpdateDCStoreFreezeStock | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:117 | inline SQL: tblDCStoreFreeze | @DCStoreFreezeId, @StockQty |
| OtherStockActionDAL.LoadStockDCStockData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:55 | inline SQL: tblDCStore | @DCStoreId |
| OtherStockActionDAL.UpdateDCStock | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:126 | inline SQL: tblDCStore | @DCStoreId, @StockQty |

## 16. Stored Procedure Mapping
None (parameterised inline SQL through `SInventorySql`).

## 17. Database Mapping
| Table | R/W | Notes |
|---|---|---|
| tblDCStoreFreeze | R + W | `StockQty` reduced (row is never deleted or marked released) |
| tblDCStore | R + W | `StockQty` increased on the original row |
| tblUnitPrice | R | INNER JOIN (no IsActive filter) |
| tblProduct | R | manufacturer filter |
| tblCompanyUnit, tblUserCompanyUnit, tblManufacturer | R | dropdowns |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Return qty | non-empty and not "0" | `:329,455` | Insert Return Quantity !! | BR-M10-035 |
| Return qty <= freeze qty | compared with integer-truncated cell text | `:331-334,458-463` | Return Quantity must be Less then Stock Qty | BR-M10-035 |
| Search result | must not be empty | `Load()` | No Data Found!! | - |
| Row already staged | duplicate `DCStoreFreezeId` not added | `HasDCStoreId :245-257` | (silent) | BR-M10-037 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-034 | The release list shows frozen DC rows (`tblDCStoreFreeze.StockQty>0`) of the chosen DC (`F.ComUnitId`) and manufacturer (`tblProduct.ManufacId`), ordered by expiry date. DC choices are limited to the user's units. | `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:17-30,93-105` | `LoadData`, `DCLoad` | CONFIRMED |
| BR-M10-035 | A release needs a non-empty, non-zero return quantity not greater than the (integer-truncated) freeze quantity; decimal input is allowed by the client filter (`.`) but compared/updated as decimal. Negative values cannot be typed (client filter). | `OtherStockAction.aspx.cs:329-335,455-463`; aspx `FilteredTextBoxExtender` | `submitButton0_OnClick` | CONFIRMED |
| BR-M10-036 | Effect of a release: `tblDCStoreFreeze.StockQty -= qty` then (if the first update succeeded) `tblDCStore.StockQty += qty` on the original stock row identified by `DCStoreId`. Two independent commits; the freeze row keeps its `StockCondition` and stays with quantity 0 (no release record). | `OtherStockAction.aspx.cs:337-364,468-497`; `OtherStockActionDAL.cs:116-132` | `submitButton0_OnClick`, `loadGridView_RowCommand` | CONFIRMED |
| BR-M10-037 | Rows are staged through an add/remove list before submit; a freeze row already staged is not added twice. | `OtherStockAction.aspx.cs:90-153,245-257` | `addButton_Click` | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]` restricts the DC list (`tblUserCompanyUnit`). Commented-out code shows an older `admin`-only branch (`:403-453`). No further role check; anyone who can open the page can release any listed freeze row.

## 21. Status Lifecycle
Freeze quantity lifecycle: created by StockConditionFreeze (`StockQty=qty`) -> reduced by release to 0; no explicit status column; `StockCondition` text on the freeze row is unchanged after release.

## 22. Error Handling
No try/catch; `dt.Rows[0]` on a vanished row throws; `Convert.ToDecimal` on hand-edited postback data throws; failures of the second UPDATE go unnoticed.

## 23. Audit
No audit columns are written (no user/date on release). Only the original freeze creation records `EntryBy/EntryDate` (in `tblStockConditionFreeze`).

## 24. Integration
None. (Sub-depot variants exist in the DAL: `SubDeportLoadData`, `SubdeportUpdateDCStock` - not used by this page.)

## 25. Reports
None.

## 26. Dependencies
`OtherStockActionBLL/DAL`, `SInventorySql`; tables above.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-375-01 | Search DC/manufacturer with frozen stock | rows sorted by expiry | BR-M10-034 |
| TC-375-02 | Tick row with blank qty, Add | row not added | BR-M10-037 |
| TC-375-03 | Add same row twice | second add ignored | BR-M10-037 |
| TC-375-04 | Release 3 of 5 | freeze 5->2, tblDCStore.StockQty +3 | BR-M10-036 |
| TC-375-05 | Qty 7 > 5 | `Return Quantity must be Less then Stock Qty`, no change | BR-M10-035 |
| TC-375-06 | Qty 0 | `Insert Return Quantity !!` | BR-M10-035 |
| TC-375-07 | Freeze row from central-WH freeze (no ComUnitId) | never listed | BR-M10-034 / PAGE-381 BR-M10-032 |
| TC-375-08 | Simulate failure of second UPDATE | freeze reduced, DC stock unchanged (loss) | BR-M10-036 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OtherStockAction.aspx`, `Solution.Web/SInventory_UI/OtherStockAction.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Negative/over-release protection | None existed | UI compare only; rereads qty then writes absolute value (lost-update risk) | PARTIAL | `OtherStockAction.aspx.cs:343-361` |

## 30. Known Gaps
- No transaction; read-modify-write of absolute quantities (lost update under concurrency); no audit trail of release; freeze row persists at 0.
- Integer truncation in comparison; `INNER JOIN tblUnitPrice` can duplicate rows.
- Commented code and a Sub-depot copy in the DAL indicate legacy variants.

## 31. Change Impact
`tblDCStore.StockQty` and `tblDCStoreFreeze.StockQty` are shared with PAGE-381, `ReturnInvoiceDAL`/`SalesReturnDAL`/`InvoiceDAL` (freeze rows created for returns), `SalesOperationRollbackDAL`, and the reports `DCStockReportDAL`.

