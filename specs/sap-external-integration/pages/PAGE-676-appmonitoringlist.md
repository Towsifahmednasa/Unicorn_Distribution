---
page_id: PAGE-676
module: M17
page: AppMonitoringList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-676 — AppMonitoringList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-676 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | AppMonitoringList |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_AppMonitoringList |
| aspx size | 12182 bytes |
| Code-behind LOC | 162 |
| Inline script size | 3014 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **AppMonitoringList** (Report / Dashboard). Page title/heading text: `App Monitoring List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: SAP hand-off pages, MIGO, provider dropout integration, API-customer import, mobile-app data-load web methods.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `dashboardone.aspx`, `usersetup.aspx`

## 5. Route
`~/DoctorModule_UI/AppMonitoringList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | RSMSetupDal (Library.DAL/DoctorModule_DAL/RSMSetupDal.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlEmployeeName | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlUserRole | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlActiveStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | User Name  | no validator found |  |  |  |  |
| (no id) | BoundField | Employee ID | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Role | no validator found |  |  |  |  |
| (no id) | BoundField | UuId 1 | no validator found |  |  |  |  |
| (no id) | BoundField | UuId 2 | no validator found |  |  |  |  |
| (no id) | BoundField | Device Info 1 | no validator found |  |  |  |  |
| (no id) | BoundField | Device Info 2 | no validator found |  |  |  |  |
| (no id) | BoundField | OS 1 | no validator found |  |  |  |  |
| (no id) | BoundField | OS 2 | no validator found |  |  |  |  |
| (no id) | BoundField | OS Version 1 | no validator found |  |  |  |  |
| (no id) | BoundField | OS Version 2 | no validator found |  |  |  |  |
| (no id) | BoundField | App Version Name 1 | no validator found |  |  |  |  |
| (no id) | BoundField | App Version Name 2 | no validator found |  |  |  |  |
| (no id) | BoundField | Last Access Time 2 | no validator found |  |  |  |  |
| (no id) | BoundField | Last Access Time 2 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlUserRole | param | DoctorModule_UI_AppMonitoringList.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlActiveStatus | param | DoctorModule_UI_AppMonitoringList.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlEmployeeName | param | DoctorModule_UI_AppMonitoringList.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlEmployeeName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlUserRole | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlActiveStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

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
| DoctorModule_UI_AppMonitoringList.Page_Load | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:21 | object sender, EventArgs e | DoctorModule_UI_AppMonitoringList.Page_Load → CommonDataLoad.GetPermissionForUserRole / CommonDataLoad.NormalizePageUrl / RSMSetupDal.Get_AppMonitoringList_new / SeedDataDAL.GetEmployee_All / SeedDataDAL.GetUserRoleList → SP: AppMonitoringViewList_new, sp_GET_ | yes | no |
| DoctorModule_UI_AppMonitoringList.UserPersmissionValidation | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:62 |  | DoctorModule_UI_AppMonitoringList.UserPersmissionValidation → CommonDataLoad.GetPermissionForUserRole / CommonDataLoad.NormalizePageUrl → SP: sp_GET_MainPermissionByUserRoleandPageUrl | yes | no |
| DoctorModule_UI_AppMonitoringList.gv_DocumentUpload_PreRender | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:94 | object sender, EventArgs e | DoctorModule_UI_AppMonitoringList.gv_DocumentUpload_PreRender | no | no |
| DoctorModule_UI_AppMonitoringList.LoadData | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:106 |  | DoctorModule_UI_AppMonitoringList.LoadData → RSMSetupDal.Get_AppMonitoringList_new → SP: AppMonitoringViewList_new | no | no |
| DoctorModule_UI_AppMonitoringList.loadGridView_RowCommand | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:141 | object sender, GridViewCommandEventArgs e | DoctorModule_UI_AppMonitoringList.loadGridView_RowCommand | no | no |
| DoctorModule_UI_AppMonitoringList.btnSearch_Click | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:153 | object sender, EventArgs e | DoctorModule_UI_AppMonitoringList.btnSearch_Click → RSMSetupDal.Get_AppMonitoringList_new → SP: AppMonitoringViewList_new | no | no |
| DoctorModule_UI_AppMonitoringList.resetBtn_Click | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs:158 | object sender, EventArgs e | DoctorModule_UI_AppMonitoringList.resetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetEmployee_All | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:207 | delegates / no SQL literal |  |
| SeedDataDAL.GetUserRoleList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:707 | sp_Get_UserRoleInfo |  |
| RSMSetupDal.Get_AppMonitoringList_new | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:328 | AppMonitoringViewList_new | @Parameter |
| CommonDataLoad.GetPermissionForUserRole | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:18 | sp_GET_MainPermissionByUserRoleandPageUrl | @PageName, @RoleId |
| CommonDataLoad.NormalizePageUrl | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:89 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| AppMonitoringViewList_new | RSMSetupDal.Get_AppMonitoringList_new | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MainPermissionByUserRoleandPageUrl | CommonDataLoad.GetPermissionForUserRole | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeList | SeedDataDAL.GetEmployee_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | SeedDataDAL.GetUserRoleList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserRoleID
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 8 methods.
- Redirect targets: ../Dashboard_UI/DashboardOne.aspx, UserSetup.aspx?MID=, ~/user-records
- User messages: Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
RSMSetupDal

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, RSMSetupDal, SeedDataDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/AppMonitoringList.aspx`, `Solution.Web/DoctorModule_UI/AppMonitoringList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 32 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/AppMonitoringList.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M17 (SAP, Mobile-App & External Integration).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-173, PAGE-464, PAGE-549, PAGE-562, PAGE-563, PAGE-564, PAGE-571, PAGE-582, PAGE-583, PAGE-588, PAGE-601, PAGE-610 …
- Changing DAL methods CommonDataLoad.GetPermissionForUserRole, CommonDataLoad.NormalizePageUrl, RSMSetupDal.Get_AppMonitoringList_new, SeedDataDAL.GetEmployee_All, SeedDataDAL.GetUserRoleList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
