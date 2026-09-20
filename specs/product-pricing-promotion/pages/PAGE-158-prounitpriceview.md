---
page_id: PAGE-158
module: M04
page: ProUnitPriceView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-158 — ProUnitPriceView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-158 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | ProUnitPriceView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ProUnitPriceView |
| aspx size | 10571 bytes |
| Code-behind LOC | 69 |
| Inline script size | 1400 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ProUnitPriceView** (List / View). Page title/heading text: `Unit Price List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ProUnitPriceEntry.aspx`

Outbound references: `prounitpriceedit.aspx`, `prounitpriceentry.aspx`

## 5. Route
`~/SInventory_UI/ProUnitPriceView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ProUnitPriceView.aspx |
| Code-behind | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | UnitPriceBLL (Library.BLL/SInventory_BLL/UnitPriceBLL.cs) |
| DAL class | ProductUnitPriceDAL (Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→EmpCetegoryAddImageButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | VAT Amount | no validator found |  |  |  |  |
| (no id) | BoundField | MRP Price | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |
| unitPriceInfoNewImageButton | ImageButton |  | no validator found |  |  |  | onclick→unitPriceNewImageButton_Click |
| unitPriceInfoReloadImageButton | ImageButton |  | no validator found |  |  |  | onclick→unitPriceReloadImageButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | SInventory_UI_ProUnitPriceView.EmpCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:66 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_ProUnitPriceView.Page_Load | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_ProUnitPriceView.Page_Load → UnitPriceBLL.LoadProductUnitPrice → ProductUnitPriceDAL.LoadUnitPriceView | no | no |
| SInventory_UI_ProUnitPriceView.UnitPriceLoad | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:23 |  | SInventory_UI_ProUnitPriceView.UnitPriceLoad → UnitPriceBLL.LoadProductUnitPrice → ProductUnitPriceDAL.LoadUnitPriceView | no | no |
| SInventory_UI_ProUnitPriceView.loadGridView_RowCommand | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:36 | object sender, GridViewCommandEventArgs e | SInventory_UI_ProUnitPriceView.loadGridView_RowCommand | no | no |
| SInventory_UI_ProUnitPriceView.unitPriceNewImageButton_Click | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:47 | object sender, ImageClickEventArgs e | SInventory_UI_ProUnitPriceView.unitPriceNewImageButton_Click | no | no |
| SInventory_UI_ProUnitPriceView.unitPriceReloadImageButton_Click | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:51 | object sender, ImageClickEventArgs e | SInventory_UI_ProUnitPriceView.unitPriceReloadImageButton_Click → UnitPriceBLL.LoadProductUnitPrice → ProductUnitPriceDAL.LoadUnitPriceView | no | no |
| SInventory_UI_ProUnitPriceView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:55 | object sender, EventArgs e | SInventory_UI_ProUnitPriceView.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_ProUnitPriceView.EmpCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs:66 | object sender, EventArgs e | SInventory_UI_ProUnitPriceView.EmpCetegoryAddImageButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| UnitPriceBLL.LoadProductUnitPrice | Library.BLL/SInventory_BLL/UnitPriceBLL.cs:115 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ProductUnitPriceDAL.LoadUnitPriceView | Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs:70 | inline SQL: tblManufacturer, tblPackSize, tblProCategory, tblProType |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblManufacturer, tblPackSize, tblProCategory, tblProType, tblProduct, tblProductCase, tblProductSQ, tblStockUOM, tblUnitPrice
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UnitPrice
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 8 methods.
- Redirect targets: ProUnitPriceEntry.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: UnitPriceBLL; DAL: ProductUnitPriceDAL; DAO: —; Tables (inline): tblManufacturer, tblPackSize, tblProCategory, tblProType, tblProduct, tblProductCase, tblProductSQ, tblStockUOM, tblUnitPrice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ProUnitPriceView.aspx`, `Solution.Web/SInventory_UI/ProUnitPriceView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ProUnitPriceView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 16 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ProUnitPriceView.aspx |
| Data access | This document §15–17 | 0 SP names, 9 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ProductUnitPriceDAL.LoadUnitPriceView affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
