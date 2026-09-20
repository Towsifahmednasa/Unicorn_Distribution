---
page_id: PAGE-543
module: M14
page: TerritoryTargetSetupView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-543 — TerritoryTargetSetupView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-543 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | TerritoryTargetSetupView |
| Kind | List / View (INFERRED from name) |
| Web folder | DWSP |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DWSP_TerritoryTargetSetupView |
| aspx size | 17719 bytes |
| Code-behind LOC | 631 |
| Inline script size | 1375 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TerritoryTargetSetupView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/TerritoryWiseTargetSetup.aspx`

Outbound references: `areawisetargetsetupview.aspx`, `groupwisepromoqtyview.aspx`, `territorywisetargetsetup.aspx`, `zonewisetargetsetupview.aspx`

## 5. Route
`~/DWSP/TerritoryTargetSetupView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/TerritoryTargetSetupView.aspx |
| Code-behind | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | AreaWiseTargetSetupDal (Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs) |
| DAL class | CommonDal (Library.DAL/DWSP_DAL/CommonDal.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | ZoneWiseTargetSetupDal (Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs) |
| DAO/entity class | AreaWiseTargetSetupDao (Library.DAO/DWSP_DAO/AreaWiseTargetSetupDao.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| groupname | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→groupname_SelectedIndexChanged |
| ddlZone | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlZone_SelectedIndexChanged |
| hfAreaId | HiddenField |  | no validator found |  |  |  |  |
| hfZoneId | HiddenField |  | no validator found |  |  |  |  |
| ddlArea | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlArea_SelectedIndexChanged |
| amount | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| gv_List | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| HFTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | TemplateField | Amount | no validator found |  |  |  |  |
| txtAmount | TextBox | <%#Eval("Amount") %> | no validator found |  | ReadOnly=true |  | OnTextChanged→txtAmount_OnTextChanged |
| FilteredTextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| btnEdit | LinkButton |  | no validator found |  | Visible=false |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | AreaWiseTargetSetupDao.Year | DWSP_TerritoryTargetSetupView.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Year | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlmonth | AreaWiseTargetSetupDao.Month | DWSP_TerritoryTargetSetupView.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Month | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | AreaWiseTargetSetupDao.GroupId | DWSP_TerritoryTargetSetupView.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlZone | AreaWiseTargetSetupDao.RegionId | DWSP_TerritoryTargetSetupView.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| amount | AreaWiseTargetSetupDao.TargetAmount | DWSP_TerritoryTargetSetupView.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @TargetAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | AreaWiseTargetSetupDao.Amount | DWSP_TerritoryTargetSetupView.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | dt | DWSP_TerritoryTargetSetupView.groupname_SelectedIndexChanged | — | CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| hfZoneId | SelectedValue | DWSP_TerritoryTargetSetupView.groupname_SelectedIndexChanged | — | CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_TerritoryTargetSetupView.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlZone | dt | DWSP_TerritoryTargetSetupView.ddlZone_OnSelectedIndexChanged | — | AreaWiseTargetSetupDal.Get_TerritoryDataAll, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| hfAreaId | SelectedValue | DWSP_TerritoryTargetSetupView.ddlZone_OnSelectedIndexChanged | — | AreaWiseTargetSetupDal.Get_TerritoryDataAll, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | param | DWSP_TerritoryTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | param | DWSP_TerritoryTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| groupname | param | DWSP_TerritoryTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlZone | param | DWSP_TerritoryTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlArea | param | DWSP_TerritoryTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtTargetAmount | DWSP_TerritoryTargetSetupView.NewMethod | — | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_TerritoryTargetSetupView.UpDateValidation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| txtAmount | AreaWiseTargetSetupDao.Amount | DWSP_TerritoryTargetSetupView.loadGridView_RowCommand | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlZone | dt | DWSP_TerritoryTargetSetupView.ddlZone_SelectedIndexChanged | — | AreaWiseTargetSetupDal.Get_TerritoryDataAll, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| hfAreaId | SelectedValue | DWSP_TerritoryTargetSetupView.ddlZone_SelectedIndexChanged | — | AreaWiseTargetSetupDal.Get_TerritoryDataAll, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| groupname | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlZone | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlArea | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DWSP_TerritoryTargetSetupView.Page_Load | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:23 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.Page_Load → CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_ZoneinfoByEmpId / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_Zone_All_Active_ByGroup, sp_Get | yes | no |
| DWSP_TerritoryTargetSetupView.LoadInitialInfo | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:40 |  | DWSP_TerritoryTargetSetupView.LoadInitialInfo → CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_ZoneinfoByEmpId / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_Zone_All_Active_ByGroup,  | yes | no |
| DWSP_TerritoryTargetSetupView.btnSave_Click | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:178 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.btnSave_Click → AreaWiseTargetSetupDal.SaveZoneWiseTarget → SP: sp_Save_AreaWiseTargetSetup | no | yes |
| DWSP_TerritoryTargetSetupView.Unnamed_Click | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:226 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.Unnamed_Click | no | no |
| DWSP_TerritoryTargetSetupView.gv_DocumentUpload_PreRender | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:229 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.gv_DocumentUpload_PreRender | no | no |
| DWSP_TerritoryTargetSetupView.groupname_SelectedIndexChanged | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:241 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.groupname_SelectedIndexChanged → CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_Zone_All_Active_ByGroup | yes | no |
| DWSP_TerritoryTargetSetupView.txtAmount_OnTextChanged | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:285 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.txtAmount_OnTextChanged | no | no |
| DWSP_TerritoryTargetSetupView.ddlZone_OnSelectedIndexChanged | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:345 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.ddlZone_OnSelectedIndexChanged → AreaWiseTargetSetupDal.Get_TerritoryDataAll / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_TerritoryWiseTargetList | yes | no |
| DWSP_TerritoryTargetSetupView.btnSearch_Click | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:385 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.btnSearch_Click → AreaWiseTargetSetupDal.Get_TerritoryDataAll / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount, sp_Get_TerritoryWiseTargetList | no | no |
| DWSP_TerritoryTargetSetupView.resetBtn_Click | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:426 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.resetBtn_Click | no | no |
| DWSP_TerritoryTargetSetupView.NewMethod | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:431 |  | DWSP_TerritoryTargetSetupView.NewMethod → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | yes | no |
| DWSP_TerritoryTargetSetupView.GET_DataList | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:453 |  | DWSP_TerritoryTargetSetupView.GET_DataList → AreaWiseTargetSetupDal.Get_TerritoryDataAll / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount, sp_Get_TerritoryWiseTargetList | no | no |
| DWSP_TerritoryTargetSetupView.loadGridView_RowCommand | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:558 | object sender, GridViewCommandEventArgs e | DWSP_TerritoryTargetSetupView.loadGridView_RowCommand → AreaWiseTargetSetupDal.SaveZoneWiseTarget → SP: sp_Save_AreaWiseTargetSetup | no | yes |
| DWSP_TerritoryTargetSetupView.ddlZone_SelectedIndexChanged | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:598 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.ddlZone_SelectedIndexChanged → AreaWiseTargetSetupDal.Get_TerritoryDataAll / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_TerritoryWiseTargetList | yes | no |
| DWSP_TerritoryTargetSetupView.ddlArea_SelectedIndexChanged | Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs:627 | object sender, EventArgs e | DWSP_TerritoryTargetSetupView.ddlArea_SelectedIndexChanged → AreaWiseTargetSetupDal.Get_TerritoryDataAll / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount, sp_Get_TerritoryWiseTargetList | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDal.Get_Group_All | Library.DAL/DWSP_DAL/CommonDal.cs:38 | sp_NTS_Group_Active |  |
| CommonDal.Get_ZoneinfoByEmpId | Library.DAL/DWSP_DAL/CommonDal.cs:97 | sp_Get_ZoneinfoByEmpId | @EmpId |
| CommonDal.Get_Forddl_ZoneActive_All_ByGroup | Library.DAL/DWSP_DAL/CommonDal.cs:77 | sp_Get_Zone_All_Active_ByGroup | @GroupId |
| CommonDataLoad.GetArea_ByZoneId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1131 | sp_CS_GetArea_ByZoneId_Active | @id |
| AreaWiseTargetSetupDal.SaveZoneWiseTarget | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs:17 | sp_Save_AreaWiseTargetSetup | @Amount, @AreaId, @AreaWTSetupId, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @Year |
| AreaWiseTargetSetupDal.Get_TerritoryDataAll | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs:159 | sp_Get_TerritoryWiseTargetList | @Parameter |
| ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:188 | sp_Get_AreaTargetAmount | @RegionId, @Year, @month |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaTargetAmount | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryWiseTargetList | AreaWiseTargetSetupDal.Get_TerritoryDataAll | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active_ByGroup | CommonDal.Get_Forddl_ZoneActive_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneinfoByEmpId | CommonDal.Get_ZoneinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_NTS_Group_Active | CommonDal.Get_Group_All | body NOT in repository | write-like name |
| sp_Save_AreaWiseTargetSetup | AreaWiseTargetSetupDal.SaveZoneWiseTarget | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','ZoneWiseTargetSetupView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler groupname_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Target Amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler ddlZone_OnSelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler GET_DataList) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Data Not Found | UNKNOWN |
| (handler UpDateValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler UpDateValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 is-invalid | UNKNOWN |
| (handler UpDateValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','AreaWiseTargetSetupView.aspx'); | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler ddlZone_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 6 of 21 methods.
- Redirect targets: none
- User messages: ','Success','AreaWiseTargetSetupView.aspx');; ','Success','ZoneWiseTargetSetupView.aspx');; Already Exist!; Amount must be equal with target amount; Data Not Found; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: AreaWiseTargetSetupDal, CommonDal, CommonDataLoad, ZoneWiseTargetSetupDal; DAO: AreaWiseTargetSetupDao, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/TerritoryTargetSetupView.aspx`, `Solution.Web/DWSP/TerritoryTargetSetupView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DWSP/TerritoryTargetSetupView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 25 controls parsed | n/a | PARTIAL | Solution.Web/DWSP/TerritoryTargetSetupView.aspx |
| Data access | This document §15–17 | 7 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 7 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M14 (Target & DWSP Management).
- Other pages sharing this page's stored procedures: PAGE-095, PAGE-463, PAGE-538, PAGE-539, PAGE-540, PAGE-541, PAGE-542, PAGE-544, PAGE-545, PAGE-546, PAGE-547, PAGE-557, PAGE-587, PAGE-613, PAGE-615 …
- Changing DAL methods AreaWiseTargetSetupDal.Get_TerritoryDataAll, AreaWiseTargetSetupDal.SaveZoneWiseTarget, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDal.Get_Group_All, CommonDal.Get_ZoneinfoByEmpId affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
