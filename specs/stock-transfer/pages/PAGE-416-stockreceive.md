---
page_id: PAGE-416
module: M11
page: StockReceive
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-416 — StockReceive

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-416 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | StockReceive |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_StockReceive |
| aspx size | 15079 bytes |
| Code-behind LOC | 159 |
| Inline script size | 378 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockReceive** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/StockReceiveView.aspx`

Outbound references: `currentstockrpt.aspx`, `stockreceiveview.aspx`

## 5. Route
`~/SInventory_UI/StockReceive.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockReceive.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockReceive.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| JS include | ../css/Date/jquery.datepick.js |
| CSS include | ../css/Date/jquery.datepick.css |
| BLL class | CentralStoreBLL (Library.BLL/SInventory_BLL/CentralStoreBLL.cs) |
| DAL class | CentralStoreDAL (Library.DAL/SInventory_DAL/CentralStoreDAL.cs) |
| DAO/entity class | CentralStore (Library.DAO/SInventory_Entities/CentralStore.cs) |
| DAO/entity class | CurrentStock (Library.DAO/SInventory_Entities/CurrentStock.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| stockReceiveImageButton | ImageButton |  | no validator found |  |  |  | onclick→stockReceiveImageButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| ImageButton1 | ImageButton |  | no validator found |  |  |  | onclick→ImageButton1_Click |
| internalNoteNoTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→productCodeTextBox_TextChanged |
| productCodeTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→productCodeTextBox_TextChanged |
| productNameTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→productNameTextBox_TextChanged |
| productNameTextBox_AutoCompleteExtender | ajaxToolkit:AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| packSizeTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| unitPriceTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| funitPrice | ajaxToolkit:FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| BatchNoTextBox | TextBox |  | no validator found |  |  |  |  |
| quantityTextBox | TextBox |  | no validator found |  |  |  |  |
| fquantityTextBox | ajaxToolkit:FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| expDateTextBox | TextBox |  | no validator found |  |  |  |  |
| receiveDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click1 |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| internalNoteNoTextBox | CentralStore.InternalNoteNo | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.CurrentStockQty, CentralStoreDAL.HasProductName, CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDA | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | CentralStore.ProductCode | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.CurrentStockQty, CentralStoreDAL.HasProductName, CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDA | UNKNOWN | UNKNOWN | UNKNOWN |
| productNameTextBox | CentralStore.ProductName | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.CurrentStockQty, CentralStoreDAL.HasProductName, CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDA | UNKNOWN | UNKNOWN | UNKNOWN |
| packSizeTextBox | CentralStore.PackSize | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.CurrentStockQty, CentralStoreDAL.HasProductName, CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDA | UNKNOWN | UNKNOWN | UNKNOWN |
| BatchNoTextBox | CentralStore.BatchNo | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.CurrentStockQty, CentralStoreDAL.HasProductName, CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDA | UNKNOWN | UNKNOWN | UNKNOWN |
| quantityTextBox | CentralStore.Quantity | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.SaveDataForCurrentStock | @Quantity | UNKNOWN (SP body not in repo) | UNKNOWN |
| quantityTextBox | CentralStore.Quantity | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.SaveDataForStockReceive | @Quantity | UNKNOWN (SP body not in repo) | UNKNOWN |
| expDateTextBox | CentralStore.ExpDate | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.SaveDataForStockReceive | @ExpDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| receiveDateTextBox | CentralStore.ReceiveDate | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.SaveDataForStockReceive | @ReceiveDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| quantityTextBox | CentralStore.StockInQty | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.SaveDataForStockReceive | @StockInQty | UNKNOWN (SP body not in repo) | UNKNOWN |
| unitPriceTextBox | CentralStore.UnitPrice | SInventory_UI_StockReceive.submitButton_Click1 | CentralStoreBLL.SaveStockReceive | CentralStoreDAL.SaveDataForStockReceive | @UnitPrice | UNKNOWN (SP body not in repo) | UNKNOWN |
| productCodeTextBox | productCode | SInventory_UI_StockReceive.productCodeTextBox_TextChanged | CentralStoreBLL.ProductInfo | CentralStoreDAL.LoadProduct | UNKNOWN | UNKNOWN | UNKNOWN |
| productNameTextBox | productName | SInventory_UI_StockReceive.productNameTextBox_TextChanged | CentralStoreBLL.ProductInfo | CentralStoreDAL.LoadProduct | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | productCode | SInventory_UI_StockReceive.productNameTextBox_TextChanged | CentralStoreBLL.ProductInfo | CentralStoreDAL.LoadProduct | UNKNOWN | UNKNOWN | UNKNOWN |

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
| SInventory_UI_StockReceive.Page_Load | Solution.Web/SInventory_UI/StockReceive.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_StockReceive.Page_Load | no | no |
| SInventory_UI_StockReceive.GetProductInfo | Solution.Web/SInventory_UI/StockReceive.aspx.cs:27 | string productCode | SInventory_UI_StockReceive.GetProductInfo → CentralStoreBLL.ProductInfo → CentralStoreDAL.LoadProduct | no | no |
| SInventory_UI_StockReceive.submitButton_Click1 | Solution.Web/SInventory_UI/StockReceive.aspx.cs:92 | object sender, EventArgs e | SInventory_UI_StockReceive.submitButton_Click1 → CentralStoreBLL.SaveStockReceive → CentralStoreDAL.CurrentStockQty / CentralStoreDAL.HasProductName / CentralStoreDAL.SaveDataForCurrentStock / CentralStoreDAL.SaveDataForStockReceive / CentralStoreDAL.UpdateCur | no | no |
| SInventory_UI_StockReceive.stockReceiveImageButton_Click | Solution.Web/SInventory_UI/StockReceive.aspx.cs:129 | object sender, ImageClickEventArgs e | SInventory_UI_StockReceive.stockReceiveImageButton_Click | no | no |
| SInventory_UI_StockReceive.productCodeTextBox_TextChanged | Solution.Web/SInventory_UI/StockReceive.aspx.cs:133 | object sender, EventArgs e | SInventory_UI_StockReceive.productCodeTextBox_TextChanged → CentralStoreBLL.ProductInfo → CentralStoreDAL.LoadProduct | no | no |
| SInventory_UI_StockReceive.ImageButton1_Click | Solution.Web/SInventory_UI/StockReceive.aspx.cs:139 | object sender, ImageClickEventArgs e | SInventory_UI_StockReceive.ImageButton1_Click | no | no |
| SInventory_UI_StockReceive.productNameTextBox_TextChanged | Solution.Web/SInventory_UI/StockReceive.aspx.cs:146 | object sender, EventArgs e | SInventory_UI_StockReceive.productNameTextBox_TextChanged → CentralStoreBLL.ProductInfo → CentralStoreDAL.LoadProduct | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CentralStoreBLL.ProductInfo | Library.BLL/SInventory_BLL/CentralStoreBLL.cs:87 |
| CentralStoreBLL.SaveStockReceive | Library.BLL/SInventory_BLL/CentralStoreBLL.cs:18 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CentralStoreDAL.LoadProduct | Library.DAL/SInventory_DAL/CentralStoreDAL.cs:142 | inline SQL: tblUnitPrice | @ProductCode |
| CentralStoreDAL.HasProductName | Library.DAL/SInventory_DAL/CentralStoreDAL.cs:56 | inline SQL: tblCurrentStock | @ProductCode |
| CentralStoreDAL.SaveDataForCurrentStock | Library.DAL/SInventory_DAL/CentralStoreDAL.cs:39 | inline SQL: tblCurrentStock | @ComUnitCode, @ComUnitId, @PackSize, @ProductCode, @ProductName, @Quantity, @StockId |
| CentralStoreDAL.SaveDataForStockReceive | Library.DAL/SInventory_DAL/CentralStoreDAL.cs:17 | inline SQL: tblCentralStore | @BatchNo, @ExpDate, @InternalNoteNo, @PackSize, @ProductCode, @ProductName, @Quantity, @ReceiveDate, |
| CentralStoreDAL.CurrentStockQty | Library.DAL/SInventory_DAL/CentralStoreDAL.cs:66 | inline SQL: tblCurrentStock | @ProductCode |
| CentralStoreDAL.UpdateCurrentStockQuantity | Library.DAL/SInventory_DAL/CentralStoreDAL.cs:153 | inline SQL: tblCurrentStock | @ProductCode, @Quantity |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCurrentStock, tblUnitPrice
- Tables WRITTEN (inline SQL reachable): tblCentralStore, tblCurrentStock
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCentralStore | BatchNo | @BatchNo |
| tblCentralStore | ExpDate | @ExpDate |
| tblCentralStore | InternalNoteNo | @InternalNoteNo |
| tblCentralStore | PackSize | @PackSize |
| tblCentralStore | ProductCode | @ProductCode |
| tblCentralStore | ProductName | @ProductName |
| tblCentralStore | Quantity | @Quantity |
| tblCentralStore | ReceiveDate | @ReceiveDate |
| tblCentralStore | ReceiveId | @ReceiveId |
| tblCentralStore | StockInQty | @StockInQty |
| tblCentralStore | TotalAmount | @TotalAmount |
| tblCentralStore | UnitPrice | @UnitPrice |
| tblCurrentStock | ComUnitCode | @ComUnitCode |
| tblCurrentStock | ComUnitId | @ComUnitId |
| tblCurrentStock | PackSize | @PackSize |
| tblCurrentStock | ProductCode | @ProductCode |
| tblCurrentStock | ProductName | @ProductName |
| tblCurrentStock | Quantity | @Quantity |
| tblCurrentStock | StockId | @StockId |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| rBatchNoTextBox | RequiredFieldValidator | yes (validator) | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |
| (handler GetProductInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Product Information Not Found!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Batch!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Product Code!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Quantity!! | UNKNOWN |
| (handler submitButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

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
- Redirect targets: StockReceiveView.aspx
- User messages: Please Input Batch!!; Please Input Product Code!!; Please Input Quantity!!; Please input data in all Textbox; Product Information Not Found!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CentralStoreBLL; DAL: CentralStoreDAL; DAO: CentralStore, CurrentStock; Tables (inline): tblCentralStore, tblCurrentStock, tblUnitPrice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockReceive.aspx`, `Solution.Web/SInventory_UI/StockReceive.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockReceive.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 20 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockReceive.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CentralStoreDAL.CurrentStockQty, CentralStoreDAL.HasProductName, CentralStoreDAL.LoadProduct, CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDAL.SaveDataForStockReceive affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
