---
page_id: PAGE-657
module: M16
page: PrescriptionView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-657 — PrescriptionView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-657 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | PrescriptionView |
| Kind | List / View (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_PrescriptionView |
| aspx size | 22983 bytes |
| Code-behind LOC | 293 |
| Inline script size | 11136 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **PrescriptionView** (List / View). Page title/heading text: `Prescription List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/Prescription.aspx`

Outbound references: `prescription.aspx`

## 5. Route
`~/DoctorModule_UI/PrescriptionView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/PrescriptionView.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/DoctorVisit_UI/IVMasterStructureForDCR.ascx |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | SetupDAL (Library.DAL/DoctorModule_DAL/SetupDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→EmpCetegoryAddImageButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| FromDate | TextBox | Select Date | no validator found |  |  |  |  |
| ToDate | TextBox | Select Date | no validator found |  |  |  |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| EmployeeIdSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| UserRoleSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlPharmaPlatform | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Prescription Date | no validator found |  |  |  |  |
| (no id) | BoundField | Prescription Type | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor | no validator found |  |  |  |  |
| (no id) | BoundField | Pharma Platform | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | BoundField | Create By | no validator found |  |  |  |  |
| (no id) | TemplateField | Image | no validator found |  |  |  |  |
| hpImg | a |  | no validator found |  |  |  |  |
| imgShow | Image |  | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Action | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| FromDate | param | DoctorModule_UI_PrescriptionView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | param | DoctorModule_UI_PrescriptionView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| UserRoleSelect | param | DoctorModule_UI_PrescriptionView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | param | DoctorModule_UI_PrescriptionView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | param | DoctorModule_UI_PrescriptionView.param | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | DoctorModule_UI_PrescriptionView.EmpCetegoryAddImageButton_Click | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:260 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: GetApprovalStatusList, GetUser, GetUserRoleInfo, LoadPrescriptionList, ResetLink, editClick, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): PrescriptionView.aspx/Get_PrescriptionList
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_PrescriptionView.Page_Load | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:27 | object sender, EventArgs e | DoctorModule_UI_PrescriptionView.Page_Load → CommonDataLoad.GetEmpMarketStructure_Active / CommonDataLoad.GetEmployeeList_Active / SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetSMCTypeListParam / SetupDAL.Get_PrescriptionList / SetupDAL.Get → SP: sp_CS_Ge | yes | no |
| DoctorModule_UI_PrescriptionView.LoadData | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:54 |  | DoctorModule_UI_PrescriptionView.LoadData → CommonDataLoad.GetEmpMarketStructure_Active / SetupDAL.Get_PrescriptionList → SP: sp_Get_EmployeeFieldForceInfo_EmpId, sp_Get_PrescriptionList | no | no |
| DoctorModule_UI_PrescriptionView.btnSearch_Click | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:62 | object sender, EventArgs e | DoctorModule_UI_PrescriptionView.btnSearch_Click → CommonDataLoad.GetEmpMarketStructure_Active / SetupDAL.Get_PrescriptionList → SP: sp_Get_EmployeeFieldForceInfo_EmpId, sp_Get_PrescriptionList | no | no |
| DoctorModule_UI_PrescriptionView.param | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:67 |  | DoctorModule_UI_PrescriptionView.param → CommonDataLoad.GetEmpMarketStructure_Active → SP: sp_Get_EmployeeFieldForceInfo_EmpId | no | no |
| DoctorModule_UI_PrescriptionView.LoadInitialInfo | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:177 |  | DoctorModule_UI_PrescriptionView.LoadInitialInfo → CommonDataLoad.GetEmployeeList_Active / SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetSMCTypeListParam / SetupDAL.Get_UserRoleInfo → SP: sp_CS_GetEmpGeneralInfo_All, sp_GET_ActionStatusList, sp_GET_SMCTyp | yes | no |
| DoctorModule_UI_PrescriptionView.Get_PrescriptionList | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:248 | string param | DoctorModule_UI_PrescriptionView.Get_PrescriptionList → SetupDAL.Get_PrescriptionList → SP: sp_Get_PrescriptionList | no | no |
| DoctorModule_UI_PrescriptionView.EmpCetegoryAddImageButton_Click | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:260 | object sender, EventArgs e | DoctorModule_UI_PrescriptionView.EmpCetegoryAddImageButton_Click | no | no |
| DoctorModule_UI_PrescriptionView.resetBtn_Click | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:265 | object sender, EventArgs e | DoctorModule_UI_PrescriptionView.resetBtn_Click | no | no |
| DoctorModule_UI_PrescriptionView.gv_DocumentUpload_PreRender | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:270 | object sender, EventArgs e | DoctorModule_UI_PrescriptionView.gv_DocumentUpload_PreRender | no | no |
| DoctorModule_UI_PrescriptionView.loadGridView_RowCommand | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:282 | object sender, GridViewCommandEventArgs e | DoctorModule_UI_PrescriptionView.loadGridView_RowCommand | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_PrescriptionView.Get_PrescriptionList | Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs:248 | string param | ~/DoctorModule_UI/PrescriptionView.aspx/Get_PrescriptionList | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SetupDAL.Get_PrescriptionList | Library.DAL/DoctorModule_DAL/SetupDAL.cs:2920 | sp_Get_PrescriptionList | @param |
| CommonDataLoad.GetEmpMarketStructure_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:2062 | sp_Get_EmployeeFieldForceInfo_EmpId | @id |
| SeedDataDAL.GetSMCTypeListParam | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:949 | sp_GET_SMCTypeListParm | @Parm |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| SetupDAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5243 | sp_Get_UserRoleInfo |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_SMCTypeListParm | SeedDataDAL.GetSMCTypeListParam | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeFieldForceInfo_EmpId | CommonDataLoad.GetEmpMarketStructure_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_PrescriptionList | SetupDAL.Get_PrescriptionList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | SetupDAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |

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
- try/catch present in 2 of 10 methods.
- Redirect targets: Prescription.aspx, Prescription.aspx?id=, PrescriptionView.aspx
- User messages: Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, SeedDataDAL, SetupDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/PrescriptionView.aspx`, `Solution.Web/DoctorModule_UI/PrescriptionView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/PrescriptionView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 31 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/PrescriptionView.aspx |
| Data access | This document §15–17 | 6 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 6 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-167, PAGE-168, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-327, PAGE-505, PAGE-548, PAGE-549 …
- Changing DAL methods CommonDataLoad.GetEmpMarketStructure_Active, CommonDataLoad.GetEmployeeList_Active, SeedDataDAL.GetApprovalStatusList, SeedDataDAL.GetSMCTypeListParam, SetupDAL.Get_PrescriptionList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
