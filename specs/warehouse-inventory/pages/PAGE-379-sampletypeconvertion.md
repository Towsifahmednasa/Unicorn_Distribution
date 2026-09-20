---
page_id: PAGE-379
module: M10
page: SampleTypeConvertion
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-379 — SampleTypeConvertion

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-379 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | SampleTypeConvertion |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SampleTypeConvertion |
| aspx size | 34219 bytes |
| Code-behind LOC | 342 |
| Inline script size | 957 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED from labels/messages):** Records conversion of a DC's stock between "Sound" and "Sample" types (`Action`: `Sample to Sound` / `Sound to Sample`) for a chosen product, with a date. The user selects a Sales Center (DC), action, date and product, searches the DC's stock rows of that product, enters a "Convention Stock" quantity per batch row, stages rows and saves. A document `SSDnnnnnnnn` is stored and the quantities are **deducted** from the selected `tblDCStore` rows. The companion list page `SampleTypeConventionView.aspx` can delete a document, which adds the quantities back. Source: `Solution.Web/SInventory_UI/SampleTypeConvertion.aspx.cs`, `Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs`.

## 3. Business Objective
INFERRED: take saleable DC stock out of the sellable pool for use as samples (or reverse). The code does not distinguish the two actions when changing stock - see BR-M10-041.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/SampleTypeConventionView.aspx`

Outbound references: `sampletypeconventionview.aspx`

## 5. Route
`~/SInventory_UI/SampleTypeConvertion.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SampleTypeConvertion.aspx |
| Code-behind | Solution.Web/SInventory_UI/SampleTypeConvertion.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | SampleTypeConventionBLL (Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs) |
| DAL class | SampleTypeConventionDal (Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs) |
| DAO/entity class | SampleStockForDcDetails (Library.DAO/SInventory_Entities/SampleStockForDcDetails.cs) |
| DAO/entity class | SampleStockForDcMaster (Library.DAO/SInventory_Entities/SampleStockForDcMaster.cs) |

## 7. UI Control Inventory
No validators. Controls: `DistributioncenterDropDownList1` (label `SC`, all company units, text `ComUnitCode:ComUnitName`, required), `ActionDropDownList` (values 0 `Select One`, 1 `Sample to Sound`, 2 `Sound to Sample`, required != 0), `DateTextBox` (required, CalendarExtender), `productDropDownList` (`ProductCode:ProductName`, required), `Button1` `Search Product` (`No Product Found!!` if none), `DerectStoctGridView` (chk, code, name, Stock Qty, batch, exp, receive date, `ConventionTextBox` digits-only extender), `addButton` `Add`, `ProductGridView` (staged, DataKeys `DCStoreId`, remove button), `submitButton` `Submit`, view link -> `SampleTypeConventionView.aspx`. `DistributioncenterDropDownList1_OnSelectedIndexChanged` writes `Session["ComUnitId"]` (side effect for shared autocomplete services).

## 8. Field Mapping
| UI | DTO | Handler | DAL | Table.Column |
|---|---|---|---|---|
| DistributioncenterDropDownList1 | `SampleStockForDcMaster.ComUnitId` | `submitButton_Click1 :291-303` | `SaveDataForSubDcStockOutMaster :49-65` | `tblSampleStockForDcMaster.ComUnitId` |
| ActionDropDownList (text) | `Action` | same | same | `Action` |
| DateTextBox | `Date` | same | same | `Date` |
| LoginName / Now / "Posted" | `EntryBy` / `EntryDate` / `Status` | same | same | `EntryBy`, `EntryDate`, `Status` |
| (generated) | id `MAX+1`, code `SSD`+8 digits | BLL `SaveDataForSubDcStockOutMasterBll` | `SubStockOutMasterCodeGenerator` | `SampleStockForDcMasterId`, `SampleStockForDcMasterCode` |
| staged row DataKey / cells | `DCStoreId`, `ProductCode`, `ProductName`, `BatchNo`, `ExpDate`, `ReceiveDate`, `SampleStock` (Int32 of Convention Stock) | `:312-323` | `SaveDataForStockOutDetailDal :67-84` | `tblSampleStockForDcDetails.*` |
| same | `SampleStock` | BLL `StockOut` | `StockOuTCentral :128-136` | `tblDCStore.StockQty = StockQty - @SampleStock WHERE DCStoreId` |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Text/Value |
|---|---|---|---|
| DistributioncenterDropDownList1 | `SampleTypeConventionDal.SalesCenterLoadDal :20-24` | `select ComUnitId,(ComUnitCode+':'+ComUnitName) as Com from tblCompanyUnit` | `Com` / `ComUnitId` |
| productDropDownList | `ProductLoadDal :26-31` | `SELECT (ProductCode+':'+ProductName)Pro,* FROM tblProduct` | `Pro` / `ProductId` |
| stock grid | `GetProductDcStore :33-47` | `tblDCStore LEFT JOIN tblProduct ... WHERE ComUnitId=@ComUnitId AND ProductId=@ProductId AND StockQty>0` | rows |

