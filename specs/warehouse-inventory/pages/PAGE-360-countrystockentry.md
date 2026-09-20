---
page_id: PAGE-360
module: M10
page: CountryStockEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-360 — CountryStockEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-360 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | CountryStockEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CountryStockEntry |
| aspx size | 14951 bytes |
| Code-behind LOC | 265 |
| Inline script size | 378 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CountryStockEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/CountryStockEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CountryStockEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| JS include | ../css/Date/jquery.datepick.js |
| CSS include | ../css/Date/jquery.datepick.css |
| BLL class | DCStoreBLL (Library.BLL/SInventory_BLL/DCStoreBLL.cs) |
| DAL class | DCStoreDAL (Library.DAL/SInventory_DAL/DCStoreDAL.cs) |
| DAO/entity class | CurrentStock (Library.DAO/SInventory_Entities/CurrentStock.cs) |
| DAO/entity class | DCStore (Library.DAO/SInventory_Entities/DCStore.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| stockReceiveImageButton | ImageButton |  | no validator found |  |  |  | onclick→stockReceiveImageButton_Click |
| reportImageButton | ImageButton |  | no validator found |  |  |  | onclick→reportImageButton_Click |
| chalanNoDateTextBox | TextBox |  | no validator found |  |  |  |  |
| chalanDateTextBox | TextBox |  | no validator found |  |  |  |  |
| productCodeTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→productCodeTextBox_TextChanged |
| BatchNoTextBox | TextBox |  | no validator found |  |  |  |  |
| productNameTextBox | TextBox |  | no validator found |  |  |  |  |
| quantityTextBox | TextBox |  | no validator found |  |  |  |  |
| packSizeTextBox | TextBox |  | no validator found |  |  |  |  |
| expDateTextBox | TextBox |  | no validator found |  |  |  |  |
| receiveDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| addToListButton | Button | Add To List | no validator found |  |  |  | onclick→addToListButton_Click |
| stockGridView | GridView |  | no validator found |  |  |  | onrowcommand→stockGridView_RowCommand |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | Exp Date | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| removeLinkButton | LinkButton |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| productCodeTextBox | productCode | SInventory_UI_CountryStockEntry.productCodeTextBox_TextChanged | DCStoreBLL.ProductInfo | DCStoreDAL.LoadProductCode, DCStoreDAL.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | DCStore.ProductCode | SInventory_UI_CountryStockEntry.addToListButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| productNameTextBox | DCStore.ProductName | SInventory_UI_CountryStockEntry.addToListButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| packSizeTextBox | DCStore.PackSize | SInventory_UI_CountryStockEntry.addToListButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| BatchNoTextBox | DCStore.BatchNo | SInventory_UI_CountryStockEntry.addToListButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| quantityTextBox | DCStore.Quantity | SInventory_UI_CountryStockEntry.addToListButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| expDateTextBox | DCStore.ExpDate | SInventory_UI_CountryStockEntry.addToListButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| chalanDateTextBox | DCStore.ChalanDate | SInventory_UI_CountryStockEntry.submitButton_Click | DCStoreBLL.SaveDhakaStock | DCStoreDAL.CurrentStockParameters, DCStoreDAL.CurrentStockQty, DCStoreDAL.DcStoreParameters, DCStoreDAL.HasProductcode,  | UNKNOWN | UNKNOWN | UNKNOWN |
| chalanNoDateTextBox | DCStore.ChalanNo | SInventory_UI_CountryStockEntry.submitButton_Click | DCStoreBLL.SaveDhakaStock | DCStoreDAL.CurrentStockParameters, DCStoreDAL.CurrentStockQty, DCStoreDAL.DcStoreParameters, DCStoreDAL.HasProductcode,  | UNKNOWN | UNKNOWN | UNKNOWN |
| receiveDateTextBox | DCStore.ReceiveDate | SInventory_UI_CountryStockEntry.submitButton_Click | DCStoreBLL.SaveDhakaStock | DCStoreDAL.CurrentStockParameters, DCStoreDAL.CurrentStockQty, DCStoreDAL.DcStoreParameters, DCStoreDAL.HasProductcode,  | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_CountryStockEntry.Page_Load | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_CountryStockEntry.Page_Load | no | no |
| SInventory_UI_CountryStockEntry.productCodeTextBox_TextChanged | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:80 | object sender, EventArgs e | SInventory_UI_CountryStockEntry.productCodeTextBox_TextChanged → DCStoreBLL.ProductInfo → DCStoreDAL.LoadProductCode / DCStoreDAL.SingleParameter | no | no |
| SInventory_UI_CountryStockEntry.stockGridView_RowCommand | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:102 | object sender, GridViewCommandEventArgs e | SInventory_UI_CountryStockEntry.stockGridView_RowCommand | no | no |
| SInventory_UI_CountryStockEntry.addToListButton_Click | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:140 | object sender, EventArgs e | SInventory_UI_CountryStockEntry.addToListButton_Click | no | no |
| SInventory_UI_CountryStockEntry.submitButton_Click | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:198 | object sender, EventArgs e | SInventory_UI_CountryStockEntry.submitButton_Click → DCStoreBLL.SaveDhakaStock → DCStoreDAL.CurrentStockParameters / DCStoreDAL.CurrentStockQty / DCStoreDAL.DcStoreParameters / DCStoreDAL.HasProductcode / DCStoreDAL.SaveDataForCurrentStock / DCStoreDAL.SaveDat | no | no |
| SInventory_UI_CountryStockEntry.reportImageButton_Click | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:258 | object sender, ImageClickEventArgs e | SInventory_UI_CountryStockEntry.reportImageButton_Click | no | no |
| SInventory_UI_CountryStockEntry.stockReceiveImageButton_Click | Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs:262 | object sender, ImageClickEventArgs e | SInventory_UI_CountryStockEntry.stockReceiveImageButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DCStoreBLL.ProductInfo | Library.BLL/SInventory_BLL/DCStoreBLL.cs:90 |
| DCStoreBLL.SaveDhakaStock | Library.BLL/SInventory_BLL/DCStoreBLL.cs:18 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DCStoreDAL.LoadProductCode | Library.DAL/SInventory_DAL/DCStoreDAL.cs:80 | inline SQL: tblProduct, tblUnitPrice |  |
| DCStoreDAL.SingleParameter | Library.DAL/SInventory_DAL/DCStoreDAL.cs:151 | delegates / no SQL literal |  |
| DCStoreDAL.HasProductcode | Library.DAL/SInventory_DAL/DCStoreDAL.cs:31 | inline SQL: tblCurrentStock |  |
| DCStoreDAL.SaveDataForCurrentStock | Library.DAL/SInventory_DAL/DCStoreDAL.cs:24 | inline SQL: tblCurrentStock |  |
| DCStoreDAL.CurrentStockParameters | Library.DAL/SInventory_DAL/DCStoreDAL.cs:137 | delegates / no SQL literal | @ComUnitCode, @ComUnitId, @PackSize, @ProductCode, @ProductName, @Quantity, @StockId |
| DCStoreDAL.CurrentStockQty | Library.DAL/SInventory_DAL/DCStoreDAL.cs:37 | inline SQL: tblCurrentStock |  |
| DCStoreDAL.UpdateCurrentStockQuantity | Library.DAL/SInventory_DAL/DCStoreDAL.cs:108 | inline SQL: tblCurrentStock | @ProductCode, @Quantity |
| DCStoreDAL.SaveDataForDhakaStock | Library.DAL/SInventory_DAL/DCStoreDAL.cs:17 | inline SQL: tblDCStore |  |
| DCStoreDAL.DcStoreParameters | Library.DAL/SInventory_DAL/DCStoreDAL.cs:118 | delegates / no SQL literal | @BatchNo, @ChalanDate, @ChalanNo, @ComUnitId, @ExpDate, @PackSize, @ProductCode, @ProductName, @Quan |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCurrentStock, tblProduct, tblUnitPrice
- Tables WRITTEN (inline SQL reachable): tblCurrentStock, tblDCStore
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCurrentStock | ComUnitCode | @ComUnitCode |
| tblCurrentStock | ComUnitId | @ComUnitId |
| tblCurrentStock | PackSize | @PackSize |
| tblCurrentStock | ProductCode | @ProductCode |
| tblCurrentStock | ProductName | @ProductName |
| tblCurrentStock | Quantity | @Quantity |
| tblCurrentStock | StockId | @StockId |
| tblDCStore | BatchNo | @BatchNo |
| tblDCStore | ChalanDate | @ChalanDate |
| tblDCStore | ChalanNo | @ChalanNo |
| tblDCStore | ComUnitId | @ComUnitId |
| tblDCStore | ExpDate | @ExpDate |
| tblDCStore | PackSize | @PackSize |
| tblDCStore | ProductCode | @ProductCode |
| tblDCStore | ProductName | @ProductName |
| tblDCStore | Quantity | @Quantity |
| tblDCStore | ReceiveDate | @ReceiveDate |
| tblDCStore | ReceiveId | @ReceiveId |
| tblDCStore | StorageLocation | @StorageLocation |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Company Address!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Company Contact Number!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Company Name!! | UNKNOWN |
| (handler productCodeTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Employee Information Not Found!! | UNKNOWN |
| (handler addToListButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input Data in texbox | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 11 methods.
- Redirect targets: ../Report_UI/CountryStockRpt.aspx
- User messages: Employee Information Not Found!!; Please Input Company Address!!; Please Input Company Contact Number!!; Please Input Company Name!!; Please input Data in texbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DCStoreBLL; DAL: DCStoreDAL; DAO: CurrentStock, DCStore; Tables (inline): tblCurrentStock, tblDCStore, tblProduct, tblUnitPrice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CountryStockEntry.aspx`, `Solution.Web/SInventory_UI/CountryStockEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CountryStockEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 25 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CountryStockEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DCStoreDAL.CurrentStockParameters, DCStoreDAL.CurrentStockQty, DCStoreDAL.DcStoreParameters, DCStoreDAL.HasProductcode, DCStoreDAL.LoadProductCode affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
