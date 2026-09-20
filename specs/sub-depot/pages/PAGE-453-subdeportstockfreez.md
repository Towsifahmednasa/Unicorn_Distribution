---
page_id: PAGE-453
module: M12
page: SubDeportStockFreez
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-453 — SubDeportStockFreez

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-453 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubDeportStockFreez |
| Kind | Page (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SubDeportStockFreez |
| aspx size | 14978 bytes |
| Code-behind LOC | 488 |
| Inline script size | 1189 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**SubDeportStockFreez** (breadcrumb caption `Freeze Stock Release`, `SubDeportStockFreez.aspx:12`) releases stock that was held in the sub-depot freeze table (`tblSubDepotStoreFreeze`) back into the sub-depot's sellable stock (`tblSubDepotStore`). The user picks a DC, a sub-depot and a manufacturer, presses Search, ticks freeze rows, types a Return Qty per row, adds them to a second grid and presses the green button (captioned `Print Invoice` but wired to the release logic). CONFIRMED (`SubDeportStockFreez.aspx.cs:369-482`, `.aspx:126-135`).

## 3. Business Objective
Freeze rows arise from (a) damaged/un-received quantities recorded when a sub-depot receives a DC chalan (`Restricted`, BR-M12-016) and (b) sub-depot sales returns (`ReturnStock`, `ReturnInvoiceDAL.SubDCStockInDAL`, `ReturnInvoiceDAL.cs:1212-1224`). This page is the only M12 screen that converts frozen stock back to `Available` sellable stock, i.e. it decides when quarantined/returned goods may be sold again. INFERRED business purpose (from data flow); `Blocked` rows are counted by the stock report but no code in the repo creates them for sub-depots.

## 4. Navigation
Menu placement DB-driven (path `UNKNOWN`). No inbound/outbound links in code. Reset button reloads the same page (`Unnamed_Click`, `.cs:484-487`). Master page: `NewMasterPage.master` (`SubDeportStockFreez.aspx:1`).

## 5. Route
`~/SubDepot_UI/SubDeportStockFreez.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Handler |
|---|---|---|---|---|---|---|
| dcDropDownList | Distribution Centerr: (sic) | none (Search needs a numeric sub-depot value) | `OtherStockActionBLL.DCLoad(ddl, Session["UserId"])` - only the user's assigned DCs (`.cs:42`) | - | `tblCompanyUnit` JOIN `tblUserCompanyUnit` (`OtherStockActionDAL.cs:93-104`) | `dcDropDownList_SelectedIndexChanged` (:354) -> loads sub-depots, clears grid |
| subdeportDropDownList1 | Sub-Depot: | required in practice - `Convert.ToInt32(SelectedValue)` throws on empty | `--------Select---------` | - | `RequisitionBLL.SubdeportLoad` | - |
| manufacturerDropDownList | Manufacturer: | yes (int parse) | index 1 (`.cs:43`) | - | `OtherStockActionBLL.LoadmanufacturerName` | `manufacturerDropDownList_SelectedIndexChanged` (:363) clears grid |
| submitButton `Search` | - | - | - | - | `OtherStockActionBLL.SubDeportLoadData(SubDepotId, ManufacId)` | `submitButton_Click` (:72) -> `Load()` `No Data Found!!` if empty |
| loadGridView | #SL, ProductCode, Product Name, Batch, ExpDate, StockQty, Amount, StockCondition, Return Qty, Remarks, select | - | - | DataKeys `SDStoreFreezeId,SubDCStoreId`; `RowCommand` bound but no CommandName button exists | `tblSubDepotStoreFreeze` | - |
| returnQtyTextBox | Return Qty | non-empty to be added | empty | digits and `.` only (FilteredTextBoxExtender) | user | - |
| chkSelectAll / chkSelect | - | ticked to be added | unchecked | AutoPostBack | - | `chkSelectAll_CheckedChanged` (:21) |
| addButton | Add to list | - | - | - | builds `labelGridView` | `addButton_Click` (:82) |
| labelGridView | same columns + Remove | - | - | DataKeys `DCStoreFreezeId,DCStoreId` (values are SDStoreFreezeId/SubDCStoreId) | in-page DataTable | `DeleteImageButton_Click` (:148) -> `ReSetGridStatus` |
| submitButton0 (green, text `Print Invoice`) | - | - | - | - | releases stock (BR-M12-029/030) | `submitButton0_OnClick` (:369) |
| Reset | - | - | - | - | - | `Unnamed_Click` (:484) |

## 8. Field Mapping
| UI control | Variable | Handler (line) | BLL/DAL | SQL parameter | Table.column |
|---|---|---|---|---|---|
| subdeportDropDownList1 | `SubDepotId` | `Load()` :47 | `OtherStockActionBLL.SubDeportLoadData` / `OtherStockActionDAL.SubDeportLoadData` :137 | `@SubDepotId` | filters `tblSubDepotStoreFreeze.SubDepotId` |
| manufacturerDropDownList | `ManufacId` | :47 | same | `@ManufacId` | filters `tblProduct.ManufacId` |
| loadGridView DataKeys[i][0] | `SDStoreFreezeId` | :399 | `LoadSubdeportStockData` :153 / `SubdeportUpdateDCStoreFreezeStock` :161 | `@SDStoreFreezeId`, `@StockQty` | `tblSubDepotStoreFreeze.StockQty` (read then UPDATE) |
| loadGridView DataKeys[i][1] | `SubDCStoreId` | :400 | `SubdeportLoadStockDCStockData` :170 / `SubdeportUpdateDCStock` :178 | `@SubDCStoreId`, `@StockQty` | `tblSubDepotStore.StockQty` (read then UPDATE) |
| returnQtyTextBox / labelGridView cell[8] | `Convert.ToDecimal(Cells[8].Text)` | :406,419 | - | new StockQty = old -/+ qty | both quantities |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Value/Text | Tables |
|---|---|---|---|---|
| dcDropDownList | `LoadDropDown` :41-42 | `SELECT * FROM tblCompanyUnit LEFT JOIN tblUserCompanyUnit ON ... WHERE UserId=@UserId` (`OtherStockActionDAL.cs:93-104`) | ComUnitId / ComUnitName | tblCompanyUnit, tblUserCompanyUnit |
| subdeportDropDownList1 | DC change :357 -> `RequisitionBLL.SubdeportLoad` | see BR-M12-001 | SubDepotId / `Code:Name` | tblSubDepot |
| manufacturerDropDownList | :41 | `SELECT * FROM tblManufacturer` | ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL / DB change |
|---|---|---|---|
| `Search` | `submitButton_Click` -> `Load()` | `OtherStockActionBLL.SubDeportLoadData` -> `OtherStockActionDAL.SubDeportLoadData` | parameterised SELECT on `tblSubDepotStoreFreeze` JOIN `tblUnitPrice`, `tblProduct` (read only) |
| `Add to list` | `addButton_Click` | none | none |
| Remove | `DeleteImageButton_Click` | none | none |
| green `Print Invoice` button | `submitButton0_OnClick` | `OtherStockActionBLL.LoadSubdeportStockData`, `SubdeportUpdateDCStoreFreezeStock`, `SubdeportLoadStockDCStockData`, `SubdeportUpdateDCStock` | `UPDATE tblSubDepotStoreFreeze SET StockQty=@StockQty WHERE SDStoreFreezeId=@...`; `UPDATE tblSubDepotStore SET StockQty=@StockQty WHERE SubDCStoreId=@...` (parameterised, `OtherStockActionDAL.cs:161-185`) |
| (grid RowCommand `ViewData`) | `loadGridView_RowCommand` :258-353 | **main-depot** methods `LoadStockData`, `UpdateDCStoreFreezeStock`, `LoadStockDCStockData`, `UpdateDCStock` (tblDCStoreFreeze/tblDCStore!) | dead code: no button emits `CommandName="ViewData"` in the aspx (GAP-M12-016) |

## 11. Frontend Flow
- `Page_Load` (first load): DC list restricted to the user, manufacturer list, manufacturer index 1 (`.cs:16-19,41-44`).
- DC change -> sub-depot list; Search -> `loadGridView`; ticked rows + Return Qty -> `Add to list` -> `labelGridView` (a freeze row id can be added only once - `HasDCStoreId` :200); Remove rebuilds `labelGridView` and calls `ReSetGridStatus` (unticks/clears rows no longer listed, :215-249).
- Release button iterates `labelGridView` rows (BR-M12-029..031), then `Load2()` refreshes `loadGridView` and clears `labelGridView`.
- JS: `pageLoad()` select2 only (`.aspx:57-70`). No AJAX/PageMethods/modals; alerts via `ScriptManager`.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `submitButton0_OnClick` | `SubDeportStockFreez.aspx.cs:369-482` | for each `labelGridView` row i: `bigStore = Convert.ToInt32(Cells[5].Text)`; if `loadGridView.Rows[i].Cells[7].Text != "Restricted"` AND LoginName != "21900": qty empty/0 -> alert `Insert Return Quantity !!`; qty > bigStore -> alert `Return Quantity must be Less then Stock Qty`; else read freeze StockQty, if >=0 write `freeze.StockQty = old - qty`, read store StockQty, write `store.StockQty = old + qty`, alert `Stock Return Successfully !!`. A second identical block runs when LoginName == "21900" (no Restricted check). Finally `Load2()` | no | no |
| `addButton_Click` | :82-145 | ticked rows with non-empty Return Qty and not already listed -> `labelGridView` | no | no |
| `loadGridView_RowCommand` | :258-353 | unreachable (see §10) | no | no |
| `Load()` / `Load2()` | :45-71 | `Convert.ToInt32` of both dropdowns then query | no | no |

Both quantities are updated by two separate UPDATE statements: if the second fails after the first, the frozen quantity is already reduced and the store quantity is not increased (no transaction; `SInventorySql.Execute` -> `UpdateDataByUpdateCommandNew` has try/finally without catch, so the exception surfaces as an unhandled error - `SInventorySql.cs:36-40`, `ClsCommonInternalDAL.cs:199-224`; GAP-M12-007). Also `Execute` returns `rowsAffected>0` and the page ignores the return value.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:20 |
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| OtherStockActionBLL.SubDeportLoadData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:77 |
| OtherStockActionBLL.LoadStockData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:61 |
| OtherStockActionBLL.UpdateDCStoreFreezeStock | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:57 |
| OtherStockActionBLL.LoadStockDCStockData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:65 |
| OtherStockActionBLL.UpdateDCStock | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:70 |
| RequisitionBLL.SubdeportLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:157 |
| OtherStockActionBLL.LoadSubdeportStockData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:81 |
| OtherStockActionBLL.SubdeportUpdateDCStoreFreezeStock | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:85 |
| OtherStockActionBLL.SubdeportLoadStockDCStockData | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:90 |
| OtherStockActionBLL.SubdeportUpdateDCStock | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:95 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:33 | inline SQL: tblManufacturer |  |
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| OtherStockActionDAL.SubDeportLoadData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:138 | inline SQL: tblProduct, tblSubDepotStoreFreeze, tblUnitPrice | @ManufacId, @SubDepotId |
| OtherStockActionDAL.LoadStockData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:47 | inline SQL: tblDCStoreFreeze | @DCStoreFreezeId |
| OtherStockActionDAL.UpdateDCStoreFreezeStock | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:117 | inline SQL: tblDCStoreFreeze | @DCStoreFreezeId, @StockQty |
| OtherStockActionDAL.LoadStockDCStockData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:55 | inline SQL: tblDCStore | @DCStoreId |
| OtherStockActionDAL.UpdateDCStock | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:126 | inline SQL: tblDCStore | @DCStoreId, @StockQty |
| RequisitionDAL.SubdeportLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:46 | inline SQL: tblSubDepot | @ComUnitId |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| OtherStockActionDAL.LoadSubdeportStockData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:154 | inline SQL: tblSubDepotStoreFreeze | @SDStoreFreezeId |
| OtherStockActionDAL.SubdeportUpdateDCStoreFreezeStock | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:162 | inline SQL: tblSubDepotStoreFreeze | @SDStoreFreezeId, @StockQty |
| OtherStockActionDAL.SubdeportLoadStockDCStockData | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:171 | inline SQL: tblSubDepotStore | @SubDCStoreId |
| OtherStockActionDAL.SubdeportUpdateDCStock | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:179 | inline SQL: tblSubDepotStore | @StockQty, @SubDCStoreId |

## 16. Stored Procedure Mapping
No stored procedure is called. All statements are inline parameterised SQL through `SInventorySql` (`OtherStockActionDAL.cs:137-185`). Body-level effects are therefore fully verifiable for the active path (COMPLETE for SQL).

## 17. Database Mapping
| Table | Access | Detail | Source |
|---|---|---|---|
| tblSubDepotStoreFreeze | READ / WRITE | SELECT F.SDStoreFreezeId,F.SubDCStoreId,ProductCode,ProductName,BatchNo,ExpDate,StockQty,(UnitPrice*StockQty) Amount,StockCondition,Remarks WHERE StockQty>0 AND SubDepotId=@ AND P.ManufacId=@ ORDER BY ExpDate; UPDATE StockQty | `OtherStockActionDAL.cs:137-168` |
| tblSubDepotStore | READ / WRITE | StockQty read and UPDATEd | `OtherStockActionDAL.cs:170-185` |
| tblUnitPrice | READ | INNER JOIN on ProductCode (no IsActive filter) | :141-145 |
| tblProduct, tblCompanyUnit, tblUserCompanyUnit, tblSubDepot, tblManufacturer | READ | look-ups | - |
Dead path additionally references `tblDCStoreFreeze`, `tblDCStore` (main depot).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Return Qty | digits and `.` | FilteredTextBoxExtender | - | - | BR-M12-029 |
| Return Qty | not empty / not 0 | - | yes | `Insert Return Quantity !!` | BR-M12-029 |
| Return Qty | <= StockQty of freeze row (integer parse of cell) | - | yes | `Return Quantity must be Less then Stock Qty` | BR-M12-029 |
| Restricted rows | only login `21900` | - | yes (wrong-row read) | none (silent skip) | BR-M12-031 |
| Search | rows exist | - | yes | `No Data Found!!` | BR-M12-028 |
| Sub-depot / manufacturer | must be selected | - | int parse throws `FormatException` on empty | (yellow page) | - |
| After release | success | - | - | `Stock Return Successfully !!` | BR-M12-030 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId). Every sub-depot dropdown is filtered by the DC chosen just before it (`WHERE ComUnitId=@ComUnitId`, label `SubDepotCode:SubDepotName`, value SubDepotId, default item `--------Select---------`). | Library.DAL/SInventory_DAL/RequisitionDAL.cs:45-52,761-770; Library.DAL/SubDepot_DAL/SubDepotDAL.cs:15-19 | RequisitionDAL.SubdeportLoad | CONFIRMED |
| BR-M12-016 | Damaged/un-received qty > 0 on receive creates a freeze row (INSERT tblSubDepotStoreFreeze; StockCondition 'Restricted'; StockQty = TotalQuantity = damage qty; SubDCStoreId = the store row just created; SubDepotId = chalan sub-depot). Restricted stock is therefore excluded from `Available` sellable stock but counted in the stock report as RQty. | Solution.Web/SubDepot_UI/ProductReceiveBySubDeport.aspx.cs:74-99; Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:37-47; Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:35-41; Library.DAL/SInventory_DAL/DCStockReportDAL.cs:530-540 | SubDepotChalanBLL.SaveDCStoreFreeze2 | CONFIRMED |
| BR-M12-028 | Freeze list (Freeze Stock Release): rows of tblSubDepotStoreFreeze with StockQty>0 for the chosen sub-depot and manufacturer, ordered by ExpDate, with Amount = tblUnitPrice.UnitPrice x StockQty (selling/trade price, join NOT restricted to IsActive - duplicate price rows would duplicate freeze rows). All StockCondition values are listed ('Restricted', 'ReturnStock', 'Blocked'). | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:137-151; Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx.cs:45-58 | OtherStockActionDAL.SubDeportLoadData | CONFIRMED |
| BR-M12-029 | Release quantity rules: return qty must be non-empty and not 0 (`Insert Return Quantity !!`) and <= the row's StockQty (`Return Quantity must be Less then Stock Qty`). Both checks operate on integer-parsed cell text (`Convert.ToInt32`). | Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx.cs:369-482 | submitButton0_OnClick | CONFIRMED |
| BR-M12-030 | Release effect: tblSubDepotStoreFreeze.StockQty := StockQty - qty and tblSubDepotStore.StockQty := StockQty + qty on the ORIGINAL store row (SubDCStoreId), by read-modify-write (values read then written back, no transaction, no ledger row, no audit user/date). | Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx.cs:399-424,446-471; Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:153-185 | OtherStockActionBLL.Subdeport* | CONFIRMED |
| BR-M12-031 | Rows whose StockCondition is 'Restricted' can be released only when Session LoginName = '21900' (hard-coded user). For everyone else the condition is read from the wrong grid (loadGridView row i instead of the selected labelGridView row i) and the row is silently skipped or processed depending on which row happens to be at the same index. | Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx.cs:384-386,432-433 | submitButton0_OnClick | CONFIRMED |
| BR-M12-043 | Sub-depot stock report is quantity-only and is built from five buckets per sub-depot/product/batch: Available (tblSubDepotStore.StockQty>0 & StockCondition='Available'), Booked-for-delivery (undelivered sub-invoice quantities), In-transit (DC->Sub chalans with IsDeliver='False'), Blocked and Restricted (tblSubDepotStoreFreeze StockQty>0 by condition). No cost, rate or amount is produced. | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:462-545 | DCStockReportDAL.SubDeportStockReportDetailDataDAL | CONFIRMED |

Calculations: `Amount = UnitPrice x StockQty` (SQL); `newFreezeQty = old - qty`; `newStoreQty = old + qty` (`.cs:412,420`). Quantities are re-read from the DB just before update, but check `qty <= StockQty` uses the value shown in the grid (stale after another user's change).

## 20. Permission Matrix
- Session keys: `UserId` (DC list, `.cs:42`), `LoginName` (hard-coded compare to `"21900"` at :267,:310,:385,:432).
- **Hard-coded privileged user**: login `21900` is the only user allowed to release `Restricted` stock (BR-M12-031). Not a role/permission table check.
- Otherwise menu-based only; DC list restricted to `tblUserCompanyUnit` assignments, but the sub-depot/manufacturer values are posted values and are not re-validated against the user's DCs.

## 21. Status Lifecycle
`tblSubDepotStoreFreeze.StockCondition` values seen in code: `Restricted` (created at receive, `ProductReceiveBySubDeport.aspx.cs:93`), `ReturnStock` (sales-return receive, `ReturnInvoiceDAL.cs:1212-1224`), `Blocked` (counted by the stock report, never created by M12 code). Quantity lifecycle on this page: `frozen StockQty -> (release qty) -> 0 remaining` while the store row gains the same quantity. The `StockCondition` value is not changed on partial release; a row disappears from the list when StockQty reaches 0 (list filter `StockQty>0`). There is no status column recording who released/when.

## 22. Error Handling
No try/catch. `Convert.ToInt32` on decimal stock text (e.g. `10.00`) or on an empty dropdown value throws `FormatException`; `dt.Rows[0]` on a missing row throws `IndexOutOfRange`. Messages through JS `alert`. `Global.asax` `Application_Error` empty.

## 23. Audit
No audit trail: no user/time is written for a release; only the changed quantities remain. (No audit columns exist in the two UPDATE statements.)

## 24. Integration
None.

## 25. Reports
None. The green button is labelled `Print Invoice` (`fa-print`) but no report opens - the label is misleading (GAP-M12-016). Stock effect can be verified with the Sub-depot Stock Report (RPT-M12-06).

## 26. Dependencies
BLL: `OtherStockActionBLL`, `RequisitionBLL`. DAL: `OtherStockActionDAL` (shared with main-depot `OtherStockAction.aspx`), `RequisitionDAL`, `SInventorySql`. Tables: see §17. Control toolkit: `FilteredTextBoxExtender`.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Search | choose DC, sub-depot, manufacturer, Search | freeze rows StockQty>0 for that sub-depot/manufacturer sorted by ExpDate; Amount = UnitPrice x StockQty | BR-M12-028 |
| TC-02 | Release partial (positive) | tick row (StockQty 10, condition `ReturnStock`), qty 4, Add, green button as a normal user | freeze 6, store +4, alert `Stock Return Successfully !!` | BR-M12-030 |
| TC-03 | Restricted as normal user (negative) | row condition `Restricted`, login not 21900 | no update, no message (verify the row at same index in loadGridView) | BR-M12-031 |
| TC-04 | Restricted as 21900 | same as TC-03 as login `21900` | stock released | BR-M12-031 |
| TC-05 | Qty > stock | qty 11 on StockQty 10 | alert `Return Quantity must be Less then Stock Qty`, no update | BR-M12-029 |
| TC-06 | Zero/empty qty | qty 0 (row already added) | alert `Insert Return Quantity !!` | BR-M12-029 |
| TC-07 | Decimal stock | StockQty 10.50 | `Convert.ToInt32` FormatException expected (defect) | BR-M12-029 |
| TC-08 | Duplicate add | add same freeze row twice | second add ignored | - |
| TC-09 | Duplicate price rows | product with 2 tblUnitPrice rows (IsActive 0/1) | freeze row listed twice (join not filtered) | BR-M12-028 |
| TC-10 | Concurrent release | two users release the same row | both read same StockQty, last write wins (no transaction) | BR-M12-030 |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx`, `Solution.Web/SubDepot_UI/SubDeportStockFreez.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SubDeportStockFreez.aspx(.cs)` | n/a | MISSING | - |
| Freeze release rules | None existed | quantity checks + Restricted only by `21900` | freeze/store StockQty updates | CONFIRMED | BR-M12-029..031 |
| Valuation | None existed | selling price x qty, no cost | - | CONFIRMED | BR-M12-028 |
| Permissions | None existed | hard-coded login | - | GAP | GAP-M12-016 |

## 30. Known Gaps
- Hard-coded login `21900` bypass; duplicated 100-line blocks; the Restricted test reads `loadGridView` at the same index instead of the selected freeze row (BR-M12-031).
- `Convert.ToInt32` of a decimal quantity; no transaction/locking around read-modify-write; last-writer-wins.
- Unreachable `RowCommand` path uses **main depot** freeze/store methods (would corrupt `tblDCStoreFreeze/tblDCStore` if ever triggered).
- Button caption `Print Invoice` for a release action; misspelt labels (`Distribution Centerr`).
- Amount join to `tblUnitPrice` ignores `IsActive` and can duplicate rows; amounts are trade price, not cost.
- No audit of who released; `Blocked` condition never produced by sub-depot code.

## 31. Change Impact
- `OtherStockActionDAL`/`OtherStockActionBLL` are shared with main-depot `OtherStockAction.aspx`; the `Subdeport*` methods (:137-185) are used by this page only (grep).
- Data shared with: `ProductReceiveBySubDeport` (creates `Restricted` rows), `ReturnInvoiceDAL.SubDCStockInDAL` (creates `ReturnStock` rows), `DCStockReportDAL.SubDeportStockReportDetailDataDAL` (reports RQty/BQty), `SubDeportProformaorInvoiceReturn` (freeze check queries main tables).
- Changing StockCondition semantics or the freeze list filter affects WF-M12-04 and RPT-M12-06.

