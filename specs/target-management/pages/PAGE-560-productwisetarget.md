---
page_id: PAGE-560
module: M14
page: ProductWiseTarget
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-560 — ProductWiseTarget

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-560 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | ProductWiseTarget |
| Kind | Page (INFERRED from name) |
| Web folder | Target_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Target_UI_ProductWiseTarget |
| aspx size | 22467 bytes |
| Code-behind LOC | 67 |
| Inline script size | 9679 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ProductWiseTarget** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `Target_UI/ProductWiseTargetList.aspx`

Outbound references: `productwisetargetlist.aspx`

## 5. Route
`~/Target_UI/ProductWiseTarget.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Target_UI/ProductWiseTarget.aspx |
| Code-behind | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | ../assets/year-select.js |
| DAL class | ProuctWiseSalesDAL (Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| ddlGroup | select |  | no validator found |  |  |  |  |
| zoneSelect | select |  | no validator found |  |  |  |  |
| areaSelect | select |  | no validator found |  |  |  |  |
| territorySelect | select |  | no validator found |  |  |  |  |
| product | select |  | no validator found |  |  |  |  |
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
| ddlGroup | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| zoneSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| areaSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| territorySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| product | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| month | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| year | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: ConfirmationClick, FinalSave, GetArea_All_ByZone, GetArea_ByZone, GetData, GetGroupAllInfo, GetGroupInfo, GetTerritory_ByAreaId, GetTerritory_ByAreaId_All, GetZone_All, GetZone_ByGroup, LoadProduct, Month, RemoveValidationTooltip, Save, Validation, ValidationTooltip, getUrlVars
- jQuery/DOM bindings: #areaSelect:change, #customSwitch1:change, #ddlGroup:change, #zoneSelect:change
- AJAX endpoints (`url:`): ProductWiseTarget.aspx/LoadMonthlyTargetById, ProductWiseTarget.aspx/SaveTarget
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Target_UI_ProductWiseTarget.Page_Load | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:17 | object sender, EventArgs e | Target_UI_ProductWiseTarget.Page_Load | no | no |
| Target_UI_ProductWiseTarget.SaveTarget | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:23 | ProductWiseTargetDAO aTargetDao | Target_UI_ProductWiseTarget.SaveTarget → ProuctWiseSalesDAL.SaveProductWiseSales / ProuctWiseSalesDAL.UpdateMonthlyTarget | no | no |
| Target_UI_ProductWiseTarget.LoadMonthlyTargetById | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:47 | string id | Target_UI_ProductWiseTarget.LoadMonthlyTargetById → ProuctWiseSalesDAL.LoadMonthlyTargetById | no | no |
| Target_UI_ProductWiseTarget.LoadProduct | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:58 |  | Target_UI_ProductWiseTarget.LoadProduct → ProuctWiseSalesDAL.LoadProduct | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| Target_UI_ProductWiseTarget.SaveTarget | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:23 | ProductWiseTargetDAO aTargetDao | ~/Target_UI/ProductWiseTarget.aspx/SaveTarget | via BLL |
| Target_UI_ProductWiseTarget.LoadMonthlyTargetById | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:47 | string id | ~/Target_UI/ProductWiseTarget.aspx/LoadMonthlyTargetById | via BLL |
| Target_UI_ProductWiseTarget.LoadProduct | Solution.Web/Target_UI/ProductWiseTarget.aspx.cs:58 |  | ~/Target_UI/ProductWiseTarget.aspx/LoadProduct | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ProuctWiseSalesDAL.UpdateMonthlyTarget | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:49 | inline SQL: tblProductWiseSalesTarget |  |
| ProuctWiseSalesDAL.SaveProductWiseSales | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:16 | inline SQL: tblProductWiseSalesTarget |  |
| ProuctWiseSalesDAL.LoadMonthlyTargetById | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:110 | inline SQL: tblProductWiseSalesTarget |  |
| ProuctWiseSalesDAL.LoadProduct | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:115 | inline SQL: tblProduct |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblProduct, tblProductWiseSalesTarget
- Tables WRITTEN (inline SQL reachable): tblProductWiseSalesTarget
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
- try/catch present in 0 of 4 methods.
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
BLL: —; DAL: ProuctWiseSalesDAL; DAO: —; Tables (inline): tblProduct, tblProductWiseSalesTarget

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/Target_UI/ProductWiseTarget.aspx`, `Solution.Web/Target_UI/ProductWiseTarget.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Target_UI/ProductWiseTarget.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 15 controls parsed | n/a | PARTIAL | Solution.Web/Target_UI/ProductWiseTarget.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M14 (Target & DWSP Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ProuctWiseSalesDAL.LoadMonthlyTargetById, ProuctWiseSalesDAL.LoadProduct, ProuctWiseSalesDAL.SaveProductWiseSales, ProuctWiseSalesDAL.UpdateMonthlyTarget affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
