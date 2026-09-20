---
page_id: PAGE-557
module: M14
page: CommonDataLoad
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-557 — CommonDataLoad

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-557 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | CommonDataLoad |
| Kind | Page (INFERRED from name) |
| Web folder | Target_UI |
| Master page | ~/MasterPages/MasterPage.master |
| Page class | DoctorModule_UI_CommonDataLoad |
| aspx size | 372 bytes |
| Code-behind LOC | 410 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Not a screen: an empty page (master page + two empty content placeholders) that exists to host **static `[WebMethod]` JSON lookup endpoints** (`Target_UI/CommonDataLoad.aspx/<Method>`) for drop-downs on the Target pages. About 40 lookups (group, zone, area, territory, market, designation, doctor speciality/brand, institute, station type, fiscal year, ...). It is a strict subset/duplicate of `DoctorModule_UI/CommonDataLoad.aspx` (PAGE-677); same class name `DoctorModule_UI_CommonDataLoad`. CONFIRMED: `Solution.Web/Target_UI/CommonDataLoad.aspx.cs:1-409`, `.aspx:1-7`.

## 3. Business Objective
Provide JSON reference data to client-side scripts (jQuery AJAX) without writing an ASMX service. Only lookups - no writes.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/Target_UI/CommonDataLoad.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Target_UI/CommonDataLoad.aspx |
| Code-behind | Solution.Web/Target_UI/CommonDataLoad.aspx.cs |
| Master | Solution.Web/MasterPages/MasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |

