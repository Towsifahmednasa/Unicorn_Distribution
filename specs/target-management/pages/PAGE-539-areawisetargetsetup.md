---
page_id: PAGE-539
module: M14
page: AreaWiseTargetSetup
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-539 — AreaWiseTargetSetup

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-539 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | AreaWiseTargetSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DWSP |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DWSP_AreaWiseTargetSetup |
| aspx size | 16102 bytes |
| Code-behind LOC | 614 |
| Inline script size | 1375 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Lets a zone manager (DZSM) or head office **split a zone's monthly target across the zone's Areas** (AM areas). Pick Year, Month, Group and Zone; the page shows the zone target read from the Zone-level setup and a grid of the zone's areas with an editable amount each; Save writes one row per area. CONFIRMED: `Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs`.

## 3. Business Objective
Step 3 of the target cascade (BR-M14-001): give each area (Area Manager) a share of the zone target.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/AreaWiseTargetSetupView.aspx`

Outbound references: `areawisetargetsetupview.aspx`

## 5. Route
`~/DWSP/AreaWiseTargetSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/AreaWiseTargetSetup.aspx |
| Code-behind | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs |
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
| ddlYear | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlmonth_SelectedIndexChanged |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlmonth_SelectedIndexChanged |
| groupname | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→groupname_SelectedIndexChanged |
| hfZoneId | HiddenField |  | no validator found |  |  |  |  |
| ddlZone | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlZone_OnSelectedIndexChanged |
| amount | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| FilteredTextBoxExtender8 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| gv_List | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| HFAreaId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | TemplateField | Amount | no validator found |  |  |  |  |
| txtAmount | TextBox | <%#Eval("Amount") %> | no validator found |  |  |  | OnTextChanged→txtAmount_OnTextChanged |
| FilteredTextBoxExtender8 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | AreaWiseTargetSetupDao.Year | DWSP_AreaWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Year | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlmonth | AreaWiseTargetSetupDao.Month | DWSP_AreaWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Month | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | AreaWiseTargetSetupDao.GroupId | DWSP_AreaWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlZone | AreaWiseTargetSetupDao.RegionId | DWSP_AreaWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| amount | AreaWiseTargetSetupDao.TargetAmount | DWSP_AreaWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @TargetAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | AreaWiseTargetSetupDao.Amount | DWSP_AreaWiseTargetSetup.btnSave_Click | (none — page calls DAL directly) | AreaWiseTargetSetupDal.SaveZoneWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | dt | DWSP_AreaWiseTargetSetup.groupname_SelectedIndexChanged | — | CommonDal.GetArea_ByZoneId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDataLoad.GetZone_byGroupId_forDSM, Zo | UNKNOWN | UNKNOWN | UNKNOWN |
| hfZoneId | SelectedValue | DWSP_AreaWiseTargetSetup.groupname_SelectedIndexChanged | — | CommonDal.GetArea_ByZoneId_All, CommonDal.Get_Forddl_ZoneActive_All_ByGroup, CommonDataLoad.GetZone_byGroupId_forDSM, Zo | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_AreaWiseTargetSetup.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | dtTargetAmount | DWSP_AreaWiseTargetSetup.NewMethod | — | ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlZone | ds | DWSP_AreaWiseTargetSetup.ddlZone_OnSelectedIndexChanged | — | CommonDal.GetArea_ByZoneId_All, ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| groupname | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlZone | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DWSP_AreaWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:318 | — | AreaWiseTargetSetupDal.SaveZoneWiseTarget | sp_Save_AreaWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DWSP_AreaWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:318 | — | AreaWiseTargetSetupDal.SaveZoneWiseTarget | sp_Save_AreaWiseTargetSetup | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DWSP_AreaWiseTargetSetup.Unnamed_Click | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:366 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- `Page_Load` (first load): reads `RoleTypeName`, `EmpInfoId`, `RoleTypeId` from Session (try/catch-swallowed), fills Month/Year, then for role `DZSM` pre-selects the caller's Group and stores the caller's Zone in `hfZoneId` (from `sp_HigharchyInfoByEmployeeId`), locks Group (`Enabled=false`); `LoadInitialInfo()` loads groups and, for `RoleTypeId == "3"`, selects the DZSM's group via `sp_Get_DZSMinfoByEmpId` and cascades. `?MID` shows `btnUpdate` else `btnSave` (`AreaWiseTargetSetup.aspx.cs:31-185`).
- Group change -> zone list. Non-DZSM: all active zones of the group (`sp_Get_Zone_All_Active_ByGroup`); `RoleTypeId 3`: zone pre-selected from `hfZoneId` and locked; `RoleTypeId 5`: zone enabled. (The `strRole == "DZSM"` branch that limits zones via `sp_Get_Zone_ForDSM` only works in the first request because `strRole` is a page field that is reset on every postback - see §30.)
- Zone change -> `GetArea_ByZoneId_All` (`sp_Get_AreaAll_ByZoneId`) fills `gv_List`; `NewMethod()` reads the zone target (`sp_Get_ZoneTargetAmount @RegionId,@month,@Year`) into read-only `amount`.
- Amount edit -> running-sum cap (BR-M14-006). Client: SweetAlert confirm, numeric filter.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DWSP_AreaWiseTargetSetup.Page_Load | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:32 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.Page_Load → CommonDal.GetArea_ByZoneId_All / CommonDal.Get_DZSMinfoByEmpId / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDataLoad.GetHigharcyInfoByEmployeeId / CommonDataLoad.Get → SP: sp_Get_AreaAll_B | yes | no |
| DWSP_AreaWiseTargetSetup.LoadInitialInfo | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:125 |  | DWSP_AreaWiseTargetSetup.LoadInitialInfo → CommonDal.GetArea_ByZoneId_All / CommonDal.Get_DZSMinfoByEmpId / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDal.Get_Group_All / CommonDataLoad.GetZone_byGroupId_forDSM / ZoneWiseTargetSetupDa → SP: sp_Get_Are | yes | no |
| DWSP_AreaWiseTargetSetup.btnSave_Click | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:318 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.btnSave_Click → AreaWiseTargetSetupDal.SaveZoneWiseTarget → SP: sp_Save_AreaWiseTargetSetup | no | yes |
| DWSP_AreaWiseTargetSetup.Unnamed_Click | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:366 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.Unnamed_Click | no | no |
| DWSP_AreaWiseTargetSetup.gv_DocumentUpload_PreRender | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:369 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.gv_DocumentUpload_PreRender | no | no |
| DWSP_AreaWiseTargetSetup.groupname_SelectedIndexChanged | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:381 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.groupname_SelectedIndexChanged → CommonDal.GetArea_ByZoneId_All / CommonDal.Get_Forddl_ZoneActive_All_ByGroup / CommonDataLoad.GetZone_byGroupId_forDSM / ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone → SP: sp_Get_AreaAll_ByZon | yes | no |
| DWSP_AreaWiseTargetSetup.txtAmount_OnTextChanged | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:477 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.txtAmount_OnTextChanged | no | no |
| DWSP_AreaWiseTargetSetup.NewMethod | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:537 |  | DWSP_AreaWiseTargetSetup.NewMethod → ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone → SP: sp_Get_ZoneTargetAmount | yes | no |
| DWSP_AreaWiseTargetSetup.ddlZone_OnSelectedIndexChanged | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:557 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.ddlZone_OnSelectedIndexChanged → CommonDal.GetArea_ByZoneId_All / ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone → SP: sp_Get_AreaAll_ByZoneId, sp_Get_ZoneTargetAmount | no | no |
| DWSP_AreaWiseTargetSetup.ddlYear_SelectedIndexChanged | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:599 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.ddlYear_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone → SP: sp_Get_ZoneTargetAmount | no | no |
| DWSP_AreaWiseTargetSetup.groupname_SelectedIndexChanged1 | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:604 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.groupname_SelectedIndexChanged1 → ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone → SP: sp_Get_ZoneTargetAmount | no | no |
| DWSP_AreaWiseTargetSetup.ddlmonth_SelectedIndexChanged | Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs:610 | object sender, EventArgs e | DWSP_AreaWiseTargetSetup.ddlmonth_SelectedIndexChanged → ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone → SP: sp_Get_ZoneTargetAmount | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetHigharcyInfoByEmployeeId | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1866 | sp_HigharchyInfoByEmployeeId | @EmployeeId, @RoleId |
| CommonDal.Get_Group_All | Library.DAL/DWSP_DAL/CommonDal.cs:38 | sp_NTS_Group_Active |  |
| CommonDal.Get_DZSMinfoByEmpId | Library.DAL/DWSP_DAL/CommonDal.cs:116 | sp_Get_DZSMinfoByEmpId | @EmpId |
| CommonDataLoad.GetZone_byGroupId_forDSM | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1275 | sp_Get_Zone_ForDSM | @GroupId |
| CommonDal.Get_Forddl_ZoneActive_All_ByGroup | Library.DAL/DWSP_DAL/CommonDal.cs:77 | sp_Get_Zone_All_Active_ByGroup | @GroupId |
| ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:89 | sp_Get_ZoneTargetAmount | @RegionId, @Year, @month |
| CommonDal.GetArea_ByZoneId_All | Library.DAL/DWSP_DAL/CommonDal.cs:139 | sp_Get_AreaAll_ByZoneId | @id |
| AreaWiseTargetSetupDal.SaveZoneWiseTarget | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs:17 | sp_Save_AreaWiseTargetSetup | @Amount, @AreaId, @AreaWTSetupId, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @Year |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_AreaAll_ByZoneId | CommonDal.GetArea_ByZoneId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DZSMinfoByEmpId | CommonDal.Get_DZSMinfoByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneTargetAmount | ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active_ByGroup | CommonDal.Get_Forddl_ZoneActive_All_ByGroup | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_ForDSM | CommonDataLoad.GetZone_byGroupId_forDSM | body NOT in repository | read-like name (INFERRED) |
| sp_HigharchyInfoByEmployeeId | CommonDataLoad.GetHigharcyInfoByEmployeeId | body NOT in repository | read-like name (INFERRED) |
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
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Table can not be Empty! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','AreaWiseTargetSetupView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler groupname_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Target Amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-002 | Year drop-down is a fixed list 2015-2050, default = current year. Month drop-down lists 12 entries starting from the *current calendar month* and wrapping (e.g. in September: September ... August); each item's Text is the month name and Value is the calendar month number; the current month is pre-selected. | `DWSP/AreaWiseTargetSetup.aspx.cs:187-215` (same `GetYearList`/`GetMonthList` copy in every DWSP page) | code-behind | CONFIRMED |
| BR-M14-003 | Except on the DWSP day-wise page, the month is persisted as the month **name** text (`ddlmonth.SelectedItem.Text`, e.g. `September`) plus a text year; the DWSP page and the monthly report use the month **number**. The two conventions meet only inside SPs whose bodies are not in the repo. | `DWSP/AreaWiseTargetSetup.aspx.cs:317-352`; `DWSP/AMDayWiseDWSPSetup.aspx.cs:666-680` | code-behind | CONFIRMED |
| BR-M14-006 | Running-sum cap (advisory), identical to the Zone page: cumulative area amounts `<=` zone target, else message `Amount must be equal with target amount` and the row is cleared; empty parent -> `Please Select Target Amount`; **not re-checked on Save**. | `AreaWiseTargetSetup.aspx.cs:476-535` | code-behind | CONFIRMED |
| BR-M14-010 | **Area save validation.** Required: Year, Month, Group, zone target `amount` non-empty, >=1 row, all row amounts non-empty. Zone drop-down is not itself validated (a blank zone becomes `RegionId = NULL`). Each row -> `sp_Save_AreaWiseTargetSetup(@AreaWTSetupId=0,@Year,@Month(name),@GroupId,@TargetAmount,@RegionId,@AreaId,@Amount,@EntryBy=Session UserId)` via `SaveData`; `isSuccess` is `true` whenever no exception occurs, so the `Already Exist!` branch cannot trigger (BR-M14-012). | `AreaWiseTargetSetup.aspx.cs:217-364`; `AreaWiseTargetSetupDal.cs:16-42` | code-behind + DAL | CONFIRMED |
| BR-M14-011 | **Role scoping.** DZSM (`RoleTypeName=="DZSM"` on first load / `RoleTypeId==3`) is confined to own Group and Zone (drop-downs locked). `RoleTypeId` 5 keeps the zone drop-down enabled (role meaning is not stated in this file; INFERRED head-office/NSM level) so it can pick any zone. No server-side re-validation: a crafted postback can select another zone. | `AreaWiseTargetSetup.aspx.cs:31-122,380-475` | code-behind | CONFIRMED (behaviour); INFERRED (role-5 meaning) |

