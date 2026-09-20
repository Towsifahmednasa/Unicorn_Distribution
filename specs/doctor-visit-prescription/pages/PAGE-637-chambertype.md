---
page_id: PAGE-637
module: M16
page: ChamberType
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-637 — ChamberType

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-637 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | ChamberType |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorMaster_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorMaster_UI_ChamberType |
| aspx size | 11165 bytes |
| Code-behind LOC | 41 |
| Inline script size | 5040 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ChamberType** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorMaster_UI/ChamberTypeView.aspx`

Outbound references: `chambertypeview.aspx`

## 5. Route
`~/DoctorMaster_UI/ChamberType.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorMaster_UI/ChamberType.aspx |
| Code-behind | Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | DoctorChamberDal (Library.DAL/DoctorMaster_DAL/DoctorChamberDal.cs) |
| DAO/entity class | DoctorChamber (Library.DAO/DoctorModule_DAO/DoctorChamber.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| mainName | input | Chamber Type | yes (html) |  |  |  |  |
| customSwitch1 | input |  | no validator found |  |  |  | onchange→IsActiveChange() |
| acDate | input | Select Date | no validator found |  |  |  |  |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | TextBox |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: ConfirmationClick, FinalSave, GetData, RemoveValidationTooltip, Save, Validation, ValidationTooltip
- jQuery/DOM bindings: #customSwitch1:change
- AJAX endpoints (`url:`): ChamberType.aspx/GetDoctorDegreeEditDataa, ChamberType.aspx/Save_DoctorChamber
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorMaster_UI_ChamberType.Page_Load | Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs:16 | object sender, EventArgs e | DoctorMaster_UI_ChamberType.Page_Load | no | no |
| DoctorMaster_UI_ChamberType.Save_DoctorChamber | Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs:29 | DoctorChamber chamber | DoctorMaster_UI_ChamberType.Save_DoctorChamber → DoctorChamberDal.SaveDoctorChamber → SP: sp_Save_DoctorChamber, sp_Update_DoctorChamber, sp_check_DoctorChamber | no | yes |
| DoctorMaster_UI_ChamberType.GetDoctorDegreeEditDataa | Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs:38 | int id | DoctorMaster_UI_ChamberType.GetDoctorDegreeEditDataa → DoctorChamberDal.GetDoctorChamberForEditt → SP: sp_Get_DoctorChamber_ById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorMaster_UI_ChamberType.Save_DoctorChamber | Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs:29 | DoctorChamber chamber | ~/DoctorMaster_UI/ChamberType.aspx/Save_DoctorChamber | via BLL |
| DoctorMaster_UI_ChamberType.GetDoctorDegreeEditDataa | Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs:38 | int id | ~/DoctorMaster_UI/ChamberType.aspx/GetDoctorDegreeEditDataa | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DoctorChamberDal.SaveDoctorChamber | Library.DAL/DoctorMaster_DAL/DoctorChamberDal.cs:20 | sp_Save_DoctorChamber, sp_Update_DoctorChamber, sp_check_DoctorChamber | @Activedate, @ChamberName, @EntryBy, @UpdateBy |
| DoctorChamberDal.GetDoctorChamberForEditt | Library.DAL/DoctorMaster_DAL/DoctorChamberDal.cs:101 | sp_Get_DoctorChamber_ById | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_DoctorChamber_ById | DoctorChamberDal.GetDoctorChamberForEditt | body NOT in repository | read-like name (INFERRED) |
| sp_Save_DoctorChamber | DoctorChamberDal.SaveDoctorChamber | body NOT in repository | write-like name |
| sp_Update_DoctorChamber | DoctorChamberDal.SaveDoctorChamber | body NOT in repository | write-like name |
| sp_check_DoctorChamber | DoctorChamberDal.SaveDoctorChamber | body NOT in repository | read-like name (INFERRED) |

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
BLL: —; DAL: DoctorChamberDal; DAO: DoctorChamber, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DoctorMaster_UI/ChamberType.aspx`, `Solution.Web/DoctorMaster_UI/ChamberType.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorMaster_UI/ChamberType.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 9 controls parsed | n/a | PARTIAL | Solution.Web/DoctorMaster_UI/ChamberType.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DoctorChamberDal.GetDoctorChamberForEditt, DoctorChamberDal.SaveDoctorChamber affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
