---
page_id: PAGE-578
module: M15
page: LeaveConfig
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-578 — LeaveConfig

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-578 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | LeaveConfig |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_LeaveConfig |
| aspx size | 17566 bytes |
| Code-behind LOC | 580 |
| Inline script size | 1822 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED (labels): define a **leave rule** ("Leave Configuration Information"): Leave Name, Count Govt. Leave (Yes/No), Count Employee Holiday (Yes/No) with the weekly-holiday **Day** when Yes, Eligible for Probation Employee (Yes/No), Leave Type (from `sp_Webapi_Get_LeaveConType`, values include `Annual`, `Sick`, `Casual`, `Foreign`), the employees the rule applies to (only for `Foreign`), a **Leave Count** grid (`Joining-date count` band -> `Days` per month) shown for Annual/Sick/Casual, and Active. Lists: `LeaveConfigList.aspx` (PAGE-579).

## 3. Business Objective
INFERRED: parametrise how yearly/monthly leave entitlement is computed and whether public holidays and weekly off days are counted as leave, feeding the yearly leave processing (PAGE-611 / WF-M15-03) and the balance report. The formula itself is in SPs and is UNKNOWN.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/LeaveConfigList.aspx`

Outbound references: `leaveconfiglist.aspx`

## 5. Route
`~/DoctorModule_UI/LeaveConfig.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/LeaveConfig.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | BonusCampaignNewDAL (Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | LeaveDal (Library.DAL/DoctorModule_DAL/LeaveDal.cs) |
| DAO/entity class | EmployeeLeave (Library.DAO/DoctorModule_DAO/EmployeeLeave.cs) |
| DAO/entity class | LeaveConfigCountDtl (Library.DAO/DoctorModule_DAO/LeaveConfigDAO.cs) |
| DAO/entity class | LeaveConfigDAO (Library.DAO/DoctorModule_DAO/LeaveConfigDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| txtLeaveName | TextBox | Leave Name | no validator found |  |  |  |  |
| rbCountGovtLeave | RadioButtonList |  | no validator found |  |  |  |  |
| rbEmployeeWeeklyHoliday | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbEmployeeWeeklyHoliday_SelectedIndexChanged |
| ddlDayName | DropDownList |  | no validator found |  |  | see §9 |  |
| rbEligbleforProbationEmployee | RadioButtonList |  | no validator found |  |  |  |  |
| ddlLeaveConType | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlLeaveConType_SelectedIndexChanged |
| EmployeeIdSelect | ListBox |  | no validator found |  |  |  |  |
| itemGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Joining Date Count | no validator found |  |  |  |  |
| hfJoiningDateCount | HiddenField |  | no validator found |  |  |  |  |
| ddlJoiningDateCount | DropDownList |  | no validator found |  |  | see §9 |  |
| (no id) | TemplateField | Monthly Leave | no validator found |  |  |  |  |
| txtCountDays | TextBox | <%# Eval("CountDays")%> | no validator found |  |  |  |  |
| FilteredTextBoxExtender2 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | Actions | no validator found |  |  |  |  |
| btnAdd | LinkButton |  | no validator found |  |  |  | OnClick→btnAdd_Click |
| btnDel | LinkButton |  | no validator found |  |  |  | OnClick→btnDel_Click |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| chkIsActive | input |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlJoiningDateCount | LeaveConfigCountDtl.JoiningDateCountId | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @JoiningDateCountId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtCountDays | LeaveConfigCountDtl.DaysPerMonthly | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @DaysPerMonthly | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | LeaveConfigCountDtl.LeaveConfigId | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @LeaveConfigId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLeaveName | LeaveConfigDAO.LeaveName | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @LeaveName | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlDayName | LeaveConfigDAO.DayNameId | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @DayNameId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlLeaveConType | LeaveConfigDAO.LeaveTypeId | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @LeaveTypeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | LeaveConfigDAO.IsActive | DoctorModule_UI_LeaveConfig.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveLeaveConfig | @IsActive | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbCountGovtLeave | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbEmployeeWeeklyHoliday | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlDayName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbEligbleforProbationEmployee | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlLeaveConType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlJoiningDateCount | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnAdd | LinkButton click | (postback) | DoctorModule_UI_LeaveConfig.btnAdd_Click | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:374 | — | CommonDataLoad.Get_JoiningDateCountInfo | sp_Get_JoiningDateCountInfo | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnDel | LinkButton click | (postback) | DoctorModule_UI_LeaveConfig.btnDel_Click | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:435 | — | CommonDataLoad.Get_JoiningDateCountInfo | sp_Get_JoiningDateCountInfo | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_LeaveConfig.btnSave_Click | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:476 | — | BonusCampaignNewDAL.SaveLeaveConfig | sp_Save_LeaveConfigDtl, sp_Save_LeaveConfigFroenEmp, sp_Save_LeaveConfigMaster, sp_Up_LeaveConfigMaster | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_LeaveConfig.btnSave_Click | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:476 | — | BonusCampaignNewDAL.SaveLeaveConfig | sp_Save_LeaveConfigDtl, sp_Save_LeaveConfigFroenEmp, sp_Save_LeaveConfigMaster, sp_Up_LeaveConfigMaster | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | DoctorModule_UI_LeaveConfig.Unnamed_Click | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:577 | — | — | — | read-only / none detected |

## 11. Frontend Flow
`Page_Load` (`LeaveConfig.aspx.cs:21-48`): `LoadInitialInfo` (active employees, day names `sp_Webapi_Get_DayName`, leave con-types), `LoadInitialGrid` (one empty grid row), `LoadgvDDl` (joining-date-count options `sp_Get_JoiningDateCountInfo` per row); `?id=` -> `GetOneRecord` (`:50-163`, `sp_GET_LeaveConfigSetupById`, sets radios, type, day, active, employee CSV, grid). Events: `rbEmployeeWeeklyHoliday_SelectedIndexChanged` -> Day picker visible only when Yes (`:307`); `ddlLeaveConType_SelectedIndexChanged` (`:324`) resets grid, hides panels, `Foreign` -> employee list, `Annual|Sick|Casual` -> count grid (matched on **display text**); `btnAdd_Click`/`btnDel_Click` add/remove grid rows preserving values via hidden fields. Save/Update -> `btnSave_Click` (`:475`); success -> `LeaveConfigList.aspx`. Markup also has legacy WebMethods `GetEmployeeEditData`/`Save_Leaveinfo` (copy of PAGE-577, unused here).

## 12. Backend Flow
`btnSave_Click`: `Validation()` (always true, `:571-574`) -> grid rows with both a joining-date-count and a days value become `LeaveConfigCountDtl(JoiningDateCountId, DaysPerMonthly=text)` -> `LeaveConfigDAO` (radio item 0 = Yes -> true) -> `BonusCampaignNewDAL.SaveLeaveConfig(aMaster, empCsv, rows, UserId)` (`Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:740`): id>0 -> `sp_Up_LeaveConfigMaster`, else `sp_Save_LeaveConfigMaster` (PK); pk>0 -> `sp_Save_LeaveConfigDtl(@LeaveConfigId,@JoiningDateCountId,@DaysPerMonthly)` per row and `sp_Save_LeaveConfigFroenEmp(@LeaveConfigId,@EmployeeId)` per selected employee (exceptions in this employee loop are swallowed); pk<=0 -> `isSuccess=false`. `isSuccess` is overwritten by each SP call. Failure message `Already Exist!`.

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_LeaveConfig.GetEmployeeEditData | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:293 | int id | ~/DoctorModule_UI/LeaveConfig.aspx/GetEmployeeEditData | via BLL |
| DoctorModule_UI_LeaveConfig.Save_Leaveinfo | Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs:299 | EmployeeLeave employee | ~/DoctorModule_UI/LeaveConfig.aspx/Save_Leaveinfo | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| BonusCampaignNewDAL.GetLeaveConfigSetupById | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:1184 | sp_GET_LeaveConfigSetupById | @id |
| CommonDataLoad.Get_JoiningDateCountInfo | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:503 | sp_Get_JoiningDateCountInfo |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| CommonDataLoad.Get_DayName | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:521 | sp_Webapi_Get_DayName |  |
| CommonDataLoad.Get_LeaveConType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:541 | sp_Webapi_Get_LeaveConType |  |
| LeaveDal.GetEmployeeLeaveForEdit | Library.DAL/DoctorModule_DAL/LeaveDal.cs:158 | sp_Get_EmployeeLeaveIinfo_ById | @id |
| LeaveDal.Save_LeaveInfo | Library.DAL/DoctorModule_DAL/LeaveDal.cs:19 | sp_Save_Employe_LeaveType_Infos, sp_Update_Employee_Leave_info, sp_check_LeaveIn | @EntryBy, @IsActive, @LeaveDays, @LeaveTypeId, @LeaveTypeName, @Name, @UpdateBy, @id |
| BonusCampaignNewDAL.SaveLeaveConfig | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:741 | sp_Save_LeaveConfigDtl, sp_Save_LeaveConfigFroenEmp, sp_Save_LeaveConfigMaster,  | @CountEmployeeHoliday, @CountGovtLeave, @DayNameId, @DaysPerMonthly, @EligbleforProbationEmployee, @ |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_LeaveConfigSetupById | BonusCampaignNewDAL.GetLeaveConfigSetupById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeLeaveIinfo_ById | LeaveDal.GetEmployeeLeaveForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_JoiningDateCountInfo | CommonDataLoad.Get_JoiningDateCountInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Save_Employe_LeaveType_Infos | LeaveDal.Save_LeaveInfo | body NOT in repository | write-like name |
| sp_Save_LeaveConfigDtl | BonusCampaignNewDAL.SaveLeaveConfig | body NOT in repository | write-like name |
| sp_Save_LeaveConfigFroenEmp | BonusCampaignNewDAL.SaveLeaveConfig | body NOT in repository | write-like name |
| sp_Save_LeaveConfigMaster | BonusCampaignNewDAL.SaveLeaveConfig | body NOT in repository | write-like name |
| sp_Up_LeaveConfigMaster | BonusCampaignNewDAL.SaveLeaveConfig | body NOT in repository | write-like name |
| sp_Update_Employee_Leave_info | LeaveDal.Save_LeaveInfo | body NOT in repository | write-like name |
| sp_Webapi_Get_DayName | CommonDataLoad.Get_DayName | body NOT in repository | read-like name (INFERRED) |
| sp_Webapi_Get_LeaveConType | CommonDataLoad.Get_LeaveConType | body NOT in repository | read-like name (INFERRED) |
| sp_check_LeaveInfo | LeaveDal.Save_LeaveInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| all | **none** (`Validation()` returns true) - leave name may be blank, no duplicate check in C# | `LeaveConfig.aspx.cs:571` | - | BR-M15-041 |
| txtCountDays | characters `0-9` and `.` only | `FilteredTextBoxExtender` (markup `:203-204`) | - | BR-M15-042 |
| grid row | saved only if joining-date-count AND days present; incomplete rows silently dropped | `:484-506` | - | BR-M15-042 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-041 | Leave rule attributes saved: name, Count Govt Leave (bool), Count Employee Holiday (bool) + Day (`DayNameId`, only meaningful when holiday counting is Yes), Eligible for Probation Employee (bool), Leave Type id, Active; no field is validated. | `LeaveConfig.aspx.cs:510-536,571` | code-behind | CONFIRMED |
| BR-M15-042 | Entitlement bands: each grid row pairs a "joining date count" option with a numeric `DaysPerMonthly`; rows lacking either are dropped; numeric-only input. Meaning of bands/monthly days in accrual: UNKNOWN. | `:484-506`; `BonusCampaignNewDAL.cs:790-800` | code-behind + SP | CONFIRMED (capture), UNKNOWN (use) |
| BR-M15-043 | UI panels depend on the leave type **text**: `Foreign` -> employee multi-select (rule restricted to selected employees, `sp_Save_LeaveConfigFroenEmp`), `Annual`/`Sick`/`Casual` -> count grid; other types show neither. | `:324-371` | code-behind | CONFIRMED |
| BR-M15-044 | Weekly-holiday day selector is available only when Count Employee Holiday = Yes; the stored `DayNameId` is nullable. | `:307-315,526` | code-behind | CONFIRMED |

## 20. Permission Matrix
Session `UserId` only; no role checks; menu-only. Web-method endpoints `GetEmployeeEditData`/`Save_Leaveinfo` on this page take the session id only for writes.

## 21. Status Lifecycle
`IsActive` bool only (checkbox `chkIsActive`).

## 22. Error Handling
Read failures swallowed (`GetOneRecord` empty `catch`); employee-save loop exceptions swallowed; DAL rollback+rethrow; generic `Already Exist!`.

## 23. Audit
`@EntryBy` = `Session["UserId"]` (both insert and update - the update SP also receives `@EntryBy`); details no audit.

## 24. Integration
sp_Webapi_Get_DayName, sp_Webapi_Get_LeaveConType

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: BonusCampaignNewDAL, CommonDataLoad, LeaveDal; DAO: EmployeeLeave, LeaveConfigCountDtl, LeaveConfigDAO, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-578-01 | Type Annual, add band row (band + 1.5), Save | Master + 1 detail | BR-M15-042 |
| TC-578-02 | Row with days but no band | Dropped silently | BR-M15-042 |
| TC-578-03 | Type Foreign, pick 2 employees | 2 `LeaveConfigFroenEmp` rows | BR-M15-043 |
| TC-578-04 | Count Employee Holiday = Yes | Day selector appears | BR-M15-044 |
| TC-578-05 | Save with blank name (edge) | Accepted (no validation) | BR-M15-041 |
| TC-578-06 | Edit and Save (edge) | Details re-inserted; check duplicates (SP unknown) | GAP-M15-020 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/LeaveConfig.aspx`, `Solution.Web/DoctorModule_UI/LeaveConfig.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | LeaveConfig.aspx(.cs) | n/a | MISSING | - |
| Accrual semantics | None existed | capture only | SPs absent | UNKNOWN | GAP-M15-001, 020 |
| Validation | None existed | none | UNKNOWN | ACTUAL-GAP | BR-M15-041 |

## 30. Known Gaps
- **GAP-M15-020** no validation, text-driven UI, unknown accrual semantics, unclear detail replacement on update.
- Dead legacy WebMethods and `using System.Web.Mvc`.

## 31. Change Impact
Feeds PAGE-611 yearly process and leave balance SPs (`sp_GET_EmployeeLeaveBalance`, report SPs) - changes to config semantics alter entitlements. `BonusCampaignNewDAL` is shared with campaign/bonus pages (module: product pricing).

