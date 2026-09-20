---
page_id: PAGE-670
module: M16
page: DoctorView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-670 — DoctorView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-670 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | DoctorView |
| Kind | List / View (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_DoctorView |
| aspx size | 25848 bytes |
| Code-behind LOC | 511 |
| Inline script size | 2981 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DoctorView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/DoctorEntry.aspx`

Outbound references: `dashboardone.aspx`, `doctorentry.aspx`

## 5. Route
`~/MasterSetup_UI/DoctorView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/DoctorView.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStforAll.ascx |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | DoctorDAL (Library.DAL/MasterSetup_DAL/DoctorDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→EmpCetegoryAddImageButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| ddlSalesPerson | DropDownList |  | no validator found |  |  | see §9 |  |
| DoctorTypeSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| degreeSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| custNameTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→custNameTextBox_TextChanged |
| at_txt_JobCirculation | AutoCompleteExtender |  | no validator found |  |  |  |  |
| hfCustomerId | HiddenField |  | no validator found |  |  |  |  |
| ddlProgramType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlPharmaPlatform | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlApprovalStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| frmDate | TextBox | Select Date | no validator found |  |  |  |  |
| toDate | TextBox | Select Date | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | OnClick→resetBtn_Click |
| lblCount | Label | Total : 0 | no validator found |  |  |  |  |
| btnExportToExcel | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Code | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Name | no validator found |  |  |  |  |
| (no id) | BoundField | Street Address | no validator found |  |  |  |  |
| (no id) | BoundField | Mobile NO | no validator found |  | Visible=True |  |  |
| (no id) | BoundField | Sales Person Name | no validator found |  |  |  |  |
| (no id) | BoundField | Zone Code | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Area Code | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Territory Code | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Market Code | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Market Name | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Designation | no validator found |  |  |  |  |
| (no id) | BoundField | Degree | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Type | no validator found |  |  |  |  |
| (no id) | BoundField | Provider Type | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Pharma Platform | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Doctor Speciality | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Tour Type | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Dept Tagging | no validator found |  | Visible=False |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| custNameTextBox | empName | MasterSetup_UI_DoctorView.custNameTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlProgramType | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlSalesPerson | spId | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| DoctorTypeSelect | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| degreeSelect | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlStatus | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlApprovalStatus | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerId | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| frmDate | param | MasterSetup_UI_DoctorView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlSalesPerson | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| DoctorTypeSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| degreeSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProgramType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlApprovalStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | MasterSetup_UI_DoctorView.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:331 | — | — | — | read-only / none detected |
| btnSearch | LinkButton click | (postback) | MasterSetup_UI_DoctorView.btnSearch_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:335 | — | DoctorDAL.GetDoctorList | sp_Get_DoctorList | SP executed — effect on tables UNKNOWN (body not in repo) |
| resetBtn | LinkButton click | (postback) | MasterSetup_UI_DoctorView.resetBtn_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:507 | — | — | — | read-only / none detected |
| btnExportToExcel | LinkButton click | (postback) | MasterSetup_UI_DoctorView.btnExportToExcel_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:233 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_DoctorView.Page_Load | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:30 | object sender, EventArgs e | MasterSetup_UI_DoctorView.Page_Load → CommonDataLoad.GetDoctorDegree_All / CommonDataLoad.GetDoctorSalesPerson_forSearch / CommonDataLoad.GetDoctorType_Active / CommonDataLoad.GetPermissionForUserRole / CommonDataLoad.NormalizePageUrl / S → SP: sp_GET_ActionSt | yes | no |
| MasterSetup_UI_DoctorView.UserPersmissionValidation | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:167 |  | MasterSetup_UI_DoctorView.UserPersmissionValidation → CommonDataLoad.GetPermissionForUserRole / CommonDataLoad.NormalizePageUrl → SP: sp_GET_MainPermissionByUserRoleandPageUrl | yes | no |
| MasterSetup_UI_DoctorView.custNameTextBox_TextChanged | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:200 | object sender, EventArgs e | MasterSetup_UI_DoctorView.custNameTextBox_TextChanged | no | no |
| MasterSetup_UI_DoctorView.btnExportToExcel_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:233 | object sender, EventArgs e | MasterSetup_UI_DoctorView.btnExportToExcel_Click | no | no |
| MasterSetup_UI_DoctorView.loadGridView_PageIndexChanging | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:297 | object sender, GridViewPageEventArgs e | MasterSetup_UI_DoctorView.loadGridView_PageIndexChanging | no | no |
| MasterSetup_UI_DoctorView.loadGridView_RowCommand | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:302 | object sender, GridViewCommandEventArgs e | MasterSetup_UI_DoctorView.loadGridView_RowCommand | no | no |
| MasterSetup_UI_DoctorView.gv_DocumentUpload_PreRender | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:320 | object sender, EventArgs e | MasterSetup_UI_DoctorView.gv_DocumentUpload_PreRender | no | no |
| MasterSetup_UI_DoctorView.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:331 | object sender, EventArgs e | MasterSetup_UI_DoctorView.EmpCetegoryAddImageButton_Click | no | no |
| MasterSetup_UI_DoctorView.btnSearch_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:335 | object sender, EventArgs e | MasterSetup_UI_DoctorView.btnSearch_Click → DoctorDAL.GetDoctorList → SP: sp_Get_DoctorList | no | no |
| MasterSetup_UI_DoctorView.LoadData | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:339 | string parm | MasterSetup_UI_DoctorView.LoadData → DoctorDAL.GetDoctorList → SP: sp_Get_DoctorList | no | no |
| MasterSetup_UI_DoctorView.Parm | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:418 |  | MasterSetup_UI_DoctorView.Parm | no | no |
| MasterSetup_UI_DoctorView.resetBtn_Click | Solution.Web/MasterSetup_UI/DoctorView.aspx.cs:507 | object sender, EventArgs e | MasterSetup_UI_DoctorView.resetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetDoctorType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:830 | sp_Get_DoctorType_Active |  |
| CommonDataLoad.GetDoctorSalesPerson_forSearch | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:852 | sp_GetDoctor_SalesPerson_forSearch |  |
| CommonDataLoad.GetDoctorDegree_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1092 | sp_Get_Degree_All |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| SeedDataDAL.GetProgramTypeListParam | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:928 | sp_GET_ProgramTypeListParm | @Parm |
| SeedDataDAL.GetSMCTypeListParam | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:949 | sp_GET_SMCTypeListParm | @Parm |
| CommonDataLoad.GetPermissionForUserRole | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:18 | sp_GET_MainPermissionByUserRoleandPageUrl | @PageName, @RoleId |
| CommonDataLoad.NormalizePageUrl | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:89 | delegates / no SQL literal |  |
| DoctorDAL.GetDoctorList | Library.DAL/MasterSetup_DAL/DoctorDAL.cs:417 | sp_Get_DoctorList | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MainPermissionByUserRoleandPageUrl | CommonDataLoad.GetPermissionForUserRole | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramTypeListParm | SeedDataDAL.GetProgramTypeListParam | body NOT in repository | read-like name (INFERRED) |
| sp_GET_SMCTypeListParm | SeedDataDAL.GetSMCTypeListParam | body NOT in repository | read-like name (INFERRED) |
| sp_GetDoctor_SalesPerson_forSearch | CommonDataLoad.GetDoctorSalesPerson_forSearch | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Degree_All | CommonDataLoad.GetDoctorDegree_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorList | DoctorDAL.GetDoctorList | LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:883 | read-like name (INFERRED) |
| sp_Get_DoctorType_Active | CommonDataLoad.GetDoctorType_Active | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblDoctorDegreeDetail
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler custNameTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Input Correct Data ! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, RoleTypeName, UserRoleID
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 15 methods.
- Redirect targets: ../Dashboard_UI/DashboardOne.aspx, DoctorEntry.aspx, DoctorView.aspx
- User messages: Input Correct Data !; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, DoctorDAL, SeedDataDAL; DAO: —; Tables (inline): tblDoctorDegreeDetail

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnSearch with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSearch with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click resetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click resetBtn with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnExportToExcel with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnExportToExcel with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/DoctorView.aspx`, `Solution.Web/MasterSetup_UI/DoctorView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/DoctorView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 51 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/DoctorView.aspx |
| Data access | This document §15–17 | 8 SP names, 1 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 8 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- Inline SQL in code-behind without SqlParameter (possible string-built SQL — verify): Parm

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-167, PAGE-168, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-464, PAGE-505, PAGE-548, PAGE-549, PAGE-557, PAGE-562 …
- Changing DAL methods CommonDataLoad.GetDoctorDegree_All, CommonDataLoad.GetDoctorSalesPerson_forSearch, CommonDataLoad.GetDoctorType_Active, CommonDataLoad.GetPermissionForUserRole, CommonDataLoad.NormalizePageUrl affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
