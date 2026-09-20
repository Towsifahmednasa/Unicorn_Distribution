---
page_id: PAGE-595
module: M15
page: TourPurposeOtherSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-595 — TourPurposeOtherSetup

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-595 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | TourPurposeOtherSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_TourPurposeOtherSetup |
| aspx size | 28006 bytes |
| Code-behind LOC | 863 |
| Inline script size | 1930 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED (labels `Visit Type`, `Tour Purpose`, `Role`, `Territory/Area/Region/Group`, `Station Type`): configures **which tour purposes are available in a field-force tour plan, and to whom** - for each tour purpose (e.g. training, meeting, "other" non-market work), a list of applicability rows = (role: MIO / AM / DZSM / Regional Head(NSM)) x (their territory / area / region / group) x (station type HQ / EX-HQ / OS), plus a Market-Visit vs Other-Visit flag and an Active flag. The page also carries a leftover copy of the Expense-Type screen (name/amount/role-type/employee/DA-field panels) that is not wired to this page's save.

## 3. Business Objective
INFERRED: control what a MIO/AM/DZSM/NSM may choose as a tour purpose when planning a tour (mobile/web tour plan) and which purposes need a market (`Market Visit`, VisitTypeId 1) versus not (`Other Visit`, VisitTypeId 2). The TADA screen uses the same distinction: a tour purpose whose text contains `(Other Visit)` does not require a market (PAGE-587 BR-M15-035).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `expensetypeview.aspx`

