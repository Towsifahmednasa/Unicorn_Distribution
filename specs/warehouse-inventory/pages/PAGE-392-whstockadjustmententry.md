---
page_id: PAGE-392
module: M10
page: WHStockAdjustmentEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-392 — WHStockAdjustmentEntry

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-392 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WHStockAdjustmentEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WHStockAdjustmentEntry |
| aspx size | 20699 bytes |
| Code-behind LOC | 256 |
| Inline script size | 738 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Central Warehouse stock adjustment. The user enters transaction date, adjustment type, from store (warehouse), optional to store, remarks, chooses a product, lists that product's central stock rows (`tblCentralStore`, `Quantity>0`), ticks rows and enters quantities; saving records an **Approved** adjustment (`tblWHStockAdjustment` + `tblWHAdjustmentDetail`) and reduces `tblCentralStore.Quantity`. Source: `Solution.Web/SInventory_UI/WHStockAdjustmentEntry.aspx.cs`, `Library.DAL/SInventory_DAL/WHStockAdjDAL.cs`.

## 3. Business Objective
Correct/write down central stock (damage, loss, counting differences) with an audit document instead of ad-hoc updates. INFERRED. Note the adjustment type/to-store are recorded but do not change the arithmetic (BR-M10-052).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/WHStockAdjustmentView.aspx`

Outbound references: `whstockadjustmentview.aspx`

## 5. Route
`~/SInventory_UI/WHStockAdjustmentEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WHStockAdjustmentEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/WHStockAdjustmentEntry.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | WHStockAdjDAL (Library.DAL/SInventory_DAL/WHStockAdjDAL.cs) |
| DAO/entity class | WHStockAdjDAO (Library.DAO/SInventory_Entities/WHStockAdjDAO.cs) |
| DAO/entity class | WhStockAdjDetailDAO (Library.DAO/SInventory_Entities/WhStockAdjDetailDAO.cs) |

## 7. UI Control Inventory
Labels (aspx): Transaction Date*, Adjustment Type*, From Store*, To Store*, Remarks*, Product* (all shown with red asterisk). No validators. Server checks: transaction date, from store (`fromstoreDropDownList`, pre-set to index 1), remarks, and at least one ticked row with quantity.

| Control | Required (server) | Notes |
|---|---|---|
| transactionDateTextBox | Yes (`Give Transaction Date !!!`) | date used in transaction no. |
| adjustmentTypeDropDownList | not validated | `tblAdjustmentType` |
| fromstoreDropDownList | Yes (`Give Whearhouse`) | static list in `.aspx`: blank and `1 = CWH` (pre-selected index 1 on first load) |
| tostoreDropDownList1 | No (0 if blank) | `tblCompanyUnit` |
| remarksTextBox | Yes (`Give Remarks`) | - |
| productDropDownList | needed to list rows | `ProductCode:ProductName` |
| productGridView | rows with `cStockTextBox` (Quantity), `reqQtyTextBox` (adjust qty, autopostback compare), `chkSelect` | DataKeys `ReceiveId` |
| btnSave | hidden if `sp_GET_btnShow` returns rows | `PermissionForShowBtn` is called on **every** postback (not inside `!IsPostBack`, `:22`) |

## 8. Field Mapping
| UI | DTO | DAL | Table.Column |
|---|---|---|---|
| transactionDateTextBox | `TransactionDate` | `SaveWHStockAdjMaster :39-74` (parameterised, returns identity via `SaveDataByInsertCommandWithIdentity`) | `tblWHStockAdjustment.TransactionDate` |
| (generated) | `TransactionNo` = `GetTransNo(date)` | `GetTransNo :149-155` | `TransactionNo` |
| adjustmentTypeDropDownList | `AdjustmentType` | same | `AdjustmentType` |
| stockeffectTextBox (hidden) | `StockEffect` | same | `StockEffect` |
| fromstoreDropDownList / tostoreDropDownList1 | `FromStore` / `toStore` (0 if blank) | same | `FromStore`, `toStore` |
| remarksTextBox | `Remarks` | same | `Remarks` |
| Login / Now / `Approved` | `EntryBy`, `EntryDate`, `ActionStatus` | same | `EntryBy`, `EntryDate`, `ActionStatus` |
| row: reqQtyTextBox | `Quantity` | `SaveWHStockAdjDetail :76-108` | `tblWHAdjustmentDetail.Quantity` |
| row DataKey | `ReceiveId` | same | `ReceiveId` |
| row qty + sign `-` | qty | `UpdateCentralStoreStock :174-178` | `UPDATE tblCentralStore SET Quantity=Quantity-<qty> WHERE ReceiveId='<id>' AND ProductId='<pid>'` |

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| adjustmentTypeDropDownList | DropDownList → WHStockAdjDAL.LoadAdjustmentType | inline SQL | AdjustmentType/AdjustmentTypeId | tblAdjustmentType |
| fromstoreDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| tostoreDropDownList1 | DropDownList → WHStockAdjDAL.LoadTostore | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit |
| productDropDownList | DropDownList → WHStockAdjDAL.LoadProduct | inline SQL | Product/ProductId | tblProduct |

