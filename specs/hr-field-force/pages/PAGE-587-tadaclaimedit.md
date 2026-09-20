---
page_id: PAGE-587
module: M15
page: TADAClaimEdit
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-587 — TADAClaimEdit

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-587 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | TADAClaimEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_TADAClaimEdit |
| aspx size | 25478 bytes |
| Code-behind LOC | 778 |
| Inline script size | 9953 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED (fields `TadaDate`, `DAAmount`, `HotelName`, `HotelPhone`, `TourType`, `TourPurpose`, market cascade; list page title `DA Information List`): create or correct one **daily-allowance (DA) claim per employee per tour date**. The form is designed to be *auto-filled from the employee's approved tour plan for that date*: choosing an employee and a date loads the planned market (group -> region -> area -> territory -> sub-territory -> market), tour type, tour purpose and the DA amount from the plan (`Get_TourPlanByTourPlanDate`), which the user can adjust before saving. `TADAClaimView.aspx` (PAGE-588) is the list; edit opens this page with `?MID=<TadaID>` in a new tab.

## 3. Business Objective
CONFIRMED by code: pay a field employee the day's allowance for a planned tour day and record where the employee actually was (market, hotel and hotel phone for overnight tours), so the claim can be approved (DA Claim approval, Approval_UI, MenuId 376 - see workflow WF-M15-05) and rolled into the monthly expense report (`sp_RPT_MonthlyExpense`, PAGE-623/624/628).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/TADAClaimView.aspx`, `Reports_UI/TargetAChivementReport.aspx`

Outbound references: `tadaclaimview.aspx`

