---
page_id: PAGE-544
module: M14
page: TerritoryWiseTargetSetup
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-544 — TerritoryWiseTargetSetup

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-544 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | TerritoryWiseTargetSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DWSP |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DWSP_TerritoryWiseTargetSetup |
| aspx size | 17557 bytes |
| Code-behind LOC | 560 |
| Inline script size | 1375 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TerritoryWiseTargetSetup** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/TerritoryTargetSetupView.aspx`

Outbound references: `territorytargetsetupview.aspx`

## 5. Route
`~/DWSP/TerritoryWiseTargetSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx |
| Code-behind | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | AreaWiseTargetSetupDal (Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs) |
| DAL class | CommonDal (Library.DAL/DWSP_DAL/CommonDal.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | ZoneWiseTargetSetupDal (Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | TerritoryTargetSetupDao (Library.DAO/DWSP_DAO/AreaWiseTargetSetupDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlmonth_SelectedIndexChanged |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlmonth_SelectedIndexChanged |
| groupname | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→groupname_SelectedIndexChanged1 |
| hfAreaId | HiddenField |  | no validator found |  |  |  |  |
| hfZoneId | HiddenField |  | no validator found |  |  |  |  |
| ddlZone | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlZone_OnSelectedIndexChanged |
| ddlArea | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlArea_SelectedIndexChanged |
| amount | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| FilteredTextBoxExtender8 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| gv_List | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| HFTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | TemplateField | Amount | no validator found |  |  |  |  |
| txtAmount | TextBox | <%#Eval("Amount") %> | no validator found |  |  |  | OnTextChanged→txtAmount_OnTextChanged |
| FilteredTextBoxExtender8 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | TerritoryTargetSetupDao.Year | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @Year | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlmonth | TerritoryTargetSetupDao.Month | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @Month | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | TerritoryTargetSetupDao.GroupId | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlZone | TerritoryTargetSetupDao.RegionId | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlArea | TerritoryTargetSetupDao.AreaId | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| amount | TerritoryTargetSetupDao.TargetAmount | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @TargetAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| HFTerritoryId | TerritoryTargetSetupDao.TerritoryId | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | TerritoryTargetSetupDao.Amount | DWSP_TerritoryWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | dt | DWSP_TerritoryWiseTargetSetup.groupname_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDataLoad.GetArea_ByZoneId_Active, | UNKNOWN | UNKNOWN | UNKNOWN |
| hfZoneId | SelectedValue | DWSP_TerritoryWiseTargetSetup.groupname_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDataLoad.GetArea_ByZoneId_Active, | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_TerritoryWiseTargetSetup.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtTargetAmount | DWSP_TerritoryWiseTargetSetup.NewMethod | — | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlZone | dt | DWSP_TerritoryWiseTargetSetup.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDataLoad.GetArea_ByZoneId_Active, ZoneWiseTargetSetupDal.Get_TerriTargetAmount | UNKNOWN | UNKNOWN | UNKNOWN |
| hfAreaId | SelectedValue | DWSP_TerritoryWiseTargetSetup.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDataLoad.GetArea_ByZoneId_Active, ZoneWiseTargetSetupDal.Get_TerriTargetAmount | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlArea | ds | DWSP_TerritoryWiseTargetSetup.ddlArea_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | UNKNOWN | UNKNOWN | UNKNOWN |

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
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DWSP_TerritoryWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:245 | — | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | sp_Save_TerritoryWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DWSP_TerritoryWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:245 | — | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | sp_Save_TerritoryWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DWSP_TerritoryWiseTargetSetup.Unnamed_Click | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:294 | — | — | — | read-only / none detected |

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
| DWSP_TerritoryWiseTargetSetup.Page_Load | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:25 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.Page_Load → CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_ZoneinfoByEmpId / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_Zone_All_Active_ByGroup, sp_Get | yes | no |
| DWSP_TerritoryWiseTargetSetup.LoadInitialInfo | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:53 |  | DWSP_TerritoryWiseTargetSetup.LoadInitialInfo → CommonDal.GetTeritory_ByAreaId_All / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_ZoneinfoByEmpId / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Act | yes | no |
| DWSP_TerritoryWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:245 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.btnSave_Click → AreaWiseTargetSetupDal.SaveTerritoryWiseTarget → SP: sp_Save_TerritoryWiseTargetSetup | no | yes |
| DWSP_TerritoryWiseTargetSetup.Unnamed_Click | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:294 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.Unnamed_Click | no | no |
| DWSP_TerritoryWiseTargetSetup.gv_DocumentUpload_PreRender | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:297 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.gv_DocumentUpload_PreRender | no | no |
| DWSP_TerritoryWiseTargetSetup.groupname_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:309 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.groupname_SelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_All / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDataLoad.GetArea_ByZoneId_Active / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_CS_GetAr | yes | no |
| DWSP_TerritoryWiseTargetSetup.txtAmount_OnTextChanged | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:364 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.txtAmount_OnTextChanged | no | no |
| DWSP_TerritoryWiseTargetSetup.NewMethod | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:424 |  | DWSP_TerritoryWiseTargetSetup.NewMethod → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | yes | no |
| DWSP_TerritoryWiseTargetSetup.ddlZone_OnSelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:445 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.ddlZone_OnSelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_All / CommonDataLoad.GetArea_ByZoneId_Active / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_AreaTargetAmount, s | yes | no |
| DWSP_TerritoryWiseTargetSetup.ddlYear_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:496 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.ddlYear_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | no | no |
| DWSP_TerritoryWiseTargetSetup.groupname_SelectedIndexChanged1 | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:501 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.groupname_SelectedIndexChanged1 → CommonDal.GetTeritory_ByAreaId_All / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDataLoad.GetArea_ByZoneId_Active / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_CS_GetA | no | no |
| DWSP_TerritoryWiseTargetSetup.ddlArea_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:507 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.ddlArea_SelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_All / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount, sp_Get_TerritoryAll_ByAreaId | yes | no |
| DWSP_TerritoryWiseTargetSetup.ddlmonth_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs:556 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetup.ddlmonth_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | no | no |

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
| CommonDal.GetTeritory_ByAreaId_All | Library.DAL/DWSP_DAL/CommonDal.cs:159 | sp_Get_TerritoryAll_ByAreaId | @id |
| AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs:58 | sp_Save_TerritoryWiseTargetSetup | @Amount, @AreaId, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @TerritoryId, @TerritoryWTSe |
| ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:188 | sp_Get_AreaTargetAmount | @RegionId, @Year, @month |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaTargetAmount | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryAll_ByAreaId | CommonDal.GetTeritory_ByAreaId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active_ByGroup | CommonDal.Get_Forddl_ZoneActive_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneinfoByEmpId | CommonDal.Get_ZoneinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_NTS_Group_Active | CommonDal.Get_Group_All | body NOT in repository | write-like name |
| sp_Save_TerritoryWiseTargetSetup | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Table can not be Empty! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TerritoryTargetSetupView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler groupname_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Target Amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler ddlZone_OnSelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 6 of 17 methods.
- Redirect targets: none
- User messages: ','Success','TerritoryTargetSetupView.aspx');; Already Exist!; Amount must be equal with target amount; Please Select From List; Please Select Target Amount; Table can not be Empty!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: AreaWiseTargetSetupDal, CommonDal, CommonDataLoad, ZoneWiseTargetSetupDal; DAO: ResultInfo, TerritoryTargetSetupDao; Tables (inline): —

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
| TC-07 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx`, `Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/DWSP/TerritoryWiseTargetSetup.aspx |
| Data access | This document §15–17 | 7 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 7 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M14 (Target & DWSP Management).
- Other pages sharing this page's stored procedures: PAGE-095, PAGE-463, PAGE-538, PAGE-539, PAGE-540, PAGE-541, PAGE-542, PAGE-543, PAGE-545, PAGE-546, PAGE-547, PAGE-557, PAGE-587, PAGE-613, PAGE-615 …
- Changing DAL methods AreaWiseTargetSetupDal.SaveTerritoryWiseTarget, CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDal.Get_Group_All, CommonDal.Get_ZoneinfoByEmpId affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