## 10. Button Action Mapping
| Button | Handler | Effect | DB |
|---|---|---|---|
| Button2 (search) | `Button2_Click :93` | `LoadProductFromWH(productId)` = `SELECT *,ROW_NUMBER()... FROM tblCentralStore WHERE ProductId='<id>' and Quantity>0` | none |
| btnSave | `Button1_Click :148` | validate, save master, detail, decrement stock per ticked row | INSERT master/detail, UPDATE `tblCentralStore` |
| btnReset | `btnReset_Click :252` | reload page | none |
| reqQtyTextBox change | `reqQtyTextBox_OnTextChanged :232` | if `Quantity` (C.Stock) < qty -> alert and clear | none |

## 11. Frontend Flow
Init: `PermissionForShowBtn` (every request), then on first load `DropDownList()` (products, adjustment types, to-store) and `fromstoreDropDownList.SelectedIndex=1`. Qty check runs as an autopostback of the qty textbox. Client JS `pageLoad` cosmetic; header select-all toggles rows.

## 12. Backend Flow
`Button1_Click`: `Validation()` -> DAO (`ActionStatus="Approved"`, `AdjustmentType`, `toStore` or 0) -> `GetTransNo(date)` -> `SaveWHStockAdjMaster` (identity) -> if >0, for each ticked row: `SaveWHStockAdjDetail` (identity) and if >0 `UpdateCentralStoreStock(productId, qty, receiveId, "-")` (sign hard-coded to `-`; the `AdjustmentType==1 => "+"` branch is commented out) -> `Data Saved Successfully`, `Clear()`. No try/catch/transaction.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WHStockAdjDAL.LoadProduct | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:157 | inline SQL: tblProduct |  |
| WHStockAdjDAL.LoadAdjustmentType | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:163 | inline SQL: tblAdjustmentType |  |
| WHStockAdjDAL.LoadTostore | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:169 | inline SQL: tblCompanyUnit |  |
| CommonDataLoad.GetPermissionForShowBtn | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:65 | sp_GET_btnShow | @PageName |
| WHStockAdjDAL.LoadProductFromWH | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:128 | inline SQL: tblCentralStore |  |
| WHStockAdjDAL.GetTransNo | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:150 | inline SQL: tblWHStockAdjustment |  |
| WHStockAdjDAL.SaveWHStockAdjMaster | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:40 | inline SQL: tblWHStockAdjustment | @ActionStatus, @AdjustmentType, @EntryBy, @EntryDate, @FromStore, @Remarks, @StockEffect, @Transacti |
| WHStockAdjDAL.SaveWHStockAdjDetail | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:77 | inline SQL: tblWHAdjustmentDetail | @Quantity, @ReceiveId, @WHStockAdjId |
| WHStockAdjDAL.UpdateCentralStoreStock | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:175 | inline SQL: tblCentralStore |  |

## 16. Stored Procedure Mapping
`sp_GET_btnShow` (permission, body absent). Otherwise inline SQL (mix of parameterised inserts and concatenated selects/updates).

