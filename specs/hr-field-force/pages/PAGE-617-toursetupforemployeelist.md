---
page_id: PAGE-617
module: M15
page: TourSetupForEmployeeList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-617 — TourSetupForEmployeeList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-617 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | TourSetupForEmployeeList |
| Kind | List / View (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_TourSetupForEmployeeList |
| aspx size | 10719 bytes |
| Code-behind LOC | 187 |
| Inline script size | 2952 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TourSetupForEmployeeList** (List / View). Page title/heading text: `Tour Type Setup List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/TourSetupForEmployee.aspx`

Outbound references: `customerentry.aspx`, `groupwisepromoqtyentry.aspx`, `groupwisepromoqtyview.aspx`, `toursetupforemployee.aspx`

## 5. Route
`~/MasterSetup_UI/TourSetupForEmployeeList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | TourSetupForEmployeeDAL (Library.DAL/MasterSetup_DAL/TourSetupForEmployeeDAL.cs) |
| DAO/entity class | TourTypeSetupDAO (Library.DAO/MasterSetup_DAO/TourTypeSetupDAO.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlMIO | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Tour Type | no validator found |  |  |  |  |
| (no id) | BoundField | Role Type | no validator found |  |  |  |  |
| (no id) | TemplateField | Count | no validator found |  |  |  |  |
| txtCountNo | TextBox | <%#Eval("CountNo") %> | no validator found |  | ReadOnly=true |  |  |
| FilteredTssextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| lbtUpDate | LinkButton |  | no validator found |  | Visible=false |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| txtCountNo | TourTypeSetupDAO.CountNo | MasterSetup_UI_TourSetupForEmployeeList.loadGridView_RowCommand | (none — page calls DAL directly) | TourSetupForEmployeeDAL.SaveDAInfo | @CountNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlMIO | param | MasterSetup_UI_TourSetupForEmployeeList.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlMIO | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

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
| MasterSetup_UI_TourSetupForEmployeeList.Page_Load | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:25 | object sender, EventArgs e | MasterSetup_UI_TourSetupForEmployeeList.Page_Load → CommonDataLoad.GetEmployeeList_Active / TourSetupForEmployeeDAL.GetList → SP: sp_CS_GetEmpGeneralInfo_All, sp_Get_TourSetupEmployeeList | no | no |
| MasterSetup_UI_TourSetupForEmployeeList.LoadInitialInfo | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:35 |  | MasterSetup_UI_TourSetupForEmployeeList.LoadInitialInfo → CommonDataLoad.GetEmployeeList_Active → SP: sp_CS_GetEmpGeneralInfo_All | yes | no |
| MasterSetup_UI_TourSetupForEmployeeList.gv_DocumentUpload_PreRender | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:91 | object sender, EventArgs e | MasterSetup_UI_TourSetupForEmployeeList.gv_DocumentUpload_PreRender | no | no |
| MasterSetup_UI_TourSetupForEmployeeList.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:102 | object sender, EventArgs e | MasterSetup_UI_TourSetupForEmployeeList.EmpCetegoryAddImageButton_Click | no | no |
| MasterSetup_UI_TourSetupForEmployeeList.LoadData | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:107 | string parm | MasterSetup_UI_TourSetupForEmployeeList.LoadData → TourSetupForEmployeeDAL.GetList → SP: sp_Get_TourSetupEmployeeList | no | no |
| MasterSetup_UI_TourSetupForEmployeeList.loadGridView_RowCommand | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:123 | object sender, GridViewCommandEventArgs e | MasterSetup_UI_TourSetupForEmployeeList.loadGridView_RowCommand → TourSetupForEmployeeDAL.SaveDAInfo → SP: sp_Check_TourSetupEmployeeList, sp_Check_TourSetupEmployeeListRoleType, sp_Save_TourSetupEmployee, sp_Update_TourSetupEmployee | no | yes |
| MasterSetup_UI_TourSetupForEmployeeList.btnSearch_Click | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:163 | object sender, EventArgs e | MasterSetup_UI_TourSetupForEmployeeList.btnSearch_Click → TourSetupForEmployeeDAL.GetList → SP: sp_Get_TourSetupEmployeeList | no | no |
| MasterSetup_UI_TourSetupForEmployeeList.resetBtn_Click | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs:184 | object sender, EventArgs e | MasterSetup_UI_TourSetupForEmployeeList.resetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| TourSetupForEmployeeDAL.GetList | Library.DAL/MasterSetup_DAL/TourSetupForEmployeeDAL.cs:18 | sp_Get_TourSetupEmployeeList | @Parm |
| TourSetupForEmployeeDAL.SaveDAInfo | Library.DAL/MasterSetup_DAL/TourSetupForEmployeeDAL.cs:39 | sp_Check_TourSetupEmployeeList, sp_Check_TourSetupEmployeeListRoleType, sp_Save_ | @CountNo, @EmpInfoId, @EntryBy, @EntryDate, @IsEmployeeWise, @IsRoleWise, @RoleTypeId, @StationTypeI |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Check_TourSetupEmployeeList | TourSetupForEmployeeDAL.SaveDAInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Check_TourSetupEmployeeListRoleType | TourSetupForEmployeeDAL.SaveDAInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourSetupEmployeeList | TourSetupForEmployeeDAL.GetList | body NOT in repository | read-like name (INFERRED) |
| sp_Save_TourSetupEmployee | TourSetupForEmployeeDAL.SaveDAInfo | body NOT in repository | write-like name |
| sp_Update_TourSetupEmployee | TourSetupForEmployeeDAL.SaveDAInfo | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TourSetupForEmployeeList.aspx'); | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 11 methods.
- Redirect targets: CustomerEntry.aspx, GroupWisePromoQtyView.aspx
- User messages: ','Success','TourSetupForEmployeeList.aspx');; Already Exist!; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, TourSetupForEmployeeDAL; DAO: TourTypeSetupDAO; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx`, `Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 19 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/TourSetupForEmployeeList.aspx |
| Data access | This document §15–17 | 6 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 6 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-548, PAGE-549, PAGE-555, PAGE-557, PAGE-562, PAGE-563, PAGE-564, PAGE-570, PAGE-571, PAGE-578, PAGE-582, PAGE-587, PAGE-588, PAGE-610, PAGE-616 …
- Changing DAL methods CommonDataLoad.GetEmployeeList_Active, TourSetupForEmployeeDAL.GetList, TourSetupForEmployeeDAL.SaveDAInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