## 7. UI Control Inventory
No server/HTML controls with an ID were extracted (viewer-only or code-generated page).

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
No UI. Callers use `$.ajax` POST to `CommonDataLoad.aspx/<Method>` with JSON body (ASP.NET page-method protocol). No caller of `Target_UI/CommonDataLoad.aspx/*` exists anywhere under `Solution.Web` (grep 2026-09-19): the Target_UI pages call their own page methods (`MonthlyTarget.aspx/SaveTarget`, `ProductWiseTarget.aspx/LoadProduct`, ...).

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_CommonDataLoad.Page_Load | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:18 | object sender, EventArgs e | DoctorModule_UI_CommonDataLoad.Page_Load | no | no |
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:25 |  | DoctorModule_UI_CommonDataLoad.GetDesignation_Active → CommonDataLoad.GetDesignation_Active → SP: sp_Get_Designation_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorSpeciality_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:33 |  | DoctorModule_UI_CommonDataLoad.GetDoctorSpeciality_Active → CommonDataLoad.DoctorSpeciality_Active → SP: sp_Get_DoctorSpeciality_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorBrand_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:41 |  | DoctorModule_UI_CommonDataLoad.GetDoctorBrand_Active → CommonDataLoad.GetDoctorBrand_Active → SP: sp_Get_DoctorBrand_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetInstitute_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:49 |  | DoctorModule_UI_CommonDataLoad.GetInstitute_Active → CommonDataLoad.GetInstitute_Active → SP: sp_Get_CustMasterInstitution | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:60 | int id | DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_Active → CommonDataLoad.GetZone_byGroupId_Active → SP: sp_Get_Zone_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetChamber_ByDoctorId | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:70 | int id | DoctorModule_UI_CommonDataLoad.GetChamber_ByDoctorId → CommonDataLoad.GetChamber_ByDoctorId → SP: sp_GetChamber_ByDoctorId | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:77 | int id | DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_All → CommonDataLoad.GetZone_byGroupId_All → SP: sp_Get_Zone_AllByGroup | no | no |
| DoctorModule_UI_CommonDataLoad.GetCompanyInfoAll | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:86 |  | DoctorModule_UI_CommonDataLoad.GetCompanyInfoAll → CommonDataLoad.GetCompanyInfoAll → SP: sp_GET_CompanyInfo | no | no |
| DoctorModule_UI_CommonDataLoad.GetExpenseField_ByExpenseType | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:94 | int id | DoctorModule_UI_CommonDataLoad.GetExpenseField_ByExpenseType → CommonDataLoad.GetExpenseField_ByExpenseType → SP: sp_Get_ExpenseDetails_ByExpenseId | no | no |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:104 | int id | DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_Active → CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:112 | int id | DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_All → CommonDataLoad.GetArea_ByZoneId_All → SP: sp_CS_GetArea_ByZoneId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetContactType | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:120 |  | DoctorModule_UI_CommonDataLoad.GetContactType → CommonDataLoad.GetContactType → SP: sp_Get_ContactType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetSpecialDay_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:128 |  | DoctorModule_UI_CommonDataLoad.GetSpecialDay_Active → CommonDataLoad.GetSpecialDay_Active → SP: sp_Get_DoctorSpecialDay_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetStationType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:135 |  | DoctorModule_UI_CommonDataLoad.GetStationType_Active → CommonDataLoad.GetStationType_Active → SP: sp_Get_StationType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetChamberType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:143 |  | DoctorModule_UI_CommonDataLoad.GetChamberType_Active → CommonDataLoad.GetChamberType_Active → SP: sp_Get_ChamberType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDivision_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:151 |  | DoctorModule_UI_CommonDataLoad.GetDivision_Active → CommonDataLoad.GetDivision_Active → SP: sp_Get_Division_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDistrict_ByDivision_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:159 | int id | DoctorModule_UI_CommonDataLoad.GetDistrict_ByDivision_Active → CommonDataLoad.GetDistrict_ByDivision_Active → SP: sp_Get_DistrictList_OnlyActive_ByDivisionId | no | no |
| DoctorModule_UI_CommonDataLoad.GetThana_ByDistrict_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:167 | int id | DoctorModule_UI_CommonDataLoad.GetThana_ByDistrict_Active → CommonDataLoad.GetThana_ByDistrict_Active → SP: sp_Get_ThanaList_OnlyActive_Bydistrict_id | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:175 |  | DoctorModule_UI_CommonDataLoad.GetDoctorType_Active → CommonDataLoad.GetDoctorType_Active → SP: sp_Get_DoctorType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorCustomer_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:183 |  | DoctorModule_UI_CommonDataLoad.GetDoctorCustomer_Active → CommonDataLoad.DoctorCustomer_Active → SP: sp_Get_DoctorCustomer_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetProgramType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:191 |  | DoctorModule_UI_CommonDataLoad.GetProgramType_Active → CommonDataLoad.DoctorProgramType_Active → SP: sp_Get_DoctorProgramType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDegree_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:199 |  | DoctorModule_UI_CommonDataLoad.GetDegree_Active → CommonDataLoad.GetDegree_Active → SP: sp_Get_Degree_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:207 |  | DoctorModule_UI_CommonDataLoad.GetZone_Active → CommonDataLoad.GetZone_Active → SP: sp_CS_GetZone_Active, sp_CS_GetZone_ByGroupId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:216 |  | DoctorModule_UI_CommonDataLoad.GetGroupInfo_Active → CommonDataLoad.GetGroupInfo_Active → SP: sp_CS_Group_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetRoleTypeInfo | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:229 |  | DoctorModule_UI_CommonDataLoad.GetRoleTypeInfo → CommonDataLoad.GetRoleTypeInfoDAL → SP: sp_CS_RoleType | no | no |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:236 |  | DoctorModule_UI_CommonDataLoad.GetGroupInfo_All → CommonDataLoad.GetGroupInfo_All → SP: sp_CS_Group_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetFiscalYearInfo_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:243 |  | DoctorModule_UI_CommonDataLoad.GetFiscalYearInfo_Active → CommonDataLoad.GetFiscalYearInfo_Active → SP: sp_CS_FiscalYearInfo_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_ActiveById | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:251 | int id | DoctorModule_UI_CommonDataLoad.GetZone_ActiveById → CommonDataLoad.GetZone_Active → SP: sp_CS_GetZone_Active, sp_CS_GetZone_ByGroupId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:261 | int id | DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_Active → CommonDataLoad.GetTerritory_ByAreaId_Active → SP: sp_CS_GetTerritory_ByAreaId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:269 | int id | DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_Active → CommonDataLoad.GetSubTerritory_ByTerritoryId_Active → SP: sp_CS_GetSubTerritory_ByTerritoryId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:277 | int id | DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_All → CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle → SP: sp_CS_GetSubTerritory_ByTerritoryId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:284 | int id | DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_All → CommonDataLoad.GetTerritory_ByAreaId_All → SP: sp_CS_GetTerritory_ByAreaId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:292 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_Active → CommonDataLoad.GetMarket_ByTerritoryId_Active → SP: sp_CS_GetMarket_BySubTerritoryId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:302 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_Active → CommonDataLoad.GetMarket_ByTerritoryId_Active → SP: sp_CS_GetMarket_BySubTerritoryId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:309 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_All → CommonDataLoad.GetMarket_BySubTerritoryId_All → SP: sp_CS_GetMarket_BySubTerritoryId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:318 |  | DoctorModule_UI_CommonDataLoad.GetEmployeeList_All → CommonDataLoad.GetEmployeeList_All → SP: sp_CS_GetEmpGeneralInfo_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:326 |  | DoctorModule_UI_CommonDataLoad.GetEmployeeList_Active → CommonDataLoad.GetEmployeeList_Active → SP: sp_CS_GetEmpGeneralInfo_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetDepartment_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:334 |  | DoctorModule_UI_CommonDataLoad.GetDepartment_Active → CommonDataLoad.GetDepartment_Active → SP: sp_CS_Department_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active_Emp | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:342 |  | DoctorModule_UI_CommonDataLoad.GetDesignation_Active_Emp → CommonDataLoad.GetDesignation_Active_Emp → SP: sp_CS_Designation_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetShift_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:349 |  | DoctorModule_UI_CommonDataLoad.GetShift_Active → CommonDataLoad.GetShift_Active → SP: sp_CS_Shift_Active | no | no |
| DoctorModule_UI_CommonDataLoad.CompanyList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:356 |  | DoctorModule_UI_CommonDataLoad.CompanyList_Active → CommonDataLoad.CompanyList_Active → SP: sp_CS_GetEmpGeneralInfo_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:364 |  | DoctorModule_UI_CommonDataLoad.GetTourTypeList_Active → CommonDataLoad.GetTourTypeList_Active → SP: sp_CS_TourPlanType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:373 |  | DoctorModule_UI_CommonDataLoad.GetTourTypeList_All → CommonDataLoad.GetTourTypeList_ALL → SP: sp_CS_TourPlanType_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetTransportList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:381 |  | DoctorModule_UI_CommonDataLoad.GetTransportList_Active → CommonDataLoad.GetTransportList_Active → SP: sp_CS_Transport_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTransportList_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:389 |  | DoctorModule_UI_CommonDataLoad.GetTransportList_All → CommonDataLoad.GetTransportList_All → SP: sp_CS_Transport_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetUserList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:397 |  | DoctorModule_UI_CommonDataLoad.GetUserList_Active → CommonDataLoad.GetUserList_Active → SP: sp_CS_UserInfo_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetExpenseType | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:404 |  | DoctorModule_UI_CommonDataLoad.GetExpenseType → CommonDataLoad.GetExpenseType → SP: sp_CS_GetExpenseType_Active | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:25 |  | ~/Target_UI/CommonDataLoad.aspx/GetDesignation_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorSpeciality_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:33 |  | ~/Target_UI/CommonDataLoad.aspx/GetDoctorSpeciality_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorBrand_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:41 |  | ~/Target_UI/CommonDataLoad.aspx/GetDoctorBrand_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetInstitute_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:49 |  | ~/Target_UI/CommonDataLoad.aspx/GetInstitute_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:60 | int id | ~/Target_UI/CommonDataLoad.aspx/GetZone_byGroupId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetChamber_ByDoctorId | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:70 | int id | ~/Target_UI/CommonDataLoad.aspx/GetChamber_ByDoctorId | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:77 | int id | ~/Target_UI/CommonDataLoad.aspx/GetZone_byGroupId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetCompanyInfoAll | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:86 |  | ~/Target_UI/CommonDataLoad.aspx/GetCompanyInfoAll | via BLL |
| DoctorModule_UI_CommonDataLoad.GetExpenseField_ByExpenseType | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:94 | int id | ~/Target_UI/CommonDataLoad.aspx/GetExpenseField_ByExpenseType | via BLL |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:104 | int id | ~/Target_UI/CommonDataLoad.aspx/GetArea_ByZoneId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:112 | int id | ~/Target_UI/CommonDataLoad.aspx/GetArea_ByZoneId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetContactType | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:120 |  | ~/Target_UI/CommonDataLoad.aspx/GetContactType | via BLL |
| DoctorModule_UI_CommonDataLoad.GetSpecialDay_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:128 |  | ~/Target_UI/CommonDataLoad.aspx/GetSpecialDay_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetStationType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:135 |  | ~/Target_UI/CommonDataLoad.aspx/GetStationType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetChamberType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:143 |  | ~/Target_UI/CommonDataLoad.aspx/GetChamberType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDivision_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:151 |  | ~/Target_UI/CommonDataLoad.aspx/GetDivision_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDistrict_ByDivision_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:159 | int id | ~/Target_UI/CommonDataLoad.aspx/GetDistrict_ByDivision_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetThana_ByDistrict_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:167 | int id | ~/Target_UI/CommonDataLoad.aspx/GetThana_ByDistrict_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:175 |  | ~/Target_UI/CommonDataLoad.aspx/GetDoctorType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorCustomer_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:183 |  | ~/Target_UI/CommonDataLoad.aspx/GetDoctorCustomer_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetProgramType_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:191 |  | ~/Target_UI/CommonDataLoad.aspx/GetProgramType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDegree_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:199 |  | ~/Target_UI/CommonDataLoad.aspx/GetDegree_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:207 |  | ~/Target_UI/CommonDataLoad.aspx/GetZone_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:216 |  | ~/Target_UI/CommonDataLoad.aspx/GetGroupInfo_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetRoleTypeInfo | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:229 |  | ~/Target_UI/CommonDataLoad.aspx/GetRoleTypeInfo | via BLL |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:236 |  | ~/Target_UI/CommonDataLoad.aspx/GetGroupInfo_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetFiscalYearInfo_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:243 |  | ~/Target_UI/CommonDataLoad.aspx/GetFiscalYearInfo_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_ActiveById | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:251 | int id | ~/Target_UI/CommonDataLoad.aspx/GetZone_ActiveById | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:261 | int id | ~/Target_UI/CommonDataLoad.aspx/GetTerritory_ByAreaId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:269 | int id | ~/Target_UI/CommonDataLoad.aspx/GetSubTerritory_ByTerritoryId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:277 | int id | ~/Target_UI/CommonDataLoad.aspx/GetSubTerritory_ByTerritoryId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:284 | int id | ~/Target_UI/CommonDataLoad.aspx/GetTerritory_ByAreaId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:292 | int id | ~/Target_UI/CommonDataLoad.aspx/GetMarket_ByTerritoryId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:302 | int id | ~/Target_UI/CommonDataLoad.aspx/GetMarket_BySubTerritoryId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:309 | int id | ~/Target_UI/CommonDataLoad.aspx/GetMarket_BySubTerritoryId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:318 |  | ~/Target_UI/CommonDataLoad.aspx/GetEmployeeList_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:326 |  | ~/Target_UI/CommonDataLoad.aspx/GetEmployeeList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDepartment_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:334 |  | ~/Target_UI/CommonDataLoad.aspx/GetDepartment_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active_Emp | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:342 |  | ~/Target_UI/CommonDataLoad.aspx/GetDesignation_Active_Emp | via BLL |
| DoctorModule_UI_CommonDataLoad.GetShift_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:349 |  | ~/Target_UI/CommonDataLoad.aspx/GetShift_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.CompanyList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:356 |  | ~/Target_UI/CommonDataLoad.aspx/CompanyList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:364 |  | ~/Target_UI/CommonDataLoad.aspx/GetTourTypeList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:373 |  | ~/Target_UI/CommonDataLoad.aspx/GetTourTypeList_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTransportList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:381 |  | ~/Target_UI/CommonDataLoad.aspx/GetTransportList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTransportList_All | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:389 |  | ~/Target_UI/CommonDataLoad.aspx/GetTransportList_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetUserList_Active | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:397 |  | ~/Target_UI/CommonDataLoad.aspx/GetUserList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetExpenseType | Solution.Web/Target_UI/CommonDataLoad.aspx.cs:404 |  | ~/Target_UI/CommonDataLoad.aspx/GetExpenseType | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetDesignation_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:733 | sp_Get_Designation_All_Active |  |
| CommonDataLoad.DoctorSpeciality_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:811 | sp_Get_DoctorSpeciality_Active |  |
| CommonDataLoad.GetDoctorBrand_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:874 | sp_Get_DoctorBrand_Active |  |
| CommonDataLoad.GetInstitute_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:895 | sp_Get_CustMasterInstitution |  |
| CommonDataLoad.GetZone_byGroupId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1213 | sp_Get_Zone_All_Active | @GroupId |
| CommonDataLoad.GetChamber_ByDoctorId | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1234 | sp_GetChamber_ByDoctorId | @DoctorId |
| CommonDataLoad.GetZone_byGroupId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1254 | sp_Get_Zone_AllByGroup | @GroupId |
| CommonDataLoad.GetCompanyInfoAll | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1412 | sp_GET_CompanyInfo |  |
| CommonDataLoad.GetExpenseField_ByExpenseType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1501 | sp_Get_ExpenseDetails_ByExpenseId | @id |
| CommonDataLoad.GetArea_ByZoneId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1131 | sp_CS_GetArea_ByZoneId_Active | @id |
| CommonDataLoad.GetArea_ByZoneId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1152 | sp_CS_GetArea_ByZoneId_All | @id |
| CommonDataLoad.GetContactType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:914 | sp_Get_ContactType_Active |  |
| CommonDataLoad.GetSpecialDay_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:933 | sp_Get_DoctorSpecialDay_All_Active |  |
| CommonDataLoad.GetStationType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:951 | sp_Get_StationType_Active |  |
| CommonDataLoad.GetChamberType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:971 | sp_Get_ChamberType_Active |  |
| CommonDataLoad.GetDivision_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:989 | sp_Get_Division_Active |  |
| CommonDataLoad.GetDistrict_ByDivision_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1008 | sp_Get_DistrictList_OnlyActive_ByDivisionId | @id |
| CommonDataLoad.GetThana_ByDistrict_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1031 | sp_Get_ThanaList_OnlyActive_Bydistrict_id | @id |
| CommonDataLoad.GetDoctorType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:830 | sp_Get_DoctorType_Active |  |
| CommonDataLoad.DoctorCustomer_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:792 | sp_Get_DoctorCustomer_Active |  |
| CommonDataLoad.DoctorProgramType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1054 | sp_Get_DoctorProgramType_Active |  |
| CommonDataLoad.GetDegree_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1073 | sp_Get_Degree_All_Active |  |
| CommonDataLoad.GetZone_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:128 | sp_CS_GetZone_Active |  |
| CommonDataLoad.GetGroupInfo_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:187 | sp_CS_Group_Active |  |
| CommonDataLoad.GetRoleTypeInfoDAL | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:339 | sp_CS_RoleType |  |
| CommonDataLoad.GetGroupInfo_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:374 | sp_CS_Group_All |  |
| CommonDataLoad.GetFiscalYearInfo_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:431 | sp_CS_FiscalYearInfo_Active |  |
| CommonDataLoad.GetTerritory_ByAreaId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1540 | sp_CS_GetTerritory_ByAreaId_Active | @id |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1625 | sp_CS_GetSubTerritory_ByTerritoryId_Active | @id |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1782 | sp_CS_GetSubTerritory_ByTerritoryId_All | @id |
| CommonDataLoad.GetTerritory_ByAreaId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1823 | sp_CS_GetTerritory_ByAreaId_All | @id |
| CommonDataLoad.GetMarket_ByTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1994 | sp_CS_GetMarket_BySubTerritoryId_Active | @id |
| CommonDataLoad.GetMarket_BySubTerritoryId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1717 | sp_CS_GetMarket_BySubTerritoryId_All | @id |
| CommonDataLoad.GetEmployeeList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1761 | sp_CS_GetEmpGeneralInfo_All |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| CommonDataLoad.GetDepartment_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:559 | sp_CS_Department_Active |  |
| CommonDataLoad.GetDesignation_Active_Emp | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:579 | sp_CS_Designation_Active |  |
| CommonDataLoad.GetShift_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:598 | sp_CS_Shift_Active |  |
| CommonDataLoad.CompanyList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:616 | sp_CS_GetEmpGeneralInfo_Active |  |
| CommonDataLoad.GetTourTypeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:635 | sp_CS_TourPlanType_Active |  |
| CommonDataLoad.GetTourTypeList_ALL | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:654 | sp_CS_TourPlanType_All |  |
| CommonDataLoad.GetTransportList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:673 | sp_CS_Transport_Active |  |
| CommonDataLoad.GetTransportList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:693 | sp_CS_Transport_All |  |
| CommonDataLoad.GetUserList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:713 | sp_CS_UserInfo_Active |  |
| CommonDataLoad.GetExpenseType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:449 | sp_CS_GetExpenseType_Active |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_Department_Active | CommonDataLoad.GetDepartment_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Designation_Active | CommonDataLoad.GetDesignation_Active_Emp | body NOT in repository | read-like name (INFERRED) |
| sp_CS_FiscalYearInfo_Active | CommonDataLoad.GetFiscalYearInfo_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_All | CommonDataLoad.GetArea_ByZoneId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_Active | CommonDataLoad.CompanyList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active, CommonDataLoad.GetEmployeeList_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetExpenseType_Active | CommonDataLoad.GetExpenseType | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_BySubTerritoryId_Active | CommonDataLoad.GetMarket_ByTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_BySubTerritoryId_All | CommonDataLoad.GetMarket_BySubTerritoryId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetSubTerritory_ByTerritoryId_Active | CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetSubTerritory_ByTerritoryId_All | CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_Active | CommonDataLoad.GetTerritory_ByAreaId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_All | CommonDataLoad.GetTerritory_ByAreaId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetZone_Active | CommonDataLoad.GetZone_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetZone_ByGroupId_Active | CommonDataLoad.GetZone_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Group_Active | CommonDataLoad.GetGroupInfo_Active | body NOT in repository | write-like name |
| sp_CS_Group_All | CommonDataLoad.GetGroupInfo_All | body NOT in repository | write-like name |
| sp_CS_RoleType | CommonDataLoad.GetRoleTypeInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Shift_Active | CommonDataLoad.GetShift_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_TourPlanType_Active | CommonDataLoad.GetTourTypeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_TourPlanType_All | CommonDataLoad.GetTourTypeList_ALL | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Transport_Active | CommonDataLoad.GetTransportList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Transport_All | CommonDataLoad.GetTransportList_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_UserInfo_Active | CommonDataLoad.GetUserList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CompanyInfo | CommonDataLoad.GetCompanyInfoAll | body NOT in repository | read-like name (INFERRED) |
| sp_GetChamber_ByDoctorId | CommonDataLoad.GetChamber_ByDoctorId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ChamberType_Active | CommonDataLoad.GetChamberType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ContactType_Active | CommonDataLoad.GetContactType | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CustMasterInstitution | CommonDataLoad.GetInstitute_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Degree_All_Active | CommonDataLoad.GetDegree_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Designation_All_Active | CommonDataLoad.GetDesignation_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DistrictList_OnlyActive_ByDivisionId | CommonDataLoad.GetDistrict_ByDivision_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Division_Active | CommonDataLoad.GetDivision_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorBrand_Active | CommonDataLoad.GetDoctorBrand_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorCustomer_Active | CommonDataLoad.DoctorCustomer_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorProgramType_Active | CommonDataLoad.DoctorProgramType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorSpecialDay_All_Active | CommonDataLoad.GetSpecialDay_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorSpeciality_Active | CommonDataLoad.DoctorSpeciality_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorType_Active | CommonDataLoad.GetDoctorType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpenseDetails_ByExpenseId | CommonDataLoad.GetExpenseField_ByExpenseType | body NOT in repository | read-like name (INFERRED) |
| sp_Get_StationType_Active | CommonDataLoad.GetStationType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ThanaList_OnlyActive_Bydistrict_id | CommonDataLoad.GetThana_ByDistrict_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_AllByGroup | CommonDataLoad.GetZone_byGroupId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active | CommonDataLoad.GetZone_byGroupId_Active | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-024 | **Lookup endpoints are unauthenticated at code level.** Each `[WebMethod] public static string X(...)` runs `CommonDataLoad.<Dal>()` and returns `JsonConvert.SerializeObject(DataTable)`. No Session or role test in any method. Because page methods bypass the master page's `Session["UserId"]` check (that check lives in `NewMasterPage.master.cs`, and this page uses the older `MasterPage.master`), an unauthenticated HTTP client can POST to them (INFERRED - `Web.config` has Forms auth but no `<authorization>` deny rules, lines 80-82). | `Target_UI/CommonDataLoad.aspx.cs:24-409`; `Web.config:80-82` | code-behind | CONFIRMED (no checks); INFERRED (anonymous reachability) |

