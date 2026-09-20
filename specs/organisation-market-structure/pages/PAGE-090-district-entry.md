---
page_id: PAGE-090
module: M03
page: District_Entry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-090 — District_Entry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-090 |
| Module | M03 — Organisation & Market Structure Master (INFERRED from folder + file name) |
| Page name | District_Entry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | Thana_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Thana_UI_District_Entry |
| aspx size | 11187 bytes |
| Code-behind LOC | 56 |
| Inline script size | 5698 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **District_Entry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Company/branch/unit, geography (division/district/thana), sales hierarchy (national/zone/region/area/territory/market), routes, depot-territory mapping and structure transfers.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `Thana_UI/District_View.aspx`

Outbound references: `district_view.aspx`

## 5. Route
`~/Thana_UI/District_Entry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Thana_UI/District_Entry.aspx |
| Code-behind | Solution.Web/Thana_UI/District_Entry.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | ThanaDal (Library.DAL/Thana_DAL/ThanaDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| DivisionDDL | select |  | no validator found |  |  |  |  |
| DistrictDDL | select |  | no validator found |  |  |  |  |
| ThanaName | input | Enter District | no validator found |  |  |  |  |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| DivisionDDL | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| DistrictDDL | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: ConfirmationClick, FinalSave, GetDisivionAll, GetDistrictByDisivion, GetThanaEdit, IsActiveChange, RemoveValidationTooltip, ResetLink, Save, Validation, ValidationTooltip, getUrlVars
- jQuery/DOM bindings: #DivisionDDL:change
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Thana_UI_District_Entry.Page_Load | Solution.Web/Thana_UI/District_Entry.aspx.cs:18 | object sender, EventArgs e | Thana_UI_District_Entry.Page_Load | no | no |
| Thana_UI_District_Entry.Save_ThanaInfo | Solution.Web/Thana_UI/District_Entry.aspx.cs:23 | ThanaDao thanaDao | Thana_UI_District_Entry.Save_ThanaInfo → ThanaDal.SaveThanaInfo → SP: sp_Save_ThanaInfo, sp_UD_ThanaInfo, sp_check_ThanaInfo | no | yes |
| Thana_UI_District_Entry.Get_Division_All_DDL | Solution.Web/Thana_UI/District_Entry.aspx.cs:32 |  | Thana_UI_District_Entry.Get_Division_All_DDL → ThanaDal.GetDivision_All_DDL → SP: sp_Get_Division_Active_DDL | no | no |
| Thana_UI_District_Entry.Get_District_All_DDL | Solution.Web/Thana_UI/District_Entry.aspx.cs:41 | string id | Thana_UI_District_Entry.Get_District_All_DDL → ThanaDal.GetDistrict_All_DDL → SP: sp_Get_District_Active_DDL | no | no |
| Thana_UI_District_Entry.Get_Thana_By_Id | Solution.Web/Thana_UI/District_Entry.aspx.cs:50 | string id | Thana_UI_District_Entry.Get_Thana_By_Id → ThanaDal.Get_Thana_By_Id_Dal → SP: sp_GET_ThanaInfo_ById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| Thana_UI_District_Entry.Save_ThanaInfo | Solution.Web/Thana_UI/District_Entry.aspx.cs:23 | ThanaDao thanaDao | ~/Thana_UI/District_Entry.aspx/Save_ThanaInfo | via BLL |
| Thana_UI_District_Entry.Get_Division_All_DDL | Solution.Web/Thana_UI/District_Entry.aspx.cs:32 |  | ~/Thana_UI/District_Entry.aspx/Get_Division_All_DDL | via BLL |
| Thana_UI_District_Entry.Get_District_All_DDL | Solution.Web/Thana_UI/District_Entry.aspx.cs:41 | string id | ~/Thana_UI/District_Entry.aspx/Get_District_All_DDL | via BLL |
| Thana_UI_District_Entry.Get_Thana_By_Id | Solution.Web/Thana_UI/District_Entry.aspx.cs:50 | string id | ~/Thana_UI/District_Entry.aspx/Get_Thana_By_Id | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ThanaDal.SaveThanaInfo | Library.DAL/Thana_DAL/ThanaDal.cs:36 | sp_Save_ThanaInfo, sp_UD_ThanaInfo, sp_check_ThanaInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id |
| ThanaDal.GetDivision_All_DDL | Library.DAL/Thana_DAL/ThanaDal.cs:155 | sp_Get_Division_Active_DDL |  |
| ThanaDal.GetDistrict_All_DDL | Library.DAL/Thana_DAL/ThanaDal.cs:215 | sp_Get_District_Active_DDL | @id |
| ThanaDal.Get_Thana_By_Id_Dal | Library.DAL/Thana_DAL/ThanaDal.cs:174 | sp_GET_ThanaInfo_ById | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ThanaInfo_ById | ThanaDal.Get_Thana_By_Id_Dal | body NOT in repository | read-like name (INFERRED) |
| sp_Get_District_Active_DDL | ThanaDal.GetDistrict_All_DDL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Division_Active_DDL | ThanaDal.GetDivision_All_DDL | body NOT in repository | read-like name (INFERRED) |
| sp_Save_ThanaInfo | ThanaDal.SaveThanaInfo | body NOT in repository | write-like name |
| sp_UD_ThanaInfo | ThanaDal.SaveThanaInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_ThanaInfo | ThanaDal.SaveThanaInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
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
- try/catch present in 0 of 5 methods.
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
BLL: —; DAL: ThanaDal; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/Thana_UI/District_Entry.aspx`, `Solution.Web/Thana_UI/District_Entry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Thana_UI/District_Entry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 9 controls parsed | n/a | PARTIAL | Solution.Web/Thana_UI/District_Entry.aspx |
| Data access | This document §15–17 | 6 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 6 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M03 (Organisation & Market Structure Master).
- Other pages sharing this page's stored procedures: PAGE-094
- Changing DAL methods ThanaDal.GetDistrict_All_DDL, ThanaDal.GetDivision_All_DDL, ThanaDal.Get_Thana_By_Id_Dal, ThanaDal.SaveThanaInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
