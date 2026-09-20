---
page_id: PAGE-649
module: M16
page: PrescriptionType
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-649 — PrescriptionType

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-649 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | PrescriptionType |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorMaster_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorMaster_UI_PrescriptionType |
| aspx size | 9480 bytes |
| Code-behind LOC | 45 |
| Inline script size | 3706 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **PrescriptionType** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/PrescriptionTypeView.aspx`

Outbound references: `prescriptiontypeview.aspx`

## 5. Route
`~/DoctorMaster_UI/PrescriptionType.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx |
| Code-behind | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | PrescriptionTypeDal (Library.DAL/DoctorMaster_DAL/PrescriptionTypeDal.cs) |
| DAO/entity class | PrescriptionType (Library.DAO/SInventory_Entities/PrescriptionType.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| mainName | input | Prescription Type | yes (html) |  |  |  |  |
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
- Client script functions in page: ConfirmationClick, FinalSave, GetPrescriptionTypeData, IsActiveChange, Save, Validation
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): PrescriptionType.aspx/GetDoctorSpecialDayForEdit, PrescriptionType.aspx/Save_PrescriptionType
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorMaster_UI_PrescriptionType.Page_Load | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs:19 | object sender, EventArgs e | DoctorMaster_UI_PrescriptionType.Page_Load | no | no |
| DoctorMaster_UI_PrescriptionType.Save_PrescriptionType | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs:32 | PrescriptionType aPrescriptionType | DoctorMaster_UI_PrescriptionType.Save_PrescriptionType → PrescriptionTypeDal.Save_PrescriptionType → SP: sp_Save_PrescriptionType, sp_Update_PrescriptionType, sp_check_PrescriptionType | no | yes |
| DoctorMaster_UI_PrescriptionType.GetDoctorSpecialDayForEdit | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs:42 | int id | DoctorMaster_UI_PrescriptionType.GetDoctorSpecialDayForEdit → PrescriptionTypeDal.GetPrescriptionForEdit → SP: sp_Get_PrescriptionType_ById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorMaster_UI_PrescriptionType.Save_PrescriptionType | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs:32 | PrescriptionType aPrescriptionType | ~/DoctorMaster_UI/PrescriptionType.aspx/Save_PrescriptionType | via BLL |
| DoctorMaster_UI_PrescriptionType.GetDoctorSpecialDayForEdit | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs:42 | int id | ~/DoctorMaster_UI/PrescriptionType.aspx/GetDoctorSpecialDayForEdit | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| PrescriptionTypeDal.Save_PrescriptionType | Library.DAL/DoctorMaster_DAL/PrescriptionTypeDal.cs:66 | sp_Save_PrescriptionType, sp_Update_PrescriptionType, sp_check_PrescriptionType | @Activedate, @EntryBy, @IsActive, @PrescriptionType, @PrescriptionTypeId, @UpdateBy, PrescriptionTyp |
| PrescriptionTypeDal.GetPrescriptionForEdit | Library.DAL/DoctorMaster_DAL/PrescriptionTypeDal.cs:16 | sp_Get_PrescriptionType_ById | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_PrescriptionType_ById | PrescriptionTypeDal.GetPrescriptionForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Save_PrescriptionType | PrescriptionTypeDal.Save_PrescriptionType | body NOT in repository | write-like name |
| sp_Update_PrescriptionType | PrescriptionTypeDal.Save_PrescriptionType | body NOT in repository | write-like name |
| sp_check_PrescriptionType | PrescriptionTypeDal.Save_PrescriptionType | body NOT in repository | read-like name (INFERRED) |

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
BLL: —; DAL: PrescriptionTypeDal; DAO: PrescriptionType, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DoctorMaster_UI/PrescriptionType.aspx`, `Solution.Web/DoctorMaster_UI/PrescriptionType.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 9 controls parsed | n/a | PARTIAL | Solution.Web/DoctorMaster_UI/PrescriptionType.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: PAGE-655
- Changing DAL methods PrescriptionTypeDal.GetPrescriptionForEdit, PrescriptionTypeDal.Save_PrescriptionType affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
