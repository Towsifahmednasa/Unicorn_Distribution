---
page_id: PAGE-387
module: M10
page: WarehouseStockInEdit
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-387 — WarehouseStockInEdit

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-387 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WarehouseStockInEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_WarehouseStockInEdit |
| aspx size | 28996 bytes |
| Code-behind LOC | 752 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Stand-alone edit form for an existing warehouse stock-in, opened with `?ID=<WHStockInMasterID>` (`Page_Load :20-26`). Same layout as `WarehouseStockIn.aspx` without the supplier field. It is a near-duplicate of the edit branch in `WarehouseStockIn.aspx` (legacy popup design). **No live caller exists**: `WarehouseStockInView.PopUp()` (which builds the URL) is never invoked - the Edit command instead redirects to `WarehouseStockIn.aspx` through `Session["IDInformation"]` (`WarehouseStockInView.aspx.cs:43-47,72-79`). The page is reachable only by menu (DB, UNKNOWN) or direct URL.

## 3. Business Objective
INFERRED: allow correction of a stock-in header/lines in a popup before approval. Currently dormant/legacy (see section 30).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/WarehouseStockInView.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/WarehouseStockInEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| CSS include | ../css/GV.css |
| BLL class | WarehouseStockInBll (Library.BLL/SInventory_BLL/WarehouseStockInBll.cs) |
| DAL class | WarehouseStockInDal (Library.DAL/SInventory_DAL/WarehouseStockInDal.cs) |
| DAO/entity class | WarehouseStockInMasterDao (Library.DAO/SInventory_Entities/WarehouseStockInMasterDao.cs) |
| DAO/entity class | WharehouseStockInDetailsDao (Library.DAO/SInventory_Entities/WharehouseStockInDetailsDao.cs) |

## 7. UI Control Inventory
Same controls as PAGE-385 except: no `supplierDropDownList`; `submitButton` (asp:Button, text `Update`, confirm `Are you sure you want to Update ?`) and `closeButton`. **No validators.** Server checks (`Validation()` in `WarehouseStockInEdit.aspx.cs`): manufacturer, stock-in date, challan no, challan date, and per-row product code, batch, exp date, mfg date, qty, **price** (required here, unlike PAGE-385) and vat; messages identical to PAGE-385 plus `Please fill out Price!!`. Product selection does not auto-fill price/VAT/UOM (`GetProductInGrid` sets id, name, pack size only).

## 8. Field Mapping
Same as PAGE-385 update path (`UpdateAllData`): master fields -> `WarehouseStockInDal.UpdateStockInMasterInfo`; details -> delete all + `SaveWarehouseStockInDetail`. `SupplierId` is not set on the DAO (defaults 0) and `Status` is **not set** (null).

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | DropDownLoad → WarehouseStockInBll.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Handler | Effect | DB change |
|---|---|---|---|
| submitButton | `submitButton_Click` -> `Validation()` -> `UpdateAllData` | UPDATE master, DELETE details, INSERT details | `tblWHStockInMaster`, `tblWHStockInDetail` |
| ImageButton1/2 | add/remove row | UI only | none |
| closeButton | `closeButton_Click` | close/redirect | none |

## 11. Frontend Flow
Init: `InitialGrid`, `DropDownLoad`, then `stockInIdHiddenField=Request.QueryString["ID"]` and `LoadWarehouseStockInInfo(id)` (`:20-26`; unguarded: null/invalid ID throws). Totals/date helpers identical to PAGE-385 (`CalculateTotalValue`, `mfgDateTextBox_OnTextChanged` +2 years).

## 12. Backend Flow
`UpdateAllData`: builds master DAO without `Status` and `SupplierId`; `UpdateStockInMasterInfo` (SQL concatenation gives `Status = ''`, `ManufacId = ''`, `SupplierId = '0'`); `DeleteWhStockInDetailById`; foreach detail `SaveWarehouseStockInDetail`; `Clear()`; alert `Data Updated Successfully`. No try/catch, no transaction.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WarehouseStockInBll.LoadmanufacturerName | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:19 |
| WarehouseStockInBll.LoadWarehouseStockInMasterInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:122 |
| WarehouseStockInBll.LoadWarehouseStockInDetailInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:128 |
| WarehouseStockInBll.ProductInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:32 |
| WarehouseStockInBll.UpdateStockInMasterInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:133 |
| WarehouseStockInBll.DeleteWhStockInDetailById | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:152 |
| WarehouseStockInBll.UpdateWarehouseStockInDetail | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:143 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WarehouseStockInDal.LoadmanufacturerName | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:17 | inline SQL: tblManufacturer |  |
| WarehouseStockInDal.GetWarehouseStockInMasterInfo | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:199 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.GetWarehouseStockInDetailInfo | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:206 | inline SQL: tblProduct, tblStockUOM, tblWHStockInDetail |  |
| WarehouseStockInDal.LoadProductCode | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:32 | inline SQL: tblProduct, tblStockUOM, tblUnitPrice |  |
| WarehouseStockInDal.UpdateStockInMasterInfo | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:237 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.DeleteWhStockInDetailById | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:280 | inline SQL: tblWHStockInDetail, tblWHStockInDetail |  |
| WarehouseStockInDal.SaveWarehouseStockInDetail | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:137 | inline SQL: tblWHStockInDetail |  |

