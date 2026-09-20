---
page_id: PAGE-030
module: M03
page: AMSetupEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "UNKNOWN", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-030 — AMSetupEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-030 |
| Module | M03 — Organisation & Market Structure Master (INFERRED from folder + file name) |
| Page name | AMSetupEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_AMSetupEdit |
| aspx size | 19152 bytes |
| Code-behind LOC | 14 |
| Inline script size | 9161 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **AMSetupEdit** (Edit form). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Company/branch/unit, geography (division/district/thana), sales hierarchy (national/zone/region/area/territory/market), routes, depot-territory mapping and structure transfers.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `amrecords.aspx`

## 5. Route
`~/DoctorModule_UI/AMSetupEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/AMSetupEdit.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/AMSetupEdit.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| btnBTL | a |  | no validator found |  |  |  |  |
| ddlGroup | select |  | no validator found |  |  |  |  |
| zoneSelect | select |  | no validator found |  |  |  |  |
| areaSelect | select |  | no validator found |  |  |  |  |
| isVacent | input |  | no validator found |  |  |  | onchange→IsVacantChange() |
| ddlEmployee | select |  | no validator found |  |  |  |  |
| customSwitch1 | input |  | no validator found |  |  |  | onchange→IsActiveChange() |
| acDate | input | Select Date | no validator found |  |  |  |  |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | input | 0 | no validator found |  |  |  |  |
| hfEMPMID | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlGroup | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| zoneSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| areaSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlEmployee | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: FinalSave, GetAMEmployee_All, GetAMMAsterData, GetArea_All_ByZone, GetArea_ByZone, GetData, GetEmployee, GetGroup, GetGroupAllInfo, GetGroupInfo, GetZone_All, GetZone_ByGroup, IsVacantChange, RemoveValidationTooltip, ResetLink, Save, Validation, ValidationTooltip, getUrlVars
- jQuery/DOM bindings: #ddlGroup:change, #zoneSelect:change
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_AMSetupEdit.Page_Load | Solution.Web/DoctorModule_UI/AMSetupEdit.aspx.cs:11 | object sender, EventArgs e | DoctorModule_UI_AMSetupEdit.Page_Load | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
No DAL method reached.

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

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
- try/catch present in 0 of 1 methods.
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
BLL: —; DAL: —; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/AMSetupEdit.aspx`, `Solution.Web/DoctorModule_UI/AMSetupEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/AMSetupEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 14 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/AMSetupEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 0 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M03 (Organisation & Market Structure Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods — affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
