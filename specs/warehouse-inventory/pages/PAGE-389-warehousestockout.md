---
page_id: PAGE-389
module: M10
page: WarehouseStockOut
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-389 — WarehouseStockOut

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-389 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WarehouseStockOut |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WarehouseStockIn |
| aspx size | 51305 bytes |
| Code-behind LOC | 856 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Issue (write off / return) part of an **Approved** warehouse stock-in out of Central Warehouse stock, with a mandatory reason. The user reaches the page from `WHStockInList.aspx` (approved stock-ins, `Session["WHId"]`), sees the original lines with current central stock, ticks lines and enters "Out Quantity"; saving records a stock-out document and reduces `tblCentralStore.Quantity`. Source: `Solution.Web/SInventory_UI/WarehouseStockOut.aspx.cs:23-32,76-99,381-454,456-580`. Note: the `.aspx` `Inherits="SInventory_UI_WarehouseStockIn"` - the code-behind file declares class `SInventory_UI_WarehouseStockIn` (same name as PAGE-385).

## 3. Business Objective
Let the warehouse correct or remove received stock (damage, return to supplier, error) with an audit trail (reason, who, when) instead of editing the stock table. INFERRED from the `Reason` field and the stock-out master/detail tables; the `.aspx` labels are copied from the stock-in form (`Stock In Date`, `Challan No`).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/WHStockInList.aspx`

Outbound references: `warehousestockinview.aspx`, `whfreezestockrelease.aspx`, `whstockinlist.aspx`

## 5. Route
`~/SInventory_UI/WarehouseStockOut.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WarehouseStockOut.aspx |
| Code-behind | Solution.Web/SInventory_UI/WarehouseStockOut.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | WHStockListBLL (Library.BLL/SInventory_BLL/WHStockListBLL.cs) |
| BLL class | WarehouseStockInBll (Library.BLL/SInventory_BLL/WarehouseStockInBll.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | WHStockListDAL (Library.DAL/SInventory_DAL/WHStockListDAL.cs) |
| DAL class | WarehouseStockInDal (Library.DAL/SInventory_DAL/WarehouseStockInDal.cs) |
| DAO/entity class | WarehouseStockInMasterDao (Library.DAO/SInventory_Entities/WarehouseStockInMasterDao.cs) |
| DAO/entity class | WharehouseStockInDetailsDao (Library.DAO/SInventory_Entities/WharehouseStockInDetailsDao.cs) |

## 7. UI Control Inventory
No validators (0). Verified controls:

| Control | Label | Required | Default / state | Notes |
|---|---|---|---|---|
| manufacturerDropDownList | Manufacturer | Yes (`Please Select Manufacturer !!!`) | set from stock-in master `ManufacId` | `tblManufacturer` |
| stockInDateTextBox | Stock In Date | Yes | master `WHStockInDate` (dd-MMM-yyyy) | stored as the stock-OUT date |
| challanNoTextBox / challanDateTextBox | Challan No/Date | Yes | from master | - |
| referenceNoTextBox / referenceDateTextBox / remarksTextBox | Reference / Remark | No | from master | not persisted on stock-out master (only ManufacId, date, EntryBy/Date, WHStockInMasterID, Reason) |
| reasonTextBox | Reason | Yes (`Please Give Stock Out Reason !!!`) | empty | persisted to `tblWHStockOutMaster.Reason` |
| productGridView | lines | per row: code, batch, exp, mfg, qty, price, vat non-empty (`Please fill out ...!!`) | rows from `WHStockListDAL.LoadWHStockwithDetail` | DataKeys = `WHStockInDetailID`, `ReceiveId` |
| chkSelect / chkSelectAll | select line | at least one checked (`Please Choose Product !!`) | unchecked | - |
| cStockTextBox | C.Stock | - | `tblCentralStore.Quantity` | ReadOnly |
| qtyTextBox | Out Qty | Yes for checked rows, non-empty and non-zero (`Please fill out Out Quantity!!`) | empty | on change: if qty > C.Stock -> `Cannot be greater than Current Stock Qty !!` and qty reset to `0` |
| submitButton | Save | - | hidden when `sp_GET_btnShow` returns rows (`:44-48`) | - |
| cancelButton | Cancel | - | redirects to `WhFreezeStockRelease.aspx` (`:72-75`, odd target) | - |
| LinkButton1 | back to list | - | redirect `WHStockInList.aspx` (`:852-855`) | - |

## 8. Field Mapping
| UI control | DTO | Handler | BLL | DAL | Table.Column |
|---|---|---|---|---|---|
| masterIdHiddenField | `WHStockInMasterID` | `SaveAllData :386` | `WHStockListBLL.SaveStockOutMasterInfo` | `WHStockListDAL.SaveStockOutMasterInformation :35-60` | `tblWHStockOutMaster.WHStockInMasterID` |
| (generated) | `WHStockOutMasterID` = `MAX(WHStockOutMasterID)+1` | BLL `StockInId` | `ClsPrimaryKeyFind` | same | `WHStockOutMasterID` |
| (generated) | code `WHS-`+7 digits from `MAX(SUBSTRING(WHStockOutCode,5,15))+1` | BLL `WHStockInCode` | `WarehouseStockInCodeId :28-33` | same | `WHStockOutCode` |
| manufacturerDropDownList | `ManufacId` | `:388` | - | same | `ManufacId` |
| stockInDateTextBox | `WHStockInDate` (as `WHStockOutDate`) | `:387` | - | same | `WHStockOutDate` |
| Session LoginName / Now | `EntryBy` / `EntryDate` | `:407-408` | - | same | `EntryBy`, `EntryDate` |
| reasonTextBox | `Reason` | `:409` | - | same | `Reason` |
| DataKeys[i][0] | `WHStockInDetailID` | `:437` | `SaveWarehouseStockOutDetail` | `SaveWarehouseStockOutDetail :62-80` | `tblWHStockOutDetail.WHStockInDetailID` |
| productidHiddenField | `ProductId` | `:439` | same | same | `ProductId` |
| qtyTextBox | `Qty` | `:443` | same | same | `tblWHStockOutDetail.Qty` |
| DataKeys[i][1] | `ReceiveId` | `:447` | same | same | `ReceiveId` |
| qtyTextBox (again) | qty | BLL loop | `WHStockListDAL.UpdateCentralWH :82-87` | `UPDATE tblCentralStore SET Quantity = Quantity - <qty> WHERE MigoDetailID = '<WHStockInDetailID>'` | `tblCentralStore.Quantity` |
Status is not written on the stock-out master (the assignment is commented out in the DAL).

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | DropDownLoad → WarehouseStockInBll.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |
| manufacturerDropDownList | DropDownLoad → WarehouseStockInBll.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Handler | Effect | DB change |
|---|---|---|---|
| submitButton | `submitButton_Click :583` -> `Validation()` -> `SaveAllData` | Insert stock-out master; for each checked line insert stock-out detail then decrement central stock | INSERT `tblWHStockOutMaster`, INSERT `tblWHStockOutDetail` xN, UPDATE `tblCentralStore` xN |
| chkSelectAll | `chkSelectAll_CheckedChanged :819` | toggles all rows | none |
| qtyTextBox (autopostback) | `qtyTextBox_OnTextChanged :836` | compares with C.Stock | none |
| cancelButton | `:72` | redirect `WhFreezeStockRelease.aspx` | none |

## 11. Frontend Flow
Init: `PermissionForShowBtn`, `InitialGrid`, `DropDownLoad` (manufacturers), `loadData()` which reads `Session["WHId"]` (throws NullReference if page opened directly) and binds master + lines (`:23-32,76-99`). Calculations of line/total value are unchanged from PAGE-385 but the price/qty columns are effectively read-only for stock-out purposes; only `qtyTextBox` (Out Qty) is persisted. JS: `pageLoad()` cosmetic.

## 12. Backend Flow
1. `loadData` -> `WHStockListBLL.LoadWHStockwithDetail(Session["WHId"])` -> `WHStockListDAL.LoadWHStockwithDetail` (LEFT JOIN `tblWHStockInDetail`, `tblProduct`, `tblCentralStore ON MigoDetailID = WHStockInDetailID`; `Quantity AS CStock`, `ReceiveId`).
2. `SaveAllData` (`:381-454`): master DAO (`Status="Posted"` set on DAO but not persisted), `SaveStockOutMasterInfo` (ids), then for each **checked** row build detail DAO, then `SaveWarehouseStockOutDetail(list)` = foreach { INSERT detail; `UpdateCentralWH(WHStockInDetailID, Qty)` }; `Clear()`; alert `Data Saved Successfully`.
3. No transaction; no try/catch; quantity vs stock re-check is **not** repeated on the server (only the postback in step 4 of section 11).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WarehouseStockInBll.LoadmanufacturerName | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:19 |
| WHStockListBLL.LoadWHStockwithDetail | Library.BLL/SInventory_BLL/WHStockListBLL.cs:22 |
| WarehouseStockInBll.ProductInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:32 |
| WHStockListBLL.SaveStockOutMasterInfo | Library.BLL/SInventory_BLL/WHStockListBLL.cs:32 |
| WHStockListBLL.StockInId | Library.BLL/SInventory_BLL/WHStockListBLL.cs:40 |
| WHStockListBLL.WHStockInCode | Library.BLL/SInventory_BLL/WHStockListBLL.cs:46 |
| WHStockListBLL.OrdnNoGenerator | Library.BLL/SInventory_BLL/WHStockListBLL.cs:52 |
| WHStockListBLL.SaveWarehouseStockOutDetail | Library.BLL/SInventory_BLL/WHStockListBLL.cs:84 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WarehouseStockInDal.LoadmanufacturerName | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:17 | inline SQL: tblManufacturer |  |
| CommonDataLoad.GetPermissionForShowBtn | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:65 | sp_GET_btnShow | @PageName |
| WHStockListDAL.LoadWHStockwithDetail | Library.DAL/SInventory_DAL/WHStockListDAL.cs:20 | inline SQL: tblCentralStore, tblProduct, tblWHStockInDetail, tblWHStockInMaster |  |
| WarehouseStockInDal.LoadProductCode | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:32 | inline SQL: tblProduct, tblStockUOM, tblUnitPrice |  |
| WHStockListDAL.SaveStockOutMasterInformation | Library.DAL/SInventory_DAL/WHStockListDAL.cs:36 | inline SQL: tblWHStockOutMaster |  |
| WHStockListDAL.WarehouseStockInCodeId | Library.DAL/SInventory_DAL/WHStockListDAL.cs:29 | inline SQL: tblWHStockOutMaster |  |
| WHStockListDAL.SaveWarehouseStockOutDetail | Library.DAL/SInventory_DAL/WHStockListDAL.cs:63 | inline SQL: tblWHStockOutDetail |  |
| WHStockListDAL.UpdateCentralWH | Library.DAL/SInventory_DAL/WHStockListDAL.cs:83 | inline SQL: tblCentralStore |  |

## 16. Stored Procedure Mapping
No business SP. `sp_GET_btnShow` (permission, body absent) and generic `ExecuteAllSqlQueryByStoreProcedure`.

## 17. Database Mapping
| Table | R/W | Notes |
|---|---|---|
| tblWHStockInMaster / tblWHStockInDetail | R | source document |
| tblCentralStore | R + W | `Quantity` decremented by `MigoDetailID` |
| tblProduct | R | names |
| tblWHStockOutMaster | W | WHStockOutMasterID, WHStockOutCode, ManufacId, WHStockOutDate, EntryBy, EntryDate, WHStockInMasterID, Reason |
| tblWHStockOutDetail | W | WHStockOutMasterID, ProductId, Qty, WHStockInDetailID, ReceiveId |
Not touched: `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock` (see GAP-M10-006).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Manufacturer, stock-in date, challan no/date | required | `Validation :458-488` | Please Select Manufacturer !!! / Please insert stock in date !!! / Please insert challan number !!! / Please insert challan date !!! | BR-M10-023 |
| Reason | required | `:489` | Please Give Stock Out Reason !!! | BR-M10-023 |
| Each line code/batch/exp/mfg/req.qty/price/vat | non-empty (all rows, checked or not) | `:502-549` | Please fill out product code!! / batch!! / exp. Date!! / Mfg. Date!! / Req.Qty!! / Price!! / vat!! | BR-M10-023 |
| Checked line Out Qty | non-empty and != 0 | `:551-568` | Please fill out Out Quantity!! | BR-M10-023 |
| At least one checked line | count > 0 | `:571-575` | Please Choose Product !! | BR-M10-023 |
| Out Qty <= C.Stock | client-postback only | `qtyTextBox_OnTextChanged :846` | Cannot be greater than Current Stock Qty !! | BR-M10-024 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-022 | Stock-out is driven from an **Approved** stock-in: `WHStockInList` lists `tblWHStockInMaster.Status='Approved'`, stores `Session["WHId"]`, and the page loads that document's lines with their central stock (`tblCentralStore.Quantity` matched on `MigoDetailID = WHStockInDetailID`). | `Solution.Web/SInventory_UI/WHStockInList.aspx.cs:47-56`; `Library.DAL/SInventory_DAL/WHStockListDAL.cs:14-27` | `loadData` | CONFIRMED |
| BR-M10-023 | Save requires manufacturer, date, challan no/date, reason, every line's code/batch/exp/mfg/qty/price/VAT filled, at least one line checked and, for checked lines, a non-zero Out Quantity. | `WarehouseStockOut.aspx.cs:456-580` | `Validation` | CONFIRMED |
| BR-M10-024 | Out Quantity may not exceed the displayed current stock: enforced only on the qty textbox postback (value reset to `0`); it is not re-validated at save and there is no floor in the SQL. | `WarehouseStockOut.aspx.cs:836-851`; `WHStockListDAL.cs:82-87` | `qtyTextBox_OnTextChanged` | CONFIRMED |
| BR-M10-025 | Saving writes one `tblWHStockOutMaster` (id=`MAX+1`, code `WHS-nnnnnnn` from max of `SUBSTRING(WHStockOutCode,5,15)`+1, Reason, EntryBy/Date) and one `tblWHStockOutDetail` per checked line, and reduces `tblCentralStore.Quantity` by the Out Qty for **all** rows sharing that `MigoDetailID`. | `WHStockListBLL.cs:33-87`; `WHStockListDAL.cs:28-87` | `SaveAllData` | CONFIRMED |
| BR-M10-026 | Stock-out does not adjust the weighted-average ledger (`tbl_INV_CentralStore_WeightedAvg`) or batch ledger (`tbl_INV_Central_BatchWiseStock`); only the legacy `tblCentralStore.Quantity` changes. Requisition picking and the national valuation report read the ledger tables, so they will not see this stock-out. | `WHStockListDAL.cs:82-87` vs `RequisitionDAL.cs:172-235,300-385`; `SP_Update_StockValuation_TransactionDate_Logic.sql:283-307` | - | CONFIRMED (code) / impact INFERRED |

## 20. Permission Matrix
`Session["LoginName"]` (EntryBy) and `Session["WHId"]` (context). Save button hidden via `sp_GET_btnShow` (body absent). No role checks.

## 21. Status Lifecycle
Stock-out master has no Status column written (assignment commented). Upstream: stock-in `Approved` is the precondition; no reverse link (approved stock-in stays `Approved` after stock-out; a second stock-out can be raised again for the same lines).

## 22. Error Handling
No try/catch; `Session["WHId"]` null -> NullReferenceException at load; conversions unguarded (`Convert.ToDecimal(qtyTextBox.Text)`).

## 23. Audit
`EntryBy`, `EntryDate` on `tblWHStockOutMaster`; `Reason`. No before/after quantity log.

## 24. Integration
None.

## 25. Reports
None from this page. Monthly warehouse reports read `tblWHStockOutDetail` (`sp_WHBeenCard`, historical `CWMonthlyInventoryReportDal` SQL: `OtherIsueSample`).

## 26. Dependencies
`WHStockListBLL`, `WHStockListDAL`, `WarehouseStockInBll` (product/manufacturer lookups), `ClsPrimaryKeyFind`, `CommonDataLoad`; tables in section 17.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-389-01 | Open from WHStockInList for an approved doc | lines + C.Stock shown | BR-M10-022 |
| TC-389-02 | Open URL directly (no session) | NullReferenceException | BR-M10-022 |
| TC-389-03 | Save without reason | `Please Give Stock Out Reason !!!` | BR-M10-023 |
| TC-389-04 | Tick a line, leave Out Qty blank | `Please fill out Out Quantity!!` | BR-M10-023 |
| TC-389-05 | Out Qty 15 > C.Stock 10 | alert, qty reset `0`; saving then blocked by non-zero rule | BR-M10-024 |
| TC-389-06 | Out Qty 4, stock 10 | master+detail saved, `tblCentralStore.Quantity` 10 -> 6 | BR-M10-025 |
| TC-389-07 | Two central rows same `MigoDetailID` | both decremented by 4 (double effect) | BR-M10-025 |
| TC-389-08 | Compare `tbl_INV_CentralStore_WeightedAvg.Close_Qty` before/after | unchanged | BR-M10-026 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WarehouseStockOut.aspx`, `Solution.Web/SInventory_UI/WarehouseStockOut.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Stock effect | None existed | legacy `tblCentralStore` only | GAP | BR-M10-026 |

## 30. Known Gaps
- GAP-M10-006: `tblCentralStore` vs weighted-average ledger divergence (BR-M10-026).
- GAP-M10-021: class-name reuse with `WarehouseStockIn.aspx` (`Inherits="SInventory_UI_WarehouseStockIn"`); compile behaviour UNKNOWN.
- UPDATE by `MigoDetailID` not `ReceiveId` (DataKeys[1] ignored) can hit several rows; no floor -> negative quantity possible if server-side check bypassed.
- Cancel redirects to `WhFreezeStockRelease.aspx` (wrong target); `.aspx` labels say Stock In.
- No transaction; string SQL; culture-dependent `WHStockOutDate` string (`WHStockListDAL.cs:47`).

## 31. Change Impact
Shares `WHStockListDAL/BLL` with `WHStockInList`; `tblCentralStore.Quantity` also updated by freeze pages (PAGE-381/394/390), adjustment (PAGE-392), sample stock (PAGE-376), `StockRcvByWhDal`, `RollBackDAL`, `RequisitionDAL`, `ChallanReportViewDAL` - any change to quantity semantics must be reviewed across these.

