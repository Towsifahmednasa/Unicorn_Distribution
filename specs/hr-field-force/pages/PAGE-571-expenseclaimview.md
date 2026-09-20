---
page_id: PAGE-571
module: M15
page: ExpenseClaimView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-571 — ExpenseClaimView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-571 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | ExpenseClaimView |
| Kind | List / View (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_ExpenseClaimView |
| aspx size | 24190 bytes |
| Code-behind LOC | 283 |
| Inline script size | 11686 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ExpenseClaimView** (List / View). Page title/heading text: `Expense  Claim List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/ExpenseClaim.aspx`

Outbound references: `expenseclaim.aspx`

## 5. Route
`~/DoctorModule_UI/ExpenseClaimView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| hfEmpTerrId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpAreaId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpRegionId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpGroupId | HiddenField |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| FromDate | TextBox | Select Date | no validator found |  |  |  |  |
| EmployeeIdSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ToDate | TextBox | Select Date | no validator found |  |  |  |  |
| UserRoleSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Expense Date | no validator found |  |  |  |  |
| (no id) | BoundField | Employee ID | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Designation | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | BoundField | Expense Type  | no validator found |  |  |  |  |
| (no id) | BoundField | Expense Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Comments | no validator found |  |  |  |  |
| (no id) | TemplateField | Image | no validator found |  |  |  |  |
| hpImg | a |  | no validator found |  |  |  |  |
| imgShow | Image |  | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | BoundField | Created By | no validator found |  |  |  |  |
| (no id) | BoundField | Created At | no validator found |  |  |  |  |
| (no id) | BoundField | Updated By | no validator found |  |  |  |  |
| (no id) | BoundField | Updated At | no validator found |  |  |  |  |
| (no id) | BoundField | Forwarded By | no validator found |  |  |  |  |
| (no id) | BoundField | Forwarded At | no validator found |  |  |  |  |
| (no id) | BoundField | Approved By | no validator found |  |  |  |  |
| (no id) | BoundField | Approved At | no validator found |  |  |  |  |
| (no id) | TemplateField | Action | no validator found |  |  |  |  |
| hfExpenseClaimID | HiddenField |  | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| FromDate | param | DoctorModule_UI_ExpenseClaimView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | param | DoctorModule_UI_ExpenseClaimView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| UserRoleSelect | param | DoctorModule_UI_ExpenseClaimView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | param | DoctorModule_UI_ExpenseClaimView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: GetApprovalStatusList, GetAreaList, GetEmpList, GetUserRoleInfo, ResetClick, SelectOption_DtTable_Async_True, editClick, pageLoad, un
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_ExpenseClaimView.Page_Load | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:28 | object sender, EventArgs e | DoctorModule_UI_ExpenseClaimView.Page_Load → CommonDataLoad.GetEmpMarketStructure_Active / CommonDataLoad.GetEmployeeList_Active / SeedDataDAL.GetApprovalStatusList / Setup2DAL.GetExpenseClaimList / Setup2DAL.Get_UserRoleInfo → SP: sp_CS_GetEmpGeneralInfo_All, | yes | no |
| DoctorModule_UI_ExpenseClaimView.GetExpenseClaimList | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:49 | string param | DoctorModule_UI_ExpenseClaimView.GetExpenseClaimList → Setup2DAL.GetExpenseClaimList → SP: sp_Get_ExpenseClaimList | no | no |
| DoctorModule_UI_ExpenseClaimView.Get_UserRoleInfo | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:58 |  | DoctorModule_UI_ExpenseClaimView.Get_UserRoleInfo → Setup2DAL.Get_UserRoleInfo → SP: sp_Get_UserRoleInfo | no | no |
| DoctorModule_UI_ExpenseClaimView.gv_DocumentUpload_PreRender | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:67 | object sender, EventArgs e | DoctorModule_UI_ExpenseClaimView.gv_DocumentUpload_PreRender | no | no |
| DoctorModule_UI_ExpenseClaimView.loadGridView_RowCommand | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:79 | object sender, GridViewCommandEventArgs e | DoctorModule_UI_ExpenseClaimView.loadGridView_RowCommand | no | no |
| DoctorModule_UI_ExpenseClaimView.LoadInitialInfo | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:92 |  | DoctorModule_UI_ExpenseClaimView.LoadInitialInfo → CommonDataLoad.GetEmployeeList_Active / SeedDataDAL.GetApprovalStatusList / Setup2DAL.Get_UserRoleInfo → SP: sp_CS_GetEmpGeneralInfo_All, sp_GET_ActionStatusList, sp_Get_UserRoleInfo | yes | no |
| DoctorModule_UI_ExpenseClaimView.LoadData | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:147 |  | DoctorModule_UI_ExpenseClaimView.LoadData → CommonDataLoad.GetEmpMarketStructure_Active / Setup2DAL.GetExpenseClaimList → SP: sp_Get_EmployeeFieldForceInfo_EmpId, sp_Get_ExpenseClaimList | no | no |
| DoctorModule_UI_ExpenseClaimView.btnSearch_Click | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:184 | object sender, EventArgs e | DoctorModule_UI_ExpenseClaimView.btnSearch_Click → CommonDataLoad.GetEmpMarketStructure_Active / Setup2DAL.GetExpenseClaimList → SP: sp_Get_EmployeeFieldForceInfo_EmpId, sp_Get_ExpenseClaimList | no | no |
| DoctorModule_UI_ExpenseClaimView.param | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:189 |  | DoctorModule_UI_ExpenseClaimView.param → CommonDataLoad.GetEmpMarketStructure_Active → SP: sp_Get_EmployeeFieldForceInfo_EmpId | yes | no |
| DoctorModule_UI_ExpenseClaimView.resetBtn_Click | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:280 | object sender, EventArgs e | DoctorModule_UI_ExpenseClaimView.resetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_ExpenseClaimView.GetExpenseClaimList | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:49 | string param | ~/DoctorModule_UI/ExpenseClaimView.aspx/GetExpenseClaimList | via BLL |
| DoctorModule_UI_ExpenseClaimView.Get_UserRoleInfo | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs:58 |  | ~/DoctorModule_UI/ExpenseClaimView.aspx/Get_UserRoleInfo | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.GetExpenseClaimList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2358 | sp_Get_ExpenseClaimList | @param |
| CommonDataLoad.GetEmpMarketStructure_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:2062 | sp_Get_EmployeeFieldForceInfo_EmpId | @id |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| Setup2DAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4727 | sp_Get_UserRoleInfo |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeFieldForceInfo_EmpId | CommonDataLoad.GetEmpMarketStructure_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpenseClaimList | Setup2DAL.GetExpenseClaimList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | Setup2DAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 3 of 10 methods.
- Redirect targets: ExpenseClaimView.aspx
- User messages: Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, SeedDataDAL, Setup2DAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx`, `Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 45 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/ExpenseClaimView.aspx |
| Data access | This document §15–17 | 5 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 5 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-167, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-327, PAGE-548, PAGE-549, PAGE-555, PAGE-557 …
- Changing DAL methods CommonDataLoad.GetEmpMarketStructure_Active, CommonDataLoad.GetEmployeeList_Active, SeedDataDAL.GetApprovalStatusList, Setup2DAL.GetExpenseClaimList, Setup2DAL.Get_UserRoleInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
