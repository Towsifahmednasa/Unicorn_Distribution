---
page_id: PAGE-538
module: M14
page: AMDayWiseDWSPSetup
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-538 — AMDayWiseDWSPSetup

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-538 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | AMDayWiseDWSPSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DWSP |
| Master page | none (stand-alone HTML page) |
| Page class | DWSP_AMDayWiseDWSPSetup |
| aspx size | 55888 bytes |
| Code-behind LOC | 812 |
| Inline script size | 15428 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Stand-alone page (no master page), opened with `?EmpId=<id>`, on which an Area Manager / DZSM enters the **day-wise sales plan (DWSP) for one territory and month**: for every calendar day the planned sales value in three buckets - General, FCB and Campaign - with live totals. CONFIRMED: `Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs`.

## 3. Business Objective
Break the territory's monthly target (read-only `amount`, from `sp_Get_TerritoryWiseTargetSetup`) into a daily plan that the field force and the DWSP monthly report (PAGE-548) use to track daily sales. Business meaning of FCB is UNKNOWN (no expansion in code).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/TerritoryWiseTargetSetupApps.aspx`

Outbound references: `territorywisetargetsetupapps.aspx`

## 5. Route
`~/DWSP/AMDayWiseDWSPSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx |
| Code-behind | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs |
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
| ddlTerritory | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlTerritory_SelectedIndexChanged |
| amount | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| FilteredTextBoxExtender8 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| lblGeneral | Label | 0 | no validator found |  |  |  |  |
| lblFCBAmount | Label | 0 | no validator found |  |  |  |  |
| lblCampaign | Label | 0 | no validator found |  |  |  |  |
| lblAllTotal | Label | 0 | no validator found |  |  |  |  |
| gv_List | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| HFDWSPDate | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Date | no validator found |  |  |  |  |
| (no id) | TemplateField | General Amount | no validator found |  |  |  |  |
| txtGeneralAmount | TextBox | <%#Eval("GeneralAmount") %> | no validator found |  |  |  | OnTextChanged→txtGeneralAmount_TextChanged |
| hh | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| (no id) | TemplateField | FCB Amount | no validator found |  |  |  |  |
| txtFCBAmount | TextBox | <%#Eval("FCBAmount") %> | no validator found |  |  |  | OnTextChanged→txtFCBAmount_TextChanged |
| jjgj | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Campaign Amount | no validator found |  |  |  |  |
| txtCampaignAmount | TextBox | <%#Eval("CampaignAmount") %> | no validator found |  |  |  | OnTextChanged→txtCampaignAmount_TextChanged |
| fjfjfjfhjf | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnAdd | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnAdd_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| lblAllTotal | total | DWSP_AMDayWiseDWSPSetup.Validation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlTerritory | TerritoryTargetSetupDao.TerritoryId | DWSP_AMDayWiseDWSPSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.Save_DWSPMaster | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| HFDWSPDate | TerritoryTargetSetupDao.DWSPDate | DWSP_AMDayWiseDWSPSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.Save_DWSPMaster | @DWSPDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtFCBAmount | TerritoryTargetSetupDao.FCBAmount | DWSP_AMDayWiseDWSPSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.Save_DWSPMaster | @FCBAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtGeneralAmount | TerritoryTargetSetupDao.GeneralAmount | DWSP_AMDayWiseDWSPSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.Save_DWSPMaster | @GeneralAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtCampaignAmount | TerritoryTargetSetupDao.CampaignAmount | DWSP_AMDayWiseDWSPSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.Save_DWSPMaster | @CampaignAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | dt | DWSP_AMDayWiseDWSPSetup.groupname_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_IsVaccant, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDal.Get_RoleTypeEmpId, Comm | UNKNOWN | UNKNOWN | UNKNOWN |
| hfZoneId | SelectedValue | DWSP_AMDayWiseDWSPSetup.groupname_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_IsVaccant, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDal.Get_RoleTypeEmpId, Comm | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_AMDayWiseDWSPSetup.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtTargetAmount | DWSP_AMDayWiseDWSPSetup.NewMethod | — | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlZone | dt | DWSP_AMDayWiseDWSPSetup.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_IsVaccant, CommonDal.Get_RoleTypeEmpId, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| hfAreaId | SelectedValue | DWSP_AMDayWiseDWSPSetup.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_IsVaccant, CommonDal.Get_RoleTypeEmpId, CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlArea | dt | DWSP_AMDayWiseDWSPSetup.ddlArea_SelectedIndexChanged | — | CommonDal.GetTeritory_ByAreaId_IsVaccant | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtDateStarttoEnd | DWSP_AMDayWiseDWSPSetup.ddlTerritory_SelectedIndexChanged | — | ZoneWiseTargetSetupDal.GetDatewithinDateRange, ZoneWiseTargetSetupDal.Get_FirstDate_LastDatebyYearMonth, ZoneWiseTargetS | UNKNOWN | UNKNOWN | UNKNOWN |
| txtGeneralAmount | Genvalue | DWSP_AMDayWiseDWSPSetup.calculation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| txtFCBAmount | fcbvalue | DWSP_AMDayWiseDWSPSetup.calculation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| txtCampaignAmount | Campvalue | DWSP_AMDayWiseDWSPSetup.calculation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| groupname | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlZone | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlArea | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlTerritory | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DWSP_AMDayWiseDWSPSetup.btnSave_Click | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:272 | — | AreaWiseTargetSetupDal.Save_DWSPMaster | sp_Save_DWSPMaster | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DWSP_AMDayWiseDWSPSetup.Unnamed_Click | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:319 | — | — | — | read-only / none detected |
| btnAdd | LinkButton click | (postback) | DWSP_AMDayWiseDWSPSetup.btnAdd_Click | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:662 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- `Page_Load` first request: fills Month/Year; if `?EmpId=` present -> `LoadInitialInfo()` (group list; `sp_Get_RoleTypeByEmpId`; role 2 -> `sp_Get_ZoneinfoByEmpId` fills zone/area/group and locks group; role 3 -> `sp_Get_DZSMinfoByEmpId`), else only `btnSave` visible.
- Zone -> Area -> Territory cascade: territory list = `GetTeritory_ByAreaId_IsVaccant` (`sp_Get_TerritoryAll_ByAreaIdIsVaccant`). Grid is cleared when the area changes.
- `ddlTerritory_SelectedIndexChanged`: `Get_FirstDate_LastDatebyYearMonth(month,year)` (SP gets `@StartDate = yyyyMM01` and `@EndDate = yyyyMM01` - identical values, `ZoneWiseTargetSetupDal.cs:133-160`) -> `FirstDay/LastDay` -> `GetDatewithinDateRange` -> binds one grid row per day (`HFDWSPDate` hidden + three text boxes); then `NewMethod()` reads the territory monthly target into read-only `amount`.
- Each text box is AutoPostBack: `calculation()` recomputes footer labels `lblGeneral`, `lblFCBAmount`, `lblCampaign` and `lblAllTotal = general + campaign + fcb` (blank counts 0) (`:730-812`).
- `btnAdd` (hidden by default) -> `TerritoryWiseTargetSetupApps.aspx?EmpId=`; Reset reloads with `EmpId`. Client: SweetAlert confirm, numeric filter on each text box.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DWSP_AMDayWiseDWSPSetup.Page_Load | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:31 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.Page_Load → CommonDal.Get_DZSMinfoByEmpId / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_RoleTypeEmpId / CommonDal.Get_ZoneinfoByEmpId / CommonDataLoad.GetArea_ByZoneId_Ac → SP: sp_CS_GetArea_ByZ | yes | no |
| DWSP_AMDayWiseDWSPSetup.LoadInitialInfo | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:60 |  | DWSP_AMDayWiseDWSPSetup.LoadInitialInfo → CommonDal.GetTeritory_ByAreaId_IsVaccant / CommonDal.Get_DZSMinfoByEmpId / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDal.Get_RoleTypeEmpId / CommonDal.Get_ZoneinfoB → SP: sp_CS_GetAr | yes | no |
| DWSP_AMDayWiseDWSPSetup.btnSave_Click | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:272 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.btnSave_Click → AreaWiseTargetSetupDal.Save_DWSPMaster → SP: sp_Save_DWSPMaster | no | yes |
| DWSP_AMDayWiseDWSPSetup.Unnamed_Click | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:319 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.Unnamed_Click | no | no |
| DWSP_AMDayWiseDWSPSetup.gv_DocumentUpload_PreRender | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:324 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.gv_DocumentUpload_PreRender | no | no |
| DWSP_AMDayWiseDWSPSetup.groupname_SelectedIndexChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:336 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.groupname_SelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_IsVaccant / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_RoleTypeEmpId / CommonDataLoad.GetArea_ByZoneId_Active / CommonDataLoad.GetHigharcyInfoByEmployee →  | yes | no |
| DWSP_AMDayWiseDWSPSetup.txtAmount_OnTextChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:496 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.txtAmount_OnTextChanged | no | no |
| DWSP_AMDayWiseDWSPSetup.NewMethod | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:557 |  | DWSP_AMDayWiseDWSPSetup.NewMethod → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId → SP: sp_Get_TerritoryWiseTargetSetup | yes | no |
| DWSP_AMDayWiseDWSPSetup.ddlZone_OnSelectedIndexChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:579 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.ddlZone_OnSelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_IsVaccant / CommonDal.Get_RoleTypeEmpId / CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active, sp_Get_RoleTypeByEmpId, sp_Get_TerritoryAll_ByAreaIdIs | yes | no |
| DWSP_AMDayWiseDWSPSetup.ddlYear_SelectedIndexChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:623 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.ddlYear_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId → SP: sp_Get_TerritoryWiseTargetSetup | no | no |
| DWSP_AMDayWiseDWSPSetup.groupname_SelectedIndexChanged1 | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:628 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.groupname_SelectedIndexChanged1 → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId → SP: sp_Get_TerritoryWiseTargetSetup | no | no |
| DWSP_AMDayWiseDWSPSetup.ddlArea_SelectedIndexChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:634 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.ddlArea_SelectedIndexChanged → CommonDal.GetTeritory_ByAreaId_IsVaccant → SP: sp_Get_TerritoryAll_ByAreaIdIsVaccant | no | no |
| DWSP_AMDayWiseDWSPSetup.ddlmonth_SelectedIndexChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:656 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.ddlmonth_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId → SP: sp_Get_TerritoryWiseTargetSetup | no | no |
| DWSP_AMDayWiseDWSPSetup.btnAdd_Click | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:662 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.btnAdd_Click | no | no |
| DWSP_AMDayWiseDWSPSetup.ddlTerritory_SelectedIndexChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:667 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.ddlTerritory_SelectedIndexChanged → ZoneWiseTargetSetupDal.GetDatewithinDateRange / ZoneWiseTargetSetupDal.Get_FirstDate_LastDatebyYearMonth / ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId → SP: sp_GetDatewithinDateRange | yes | no |
| DWSP_AMDayWiseDWSPSetup.txtGeneralAmount_TextChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:716 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.txtGeneralAmount_TextChanged | no | no |
| DWSP_AMDayWiseDWSPSetup.txtFCBAmount_TextChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:721 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.txtFCBAmount_TextChanged | no | no |
| DWSP_AMDayWiseDWSPSetup.txtCampaignAmount_TextChanged | Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs:726 | object sender, EventArgs e | DWSP_AMDayWiseDWSPSetup.txtCampaignAmount_TextChanged | no | no |

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
| CommonDal.GetTeritory_ByAreaId_IsVaccant | Library.DAL/DWSP_DAL/CommonDal.cs:202 | sp_Get_TerritoryAll_ByAreaIdIsVaccant | @id |
| AreaWiseTargetSetupDal.Save_DWSPMaster | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs:101 | sp_Save_DWSPMaster | @CampaignAmount, @DWSPDate, @FCBAmount, @GeneralAmount, @TerritoryId, @empId |
| ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:112 | sp_Get_TerritoryWiseTargetSetup | @RegionId, @Year, @month |
| ZoneWiseTargetSetupDal.Get_FirstDate_LastDatebyYearMonth | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:134 | sp_Get_FirstDate_LastDatebyYearMonth | @EndDate, @StartDate |
| ZoneWiseTargetSetupDal.GetDatewithinDateRange | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:166 | sp_GetDatewithinDateRange | @fDate, @tDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GetDatewithinDateRange | ZoneWiseTargetSetupDal.GetDatewithinDateRange | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DZSMinfoByEmpId | CommonDal.Get_DZSMinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_FirstDate_LastDatebyYearMonth | ZoneWiseTargetSetupDal.Get_FirstDate_LastDatebyYearMonth | body NOT in repository | read-like name (INFERRED) |
| sp_Get_RoleTypeByEmpId | CommonDal.Get_RoleTypeEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryAll_ByAreaIdIsVaccant | CommonDal.GetTeritory_ByAreaId_IsVaccant | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryWiseTargetSetup | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active_ByGroup | CommonDal.Get_Forddl_ZoneActive_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_ForDSM | CommonDataLoad.GetZone_byGroupId_forDSM | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneinfoByEmpId | CommonDal.Get_ZoneinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_HigharchyInfoByEmployeeId | CommonDataLoad.GetHigharcyInfoByEmployeeId | body NOT in repository | read-like name (INFERRED) |
| sp_NTS_Group_Active | CommonDal.Get_Group_All | body NOT in repository | write-like name |
| sp_Save_DWSPMaster | AreaWiseTargetSetupDal.Save_DWSPMaster | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Table can not be Empty! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Total Target  Can not be Empty!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TerritoryWiseTargetSetupApps.aspx?EmpId= | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler groupname_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler ddlZone_OnSelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler ddlArea_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-002 | Year drop-down is a fixed list 2015-2050, default = current year. Month drop-down lists 12 entries starting from the *current calendar month* and wrapping (e.g. in September: September ... August); each item's Text is the month name and Value is the calendar month number; the current month is pre-selected. | `DWSP/AreaWiseTargetSetup.aspx.cs:187-215` (same `GetYearList`/`GetMonthList` copy in every DWSP page) | code-behind | CONFIRMED |
| BR-M14-003 | Except on the DWSP day-wise page, the month is persisted as the month **name** text (`ddlmonth.SelectedItem.Text`, e.g. `September`) plus a text year; the DWSP page and the monthly report use the month **number**. The two conventions meet only inside SPs whose bodies are not in the repo. | `DWSP/AreaWiseTargetSetup.aspx.cs:317-352`; `DWSP/AMDayWiseDWSPSetup.aspx.cs:666-680` | code-behind | CONFIRMED |
| BR-M14-015 | **DWSP entry validation.** Required: Year, Month, Territory; grid must have rows (`Table can not be Empty!`); the *sum of all General+FCB+Campaign* (`lblAllTotal`) must be non-zero (`Total Target  Can not be Empty!!` - note the double space). Individual days may be blank (saved as 0). Negative or decimal values are limited only by the client filter. | `AMDayWiseDWSPSetup.aspx.cs:174-270,730-812` | code-behind | CONFIRMED |
| BR-M14-016 | **No reconciliation to the territory target.** The read-only `amount` (territory monthly target) is displayed but never compared with `lblAllTotal` on Save; the older `txtAmount_OnTextChanged` cap references a `txtAmount` control that does not exist in this page's grid and is unused. | `AMDayWiseDWSPSetup.aspx.cs:495-555` | code-behind | CONFIRMED |
| BR-M14-017 | **DWSP save payload.** One `sp_Save_DWSPMaster` call per day row inside one transaction (BR-M14-012): `@DWSPDate`, `@empId` = `Request.QueryString["EmpId"]` (int cast - non-numeric -> unhandled exception), `@TerritoryId`, `@FCBAmount`, `@CampaignAmount`, `@GeneralAmount`. No `@EntryBy`/`@Year`/`@Month`: the DAL method ignores its `sessionUser` argument (`UserId` = employee's UserId) - audit of who entered is UNKNOWN. Success -> redirect to `TerritoryWiseTargetSetupApps.aspx?EmpId=`; failure (never true-false without exception) -> `Already Exist!`. | `AMDayWiseDWSPSetup.aspx.cs:271-317`; `AreaWiseTargetSetupDal.cs:101-131` | code-behind + DAL | CONFIRMED |
| BR-M14-018 | **Month bounds come from SP.** First/last day and day list are produced by `sp_Get_FirstDate_LastDatebyYearMonth` / `sp_GetDatewithinDateRange`; page builds the date argument as `Year + ("0" if month<10) + month + "01"` (`yyyyMM01`). Past-day locking code exists but is commented out (`:686-712`) - all days, including past ones, are editable. | `ZoneWiseTargetSetupDal.cs:133-185`; `AMDayWiseDWSPSetup.aspx.cs:666-729` | DAL + code-behind | CONFIRMED |

## 20. Permission Matrix
- **No Session key is read** in this file (CONFIRMED) and there is no master page: the page is reachable and writable by any HTTP client that supplies `?EmpId=` (see PAGE-545 §20). Identity/authorization are entirely URL-parameter based.
- Role logic: only `RoleTypeId` 2 (AM) and 3 (DZSM) resolved from `EmpId` are scoped; other roles get an unscoped page.

## 21. Status Lifecycle
No status column; a DWSP row exists per (employee, territory, date). Any approval state visible in the DWSP report (`ApprovalStatus` filter in PAGE-548) is set elsewhere / by SPs (UNKNOWN).

## 22. Error Handling
- `Page_Load` month/year list creation and every loader are wrapped in empty catch blocks.
- `Convert.ToInt32(Request.QueryString["EmpId"])` (save) and `decimal.Parse` on grid text are unguarded.
- Messages: `Table can not be Empty!`, `Total Target  Can not be Empty!!` (faildalert), success popup redirect, `Already Exist!`.

## 23. Audit
No audit column is passed to the SP (see BR-M14-017). Audit columns: the page passes an `@EntryBy`-style value to the SP (see §8) but which audit columns/tables the SP populates is `UNKNOWN` (SP body not in repo). `UpdateBy/UpdateDate/EntryDate` exist on the DAO (`Library.DAO/DWSP_DAO/*.cs`) but are never set by the pages.

## 24. Integration
Probably launched from the mobile app (INFERRED; see specs/integrations/INT-09). No outbound integration.

## 25. Reports
Data entered here appears in the DWSP monthly report (PAGE-548 / RPT-M14-01).

## 26. Dependencies
BLL: —; DAL: AreaWiseTargetSetupDal, CommonDal, CommonDataLoad, ZoneWiseTargetSetupDal; DAO: ResultInfo, TerritoryTargetSetupDao; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-538-01 | Positive | `?EmpId=<AM>`; pick territory; enter General/FCB/Campaign for some days; Save | one `sp_Save_DWSPMaster` per day (all days, blanks=0) in one txn; redirect to Territory page | BR-M14-015/017 |
| TC-538-02 | Negative | All boxes blank | popup "Total Target  Can not be Empty!!" | BR-M14-015 |
| TC-538-03 | Negative | Territory not selected | Territory marked invalid | BR-M14-015 |
| TC-538-04 | Edge | Total higher/lower than monthly target | saved (no reconciliation) | BR-M14-016 |
| TC-538-05 | Edge | Month 12 vs month 9 | date argument `202612 01` vs `202609 01` produced correctly | BR-M14-018 |
| TC-538-06 | Edge | Edit a past date | allowed | BR-M14-018 |
| TC-538-07 | Security | Open with no session | page works | permission |
| TC-538-08 | Negative | `?EmpId=abc` and Save | unhandled FormatException | BR-M14-017 |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx`, `Solution.Web/DWSP/AMDayWiseDWSPSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `AMDayWiseDWSPSetup.aspx(.cs)` | n/a | MISSING | - |
| Total vs target | None existed | not reconciled | UNKNOWN | UNKNOWN | BR-M14-016 |
| Audit | None existed | no EntryBy | UNKNOWN | RISK | BR-M14-017 |
| Auth | None existed | none | n/a | RISK | §20 |

## 30. Known Gaps
- Unauthenticated write endpoint keyed on `EmpId` (BR-M14-017, §20).
- No reconciliation of daily total to territory target (BR-M14-016); dead `txtAmount_OnTextChanged` cap referencing a non-existent grid control.
- `sp_Get_FirstDate_LastDatebyYearMonth` invoked with `@EndDate == @StartDate` (`ZoneWiseTargetSetupDal.cs:141-149`) - SP presumably derives the month end; if not, `LastDay` is wrong (UNKNOWN).
- Empty catch blocks; duplicated month/year helper code across ~10 pages.
- Save success text "Already Exist!" branch unreachable (BR-M14-012).
- Bulk of code (300+ lines of DZSM hierarchy handling in `groupname_SelectedIndexChanged`) contains duplicate `GetHigharcyInfoByEmployeeId` calls (lines 355-395).
- SP bodies missing: `sp_Save_DWSPMaster`, `sp_Get_TerritoryWiseTargetSetup`, `sp_Get_FirstDate_LastDatebyYearMonth`, `sp_GetDatewithinDateRange`, `sp_Get_TerritoryAll_ByAreaIdIsVaccant`, plus role/zone helpers (GAP-M14-001).

## 31. Change Impact
Writes the rows read by `sp_Process_DWSPReport_Territory` (PAGE-548). Shares `AreaWiseTargetSetupDal`, `CommonDal`, `ZoneWiseTargetSetupDal` with PAGE-539/545/546. Linked to PAGE-545 in both directions (`btnAdd` and post-save redirect).

