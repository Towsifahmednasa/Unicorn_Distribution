---
page_id: PAGE-393
module: M10
page: WHStockAdjustmentView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-393 — WHStockAdjustmentView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-393 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WHStockAdjustmentView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WHStockAdjustmentView |
| aspx size | 12503 bytes |
| Code-behind LOC | 127 |
| Inline script size | 2270 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **WHStockAdjustmentView** (List / View). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/WHStockAdjustmentEntry.aspx`

Outbound references: `whstockadjustmententry.aspx`, `whstockadjustmentlistreportviewer.aspx`

## 5. Route
`~/SInventory_UI/WHStockAdjustmentView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx |
| Code-behind | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | WHStockAdjDAL (Library.DAL/SInventory_DAL/WHStockAdjDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Transaction No | no validator found |  |  |  |  |
| (no id) | BoundField | Transaction Date | no validator found |  |  |  |  |
| (no id) | BoundField | Adjustment Type | no validator found |  |  |  |  |
| (no id) | BoundField | StockEffect | no validator found |  |  |  |  |
| (no id) | BoundField | From Store | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| LinkButton12 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | parameter | SInventory_UI_WHStockAdjustmentView.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | parameter | SInventory_UI_WHStockAdjustmentView.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_WHStockAdjustmentView.viewRptButton_Click | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:107 | — | WHStockAdjDAL.LoadWHStockAdjustment | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_WHStockAdjustmentView.Unnamed_Click | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:112 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: exportTableToExcel, exportToExcel, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_WHStockAdjustmentView.Page_Load | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_WHStockAdjustmentView.Page_Load → WHStockAdjDAL.LoadWHStockAdjustment | no | no |
| SInventory_UI_WHStockAdjustmentView.viewLinkButton_OnClick | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:21 | object sender, EventArgs e | SInventory_UI_WHStockAdjustmentView.viewLinkButton_OnClick | no | no |
| SInventory_UI_WHStockAdjustmentView.LoadGrid | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:32 |  | SInventory_UI_WHStockAdjustmentView.LoadGrid → WHStockAdjDAL.LoadWHStockAdjustment | no | no |
| SInventory_UI_WHStockAdjustmentView.loadGridView_RowCommand | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:60 | object sender, GridViewCommandEventArgs e | SInventory_UI_WHStockAdjustmentView.loadGridView_RowCommand → WHStockAdjDAL.DeleteStockWHAdjustment / WHStockAdjDAL.LoadWHStockAdjustment / WHStockAdjDAL.LoadWHStockAdjustmentById / WHStockAdjDAL.UpdateCentralStoreStock | no | no |
| SInventory_UI_WHStockAdjustmentView.viewRptButton_Click | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:107 | object sender, EventArgs e | SInventory_UI_WHStockAdjustmentView.viewRptButton_Click → WHStockAdjDAL.LoadWHStockAdjustment | no | no |
| SInventory_UI_WHStockAdjustmentView.Unnamed_Click | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:112 | object sender, EventArgs e | SInventory_UI_WHStockAdjustmentView.Unnamed_Click | no | no |
| SInventory_UI_WHStockAdjustmentView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs:117 | object sender, EventArgs e | SInventory_UI_WHStockAdjustmentView.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WHStockAdjDAL.LoadWHStockAdjustment | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:17 | inline SQL: tblAdjustmentType, tblWHStockAdjustment, tblWearhouse |  |
| WHStockAdjDAL.LoadWHStockAdjustmentById | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:141 | inline SQL: tblAdjustmentType, tblCentralStore, tblWHAdjustmentDetail, tblWHStockAdjustment |  |
| WHStockAdjDAL.UpdateCentralStoreStock | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:175 | inline SQL: tblCentralStore |  |
| WHStockAdjDAL.DeleteStockWHAdjustment | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:180 | inline SQL: tblWHAdjustmentDetail, tblWHStockAdjustment, tblWHAdjustmentDetail, tblWHStockAdjustment |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblAdjustmentType, tblCentralStore, tblWHAdjustmentDetail, tblWHStockAdjustment, tblWearhouse
- Tables WRITTEN (inline SQL reachable): tblCentralStore, tblWHAdjustmentDetail, tblWHStockAdjustment
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: WHStockAdjustmentEntry.aspx, WHStockAdjustmentView.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: WHStockAdjDAL; DAO: —; Tables (inline): tblAdjustmentType, tblCentralStore, tblWHAdjustmentDetail, tblWHStockAdjustment, tblWearhouse

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tblCentralStore, tblWHAdjustmentDetail, tblWHStockAdjustment |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tblCentralStore, tblWHAdjustmentDetail, tblWHStockAdjustment |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx`, `Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 24 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/WHStockAdjustmentView.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods WHStockAdjDAL.DeleteStockWHAdjustment, WHStockAdjDAL.LoadWHStockAdjustment, WHStockAdjDAL.LoadWHStockAdjustmentById, WHStockAdjDAL.UpdateCentralStoreStock affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