## 20. Permission Matrix
- Session keys read: `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (Page_Load / group handler), `UserId` (save audit value).
- Enforcement is client-state based only (BR-M14-011); no server-side check that the posted `ZoneId` belongs to the DZSM. Menu placement decides who can open the page.

## 21. Status Lifecycle
No status column handled. A row per (Group, Year, Month, Zone, Area); duplicate handling inside SP (UNKNOWN).

## 22. Error Handling
- Page_Load Session reads wrapped in `try { } catch { }` (silent); `dtMarket.Rows[0]` on line ~55 is unguarded for DZSM users with no hierarchy row (unhandled `IndexOutOfRange`).
- Validation: CSS + tooltip; `Table can not be Empty!`.
- Save exceptions propagate (DAL rollback+rethrow), no page-level handling.

## 23. Audit
Audit columns: the page passes an `@EntryBy`-style value to the SP (see §8) but which audit columns/tables the SP populates is `UNKNOWN` (SP body not in repo). `UpdateBy/UpdateDate/EntryDate` exist on the DAO (`Library.DAO/DWSP_DAO/*.cs`) but are never set by the pages.

## 24. Integration
None (database only).

## 25. Reports
None; list is PAGE-540.

## 26. Dependencies
BLL: —; DAL: AreaWiseTargetSetupDal, CommonDal, CommonDataLoad, ZoneWiseTargetSetupDal; DAO: AreaWiseTargetSetupDao, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-539-01 | Positive | HO user picks Group/Zone with a zone target; splits across areas; Save | one SP call per area; redirect to `AreaWiseTargetSetupView.aspx` | BR-M14-010 |
| TC-539-02 | Positive | Login as DZSM | Group and Zone locked to own hierarchy | BR-M14-011 |
| TC-539-03 | Negative | Zone target missing | `amount` empty -> validation blocks Save (amount invalid) | BR-M14-010 |
| TC-539-04 | Negative | Amounts exceed zone target | offending row cleared + message | BR-M14-006 |
| TC-539-05 | Edge | Sum < zone target then Save | saved (no server check) | BR-M14-006 |
| TC-539-06 | Security | DZSM tampers `ddlZone` value in POST | server accepts (no check) | BR-M14-011 |
| TC-539-07 | Edge | Re-save existing month | success message even if SP ignored duplicates | BR-M14-010 |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/AreaWiseTargetSetup.aspx`, `Solution.Web/DWSP/AreaWiseTargetSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `AreaWiseTargetSetup.aspx(.cs)` | n/a | MISSING | - |
| Sum(area)=zone | None existed | UI cap only | UNKNOWN | UNKNOWN | BR-M14-006 |
| DZSM scoping | None existed | client-state only | n/a | RISK | BR-M14-011 |

## 30. Known Gaps
- Advisory-only cap; wrong message wording (BR-M14-006).
- Role-scoping variables (`strRole`, `masArea`, `areaId`) are page fields reset every postback: the DZSM zone-restriction branch of `groupname_SelectedIndexChanged` is dead after the first request (lines 380-420).
- Unguarded `dtMarket.Rows[0]` (line ~55) and `if (EmpInfoId != "" || EmpInfoId != null)` is always true (dead guard).
- `?MID` edit mode not implemented (no load; AreaWTSetupId always 0).
- `Unnamed_Click` (Reset) empty.
- `AreaWiseTargetSetupDal.SaveZoneWiseTarget` is a mis-named method (it saves *area* targets) - copy/paste naming.
- SP bodies missing: `sp_Save_AreaWiseTargetSetup`, `sp_Get_ZoneTargetAmount`, `sp_Get_AreaAll_ByZoneId`, `sp_Get_Zone_All_Active_ByGroup`, `sp_Get_Zone_ForDSM`, `sp_HigharchyInfoByEmployeeId`, `sp_Get_DZSMinfoByEmpId` (GAP-M14-001).

## 31. Change Impact
Reads zone rows from PAGE-546; writes area rows read by the Territory pages (`sp_Get_AreaTargetAmount`, PAGE-544/545) and listed by PAGE-540. Shares `CommonDal`, `CommonDataLoad`, `ZoneWiseTargetSetupDal` with PAGE-538/545/546.

