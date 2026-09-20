---
page_id: PAGE-378
module: M10
page: SampleTypeConventionView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-378 — SampleTypeConventionView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-378 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | SampleTypeConventionView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SampleTypeConventionView |
| aspx size | 5660 bytes |
| Code-behind LOC | 126 |
| Inline script size | 657 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SampleTypeConventionView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/SampleTypeConvertion.aspx`

Outbound references: `custmasterentry.aspx`, `customermasteredit.aspx`, `customermasterviewer.aspx`, `dcstockoutreportviewer.aspx`, `sampletypeconvertion.aspx`

## 5. Route
`~/SInventory_UI/SampleTypeConventionView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx |
| Code-behind | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | SampleTypeConventionBLL (Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs) |
| DAL class | SampleTypeConventionDal (Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs) |
| DAO/entity class | SampleStockForDcDetails (Library.DAO/SInventory_Entities/SampleStockForDcDetails.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, onrowcommand→loadGridView_RowCommand |
| (no id) | BoundField | Convertion Name | no validator found |  |  |  |  |
| (no id) | BoundField | Convertion Stock Date | no validator found |  |  |  |  |
| (no id) | BoundField | Action | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return GetConfirmation(); |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| reportImageButton | ImageButton |  | no validator found |  |  |  |  |
| GridView1 | GridView |  | no validator found |  | Visible=False |  |  |
| (no id) | BoundField |  | no validator found |  |  |  |  |
| (no id) | BoundField |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| editImageButton | ImageButton | return GetConfirmation(); | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: GetConfirmation
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_SampleTypeConventionView.Page_Load | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:19 | object sender, EventArgs e | SInventory_UI_SampleTypeConventionView.Page_Load → SampleTypeConventionBLL.DcStockOutBll → SampleTypeConventionDal.DcStockOutViewDal | no | no |
| SInventory_UI_SampleTypeConventionView.DcStockOutgrid | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:34 |  | SInventory_UI_SampleTypeConventionView.DcStockOutgrid → SampleTypeConventionBLL.DcStockOutBll → SampleTypeConventionDal.DcStockOutViewDal | no | no |
| SInventory_UI_SampleTypeConventionView.loadGridView_RowCommand | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:40 | object sender, GridViewCommandEventArgs e | SInventory_UI_SampleTypeConventionView.loadGridView_RowCommand → SampleTypeConventionBLL.DcStockOutBll / SampleTypeConventionBLL.DcStockOutDetailsDelete / SampleTypeConventionBLL.DcStockOutMasterDelete / SampleTypeConventionBLL.LoadSampleStock / SampleTypeConv | no | no |
| SInventory_UI_SampleTypeConventionView.CustMasterNewImageButton_Click | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:96 | object sender, ImageClickEventArgs e | SInventory_UI_SampleTypeConventionView.CustMasterNewImageButton_Click | no | no |
| SInventory_UI_SampleTypeConventionView.DcStockOutAddImageButton_Click | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:100 | object sender, ImageClickEventArgs e | SInventory_UI_SampleTypeConventionView.DcStockOutAddImageButton_Click | no | no |
| SInventory_UI_SampleTypeConventionView.rptImageButton_Click | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:104 | object sender, ImageClickEventArgs e | SInventory_UI_SampleTypeConventionView.rptImageButton_Click | no | no |
| SInventory_UI_SampleTypeConventionView.Button1_Click | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:111 | object sender, EventArgs e | SInventory_UI_SampleTypeConventionView.Button1_Click → SampleTypeConventionBLL.DcStockOutBll → SampleTypeConventionDal.DcStockOutViewDal | no | no |
| SInventory_UI_SampleTypeConventionView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs:116 | object sender, EventArgs e | SInventory_UI_SampleTypeConventionView.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SampleTypeConventionBLL.DcStockOutBll | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:111 |
| SampleTypeConventionBLL.LoadSampleStock | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:126 |
| SampleTypeConventionBLL.StockIn | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:102 |
| SampleTypeConventionBLL.DcStockOutDetailsDelete | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:116 |
| SampleTypeConventionBLL.DcStockOutMasterDelete | Library.BLL/SInventory_BLL/SampleTypeConventionBLL.cs:121 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SampleTypeConventionDal.DcStockOutViewDal | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:87 | inline SQL: tblCompanyUnit, tblSampleStockForDcMaster |  |
| SampleTypeConventionDal.LoadSampleStock | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:139 | inline SQL: tblSampleStockForDcDetails | @SampleStockForDcMasterId |
| SampleTypeConventionDal.StockInCentral | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:119 | inline SQL: tblDCStore | @DCStoreId, @SampleStock |
| SampleTypeConventionDal.DcStockOutDetailsDeleteDal | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:108 | inline SQL: tblSampleStockForDcDetails, tblSampleStockForDcDetails | @SampleStockForDcMasterId |
| SampleTypeConventionDal.DcStockOutMasterDeleteDal | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:97 | inline SQL: tblSampleStockForDcMaster, tblSampleStockForDcMaster | @SampleStockForDcMasterId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblSampleStockForDcDetails, tblSampleStockForDcMaster
- Tables WRITTEN (inline SQL reachable): tblDCStore, tblSampleStockForDcDetails, tblSampleStockForDcMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DcStockOutMasterId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
- Redirect targets: CustMasterEntry.aspx, SampleTypeConvertion.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: SampleTypeConventionBLL; DAL: SampleTypeConventionDal; DAO: SampleStockForDcDetails; Tables (inline): tblCompanyUnit, tblDCStore, tblSampleStockForDcDetails, tblSampleStockForDcMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SampleTypeConventionView.aspx`, `Solution.Web/SInventory_UI/SampleTypeConventionView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 17 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SampleTypeConventionView.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods SampleTypeConventionDal.DcStockOutDetailsDeleteDal, SampleTypeConventionDal.DcStockOutMasterDeleteDal, SampleTypeConventionDal.DcStockOutViewDal, SampleTypeConventionDal.LoadSampleStock, SampleTypeConventionDal.StockInCentral affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
