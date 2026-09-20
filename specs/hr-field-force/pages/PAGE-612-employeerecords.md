---
page_id: PAGE-612
module: M15
page: EmployeeRecords
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-612 — EmployeeRecords

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-612 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmployeeRecords |
| Kind | List / View (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_EmployeeRecords |
| aspx size | 19437 bytes |
| Code-behind LOC | 66 |
| Inline script size | 11964 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **EmployeeRecords** (List / View). Page title/heading text: `Employee List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/UserSetup.aspx`, `MasterSetup_UI/EmployeeSetup.aspx`, `MasterSetup_UI/EmployeeSetupEdit.aspx`, `MasterSetup_UI/EmployeeSetupForNewJoiner.aspx`

Outbound references: `dashboardone.aspx`, `employeesetup.aspx`

## 5. Route
`~/MasterSetup_UI/EmployeeRecords.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | EmployeeInformationDaL (Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| btnAdd | a |  | no validator found |  |  |  |  |
| chkShow | CheckBox |  | no validator found |  | Visible=false |  |  |
| EmployeeIdSelect | select |  | no validator found |  |  |  |  |
| UserRoleSelect | select |  | no validator found |  |  |  |  |
| DesignationSelect | select |  | no validator found |  |  |  |  |
| ApprovalStatusSelect | select |  | no validator found |  |  |  |  |
| btnSave | button |  | no validator found |  |  |  | onclick→GetPrescription() |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| DesignationSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: AMeditClick, DZSMeditClick, DeleteClick, EmpeditClick, Final_DeleteClick, GetEmpDsg, GetEmpList, GetPrescription, GetUserRoleInfo, MIOeditClick, NSMMeditClick, ResetClick, UsereditClick, editClick, un
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): /EmployeeGeneral/Delete_EmployeeInformation
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_EmployeeRecords.Page_Load | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx.cs:20 | object sender, EventArgs e | MasterSetup_UI_EmployeeRecords.Page_Load → CommonDataLoad.GetPermissionForUserRole / CommonDataLoad.NormalizePageUrl → SP: sp_GET_MainPermissionByUserRoleandPageUrl | no | no |
| MasterSetup_UI_EmployeeRecords.GetEmployeeInformationList | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx.cs:28 | string param | MasterSetup_UI_EmployeeRecords.GetEmployeeInformationList → EmployeeInformationDaL.GetEmployeeInformationList → SP: sp_Get_EmployeeInformationList_Prm | no | no |
| MasterSetup_UI_EmployeeRecords.UserPersmissionValidation | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx.cs:37 |  | MasterSetup_UI_EmployeeRecords.UserPersmissionValidation → CommonDataLoad.GetPermissionForUserRole / CommonDataLoad.NormalizePageUrl → SP: sp_GET_MainPermissionByUserRoleandPageUrl | yes | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| MasterSetup_UI_EmployeeRecords.GetEmployeeInformationList | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx.cs:28 | string param | ~/MasterSetup_UI/EmployeeRecords.aspx/GetEmployeeInformationList | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetPermissionForUserRole | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:18 | sp_GET_MainPermissionByUserRoleandPageUrl | @PageName, @RoleId |
| CommonDataLoad.NormalizePageUrl | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:89 | delegates / no SQL literal |  |
| EmployeeInformationDaL.GetEmployeeInformationList | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:344 | sp_Get_EmployeeInformationList_Prm | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_MainPermissionByUserRoleandPageUrl | CommonDataLoad.GetPermissionForUserRole | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeInformationList_Prm | EmployeeInformationDaL.GetEmployeeInformationList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserRoleID
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 3 methods.
- Redirect targets: ../Dashboard_UI/DashboardOne.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, EmployeeInformationDaL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/EmployeeRecords.aspx`, `Solution.Web/MasterSetup_UI/EmployeeRecords.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 19 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/EmployeeRecords.aspx |
| Data access | This document §15–17 | 2 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-173, PAGE-464, PAGE-564, PAGE-670, PAGE-676, PAGE-695, PAGE-696, PAGE-697, PAGE-699, PAGE-701, PAGE-702, PAGE-703, PAGE-704, PAGE-705 …
- Changing DAL methods CommonDataLoad.GetPermissionForUserRole, CommonDataLoad.NormalizePageUrl, EmployeeInformationDaL.GetEmployeeInformationList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