## 10. Button Action Mapping
| Button | Handler | Effect | DB |
|---|---|---|---|
| Button1 (Search Product) | `Button1_Click :199` | binds DC stock rows for product | none |
| addButton | `addButton_Click :65` | stages ticked rows with qty (dedupe by DCStoreId) | none |
| DeleteImageButton | `:117` | unstage | none |
| submitButton | `submitButton_Click1 :291` | validates, saves master, details, then deducts stock, redirects to view page | INSERT master/details; UPDATE `tblDCStore` |
| LinkButton4 (cancel) | `cancelButton_Click :29` -> `Clear` | reset | none |

## 11. Frontend Flow
Init: `ProductLoadBll`, `SalesCenterLoadBll`. `dQtyTextBox_TextChanged` (`:161-197`) loops checked rows and compares Convention qty to stock qty; if greater -> `Stock Out Qty. can't be more then Stock Quantity` and clears the box. Staging via view-state grids. JS: `pageLoad` cosmetic.

## 12. Backend Flow
`submitButton_Click1` (`:291-341`): `Validation()`; master insert; if OK, build detail list from `ProductGridView` cells (`Convert.ToInt32(cell6 text)`), `SaveDataForSubStockOutDetailBll` (foreach insert with `MAX+1` ids), `StockOut` (foreach `tblDCStore.StockQty -= SampleStock`), alert, redirect to `SampleTypeConventionView.aspx`. If the detail save returns false nothing further happens silently; no try/catch/transaction. Stock is never re-read/compared at save time.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SampleTypeConventionBLL.ProductLoadBll | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:22 |
| SampleTypeConventionBLL.SalesCenterLoadBll | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:27 |
| SampleTypeConventionBLL.GetProductDcStore | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:32 |
| SampleTypeConventionBLL.SaveDataForSubDcStockOutMasterBll | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:37 |
| SampleTypeConventionBLL.SubStockOutMasterCodeGenerator | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:44 |
| SampleTypeConventionBLL.SaveDataForSubStockOutDetailBll | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:82 |
| SampleTypeConventionBLL.StockOut | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:92 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SampleTypeConventionDal.ProductLoadDal | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:27 | inline SQL: tblProduct |  |
| SampleTypeConventionDal.SalesCenterLoadDal | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:21 | inline SQL: tblCompanyUnit |  |
| SampleTypeConventionDal.GetProductDcStore | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:34 | inline SQL: tblDCStore, tblProduct | @ComUnitId, @ProductId |
| SampleTypeConventionDal.SaveDataForSubDcStockOutMaster | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:50 | inline SQL: tblSampleStockForDcMaster | @Action, @ComUnitId, @Date, @EntryBy, @EntryDate, @SampleStockForDcMasterCode, @SampleStockForDcMast |
| SampleTypeConventionDal.SaveDataForStockOutDetailDal | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:68 | inline SQL: tblSampleStockForDcDetails | @BatchNo, @DCStoreId, @ExpDate, @ProductCode, @ProductName, @ReceiveDate, @SampleStock, @SampleStock |
| SampleTypeConventionDal.StockOuTCentral | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:129 | inline SQL: tblDCStore | @DCStoreId, @SampleStock |

## 16. Stored Procedure Mapping
None.

