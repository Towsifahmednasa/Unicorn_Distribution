---
page_id: PAGE-625
module: M15
page: OrganogramReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-625 — OrganogramReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-625 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | OrganogramReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Reports_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Reports_UI_OrganogramReport |
| aspx size | 7993 bytes |
| Code-behind LOC | 125 |
| Inline script size | 2856 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrganogramReport** (Report / Dashboard). Page title/heading text: `Organogram Report`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `productedit.aspx`, `productentry.aspx`

## 5. Route
`~/Reports_UI/OrganogramReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Reports_UI/OrganogramReport.aspx |
| Code-behind | Solution.Web/Reports_UI/OrganogramReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/Reports_UI/IVMarketStructureMarket.ascx |
| DAL class | OrderTrackingDAL (Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→EmpCetegoryAddImageButton_Click |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Group | no validator found |  |  |  |  |
| (no id) | BoundField | NSM Emp. | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | DSM Emp. | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | AM Emp. | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Emp. | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Count | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Count | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Route | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | Reports_UI_OrganogramReport.EmpCetegoryAddImageButton_Click | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:111 | — | — | — | read-only / none detected |
| submitButton | LinkButton click | (postback) | Reports_UI_OrganogramReport.SearchButton_Click | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:32 | — | OrderTrackingDAL.GetOrganogramreportList | sp_GetOrganogramreportList | SP executed — effect on tables UNKNOWN (body not in repo) |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Reports_UI_OrganogramReport.Page_Load | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:18 | object sender, EventArgs e | Reports_UI_OrganogramReport.Page_Load | no | no |
| Reports_UI_OrganogramReport.SearchButton_Click | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:32 | object sender, EventArgs e | Reports_UI_OrganogramReport.SearchButton_Click → OrderTrackingDAL.GetOrganogramreportList → SP: sp_GetOrganogramreportList | no | no |
| Reports_UI_OrganogramReport.Parm_2 | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:38 |  | Reports_UI_OrganogramReport.Parm_2 | no | no |
| Reports_UI_OrganogramReport.ProductLoad | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:81 | string parm | Reports_UI_OrganogramReport.ProductLoad → OrderTrackingDAL.GetOrganogramreportList → SP: sp_GetOrganogramreportList | no | no |
| Reports_UI_OrganogramReport.loadGridView_RowCommand | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:94 | object sender, GridViewCommandEventArgs e | Reports_UI_OrganogramReport.loadGridView_RowCommand | no | no |
| Reports_UI_OrganogramReport.miaTargetNewImageButton_Click | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:106 | object sender, ImageClickEventArgs e | Reports_UI_OrganogramReport.miaTargetNewImageButton_Click | no | no |
| Reports_UI_OrganogramReport.EmpCetegoryAddImageButton_Click | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:111 | object sender, EventArgs e | Reports_UI_OrganogramReport.EmpCetegoryAddImageButton_Click | no | no |
| Reports_UI_OrganogramReport.gv_DocumentUpload_PreRender | Solution.Web/Reports_UI/OrganogramReport.aspx.cs:115 | object sender, EventArgs e | Reports_UI_OrganogramReport.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderTrackingDAL.GetOrganogramreportList | Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:343 | sp_GetOrganogramreportList | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GetOrganogramreportList | OrderTrackingDAL.GetOrganogramreportList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
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
- Redirect targets: ProductEntry.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: OrderTrackingDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/Reports_UI/OrganogramReport.aspx`, `Solution.Web/Reports_UI/OrganogramReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Reports_UI/OrganogramReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 21 controls parsed | n/a | PARTIAL | Solution.Web/Reports_UI/OrganogramReport.aspx |
| Data access | This document §15–17 | 1 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderTrackingDAL.GetOrganogramreportList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