## 20. Permission Matrix
No Session key read anywhere in the file (CONFIRMED). No per-method authorization. Menu does not apply (page has no UI).

## 21. Status Lifecycle
Not applicable (read-only lookups).

## 22. Error Handling
No error handling: DAL exceptions propagate to ASP.NET, returned to the client as a JSON error (`ExceptionType/Message/StackTrace` unless `customErrors` hides them; `Web.config` custom errors setting not reviewed).

## 23. Audit
None.

## 24. Integration
Internal AJAX JSON endpoints (see specs/integrations/INT-12-page-method-json-endpoints.md). Not an external integration.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad; DAO: —; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-557-01 | Positive | POST `{}` to `.../GetGroupInfo_Active` while logged in | JSON string array of active groups | BR-M14-024 |
| TC-557-02 | Positive | POST `{"id":1}` to `GetZone_byGroupId_Active` | JSON of zones for group 1 | BR-M14-024 |
| TC-557-03 | Security | Same POST with no session cookie | expected to be rejected; code suggests it succeeds (verify) | BR-M14-024 |
| TC-557-04 | Negative | `id` non-integer | model-binding error (HTTP 500) | - |
| TC-557-05 | Consistency | Compare result vs PAGE-677 equivalents | identical (shared `CommonDataLoad` DAL) | - |

## 28. Reverse Engineering References
- `Solution.Web/Target_UI/CommonDataLoad.aspx`, `Solution.Web/Target_UI/CommonDataLoad.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | subset copy of PAGE-677 | n/a | MISSING | - |
| Duplicate | None existed | same class name, overlapping methods | n/a | DUPLICATE | PAGE-677 |

## 30. Known Gaps
- Duplicate of `DoctorModule_UI/CommonDataLoad.aspx` (missing ~10 methods: e.g. `GetNSMEmployee_All`, `GetNational_Active`, `GetCustomerType_All`).
- Unauthenticated JSON lookups (BR-M14-024).
- Uses legacy `MasterPage.master` although the page has no visible content.
- No repo caller of `Target_UI/CommonDataLoad.aspx/*` (grep of `Solution.Web` *.aspx/*.js/*.master/*.cs, 2026-09-19) - dead code candidate; client scripts call `DoctorModule_UI/CommonDataLoad.aspx/...` instead.

## 31. Change Impact
Shares `SalesSolution.Web.DataLayer.CommonDataLoad` DAL with dozens of pages; endpoints are contract for any script that calls `Target_UI/CommonDataLoad.aspx/*`.