## 5. Route
`~/DoctorModule_UI/TourPurposeOtherSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAL class | SetupDAL (Library.DAL/DoctorModule_DAL/SetupDAL.cs) |
| DAO/entity class | ExpenseTypeMaster (Library.DAO/DoctorModule_DAO/ExpenseTypeMaster.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | TourPurposeOtherSetup (Library.DAO/DoctorModule_DAO/ExpenseTypeDetails.cs) |
| DAO/entity class | TourPurposeOtherSetupDtl (Library.DAO/DoctorModule_DAO/ExpenseTypeDetails.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| visitType | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→VisitType_SelectedIndexChanged |
| tourPurpose | TextBox | Tour Purpose | no validator found |  | Visible=false |  |  |
| ddlTourPurpose | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlTourPurpose_SelectedIndexChanged |
| chkIsActive | input |  | no validator found |  |  |  |  |
| roleSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→roleSelect_SelectedIndexChanged |
| territorySelect | DropDownList |  | no validator found |  |  | see §9 |  |
| areaSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| regionSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| GroupSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| StationSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| btnAdd | LinkButton |  | no validator found |  |  |  | OnClick→btnAdd_Click |
| gvList | GridView |  | no validator found |  |  |  | OnRowCommand→gvList_RowCommand |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| (no id) | TemplateField | Role | no validator found |  |  |  |  |
| lblRole | Label | <%# Eval("Role") %> | no validator found |  |  |  |  |
| hfRoleId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Territory | no validator found |  |  |  |  |
| lblTerritory | Label | <%# Eval("Territory") %> | no validator found |  |  |  |  |
| hfTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Area | no validator found |  |  |  |  |
| lblArea | Label | <%# Eval("Area") %> | no validator found |  |  |  |  |
| hfAreaId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Region | no validator found |  |  |  |  |
| lblRegion | Label | <%# Eval("Region") %> | no validator found |  |  |  |  |
| hfRegionId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Group | no validator found |  |  |  |  |
| lblGroup | Label | <%# Eval("Group") %> | no validator found |  |  |  |  |
| hfGroupId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Station Type | no validator found |  |  |  |  |
| lblStationType | Label | <%# Eval("StationType") %> | no validator found |  |  |  |  |
| hfStationTypeId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Action | no validator found |  |  |  |  |
| lnkDelete | LinkButton |  | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| LinkButton2 | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |
| restbtn | LinkButton |  | no validator found |  |  |  | OnClick→restbtn_Click |
| TxtName | TextBox |  | no validator found |  |  |  |  |
| ddlRoleType | ListBox |  | no validator found |  |  |  |  |
| EmployeeIdSelect | ListBox |  | no validator found |  |  |  |  |
| chkFixed | CheckBox | is Fixed Amount | no validator found |  |  |  | OnCheckedChanged→chkFixed_CheckedChanged |
| txtAmount | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| rbImgType | RadioButtonList |  | no validator found |  |  |  |  |
| txtFieldName | TextBox |  | no validator found |  |  |  |  |
| IsRequired | DropDownList |  | no validator found |  |  | see §9 |  |
| hfForMe | HiddenField |  | no validator found |  |  |  |  |
| addButtonDA | LinkButton |  | no validator found |  |  |  | OnClick→addButtonDA_Click |
| gv_DA | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfExpenseTypDetailsId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Field Name | no validator found |  |  |  |  |
| lbl_FieldName | Label | <%# Eval("FieldName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Required | no validator found |  |  |  |  |
| lbl_IsRequied | Label | <%# Eval("IsRequied") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| deleteImageButton | LinkButton |  | no validator found |  |  |  | OnClick→deleteImageButton_Click |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| customSwitch1 | input |  | no validator found |  |  |  | onchange→IsActiveChange() |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| roleSelect | roleId | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| roleSelect | roleName | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| territorySelect | territoryId | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| territorySelect | territoryName | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| areaSelect | areaId | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| areaSelect | areaName | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| regionSelect | regionId | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| regionSelect | regionName | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | GroupId | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | GroupName | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| StationSelect | stationTypeId | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| StationSelect | stationTypeName | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| txtFieldName | newFieldName | DoctorModule_UI_TourPurposeOtherSetup.Add | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| hfExpenseTypDetailsId | dt | DoctorModule_UI_TourPurposeOtherSetup.Remove | — | SetupDAL.checkFroDelete, SetupDAL.delExpensDetls | UNKNOWN | UNKNOWN | UNKNOWN |
| hfExpenseTypDetailsId | Res | DoctorModule_UI_TourPurposeOtherSetup.Remove | — | SetupDAL.checkFroDelete, SetupDAL.delExpensDetls | UNKNOWN | UNKNOWN | UNKNOWN |
| lblRole | TourPurposeOtherSetupDtl.RoleName | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @RoleName | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfTerritoryId | TourPurposeOtherSetupDtl.TerritoryId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfAreaId | TourPurposeOtherSetupDtl.AreaId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfRegionId | TourPurposeOtherSetupDtl.RegionId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfGroupId | TourPurposeOtherSetupDtl.GroupId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStationTypeId | TourPurposeOtherSetupDtl.TourTypeId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @TourTypeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | TourPurposeOtherSetup.TourPurposeOtherSetupId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @TourPurposeOtherSetupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlTourPurpose | TourPurposeOtherSetup.TourPurposeId | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @TourPurposeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | TourPurposeOtherSetup.IsActive | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveTPOtherMaster | @IsActive | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| visitType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlTourPurpose | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| roleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| territorySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| areaSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| regionSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| GroupSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| StationSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlRoleType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbImgType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| IsRequired | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnAdd | LinkButton click | (postback) | DoctorModule_UI_TourPurposeOtherSetup.btnAdd_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:137 | — | — | — | read-only / none detected |
| LinkButton1 | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:716 | — | SetupDAL.SaveTPOtherMaster | spInsertTourPurposeOtherSetup, spInsertTourPurposeOtherSetupDtl | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| LinkButton2 | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:716 | — | SetupDAL.SaveTPOtherMaster | spInsertTourPurposeOtherSetup, spInsertTourPurposeOtherSetupDtl | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| restbtn | LinkButton click | (postback) | DoctorModule_UI_TourPurposeOtherSetup.restbtn_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:855 | — | — | — | read-only / none detected |
| addButtonDA | LinkButton click | (postback) | DoctorModule_UI_TourPurposeOtherSetup.addButtonDA_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:504 | — | — | — | read-only / none detected |
| deleteImageButton | LinkButton click | (postback) | DoctorModule_UI_TourPurposeOtherSetup.deleteImageButton_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:582 | — | — | — | read-only / none detected |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:716 | — | SetupDAL.SaveTPOtherMaster | spInsertTourPurposeOtherSetup, spInsertTourPurposeOtherSetupDtl | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_TourPurposeOtherSetup.btnSave_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:716 | — | SetupDAL.SaveTPOtherMaster | spInsertTourPurposeOtherSetup, spInsertTourPurposeOtherSetupDtl | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | DoctorModule_UI_TourPurposeOtherSetup.btnReset_Click | Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs:813 | — | — | — | read-only / none detected |

## 11. Frontend Flow
1. `Page_Load` (`TourPurposeOtherSetup.aspx.cs:56-129`, first GET): clears the session grid (`Session["VisitDataTable"] = null` `:60`), shows the role panel and hides territory/area/region panels, fills purposes (`sp_Get_TourPurposeDDLNew`), tour types (`sp_Get_TourPlanTypeDDL`, displayed as HQ=2 / EX-HQ=1 / OS=3 static list in markup), territory/area/zone/group pick-lists (`sp_Get_TerritoryListOrdPerALL`, `sp_Get_AreaListOrdPerALL`, `sp_Get_ZoneListOrdPer`, `sp_Get_GroupListOrdPer` - "OrdPer" = order-permission-filtered lists, INFERRED), role types and employees for the unused expense panel. With `?id=` -> `btnUpdate` + `GetOneRecord`.
2. Visit type radio (AutoPostBack) `VisitType_SelectedIndexChanged`: `market` -> hide role panel, show territory/area/region; `other` (default) -> show role panel.
3. Role select: MIO -> territory box; AM -> area; DZSM -> region; NSM ("Regional Head") -> group (`:827-858`).
4. **Add to list** (`btnAdd_Click` `:136-230`): builds a session-held `DataTable` (`Session["VisitDataTable"]`) of rows and binds `gvList`; delete-row via `gvList_RowCommand` (`:244`).
5. Selecting a tour purpose triggers `ddlTourPurpose_SelectedIndexChanged` -> `GetOneRecord(ddlTourPurpose.SelectedValue)` (`:859-861`) - it passes the **purpose id** to a loader that expects the setup id (`GetEditDataForTPOtherId` -> `sp_Get_TourPurposeOtherSetupId`); effect UNKNOWN.
6. Save: `btnSave` -> `btnSave_Click`; success -> `successalert(...,'TourPurposeOtherSetup.aspx')`.

## 12. Backend Flow
`btnSave_Click` (`:715-800`): `Validation()` -> loop `gvList.Rows` building `TourPurposeOtherSetupDtl` (`RoleName = lblRole.Text` (**text, not id**), TerritoryId/AreaId/RegionId/GroupId/TourTypeId from hidden fields, blank -> null) -> `TourPurposeOtherSetup` master (`TourPurposeOtherSetupId`, `TourPurposeId`, `VisitTypeId` = 1 if radio item 0 (`Market Visit`) else 2, `IsActive`) -> `SetupDAL.SaveTPOtherMaster(aMaster, list, Session["UserId"])` (`Library.DAL/DoctorModule_DAL/SetupDAL.cs:4421`). DAL: id==0 -> `SaveDataReturnPrimaryKey("spInsertTourPurposeOtherSetup")` (`@TourPurposeOtherSetupId,@IsActive,@TourPurposeId,@VisitTypeId,@EntryBy`) then for each detail `SaveData("spInsertTourPurposeOtherSetupDtl")` (`@TourPurposeOtherSetupId,@RoleName,@TerritoryId,@AreaId,@RegionId,@GroupId,@TourTypeId`); **id>0 branch contains only commented-out code (`SetupDAL.cs:4434-4482`), so nothing is saved and `pk` stays 0**. Result mapping in page: `isSuccess` -> success; `isDuplicateCheck` -> `Already Exist!`; `isValiCheck` -> `Data cannot be deactivated!`; else `Already Exist!` (`:750-800`). Transaction: implicit (first write) committed at connection close; `isSuccess` is overwritten by each detail insert so only the last detail's result is reported.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetRoleTypeInfoDALFFS | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:356 | sp_CS_RoleTypeFSS |  |
| CommonDataLoad.GetEmployeeList_ActiveFS | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:484 | sp_CS_GetEmpGeneralInfo_AllFSS |  |
| SetupDAL.GetEditDataForTPOtherId | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5152 | sp_Get_TourPurposeOtherSetupId | @id |
| SetupDAL.Get_TourPurposeInfoNew | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5300 | sp_Get_TourPurposeDDLNew |  |
| SetupDAL.Get_TourPlanTypeDDL | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5319 | sp_Get_TourPlanTypeDDL |  |
| Setup2DAL.GetTerritoryListOrdPerALL | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:434 | sp_Get_TerritoryListOrdPerALL |  |
| Setup2DAL.GetAreaListOrdPerALL | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:413 | sp_Get_AreaListOrdPerALL |  |
| Setup2DAL.GetZoneListOrdPer | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:455 | sp_Get_ZoneListOrdPer |  |
| Setup2DAL.GetGroupListOrdPer | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:475 | sp_Get_GroupListOrdPer |  |
| SetupDAL.checkFroDelete | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5174 | sp_check_Vali_MarketStructure | @MasterId, @PageName |
| SetupDAL.delExpensDetls | Library.DAL/DoctorModule_DAL/SetupDAL.cs:4253 | sp_ExpenseTypeDtl | @ExpenseTypeId |
| SetupDAL.SaveTPOtherMaster | Library.DAL/DoctorModule_DAL/SetupDAL.cs:4422 | spInsertTourPurposeOtherSetup, spInsertTourPurposeOtherSetupDtl | @AreaId, @EntryBy, @GroupId, @IsActive, @RegionId, @RoleName, @TerritoryId, @TourPurposeId, @TourPur |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| spInsertTourPurposeOtherSetup | SetupDAL.SaveTPOtherMaster | body NOT in repository | write-like name |
| spInsertTourPurposeOtherSetupDtl | SetupDAL.SaveTPOtherMaster | body NOT in repository | write-like name |
| sp_CS_GetEmpGeneralInfo_AllFSS | CommonDataLoad.GetEmployeeList_ActiveFS | body NOT in repository | read-like name (INFERRED) |
| sp_CS_RoleTypeFSS | CommonDataLoad.GetRoleTypeInfoDALFFS | body NOT in repository | read-like name (INFERRED) |
| sp_ExpenseTypeDtl | SetupDAL.delExpensDetls | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaListOrdPerALL | Setup2DAL.GetAreaListOrdPerALL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_GroupListOrdPer | Setup2DAL.GetGroupListOrdPer | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryListOrdPerALL | Setup2DAL.GetTerritoryListOrdPerALL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPlanTypeDDL | SetupDAL.Get_TourPlanTypeDDL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPurposeDDLNew | SetupDAL.Get_TourPurposeInfoNew | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPurposeOtherSetupId | SetupDAL.GetEditDataForTPOtherId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneListOrdPer | Setup2DAL.GetZoneListOrdPer | body NOT in repository | read-like name (INFERRED) |
| sp_check_Vali_MarketStructure | SetupDAL.checkFroDelete | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| ddlTourPurpose | required on save | `Validation()` `:676-694` | tooltip only (`please fill out this field`) | BR-M15-029 |
| gvList | >= 1 row | `:689-697` | `please Add to List One Row` (alert) | BR-M15-029 |
| roleSelect (add) | must not be "Select One"/0 | `btnAdd_Click` `:161-165` | silent (focus only) | BR-M15-030 |
| StationSelect (add) | must be chosen | `:167-171` | silent | BR-M15-030 |
| territory/area/region/group (add) | at least one chosen | `:174-185` | `Please select at least one of Territory, Area,  Region or Group` | BR-M15-030 |
| duplicate row (add) | same role+territory+area+region+group+station already in grid | `:188-202` | `Already Exist in Table!` | BR-M15-030 |
| server duplicate | not checked in code (SP may) | - | `Already Exist!` shown for any failure | GAP-M15-010 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-029 | A tour-purpose setup needs a tour purpose and at least one applicability row; visit type default is `Other Visit` (2); `Market Visit` maps to 1. | `TourPurposeOtherSetup.aspx.cs:676-697,771-781`; markup `TourPurposeOtherSetup.aspx:58-62` | code-behind | CONFIRMED |
| BR-M15-030 | An applicability row requires role and station type, plus at least one market node (territory, area, region or group); identical rows cannot be added twice; a role only exposes its own level (MIO->territory, AM->area, DZSM->region, NSM "Regional Head"->group). | `:136-202,827-858` | code-behind | CONFIRMED |
| BR-M15-031 | Station type values are HQ=2, EX-HQ=1, OS=3 (static markup) and are saved as `TourTypeId` in each detail row; the same three tour types are used by the tour quota (PAGE-616) and TADA rules (PAGE-589). | `TourPurposeOtherSetup.aspx:175-181`; `SetupDAL.cs:4515` | markup + DAL | CONFIRMED (values), INFERRED (sharing) |
| BR-M15-032 | Editing an existing setup is not implemented: the update branch of `SaveTPOtherMaster` is commented out, so an edit save writes nothing and the user sees `Already Exist!`. | `Library.DAL/DoctorModule_DAL/SetupDAL.cs:4434-4482` | DAL | CONFIRMED |
| BR-M15-033 | The role is persisted as the text of the label (`RoleName`), not the role id, and `Group` name in the grid is blank unless a region is selected (`GroupName = regionId=="0" ? "" : ...` uses the wrong variable). | `TourPurposeOtherSetup.aspx.cs:156,748` | code-behind | CONFIRMED |

## 20. Permission Matrix
Session keys: `UserId` (entry user) only. No role/permission validation, no `UserPersmissionValidation`. Grid state is stored in `Session["VisitDataTable"]`, shared across all tabs of the same user (last writer wins).

## 21. Status Lifecycle
`IsActive` (checkbox `chkIsActive`) on the master only; no other status. Deactivation guard message `Data cannot be deactivated!` is mapped (`isValiCheck`) but the DAL never sets it for this entity (dead branch).

## 22. Error Handling
Validation popups via `faildalert`; all lookup loads swallow exceptions; DAL rolls back + rethrows (unhandled at page level). Generic `Already Exist!` for every failure including the unimplemented edit.

## 23. Audit
`@EntryBy = Session["UserId"]` on master insert; detail rows carry no audit param. No update audit (update not implemented).

## 24. Integration
None. Consumed by tour-plan creation (mobile/web - not in M15 code); consumer SPs UNKNOWN.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, Setup2DAL, SetupDAL; DAO: ExpenseTypeMaster, ResultInfo, TourPurposeOtherSetup, TourPurposeOtherSetupDtl; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-595-01 | Add row: role MIO, station HQ, territory T1 -> Add -> pick purpose -> Save | Master + 1 detail saved, `Operation successful!` | BR-M15-029, 030 |
| TC-595-02 | Add row with no market node | `Please select at least one of Territory, Area,  Region or Group` | BR-M15-030 |
| TC-595-03 | Add same row twice | `Already Exist in Table!` | BR-M15-030 |
| TC-595-04 | Save with no rows | `please Add to List One Row` | BR-M15-029 |
| TC-595-05 | Open `?id=<existing>`, change active flag, Save | Nothing saved; `Already Exist!` shown | BR-M15-032 |
| TC-595-06 | Add NSM row, group only | Grid group name blank (defect) | BR-M15-033 |
| TC-595-07 | Two browser tabs adding rows | Rows mix through session table (edge) | GAP-M15-010 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx`, `Solution.Web/DoctorModule_UI/TourPurposeOtherSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | TourPurposeOtherSetup.aspx(.cs) | n/a | MISSING | - |
| Edit | None existed | Not implemented | n/a | ACTUAL-DEFECT | BR-M15-032 |
| Table/SP mapping | None existed | 2 write SPs, 6 lookup SPs | bodies absent | UNKNOWN | GAP-M15-001 |

## 30. Known Gaps
- **GAP-M15-010** Edit no-op with misleading message; session-held grid; role saved as text; generic error text; leftover Expense-Type panels/handlers (`chkFixed_CheckedChanged`, `addButtonDA_Click`, `Remove`) unused (dead code, ~250 lines).
- `ddlTourPurpose_SelectedIndexChanged` passes purpose id to a setup-id loader.
- Inherits GAP-M15-001, 003, 005.

## 31. Change Impact
`SetupDAL` is shared by many Doctor-module pages (ExpenseType PAGE-572, Transport, TourPurpose*). SPs `spInsertTourPurposeOtherSetup*` / `sp_Get_TourPurposeOtherSetupId` are specific. Tour-plan consumers of these tables are outside M15 code (UNKNOWN).

