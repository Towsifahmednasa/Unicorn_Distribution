---
page_id: PAGE-545
module: M14
page: TerritoryWiseTargetSetupApps
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-545 — TerritoryWiseTargetSetupApps

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-545 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | TerritoryWiseTargetSetupApps |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DWSP |
| Master page | none (stand-alone HTML page) |
| Page class | DWSP_TerritoryWiseTargetSetupApps |
| aspx size | 50710 bytes |
| Code-behind LOC | 700 |
| Inline script size | 15428 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Stand-alone (no master page) version of the territory target page, opened with `?EmpId=<employee id>`. It lets a **DZSM or Area Manager split an area's monthly target across the area's territories (MIO territories)**. The employee id in the URL decides whose hierarchy (group/zone/area) is pre-selected and locked. The `...Apps` suffix and the missing master page suggest it is loaded inside the field-force mobile app (WebView) - INFERRED (no inbound link exists in the repo; menu is DB-driven). CONFIRMED mechanics: `Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs`.

## 3. Business Objective
Step 4 of the cascade (BR-M14-001): area target -> territory targets, and a shortcut (`btnAdd`) to the day-wise DWSP page (PAGE-538) for the same employee.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/AMDayWiseDWSPSetup.aspx`

Outbound references: `amdaywisedwspsetup.aspx`

## 5. Route
`~/DWSP/TerritoryWiseTargetSetupApps.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx |
| Code-behind | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs |
| JS include | ../VerticalAsset/js/pace.min.js |
| JS include | ../VerticalAsset/js/bootstrap.bundle.min.js |
| JS include | ../VerticalAsset/js/jquery.min.js |
| JS include | ../VerticalAsset/plugins/simplebar/js/simplebar.min.js |
| JS include | ../VerticalAsset/plugins/metismenu/js/metisMenu.min.js |
| JS include | ../VerticalAsset/plugins/perfect-scrollbar/js/perfect-scrollbar.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/legacy.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/picker.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/picker.time.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/picker.date.js |
| CSS include | https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css |
| CSS include | ../VerticalAsset/plugins/simplebar/css/simplebar.css |
| CSS include | ../VerticalAsset/plugins/perfect-scrollbar/css/perfect-scrollbar.css |
| CSS include | ../VerticalAsset/plugins/metismenu/css/metisMenu.min.css |
| CSS include | ../VerticalAsset/css/pace.min.css |
| CSS include | ../VerticalAsset/plugins/datatable/css/dataTables.bootstrap5.min.css |
| DAL class | AreaWiseTargetSetupDal (Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs) |
| DAL class | CommonDal (Library.DAL/DWSP_DAL/CommonDal.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | ZoneWiseTargetSetupDal (Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | TerritoryTargetSetupDao (Library.DAO/DWSP_DAO/AreaWiseTargetSetupDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
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
| btnSave | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnAdd | LinkButton |  | no validator found |  |  |  | OnClick→btnAdd_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | TerritoryTargetSetupDao.Year | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @Year | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlmonth | TerritoryTargetSetupDao.Month | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @Month | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | TerritoryTargetSetupDao.GroupId | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlZone | TerritoryTargetSetupDao.RegionId | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlArea | TerritoryTargetSetupDao.AreaId | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| amount | TerritoryTargetSetupDao.TargetAmount | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @TargetAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| HFTerritoryId | TerritoryTargetSetupDao.TerritoryId | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | TerritoryTargetSetupDao.Amount | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | dt | DWSP_TerritoryWiseTargetSetupApps.groupname_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDal.Get_RoleTypeEmpId, CommonData | UNKNOWN | UNKNOWN | UNKNOWN |
| hfZoneId | SelectedValue | DWSP_TerritoryWiseTargetSetupApps.groupname_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDal.Get_RoleTypeEmpId, CommonData | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_TerritoryWiseTargetSetupApps.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtTargetAmount | DWSP_TerritoryWiseTargetSetupApps.NewMethod | — | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlZone | dt | DWSP_TerritoryWiseTargetSetupApps.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_RoleTypeEmpId, CommonDataLoad.GetArea_ByZoneId_Active, ZoneWiseTargetS | UNKNOWN | UNKNOWN | UNKNOWN |
| hfAreaId | SelectedValue | DWSP_TerritoryWiseTargetSetupApps.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, CommonDal.Get_RoleTypeEmpId, CommonDataLoad.GetArea_ByZoneId_Active, ZoneWiseTargetS | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlArea | ds | DWSP_TerritoryWiseTargetSetupApps.ddlArea_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_All, ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | UNKNOWN | UNKNOWN | UNKNOWN |

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
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:275 | — | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | sp_Save_TerritoryWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DWSP_TerritoryWiseTargetSetupApps.Unnamed_Click | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:326 | — | — | — | read-only / none detected |
| btnAdd | LinkButton click | (postback) | DWSP_TerritoryWiseTargetSetupApps.btnAdd_Click | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:697 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- No master page; page is a full HTML document. `Page_Load` (first request): fills Month/Year; if `?EmpId=` present -> `EmpId = Request.QueryString["EmpId"]`, `LoadInitialInfo()`; else shows `btnSave` (`...Apps.aspx.cs:29-58`).
- `LoadInitialInfo`: groups from `sp_NTS_Group_Active`; `sp_Get_RoleTypeByEmpId(@EmpId)` -> `RoleTypeId`/`UserId`. Role `2` (AM): `UserId` remembered, zone/area/group from `sp_Get_ZoneinfoByEmpId`, group locked, cascades. Role `3` (DZSM): `sp_Get_DZSMinfoByEmpId`, group locked.
- Group/Zone/Area handlers cascade: zone list (`sp_Get_Zone_All_Active_ByGroup` or DZSM-limited `sp_Get_Zone_ForDSM`), areas (`sp_CS_GetArea_ByZoneId_Active`), then territories of the area (`GetTeritory_ByAreaId_All` -> `sp_Get_TerritoryAll_ByAreaId`), area target (`sp_Get_AreaTargetAmount`) into read-only `amount`.
- `txtAmount_OnTextChanged`: running-sum cap (BR-M14-006). `btnAdd` -> redirect `AMDayWiseDWSPSetup.aspx?EmpId=`; Reset -> reload same URL.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DWSP_TerritoryWiseTargetSetupApps.Page_Load | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:30 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.Page_Load → CommonDal.Get_DZSMinfoByEmpId / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_RoleTypeEmpId / CommonDal.Get_ZoneinfoByEmpId / CommonDataLoad.GetArea_ByZoneId_Ac → SP: sp_CS_G | yes | no |
| DWSP_TerritoryWiseTargetSetupApps.LoadInitialInfo | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:60 |  | DWSP_TerritoryWiseTargetSetupApps.LoadInitialInfo → CommonDal.GetTeritory_ByAreaId_All / CommonDal.Get_DZSMinfoByEmpId / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_RoleTypeEmpId / CommonDal.Get_ZoneinfoByEmpId → SP: s | yes | no |
| DWSP_TerritoryWiseTargetSetupApps.btnSave_Click | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:275 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.btnSave_Click → AreaWiseTargetSetupDal.SaveTerritoryWiseTarget → SP: sp_Save_TerritoryWiseTargetSetup | no | yes |
| DWSP_TerritoryWiseTargetSetupApps.Unnamed_Click | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:326 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.Unnamed_Click | no | no |
| DWSP_TerritoryWiseTargetSetupApps.gv_DocumentUpload_PreRender | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:331 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.gv_DocumentUpload_PreRender | no | no |
| DWSP_TerritoryWiseTargetSetupApps.groupname_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:343 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.groupname_SelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_All / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_RoleTypeEmpId / CommonDataLoad.GetArea_ByZoneId_Active / CommonDataLoad.GetHigharcyInfoByEmploye | yes | no |
| DWSP_TerritoryWiseTargetSetupApps.txtAmount_OnTextChanged | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:503 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.txtAmount_OnTextChanged | no | no |
| DWSP_TerritoryWiseTargetSetupApps.NewMethod | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:563 |  | DWSP_TerritoryWiseTargetSetupApps.NewMethod → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | yes | no |
| DWSP_TerritoryWiseTargetSetupApps.ddlZone_OnSelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:585 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.ddlZone_OnSelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_All / CommonDal.Get_RoleTypeEmpId / CommonDataLoad.GetArea_ByZoneId_Active / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_CS_GetArea_ByZoneId_ | yes | no |
| DWSP_TerritoryWiseTargetSetupApps.ddlYear_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:638 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.ddlYear_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | no | no |
| DWSP_TerritoryWiseTargetSetupApps.groupname_SelectedIndexChanged1 | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:643 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.groupname_SelectedIndexChanged1 → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | no | no |
| DWSP_TerritoryWiseTargetSetupApps.ddlArea_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:649 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.ddlArea_SelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_All / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount, sp_Get_TerritoryAll_ByAreaId | no | no |
| DWSP_TerritoryWiseTargetSetupApps.ddlmonth_SelectedIndexChanged | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:691 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.ddlmonth_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea → SP: sp_Get_AreaTargetAmount | no | no |
| DWSP_TerritoryWiseTargetSetupApps.btnAdd_Click | Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs:697 | object sender, EventArgs e | DWSP_TerritoryWiseTargetSetupApps.btnAdd_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDal.Get_Group_All | Library.DAL/DWSP_DAL/CommonDal.cs:38 | sp_NTS_Group_Active |  |
| CommonDal.Get_RoleTypeEmpId | Library.DAL/DWSP_DAL/CommonDal.cs:18 | sp_Get_RoleTypeByEmpId | @EmpId |
| CommonDal.Get_ZoneinfoByEmpId | Library.DAL/DWSP_DAL/CommonDal.cs:97 | sp_Get_ZoneinfoByEmpId | @EmpId |
| CommonDal.Get_DZSMinfoByEmpId | Library.DAL/DWSP_DAL/CommonDal.cs:116 | sp_Get_DZSMinfoByEmpId | @EmpId |
| CommonDal.Get_Forddl_ZoneActive_All_ByGroup | Library.DAL/DWSP_DAL/CommonDal.cs:77 | sp_Get_Zone_All_Active_ByGroup | @GroupId |
| CommonDataLoad.GetHigharcyInfoByEmployeeId | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1866 | sp_HigharchyInfoByEmployeeId | @EmployeeId, @RoleId |
| CommonDataLoad.GetZone_byGroupId_forDSM | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1275 | sp_Get_Zone_ForDSM | @GroupId |
| CommonDataLoad.GetArea_ByZoneId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1131 | sp_CS_GetArea_ByZoneId_Active | @id |
| CommonDal.GetTeritory_ByAreaId_All | Library.DAL/DWSP_DAL/CommonDal.cs:159 | sp_Get_TerritoryAll_ByAreaId | @id |
| AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs:58 | sp_Save_TerritoryWiseTargetSetup | @Amount, @AreaId, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @TerritoryId, @TerritoryWTSe |
| ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:188 | sp_Get_AreaTargetAmount | @RegionId, @Year, @month |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaTargetAmount | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DZSMinfoByEmpId | CommonDal.Get_DZSMinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_RoleTypeByEmpId | CommonDal.Get_RoleTypeEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryAll_ByAreaId | CommonDal.GetTeritory_ByAreaId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active_ByGroup | CommonDal.Get_Forddl_ZoneActive_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_ForDSM | CommonDataLoad.GetZone_byGroupId_forDSM | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneinfoByEmpId | CommonDal.Get_ZoneinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_HigharchyInfoByEmployeeId | CommonDataLoad.GetHigharcyInfoByEmployeeId | body NOT in repository | read-like name (INFERRED) |
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
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TerritoryWiseTargetSetupApps.aspx?EmpId= | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler groupname_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Target Amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler ddlZone_OnSelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-002 | Year drop-down is a fixed list 2015-2050, default = current year. Month drop-down lists 12 entries starting from the *current calendar month* and wrapping (e.g. in September: September ... August); each item's Text is the month name and Value is the calendar month number; the current month is pre-selected. | `DWSP/AreaWiseTargetSetup.aspx.cs:187-215` (same `GetYearList`/`GetMonthList` copy in every DWSP page) | code-behind | CONFIRMED |
| BR-M14-003 | Except on the DWSP day-wise page, the month is persisted as the month **name** text (`ddlmonth.SelectedItem.Text`, e.g. `September`) plus a text year; the DWSP page and the monthly report use the month **number**. The two conventions meet only inside SPs whose bodies are not in the repo. | `DWSP/AreaWiseTargetSetup.aspx.cs:317-352`; `DWSP/AMDayWiseDWSPSetup.aspx.cs:666-680` | code-behind | CONFIRMED |
| BR-M14-006 | Running-sum cap (advisory) - same logic and wording as Zone/Area pages; territory amounts cumulative `<=` area target; **no server re-check on Save**. | `...Apps.aspx.cs:502-561` | code-behind | CONFIRMED |
| BR-M14-013 | **Employee-context resolution.** `EmpId` from the URL is looked up in `sp_Get_RoleTypeByEmpId`; only `RoleTypeId` 2 and 3 are handled (others get no pre-selection). For AM (2) the zone/area are taken from `sp_Get_ZoneinfoByEmpId`; for DZSM (3) the group/zone from `sp_HigharchyInfoByEmployeeId(@EmployeeId,'3')`. Locked drop-downs (`Enabled=false`) express the scoping. | `...Apps.aspx.cs:59-143,342-501` | code-behind | CONFIRMED |
| BR-M14-014 | **Territory save.** Validation: Year, Month, Group, area target `amount` non-empty, >=1 row, all amounts non-empty (`Table can not be Empty!`). Each row -> `sp_Save_TerritoryWiseTargetSetup(@TerritoryWTSetupId=0,@Year,@Month(name),@GroupId,@TargetAmount,@RegionId,@AreaId,@TerritoryId,@Amount,@EntryBy)`. `@EntryBy` is the **employee's UserId resolved from the URL, not the logged-in user** - and because `UserId` is a page field assigned only in the first request, on the Save postback it is the empty string `""` (page instance is recreated) - see §30. Success redirects to the same page with `?EmpId=`. | `...Apps.aspx.cs:25,84,99,174-324`; `AreaWiseTargetSetupDal.cs:44-90` | code-behind + DAL | CONFIRMED |

## 20. Permission Matrix
- **No Session keys are read at all** and there is no master page, hence no `Session["UserId"]` redirect. Any client that can reach the URL with an `EmpId` can open and save (CONFIRMED by absence; forms-auth without `<authorization>` in `Web.config:80-82` means anonymous users are allowed - INFERRED from missing deny rules).
- Authorization relies on the caller knowing a valid `EmpId`. Roles other than AM/DZSM (RoleTypeId 2/3) are not blocked, they simply get an unscoped form.

## 21. Status Lifecycle
No status handled. Rows keyed by (Year, Month, Group, Region, Area, Territory); duplicates decided by SP (UNKNOWN).

## 22. Error Handling
- Nearly all handlers wrap in `try { } catch { }` and swallow errors (empty catch bodies) -> failures show as empty drop-downs.
- Save errors: DAL rolls back and rethrows; no page handler.
- Messages: `Table can not be Empty!`, `Amount must be equal with target amount`, `Please Select Target Amount`, success/`Already Exist!` popups.

## 23. Audit
`@EntryBy` receives the (empty) `UserId` field (BR-M14-014); otherwise as in the other pages: Audit columns: the page passes an `@EntryBy`-style value to the SP (see §8) but which audit columns/tables the SP populates is `UNKNOWN` (SP body not in repo). `UpdateBy/UpdateDate/EntryDate` exist on the DAO (`Library.DAO/DWSP_DAO/*.cs`) but are never set by the pages.

## 24. Integration
Likely consumed from the field-force mobile app WebView (INFERRED, see specs/integrations/INT-09). No outbound integration.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: AreaWiseTargetSetupDal, CommonDal, CommonDataLoad, ZoneWiseTargetSetupDal; DAO: ResultInfo, TerritoryTargetSetupDao; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-545-01 | Positive | Open `?EmpId=<AM id>`; pick month; split area target over territories; Save | AM's zone/area/group pre-selected+locked; rows saved; redirect to same URL | BR-M14-013/014 |
| TC-545-02 | Positive | Open `?EmpId=<DZSM id>` | group locked; zone list limited to DZSM zones | BR-M14-013 |
| TC-545-03 | Negative | Open with no `EmpId` | form loads with `btnSave` and no scoping | BR-M14-013 |
| TC-545-04 | Negative | Amounts exceed area target | row cleared + message | BR-M14-006 |
| TC-545-05 | Security | Call URL with no cookie/session | page renders and saves (no session check) | permission |
| TC-545-06 | Defect | Save, then inspect `@EntryBy` sent | empty string | BR-M14-014 |
| TC-545-07 | Edge | Click "Add" | goes to `AMDayWiseDWSPSetup.aspx?EmpId=` | - |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx`, `Solution.Web/DWSP/TerritoryWiseTargetSetupApps.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `TerritoryWiseTargetSetupApps.aspx(.cs)` | n/a | MISSING | - |
| Auth | None existed | none | n/a | RISK | §20 |
| EntryBy | None existed | empty on save | UNKNOWN | DEFECT | BR-M14-014 |

## 30. Known Gaps
- Unauthenticated form: no Session/master-page guard; identity is just the `EmpId` query string (IDOR-style).
- `UserId`/`EmpId` are instance fields set only on the first request; on postback they are `""`, so `@EntryBy` is blank and all `Get_RoleTypeEmpId(EmpId)` lookups inside postback handlers run with an empty id (`...Apps.aspx.cs:19-25,47,84`).
- Advisory-only cap (BR-M14-006). `?MID` not used here.
- Empty catch blocks everywhere; unguarded `dtMarket.Rows[0]`.
- `Get_TerriTargetAmount_All_ByTeritoryId` (used by PAGE-538) passes a *territory* id in a parameter named `@RegionId` (`ZoneWiseTargetSetupDal.cs:111-131`) - semantic mismatch, SP body UNKNOWN.
- SP bodies missing: `sp_Save_TerritoryWiseTargetSetup`, `sp_Get_AreaTargetAmount`, `sp_Get_TerritoryAll_ByAreaId`, `sp_Get_RoleTypeByEmpId`, `sp_Get_ZoneinfoByEmpId` (GAP-M14-001).

## 31. Change Impact
Territory rows written here feed `sp_Get_TerritoryWiseTargetSetup` (DWSP page PAGE-538) and PAGE-543 (view). Shares DAL with PAGE-538/539/546. If the page is really embedded in the mobile app, changing its URL/query contract breaks the app (INFERRED).