## 17. Database Mapping
| Table | R/W | Columns |
|---|---|---|
| tblWHStockAdjustment | W | TransactionNo, TransactionDate, AdjustmentType, StockEffect, FromStore, toStore, Remarks, EntryBy, EntryDate, ActionStatus |
| tblWHAdjustmentDetail | W | WHStockAdjId, Quantity, ReceiveId |
| tblCentralStore | R + W | `Quantity` decremented |
| tblAdjustmentType, tblCompanyUnit, tblProduct | R | dropdowns |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Date | required | `Validation :125` | Give Transaction Date !!! | BR-M10-049 |
| From store | required | `:130` | Give Whearhouse | BR-M10-049 |
| Remarks | required | `:135` | Give Remarks | BR-M10-049 |
| Rows | >=1 ticked with qty | `ProductGridValudation :107-122` | Check or Give quantity of at least One Product | BR-M10-049 |
| Qty <= C.Stock | qty textbox postback | `:244` | Quantity must be greater then Current Stock !!! (wording inverted) | BR-M10-050 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-049 | An adjustment needs transaction date, from store, remarks and at least one ticked line with a quantity. Adjustment type and to-store are not validated. | `WHStockAdjustmentEntry.aspx.cs:123-147` | `Validation` | CONFIRMED |
| BR-M10-050 | Selectable stock rows are `tblCentralStore` rows of the chosen product with `Quantity>0`; entered quantity may not exceed the row's quantity (postback check with a misleading message; not re-checked at save; no floor in SQL). | `WHStockAdjDAL.cs:127-131`; `WHStockAdjustmentEntry.aspx.cs:232-250` | `reqQtyTextBox_OnTextChanged` | CONFIRMED |
| BR-M10-051 | Transaction number = `WHA-` + `yy` + `MM` + (`MAX(SUBSTRING(TransactionNo,9,11))`, default 10000, `+1`) - a single running sequence across all months (the max ignores month). Built by concatenating the user-entered date text into SQL. | `WHStockAdjDAL.cs:149-155` | `GetTransNo` | CONFIRMED |
| BR-M10-052 | Every saved adjustment is created `ActionStatus='Approved'` (no approval step) and **always decreases** `tblCentralStore.Quantity` on the selected rows (`sign="-"`), whatever the adjustment type; `toStore` is stored but no stock is added anywhere. | `WHStockAdjustmentEntry.aspx.cs:148-216`; `WHStockAdjDAL.cs:174-178` | `Button1_Click` | CONFIRMED |
| BR-M10-053 | Deleting an adjustment from `WHStockAdjustmentView` (EditData button) adds the detail quantities back (`sign="+"`) then deletes details and master. | `WHStockAdjustmentView.aspx.cs:58-88`; `WHStockAdjDAL.cs:179-183` | `loadGridView_RowCommand` | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]`; button hidden by `sp_GET_btnShow` (body/per-user scope UNKNOWN). No role check; auto-approved so there is no checker.

## 21. Status Lifecycle
`ActionStatus`: `Approved` at creation (no other value written). Delete restores stock.

## 22. Error Handling
No try/catch; conversion errors unhandled; message text has copy/paste mistakes.

## 23. Audit
`EntryBy`, `EntryDate`; TransactionNo/Date. No update audit.

## 24. Integration
None.

## 25. Reports
`WHStockAdjustmentListReportViewer.aspx?WHStockAdjId=` -> `crpWhStockAdjustmentListInfo.rpt` (RPT-M10-05) from the view page.

## 26. Dependencies
`WHStockAdjDAL`, DAOs `WHStockAdjDAO`, `WhStockAdjDetailDAO`, `CommonDataLoad`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-392-01 | Save without remarks | `Give Remarks` | BR-M10-049 |
| TC-392-02 | Save with no ticked qty | `Check or Give quantity of at least One Product` | BR-M10-049 |
| TC-392-03 | Qty 12 with C.Stock 10 | alert, cleared | BR-M10-050 |
| TC-392-04 | Type "Addition" (type id 1) | stock still decreases | BR-M10-052 |
| TC-392-05 | Two saves same month | numbers WHA-yymm10001, WHA-yymm10002 | BR-M10-051 |
| TC-392-06 | Delete adjustment | quantities restored | BR-M10-053 |
| TC-392-07 | Bypass client check with qty > stock | negative `tblCentralStore.Quantity` | BR-M10-050 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WHStockAdjustmentEntry.aspx`, `Solution.Web/SInventory_UI/WHStockAdjustmentEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | - | MISSING |
| Adjustment types (increase/decrease) | None existed | decrease only | GAP (BR-M10-052) |

## 30. Known Gaps
- GAP-M10-012: adjustments cannot increase stock; type ignored; auto-approved (no maker/checker).
- Legacy `tblCentralStore` only; weighted-average ledger not adjusted (GAP-M10-006).
- `GetTransNo` SQL injection via date text; `transactionDateTextBox` label copy-paste artefacts; `PermissionForShowBtn` executed on every postback; message wording inverted.

## 31. Change Impact
`tblCentralStore.Quantity` shared with PAGE-389/381/394/390/376; adjustment tables read by `WHStockAdjustmentView`, report viewer, `CWMonthlyInventory` (historical SQL).