## 5. Route
`~/DoctorModule_UI/TADAClaimEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAL class | SetupDAL (Library.DAL/DoctorModule_DAL/SetupDAL.cs) |
| DAO/entity class | MonthlyAllowance (Library.DAO/DoctorModule_DAO/MonthlyAllowance.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | TADADAO (Library.DAO/DoctorModule_DAO/MonthlyAllowance.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| EmployeeIdSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→EmployeeIdSelect_SelectedIndexChanged |
| TadaDate | TextBox | Select Date | no validator found |  |  |  | OnTextChanged→TadaDate_TextChanged |
| txtDAAmount | TextBox | Amount | no validator found |  |  |  |  |
| ddlTourType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlTourPurpose | DropDownList |  | no validator found |  |  | see §9 |  |
| txtHotelName | TextBox | Hotel Name | no validator found |  |  |  |  |
| txtHotelPhone | TextBox | Hotel Phone | no validator found |  |  |  |  |
| txtRemarks | TextBox | Remarks | no validator found |  |  |  |  |
| GroupSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→GroupSelect_SelectedIndexChanged |
| ZoneSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ZoneSelect_SelectedIndexChanged |
| AreaSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→AreaSelect_SelectedIndexChanged |
| TeritorySelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→TeritorySelect_SelectedIndexChanged |
| SubTeritory | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→SubTeritory_SelectedIndexChanged |
| MarketSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |
| restbtn | LinkButton |  | no validator found |  |  |  | OnClick→restbtn_Click |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| EmployeeIdSelect | dtTTPs | DoctorModule_UI_TADAClaimEdit.GetOneRecord | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | dt | DoctorModule_UI_TADAClaimEdit.GetOneRecord | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | DoctorModule_UI_TADAClaimEdit.GetOneRecord | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | DoctorModule_UI_TADAClaimEdit.GetOneRecord | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| TeritorySelect | dt | DoctorModule_UI_TADAClaimEdit.GetOneRecord | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| SubTeritory | dt | DoctorModule_UI_TADAClaimEdit.GetOneRecord | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlTourPurpose | selectedText | DoctorModule_UI_TADAClaimEdit.Validation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| id_mastetID | TADADAO.TadaID | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @TadaID | UNKNOWN (SP body not in repo) | UNKNOWN |
| EmployeeIdSelect | TADADAO.EmpInfoId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @EmpInfoId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TadaDate | TADADAO.TadaDate | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @TadaDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtRemarks | TADADAO.Remarks | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @Remarks | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtHotelName | TADADAO.HotelName | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @HotelName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtHotelPhone | TADADAO.HotelPhone | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @HotelPhone | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtDAAmount | TADADAO.DAAmount | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @DAAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlTourType | TADADAO.TourTypeId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @TourTypeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| GroupSelect | TADADAO.GroupId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ZoneSelect | TADADAO.RegionId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| AreaSelect | TADADAO.AreaId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TeritorySelect | TADADAO.TerritoryId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| SubTeritory | TADADAO.SubTerritoryId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @SubTerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| MarketSelect | TADADAO.MarketId | DoctorModule_UI_TADAClaimEdit.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.SaveTADADA | @MarketId | UNKNOWN (SP body not in repo) | UNKNOWN |
| EmployeeIdSelect | dt33 | DoctorModule_UI_TADAClaimEdit.LoadInfo | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | dt | DoctorModule_UI_TADAClaimEdit.LoadInfo | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | DoctorModule_UI_TADAClaimEdit.LoadInfo | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | DoctorModule_UI_TADAClaimEdit.LoadInfo | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| TeritorySelect | dt | DoctorModule_UI_TADAClaimEdit.LoadInfo | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| SubTeritory | dt | DoctorModule_UI_TADAClaimEdit.LoadInfo | — | CommonDataLoad.GetArea_ByZoneId_All, CommonDataLoad.GetGroupInfo_All, CommonDataLoad.GetMarket_BySubTerritoryId_All, Com | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | dt | DoctorModule_UI_TADAClaimEdit.GroupSelect_SelectedIndexChanged | — | CommonDataLoad.GetZone_byGroupId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | DoctorModule_UI_TADAClaimEdit.ZoneSelect_SelectedIndexChanged | — | CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | DoctorModule_UI_TADAClaimEdit.AreaSelect_SelectedIndexChanged | — | CommonDataLoad.GetTerritory_ByAreaId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| TeritorySelect | dt | DoctorModule_UI_TADAClaimEdit.TeritorySelect_SelectedIndexChanged | — | CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| SubTeritory | dt | DoctorModule_UI_TADAClaimEdit.SubTeritory_SelectedIndexChanged | — | CommonDataLoad.GetMarket_BySubTerritoryId_Active | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlTourType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlTourPurpose | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| GroupSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ZoneSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| AreaSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| TeritorySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| SubTeritory | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| MarketSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_TADAClaimEdit.btnSave_Click | Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx.cs:394 | — | Setup2DAL.SaveTADADA | sp_Save_TadaClaimMaster, sp_Update_TadaClaimMaster | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_TADAClaimEdit.btnSave_Click | Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx.cs:394 | — | Setup2DAL.SaveTADADA | sp_Save_TadaClaimMaster, sp_Update_TadaClaimMaster | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| restbtn | LinkButton click | (postback) | DoctorModule_UI_TADAClaimEdit.restbtn_Click | Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx.cs:451 | — | — | — | read-only / none detected |

## 11. Frontend Flow
1. `Page_Load` (`TADAClaimEdit.aspx.cs:22-54`, first GET): `TadaDate` = today (`dd MMMM, yyyy`), `LoadInitialInfo()` (tour types = station types `Get_StationTypeInfo`; tour purposes `Get_TourPurposeInfo`; active employees `GetEmployeeList_Active`), then `?MID` -> `btnUpdate` + `GetOneRecord`, else `btnSave`. All errors in this method are swallowed (`catch { }`).
2. `GetOneRecord` (`:55-239`): reads `sp_Get_TadaClaimMaster_ById` and rebuilds each cascade level from the saved ids (all levels loaded with the *All* variants, i.e. including inactive nodes); on any failure it clears every field silently (`:218-238`).
3. `EmployeeIdSelect_SelectedIndexChanged` (`:747`) and `TadaDate_TextChanged` (`:455`) both call `LoadInfo()` (`:485-617`): `sp_Get_TourPlanByTourPlanDate(@empId,@tadaDate)`; row 0 supplies GroupId..MarketId (cascades rebuilt), `DAAmount`, `TourTypeId`, `TPId` (purpose). If no plan exists (`Rows[0]` throws) -> alert **` Tour plan not Exists!`** and all fields and cascade lists are cleared.
4. Manual cascade postbacks: Group -> Zone -> Area -> Territory -> SubTerritory -> Market (`:619-745`, active-only lists).
5. Save -> `btnSave_Click`; success -> `successalert(...,'TADAClaimView.aspx')`, failure `faildalert('Operation Faild!')`. `restbtn_Click` redirects to a blank page.

## 12. Backend Flow
`btnSave_Click` (`:393-447`): `Validation()` -> `TADADAO` (`TadaID`, `EmpInfoId`, `TadaDate` (`DateTime.Parse(...).Date`), `Remarks`, `HotelName`, `HotelPhone`, `DAAmount = Convert.ToDecimal(txtDAAmount.Text)`, `TourTypeId`, `GroupId..MarketId`, each nullable when index 0) -> `Setup2DAL.SaveTADADA(aMaster, Session["UserId"])` (`Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4367`): `TadaID>0` -> `UpdateData("sp_Update_TadaClaimMaster")`, else `SaveDataReturnPrimaryKey("sp_Save_TadaClaimMaster")`; **the DAL sends `@UpdateBy` in both cases and never `@EntryBy`** (`:4398`). `isSuccess` true when an update executes or pk>0. **The tour purpose (`ddlTourPurpose`) is validated but not part of `TADADAO`/SP parameters - it is not saved.** No transaction beyond the single SP call; DAL rolls back and rethrows.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.GetTadaForEdit | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4212 | sp_Get_TadaClaimMaster_ById | @id |
| Setup2DAL.Get_TourPlanByTourPlanDate | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4236 | sp_Get_TourPlanByTourPlanDate | @empId, @tadaDate |
| CommonDataLoad.GetGroupInfo_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:374 | sp_CS_Group_All |  |
| CommonDataLoad.GetZone_byGroupId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1254 | sp_Get_Zone_AllByGroup | @GroupId |
| CommonDataLoad.GetArea_ByZoneId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1152 | sp_CS_GetArea_ByZoneId_All | @id |
| CommonDataLoad.GetTerritory_ByAreaId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1823 | sp_CS_GetTerritory_ByAreaId_All | @id |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1782 | sp_CS_GetSubTerritory_ByTerritoryId_All | @id |
| CommonDataLoad.GetMarket_BySubTerritoryId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1717 | sp_CS_GetMarket_BySubTerritoryId_All | @id |
| SetupDAL.Get_StationTypeInfo | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5262 | sp_GET_StationTypeListAll |  |
| SetupDAL.Get_TourPurposeInfo | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5281 | sp_Get_TourPurposeDDL |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| Setup2DAL.SaveTADADA | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4368 | sp_Save_TadaClaimMaster, sp_Update_TadaClaimMaster | @AreaId, @DAAmount, @EmpInfoId, @GroupId, @HotelName, @HotelPhone, @MarketId, @RegionId, @Remarks, @ |
| CommonDataLoad.GetZone_byGroupId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1213 | sp_Get_Zone_All_Active | @GroupId |
| CommonDataLoad.GetArea_ByZoneId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1131 | sp_CS_GetArea_ByZoneId_Active | @id |
| CommonDataLoad.GetTerritory_ByAreaId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1540 | sp_CS_GetTerritory_ByAreaId_Active | @id |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1625 | sp_CS_GetSubTerritory_ByTerritoryId_Active | @id |
| CommonDataLoad.GetMarket_BySubTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1668 | sp_CS_GetMarket_BySubTerritoryId_Active | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_All | CommonDataLoad.GetArea_ByZoneId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_BySubTerritoryId_Active | CommonDataLoad.GetMarket_BySubTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_BySubTerritoryId_All | CommonDataLoad.GetMarket_BySubTerritoryId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetSubTerritory_ByTerritoryId_Active | CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetSubTerritory_ByTerritoryId_All | CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_Active | CommonDataLoad.GetTerritory_ByAreaId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_All | CommonDataLoad.GetTerritory_ByAreaId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Group_All | CommonDataLoad.GetGroupInfo_All | body NOT in repository | write-like name |
| sp_GET_StationTypeListAll | SetupDAL.Get_StationTypeInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TadaClaimMaster_ById | Setup2DAL.GetTadaForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPlanByTourPlanDate | Setup2DAL.Get_TourPlanByTourPlanDate | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPurposeDDL | SetupDAL.Get_TourPurposeInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_AllByGroup | CommonDataLoad.GetZone_byGroupId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active | CommonDataLoad.GetZone_byGroupId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Save_TadaClaimMaster | Setup2DAL.SaveTADADA | body NOT in repository | write-like name |
| sp_Update_TadaClaimMaster | Setup2DAL.SaveTADADA | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| EmployeeIdSelect | required | `Validation()` `:323` | tooltip `please fill out this field` | BR-M15-034 |
| TadaDate | required | `:330` | tooltip | BR-M15-034 |
| txtDAAmount | required (non-empty) | `:339` | tooltip; non-numeric text -> `FormatException` in `Convert.ToDecimal` (unhandled) | BR-M15-034, BR-M15-038 |
| ddlTourType | required | `:348` | tooltip | BR-M15-034 |
| ddlTourPurpose | required | `:357` | tooltip | BR-M15-034 |
| MarketSelect | required **unless** selected purpose text contains `(Other Visit)` | `:365-385` | tooltip | BR-M15-035 |
| Tour plan for employee+date | must exist to auto-fill | `LoadInfo` | ` Tour plan not Exists!` | BR-M15-036 |
| Amount range / duplicate claim for same employee+date / date in future / approved claim lock | **none in code** (SP may) | - | - | GAP-M15-011 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-034 | A DA claim needs employee, date, DA amount, tour type and tour purpose; remarks, hotel name/phone and all market levels are optional (subject to BR-M15-035). | `TADAClaimEdit.aspx.cs:323-363` | code-behind | CONFIRMED |
| BR-M15-035 | Market is mandatory unless the chosen tour purpose text contains the literal `(Other Visit)` (string match on the display text, not on an id/flag). | `:365-385` | code-behind | CONFIRMED |
| BR-M15-036 | The claim is pre-populated from the employee's tour plan for the date (market hierarchy, DA amount, tour type, purpose); if no plan row exists the form is reset and the message ` Tour plan not Exists!` shown. How the plan's `DAAmount` is derived (TADA market rules PAGE-589 by role/market/tour type) is inside the SP - UNKNOWN. | `:485-617,455-483,747-775` | code-behind + SP `sp_Get_TourPlanByTourPlanDate` | CONFIRMED (flow), UNKNOWN (amount derivation) |
| BR-M15-037 | Save routing: `TadaID==0` -> `sp_Save_TadaClaimMaster`; `>0` -> `sp_Update_TadaClaimMaster`; only `@UpdateBy=Session UserId` is stamped (no explicit entry-by). | `Setup2DAL.cs:4367-4425` | DAL | CONFIRMED |
| BR-M15-038 | The DA amount is a free user-editable decimal; there is no cap, sign, or comparison with the tour plan/rule amount in code. | `TADAClaimEdit.aspx.cs:409,607` | code-behind | CONFIRMED |
| BR-M15-039 | Visibility of DA claims in the list is role-scoped: AM sees own area, DZSM own region, NSM own group (via `GetEmpMarketStructure_Active(Session EmpInfoId)` -> `EmpAreaId/EmpRegionId/EmpGroupId`), all other role types see all rows; date, status, role, employee filters are appended as raw SQL text. | `TADAClaimView.aspx.cs` (`param()`) | PAGE-588 | CONFIRMED |
| BR-M15-040 | The tour purpose selected on this page is not persisted (not in DTO/SP call). | `TADAClaimEdit.aspx.cs:400-428`; `Setup2DAL.cs:4367-4405` | code-behind + DAL | CONFIRMED |

## 20. Permission Matrix
Session: `UserId` (audit). No `UserRoleID`/permission check; employee dropdown lists **all active employees**, so any user reaching the page can create/edit a DA claim for any employee. Row scoping exists only on the list page (BR-M15-039). Approval routing for DA claims is on `Approval_UI/DAApprovalList.aspx` (statuses Accepted/Verified/Rejected, Type `DA Claim`, MenuId 376; `Accepted` when `RoleTypeId==5` or `EmpInfoId=="496"`, else `Verified`; `Approval_UI/DAApprovalList.aspx.cs:140-182`).

## 21. Status Lifecycle
`tblTadaClaimMaster.ApprovalStatus` (name INFERRED from list filter `mas.ApprovalStatus`, `TADAClaimView.aspx.cs`); values come from `sp_GET_ActionStatusList` (`SoftwareUseId`/`WebShow` - values UNKNOWN). Approval log statuses written by the approval page: `Verified` (intermediate approver), `Accepted` (role type 5 = final), `Rejected`. **This edit page does not check or change status: an approved claim can still be edited/saved** (no lock).

## 22. Error Handling
Read/cascade blocks swallow exceptions; `LoadInfo` failures show ` Tour plan not Exists!` for *any* exception (including SP errors); DAL exceptions rethrown (unhandled in `btnSave_Click`); failure text `Operation Faild!`.

## 23. Audit
`@UpdateBy = Session["UserId"]` only; SP-side audit columns UNKNOWN. Approval audit is in the approval log (`sp_webapi_SaveAppLog`).

## 24. Integration
None external. Feeds `sp_RPT_MonthlyExpense`/monthly expense Crystal report (`crpEmpExpense.rpt`, DA column `daAmount`).

## 25. Reports
DA list export to CSV: PAGE-588 `btnExportToExcel_Click` (file `DA Information List_<timestamp>.csv`). Monthly expense: RPT-M15-02.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, Setup2DAL, SetupDAL; DAO: MonthlyAllowance, ResultInfo, TADADAO; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-587-01 | Pick employee with a plan for the date | Cascade, DA amount, tour type, purpose auto-filled | BR-M15-036 |
| TC-587-02 | Pick employee/date without plan | Alert ` Tour plan not Exists!`, fields cleared | BR-M15-036 |
| TC-587-03 | Save with purpose containing `(Other Visit)` and no market | Saved | BR-M15-035 |
| TC-587-04 | Save with normal purpose and no market | Market flagged, no save | BR-M15-035 |
| TC-587-05 | Empty DA amount | Flagged | BR-M15-034 |
| TC-587-06 | DA amount `abc` (edge) | Unhandled FormatException | BR-M15-038 |
| TC-587-07 | Edit an already approved claim (edge) | Save succeeds (no lock) | GAP-M15-011 |
| TC-587-08 | Save then reopen `?MID` | Purpose reloaded from the tour plan, not from the claim | BR-M15-040 |
| TC-587-09 | AM opens PAGE-588 | Only own-area rows | BR-M15-039 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx`, `Solution.Web/DoctorModule_UI/TADAClaimEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | TADAClaimEdit.aspx(.cs) | n/a | MISSING | - |
| Approval lock | None existed | none | UNKNOWN | ACTUAL-GAP | GAP-M15-011 |
| Purpose persistence | None existed | not saved | n/a | ACTUAL-DEFECT | BR-M15-040 |
| Amount validation | None existed | none | UNKNOWN | ACTUAL-GAP | BR-M15-038 |

## 30. Known Gaps
- **GAP-M15-011** No duplicate/approval-state/date validation; approved claims editable; DA amount unbounded.
- Purpose not persisted; `@EntryBy` never sent; "Tour plan not Exists!" masks real errors; `Convert.ToDecimal` without try; `TADAClaimView` builds SQL by string concatenation from `FromDate/ToDate` text boxes (SQL injection surface, `TADAClaimView.aspx.cs` `param()`); `EmpInfoId != "" || EmpInfoId != null` is always true.
- `using DocumentFormat.OpenXml.Wordprocessing;` unused import (compilation coupling).

## 31. Change Impact
`Setup2DAL` is shared by ~40 Doctor-module pages (Expense, Mileage, MonthlyAllowance, TADA rules, Training). `sp_Get_TourPlanByTourPlanDate` also feeds the tour-plan report family (PAGE-626/627/629). DA approvals (Approval_UI) and `sp_RPT_MonthlyExpense` read the same claim table.

