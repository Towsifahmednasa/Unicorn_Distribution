---
page_id: PAGE-419
module: M11
page: StockTransferDcToDc
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-419 — StockTransferDcToDc

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-419 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | StockTransferDcToDc |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_StockTransferDcToDc |
| aspx size | 22631 bytes |
| Code-behind LOC | 312 |
| Inline script size | 378 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockTransferDcToDc** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/StockTransferDcToDc.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| JS include | ../css/Date/jquery.datepick.js |
| CSS include | ../css/Date/jquery.datepick.css |
| BLL class | ChalanBLL (Library.BLL/SInventory_BLL/ChalanBLL.cs) |
| BLL class | DCStoreBLL (Library.BLL/SInventory_BLL/DCStoreBLL.cs) |
| DAL class | ChalanDAL (Library.DAL/SInventory_DAL/ChalanDAL.cs) |
| DAL class | DCStoreDAL (Library.DAL/SInventory_DAL/DCStoreDAL.cs) |
| DAO/entity class | ChalanDetail (Library.DAO/SInventory_Entities/ChalanDetail.cs) |
| DAO/entity class | ChalanInfo (Library.DAO/SInventory_Entities/ChalanInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| chalanDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| chalanNoTextBox | TextBox |  | no validator found |  |  |  |  |
| fromComUnitCodeTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→fromComUnitCodeTextBox_TextChanged |
| toComUnitCodeTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→toComUnitCodeTextBox_TextChanged |
| fromComUnitNameTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| toComUnitNameTextBox | TextBox |  | no validator found |  |  |  |  |
| fromComUnitAddressTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| toComUnitAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| truckNoTextBox | TextBox |  | no validator found |  |  |  |  |
| driverNameTextBox | TextBox |  | no validator found |  |  |  |  |
| gridLineItemGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | SL | no validator found |  |  |  |  |
| (no id) | TemplateField | Product Code | no validator found |  |  |  |  |
| productCodeTextBox | TextBox |  | no validator found |  |  |  |  |
| fproductCodeTextBox | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Product Name | no validator found |  |  |  |  |
| productNameTextBox | TextBox |  | no validator found |  |  |  |  |
| productNameTextBox_AutoCompleteExtender | AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | Quantity | no validator found |  |  |  |  |
| quantityTextBox | TextBox |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Batch No | no validator found |  |  |  |  |
| batchNoTextBox | TextBox |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Add Item | no validator found |  |  |  |  |
| AddLButton | ImageButton |  | no validator found |  |  |  | onclick→AddLButton_Click |
| (no id) | TemplateField | Remove Item | no validator found |  |  |  |  |
| DeleteImageButton | ImageButton |  | no validator found |  |  |  | onclick→DeleteImageButton_Click |
| totalTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| grandTotalWordLabel | Label |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click1 |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| chalanDateTextBox | ChalanInfo.ChalanDate | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanInfo | @ChalanDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| chalanNoTextBox | ChalanInfo.ChalanNo | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| truckNoTextBox | ChalanInfo.TrackNo | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| driverNameTextBox | ChalanInfo.DriverName | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| fromComUnitCodeTextBox | ChalanInfo.FromComUnitCode | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| fromComUnitNameTextBox | ChalanInfo.FromComUnitName | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| fromComUnitAddressTextBox | ChalanInfo.FromComUnitAddress | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| toComUnitCodeTextBox | ChalanInfo.ToComUnitCode | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| toComUnitNameTextBox | ChalanInfo.ToComUnitName | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| toComUnitAddressTextBox | ChalanInfo.ToComUnitAddress | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | ChalanDetail.ProductCode | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| productNameTextBox | ChalanDetail.ProductName | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity | UNKNOWN | UNKNOWN | UNKNOWN |
| quantityTextBox | ChalanDetail.Quantity | SInventory_UI_StockTransferDcToDc.submitButton_Click1 | ChalanBLL.ChalanNoForShow, ChalanBLL.ChalanNoGenerator, ChalanBLL.SaveDataForChalanDetail, ChalanBLL.SaveDataForChalanIn | ChalanDAL.SaveDataForChalanDetail | @Quantity | UNKNOWN (SP body not in repo) | UNKNOWN |
| fromComUnitCodeTextBox | unitCode | SInventory_UI_StockTransferDcToDc.fromComUnitCodeTextBox_TextChanged | ChalanBLL.ComUnit | ChalanDAL.LoadComUnit | UNKNOWN | UNKNOWN | UNKNOWN |
| toComUnitCodeTextBox | unitCode | SInventory_UI_StockTransferDcToDc.toComUnitCodeTextBox_TextChanged | ChalanBLL.ComUnit | ChalanDAL.LoadComUnit | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | productCode | SInventory_UI_StockTransferDcToDc.productNameTextBox_TextChanged | DCStoreBLL.ProductInfo | DCStoreDAL.LoadProductCode, DCStoreDAL.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |

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
| SInventory_UI_StockTransferDcToDc.Page_Load | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_StockTransferDcToDc.Page_Load → ChalanBLL.ChalanNoForShow / ChalanBLL.ChalanNoGenerator | no | no |
| SInventory_UI_StockTransferDcToDc.productCodeTextBox_TextChanged | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:52 | object sender, EventArgs e | SInventory_UI_StockTransferDcToDc.productCodeTextBox_TextChanged → DCStoreBLL.ProductInfo → DCStoreDAL.LoadProductCode / DCStoreDAL.SingleParameter | no | no |
| SInventory_UI_StockTransferDcToDc.GetProduct | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:76 | int rowindex,string productCode | SInventory_UI_StockTransferDcToDc.GetProduct → DCStoreBLL.ProductInfo → DCStoreDAL.LoadProductCode / DCStoreDAL.SingleParameter | no | no |
| SInventory_UI_StockTransferDcToDc.submitButton_Click1 | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:93 | object sender, EventArgs e | SInventory_UI_StockTransferDcToDc.submitButton_Click1 → ChalanBLL.ChalanNoForShow / ChalanBLL.ChalanNoGenerator / ChalanBLL.SaveDataForChalanDetail / ChalanBLL.SaveDataForChalanInfo / ChalanBLL.UpdateDCStockQuantity → ChalanDAL.SaveDataForChalanDetail / Chalan | no | no |
| SInventory_UI_StockTransferDcToDc.fromComUnitCodeTextBox_TextChanged | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:174 | object sender, EventArgs e | SInventory_UI_StockTransferDcToDc.fromComUnitCodeTextBox_TextChanged → ChalanBLL.ComUnit → ChalanDAL.LoadComUnit | no | no |
| SInventory_UI_StockTransferDcToDc.toComUnitCodeTextBox_TextChanged | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:192 | object sender, EventArgs e | SInventory_UI_StockTransferDcToDc.toComUnitCodeTextBox_TextChanged → ChalanBLL.ComUnit → ChalanDAL.LoadComUnit | no | no |
| SInventory_UI_StockTransferDcToDc.AddLButton_Click | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:211 | object sender, ImageClickEventArgs e | SInventory_UI_StockTransferDcToDc.AddLButton_Click | no | no |
| SInventory_UI_StockTransferDcToDc.DeleteImageButton_Click | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:250 | object sender, ImageClickEventArgs e | SInventory_UI_StockTransferDcToDc.DeleteImageButton_Click | no | no |
| SInventory_UI_StockTransferDcToDc.productNameTextBox_TextChanged | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs:288 | object sender, EventArgs e | SInventory_UI_StockTransferDcToDc.productNameTextBox_TextChanged → DCStoreBLL.ProductInfo → DCStoreDAL.LoadProductCode / DCStoreDAL.SingleParameter | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ChalanBLL.ChalanNoForShow | Library.BLL/SInventory_BLL/ChalanBLL.cs:127 |
| ChalanBLL.ChalanNoGenerator | Library.BLL/SInventory_BLL/ChalanBLL.cs:134 |
| DCStoreBLL.ProductInfo | Library.BLL/SInventory_BLL/DCStoreBLL.cs:90 |
| ChalanBLL.SaveDataForChalanInfo | Library.BLL/SInventory_BLL/ChalanBLL.cs:19 |
| ChalanBLL.SaveDataForChalanDetail | Library.BLL/SInventory_BLL/ChalanBLL.cs:97 |
| ChalanBLL.UpdateDCStockQuantity | Library.BLL/SInventory_BLL/ChalanBLL.cs:59 |
| ChalanBLL.ComUnit | Library.BLL/SInventory_BLL/ChalanBLL.cs:163 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DCStoreDAL.LoadProductCode | Library.DAL/SInventory_DAL/DCStoreDAL.cs:80 | inline SQL: tblProduct, tblUnitPrice |  |
| DCStoreDAL.SingleParameter | Library.DAL/SInventory_DAL/DCStoreDAL.cs:151 | delegates / no SQL literal |  |
| ChalanDAL.SaveDataForChalanInfo | Library.DAL/SInventory_DAL/ChalanDAL.cs:18 | inline SQL: tblChalanInfo | @ChalanDate, @ChalanId, @ChalanNo, @DriverName, @FromComUnitAddress, @FromComUnitCode, @FromComUnitI |
| ChalanDAL.SaveDataForChalanDetail | Library.DAL/SInventory_DAL/ChalanDAL.cs:44 | inline SQL: tblChalanDetail | @BatchNo, @ChalanDetailsId, @ChalanId, @DCStoreId, @ProductCode, @ProductName, @Quantity, @UnitPrice |
| ChalanDAL.UpdateDCStockQuantity | Library.DAL/SInventory_DAL/ChalanDAL.cs:514 | inline SQL: tblDCStore, tblChalanDetail, tblChalanInfo, tblDCStore | @ChalanDetailsId, @DCStoreId, @IssueQty |
| ChalanDAL.LoadComUnit | Library.DAL/SInventory_DAL/ChalanDAL.cs:493 | inline SQL: tblCompanyUnit | @ComUnitCode |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblDCStore, tblProduct, tblUnitPrice
- Tables WRITTEN (inline SQL reachable): tblChalanDetail, tblChalanInfo, tblDCStore
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblChalanDetail | BatchNo | @BatchNo |
| tblChalanDetail | ChalanDetailsId | @ChalanDetailsId |
| tblChalanDetail | ChalanId | @ChalanId |
| tblChalanDetail | DCStoreId | @DCStoreId |
| tblChalanDetail | ProductCode | @ProductCode |
| tblChalanDetail | ProductName | @ProductName |
| tblChalanDetail | Quantity | @Quantity |
| tblChalanDetail | UnitPrice | @UnitPrice |
| tblChalanDetail | Value | @Value |
| tblChalanDetail | ValueWVat | @ValueWVat |
| tblChalanDetail | Vat | @Vat |
| tblChalanInfo | ChalanDate | @ChalanDate |
| tblChalanInfo | ChalanId | @ChalanId |
| tblChalanInfo | ChalanNo | @ChalanNo |
| tblChalanInfo | DriverName | @DriverName |
| tblChalanInfo | FromComUnitAddress | @FromComUnitAddress |
| tblChalanInfo | FromComUnitCode | @FromComUnitCode |
| tblChalanInfo | FromComUnitId | @FromComUnitId |
| tblChalanInfo | FromComUnitName | @FromComUnitName |
| tblChalanInfo | GrandTotal | @GrandTotal |
| tblChalanInfo | IsDeliver | @IsDeliver |
| tblChalanInfo | ManufacId | @ManufacId |
| tblChalanInfo | ToComUnitAddress | @ToComUnitAddress |
| tblChalanInfo | ToComUnitCode | @ToComUnitCode |
| tblChalanInfo | ToComUnitName | @ToComUnitName |
| tblChalanInfo | TotalValue | @TotalValue |
| tblChalanInfo | TotalVat | @TotalVat |
| tblChalanInfo | TrackNo | @TrackNo |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler fromComUnitCodeTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Company Unit Information Not Found!! | UNKNOWN |
| (handler toComUnitCodeTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Company Unit Information Not Found!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 14 methods.
- Redirect targets: none
- User messages: Company Unit Information Not Found!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ChalanBLL, DCStoreBLL; DAL: ChalanDAL, DCStoreDAL; DAO: ChalanDetail, ChalanInfo; Tables (inline): tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblDCStore, tblProduct, tblUnitPrice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockTransferDcToDc.aspx`, `Solution.Web/SInventory_UI/StockTransferDcToDc.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 32 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockTransferDcToDc.aspx |
| Data access | This document §15–17 | 0 SP names, 6 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ChalanDAL.LoadComUnit, ChalanDAL.SaveDataForChalanDetail, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateDCStockQuantity, DCStoreDAL.LoadProductCode affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
