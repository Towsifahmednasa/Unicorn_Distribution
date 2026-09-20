---
page_id: PAGE-177
module: M05
page: SMCTypeView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-177 — SMCTypeView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-177 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | SMCTypeView |
| Kind | List / View (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_SMCTypeView |
| aspx size | 9593 bytes |
| Code-behind LOC | 64 |
| Inline script size | 6241 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SMCTypeView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/SMCType.aspx`

Outbound references: `smctype.aspx`

## 5. Route
`~/MasterSetup_UI/SMCTypeView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/SMCTypeView.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | ProgramDal (Library.DAL/MasterSetup_DAL/ProgramDal.cs) |
| DAO/entity class | ProgramType (Library.DAO/DoctorModule_DAO/ProgramType.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: ActiveInactiveClick, DeleteClick, Final_Click, Final_DeleteClick, GetDegree, editClick, un
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): /Department/Delete_EmployeeDepartment, /ProgramType/ActiveInactive_departmentInfo, ProgramTypeView.aspx/GetSMCTypeList
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_SMCTypeView.Page_Load | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:20 | object sender, EventArgs e | MasterSetup_UI_SMCTypeView.Page_Load | no | no |
| MasterSetup_UI_SMCTypeView.Save_DepartmentInfo | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:26 | ProgramType department | MasterSetup_UI_SMCTypeView.Save_DepartmentInfo → ProgramDal.Save_DepartmentInfo → SP: sp_Save_ProgramTypeInfo, sp_UD_ProgramTypeInfo, sp_check_Programtype | no | yes |
| MasterSetup_UI_SMCTypeView.GetDepartmentList | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:38 |  | MasterSetup_UI_SMCTypeView.GetDepartmentList → ProgramDal.GetDepartmentList → SP: sp_GET_ProgramtypeInfo | no | no |
| MasterSetup_UI_SMCTypeView.GetDepartmentEditData | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:55 | int id | MasterSetup_UI_SMCTypeView.GetDepartmentEditData → ProgramDal.GetEmployeeLeaveForEdit → SP: sp_GET_ProgramType_ById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| MasterSetup_UI_SMCTypeView.Save_DepartmentInfo | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:26 | ProgramType department | ~/MasterSetup_UI/SMCTypeView.aspx/Save_DepartmentInfo | via BLL |
| MasterSetup_UI_SMCTypeView.GetDepartmentList | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:38 |  | ~/MasterSetup_UI/SMCTypeView.aspx/GetDepartmentList | via BLL |
| MasterSetup_UI_SMCTypeView.GetDepartmentEditData | Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs:55 | int id | ~/MasterSetup_UI/SMCTypeView.aspx/GetDepartmentEditData | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ProgramDal.Save_DepartmentInfo | Library.DAL/MasterSetup_DAL/ProgramDal.cs:18 | sp_Save_ProgramTypeInfo, sp_UD_ProgramTypeInfo, sp_check_Programtype | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @UpdateBy, @id |
| ProgramDal.GetDepartmentList | Library.DAL/MasterSetup_DAL/ProgramDal.cs:141 | sp_GET_ProgramtypeInfo | @Parameter |
| ProgramDal.GetEmployeeLeaveForEdit | Library.DAL/MasterSetup_DAL/ProgramDal.cs:215 | sp_GET_ProgramType_ById | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ProgramType_ById | ProgramDal.GetEmployeeLeaveForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramtypeInfo | ProgramDal.GetDepartmentList | body NOT in repository | read-like name (INFERRED) |
| sp_Save_ProgramTypeInfo | ProgramDal.Save_DepartmentInfo | body NOT in repository | write-like name |
| sp_UD_ProgramTypeInfo | ProgramDal.Save_DepartmentInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_Programtype | ProgramDal.Save_DepartmentInfo | body NOT in repository | read-like name (INFERRED) |

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
BLL: —; DAL: ProgramDal; DAO: ProgramType, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/SMCTypeView.aspx`, `Solution.Web/MasterSetup_UI/SMCTypeView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/SMCTypeView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 7 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/SMCTypeView.aspx |
| Data access | This document §15–17 | 5 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 5 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: PAGE-175
- Changing DAL methods ProgramDal.GetDepartmentList, ProgramDal.GetEmployeeLeaveForEdit, ProgramDal.Save_DepartmentInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