## 16. Stored Procedure Mapping
`ExecuteAllSqlQueryByStoreProcedure` (generic SQL runner, body absent). No business SP.

## 17. Database Mapping
Same tables as PAGE-385: W `tblWHStockInMaster` (UPDATE), W `tblWHStockInDetail` (DELETE+INSERT), R `tblManufacturer`, `tblProduct`.

## 18. Validation Matrix
See PAGE-385 section 18; additionally price required (`Please fill out Price!!`); supplier not validated. BR-M10-010.

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-010 | Editing via this page requires manufacturer, stock-in date, challan no/date and, for every line, code, batch, exp, mfg, qty, price and VAT (price is mandatory here, optional in PAGE-385). | `Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx.cs` `Validation()` (price block near `:493-498`) | `Validation` | CONFIRMED |
| BR-M10-011 | Saving from this page writes `Status=''` (empty), `ManufacId=''` and `SupplierId=0` on the master because the DAO fields are unset/blanked; the document then matches neither the approval queue (`Status='Posted'`) nor the View list (`Posted`/`Reject`). | `WarehouseStockInEdit.aspx.cs` `UpdateAllData` (no Status assignment); `WarehouseStockInDal.cs:236-254`; `WarehouseStockInApprovalDal.cs:24-26`; `WarehouseStockInDal.cs:191-196` | `UpdateAllData` | CONFIRMED (code); runtime result INFERRED |

## 20. Permission Matrix
Forms auth only; no role/session checks in code (only `UpdateBy=Session["LoginName"]`). No `sp_GET_btnShow` check on this page.

## 21. Status Lifecycle
Status transition attempted: any -> `''` (blank). See BR-M10-011. No approved-status guard: an `Approved` document can be edited by URL (`?ID=`), which also blanks its status.

## 22. Error Handling
No try/catch; `Request.QueryString["ID"]` missing -> exception in `Page_Load`.

## 23. Audit
`UpdateBy` (=`Session["LoginName"]`), `UpdateDate` (`DateTime.Now`); details rewritten without history.

## 24. Integration
None.

## 25. Reports
None from this page.

## 26. Dependencies
`WarehouseStockInBll`, `WarehouseStockInDal`, `WarehouseStockInMasterDao`, `WharehouseStockInDetailsDao`; sibling `WarehouseStockIn.aspx`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-387-01 | Open `WarehouseStockInEdit.aspx?ID=<posted id>` | form populated from master/details | - |
| TC-387-02 | Update with blank price | alert `Please fill out Price!!` | BR-M10-010 |
| TC-387-03 | Update valid data, then open Approval queue | document missing from queue (Status blank) | BR-M10-011 |
| TC-387-04 | Open without `ID` | exception (no guard) | - |
| TC-387-05 | Open an `Approved` document by URL and update | edit allowed; status blanked | BR-M10-011 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx`, `Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Reachability | None existed | No caller in code (`PopUp` unused) | LEGACY | `WarehouseStockInView.aspx.cs:43-47` |
| Status handling | None existed | Status not set -> blank | GAP | `WarehouseStockInEdit.aspx.cs` `UpdateAllData` |

## 30. Known Gaps
- GAP-M10-002 (High): editing an entry blanks `Status`/`ManufacId` (BR-M10-011).
- Legacy/duplicate of PAGE-385 edit branch; dormant popup path.
- No status guard, SQL concatenation, no transaction (same as PAGE-385).

## 31. Change Impact
Shares `WarehouseStockInDal.UpdateStockInMasterInfo/DeleteWhStockInDetailById/SaveWarehouseStockInDetail` with PAGE-385. Removing this page has no code dependents (grep: only `WarehouseStockInView.aspx.cs:44`).