## 17. Database Mapping
| Table | R/W | Notes |
|---|---|---|
| tblDCStore | R + W | in-place `StockQty` decrement |
| tblSampleStockForDcMaster | W | SampleStockForDcMasterId, SampleStockForDcMasterCode, ComUnitId, Action, Date, EntryBy, EntryDate, Status |
| tblSampleStockForDcDetails | W | SampleStockForDcDetailsId, SampleStockForDcMasterId, DCStoreId, ProductCode, ProductName, BatchNo, ReceiveDate, ExpDate, SampleStock |
| tblProduct, tblCompanyUnit | R | lookups |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| SC | required | `Validation :262` | Please Select sales center!! | BR-M10-038 |
| Action | != "0" | `:267` | Please Select Action !! | BR-M10-038 |
| Date | required | `:272` | Please Select  Date!! | BR-M10-038 |
| Product | required | `:278` | Please Select Product!! | BR-M10-038 |
| Staged rows | >=1 | `:284` | Please Add product and Stock  Qty | BR-M10-038 |
| Convention qty <= stock | on textbox postback only | `dQtyTextBox_TextChanged` | Stock Out Qty. can't be more then Stock Quantity | BR-M10-040 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-038 | Saving requires a sales center, an action (not `Select One`), a date, a product and at least one staged stock row. | `SampleTypeConvertion.aspx.cs:259-290` | `Validation` | CONFIRMED |
| BR-M10-039 | Selectable rows are `tblDCStore` rows of the chosen DC and product with `StockQty>0` (any StockCondition - the query has no `Available` filter). | `SampleTypeConventionDal.cs:33-47` | `GetProductDcStore` | CONFIRMED |
| BR-M10-040 | Convention quantity may not exceed the row's stock: enforced only in the textbox-change handler (input cleared); not re-checked at save; the field accepts digits only. | `SampleTypeConvertion.aspx.cs:161-197` | `dQtyTextBox_TextChanged` | CONFIRMED |
| BR-M10-041 | Save stores document `SSD`+8-digit id (`Status='Posted'`, Action text) with detail rows, then subtracts each row's quantity from `tblDCStore.StockQty` (`WHERE DCStoreId`) for **both** actions; no matching increase is written anywhere (no sample stock row is created in `tblDCStore`), and the `Action` value does not alter the arithmetic. | `SampleTypeConvertion.aspx.cs:291-341`; `SampleTypeConventionBLL.cs:28-63,80-98`; `SampleTypeConventionDal.cs:128-136` | `submitButton_Click1` | CONFIRMED |
| BR-M10-042 | Deleting a document on the View page re-adds the detail quantities to the same `tblDCStore` rows and deletes details then master (reversal, no status guard). | `SampleTypeConventionView.aspx.cs:38-62`; `SampleTypeConventionDal.cs:96-135` | `loadGridView_RowCommand` | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]` for EntryBy; `Session["ComUnitId"]` overwritten on DC selection. No role/DC-assignment restriction: all company units are listed (`SalesCenterLoadDal`), not just the user's own.

## 21. Status Lifecycle
`tblSampleStockForDcMaster.Status`: `Posted` on save (`:302`); no approval step and no other transition in this module (delete removes the document). UNKNOWN whether other modules consume `Status`.

## 22. Error Handling
No try/catch; conversions such as `Convert.ToDateTime(cell.Text)` on formatted cell text (dd-MMM-yyyy) can fail under other cultures. Failure of `SaveDataForSubStockOutDetailBll` is silent (returns true always: BLL ignores DAL result).

## 23. Audit
`EntryBy`, `EntryDate` on master; none on stock change.

## 24. Integration
None.

## 25. Reports
Print button (`Button2`) placeholder; the view page opens `DcStockOutReportViewer.aspx?DcStockOutMasterId=` (a different document type - see PAGE-363) for `ReportView`.

## 26. Dependencies
`SampleTypeConventionBLL/Dal`, `SampleStockForDcMaster/Details` DAO, `SInventorySql`, `ClsPrimaryKeyFind`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-379-01 | Save with action `Select One` | `Please Select Action !!` | BR-M10-038 |
| TC-379-02 | Search product with no stock | `No Product Found!!` | BR-M10-039 |
| TC-379-03 | Qty > stock | alert, textbox cleared | BR-M10-040 |
| TC-379-04 | Save 5 units `Sample to Sound` | master `SSD0000000n`, detail, tblDCStore.StockQty -5 | BR-M10-041 |
| TC-379-05 | Save 5 units `Sound to Sample` | identical stock effect (-5) | BR-M10-041 |
| TC-379-06 | Delete the document on view page | StockQty +5 restored, rows deleted | BR-M10-042 |
| TC-379-07 | Two users convert the same row concurrently | both pass UI check; StockQty can go negative | BR-M10-040 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SampleTypeConvertion.aspx`, `Solution.Web/SInventory_UI/SampleTypeConvertion.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Sound<->Sample semantics | None existed | Action label has no effect on quantities | GAP | BR-M10-041 |

## 30. Known Gaps
- GAP-M10-010: both conversion directions decrement DC stock and no sample-stock counterpart is created; the business meaning of the two actions is not implemented (INFERRED intent).
- No server-side stock re-check (negative stock possible), no transaction, no status/approval, BLL swallows DAL failures.
- Sample stock rows remain in `tblDCStore` semantics with in-place update, ignored by ledger-style valuation (README data model).

## 31. Change Impact
`tblDCStore.StockQty` shared with all DC stock pages; `SampleTypeConventionView` reverses; report viewer `DcStockOutReportViewer` is for `tblDeStockOut*`, not this document.

