---
page_id: PAGE-388
module: M10
page: WarehouseStockInView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-388 — WarehouseStockInView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-388 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WarehouseStockInView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WarehouseStockInView |
| aspx size | 4727 bytes |
| Code-behind LOC | 106 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **WarehouseStockInView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/WarehouseStockIn.aspx`, `SInventory_UI/WarehouseStockOut.aspx`

Outbound references: `warehousestockin.aspx`, `warehousestockinedit.aspx`, `whstockinreportviewer.aspx`

## 5. Route
`~/SInventory_UI/WarehouseStockInView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WarehouseStockInView.aspx |
| Code-behind | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | WarehouseStockInBll (Library.BLL/SInventory_BLL/WarehouseStockInBll.cs) |
| DAL class | WarehouseStockInDal (Library.DAL/SInventory_DAL/WarehouseStockInDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→addNewLinkButton_OnClick |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Code | no validator found |  |  |  |  |
| (no id) | BoundField | StockIn Date | no validator found |  |  |  |  |
| (no id) | BoundField | Challan No | no validator found |  |  |  |  |
| (no id) | BoundField | Challan Date | no validator found |  |  |  |  |
| (no id) | BoundField | Total Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| printButton | ImageButton |  | no validator found |  |  |  | OnClick→printButton_Click |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| deleteImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?'); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | SInventory_UI_WarehouseStockInView.addNewLinkButton_OnClick | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:98 | — | — | — | read-only / none detected |
| printButton | ImageButton click | (postback) | SInventory_UI_WarehouseStockInView.printButton_Click | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:57 | — | — | — | read-only / none detected |
| deleteImageButton | ImageButton | return confirm('Are you sure you want to Delete ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |

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
| SInventory_UI_WarehouseStockInView.Page_Load | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_WarehouseStockInView.Page_Load → WarehouseStockInBll.LoadWarehouseStockInData → WarehouseStockInDal.GetWarehouseStockInData | no | no |
| SInventory_UI_WarehouseStockInView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:22 | object sender, EventArgs e | SInventory_UI_WarehouseStockInView.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_WarehouseStockInView.LoadGridView | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:34 |  | SInventory_UI_WarehouseStockInView.LoadGridView → WarehouseStockInBll.LoadWarehouseStockInData → WarehouseStockInDal.GetWarehouseStockInData | no | no |
| SInventory_UI_WarehouseStockInView.printButton_Click | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:57 | object sender, EventArgs e | SInventory_UI_WarehouseStockInView.printButton_Click | no | no |
| SInventory_UI_WarehouseStockInView.loadGridView_RowCommand | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:71 | object sender, GridViewCommandEventArgs e | SInventory_UI_WarehouseStockInView.loadGridView_RowCommand → WarehouseStockInBll.DeleteWhStockInInfoById / WarehouseStockInBll.LoadWarehouseStockInData → WarehouseStockInDal.DeleteWhStockInInfoById / WarehouseStockInDal.GetWarehouseStockInData | no | no |
| SInventory_UI_WarehouseStockInView.addNewLinkButton_OnClick | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:98 | object sender, EventArgs e | SInventory_UI_WarehouseStockInView.addNewLinkButton_OnClick | no | no |
| SInventory_UI_WarehouseStockInView.reloadLinkButton_OnClick | Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs:103 | object sender, EventArgs e | SInventory_UI_WarehouseStockInView.reloadLinkButton_OnClick → WarehouseStockInBll.LoadWarehouseStockInData → WarehouseStockInDal.GetWarehouseStockInData | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WarehouseStockInBll.LoadWarehouseStockInData | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:117 |
| WarehouseStockInBll.DeleteWhStockInInfoById | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:157 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WarehouseStockInDal.GetWarehouseStockInData | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:192 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.DeleteWhStockInInfoById | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:287 | inline SQL: tblWHStockInDetail, tblWHStockInMaster, tblWHStockInDetail, tblWHStockInMaster |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblWHStockInDetail, tblWHStockInMaster
- Tables WRITTEN (inline SQL reachable): tblWHStockInDetail, tblWHStockInMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: IDInformation, ReportType
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: WarehouseStockIn.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: WarehouseStockInBll; DAL: WarehouseStockInDal; DAO: —; Tables (inline): tblWHStockInDetail, tblWHStockInMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tblWHStockInDetail, tblWHStockInMaster |
| TC-05 | Click printButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click printButton with valid data | Handler runs; rows written to tblWHStockInDetail, tblWHStockInMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WarehouseStockInView.aspx`, `Solution.Web/SInventory_UI/WarehouseStockInView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/WarehouseStockInView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 18 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/WarehouseStockInView.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods WarehouseStockInDal.DeleteWhStockInInfoById, WarehouseStockInDal.GetWarehouseStockInData affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
