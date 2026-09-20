---
page_id: PAGE-558
module: M14
page: MonthlyTarget
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-558 — MonthlyTarget

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-558 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | MonthlyTarget |
| Kind | Page (INFERRED from name) |
| Web folder | Target_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Target_UI_MonthlyTarget |
| aspx size | 13367 bytes |
| Code-behind LOC | 64 |
| Inline script size | 6454 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MonthlyTarget** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `Target_UI/MonthlyTargetView.aspx`

Outbound references: `monthlytargetview.aspx`

## 5. Route
`~/Target_UI/MonthlyTarget.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Target_UI/MonthlyTarget.aspx |
| Code-behind | Solution.Web/Target_UI/MonthlyTarget.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | ../assets/year-select.js |
| DAL class | TargetDAL (Library.DAL/TargetDAL/TargetDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| month | select |  | no validator found |  |  |  |  |
| year | select |  | no validator found |  |  |  |  |
| amount | input |  | no validator found |  |  |  |  |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | TextBox |  | no validator found |  |  |  |  |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| month | SaveTarget → TargetDAL.CheckLoadMonthlyTarget | inline SQL | UNKNOWN | tblMonthlyTarget |
| year | SaveTarget → TargetDAL.CheckLoadMonthlyTarget | inline SQL | UNKNOWN | tblMonthlyTarget |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: ConfirmationClick, FinalSave, GetData, Month, RemoveValidationTooltip, Save, Validation, ValidationTooltip, getUrlVars
- jQuery/DOM bindings: #customSwitch1:change
- AJAX endpoints (`url:`): MonthlyTarget.aspx/LoadMonthlyTargetById, MonthlyTarget.aspx/SaveTarget
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Target_UI_MonthlyTarget.Page_Load | Solution.Web/Target_UI/MonthlyTarget.aspx.cs:19 | object sender, EventArgs e | Target_UI_MonthlyTarget.Page_Load | no | no |
| Target_UI_MonthlyTarget.SaveTarget | Solution.Web/Target_UI/MonthlyTarget.aspx.cs:24 | TargetDAO aTargetDao | Target_UI_MonthlyTarget.SaveTarget → TargetDAL.CheckLoadMonthlyTarget / TargetDAL.SaveMonthlyTarget / TargetDAL.UpdateMonthlyTarget | no | no |
| Target_UI_MonthlyTarget.LoadMonthlyTargetById | Solution.Web/Target_UI/MonthlyTarget.aspx.cs:54 | string id | Target_UI_MonthlyTarget.LoadMonthlyTargetById → TargetDAL.LoadMonthlyTargetById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| Target_UI_MonthlyTarget.SaveTarget | Solution.Web/Target_UI/MonthlyTarget.aspx.cs:24 | TargetDAO aTargetDao | ~/Target_UI/MonthlyTarget.aspx/SaveTarget | via BLL |
| Target_UI_MonthlyTarget.LoadMonthlyTargetById | Solution.Web/Target_UI/MonthlyTarget.aspx.cs:54 | string id | ~/Target_UI/MonthlyTarget.aspx/LoadMonthlyTargetById | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| TargetDAL.CheckLoadMonthlyTarget | Library.DAL/TargetDAL/TargetDAL.cs:58 | inline SQL: tblMonthlyTarget |  |
| TargetDAL.UpdateMonthlyTarget | Library.DAL/TargetDAL/TargetDAL.cs:40 | inline SQL: tblMonthlyTarget |  |
| TargetDAL.SaveMonthlyTarget | Library.DAL/TargetDAL/TargetDAL.cs:17 | inline SQL: tblMonthlyTarget, tblMonthlyTarget |  |
| TargetDAL.LoadMonthlyTargetById | Library.DAL/TargetDAL/TargetDAL.cs:64 | inline SQL: tblMonthlyTarget |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblMonthlyTarget
- Tables WRITTEN (inline SQL reachable): tblMonthlyTarget
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 3 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: TargetDAL; DAO: —; Tables (inline): tblMonthlyTarget

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/Target_UI/MonthlyTarget.aspx`, `Solution.Web/Target_UI/MonthlyTarget.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Target_UI/MonthlyTarget.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 10 controls parsed | n/a | PARTIAL | Solution.Web/Target_UI/MonthlyTarget.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M14 (Target & DWSP Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods TargetDAL.CheckLoadMonthlyTarget, TargetDAL.LoadMonthlyTargetById, TargetDAL.SaveMonthlyTarget, TargetDAL.UpdateMonthlyTarget affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
