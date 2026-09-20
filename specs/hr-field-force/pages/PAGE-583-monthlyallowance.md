---
page_id: PAGE-583
module: M15
page: MonthlyAllowance
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-583 — MonthlyAllowance

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-583 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | MonthlyAllowance |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_MonthlyAllowance |
| aspx size | 18906 bytes |
| Code-behind LOC | 343 |
| Inline script size | 8783 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MonthlyAllowance** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/MonthlyAllowanceView.aspx`

Outbound references: `monthlyallowanceview.aspx`

## 5. Route
`~/DoctorModule_UI/MonthlyAllowance.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | EmployeeInformationDaL (Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAL class | SetupDAL (Library.DAL/DoctorModule_DAL/SetupDAL.cs) |
| DAO/entity class | MonthlyAllowance (Library.DAO/DoctorModule_DAO/MonthlyAllowance.cs) |
| DAO/entity class | MonthlyAllowanceDtlDAO (Library.DAO/DoctorModule_DAO/MonthlyAllowance.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| MonthlyAllowanceName | TextBox | Monthly Allowance Name | no validator found |  |  |  |  |
| Allowance | TextBox | Amount | no validator found |  |  |  |  |
| customSwitch1 | CheckBox | Is Active | no validator found | true |  |  |  |
| UserRoleSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→UserRoleSelect_SelectedIndexChanged |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfUserRoleId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpInfoId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Code | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Role | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |
| restbtn | LinkButton |  | no validator found |  |  |  | OnClick→restbtn_Click |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| UserRoleSelect | param | DoctorModule_UI_MonthlyAllowance.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| id_mastetID | dt | DoctorModule_UI_MonthlyAllowance.LoadData | — | EmployeeInformationDaL.GetEmployeeInformationListForActive, Setup2DAL.GetMonthlyAllowanceForEdit | UNKNOWN | UNKNOWN | UNKNOWN |
| hfEmpInfoId | MonthlyAllowanceDtlDAO.EmpInfoId | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @EmpInfoId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfUserRoleId | MonthlyAllowance.UserRoleId | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @UserRoleId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | MonthlyAllowance.MonthlyAllowanceId | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @MonthlyAllowanceId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | MonthlyAllowance.MonthlyAllowanceId | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @id | UNKNOWN (SP body not in repo) | UNKNOWN |
| MonthlyAllowanceName | MonthlyAllowance.MonthlyAllowanceName | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @MonthlyAllowanceName | UNKNOWN (SP body not in repo) | UNKNOWN |
| MonthlyAllowanceName | MonthlyAllowance.MonthlyAllowanceName | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @Name | UNKNOWN (SP body not in repo) | UNKNOWN |
| Allowance | MonthlyAllowance.Allowance | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @MonthlyAllowance | UNKNOWN (SP body not in repo) | UNKNOWN |
| UserRoleSelect | MonthlyAllowance.UserRoleId | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @UserRoleId | UNKNOWN (SP body not in repo) | UNKNOWN |
| customSwitch1 | MonthlyAllowance.IsActive | DoctorModule_UI_MonthlyAllowance.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveMonthlyAllowance | @IsActive | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_MonthlyAllowance.btnSave_Click | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:264 | — | Setup2DAL.SaveMonthlyAllowance | sp_Save_MonthlyAllowance, sp_Save_MonthlyAllowanceDetail, sp_Update_MonthlyAllowance, sp_check_EmployeeAllowance, sp_che | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_MonthlyAllowance.btnSave_Click | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:264 | — | Setup2DAL.SaveMonthlyAllowance | sp_Save_MonthlyAllowance, sp_Save_MonthlyAllowanceDetail, sp_Update_MonthlyAllowance, sp_check_EmployeeAllowance, sp_che | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| restbtn | LinkButton click | (postback) | DoctorModule_UI_MonthlyAllowance.restbtn_Click | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:340 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: FinalSave, GetData, IsActiveChange, RemoveValidationTooltip, ResetClick, ResetLink, Save, Validation, ValidationTooltip, getUrlVars, pageLoad
- jQuery/DOM bindings: #Allowance:keypress, .clsDecimal:keypress
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_MonthlyAllowance.Page_Load | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:20 | object sender, EventArgs e | DoctorModule_UI_MonthlyAllowance.Page_Load → EmployeeInformationDaL.GetEmployeeInformationListForActive / Setup2DAL.GetMonthlyAllowanceForEdit / SetupDAL.Get_UserRoleInfo → SP: sp_Get_EmployeeInformationListActive, sp_Get_MonthlyAllowance_ById, sp_Get_UserRole | yes | no |
| DoctorModule_UI_MonthlyAllowance.GetOneRecord | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:48 | string Id | DoctorModule_UI_MonthlyAllowance.GetOneRecord → Setup2DAL.GetMonthlyAllowanceForEdit → SP: sp_Get_MonthlyAllowance_ById | yes | no |
| DoctorModule_UI_MonthlyAllowance.LoadData | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:113 |  | DoctorModule_UI_MonthlyAllowance.LoadData → EmployeeInformationDaL.GetEmployeeInformationListForActive / Setup2DAL.GetMonthlyAllowanceForEdit → SP: sp_Get_EmployeeInformationListActive, sp_Get_MonthlyAllowance_ById | yes | no |
| DoctorModule_UI_MonthlyAllowance.chkSelectAll_CheckedChanged | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:156 | object sender, EventArgs e | DoctorModule_UI_MonthlyAllowance.chkSelectAll_CheckedChanged | no | no |
| DoctorModule_UI_MonthlyAllowance.gv_DocumentUpload_PreRender | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:166 | object sender, EventArgs e | DoctorModule_UI_MonthlyAllowance.gv_DocumentUpload_PreRender | no | no |
| DoctorModule_UI_MonthlyAllowance.LoadInitialInfo | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:177 |  | DoctorModule_UI_MonthlyAllowance.LoadInitialInfo → SetupDAL.Get_UserRoleInfo → SP: sp_Get_UserRoleInfo | yes | no |
| DoctorModule_UI_MonthlyAllowance.UserRoleSelect_SelectedIndexChanged | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:196 | object sender, EventArgs e | DoctorModule_UI_MonthlyAllowance.UserRoleSelect_SelectedIndexChanged → EmployeeInformationDaL.GetEmployeeInformationListForActive / Setup2DAL.GetMonthlyAllowanceForEdit → SP: sp_Get_EmployeeInformationListActive, sp_Get_MonthlyAllowance_ById | no | no |
| DoctorModule_UI_MonthlyAllowance.btnSave_Click | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:264 | object sender, EventArgs e | DoctorModule_UI_MonthlyAllowance.btnSave_Click → Setup2DAL.SaveMonthlyAllowance → SP: sp_Save_MonthlyAllowance, sp_Save_MonthlyAllowanceDetail, sp_Update_MonthlyAllowance, sp_check_EmployeeAllowance, sp_check_Vali_MarketStructure | no | yes |
| DoctorModule_UI_MonthlyAllowance.restbtn_Click | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs:340 | object sender, EventArgs e | DoctorModule_UI_MonthlyAllowance.restbtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.GetMonthlyAllowanceForEdit | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4188 | sp_Get_MonthlyAllowance_ById | @id |
| EmployeeInformationDaL.GetEmployeeInformationListForActive | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:567 | sp_Get_EmployeeInformationListActive | @Parm |
| SetupDAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5243 | sp_Get_UserRoleInfo |  |
| Setup2DAL.SaveMonthlyAllowance | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4260 | sp_Save_MonthlyAllowance, sp_Save_MonthlyAllowanceDetail, sp_Update_MonthlyAllow | @EmpInfoId, @EntryBy, @IsActive, @MasterId, @MonthlyAllowance, @MonthlyAllowanceId, @MonthlyAllowanc |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_EmployeeInformationListActive | EmployeeInformationDaL.GetEmployeeInformationListForActive | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MonthlyAllowance_ById | Setup2DAL.GetMonthlyAllowanceForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | SetupDAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Save_MonthlyAllowance | Setup2DAL.SaveMonthlyAllowance | body NOT in repository | write-like name |
| sp_Save_MonthlyAllowanceDetail | Setup2DAL.SaveMonthlyAllowance | body NOT in repository | write-like name |
| sp_Update_MonthlyAllowance | Setup2DAL.SaveMonthlyAllowance | body NOT in repository | write-like name |
| sp_check_EmployeeAllowance | Setup2DAL.SaveMonthlyAllowance | body NOT in repository | read-like name (INFERRED) |
| sp_check_Vali_MarketStructure | Setup2DAL.SaveMonthlyAllowance | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select at least one row ! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','MonthlyAllowanceView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Data cannot be deactivated! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 4 of 11 methods.
- Redirect targets: MonthlyAllowance.aspx
- User messages: ','Success','MonthlyAllowanceView.aspx');; Already Exist!; Data cannot be deactivated!; Please Select From List; Please Select at least one row !; form-control form-control-sm is-invalid
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: EmployeeInformationDaL, Setup2DAL, SetupDAL; DAO: MonthlyAllowance, MonthlyAllowanceDtlDAO, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnUpdate with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnUpdate with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click restbtn with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click restbtn with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx`, `Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/MonthlyAllowance.aspx |
| Data access | This document §15–17 | 8 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 8 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-042, PAGE-046, PAGE-057, PAGE-064, PAGE-099, PAGE-103, PAGE-105, PAGE-549, PAGE-562, PAGE-563, PAGE-564, PAGE-566 …
- Changing DAL methods EmployeeInformationDaL.GetEmployeeInformationListForActive, Setup2DAL.GetMonthlyAllowanceForEdit, Setup2DAL.SaveMonthlyAllowance, SetupDAL.Get_UserRoleInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
